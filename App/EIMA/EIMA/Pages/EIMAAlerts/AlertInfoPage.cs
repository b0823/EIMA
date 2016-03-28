using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Linq;

namespace EIMA
{
	public class AlertInfoPage : ContentPage
	{
		public AlertInfoPage (EIMAAlert alert)
		{

			// display the information associated with the alert

			var sender = new Label {
				Text = "Sent by: " + alert.sender + "\n",
				FontSize = 20
			};
			var message = new Label {
				Text = alert.message + "\n",
				FontSize = 20
			};
			var type = new Label {
				Text = "Alert Type: " + alert.alertType,
				FontSize = 20
			};
			var time = new Label {
				Text = "Timestamp: " + alert.timestamp,
				FontSize = 20
			};

			var command1 = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ());
			var backButton = new Button {
				Text = "Back",
				HorizontalOptions = LayoutOptions.End,
				Command = command1,
			};
				
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (sender);
			stackLayout.Children.Add (message);
			stackLayout.Children.Add (type);
			stackLayout.Children.Add (time);
			stackLayout.Children.Add (backButton);
			Content = stackLayout;
		}
	}
}

