using System;

using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;
using TK.CustomMap.MapModel;

namespace EIMA
{
	/**
	 * Totally Changed this from the old sample, it was inflexible and 
	 * couldn't easily fit our needs, I need to make this page look nicer (If anyone wants to work with this feel free)
	 * still however it has the same functionality and it's much simpler code wise.
	 */
	public class FilterPage : ContentPage
	{

		private List<FilterObject> listData;
		private List<CustomSwitch> switchList;
		private StackLayout myLayout;
		private MapModel myModel;

		public FilterPage(List<FilterObject> inputData, MapModel mapModel){
			myModel = mapModel;
			listData = inputData;
			this.Title = "Filter";
			switchList = new List<CustomSwitch>();
			init ();
		}

		public void init(){

			// Build the page.
			myLayout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Spacing = 5,
				Children = {
				}
			};


			foreach (FilterObject element in listData) {

				var declaredSwitch = new CustomSwitch () {
					VerticalOptions = LayoutOptions.Start,
					HorizontalOptions = LayoutOptions.Center,
					name = element.Name
				};
				declaredSwitch.IsToggled = element.IsSelected;
				declaredSwitch.Toggled += toggledSwitch;

				switchList.Add (declaredSwitch);
				var textSwitch = new StackLayout () {
					HorizontalOptions = LayoutOptions.EndAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Horizontal,
					Children = {
						new Label () {
							VerticalOptions = LayoutOptions.Start,
							HorizontalOptions = LayoutOptions.Start,
							Text = element.Name,
							FontSize = 28
						},
						declaredSwitch
					}
				};
				myLayout.Children.Add (textSwitch);
			}

			this.Content = myLayout;


			ToolbarItem allOnTBI = null;
			ToolbarItem allOffTBI = null;

			allOnTBI = new ToolbarItem ("All", "", () => {allOn();}, 0, 0);
			allOffTBI = new ToolbarItem ("None", "", () => {allOff();}, 0, 0);

			//Change map type
			ToolbarItems.Add (allOnTBI);
			ToolbarItems.Add (allOffTBI);
		}

		void toggledSwitch(object sender, ToggledEventArgs e)
		{
//			this.Title
			var switchSent = (CustomSwitch)sender;

			DataManager data = new DataManager ();
			data.setFilter (switchSent.name, e.Value);
			myModel.filterPins (switchSent.name, e.Value);
		}

		public void allOn(){
			DataManager data = new DataManager ();
			foreach (CustomSwitch element in switchList) {
				element.IsToggled = true;
				data.setFilter (element.name, true);
				myModel.filterPins (element.name, true);
			}
		}
		public void allOff(){
			DataManager data = new DataManager ();
			foreach (CustomSwitch element in switchList) {
				element.IsToggled = false;
				data.setFilter (element.name, false);
				myModel.filterPins (element.name, false);
			}
		}
	}
	//Util Classes to extend switch, and hold data
	public class FilterObject
	{
		public string Name { get; set ; }
		public bool IsSelected { get; set ; }
		public FilterObject ()
		{
		}
	}

	public class CustomSwitch : Switch
	{
		public string name;
		public CustomSwitch ()
		{
		}
	}

}

