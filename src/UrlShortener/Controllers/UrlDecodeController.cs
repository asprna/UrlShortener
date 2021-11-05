using Application.Url;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UrlShortener.Controllers
{
	/// <summary>
	/// This Controller support url decode endpoints.
	/// </summary>
	[Route("api/decode")]
    public class UrlDecodeController : BaseApiController
	{
        /// <summary>
        /// Decodes a shortened URL to its original URL.
        /// </summary>
        /// <param name="url">URL that requires to decode</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string url)
        {
            return HandleResult(await Mediator.Send(new Decode.Query { Url = url }));
        }
    }
}
