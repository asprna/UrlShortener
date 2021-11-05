using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace IntegrationTest.UrlShortener
{
	public class EncodeDecodeTest : Helper.IntegrationTest
	{
		/// <summary>
		/// We should be able to encode a url, then decode the url.
		/// After decoding, it should be same as the original url.
		// </summary>
		/// <param name="url">Url to test</param>
		[Theory]
		[InlineData("https://codesubmit.io/library/react")]
		[InlineData("https://codesubmit.io/image/image_profile_100")]
		[InlineData("https://codesubmit.io/library")]
		[InlineData("https://codesubmit.io/category/product/bread")]
		[InlineData("https://codesubmit.io/about/67")]
		[InlineData("https://codesubmit.io/list?v=dkdemd&g=hurmnos")]
		public async Task Post_EncodeDecode_Successful(string url)
		{
			//Arrange
			dynamic body = new ExpandoObject();
			body.url = HttpUtility.UrlEncode(url);
			JsonContent content = JsonContent.Create(body);

			string result = null;

			//Act
			//Will encode the url first
			var response = await TestClient.PostAsync(Helper.ApiRoutes.EncodeControllerUrl, content);
			var bodyEncode = await response.Content.ReadAsStringAsync();

			//If we get a valid encoded url, try to decode the url
			if (!string.IsNullOrEmpty(bodyEncode))
			{
				var encodedUrl = HttpUtility.UrlEncode(bodyEncode);
				var responseDecode = await TestClient.GetAsync(string.Format(Helper.ApiRoutes.DecodeControllerUrl, encodedUrl));
				result = await responseDecode.Content.ReadAsStringAsync();
			}

			//Assert
			Assert.Equal(url, result, true);
		}

		/// <summary>
		/// Encode should return an error when the domain is wrong.
		/// <returns></returns>
		[Fact]
		public async Task Post_EncodeInvalidDomain_Unsuccessful()
		{
			//Arrange
			dynamic body = new ExpandoObject();
			body.url = HttpUtility.UrlEncode("https://codesubmittest.io/library/react");
			JsonContent content = JsonContent.Create(body);

			//Act
			var response = await TestClient.PostAsync(Helper.ApiRoutes.EncodeControllerUrl, content);
			var result = await response.Content.ReadAsStringAsync();

			//Assert
			Assert.Equal("Incorrect domain", result, true);
		}

		/// <summary>
		/// Encode should return an error when the url is invalid.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_EncodeInvalidUrl_Unsuccessful()
		{
			//Arrange
			dynamic body = new ExpandoObject();
			body.url = HttpUtility.UrlEncode("codesubmittest/library/react");
			JsonContent content = JsonContent.Create(body);

			//Act
			var response = await TestClient.PostAsync(Helper.ApiRoutes.EncodeControllerUrl, content);
			//var result = await response.Content.ReadAsStringAsync();
			var bodyDecode = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

			//Assert
			Assert.Equal("Provided Url is not a valid Url", bodyDecode.errors.Url[0].ToString(), true);
		}
	}
}
