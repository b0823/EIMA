using System;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace EIMA
{
	public class IncidentPage : ContentPage
	{
		Entry incId;
		Button joinInc;

		public IncidentPage ()
		{			
			var header = new Label
			{
				Text = "Join/Create Incident",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			incId = new Entry { 
				Placeholder = "INCIDENT ID",
				VerticalOptions = LayoutOptions.Center
			};

			joinInc = new Button
			{
				Text = "Join Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};


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


			if (!String.IsNullOrEmpty (DataManager.getInstance ().getIncidentID ())) {
				incId.Text = DataManager.getInstance ().getIncidentID ();
				incId.IsEnabled = false;
			}

			createIncidentButton.Clicked  += onCreate;
			joinInc.Clicked += onJoin;

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (incId);
			stackLayout.Children.Add (joinInc);
			stackLayout.Children.Add (createIncidentButton);
			stackLayout.Children.Add (logoutButton);
			Content = stackLayout;
		}

		async void onJoin(object sender, EventArgs e)
		{
			joinInc.IsEnabled = false;

			if (!String.IsNullOrEmpty (DataManager.getInstance ().getIncidentID ())) {
				await Navigation.PushModalAsync (new RootPage ());
				joinInc.IsEnabled = true;
				return;
			}

			var startText = incId.Text;
			if (String.IsNullOrEmpty(startText)) {
				return;
			}

			var postData = new JObject ();
			postData ["token"] = DataManager.getInstance().getSecret();
			postData ["incident"] = startText; 
			var res = RestCall.POST (URLs.VALIDATETOKEN, postData);

			if ((bool)res ["result"]) {


				var joinRes = RestCall.POST (URLs.JOIN, postData);
				if ((bool)joinRes ["result"]) {

					var dater = DataManager.getInstance ();
				

					dater.resetStandAlone ();
					dater.setIncidentID (startText);
					dater.setIncidentType ("networked");


					var profileRes = RestCall.POST (URLs.GETPROFILE, postData);
					if ((bool)profileRes ["result"]) {
						var profile = (JObject)profileRes["userProfile"];
						dater.setOrg ((string)profile ["organization"]);
						dater.setStatus ((string)profile ["status"]);
						dater.setUnitID ((string)profile ["unit"]);
						dater.setName ((string)profile ["name"]);
						dater.setUnitType ((string)profile ["unitType"]);
					}
					Console.WriteLine(profileRes.ToString());


					var accessRes = RestCall.POST (URLs.ACCESSLEVEL, postData);
					if ((bool)accessRes ["result"]) {
						dater.setRole ((string)accessRes ["accessLevel"]);
					}
					Console.WriteLine(accessRes.ToString());
					incId.IsEnabled = false;
					await Navigation.PushModalAsync (new RootPage ());


					//Make requests based on role.
					//transfer to map page
				} else {
					await DisplayAlert ("Invalid Incident ID", "", "OK");
					return;
				}

			
			} else {
				joinInc.IsEnabled = true;
				onLogout (null,null);
			}				
			joinInc.IsEnabled = true;

		}

		async void onCreate(object sender, EventArgs e)
		{
			var postData = new JObject ();
			postData ["token"] = DataManager.getInstance().getSecret();

			var res = RestCall.POST (URLs.VALIDATETOKEN, postData);

			if ((bool)res ["result"]) {
				var createRes = RestCall.POST (URLs.CREATE, postData);
				Console.WriteLine (createRes);
				await DisplayAlert("Created Incident : " + createRes["incidentID"], "You can now join it.", "OK");
			} else {
				onLogout (null,null);
			}

		}

		async void onLogout(object sender, EventArgs e)
		{
			
			var postData = new JObject ();
			postData ["token"] = DataManager.getInstance().getSecret();

			RestCall.POST (URLs.LOGOUT, postData);
			DataManager.getInstance ().setSecret ("");
			DataManager.getInstance ().resetStandAlone ();


			Navigation.InsertPageBefore(new LoginPage (), this);
			await Navigation.PopAsync().ConfigureAwait(false);
		

		}

	}
}

