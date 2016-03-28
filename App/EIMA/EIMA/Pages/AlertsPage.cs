using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{

	public class AlertsPage : ContentPage
	{
		public AlertsPage ()
		{
			
			var data = DataManager.getInstance ();

			List<EIMAAlert> testAlertData = new List<EIMAAlert>();

			// The following is data to test the alerts page
			EIMAAlert alert1 = new EIMAAlert ();
			alert1.alertType = "alert 1 type";
			alert1.sender = "User 1";
			alert1.message = "alert 1 message";
			alert1.timestamp = 062920171341;
			testAlertData.Add (alert1);

			EIMAAlert alert2 = new EIMAAlert ();
			alert2.alertType = "alert 2 type";
			alert2.sender = "User 6";
			alert2.message = "alert 2 message";
			alert2.timestamp = 101320171508;
			testAlertData.Add (alert2);

			EIMAAlert alert3 = new EIMAAlert ();
			alert3.alertType = "alert 3 type";
			alert3.sender = "User 3";
			alert3.message = "alert 3 message";
			alert3.timestamp = 110720181120;
			testAlertData.Add (alert3);


			data.setAlerts (testAlertData);


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
			var sendButton = new Button {
				Text = "Send",
				// insert code to send message

				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
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