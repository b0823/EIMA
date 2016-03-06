using System;
using Xamarin.Forms;

namespace EIMA
{

	public class SettingsPage : ContentPage
	{
		readonly Label settings;
		string[] updateData = {
			"30 Seconds", "1 Minute", "5 Minutes", "10 Minutes", "30 Minutes", "1 Hour"
		};
		Picker picker;

		public SettingsPage ()
		{
			Title = "Settings";
			Icon = "SettingsIcon.png";

			settings = new Label
			{
				Text = "Settings",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var data = new DataManager ();
			if (data.isStandAlone ()) {
				standAloneUI ();
			} else
				networkedUI ();

		}
		public void setSettings(){
			string speed = new DataManager ().getUpdateSpeed ();
			var index = Array.FindIndex(updateData, row => row == speed);
			picker.SelectedIndex = index;
		}

		public void networkedUI(){
			var upSpeed = new Label
			{
				Text = "Update Speed",
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
			};

			picker = new Picker
			{
				VerticalOptions = LayoutOptions.Center
			};
			picker.SelectedIndexChanged += (sender, args) =>
			new DataManager ().setUpdateSpeed (updateData [picker.SelectedIndex]);

			foreach (string datamember in updateData) {
				picker.Items.Add (datamember);
			}
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (settings);
			stackLayout.Children.Add (upSpeed);
			stackLayout.Children.Add (picker);
			Content = stackLayout;
			setSettings ();

		}

		public async void deleteData(){
			var action = await DisplayActionSheet(
				"Are you sure you want to delete existing data?",
				"",
				null,
				"Yes",
				"Cancel"
			);

			if(action == "Yes"){
				DataManager data = new DataManager();
				data.resetStandAlone();
			}
		}

		public void standAloneUI(){

			var network = new Button
			{
				Text = "Make Incident Networked",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			var reset = new Button
			{
				Text = "Clear Incident Data",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			reset.Clicked += (sender, e) => deleteData ();

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (settings);
			stackLayout.Children.Add (network);
			stackLayout.Children.Add (reset);
			Content = stackLayout;
		}
	}
	
}