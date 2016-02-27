using Xamarin.Forms;

namespace EIMA
{

	public class LogoutPage : ContentPage
	{
		public LogoutPage ()
		{

			//TODO Work will be dependent on what mode we're in

			//STANDLONE -- Bulk of work here will come from interaction with data
				//Logout will have an option to clear current incident.

			//Networked, Logout/Leave incident options.

			Label header = new Label
			{
				Text = "Exit Options",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			Button logout = new Button
			{
				Text = "Logout",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
			};

			Button leaveIncident = new Button
			{
				Text = "Leave Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					logout,
					leaveIncident
				}
			};


			Title = "Logout";
			Icon = "Logout.png";
		}
	}

}