using Application.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Controllers
{
	/// <summary>
	/// Base API Class.
	/// All the controller should derive from this base class.
	/// </summary>
	[ApiController]
	public class BaseApiController : ControllerBase
	{
		private IMediator _mediator;
		protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

		/// <summary>
		/// Handle controller result.
		/// </summary>
		/// <typeparam name="T">Return type.</typeparam>
		/// <param name="result">Controller result.</param>
		/// <returns></returns>
		protected ActionResult HandleResult<T>(Result<T> result)
		{
			if (result == null) return NotFound();
			if (result.IsSuccess && result.Value != null)
				return Ok(result.Value);
			if (result.IsSuccess && result.Value == null)
				return NotFound();
			return BadRequest(result.Error);
		}
	}
}
