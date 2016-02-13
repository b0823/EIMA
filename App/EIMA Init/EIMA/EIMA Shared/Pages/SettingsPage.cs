using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMAMaster
{

	public class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			//TODO IDK WHAT WE'RE SETTING

			String currentSpeed = "30 Seconds";

			Label settings = new Label
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

			Picker picker = new Picker
			{
				Title = currentSpeed,
				VerticalOptions = LayoutOptions.Center
			};

			picker.Items.Add ("30 Seconds");
			picker.Items.Add ("1 Minute");
			picker.Items.Add ("5 Minutes");
			picker.Items.Add ("10 Minute");
			picker.Items.Add ("30 Minutes");
			picker.Items.Add ("1 Hour");

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
		}
	}
	
}