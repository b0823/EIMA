using System;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Json;

namespace EIMAMaster
{
	public class DataManager
	{
		static string defaultFileName = "eima.jcfg";
		static string blankData = "{\n  \"settings\": {\n      \"updateSpeed\":\"1 Minute\"\n  },\n  \"userProfile\": {\n    \"unit\":\"\",\n    \"organization\":\"\",\n    \"status\":\"\",\n    \"unitType\":\"\"\n  },\n  \"incident\": {\n    \"role\": \"use1r\",\n    \"incidentID\":\"null\",\n    \"incidentType\": \"standalone\",\n    \"mapData\":[\n    \n    ],\n    \"alerts\":[\n    ]\n  }\n}\n";
		private static JsonObject dataStore;
	
		public DataManager ()
		{

		}
		public void setData(){
			SaveText (defaultFileName, blankData);	
			string input = readIn (defaultFileName);
			dataStore = (JsonObject) JsonObject.Parse (input);
		}
		public void SaveText (string filename, string text) {
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, filename);
			System.IO.File.WriteAllText (filePath, text);
		}
		public string readIn(string filename){
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, filename);
			return System.IO.File.ReadAllText (filePath);
		}
		public string getUpdateSpeed(){
			// 2;
			return getSettings ()["updateSpeed"];
		}
		private JsonObject getIncident(){
			return (JsonObject) dataStore["incident"];
		}
		private JsonObject getSettings(){
			return (JsonObject) dataStore["settings"];
		}
	}
}

