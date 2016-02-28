using Xamarin.Forms;

namespace EIMA
{

	public class AlertsPage : ContentPage
	{
		public AlertsPage ()
		{
			//TODO
			//Core functionality of this class is clicking on messages and them opening in a new content page. 

			//Content page Ref
			//https://developer.xamarin.com/api/type/Xamarin.Forms.ContentPage/

			//Navigation  references
			//http://stackoverflow.com/questions/25165106/how-do-you-switch-pages-in-xamarin-forms
			//https://developer.xamarin.com/api/type/Xamarin.Forms.NavigationPage/
			//https://developer.xamarin.com/guides/xamarin-forms/getting-started/introduction-to-xamarin-forms/#Navigation

			//Second core functionality is opening a new message window when the + button is clicked
			//Should use the above references to create a new content page and navigate to it with a text box and a send, 
			//need to figure out specifics still.

			Button button = new Button
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
			listView.ItemsSource = new string[]{
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


			this.Content = new StackLayout
			{
				Children = 
				{
					listView,
					button
				}
			};
		}
	}
	
}