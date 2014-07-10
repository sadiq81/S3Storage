using System.Text;
using System.Globalization;

namespace S3Storage.Utils
{
	internal static class StringUtilities
	{
		private const string rfc3986validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

		public static string ToRfc3986 (this string input)
		{
			StringBuilder encoded = new StringBuilder (input.Length * 2);
			foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(input)) {
				if (rfc3986validChars.IndexOf (symbol) == -1) {
					encoded.Append ("%").Append (string.Format (CultureInfo.InvariantCulture, "{0:X2}", (int)symbol));

				} else {
					encoded.Append (symbol);
				}
			}

			return encoded.ToString ();
		}

		public static string objectKeyNameToRfc3986 (this string input)
		{
			StringBuilder encoded = new StringBuilder (input.Length * 2);
			foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(input)) {
				if (rfc3986validChars.IndexOf (symbol) == -1 && !'/'.Equals (symbol)) {
					encoded.Append ("%").Append (string.Format (CultureInfo.InvariantCulture, "{0:X2}", (int)symbol));

				} else {
					encoded.Append (symbol);
				}
			}

			return encoded.ToString ();
		}

		public static string ToHexString (this byte[] data, bool lowercase)
		{
			var sb = new StringBuilder ();
			for (var i = 0; i < data.Length; i++) {
				sb.Append (data [i].ToString (lowercase ? "x2" : "X2"));
			}
			return sb.ToString ();
		}
	}
}

