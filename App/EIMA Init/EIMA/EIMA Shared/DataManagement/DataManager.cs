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
		/**
		 * These are the set of setters and getters for adding and setting data.
		 * Setters will reset and rewrite the file. 
		 * Getters just get from the JsonObject in memory.
		 */
		public string getUpdateSpeed(){
			return getSettings ()["updateSpeed"];
		}
		public void setUpdateSpeed(string value){
			dataStore["settings"]["updateSpeed"] = value;
			rewriteObject ();
		}


		/**
		 * These are private utility functions for getting the three
		 * core objects within the data file. 
		 * I'm now questioning if I should remove these. We'll see.
		 */
		private JsonObject getIncident(){
			return (JsonObject) dataStore["incident"];
		}
		private JsonObject getSettings(){
			return (JsonObject) dataStore["settings"];
		}
		private JsonObject getUserProfile(){
			return (JsonObject) dataStore["userProfile"];
		}

		/**
		 * This function needs to be called after modification of the JsonObject
		 * This is so if someone closes right after changing settings they mantain their settings.
		 */
		public void rewriteObject(){
			string text = dataStore.ToString ();
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, defaultFileName);
			System.IO.File.WriteAllText (filePath, text);
		}

		/**
		 * This is the default file setup for the system. It gets called when the app starts.
		 * It loads the data into datastore, if there is no file, it creates a new blank one.
		 */
		public void setData(){
			SaveText (defaultFileName, blankData);	
			string input = readIn (defaultFileName);
			dataStore = (JsonObject) JsonObject.Parse (input);
		}

		public void SaveText (string filename, string text) {
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, filename);

			if (!File.Exists (filePath)) { //If there is no data file- Set the default data to filepath
				System.IO.File.WriteAllText (filePath, text);
			}

		}
		public string readIn(string filename){
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, filename);
			return System.IO.File.ReadAllText (filePath);
		}
	}
}

