using System;
using Xamarin.Forms;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EIMAMaster
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
			//TODO IDK WHAT WE'RE SETTING

			String currentSpeed = "30 Seconds";

			settings = new Label
			{
				Text = "Settings",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			Label upSpeed = new Label
			{
				Text = "Update Speed",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
			};

			picker = new Picker
			{
				Title = currentSpeed,
				VerticalOptions = LayoutOptions.Center
			};

			foreach (string datamember in updateData) {
				picker.Items.Add (datamember);
			}
				
			//TODO only on standlone, this will have handler that checks network and creates incident
			Button network = new Button
			{
				Text = "Make Incident Networked",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					settings,
					upSpeed,
					picker,
					network
				}
			};

			Title = "Settings";
			Icon = "SettingsIcon.png";
			setSettings ();
			//restCall (); This is a sample REST Call for Mark to test/look at
		}
		public void setSettings(){
			string speed = new DataManager ().getUpdateSpeed ();
			var index = Array.FindIndex(updateData, row => row == speed);
			picker.SelectedIndex = index;
		}
		public void restCall (){
			Uri url =  new Uri("https://eima-server.herokuapp.com/services/time/");
			var syncClient = new WebClient ();

			syncClient.DownloadStringCompleted += (sender, e) => 
			{
				settings.Text = e.Result;
			};

			syncClient.DownloadStringAsync(url);
		}
	}
	
}