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


		//public profilename
		public string getName(){
			return (string)dataStore["userProfile"]["name"];
		}

		public void setName(string value){
			dataStore["userProfile"]["name"] = value;
			rewriteObjectInMemory ();
		}
		//Organization
		public string getOrg(){
			return (string)dataStore["userProfile"]["organization"];
		}
		public void setOrg(string value){
			dataStore["userProfile"]["organization"] = value;
			rewriteObjectInMemory ();
		}

		//unit
		public string getUnitID(){
			return (string)dataStore["userProfile"]["unit"];
		}
		public void setUnitID(string value){
			dataStore["userProfile"]["unit"] = value;
			rewriteObjectInMemory ();
		}

		//unitType
		public string getUnitType(){
			return (string)dataStore["userProfile"]["unitType"];
		}
		public void setUnitType(string value){
			dataStore["userProfile"]["unitType"] = value;
			rewriteObjectInMemory ();
		}

		//status
		public string getStatus(){
			return (string)dataStore["userProfile"]["status"];
		}
		public void setStatus(string value){
			dataStore["userProfile"]["status"] = value;
			rewriteObjectInMemory ();
		}

		//Incident Role
		public void setRole(string role){
			dataStore ["incident"] ["role"] = role;
			rewriteObjectInMemory ();
		}


		//FILTER

		public void setFilter(string filterName, bool value){
			dataStore["settings"]["filter"][filterName] = value;
			rewriteObjectInMemory ();
		}

		public bool getFilter(string filterName){
			return (bool)dataStore["settings"]["filter"][filterName];
		}


		/*
		 * Block of is functions.
		 */

		//Incident Types
		public bool isStandAlone(){
			return "standalone".Equals((string)dataStore ["incident"] ["incidentType"]);
		}

		public bool isNetworked(){
			return "networked".Equals((string)dataStore ["incident"] ["incidentType"]);
		}


		//User Groups
		public bool isUser(){
			return "user".Equals((string)dataStore ["incident"] ["role"]);
		}

		public bool isMapEditor(){
			return "mapEditor".Equals((string)dataStore ["incident"] ["role"]);
		}

		public bool isAdmin(){
			return "admin".Equals((string)dataStore ["incident"] ["role"]);
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

