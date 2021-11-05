namespace IntegrationTest.Helper
{
	public static class ApiRoutes
	{
		/// <summary>
		/// Base URL.
		/// </summary>
		private static readonly string _baseUrl = "http://localhost:5000/api/";

		/// <summary>
		/// Base URL for encode.
		/// </summary>
		public static readonly string EncodeControllerUrl = string.Concat(_baseUrl, "encode");

		/// <summary>
		/// Base URL for decode.
		/// </summary>
		public static readonly string DecodeControllerUrl = string.Concat(_baseUrl, "decode?url={0}");
	}
}
