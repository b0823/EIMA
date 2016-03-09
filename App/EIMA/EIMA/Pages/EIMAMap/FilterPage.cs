using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{
	
	public class FilterPage : ContentPage
	{

		List<FilterObject> dzFilterData;
		List<FilterObject> assetFilterData;
		readonly List<SwitchCell> switchList;
		readonly List<SwitchCell> dzSwitchList;
		MapModel myModel;

		public FilterPage(List<FilterObject> inAssetFilterData, List<FilterObject> inDZFilterData, MapModel mapModel){
			myModel = mapModel;
			assetFilterData = inAssetFilterData;
			dzFilterData = inDZFilterData;
			Title = "Filter";

			switchList = new List<SwitchCell>();
			dzSwitchList = new List<SwitchCell>();

			buildUI ();
		}
		//ASSET SWITCHED
		void switchedCellAsset(object sender, ToggledEventArgs e){
			var switchSent = (SwitchCell)sender;

			DataManager data = new DataManager ();
			data.setFilter (switchSent.Text, e.Value);
			myModel.filterPins (switchSent.Text, e.Value);	
		}
		//ASSET TAP
		void tappedCellAsset (object sender, System.EventArgs ea)
		{
			var switchSent = (SwitchCell)sender;
			switchSent.On = !switchSent.On;
		}

		//DZ SWITCHED
		void switchedCellDZ(object sender, ToggledEventArgs e){
			var switchSent = (SwitchCell)sender;

			DataManager data = new DataManager ();
			data.setDZFilter (switchSent.Text, e.Value);
			myModel.filterDZ (switchSent.Text, e.Value);	
		}
		//DANGER ZONE TAP
		void tappedCellDZ (object sender, System.EventArgs ea)
		{
			var switchSent = (SwitchCell)sender;
			switchSent.On = !switchSent.On;

		}
		public void buildUI(){
			var section = new TableSection ("Assets");
			foreach(FilterObject element in assetFilterData){
				var cell = new SwitchCell {
					Text = element.Name, 
					On = element.IsSelected,
				};
				cell.OnChanged += switchedCellAsset;
				cell.Tapped += tappedCellAsset;

				section.Add (cell);
				switchList.Add (cell);
			}

			var dzSection = new TableSection ("Danger Zones");
			foreach(FilterObject element in dzFilterData){
				var cell = new SwitchCell {
					Text = element.Name, 
					On = element.IsSelected,
				};
				cell.OnChanged += switchedCellDZ;
				cell.Tapped += tappedCellDZ;

				dzSection.Add (cell);
				dzSwitchList.Add (cell);
			}

			TableRoot root = new TableRoot ();
			root.Add (section);
			root.Add (dzSection);
			TableView tableView = new TableView (root);

			Content = tableView;

			ToolbarItem allOnTBI;
			ToolbarItem allOffTBI;

			allOnTBI = new ToolbarItem ("All", "", allOn, 0, 0);
			allOffTBI = new ToolbarItem ("None", "", allOff, 0, 0);

			//Change map type
			ToolbarItems.Add (allOnTBI);
			ToolbarItems.Add (allOffTBI);
		}
			
		public void allOn(){
			foreach (SwitchCell element in switchList) {
				element.On = true;
			}
			foreach (SwitchCell element in dzSwitchList) {
				element.On = true;
			}
		}
		public void allOff(){
			foreach (SwitchCell element in switchList) {
				element.On = false;
			}
			foreach (SwitchCell element in dzSwitchList) {
				element.On = false;
			}
		}
	}

	//Util Classes to extend switch, and hold data
	public class FilterObject
	{
		public string Name { get; set ; }
		public bool IsSelected { get; set ; }
	}
}

