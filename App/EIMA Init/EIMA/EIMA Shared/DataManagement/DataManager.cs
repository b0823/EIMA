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
		//This is just a minified version of eima_default.jcfg, I'm saving myself some time by not 
		//reading it in, and as the data structure is fairly simple, it's easier to do this long term.
		static string blankData = "{\"settings\":{\"updateSpeed\":\"1 Minute\"},\"userProfile\"" +
			":{\"unit\":\"\",\"organization\":\"\",\"status\":\"\",\"unitType\":\"\"}" +
			",\"incident\":{\"role\":\"use1r\",\"incidentID\":\"null\",\"incidentType\"" +
			":\"standalone\",\"mapData\":[],\"alerts\":[]}}";


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
			return dataStore["settings"]["updateSpeed"];
		}
		public void setUpdateSpeed(string value){
			dataStore["settings"]["updateSpeed"] = value;
			rewriteObjectInMemory ();
		}
			

		/**
		 * This function needs to be called after modification of the JsonObject
		 * This is so if someone closes right after changing settings they mantain their settings.
		 */
		public void rewriteObjectInMemory(){
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

