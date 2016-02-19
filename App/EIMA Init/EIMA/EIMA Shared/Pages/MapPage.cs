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
			Title = "Map";
			Icon = "Map.png";

			getLocation ();


			ToolbarItem plusTBI = null;
			ToolbarItem refreshTBI = null;
			ToolbarItem filterTBI = null;

			if (Device.OS == TargetPlatform.Android) {
				plusTBI = new ToolbarItem ("", "", () => {
				}, 0, 0);
				refreshTBI = new ToolbarItem ("", "", () => {
				}, 0, 0);
				filterTBI = new ToolbarItem ("", "", () => {
				}, 0, 0);
			}
			plusTBI.Icon = "Plus.png";
			refreshTBI.Icon = "Refresh.png";
			filterTBI.Icon = "Filter.png";

			//refresh won't be present for stdAloneUser
			ToolbarItems.Add (refreshTBI);
			ToolbarItems.Add (filterTBI);
			//+ Won't be there for netUser, is there for netMapEdit/netAdmin/stdAloneUser 
			ToolbarItems.Add (plusTBI);

		}
		public async void getLocation(){
			var position = await CrossGeolocator.Current.GetPositionAsync (timeoutMilliseconds: 15000);
			Position parsed = new Position(position.Latitude,position.Longitude);
			var curLoc = MapSpan.FromCenterAndRadius(parsed, Distance.FromMiles(5));
			map.MoveToRegion(curLoc);
		}
	}

}