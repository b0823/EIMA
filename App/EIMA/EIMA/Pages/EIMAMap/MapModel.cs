using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Linq;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using TK.CustomMap;

namespace EIMA
{
	public class MapModel : INotifyPropertyChanged
	{
		private TKTileUrlOptions _tileUrlOptions;

		private MapSpan _mapRegion;
		private Position _mapCenter;
		private TKCustomMapPin _selectedPin;
		private ObservableCollection<TKCustomMapPin> _pins;
		private ObservableCollection<TKRoute> _routes;
		private ObservableCollection<TKCircle> _circles;
		private ObservableCollection<TKPolyline> _lines;
		private ObservableCollection<TKPolygon> _polygons;

		//Backup list as polygons/circles have no visible field
		private List<TKCircle> invisCircles;
		private List<TKPolygon> invisPoly;

		public TKTileUrlOptions TilesUrlOptions
		{
			get 
			{
				return this._tileUrlOptions;
				//return new TKTileUrlOptions(
				//    "http://a.basemaps.cartocdn.com/dark_all/{2}/{0}/{1}.png", 256, 256, 0, 18);
				//return new TKTileUrlOptions(
				//    "http://a.tile.openstreetmap.org/{2}/{0}/{1}.png", 256, 256, 0, 18);
			}
			set
			{
				if(this._tileUrlOptions != value)
				{
					this._tileUrlOptions = value;
					this.OnPropertyChanged("TilesUrlOptions");
				}
			}
		}

