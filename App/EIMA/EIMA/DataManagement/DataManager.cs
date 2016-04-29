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

		static DataManager instance;

		private DataManager ()
		{
		}

		public JObject getD(){
			return dataStore;
		}

		public static DataManager getInstance(){
			if (instance == null) {
				instance = new DataManager ();
			}
			return instance;
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


		public string getIncidentID(){
			return (string)dataStore["incident"]["incidentID"];
		}

		public void setIncidentID(string value){
			dataStore["incident"]["incidentID"] = value;
			rewriteObjectInMemory ();
		}

		public string getUpdateSpeed(){
			return (string)dataStore["settings"]["updateSpeed"];
		}
		public void setUpdateSpeed(string value){
			dataStore["settings"]["updateSpeed"] = value;
			rewriteObjectInMemory ();
		}

		public string getSecret(){
			return (string)dataStore["secret"];
		}

		public void setSecret(string value){
			dataStore["secret"] = value;
			rewriteObjectInMemory ();
		}


		public string getUsername(){
			return (string)dataStore["username"];
		}

		public void setUsername(string value){
			dataStore["username"] = value;
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


//		//MAP SPAN
//		public void setSpan(double num){
//			dataStore["incident"]["metainf"]["span"] = num;
//			rewriteObjectInMemory ();
//		}
//
//		//MAP DEFAULT LOCATION
//		public void setCenter(Position defLoc){
//			JObject pos = new JObject ();
//			pos ["lat"] = defLoc.Latitude;
//			pos ["long"] = defLoc.Longitude;
//
//			dataStore["incident"]["metainf"]["center"] = pos;
//			rewriteObjectInMemory ();
//		}
			
		//MAP ASSETS 
		public List<EIMAPin> getAssets(){
			List<EIMAPin> toReturn = new List<EIMAPin> ();

			JArray assets = (JArray)dataStore ["incident"] ["mapAssets"];

			foreach(JObject item in assets.Children()){
				EIMAPin toAdd = new EIMAPin ();

				toAdd.name = (string)item["name"];
				toAdd.uid = (string)item["uid"];
				toAdd.status = (string)item["status"];
				toAdd.organization = (string)item["organization"];
				toAdd.unit = (string)item ["unit"];

				toAdd.Title = toAdd.name + " (" + toAdd.organization + "," + toAdd.unit + ")";//Not sure how we want to format data
				toAdd.Subtitle = "Status:" + toAdd.status;


				toAdd.IsDraggable = !(bool)item["isUser"];


				toAdd.IsVisible = true;
				toAdd.Position = new Position ((double)item["location"]["lat"],(double)item["location"]["long"]);
				var name = ((string)item ["type"]);
				if(String.IsNullOrEmpty(name)){
					name = "Other";
				}
				toAdd.Image = (name.Replace(" ","") + ".png");
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
				toAdd ["uid"] = asset.uid;
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

		//set polygon danger zones
		public void setDangerZonePoly(List<EIMAPolygon> list){
			JArray assets = new JArray ();

			foreach (EIMAPolygon asset in list) {
				JObject toAdd = new JObject ();

				JArray coords = new JArray ();
				foreach (Position pos in asset.Coordinates) {
					JObject locObject = new JObject ();

					locObject ["lat"] = pos.Latitude;
					locObject ["long"] = pos.Longitude;

					coords.Add (locObject);
				}
				toAdd ["coords"] = coords;
				toAdd ["type"] = asset.type;
				toAdd ["note"] = asset.note;
				toAdd ["uid"] = asset.uid;

				assets.Add (toAdd);
			}

			dataStore["incident"] ["mapPolygonDangerZones"] = assets;
			rewriteObjectInMemory();

		}
		//Set circle danger zones
		public void setDangerZoneCircle(List<EIMACircle> list){
			
			JArray assets = new JArray ();
			foreach (EIMACircle asset in list) {
				JObject toAdd = new JObject ();

				JObject locObject = new JObject ();
				locObject ["lat"] = asset.Center.Latitude;
				locObject ["long"] = asset.Center.Longitude;
			
				toAdd ["location"] = locObject;
				toAdd ["type"] = asset.type;
				toAdd ["note"] = asset.note;
				toAdd ["uid"] = asset.uid;
				toAdd ["radius"] = asset.Radius;

				assets.Add (toAdd);
			}
			dataStore["incident"] ["mapCircleDangerZones"] = assets;
			rewriteObjectInMemory();
		}
		//Get circles
		public List<EIMACircle> getCircleDangerZone(){
			var toRet =  new List<EIMACircle> ();

			JArray assets = (JArray)dataStore ["incident"] ["mapCircleDangerZones"];

			foreach (JObject item in assets.Children()) {
				var circle = new EIMACircle();
				circle.Color = CONSTANTS.colorOptions [Array.IndexOf (CONSTANTS.dzTypeOptions,(string) item ["type"])];
				circle.note = (string)item ["note"];
				circle.uid = (string)item ["uid"];
				circle.Radius = (double)item ["radius"];
				circle.type = (string)item ["type"];
				circle.Center = new Position ((double)item["location"]["lat"],(double)item["location"]["long"]);

				toRet.Add (circle);
			}
			return toRet;
		}
		//Get polygons
		public List<EIMAPolygon> getPolyDangerZone(){
			var toRet = new List<EIMAPolygon> ();

			JArray assets = (JArray)dataStore ["incident"] ["mapPolygonDangerZones"];


			foreach (JObject item in assets.Children()) {
				var poly = new EIMAPolygon();
				poly.Color = CONSTANTS.colorOptions [Array.IndexOf (CONSTANTS.dzTypeOptions,(string) item ["type"])];
				poly.note = (string)item ["note"];
				poly.uid = (string)item ["uid"];
				poly.type = (string)item ["type"];

				var cordList = new List<Position> ();

				JArray coords = (JArray)item ["coords"];
				foreach (JObject pos in coords.Children()) {
					cordList.Add(new Position((double)pos["lat"],(double)pos["long"]));
				}
				List<Position> copied = new List<Position>(cordList);
				poly.Coordinates = copied;
				toRet.Add (poly);
			}

			return toRet;
		}


		//Get Users
		public List<EIMAUser> getUsers(){
			var toRet = new List<EIMAUser> ();
			JArray users = (JArray)dataStore ["incident"] ["userList"];

			foreach (JObject item in users.Children()) {
				var toAdd = new EIMAUser ();

				toAdd.level = (string)item ["level"];
				toAdd.username = (string)item ["username"];
				toAdd.name = (string)item ["name"];
				toAdd.unit = (string)item ["unit"];
				toAdd.unitType = (string)item ["unitType"];
				toAdd.status = (string)item ["status"];
				toAdd.organization = (string)item ["organization"];

				toRet.Add (toAdd);
			}
			return toRet;
		}

		//SET Users
		public void setUsers(List<EIMAUser> lst){
			JArray users = new JArray ();
			foreach (EIMAUser user in lst) {
				JObject locObject = new JObject ();
				locObject ["username"] = user.username;
				locObject ["name"] = user.name;
				locObject ["unit"] = user.unit;
				locObject ["unitType"] = user.unitType;
				locObject ["organization"] = user.organization;
				locObject ["status"] = user.status;
				locObject ["level"] = user.level;

				users.Add (locObject);
			}
			dataStore["incident"] ["userList"] = users;
			rewriteObjectInMemory();
		}

		//Get Alerts
		public List<EIMAAlert> getAlerts(){
			var toRet = new List<EIMAAlert> ();
			JArray alerts = (JArray)dataStore ["incident"] ["alerts"];

			foreach (JObject item in alerts.Children()) {
				var toAdd = new EIMAAlert ();

				toAdd.sender = (string)item ["sender"];
				toAdd.message = (string)item ["message"];
				toAdd.timestamp = (long)item ["timestamp"];

				toRet.Add (toAdd);
			}
			return toRet;
		}
		//Set Alerts
		public void setAlerts(List<EIMAAlert> lst){
			JArray users = new JArray ();
			foreach (EIMAAlert alert in lst) {
				JObject locObject = new JObject ();
				locObject ["sender"] = alert.sender;
				locObject ["message"] = alert.message;
				locObject ["timestamp"] = alert.timestamp;

				users.Add (locObject);
			}
			dataStore["incident"] ["alerts"] = users;
			rewriteObjectInMemory();
		}

		public void setIncidentType(string Type){
			dataStore ["incident"] ["incidentType"] = Type;
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
		public bool isNoAccess(){
			return "noAccess".Equals((string)dataStore ["incident"] ["role"]);
		}
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
			if (Device.OS == TargetPlatform.Android) {
				eimaInitalizeDataPath = "EIMA.Droid.DataManagement.eima_default.json";
			} else if (Device.OS == TargetPlatform.iOS) {
				eimaInitalizeDataPath = "EIMA.iOS.DataManagement.eima_default.json";
			}
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

