using System;
using Xamarin.Forms;

namespace EIMAMaster
{
	public class AddToMapPage : ContentPage
	{
		public AddToMapPage ()
		{

			/**
			 * I'm just putting this here as a placeholder until we know exactly what we want.
			 * 
			 * Basicaly we take this information save it, then ask the user to select a locaiton, 
			 * AND create the item at that location
			 * 
			 * OR.
			 * 
			 * We have them longPress(See MapModel) a location then have displayWindow Open with cancel or 
			 * add to Map,i f they add to Map this window opens, we fill in data and then add the
			 * item to the map at the known location
			 * 
			 * 
			 * The second approach seems better but I'm still contemplating. 
			 * I'm rambling now idk
			 */
			Label header = new Label
			{
				Text = "Specify Pin",
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

			Button addToMap = new Button
			{
				Text = "Add To Map",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};

			addToMap.Clicked += async (sender, e) => {
				//await Navigation.PopToRootAsync();
				//await Navigation.PopAsync(); 
				//Use these two when we go from MapPage PopModal when coming from MapModel

				await Navigation.PopModalAsync();
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
					addToMap
				}
			};

			Title = "User Profile";
			Icon = "UserProfile.png";
		}
	}
}

