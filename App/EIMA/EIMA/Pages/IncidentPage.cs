using System;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace EIMA
{
	public class IncidentPage : ContentPage
	{
		public IncidentPage ()
		{			
			var header = new Label
			{
				Text = "Join/Create Incident",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var incId = new Entry { 
				Placeholder = "INCIDENT ID",
				VerticalOptions = LayoutOptions.Center
			};

			var joinInc = new Button
			{
				Text = "Join Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};

			joinInc.Clicked += (sender, e) => DisplayAlert ("Warning", "This is yet to be implemented", "Acknowledge");

			var createIncidentButton = new Button
			{
				Text = "Create Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var logoutButton = new Button
			{
				Text = "Logout",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			logoutButton.Clicked += onLogout;

			createIncidentButton.Clicked += (sender, e) => DisplayAlert ("Warning", "This is yet to be implemented", "Acknowledge");

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (incId);
			stackLayout.Children.Add (joinInc);
			stackLayout.Children.Add (createIncidentButton);
			stackLayout.Children.Add (logoutButton);
			Content = stackLayout;
		}

		async void onLogout(object sender, EventArgs e)
		{
			
			var postData = new JObject ();
			postData ["token"] = DataManager.getInstance().getSecret();

			RestCall.POST (URLs.LOGOUT, postData);

			DataManager.getInstance ().setSecret ("");

			Navigation.InsertPageBefore(new LoginPage (), this);
			await Navigation.PopAsync().ConfigureAwait(false);
		

		}

	}
}

