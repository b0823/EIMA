using Xamarin.Forms;

namespace EIMA
{

	public class HelpPage : ContentPage
	{
		public HelpPage ()
		{
			var header = new Label
			{
				Text = "User Guide",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			//TODO
			//Core functionality of this class is allowing each of these to open a
			//new content page with a bunch of text that tells people how to use stuff.

			//Look into list view for handling their interaction with list, this is where handlers for each will be set up.
			//https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/data-and-databinding/


			var listView = new ListView();
			listView.ItemsSource = CONSTANTS.helpOptions;
			Title = "Help Page";
			Icon = "Help.png";
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (listView);
			Content = stackLayout;
		}
	}

}