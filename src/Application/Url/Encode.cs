using Application.Helper;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.Url
{
	/// <summary>
	/// Encode logic
	/// </summary>
	public class Encode
	{
		static readonly object _object = new object();

		public class Command : IRequest<Result<string>>
		{
			public string Url { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.Url)
					.Must(uri => Uri.TryCreate(HttpUtility.UrlDecode(uri), UriKind.Absolute, out _))
					.When(x => !string.IsNullOrEmpty(x.Url))
					.WithMessage("Provided Url is not a valid Url");
			}
		}

		public class Handler : IRequestHandler<Command, Result<string>>
		{
			private readonly IEncodeService _encodeService;
			private readonly IUrlStorageService _urlStorage;
			private readonly ILogger<Handler> _logger;
			private readonly string _baseUrl;
			private readonly string _shortUrl;

			public Handler(IEncodeService encodeService, IUrlStorageService urlStorage, 
				IConfiguration config, ILogger<Handler> logger)
			{
				_encodeService = encodeService;
				_urlStorage = urlStorage;
				_logger = logger;
				_baseUrl = config.GetValue<string>("Domains:MainDomain");
				_shortUrl = config.GetValue<string>("Domains:ShortUrlDomain");
			}

			public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
			{
				try
				{
					var url = new Uri(HttpUtility.UrlDecode(request.Url));

					_logger.LogInformation("Check if he url has a valid host");

					if(url.Host.Equals(_baseUrl))
					{
						_logger.LogInformation("Locking the thread while adding the record for the memory cache");
						lock (_object)
						{
							_logger.LogInformation("Encode the url");
							var encodedUrl = _encodeService.Encode(_urlStorage.NextId());
							_urlStorage.Add(url.ToString());

							return Result<string>.Success(new Uri($"https://{_shortUrl}/{encodedUrl}").ToString());
						}
					}
					return Result<string>.Failure("Incorrect domain");
				}
				catch (Exception ex)
				{
					_logger.LogError($"Error ouccured: {ex.Message}");
					return Result<string>.Failure("Request Failed");
				}
			}
		}
	}
}
