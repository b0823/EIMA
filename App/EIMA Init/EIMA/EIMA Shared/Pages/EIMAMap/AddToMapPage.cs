using System;
using Xamarin.Forms;
using TK.CustomMap;
using TK.CustomMap.MapModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace EIMAMaster
{
	public class AddToMapPage : ContentPage
	{
		public bool hasCanceled { get; set; }
		public string unitNum { get; set; }
		public string organziation { get; set; }
		public string currentStatus { get; set; }
		public string unitType { get; set; }

		private TKCustomMapPin toEdit;
		private ObservableCollection<TKCustomMapPin> pinList;

		public async void goBack(){
			await Navigation.PopModalAsync();
		}

		protected override bool OnBackButtonPressed(){
			onCancel ();
			return true;
		}
		public AddToMapPage (TKCustomMapPin pin, ObservableCollection<TKCustomMapPin> _pins)
		{
			hasCanceled = true;
			toEdit = pin;
			pinList = _pins;

			/**
			 * I'm just putting this here as a placeholder until we know exactly what we want.
			 * 
			 * Basicaly we take this information save it, then ask the user to select a locaiton, 
			 * AND create the item at that location
			 * 
			 * OR.
			 * 
			 * We have them longPress(See MapModel) a location then have displayWindow Open with cancel or 
			 * add to Map,i f they add to Map this window opens, we fill in data and then add the
			 * item to the map at the known location
			 * 
			 * 
			 * The second approach seems better but I'm still contemplating. 
			 * I'm rambling now idk..
			 * 
			 * Update : I implemented the second path as the first seems counterintuitive for usage and 
			 * telling the user to do that. I pass the pin and observable pin list in and edit here.
			 */

			buildUI ();
					
		}

		/**
		 * Call for when add asset is hit. Check data define the pin (toEdit) and do input checking 
		 * on data that is entered, which is stored in fields :
		 * Data will be set to
				unitType 
				organziation 
				unitNum 
				currentStatus
		 */
		public void addPin(){
			hasCanceled = false;
			toEdit.Title = "" + currentStatus + " " + organziation;
			goBack();
		}

		/*
		 * When cancel or back is hit. 
		 */
		public void onCancel(){
			pinList.Remove (toEdit);
			goBack ();
		}

		/**
		 * Builds interface for data, sets calls for addPin and onCancel. 
		 */
		public void buildUI(){
			Title = "Add Asset";
			Label header = new Label
			{
				Text = "Specify Pin",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
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

			unitPicker.Items.Add ("Police");
			unitPicker.Items.Add ("Fire");
			unitPicker.Items.Add ("EMS");
			unitPicker.Items.Add ("Volcano Response");
			unitPicker.Items.Add ("Hazmat");
			unitPicker.Items.Add ("Other");

			Button addToMap = new Button
			{
				Text = "Add To Map",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};


			Button cancel = new Button
			{
				Text = "Cancel",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};

			cancel.Clicked += (sender, e) => {
				onCancel();
			};

			addToMap.Clicked += (sender, e) => {
				unitType = unitPicker.SelectedIndex + " ";
				organziation = orgEntry.Text;
				unitNum = unitEntry.Text;
				currentStatus = statusEntry.Text;
				addPin();
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					unitEntry,
					orgEntry,
					statusEntry,
					unitPicker,
					addToMap,
					cancel
				}
			};
		}
			
	}
}