		/// <summary>
		/// Map region bound to <see cref="TKCustomMap"/>
		/// </summary>
		public MapSpan MapRegion
		{
			get { return this._mapRegion; }
			set
			{
				if (this._mapRegion != value)
				{
					this._mapRegion = value;
					this.OnPropertyChanged("MapRegion");
				}
			}
		}
		/// <summary>
		/// Pins bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public ObservableCollection<TKCustomMapPin> Pins
		{
			get { return this._pins; }
			set 
			{
				if (this._pins != value)
				{
					this._pins = value;
					this.OnPropertyChanged("Pins");
				}
			}
		}
		/// <summary>
		/// Routes bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public ObservableCollection<TKRoute> Routes
		{
			get { return this._routes; }
			set
			{
				if (this._routes != value)
				{
					this._routes = value;
					this.OnPropertyChanged("Routes");
				}
			}
		}
		/// <summary>
		/// Circles bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public ObservableCollection<TKCircle> Circles
		{
			get { return this._circles; }
			set
			{
				if (this._circles != value)
				{
					this._circles = value;
					this.OnPropertyChanged("Circles");
				}
			}
		}
		/// <summary>
		/// Lines bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public ObservableCollection<TKPolyline> Lines
		{
			get { return this._lines; }
			set
			{
				if (this._lines != value)
				{
					this._lines = value;
					this.OnPropertyChanged("Lines");
				}
			}
		}
		/// <summary>
		/// Polygons bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public ObservableCollection<TKPolygon> Polygons
		{
			get { return this._polygons; }
			set
			{
				if (this._polygons != value)
				{
					this._polygons = value;
					this.OnPropertyChanged("Polygons");
				}
			}
		}
		/// <summary>
		/// Map center bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Position MapCenter
		{
			get { return this._mapCenter; }
			set 
			{
				if (this._mapCenter != value)
				{
					this._mapCenter = value;
					this.OnPropertyChanged("MapCenter");
				}
			}
		}
		/// <summary>
		/// Selected pin bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public TKCustomMapPin SelectedPin
		{
			get { return this._selectedPin; }
			set
			{
				if (this._selectedPin != value)
				{
					this._selectedPin = value;
					this.OnPropertyChanged("SelectedPin");
				}
			}
		}
		/// <summary>
		/// Map Long Press bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Command<Position> MapLongPressCommand
		{
			get
			{
				return new Command<Position>(async position => 
					{
						var action = await Application.Current.MainPage.DisplayActionSheet(
							"",
							"Cancel",
							null,
							"Create Asset",
							"Add Danger Zone"
						);
						
						if (action == "Create Asset")
						{
							
							var pin = new EIMAPin
							{
								Position = position,
							};						

							AddAssetToMapPage aToMapPage = new AddAssetToMapPage(pin,false,this);//is false because this is a new pin.
							await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);

						}




						else if(action == "Add Danger Zone")
						{

							var action2 = await Application.Current.MainPage.DisplayActionSheet(
								"",
								"Cancel",
								null,
								"Circular Area",
								"Custom Area"
							);
							if (action2 == "Circular Area")
							{
								AddDZToMapPage aToMapPage = new AddDZToMapPage(this,position);
								await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);
							}

							else if (action2 == "Custom Area"){
								var stackLayout = new StackLayout (){
									VerticalOptions = LayoutOptions.Center
								};
								var text1 = new Label { 
									Text = "Vertex 1",
									FontSize = 16
								};
								var v1distancenorth = new Entry { 
									Placeholder = "Distance North (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var v1distancewest = new Entry { 
									Placeholder = "Distance West (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var text2 = new Label { 
									Text = "Vertex 2",
									FontSize = 16
								};
								var v2distancenorth = new Entry { 
									Placeholder = "Distance North (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var v2distanceeast = new Entry { 
									Placeholder = "Distance East (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var text3 = new Label { 
									Text = "Vertex 3",
									FontSize = 16
								};
								var v3distancesouth = new Entry { 
									Placeholder = "Distance South (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var v3distancewest = new Entry { 
									Placeholder = "Distance West (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var text4 = new Label { 
									Text = "Vertex 4",
									FontSize = 16
								};
								var v4distancesouth = new Entry { 
									Placeholder = "Distance South (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var v4distanceeast = new Entry { 
									Placeholder = "Distance East (miles)",
									Keyboard = Keyboard.Numeric,
									VerticalOptions = LayoutOptions.Center
								};
								var EnterButton = new Button { 
									Text = "Enter",
									VerticalOptions = LayoutOptions.Center
								};
								var CancelButton = new Button { 
									Text = "Cancel",
									VerticalOptions = LayoutOptions.Center
								};
								stackLayout.Children.Add (text1);
								stackLayout.Children.Add (v1distancenorth);
								stackLayout.Children.Add (v1distancewest);
								stackLayout.Children.Add (text2);
								stackLayout.Children.Add (v2distancenorth);
								stackLayout.Children.Add (v2distanceeast);
								stackLayout.Children.Add (text3);
								stackLayout.Children.Add (v3distancesouth);
								stackLayout.Children.Add (v3distancewest);
								stackLayout.Children.Add (text4);
								stackLayout.Children.Add (v4distancesouth);
								stackLayout.Children.Add (v4distanceeast);

								var stackLayout2 = new StackLayout ();
								stackLayout2.HorizontalOptions = LayoutOptions.Center;
								stackLayout2.Orientation = StackOrientation.Horizontal;
								stackLayout2.Children.Add (EnterButton);
								stackLayout2.Children.Add (CancelButton);
								stackLayout.Children.Add (stackLayout2);


								ContentPage radius = new ContentPage(){
									Content = stackLayout
								};
								await App.Current.MainPage.Navigation.PushModalAsync(radius);


								EnterButton.Clicked += (sender, e) => makeCustomArea (position, 
									v1distancenorth, v1distancewest, v2distancenorth, v2distanceeast,
									v3distancesouth, v3distancewest, v4distancesouth, v4distanceeast);
								
								CancelButton.Clicked += (sender, e) => goBack ();
							}

						}
					});
			}
		}
			


		public void makeCustomArea(Position position, Entry v1distancenorth, 
			Entry v1distancewest, Entry v2distancenorth, Entry v2distanceeast,
			Entry v3distancesouth, Entry v3distancewest, Entry v4distancesouth, 
			Entry v4distanceeast){

			List<Position> polyPoints = new List<Position>();

			// 0.014493 and 0.018315 are used to convert miles to degrees of latitude and longitude
			polyPoints.Add(new Position(position.Latitude+Convert.ToDouble(v2distancenorth.Text)*0.014493,
				position.Longitude+Convert.ToDouble(v2distanceeast.Text)*0.018315)); //top right
			polyPoints.Add(new Position(position.Latitude+Convert.ToDouble(v1distancenorth.Text)*0.014493,
				position.Longitude-Convert.ToDouble(v1distancewest.Text)*0.018315)); //top left
			polyPoints.Add(new Position(position.Latitude-Convert.ToDouble(v3distancesouth.Text)*0.014493,
				position.Longitude-Convert.ToDouble(v3distancewest.Text)*0.018315)); //bottom left
			polyPoints.Add(new Position(position.Latitude-Convert.ToDouble(v4distancesouth.Text)*0.014493,
				position.Longitude+Convert.ToDouble(v4distanceeast.Text)*0.018315)); //bottom right

			var poly = new TKPolygon{
				Coordinates = polyPoints,
				Color = Color.FromRgba(100, 0, 0, 80),
				StrokeWidth = 2f
			};
			this._polygons.Add(poly);
			goBack ();
		}


		public async void goBack(){
			await Application.Current.MainPage.Navigation.PopModalAsync();
		}

		/// <summary>
		/// Map Clicked bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Command<Position> MapClickedCommand
		{
			get
			{
				return new Command<Position>((positon) =>
					{
						this.SelectedPin = null;

						// Determine if a point was inside a circle
						if ((from c in this._circles let distanceInMeters = c.Center.DistanceTo(positon) * 1000 where distanceInMeters <= c.Radius select c).Any())
						{
//							var myObject = circle as EIMACircle;
//
//							if (myObject != null)
//							{
//								Application.Current.MainPage.DisplayAlert("Danger : " + myObject.type, myObject.note, "OK");
//							}

						}
					});
			}
		}
		/// <summary>
		/// Command when a place got selected
		/// </summary>
		public Command<IPlaceResult> PlaceSelectedCommand
		{
			get
			{
				return new Command<IPlaceResult>(async p =>
					{
						var gmsResult = p as GmsPlacePrediction;
						if (gmsResult != null)
						{
							var details = await GmsPlace.Instance.GetDetails(gmsResult.PlaceId);
							this.MapCenter = new Position(details.Item.Geometry.Location.Latitude, details.Item.Geometry.Location.Longitude);
							return;
						}
						var osmResult = p as OsmNominatimResult;
						if (osmResult != null)
						{
							this.MapCenter = new Position(osmResult.Latitude, osmResult.Longitude);
							return;
						}

						if (Device.OS == TargetPlatform.Android)
						{
							var prediction = (TKNativeAndroidPlaceResult)p;

							var details = await TKNativePlacesApi.Instance.GetDetails(prediction.PlaceId);

							this.MapCenter = details.Coordinate;
						}
						else if (Device.OS == TargetPlatform.iOS)
						{
							var prediction = (TKNativeiOSPlaceResult)p;

							this.MapCenter = prediction.Details.Coordinate;
						}
					});
			}
		}
		/// <summary>
		/// Pin Selected bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Command PinSelectedCommand
		{
			get
			{
				return new Command(() =>
					{
						this.MapCenter = this.SelectedPin.Position;
					});
			}
		}
		/// <summary>
		/// Drag End bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Command<TKCustomMapPin> DragEndCommand
		{
			get 
			{
				return new Command<TKCustomMapPin>(pin => 
					{
						var myObject = pin as EIMAPin;

						if (myObject != null)
						{
							saveData();
						}
					});
			}
		}
		/// <summary>
		/// Callout clicked bound to the <see cref="TKCustomMap"/>
		/// </summary>
		public Command CalloutClickedCommand
		{
			get
			{
				return new Command(async (object a) => 
					{
						var action = await Application.Current.MainPage.DisplayActionSheet(
							"Asset Selected",
							"Cancel",
							null,
							"Modify Asset Info",
							"Delete Asset"
						);

						if (action == "Delete Asset")
						{
							var eimaPin = this.SelectedPin as EIMAPin;
							if (eimaPin != null)
							{
								if(eimaPin.IsDraggable){
									this._pins.Remove(eimaPin);
									saveData();
								} else {
									await Application.Current.MainPage.DisplayAlert("Cannot Perform Delete","Asset is a user","OK");
								}
							} 
							else
								this._pins.Remove(this.SelectedPin);
						}
						else if (action == "Modify Asset Info")
						{
							var eimaPin = this.SelectedPin as EIMAPin;
							if (eimaPin != null)
							{		
								if(eimaPin.IsDraggable){
									AddAssetToMapPage editPage = new AddAssetToMapPage(eimaPin,true,this);
									await Application.Current.MainPage.Navigation.PushModalAsync(editPage);
								} else {
									await Application.Current.MainPage.DisplayAlert("Cannot Modify Information","Asset is a user","OK");
								}
							}
						}
					});
			}
		}
		public Command ClearMapCommand
		{
			get
			{
				return new Command(() => 
					{
						this._pins.Clear();
						this._circles.Clear();
						if (this._routes != null)
							this._routes.Clear();
					});
			}
		}

		/// <summary>
		/// Command when a route calculation finished
		/// </summary>
		public Command<TKRoute> RouteCalculationFinishedCommand
		{
			get
			{
				return new Command<TKRoute>(r => 
					{
						// move to the bounds of the route
						this.MapRegion = r.Bounds;
					});
			}
		}

		public MapModel()
		{
			this._pins = new ObservableCollection<TKCustomMapPin>();
			this._circles = new ObservableCollection<TKCircle>();
			this._polygons = new ObservableCollection<TKPolygon>();

			this.invisPoly = new List<TKPolygon> ();
			this.invisCircles = new List<TKCircle> ();
		}

		public void addCircle(EIMACircle circle){
			this._circles.Add (circle);
		}

		public void addPin(EIMAPin pin){
			this._pins.Add(pin);							
		}

		public void removePin(EIMAPin pin){
			this._pins.Remove(pin);							
		}

		public void saveData(){
			DataManager data = new DataManager();
			data.setAssets(eimaPinsList());
			data.setDangerZoneCircle(eimaCircles());
			data.setDangerZonePoly(eimaPolygons());
		}

		public List<EIMAPin> eimaPinsList(){
			List<EIMAPin> toReturn = new List<EIMAPin> ();
			foreach (TKCustomMapPin element in this._pins) {

				var eimaPin = element as EIMAPin;
				if (eimaPin != null)
				{
					toReturn.Add (eimaPin);
				}
			}
			return toReturn;
		}

		public List<EIMACircle> eimaCircles(){
			List<EIMACircle> toReturn = new List<EIMACircle> ();
			foreach (TKCircle element in this._circles) {

				var eimaC = element as EIMACircle;
				if (eimaC != null)
				{
					toReturn.Add (eimaC);
				}
			}
			foreach (TKCircle element in invisCircles) {

				var eimaC = element as EIMACircle;
				if (eimaC != null)
				{
					toReturn.Add (eimaC);
				}
			}
			return toReturn;
		}

		public List<EIMAPolygon> eimaPolygons(){
			List<EIMAPolygon> toReturn = new List<EIMAPolygon> ();
			foreach (TKPolygon element in this._polygons) {

				var eimaPoly = element as EIMAPolygon;
				if (eimaPoly != null)
				{
					toReturn.Add (eimaPoly);
				}
			}
			foreach (TKPolygon element in invisPoly) {

				var eimaPoly = element as EIMAPolygon;
				if (eimaPoly != null)
				{
					toReturn.Add (eimaPoly);
				}
			}
			return toReturn;
		}

		public void filterPins(string pinType,bool setting){
			foreach(TKCustomMapPin element in _pins){
				var eimaPin = element as EIMAPin;
				if (eimaPin != null)
				{
					if (eimaPin.unitType == pinType) {
						eimaPin.IsVisible = setting;
					}
				}
			}
		}
		public void filterDZ(string dzType,bool setting){

			if (setting) {
				foreach (TKPolygon element in invisPoly.ToList()) {
					
					var myObject = element as EIMAPolygon;
					if (myObject != null) {
						if (myObject.type == dzType) {
							_polygons.Add (myObject);
							invisPoly.Remove (myObject);
						}
					}

				}
				foreach (TKCircle element in invisCircles.ToList()) {

					var myObject = element as EIMACircle;
					if (myObject != null) {
						if (myObject.type == dzType) {
							_circles.Add (myObject);
							invisCircles.Remove (myObject);
						}
					}

				}
			} else {
			
				foreach (TKPolygon element in _polygons.ToList()) {

					var myObject = element as EIMAPolygon;
					if (myObject != null)
					{
						if (myObject.type == dzType) {
							invisPoly.Add (myObject);
							_polygons.Remove (myObject);
						}
					}

				}
				foreach (TKCircle element in _circles.ToList()) {

					var myObject = element as EIMACircle;
					if (myObject != null)
					{
						if (myObject.type == dzType) {
							invisCircles.Add (myObject);
							_circles.Remove (myObject);
						}
					}

				}
			
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var ev = this.PropertyChanged;

			if (ev != null)
				ev(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
