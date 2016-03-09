using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using TK.CustomMap.Overlays;

namespace EIMA
{
	public class AddDZToMapPage : ContentPage
	{
		MapModel myModel;

		public AddDZToMapPage (MapModel model, Position position) 
		{
			myModel = model;

			var header = new Label
			{
				Text = "Specify DangerZone",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};

			var text = new Label { 
				Text = "Enter radius of the area (in miles)",
				FontSize = 20
			};
			var radiusInMiles = new Entry { 
				Placeholder = "Radius",
				Keyboard = Keyboard.Numeric,
				VerticalOptions = LayoutOptions.Center
			};

			var info = new Entry { 
				Placeholder = "Info",
				VerticalOptions = LayoutOptions.Center
			};

			var typePicker = new Picker
			{
				Title = "Danger Zone Type",
				VerticalOptions = LayoutOptions.Center
			};


			foreach (string s in CONSTANTS.dzTypeOptions) {
				typePicker.Items.Add (s);
			}

			var EnterButton = new Button { 
				Text = "Enter",
				VerticalOptions = LayoutOptions.Center
			};
			var CancelButton = new Button { 
				Text = "Cancel",
				VerticalOptions = LayoutOptions.Center
			};
			var stackLayout = new StackLayout (){
				VerticalOptions = LayoutOptions.Center
			};
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (text);
			stackLayout.Children.Add (radiusInMiles);
			stackLayout.Children.Add (typePicker);
			stackLayout.Children.Add (info);
			var stackLayout2 = new StackLayout ();
			stackLayout2.HorizontalOptions = LayoutOptions.Center;
			stackLayout2.Orientation = StackOrientation.Horizontal;
			stackLayout2.Children.Add (EnterButton);
			stackLayout2.Children.Add (CancelButton);
			stackLayout.Children.Add (stackLayout2);
			Content = stackLayout;

			EnterButton.Clicked += (sender, e) => {
				if (string.IsNullOrEmpty (radiusInMiles.Text)) {
					DisplayAlert ("Must Enter a Radius", "", "Cancel");
					return;
				}
				if(typePicker.SelectedIndex < 0){
					DisplayAlert("Must Select a Danger Zone Type","","Cancel");
					return;
				}
				makeCircularArea (position, radiusInMiles.Text, info.Text, CONSTANTS.dzTypeOptions[typePicker.SelectedIndex], CONSTANTS.colorOptions[typePicker.SelectedIndex]);
			};
			CancelButton.Clicked += (sender, e) => goBack ();
		}

		public void makeCircularArea(Position position, string radiusInMiles, string info, string type, Color typeColor){



			var circle = new EIMACircle 
			{
				Center = position,
				note = info,
				type = type,
				Radius = Convert.ToDouble(radiusInMiles) * 1609.344, // Convert miles to meters
				Color = typeColor
			};
			myModel.addCircle(circle);
			myModel.saveData ();
			goBack ();
		}

		public async void goBack(){
			await Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}

}

