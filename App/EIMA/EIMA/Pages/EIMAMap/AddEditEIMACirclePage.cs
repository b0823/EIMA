using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using TK.CustomMap.Overlays;

namespace EIMA
{
	public class AddEditEIMACirclePage : ContentPage
	{
		MapModel myModel;
		Position position;
		EIMACircle toEdit;

		public AddEditEIMACirclePage (MapModel model, Position pos, bool editMode, EIMACircle edit) 
		{
			toEdit = edit;
			position = pos;
			myModel = model;
			createCircle (editMode);
		}

		public void createCircle(bool editMode){
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

			if (editMode) {				
				header.Text = "Edit DangerZone";
				EnterButton.Text = "Confirm Edit";
				info.Text = toEdit.note;
				radiusInMiles.Text = Convert.ToString(metersToMiles (toEdit.Radius));
				typePicker.SelectedIndex = Array.IndexOf (CONSTANTS.dzTypeOptions, toEdit.type);
			}


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
				if(!editMode){
					makeCircularArea (position, radiusInMiles.Text, info.Text, CONSTANTS.dzTypeOptions[typePicker.SelectedIndex], CONSTANTS.colorOptions[typePicker.SelectedIndex]);
				} else{
					toEdit.type = CONSTANTS.dzTypeOptions[typePicker.SelectedIndex];
					toEdit.note = info.Text;
					toEdit.Radius = milesToMeters(Convert.ToDouble(radiusInMiles.Text));
					toEdit.Color = CONSTANTS.colorOptions[typePicker.SelectedIndex];
					myModel.saveData ();
					goBack ();
				}
			};

			CancelButton.Clicked += (sender, e) => goBack ();
		}


		public void makeCircularArea(Position position, string radiusInMiles, string info, string type, Color typeColor){
			var circle = new EIMACircle 
			{
				Center = position,
				note = info,
				type = type,
				Radius = milesToMeters(Convert.ToDouble(radiusInMiles)), // Convert miles to meters
				Color = typeColor
			};
			myModel.addCircle(circle);
			myModel.saveData ();
			goBack ();
		}

		public double metersToMiles(double meters){
			return Math.Round(meters * 0.00062137,2);
		}

		public double milesToMeters(double miles){
			return (miles * 1609.344);
		}

		public async void goBack(){
			await Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}

}

