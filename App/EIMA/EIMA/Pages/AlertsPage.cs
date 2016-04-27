using Xamarin.Forms;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

namespace EIMA
{

	public class AlertsPage : ContentPage
	{
		public AlertsPage ()
		{

			var data = DataManager.getInstance ();

			List<EIMAAlert> testAlertData = data.getAlerts ();



			//			List<EIMAAlert> alertData = data.getAlerts ();


			Title = "Alerts";
			Icon = "Alerts.png";

			var alerts = new TableSection ("Alerts");
			foreach (EIMAAlert alert in testAlertData) {
				var cell = new TextCell {
					Text = alert.message, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new AlertInfoPage (alert))),
				};

				alerts.Add (cell);
			}
			TableRoot root = new TableRoot ();
			root.Add (alerts);
			TableView tableView = new TableView (root);

			// content page for sending messages
			ContentPage contentpage = new ContentPage ();
			var command1 = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (contentpage));
			var button = new Button
			{
				Text = "Send Message",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Start,
				Command = command1
			};
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (tableView);
			stackLayout.Children.Add (button);
			Content = stackLayout;


			// stack layout for the send message window
			var label = new Label {
				Text = "Enter your message:",
				FontSize = 20
			};
			var entry = new Entry {
				Placeholder = "Message"
			};

			var sendCommands = new Command (async o => {
				var postData = new JObject ();

				postData ["token"] = data.getSecret();
				postData ["message"] = entry.Text;
				postData ["username"] = "ADMINS";

				Console.WriteLine(RestCall.POST (URLs.SENDMESSAGE, postData));
				await Application.Current.MainPage.Navigation.PopModalAsync ();
			});

			var sendButton = new Button {
				Text = "Send",
				// insert code to send message

				Command = sendCommands
			};
			var cancelButton = new Button {
				Text = "Cancel",
				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
			};

			var stackLayout1 = new StackLayout ();
			stackLayout1.Children.Add (label);
			stackLayout1.Children.Add (entry);
			var stackLayout2 = new StackLayout ();
			stackLayout2.Children.Add (sendButton);
			stackLayout2.Children.Add (cancelButton);
			stackLayout2.HorizontalOptions = LayoutOptions.Center;
			stackLayout2.Orientation = StackOrientation.Horizontal;
			stackLayout1.Children.Add (stackLayout2);
			stackLayout1.VerticalOptions = LayoutOptions.Center;

			contentpage.Content = stackLayout1;

		}
	}

}