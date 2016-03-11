using System;
using Xamarin.Forms;

namespace EIMA
{
	public class AddEditEIMAPolygonPage : ContentPage
	{
		public AddEditEIMAPolygonPage (MapModel myModel, bool editMode, EIMAPolygon toEdit) 
		{
			var header = new Label
			{
				Text = "Specify DangerZone",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
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
				typePicker.SelectedIndex = Array.IndexOf (CONSTANTS.dzTypeOptions, toEdit.type);
			}


			stackLayout.Children.Add (header);
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
				if(typePicker.SelectedIndex < 0){
					DisplayAlert("Must Select a Danger Zone Type","","Cancel");
					return;
				}
				if(!editMode){
					//start process of selecting points. would need to set vars
					myModel.creatingPolygon = true;
					myModel.polyNote = info.Text;
					myModel.polyType = CONSTANTS.dzTypeOptions[typePicker.SelectedIndex];
					myModel.startProcedural();
					goBack ();
				} else{
					toEdit.type = CONSTANTS.dzTypeOptions[typePicker.SelectedIndex];
					toEdit.note = info.Text;
					toEdit.Color = CONSTANTS.colorOptions[typePicker.SelectedIndex];
					myModel.saveData ();
					goBack ();
				}
			};

			CancelButton.Clicked += (sender, e) => goBack ();
		}

		public async void goBack(){
			await Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}
}

