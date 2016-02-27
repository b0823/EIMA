using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System;
using System.Linq;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using TK.CustomMap.Overlays;
using TK.CustomMap.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using EIMA;

namespace TK.CustomMap.MapModel
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
							"Add Asset",
							"Cancel",
							null,
							"Create Asset",
							"Add Circle"
						);
						
						if (action == "Create Asset")
						{
							
							var pin = new EIMAPin
							{
								Position = position,
							};						

							var aToMapPage = new AddToMapPage(pin,this._pins);
							await Application.Current.MainPage.Navigation.PushModalAsync(aToMapPage);

							this._pins.Add(pin);							

						}
						else if(action == "Add Circle")
						{
							var circle = new TKCircle 
							{
								Center = position,
								Radius = 10000, 
								//It's meters, tested w/ https://www.freemaptools.com/radius-around-point.htm
								//If we're adding options to a menu before probably use miles and you'll have to convert.
								Color = Color.FromRgba(100, 0, 0, 80)
							};
							this._circles.Add(circle);
						}

					});
			}
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
							Application.Current.MainPage.DisplayAlert("Circle tap", "Circle was tapped", "OK");
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
							DataManager data = new DataManager();
							data.setAssets(eimaPinsList());
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
				return new Command(async () => 
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
							this._pins.Remove(this.SelectedPin);
							DataManager data = new DataManager();
							data.setAssets(eimaPinsList());
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
			 
		}

		public void addPin(EIMAPin pin){
			this._pins.Add(pin);							
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

		public static string randomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
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
