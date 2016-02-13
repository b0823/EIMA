using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMAMaster
{

	public class UserProfilePage : ContentPage
	{
		public UserProfilePage ()
		{
			//TODO REFINE WHAT GOES HERE? Do we need more specifiers, what's in the list?

			//TODO Bulk of work here will be saving these locally and updating to network upon changes


			Label header = new Label
			{
				Text = "EIMA",
				FontSize = 50,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var unitid = new Entry { 
				Placeholder = "Unit #",
				VerticalOptions = LayoutOptions.Center
			};
			var organization = new Entry { 
				Placeholder = "Organization",
				VerticalOptions = LayoutOptions.Center
			};

			//TODO is status like a dropdown? I.E. healthy/fine/good/in need of help/dieing?
			var status = new Entry { 
				Placeholder = "Current Status",
				VerticalOptions = LayoutOptions.Center
			};

			Picker picker = new Picker
			{
				Title = "Unit Type",
				VerticalOptions = LayoutOptions.Center
			};

			picker.Items.Add ("Police");
			picker.Items.Add ("Fire");
			picker.Items.Add ("EMS");
			picker.Items.Add ("Volcano Response");
			picker.Items.Add ("Hazmat");
			picker.Items.Add ("Other");

			Button update = new Button
			{
				Text = "Update",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};

			this.Content = new StackLayout
			{
				Children = 
				{
					header,
					unitid,
					organization,
					status,
					picker,
					update
				}
			};
			
			Title = "User Profile";
			Icon = "UserProfile.png";
		}
	}
	
}