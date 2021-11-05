namespace Application.Interfaces
{
	/// <summary>
	/// Url encode/decode service.
	/// </summary>
	public interface IEncodeService
	{
		/// <summary>
		/// Encode a given number using Bijective function.
		/// </summary>
		/// <param name="url">Url to encode</param>
		/// <returns>Encoded url</returns>
		string Encode(int number);

		/// <summary>
		/// Decode the url that was encoded using Bijective function.
		/// </summary>
		/// <param name="url">Url to decode</param>
		/// <returns>Decoded url</returns>
		int Decode(string url);
	}
}
