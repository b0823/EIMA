using Xamarin.Forms;
using System.IO;
using System.Reflection;

namespace EIMA
{

	public class HelpOptionPage : ContentPage
	{
		public HelpOptionPage (char option)
		{
			Stream stream = null;
			string text = "";

			var assembly = typeof(HelpOptionPage).GetTypeInfo().Assembly;

			if (option == '1') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.QuickStartGuide.txt");
			} else if (option == '2') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.UsersLogin.txt");
			} else if (option == '3') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.EventCreationLogin.txt");
			} else if (option == '4') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.MapEditing.txt");
			} else if (option == '5') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.AlertMessages.txt");
			} else if (option == '6') {
				stream = assembly.GetManifestResourceStream("EIMA.Droid.Resources.AdminGuide.txt");
			} 
			using (var reader = new System.IO.StreamReader (stream)) {
				text = reader.ReadToEnd ();
			}
				
			var optionText = new Label {
				Text = text,
				FontSize = 20
			};

			var cancelButton = new Button {
				Text = "Back",
				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
			};

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (optionText);
			stackLayout.Children.Add (cancelButton);
			var scrollView = new ScrollView { Content = stackLayout };
			Content = scrollView;
		}
	}


}