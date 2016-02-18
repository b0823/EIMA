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
			//stack.Children.Add (button);
			stack.Children.Add(map);
			Content = stack;
			Title = "Map";
			Icon = "Map.png";

			getLocation ();


		}
		public async void getLocation(){
			var position = await CrossGeolocator.Current.GetPositionAsync (timeoutMilliseconds: 15000);
			Position parsed = new Position(position.Latitude,position.Longitude);
			var curLoc = MapSpan.FromCenterAndRadius(parsed, Distance.FromMiles(5));
			map.MoveToRegion(curLoc);
		}
	}

}