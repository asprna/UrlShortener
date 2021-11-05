using Application.Url;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UrlShortener.Controllers
{
	/// <summary>
	/// This Controller support url encode endpoints.
	/// </summary>
	[Route("api/encode")]
    public class UrlEncodeController : BaseApiController
	{
        /// <summary>
        /// Encodes a URL to a shortened URL.
        /// </summary>
        /// <param name="url">URL that requires to encode</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Encode.Command url)
        {
            return HandleResult(await Mediator.Send(url));
        }
    }
}
