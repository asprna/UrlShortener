using Application.Helper;
using Application.Interfaces;
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
	/// Decode logic
	/// </summary>
	public class Decode
	{
		public class Query : IRequest<Result<string>>
		{
			public string Url { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<string>>
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

			public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
			{
				try
				{
					Uri url;

					_logger.LogInformation("Check if the given url is valid");

					if(Uri.TryCreate(HttpUtility.UrlDecode(request.Url), UriKind.Absolute, out url))
					{
						_logger.LogInformation("Check if the given url has aa valid host");

						if (url.Host.Equals(_shortUrl))
						{
							_logger.LogInformation("Check the unique id of the encoded url");
							var id = _encodeService.Decode(url.LocalPath.Replace("/", ""));

							_logger.LogInformation("Find the original url by the Id");
							var originalUrl = await _urlStorage.Get(id);

							if (!string.IsNullOrEmpty(originalUrl))
							{
								return Result<string>.Success(originalUrl);
							}
							return Result<string>.Success("Decode failed");
						}
						return Result<string>.Success("Incorrect domain");
					}
					return Result<string>.Success("Provided Url is not a valid Url");
				}
				catch (Exception ex)
				{
					_logger.LogError($"Error ouccured: {ex.Message}");
					return Result<string>.Success("Request Failed");
				}
			}
		}
	}
}
