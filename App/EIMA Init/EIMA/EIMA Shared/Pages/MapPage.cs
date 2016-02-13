using System;
using Xamarin.Forms;
using System.Collections.Generic;

using Xamarin.Forms.Maps;

namespace EIMAMaster
{

	public class MapPage : ContentPage
	{
		public MapPage ()
		{
			var map = new Map(
				MapSpan.FromCenterAndRadius(
					new Position(39.80,-84.08711527777777), Distance.FromMiles(0.3))) {
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
		}
	}
	
}