using Xamarin.Forms;

namespace EIMA
{
	public class IncidentPage : ContentPage
	{
		public IncidentPage ()
		{			
			var header = new Label
			{
				Text = "Join/Create Incident",
				FontSize = 40,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var incId = new Entry { 
				Placeholder = "INCIDENT ID",
				VerticalOptions = LayoutOptions.Center
			};

			var joinInc = new Button
			{
				Text = "Join Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};

			joinInc.Clicked += (sender, e) => DisplayAlert ("Warning", "This is yet to be implemented", "Acknowledge");

			var createIncidentButton = new Button
			{
				Text = "Create Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			createIncidentButton.Clicked += (sender, e) => DisplayAlert ("Warning", "This is yet to be implemented", "Acknowledge");

			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (incId);
			stackLayout.Children.Add (joinInc);
			stackLayout.Children.Add (createIncidentButton);
			Content = stackLayout;
		}

	}
}

