using System;
using Xamarin.Forms;
using System.Collections.Generic;

using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace EIMAMaster
{

	public class MapPage : ContentPage
	{
		Map map;
		Position defaultLocation = new Position (39.8, -84.08711552);

		public MapPage ()
		{

			map = new Map(
				MapSpan.FromCenterAndRadius(defaultLocation, Distance.FromMiles(0.3))) {
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(map);
			Content = stack;
			Title = "Maps";
			Icon = "Map.png";

			getLocation ();



			////TOOLBAR RELATED CODE
			ToolbarItem plusTBI = null;
			ToolbarItem refreshTBI = null;
			ToolbarItem filterTBI = null;
			ToolbarItem mapTypeTBI = null;

			plusTBI = new ToolbarItem ("", "", () => {addToMap();}, 0, 0);
			refreshTBI = new ToolbarItem ("", "", () => {refreshData();}, 0, 0);
			filterTBI = new ToolbarItem ("", "", () => {filterMapItems();}, 0, 0);
			mapTypeTBI = new ToolbarItem ("", "", () => {changeMap();}, 0, 0);

			plusTBI.Icon = "Plus.png";
			refreshTBI.Icon = "Refresh.png";
			filterTBI.Icon = "Filter.png";
			mapTypeTBI.Icon = "MapChange.png";

			//Change map type
			ToolbarItems.Add (mapTypeTBI);
			//refresh won't be present for stdAloneUser
			ToolbarItems.Add (refreshTBI);
			ToolbarItems.Add (filterTBI);
			//+ Won't be there for netUser, is there for netMapEdit/netAdmin/stdAloneUser 
			ToolbarItems.Add (plusTBI);

		}

		public void addToMap(){
			//https://forums.xamarin.com/discussion/39024/detect-a-tap-and-add-a-pin-to-the-map
		}

		public void filterMapItems(){
			//Easiest might be a pop up with a select what you want.. OK/CANCEL
			//https://developer.xamarin.com/guides/xamarin-forms/user-interface/navigation/pop-ups/
		}

		public void refreshData(){
			//This won't be implemented for a while.
		}

		public void changeMap(){
			if (map.MapType == MapType.Hybrid) {
				map.MapType = MapType.Street;
			} else
				map.MapType = MapType.Hybrid;
		}

		public async void getLocation(){
			var position = await CrossGeolocator.Current.GetPositionAsync (timeoutMilliseconds: 15000);
			Position parsed = new Position(position.Latitude,position.Longitude);
			var curLoc = MapSpan.FromCenterAndRadius(parsed, Distance.FromMiles(5));
			map.MoveToRegion(curLoc);
		}
	}

}