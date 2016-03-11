using System;
using Xamarin.Forms;

namespace EIMA
{
	public class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			var header = new Label
			{
				Text = "EIMA",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) + 20,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var subheader = new Label
			{
				Text = "Emergency Incident Management Application",
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};
			var usernameEntry = new Entry {
				Placeholder = "Username",
				VerticalOptions = LayoutOptions.Center
			};

			var passwordEntry = new Entry {
				Placeholder = "Password",
				IsPassword = true,
				VerticalOptions = LayoutOptions.Center
			};

			var loginButton = new Button
			{
				Text = "Login",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				IsEnabled = false
			};

			var standAloneButton = new Button
			{
				Text = "Standalone",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			loginButton.Clicked += OnLoginButtonClicked;

			standAloneButton.Clicked += async (sender, e) => {
				standAloneButton.IsEnabled = false;
				await Navigation.PushModalAsync (new RootPage ());
				standAloneButton.IsEnabled = true;
			};


			Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);

			// Build the page.
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (subheader);
			stackLayout.Children.Add (usernameEntry);
			stackLayout.Children.Add (passwordEntry);
			stackLayout.Children.Add (loginButton);
			stackLayout.Children.Add (standAloneButton);
			Content = stackLayout;
		}
		async void OnLoginButtonClicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Warning", "No Username/Password Auth done yet", "Acknowledge");
			await Navigation.PushModalAsync (new IncidentPage ());
			//This would contain validiation of account.
		}
	}
}
	