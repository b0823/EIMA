using System;
using Xamarin.Forms;

namespace EIMAMaster
{
	public class IncidentPage : ContentPage
	{
		public IncidentPage ()
		{			
			Label header = new Label
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

			Button joinInc = new Button
			{
				Text = "Join Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};

			joinInc.Clicked += (sender, e) => {
				DisplayAlert("Warning","This is yet to be implemented","Acknowledge");
			};

			Button createIncidentButton = new Button
			{
				Text = "Create Incident",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			createIncidentButton.Clicked += (sender, e) => {
				DisplayAlert("Warning","This is yet to be implemented","Acknowledge");
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					incId,
					joinInc,
					createIncidentButton
				}
			};
		}

	}
}

