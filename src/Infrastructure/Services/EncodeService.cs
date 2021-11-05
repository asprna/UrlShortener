using Application.Interfaces;
using System;
using System.Linq;

namespace Infrastructure.Services
{
	public class EncodeService : IEncodeService
	{
		/// <summary>
		/// Defines the alphabet.
		/// </summary>
		/// <remarks>The following characters have been removed to avoid the generation of swear words: a, A, e, E, i, I, l, o, O, u, U, y, Y, c, C, h, H.</remarks>
		private static readonly string _alphabet = "zf0TMbN95VdG1QkRpv2s6wmK3x8ZrtSDLqn7BgPWj4FXJ";

		/// <summary>
		/// If the short link length below 5, then add this char to the end of the url.
		/// </summary>
		private static readonly char _paddingChar = 'e';

		/// <summary>
		/// Defines the encoded number base. Must be equal to the length of <see cref="_alphabet"/>.
		/// </summary>
		private static readonly int _base = _alphabet.Length;

		/// <summary>
		/// Defines the maximum number of digits an encoded representation may have.
		/// </summary>
		private static readonly int _digits = 5;

		/// <summary>
		/// Defines the maximum number that can be generated. Should be equal to <see cref="_alphabet"/> to the power of <see cref="_digits"/> minus 1.
		/// </summary>
		/// <example>
		/// 5 digits (base 45): 184,528,124
		/// </example>
		private static readonly int _maxNumber = 184528124;

		/// <summary>
		/// Defines the minimum number that can be generated.
		/// </summary>
		private static readonly int _minNumber = 1;

		/// <summary>
		/// Decodes a number encoded by <see cref="Encode"/>.
		/// </summary>
		/// <param name="url">Decoded url.</param>
		/// <returns>Decoded number.</returns>
		public int Decode(string url)
		{
			return AlphabetToNumber(url);
		}

		/// <summary>
		/// Encodes a number. Decode with <see cref="Decode"/>.
		/// </summary>
		/// <param name="number">Number to encode.</param>
		/// <returns>Encoded number.</returns>
		public string Encode(int number)
		{
			return NumberToAlphabet(number);
		}

		/// <summary>
		/// Encodes a number using the alphabet.
		/// </summary>
		/// <param name="number">Number to encode.</param>
		/// <returns>Number encoded in alphabet.</returns>
		private string NumberToAlphabet(int number)
		{
			if (number < _minNumber || number > _maxNumber)
			{
				throw new ArgumentOutOfRangeException(nameof(number), number, $"Number must be greater than or equal to {_minNumber} and less than or equal to {_maxNumber}.");
			}

			var result = string.Empty;

			do
			{
				result += _alphabet[number % _base];
				number = number / _base;
			}
			while (number > 0);

			if (result.Length > _digits)
			{
				throw new ArgumentException("Number must be less than or equal to " + _maxNumber, "number");
			}

			// pad out digits to the right
			var final = string.Join(string.Empty, result.Reverse());
			
			return final.PadRight(_digits, _paddingChar);
		}

		/// <summary>
		/// Converts a number encoded in the alphabet to its numeric form.
		/// </summary>
		/// <param name="url">Encoded url.</param>
		/// <returns>Decoded number.</returns>
		private int AlphabetToNumber(string url)
		{
			//Remove any padding chars.
			url = url.Replace(_paddingChar.ToString(), string.Empty);

			int result = 0;

			foreach(var c in url)
			{
				result = (result * _base) + _alphabet.IndexOf(c);
			}
			
			return result;
		}
	}
}
