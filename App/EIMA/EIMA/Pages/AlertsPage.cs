using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{

	public class AlertsPage : ContentPage
	{
		public AlertsPage ()
		{
			//Alerts DAta, feel free to delete everything below.
			var data = DataManager.getInstance ();
			List<EIMAAlert> alertData = data.getAlerts ();

			//TODO
			//Core functionality of this class is clicking on messages and them opening in a new content page. 

			//Second core functionality is opening a new message window when the + button is clicked
			//Should use the above references to create a new content page and navigate to it with a text box and a send, 
			//need to figure out specifics still.

			var button = new Button
			{
				Text = "+",
				Font = Font.SystemFontOfSize(NamedSize.Large),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Start
			};
			Title = "Alerts";
			Icon = "Alerts.png";

			var listView = new ListView();
			listView.ItemsSource = new []{
				"Message From X",
				"Message From Y",
				"Message From Z",
				"Alert Lava incoming",
				"Close to Danger Zone",
				"In Danger Zone",
				"New Command From ICS Command",
				"Alert From I need to test scrolling",
				"Alert From HQ",
				"Message From ~",
				"Message From B",
				"Message From R",
				"Message From A",
				"Message From D",
				"Message From -",
				"Message From R",
			};


			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (listView);
			stackLayout.Children.Add (button);
			Content = stackLayout;
		}
	}
	
}