using Plugin.Geolocator;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using Xamarin.Forms.Maps;

namespace EIMA
{
	public static class DataNetworkCalls
	{
		public static void updateNetworkedData(){
			var data = DataManager.getInstance();
			var postData = new JObject ();

			postData ["token"] = data.getSecret();

			var accessRes = RestCall.POST (URLs.ACCESSLEVEL, postData);
			if ((bool)accessRes ["result"]) {
				Console.WriteLine (accessRes);

				data.setRole ((string)accessRes ["accessLevel"]);
			}

			if (data.isNoAccess () || data.isStandAlone ()) {
				return;
			}
			SENDLOCATION (postData);

			var MapData = RestCall.POST (URLs.MAPDATA, postData);
			if ((bool)MapData ["result"]) {
				Console.WriteLine (MapData);
				updateMapData (MapData);
			}

			var AlertsData = RestCall.POST (URLs.ALERTS, postData);
			if ((bool)MapData ["result"]) {
				Console.WriteLine (AlertsData);
				updateAlertsData (AlertsData);
			}
				
			if (data.isAdmin ()) {
				//user list
				var result = RestCall.POST (URLs.USERLIST, postData);
				Console.WriteLine (result);

				if(!(bool)result["result"]){
					return;
				}
				var userList = (JArray)result ["userList"];
				var addTo = new List<EIMAUser>();

				foreach (JObject item in userList.Children()) {
					
					var user = new EIMAUser ();
					user.level = (string)item ["level"];
					user.username = (string)item ["username"];
					user.name = (string)item ["name"];
					user.unit = (string)item ["unit"];
					user.unitType = (string)item ["unitType"];
					user.status = (string)item ["status"];
					user.organization = (string)item ["organization"];


					addTo.Add (user);
				}
				addTo.ForEach(Console.WriteLine);

				data.setUsers (addTo);
			}

			Console.WriteLine (data.getD());


		}

		static void updateAlertsData (JObject alertsData)
		{
			var data = DataManager.getInstance ();
			var theList = new List<EIMAAlert> ();
			var alerts = (JArray)alertsData ["alerts"];

			foreach (JObject item in alerts.Children()) {
				var toAdd = new EIMAAlert ();

				toAdd.sender = (string)item ["sender"];
				toAdd.message = (string)item ["message"];
				toAdd.timestamp = (long)item ["timestamp"];

				theList.Add (toAdd);
			}

			data.setAlerts (theList);

		}

		static void updateMapData (JObject mapData)
		{
			var data = DataManager.getInstance ();

			var assetJArr = (JArray)mapData ["mapAssets"];
			var circJArr = (JArray)mapData ["mapCircles"];
			var polyJArr = (JArray)mapData ["mapPolygons"];

			var polyList = new List<EIMAPolygon> ();
			var circleList = new List<EIMACircle> ();
			var assetList = new List<EIMAPin> ();


			foreach (JObject item in polyJArr.Children()) {
				var poly = new EIMAPolygon();
				poly.note = (string)item ["note"];
				poly.uid = (string)item ["uid"];
				poly.type = (string)item ["type"];

				var cordList = new List<Position> ();

				JArray coords = (JArray)item ["points"];
				foreach (JObject pos in coords.Children()) {
					cordList.Add(new Position((double)pos["Latitude"],(double)pos["Longitude"]));
				}
				List<Position> copied = new List<Position>(cordList);
				poly.Coordinates = copied;
				polyList.Add (poly);
			}

			foreach(JObject item in assetJArr.Children()){
				EIMAPin toAdd = new EIMAPin ();

				toAdd.name = (string)item["name"];
				toAdd.uid = (string)item["uid"];
				toAdd.status = (string)item["status"];
				toAdd.organization = (string)item["organization"];
				toAdd.unit = (string)item ["unit"];

				toAdd.Subtitle = "Status:" + toAdd.status;


				toAdd.Position = new Position ((double)item["position"]["Latitude"],(double)item["position"]["Longitude"]);
				toAdd.unitType = (string)item ["type"];

				assetList.Add (toAdd);
			}

			foreach (JObject item in circJArr.Children()) {
				var circle = new EIMACircle();
				circle.note = (string)item ["note"];
				circle.uid = (string)item ["uid"];
				circle.Radius = (double)item ["radius"];
				circle.type = (string)item ["type"];
				circle.Center = new Position ((double)item["center"]["lat"],(double)item["center"]["long"]);

				circleList.Add (circle);
			}

			data.setDangerZoneCircle (circleList);
			data.setAssets (assetList);
			data.setDangerZonePoly (polyList);

		}

		async static void SENDLOCATION(JObject postData){
			var postitionData = (JObject) postData.DeepClone ();
			var position = await CrossGeolocator.Current.GetPositionAsync ();

			postitionData ["latitude"] = position.Latitude;
			postitionData ["longitude"] = position.Longitude;

			RestCall.POST (URLs.MAPDATA, postitionData);
		}

		public static void sendMessage(){}
		public static void setPrivlege(){}

	}
}

