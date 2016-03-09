using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EIMA
{
	public class DataManager
	{
		const string eimaDataPath = "eima.jcfg";
		static string eimaInitalizeDataPath;
		static JObject dataStore;
	
		public DataManager ()
		{
			if(Device.OS == TargetPlatform.Android)
				eimaInitalizeDataPath = "EIMA.Droid.DataManagement.eima_default.json";
			else if(Device.OS == TargetPlatform.iOS)
				eimaInitalizeDataPath = "EIMA.iOS.DataManagement.eima_default.json";
		}

		public void resetStandAlone(){
			JObject defData = JObject.Parse (getDefData ());
			dataStore ["incident"] = defData ["incident"];
			rewriteObjectInMemory ();
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


		//DANGER ZONE FILTERS

		public void setDZFilter(string filterName, bool value){
			dataStore["settings"]["filterDZ"][filterName] = value;
			rewriteObjectInMemory ();
		}

		public bool getFilterDZ(string filterName){
			return (bool)dataStore["settings"]["filterDZ"][filterName];
		}


		//FILTER

		public void setFilter(string filterName, bool value){
			dataStore["settings"]["filter"][filterName] = value;
			rewriteObjectInMemory ();
		}

		public bool getFilter(string filterName){
			return (bool)dataStore["settings"]["filter"][filterName];
		}


		//MAP SPAN
		public void setSpan(double num){
			dataStore["incident"]["metainf"]["span"] = num;
			rewriteObjectInMemory ();
		}

		public double getSpan(){
			return (double)dataStore ["incident"] ["metainf"] ["span"];
		}

		//MAP DEFAULT LOCATION
		public void setCenter(Position defLoc){
			JObject pos = new JObject ();
			pos ["lat"] = defLoc.Latitude;
			pos ["long"] = defLoc.Longitude;

			dataStore["incident"]["metainf"]["center"] = pos;
			rewriteObjectInMemory ();
		}

		public Position getCenter(){
			string data = (string)dataStore ["incident"] ["metainf"] ["center"];
			if (IsValidJson (data)) {
				var obj = (JObject)dataStore ["incident"] ["metainf"] ["center"];
				return new Position ((double)obj["lat"],(double)obj["long"]);
			} 
			return default(Position);
		}
		//MAP ASSETS 
		public List<EIMAPin> getAssets(){
			List<EIMAPin> toReturn = new List<EIMAPin> ();

			JArray assets = (JArray)dataStore ["incident"] ["mapAssets"];

			foreach(JObject item in assets.Children()){
				EIMAPin toAdd = new EIMAPin ();

				toAdd.name = (string)item["name"];
				toAdd.username = (string)item["username"];
				toAdd.status = (string)item["status"];
				toAdd.organization = (string)item["organization"];
				toAdd.unit = (string)item ["unit"];

				toAdd.Title = toAdd.name + " (" + toAdd.organization + "," + toAdd.unit + ")";//Not sure how we want to format data
				toAdd.Subtitle = "Status:" + toAdd.status;


				toAdd.IsDraggable = !(bool)item["isUser"];
				toAdd.IsVisible = true;
				toAdd.Position = new Position ((double)item["location"]["lat"],(double)item["location"]["long"]);
				toAdd.Image = ((string)item["type"]).Replace(" ","") + ".png";
				toAdd.unitType = (string)item ["type"];
				toAdd.ShowCallout = true;

				toReturn.Add (toAdd);
			}

			return toReturn;
		}
		//SET
		public void setAssets(List<EIMAPin> newAssets){
			JArray assets = new JArray ();

			foreach (EIMAPin asset in newAssets) {
				JObject toAdd = new JObject ();

				JObject locObject = new JObject ();
				locObject ["lat"] = asset.Position.Latitude;
				locObject ["long"] = asset.Position.Longitude;

				toAdd ["type"] = asset.unitType;
				toAdd ["username"] = asset.username;
				toAdd ["name"] = asset.name;
				toAdd ["unit"] = asset.unit;
				toAdd ["status"] = asset.status;
				toAdd ["organization"] = asset.organization;
				toAdd ["location"] = locObject;
				toAdd ["isUser"] = !asset.IsDraggable;

				assets.Add (toAdd);
			}
			dataStore["incident"] ["mapAssets"] = assets;
			rewriteObjectInMemory();
		}
		///Danger Zones
		public void setDangerZonePoly(List<EIMAPolygon> list){

		}
		public void setDangerZoneCircle(List<EIMACircle> list){
		
		}

		public List<EIMACircle> getCircleDangerZone(){
			return new List<EIMACircle> ();
		}
		public List<EIMAPolygon> getPolyDangerZone(){
			return new List<EIMAPolygon> ();
		}

		/*
		 * Block of is functions Relating to states and user privledges.
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
		void rewriteObjectInMemory(){
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
			string input = getDataFile (eimaDataPath);
			dataStore = (JObject) JObject.Parse (input);
		}

		string getDefData(){
			var assembly = typeof(DataManager).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream(eimaInitalizeDataPath);
			string text = "";
			using (var reader = new System.IO.StreamReader (stream)) {
				text = reader.ReadToEnd ();
			}	
			return text;
		}

		//Checks if datafilepath exists. 
		//If it doesn't pull from eima_default a PCL and create that file.
		void verifyOrInitializeData () {
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, eimaDataPath);

			if (!File.Exists (filePath)) { //If there is no data file- Set the default data to filepath
				var text = getDefData();
				System.IO.File.WriteAllText (filePath, text);
			}
		}

		string getDataFile(string data){
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, data);
			return System.IO.File.ReadAllText (filePath);
		}
		static bool IsValidJson(string strInput)
		{
			strInput = strInput.Trim();
			if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
				(strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
			{
				try
				{
					JToken.Parse(strInput);
					return true;
				}
				catch (JsonReaderException jex)
				{
					//Exception in parsing json
					Console.WriteLine(jex.StackTrace);
					return false;
				}
				catch (Exception ex) //some other exception
				{
					Console.WriteLine(ex.StackTrace);
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}

