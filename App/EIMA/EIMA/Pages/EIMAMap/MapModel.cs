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

		//Used in creation of polygons.
		private List<Position> customAreaPolyPoints;
		private List<TKCustomMapPin> customAreaPolyPins;
		public bool creatingPolygon = false;
		public string polyType;
		public string polyNote;

		//Used moving circles
		private bool movingCircle = false;
		private EIMACircle circleToMove;

		//Used moving Polygons
		private bool movingPoly = false;
		private EIMAPolygon polyToMove;
		private List<TKCustomMapPin> polyPinList;

		//Backup list as polygons/circles have no visible field
		private List<TKCircle> invisCircles;
		private List<TKPolygon> invisPoly;

		public TKTileUrlOptions TilesUrlOptions
		{
			get 
			{
				return this._tileUrlOptions;
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
						if(DataManager.getInstance().isUser() && DataManager.getInstance().isNoAccess())
							return;
						var action = await Application.Current.MainPage.DisplayActionSheet(
							"",
							"Cancel",
							null,
							"Create Asset",
							"Create Danger Zone"
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




						else if(action == "Create Danger Zone")
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
								AddEditEIMACirclePage aToMapPage = new AddEditEIMACirclePage(this,position,false,null);
								await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);
							}

							else if (action2 == "Custom Area"){
								
								AddEditEIMAPolygonPage aToMapPage = new AddEditEIMAPolygonPage(this,false,null);
								await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);
							}
						}
					});
			}
		}

		public async void startProcedural(){
			var tapMapPos = await Application.Current.MainPage.DisplayActionSheet(
				"Tap the first three points of shape",
				null,
				null,
				"Start",
				"Cancel"
			);
			
			if(tapMapPos == "Cancel"){
				return;
			} else {
				customAreaPolyPoints = new List<Position>();
				customAreaPolyPins = new List<TKCustomMapPin>();
				return;
			}
		}

		public async void proceduralPolygonCall(Position pos){
			TKCustomMapPin addedPin = new TKCustomMapPin ();
			addedPin.IsDraggable = false;
			addedPin.IsVisible = true;
			addedPin.Image = "dot.png";
			addedPin.Position = pos;
			addedPin.ShowCallout = false;

			_pins.Add (addedPin);
			customAreaPolyPins.Add (addedPin);
			customAreaPolyPoints.Add (pos);

			if (customAreaPolyPoints.Count < 3) {
				return;
			} else {
				var action = await Application.Current.MainPage.DisplayActionSheet(							
					"Point Added",
					null,
					null,
					"Create Custom Area",
					"Add Another Point",
					"Delete Area");
				if (action == "Create Custom Area") {
					EIMAPolygon result = new EIMAPolygon ();
					result.Color = CONSTANTS.colorOptions[Array.IndexOf(CONSTANTS.dzTypeOptions, polyType)];

					result.Coordinates = customAreaPolyPoints;
					result.uid = CONSTANTS.generateUID();

					result.type = polyType;
					result.note = polyNote;
					_polygons.Add (result);

					creatingPolygon = false;
					customAreaPolyPoints = null;
					foreach (TKCustomMapPin element in customAreaPolyPins) {
						_pins.Remove (element);
					}
					customAreaPolyPins = null;
					saveData ();

				} else if (action == "Add Another Point") {
					return;
				} else if (action == "Delete Area") {
					creatingPolygon = false;
					customAreaPolyPoints = null;
					foreach (TKCustomMapPin element in customAreaPolyPins) {
						_pins.Remove (element);
					}
					customAreaPolyPins = null;
				}
			}
		}

		//I have absoloutely ZERO idea how this works, but it does work. It involves geometry Math. 
		//http://stackoverflow.com/questions/4287780/detecting-whether-a-gps-coordinate-falls-within-a-polygon-on-a-map 
		//First answer. was converted to C# from yava

		static bool coordinate_is_inside_polygon(
			double latitude, double longitude, 
			List<Double> lat_array, List<Double> long_array)
		{       
			int i;
			double angle=0;
			double point1_lat;
			double point1_long;
			double point2_lat;
			double point2_long;
			int n = lat_array.Count;

			for (i=0;i<n;i++) {
				point1_lat = lat_array[i] - latitude;
				point1_long = long_array[i] - longitude;
				point2_lat = lat_array[(i+1)%n] - latitude; 
				//you should have paid more attention in high school geometry.
				point2_long = long_array[(i+1)%n] - longitude;
				angle += Angle2D(point1_lat,point1_long,point2_lat,point2_long);
			}

			if (Math.Abs(angle) < Math.PI)
				return false;
			else
				return true;
		}

		//util function for point in polygon.
		static double Angle2D(double y1, double x1, double y2, double x2)
		{
			double dtheta,theta1,theta2;

			theta1 = Math.Atan2(y1,x1);
			theta2 = Math.Atan2(y2,x2);
			dtheta = theta2 - theta1;
			while (dtheta > Math.PI)
				dtheta -= (2*Math.PI);
			while (dtheta < -Math.PI)
				dtheta += (2*Math.PI);

			return(dtheta);
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
				
				return new Command<Position>(async (positon) =>
					{
						if(movingCircle){
							circleToMove.Center = positon;
							circleToMove = null;
							movingCircle = false;
							saveData();
							return;
						}
						if(creatingPolygon){
							proceduralPolygonCall(positon);
							return;
						}
						this.SelectedPin = null;

						// Determine if a point was inside a circle

						foreach(TKCircle element in _circles){
							if(element.Center.DistanceTo(positon) * 1000 <= element.Radius){
								var myObject = element as EIMACircle;
	
								if (myObject != null)
								{
									var action = await Application.Current.MainPage.DisplayActionSheet(							
										myObject.type + " (" + myObject.note + ")",
										null,
										null,
										"OK",
										"Edit Danger Zone Size or Info",
										"Move Center Of Danger Zone",
										"Delete Danger Zone");
									if(action == "OK"){
										return;
									} else if(action == "Move Center Of Danger Zone"){
										var action2 = await Application.Current.MainPage.DisplayActionSheet(
											"Tap the new location.",
											"Cancel",
											null,
											"OK"
										);
										if(action2 == "OK"){
											circleToMove = myObject;
											movingCircle = true;
											return;
										} else { 
											return;
										}
									} else if(action == "Delete Danger Zone"){
										_circles.Remove(element);
										saveData();
										return;
									} else if(action == "Edit Danger Zone Size or Info"){
										AddEditEIMACirclePage aToMapPage = new AddEditEIMACirclePage(this,new Position(-1,-1),true,myObject);
										await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);
										saveData();
										return;
									}
									else {
										return;
									}
								}	
							}
						}
						foreach(TKPolygon element in _polygons.ToList()){

							var myObject = element as EIMAPolygon;

							if (myObject != null)
							{
								var latList = new List<Double>();
								var longList = new List<Double>();

								foreach(Position point in element.Coordinates){
									latList.Add(point.Latitude);
									longList.Add(point.Longitude);
								}

								if(coordinate_is_inside_polygon(positon.Latitude, positon.Longitude, latList, longList)){
									var action = await Application.Current.MainPage.DisplayActionSheet(							
										myObject.type + " (" + myObject.note + ")",
										null,
										null,
										"OK",
										"Edit Danger Zone Info",
										"Modify Points",
										"Delete Danger Zone"
									);
									if(action == "OK"){
										return;
									} else if(action == "Modify Points"){
										movingPoly = true;
										polyToMove = myObject;
										polyPinList = new List<TKCustomMapPin>();

										foreach(Position pos in myObject.Coordinates.ToList()){
											TKCustomMapPin addedPin = new TKCustomMapPin ();
											addedPin.IsDraggable = true;
											addedPin.IsVisible = true;
											addedPin.Image = "dot.png";
											addedPin.Position = pos;
											addedPin.ShowCallout = false;
											this._pins.Add(addedPin);
											polyPinList.Add(addedPin);
										}

										_polygons.Remove(myObject);

									} else if(action == "Edit Danger Zone Info"){
										AddEditEIMAPolygonPage aToMapPage = new AddEditEIMAPolygonPage(this,true,myObject);
										await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);
									} else if(action == "Delete Danger Zone"){
										_polygons.Remove(element);
										saveData();
										return;
									} else {
										return;
									}
								}
							}
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
				return new Command<TKCustomMapPin>(async pin => 
					{
						var myObject = pin as EIMAPin;

						if (myObject != null)
						{
							saveData();
						} else if(movingPoly && polyPinList.Contains(pin)){
							var answer = await Application.Current.MainPage.DisplayAlert (
								"Contiue Editing?", "", 
								"No, Confirm Edits.",
								"Yes"
							);
							if(!answer){
								return;
							} else if (answer){

								var lst = new List<Position>();

								foreach(TKCustomMapPin polyPin in polyPinList.ToList()){
									lst.Add(polyPin.Position);
									_pins.Remove(polyPin);
								}

								polyToMove.Coordinates = lst;

								_polygons.Add (polyToMove);
								polyToMove = null;
								polyPinList = null;
								movingPoly = false;
								saveData();
							}
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
						if(DataManager.getInstance().isUser() && DataManager.getInstance().isNoAccess())
							return;
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

		public void addPoly(EIMAPolygon mypoly){
			this._polygons.Add (mypoly);
		}

		public void addPin(EIMAPin pin){
			this._pins.Add(pin);							
		}

		public void removePin(EIMAPin pin){
			this._pins.Remove(pin);							
		}

		public void saveData(){
			var data = DataManager.getInstance ();
			data.setAssets(eimaPinsList());
			data.setDangerZoneCircle(eimaCircles());
			data.setDangerZonePoly(eimaPolygons());
			if (DataManager.getInstance ().isNetworked ()) {
				DataNetworkCalls.updateAssets (eimaPinsList (), eimaCircles (), eimaPolygons ());
			}
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
			if (this.polyToMove != null) {//Add the one that's removed for editing if user is trying to break stuff
				toReturn.Add (polyToMove);
			}
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
