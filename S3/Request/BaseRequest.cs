using System;
using S3Storage.S3;
using System.Collections.Generic;
using System.Text;

namespace S3Storage.Request
{
	public class BaseRequest
	{
		private const string SEPARATOR_1 = ":";
		private const string SEPARATOR_2 = ";";
		private const  string NEW_LINE = "\n";

		public Uri Uri { get; set; }

		public Region Region { get; set; }

		public string Range { get; set; }

		public string HttpMethod { get; set; }

		public string Authorization { get; set; }

		public string ContentLength { get; set; }

		public string ContentType { get; set; }

		public string ContentMD5 { get; set; }

		public DateTime Date { get; set; }

		public string Expect{ get; set; }

		public string Host { get; set; }

		public string XAmzContentSha256 { get; set; }

		public string XAmzDate { get; set; }

		public string XAmzSecurityToken { get; set; }

		public BaseRequest ()
		{
		}

		public string GetCanonicalHeaders ()
		{
			StringBuilder sb = new StringBuilder ();
			foreach (KeyValuePair<string, string> kvp in GetHeaders ()) {
				sb.Append (kvp.Key.ToLower () + SEPARATOR_2);
			}
			sb.Remove (sb.Length - 1, 1);
			return sb.ToString ();
		}

		public string GetSignedHeaders ()
		{
			StringBuilder sb = new StringBuilder ();
			foreach (KeyValuePair<string, string> kvp in GetHeaders ()) {
				sb.Append (kvp.Key.ToLower () + SEPARATOR_1 + kvp.Value.Trim () + NEW_LINE);
			}
			return sb.ToString ();
		}

		public SortedDictionary<string, string> GetHeaders ()
		{
			SortedDictionary<string, string> headers = new SortedDictionary<string, string> ();
			if (Authorization != null) {
				headers.Add ("Authorization", Authorization);
			}
			if (ContentLength != null) {
				headers.Add ("Content-Length", ContentLength);
			}
			if (ContentType != null) {
				headers.Add ("Content-Type", ContentType);
			}
			if (ContentMD5 != null) {
				headers.Add ("Content-MD5", ContentMD5);
			}
			if (XAmzDate == null && Date != null) {
				headers.Add ("Date", Date.ToString ());
			}
			if (Expect != null) {
				headers.Add ("Expect", Expect);
			}
			if (Host != null) {
				headers.Add ("Host", Host);
			}
			if (XAmzContentSha256 != null) {
				headers.Add ("x-amz-content-sha256", XAmzContentSha256);
			}
			if (XAmzDate != null) {
				headers.Add ("x-amz-date", XAmzDate);
			}
			if (XAmzSecurityToken != null) {
				headers.Add ("x-amz-security-token", XAmzSecurityToken);
			}
			if (Range != null) {
				headers.Add ("Range", Range);
			}
			return headers;
		}
	}
}

