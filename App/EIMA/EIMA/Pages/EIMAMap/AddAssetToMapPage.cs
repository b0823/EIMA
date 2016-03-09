using System;
using Xamarin.Forms;
using System.Linq;

namespace EIMA
{
	public class AddAssetToMapPage : ContentPage
	{
		
		public bool hasCanceled { get; set; }
		bool editAsset; //true when editing, false when adding

		string[] unitTypes = {"Fire","Police", "Hazmat","EMS","Triage","Rescue","Command Post","Other"};

		EIMAPin toEdit;
		MapModel myModel;

		public AddAssetToMapPage (EIMAPin pin, bool editPin, MapModel model)
		{
			myModel = model;
			editAsset = editPin;
			hasCanceled = true;
			toEdit = pin;
			buildUI ();

		}

		public async void goBack(){
			myModel.saveData ();
			await Navigation.PopModalAsync();
		}

		protected override bool OnBackButtonPressed(){
			onCancel ();
			return true;
		}
		/**
		 * 	Data stored in pin (toEdit) values now. Doesnt make sense to replicate that.
			See https://github.com/TorbenK/TK.CustomMap/wiki/TKCustomMapPin
		 */
		public void addPin(){
			toEdit.ShowCallout = true;
			toEdit.IsVisible = true;
			hasCanceled = false;

			if (!editAsset) {
				toEdit.username = randomString (16);
				toEdit.IsDraggable = true;
			}

			toEdit.Title = toEdit.name + " (" + toEdit.organization + "," + toEdit.unit + ")";
			toEdit.Subtitle = "Status:" + toEdit.status;
		

			toEdit.Image = toEdit.unitType.Replace(" ","") + ".png" ; //only works with police atm

			myModel.addPin (toEdit);

			goBack();
		}

		/*
		 * When cancel or back is 		hit. 
		 */
		public void onCancel(){
			goBack ();
		}
		/**
		 * Builds interface for data, sets calls for addPin and onCancel. 
		 */
		public void buildUI(){
			var header = new Label
			{
				Text = "Specify Asset",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};
			var unitNameEntry = new Entry { 
				Placeholder = "Unit Name",
				VerticalOptions = LayoutOptions.Center
			};
			var unitEntry = new Entry { 
				Placeholder = "Unit #",
				VerticalOptions = LayoutOptions.Center
			};
			var orgEntry = new Entry { 
				Placeholder = "Organization",
				VerticalOptions = LayoutOptions.Center
			};

			//TODO is status like a dropdown? I.E. healthy/fine/good/in need of help/dieing?
			var statusEntry = new Entry { 
				Placeholder = "Current Status",
				VerticalOptions = LayoutOptions.Center
			};

			var unitPicker = new Picker
			{
				Title = "Unit Type",
				VerticalOptions = LayoutOptions.Center
			};


			foreach (string s in unitTypes) {
				unitPicker.Items.Add (s);
			}

			var addToMap = new Button
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				Text = "Add To Map",
				BorderWidth = 1
			};
			if (editAsset) {
				addToMap.Text = "Confirm Edit";
			}

			var cancel = new Button
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				Text = "Cancel",
				BorderWidth = 1
			};

			cancel.Clicked += (sender, e) => onCancel ();

			addToMap.Clicked += (sender, e) => {
				
				if(string.IsNullOrEmpty (unitNameEntry.Text) || unitNameEntry.Text == ("Unit Name")){
					DisplayAlert("You must enter a Unit Name!","","OK");
					return;
				}
				if(unitPicker.SelectedIndex < 0){
					DisplayAlert("You must select a Unit Type!","","OK");
					return;
				}

				toEdit.name = unitNameEntry.Text;
				toEdit.unitType = unitTypes[unitPicker.SelectedIndex];
				toEdit.organization = orgEntry.Text;
				toEdit.unit = unitEntry.Text;
				toEdit.status = statusEntry.Text;

				myModel.removePin(toEdit);
				addPin();
			};

			if (editAsset) {
				unitNameEntry.Text = toEdit.name;
				unitPicker.SelectedIndex = Array.IndexOf (unitTypes, toEdit.unitType);

				if(toEdit.organization != null || toEdit.organization != "") orgEntry.Text = toEdit.organization;
				if(toEdit.unit != null || toEdit.unit != "") unitEntry.Text = toEdit.unit;
				if(toEdit.status != null || toEdit.status != "") statusEntry.Text = toEdit.status;
			}

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (unitNameEntry);
			stackLayout.Children.Add (unitPicker);
			stackLayout.Children.Add (unitEntry);
			stackLayout.Children.Add (orgEntry);
			stackLayout.Children.Add (statusEntry);
			var stackLayout2 = new StackLayout ();
			stackLayout2.HorizontalOptions = LayoutOptions.Center;
			stackLayout2.Orientation = StackOrientation.Horizontal;
			stackLayout2.Children.Add (addToMap);
			stackLayout2.Children.Add (cancel);
			stackLayout.Children.Add (stackLayout2);
			Content = stackLayout;
		}
		public static string randomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}	
	}
}

