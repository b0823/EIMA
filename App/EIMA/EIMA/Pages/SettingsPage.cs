using System;
using Xamarin.Forms;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EIMA
{

	public class SettingsPage : ContentPage
	{
		Label settings;
		string[] updateData = new string[6]{
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
				FontSize = 50,
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
			Label upSpeed = new Label
			{
				Text = "Update Speed",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
			};

			picker = new Picker
			{
				VerticalOptions = LayoutOptions.Center
			};
			picker.SelectedIndexChanged += (sender, args) =>
			{
				new DataManager().setUpdateSpeed(updateData[picker.SelectedIndex]);
			};

			foreach (string datamember in updateData) {
				picker.Items.Add (datamember);
			}
			this.Content = new StackLayout
			{
				Children = 
				{
					settings,
					upSpeed,
					picker
				}
			};
			setSettings ();

		}

		public void standAloneUI(){

			Button network = new Button
			{
				Text = "Make Incident Networked",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			Button reset = new Button
			{
				Text = "Clear Incident Data",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			reset.Clicked += (sender, e) => {
				DataManager data = new DataManager();
				data.resetStandAlone();
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					settings,
					network,
					reset
				}
			};
		}
	}
	
}