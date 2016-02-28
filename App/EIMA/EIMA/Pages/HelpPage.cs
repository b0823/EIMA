using Xamarin.Forms;

namespace EIMA
{

	public class HelpPage : ContentPage
	{
		public HelpPage ()
		{
			Label header = new Label
			{
				Text = "User Guide",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			//TODO
			//Core functionality of this class is allowing each of these to open a
			//new content page with a bunch of text that tells people how to use stuff.

			//Look into list view for handling their interaction with list, this is where handlers for each will be set up.
			//https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/data-and-databinding/


			var listView = new ListView();
			listView.ItemsSource = new string[]{
				"Section 1 - QuickStart Guide",
				"Section 2 - Users and Login",
				"Section 3 - Event Creation and Login",
				"Section 4 - Map Editing",
				"Section 5 - Messaging and Alerts"
			};
			Title = "Help Page";
			Icon = "Help.png";
			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					listView
				}
			};
		}
	}

}