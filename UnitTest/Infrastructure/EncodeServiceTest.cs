using Infrastructure.Services;
using System;
using Xunit;

namespace UnitTest
{
	public class EncodeServiceTest
	{
		private readonly EncodeService _encodeService;

		public EncodeServiceTest()
		{
			_encodeService = new EncodeService();
		}

		/// <summary>
		/// Verify Encoder uses the algorithm correctly when encoding.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="expected"></param>
		[Theory]
		[InlineData(101, "0Geee")]
		[InlineData(1234, "Zseee")]
		[InlineData(86597, "Fnvee")]
		[InlineData(756981, "5QBBe")]
		[InlineData(5987369, "f6DLt")]
		[InlineData(69513576, "pFgDN")]
		public void Encode_ValidNumbers_ReturnValidString(int number, string expected)
		{
			//Arrange

			//Act
			var result = _encodeService.Encode(number);

			//Assert
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// Verify Encode correctly checks the maximum and minimum values
		/// </summary>
		/// <param name="number"></param>
		[Theory]
		[InlineData(0)]
		[InlineData(184528125)]
		public void Encode_ValidMinMaxNumber_ErrorReturn(int number)
		{
			//Arrange

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => _encodeService.Encode(number));
		}

		/// <summary>
		/// Verify Decoder uses the algorithm correctly when decoding.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="expected"></param>
		[Theory]
		[InlineData("0Geee", 101)]
		[InlineData("Zseee", 1234)]
		[InlineData("Fnvee", 86597)]
		[InlineData("5QBBe", 756981)]
		[InlineData("f6DLt", 5987369)]
		[InlineData("pFgDN", 69513576)]
		public void Decode_ValidUrl_ReturnValidNumber(string url, int expected)
		{
			//Arrange

			//Act
			var result = _encodeService.Decode(url);

			//Assert
			Assert.Equal(expected, result);
		}
	}
}
