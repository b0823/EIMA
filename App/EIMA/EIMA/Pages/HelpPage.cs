using Xamarin.Forms;

namespace EIMA
{

	public class HelpPage : ContentPage
	{
		public HelpPage ()
		{

			var helpOptions = new TableSection ("User Guide");
			foreach (string option in CONSTANTS.helpOptions) {
				var cell = new TextCell {
					Text = option, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new HelpOptionPage(option))),
				};

				helpOptions.Add (cell);
			}

			Title = "Help Page";
			Icon = "Help.png";

			TableRoot root = new TableRoot ();
			root.Add (helpOptions);
			TableView tableView = new TableView (root);

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (tableView);
			Content = stackLayout;

		}
	}

}