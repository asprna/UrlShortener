using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using UrlShortener;

namespace IntegrationTest.Helper
{
	public class IntegrationTest
	{
		protected readonly HttpClient TestClient;

		public IntegrationTest()
		{
			var appFactory = new WebApplicationFactory<Startup>();
			TestClient = appFactory.CreateClient();
		}
	}
}
