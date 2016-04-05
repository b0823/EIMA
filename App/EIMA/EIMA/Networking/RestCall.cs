using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EIMA
{
	public static class RestCall
	{
		public static JObject POST(string url, JObject jsonContent) 
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = "application/json";
			request.Method = "POST";

			using (var streamWriter = new StreamWriter(request.GetRequestStream()))
			{
				streamWriter.Write(jsonContent.ToString());
			}

			var response = (HttpWebResponse)request.GetResponse();
			using (var streamReader = new StreamReader(response.GetResponseStream()))
			{
				var result = streamReader.ReadToEnd();
				var toRet = (JObject) JObject.Parse (result);
				return toRet;
			}
		}
	}
}

