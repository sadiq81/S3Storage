using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

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

		public static string SerializeObject<T> (this T toSerialize)
		{
			StringWriter textWriter = new StringWriter ();
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces ();
			ns.Add ("", "");
			using (XmlWriter writer = XmlWriter.Create (textWriter, new XmlWriterSettings { OmitXmlDeclaration = true })) {
				new XmlSerializer (toSerialize.GetType ()).Serialize (writer, toSerialize, ns);
			}
			return textWriter.ToString ();
		}
	}
}

