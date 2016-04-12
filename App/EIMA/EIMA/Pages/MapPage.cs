using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using TK.CustomMap;

namespace EIMA
{

	public class MapPage : ContentPage
	{
		TKCustomMap map;
		Position defaultLocation = new Position (39.8, -84.08711552);
		FilterPage multiPage; //Used in Filter function


		MapModel myModel;

		public MapPage ()
		{		
			startAndBindMap();
			setToolBar ();
			loadData ();
		}

		public void setToolBar(){
			/**
			 * TOOLBAR RELATED CODE
			 */
			var data = DataManager.getInstance ();

			ToolbarItem filterTBI;
			ToolbarItem mapTypeTBI;

			filterTBI = new ToolbarItem ("", "", filterMapItems, 0, 0);
			mapTypeTBI = new ToolbarItem ("", "", changeMap, 0, 0);

			filterTBI.Icon = "Filter.png";
			mapTypeTBI.Icon = "MapChange.png";

			//Change map type
			ToolbarItems.Add (mapTypeTBI);

			if (data.isNetworked()) {
				ToolbarItem refreshTBI;			
				refreshTBI = new ToolbarItem ("", "", refreshData, 0, 0);
				refreshTBI.Icon = "Refresh.png";
				ToolbarItems.Add (refreshTBI);
			}

			ToolbarItems.Add (filterTBI);


		}

		public void startAndBindMap(){
			//var data = DataManager.getInstance ();
			const int miles = 5;
			Position locationDef = defaultLocation;
			MapSpan ms;

			if (locationDef != default(Position))
				ms = MapSpan.FromCenterAndRadius (locationDef, Distance.FromMiles (miles));
			else {
				ms = MapSpan.FromCenterAndRadius (defaultLocation, Distance.FromMiles (miles));
				getAndSetLocation (miles);
			}
			map = new TKCustomMap(ms);
			map.IsShowingUser = true;



			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(map);
			Content = stack;
			Title = "Maps";
			Icon = "Map.png";

			map.SetBinding(TKCustomMap.CustomPinsProperty, "Pins");
			map.SetBinding(TKCustomMap.MapClickedCommandProperty, "MapClickedCommand");
			map.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");
			map.SetBinding(TKCustomMap.MapCenterProperty, "MapCenter");
			map.SetBinding(TKCustomMap.PinSelectedCommandProperty, "PinSelectedCommand");
			map.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
			map.SetBinding(TKCustomMap.RoutesProperty, "Routes");
			map.SetBinding(TKCustomMap.PinDragEndCommandProperty, "DragEndCommand");
			map.SetBinding(TKCustomMap.CirclesProperty, "Circles");
			map.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
			map.SetBinding(TKCustomMap.PolylinesProperty, "Lines");
			map.SetBinding(TKCustomMap.PolygonsProperty, "Polygons");
			map.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
			map.SetBinding(TKCustomMap.RouteClickedCommandProperty, "RouteClickedCommand");
			map.SetBinding(TKCustomMap.RouteCalculationFinishedCommandProperty, "RouteCalculationFinishedCommand");
			map.SetBinding(TKCustomMap.TilesUrlOptionsProperty, "TilesUrlOptions");
			map.AnimateMapCenterChange = true;

			myModel = new MapModel ();

			BindingContext = myModel;

		}

		public void loadData(){
			var data = DataManager.getInstance ();
			var assetList = data.getAssets ();
			var circleList = data.getCircleDangerZone ();
			var polyList = data.getPolyDangerZone ();


			foreach(EIMAPin element in assetList){
				myModel.addPin (element);
			}
			foreach(EIMACircle element in circleList){ 
				myModel.addCircle (element);
			}
			foreach(EIMAPolygon element in polyList){ 
				myModel.addPoly (element);
			}

			foreach (string element in CONSTANTS.uTypeOptions) { //set filter settings
				myModel.filterPins (element, data.getFilter (element));
			}

			foreach (string element in CONSTANTS.dzTypeOptions) { //set filter settings
				myModel.filterDZ (element, data.getFilterDZ (element));
			}

		}

		public async void filterMapItems(){
			var items = new List<FilterObject>();
			var dzItems = new List<FilterObject>();
			var data = DataManager.getInstance ();

			foreach (string element in CONSTANTS.uTypeOptions)
			{
				items.Add(new FilterObject{Name = element, IsSelected = data.getFilter(element)});
			}

			foreach (string element in CONSTANTS.dzTypeOptions)
			{
				dzItems.Add(new FilterObject{Name = element, IsSelected = data.getFilterDZ(element)});
			}
			if (multiPage == null)
				multiPage = new FilterPage (items, dzItems, myModel);
			//multiPage.SelectAll ();//Just for proof of concept. Would need to make this data driven.
			await Navigation.PushAsync (multiPage);

		}

		public void refreshData(){
			//TODO
			//This won't be implemented for a while.
		}

		public void changeMap(){
			if (map.MapType == MapType.Hybrid) {
				map.MapType = MapType.Street;
			} else
				map.MapType = MapType.Hybrid;
		}

		public async void getAndSetLocation(double miles){
			var position = await CrossGeolocator.Current.GetPositionAsync ();
			var parsed = new Position(position.Latitude,position.Longitude);
			var curLoc = MapSpan.FromCenterAndRadius(parsed, Distance.FromMiles(miles));
			map.MoveToRegion(curLoc);
		}
	}

}