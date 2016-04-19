using Xamarin.Forms;
using System;

namespace EIMA
{

	public class LogoutPage : ContentPage
	{
		public async void goBack(object sender, EventArgs e){
			await Navigation.PopModalAsync();
		}

		public LogoutPage ()
		{

			//TODO Work will be dependent on what mode we're in

			//STANDLONE -- Bulk of work here will come from interaction with data
				//Logout will have an option to clear current incident.

			//Networked, Logout/Leave incident options.

			var header = new Label
			{
				Text = "Exit Options",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var logout = new Button
			{
				Text = "Go Back To Lobby",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
			};
			logout.Clicked += goBack;

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (logout);
			Content = stackLayout;


			Title = "Logout";
			Icon = "Logout.png";
		}
			
	}

}