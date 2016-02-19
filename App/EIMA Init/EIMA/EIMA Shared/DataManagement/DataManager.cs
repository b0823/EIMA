using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace EIMAMaster
{
	public class DataManager
	{
		private static string eimaDataPath = "eima.jcfg";
		private static string eimaInitalizeDataPath = "EIMA.DataManagement.eima_default.json";
		private static JObject dataStore;
	
		public DataManager ()
		{

		}

		/**
		 * These are the set of setters and getters for adding and setting data.
		 * Setters will reset and rewrite the file. 
		 * Getters just get from the JsonObject in memory.
		 */
		public string getUpdateSpeed(){
			return (string)dataStore["settings"]["updateSpeed"];
		}
		public void setUpdateSpeed(string value){
			dataStore["settings"]["updateSpeed"] = value;
			rewriteObjectInMemory ();
		}
			


		/**
		 * This function needs to be called after modification of the JsonObject
		 * This is so if someone closes right after changing settings they mantain their settings.
		 */
		private void rewriteObjectInMemory(){
			string text = dataStore.ToString ();
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, eimaDataPath);
			System.IO.File.WriteAllText (filePath, text);
		}

		/**
		 * This is the default file setup for the system. It gets called when the app starts.
		 * It loads the data into datastore, if there is no file, it creates a new blank one.
		 */
		public void dataStartup(){
			verifyOrInitializeData ();	
			string input = getDataFile ();
			dataStore = (JObject) JObject.Parse (input);
		}

		//Checks if datafilepath exists. 
		//If it doesn't pull from eima_default a PCL and create that file.
		private void verifyOrInitializeData () {
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, eimaDataPath);

			if (!File.Exists (filePath)) { //If there is no data file- Set the default data to filepath
				var assembly = typeof(DataManager).GetTypeInfo().Assembly;
				Stream stream = assembly.GetManifestResourceStream(eimaInitalizeDataPath);
				string text = "";
				using (var reader = new System.IO.StreamReader (stream)) {
					text = reader.ReadToEnd ();
				}				
				System.IO.File.WriteAllText (filePath, text);
			}
		}

		private string getDataFile(){
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, eimaDataPath);
			return System.IO.File.ReadAllText (filePath);
		}
	}
}

