using Xamarin.Forms;
using System.Collections.Generic;
using TK.CustomMap.MapModel;

namespace EIMA
{
	
	public class FilterPage : ContentPage
	{

		private List<FilterObject> listData;
		private readonly List<SwitchCell> switchList;
		private MapModel myModel;

		public FilterPage(List<FilterObject> inputData, MapModel mapModel){
			myModel = mapModel;
			listData = inputData;
			this.Title = "Filter";
			switchList = new List<SwitchCell>();
			buildUI ();
		}

		void switchedCell(object sender, ToggledEventArgs e){
			var switchSent = (SwitchCell)sender;

			DataManager data = new DataManager ();
			data.setFilter (switchSent.Text, e.Value);
			myModel.filterPins (switchSent.Text, e.Value);	
		}

		void tappedCell (object sender, System.EventArgs ea)
		{
			var switchSent = (SwitchCell)sender;
			switchSent.On = !switchSent.On;

			DataManager data = new DataManager ();
			data.setFilter (switchSent.Text, switchSent.On);
			myModel.filterPins (switchSent.Text, switchSent.On);
		}

		public void buildUI(){
			var section = new TableSection ("Assets");
			foreach(FilterObject element in listData){
				var cell = new SwitchCell {
					Text = element.Name, 
					On = element.IsSelected,
				};
				cell.OnChanged += switchedCell;
				cell.Tapped += tappedCell;

				section.Add (cell);
				switchList.Add (cell);
			}
			TableRoot root = new TableRoot ();
			root.Add (section);
			TableView tableView = new TableView (root);

			this.Content = tableView;

			ToolbarItem allOnTBI = null;
			ToolbarItem allOffTBI = null;

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
		}
		public void allOff(){
			foreach (SwitchCell element in switchList) {
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

