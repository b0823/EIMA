using System;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace EIMA
{
	public class LoginPage : ContentPage
	{
		readonly Entry passwordEntry;
		readonly Entry usernameEntry;

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
			usernameEntry = new Entry {
				Placeholder = "Username",
				VerticalOptions = LayoutOptions.Center
			};

			passwordEntry = new Entry {
				Placeholder = "Password",
				IsPassword = true,
				VerticalOptions = LayoutOptions.Center
			};

			var loginButton = new Button
			{
				Text = "Login",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center
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
			
				var incident = DataManager.getInstance().getIncidentID();

				if(String.IsNullOrEmpty(incident)){
					standAloneButton.IsEnabled = false;
					await Navigation.PushModalAsync (new RootPage ());
					standAloneButton.IsEnabled = true;
				} else {
					var res = await DisplayActionSheet(
						"Going into standalone mode will remove you from your current incident, Continue?",
						"",
						"",
						"Continue",
						"No"
					);
					if(res == "Continue"){
						standAloneButton.IsEnabled = false;
						await Navigation.PushModalAsync (new RootPage ());
						standAloneButton.IsEnabled = true;
						//leaveIncident();
					}
				}


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

			if(!String.IsNullOrEmpty(DataManager.getInstance().getSecret())){
				usernameEntry.Text = DataManager.getInstance ().getUsername ();
				passwordEntry.Text = "123456";
				usernameEntry.IsEnabled = false;
				passwordEntry.IsEnabled = false;
				loginButton.Text = "Go To Lobby";
			}

		}

		async void OnLoginButtonClicked(object sender, EventArgs e)
		{

			if (!String.IsNullOrEmpty (DataManager.getInstance ().getSecret ())) {
				await Navigation.PushAsync (new IncidentPage ());
				return;
			}
	

			var postData = new JObject ();
			postData ["username"] = usernameEntry.Text;
			postData ["password"] = passwordEntry.Text;

			var result = RestCall.POST (URLs.LOGIN, postData);


			if (((bool)result ["result"]) == true) {
				DataManager.getInstance ().setSecret ((string) result ["token"]);
				DataManager.getInstance ().setUsername (usernameEntry.Text);

				Navigation.InsertPageBefore(new IncidentPage (), this);
				await Navigation.PopAsync().ConfigureAwait(false);

				return;
			} else {
				await DisplayAlert ("Invalid Username or Password", "", "OK");
			}


			//This would contain validiation of account.
		}
	}
}
	