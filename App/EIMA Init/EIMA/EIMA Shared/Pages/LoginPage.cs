using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EIMAMaster
{
	public class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			Label header = new Label
			{
				Text = "EIMA",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			Label subheader = new Label
			{
				Text = "Emergency Incident Management Application",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};
			var usernameEntry = new Entry {Placeholder = "Username"};

			var passwordEntry = new Entry {
				Placeholder = "Password",
				IsPassword = true
			};

			Button loginButton = new Button
			{
				Text = "Login",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
			};
			loginButton.IsEnabled = false;

			Button standAloneButton = new Button
			{
				Text = "Standalone",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			loginButton.Clicked += OnLoginButtonClicked;
			//standAloneButton.Clicked += OnStandButtonClicked;

			standAloneButton.Clicked += async (sender, e) => {
				await Navigation.PushModalAsync (new RootPage ());
			};


			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

			// Build the page.
			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					subheader,
					usernameEntry,
					passwordEntry,
					loginButton,
					standAloneButton
				}
			};
		}
		void OnLoginButtonClicked(object sender, EventArgs e)
		{
			//TODO
			//if is connection exist
				//query for correct username password

		}
	}
}
	