using System;
using Xamarin.Forms;
using TK.CustomMap;
using TK.CustomMap.MapModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EIMA
{
	public class AddToMapPage : ContentPage
	{
		
		public bool hasCanceled { get; set; }
		private bool editAsset; //true when editing, false when adding

		private string[] unitTypes = {"Fire","Police", "Hazmat","EMS","Triage","Rescue","Command Post","Other"};

		private EIMAPin toEdit;
		private ObservableCollection<TKCustomMapPin> pinList;
		private MapModel myModel;

		public AddToMapPage (EIMAPin pin, bool editPin, MapModel model)
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
				toEdit.unique = randomString (16);
				toEdit.IsDraggable = true;
			}

			toEdit.Title = toEdit.name + " (" + toEdit.organization + "," + toEdit.unit + ")";
			toEdit.Subtitle = "Status:" + toEdit.status;
		

			toEdit.Image = toEdit.unitType.Replace(" ","") + ".png" ; //only works with police atm

			myModel.addPin (toEdit);
//			if (!this.editAsset) {
//				pinList.Remove (toEdit);
//				myModel.addPin (toEdit);
//			}


			goBack();
		}

		/*
		 * When cancel or back is 		hit. 
		 */
		public void onCancel(){
			if(!this.editAsset)
				pinList.Remove (toEdit);
			goBack ();
		}
		/**
		 * Builds interface for data, sets calls for addPin and onCancel. 
		 */
		public void buildUI(){
			Label header = new Label
			{
				Text = "Specify Asset",
				FontSize = 50,
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

			Picker unitPicker = new Picker
			{
				Title = "Unit Type",
				VerticalOptions = LayoutOptions.Center
			};


			foreach (string s in unitTypes) {
				unitPicker.Items.Add (s);
			}

			Button addToMap = new Button
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				Text = "Add To Map",
				BorderWidth = 1
			};
			if (this.editAsset) {
				addToMap.Text = "Confirm Edit";
			}

			Button cancel = new Button
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				Text = "Cancel",
				BorderWidth = 1
			};

			cancel.Clicked += (sender, e) => {
				onCancel();
			};

			addToMap.Clicked += (sender, e) => {
				
				if(unitNameEntry.Text == null || unitNameEntry.Text == ("") || unitNameEntry.Text == ("Unit Name")){
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

			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					unitNameEntry,
					unitPicker,
					unitEntry,
					orgEntry,
					statusEntry,
					new StackLayout()
					{
						HorizontalOptions = LayoutOptions.Center,
						Orientation = StackOrientation.Horizontal,
						Children = {
							addToMap,
							cancel
						}
					}
				}
			};
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

