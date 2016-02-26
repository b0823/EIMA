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

		private string unitNum { get; set; }
		private string unitName { get; set; }
		private string organziation { get; set; }
		private string currentStatus { get; set; }
		private string unitType { get; set; }

		private string[] unitTypes = {"Fire","Police", "Biohazard","EMS","Triage","Rescue","Command Post","Other"};

		private EIMAPin toEdit;
		private ObservableCollection<TKCustomMapPin> pinList;

		public async void goBack(){
			await Navigation.PopModalAsync();
		}

		protected override bool OnBackButtonPressed(){
			onCancel ();
			return true;
		}
		public AddToMapPage (EIMAPin pin, ObservableCollection<TKCustomMapPin> _pins)
		{
			hasCanceled = true;
			toEdit = pin;
			pinList = _pins;
			buildUI ();
								
		}

		/**
		 * Call for when add asset is hit. Check data define the pin (toEdit) and do input checking 
		 * on data that is entered, which is stored in fields :
		 * Data will be set to
				unitName
				unitType 
				organziation 
				unitNum 
				currentStatus
			See https://github.com/TorbenK/TK.CustomMap/wiki/TKCustomMapPin
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
				unitName = unitNameEntry.Text;
				unitType = unitTypes[unitPicker.SelectedIndex];
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
					unitNameEntry,
					unitEntry,
					orgEntry,
					statusEntry,
					unitPicker,
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
			
	}
}

