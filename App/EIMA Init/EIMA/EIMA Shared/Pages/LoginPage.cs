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

			Button standAloneButton = new Button
			{
				Text = "Standalone",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			loginButton.Clicked += OnLoginButtonClicked;

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
		async void OnLoginButtonClicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Warning", "No Username/Password Auth done yet", "Acknowledge");
			await Navigation.PushModalAsync (new IncidentPage ());
			//This would contain validiation of account.
		}
	}
}
	