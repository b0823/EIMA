using System;
using Xamarin.Forms;

namespace EIMA
{

	public class UserProfilePage : ContentPage
	{
		Entry nameEntry;
		Entry orgEntry;
		Entry unitEntry;
		Entry statusEntry;

		Picker picker;

		Button update;

		public UserProfilePage ()
		{
			buildUI ();

			update.Clicked  += (sender, e) => {
				var data = new DataManager();
				if(nameEntry.Text != ""){data.setName(nameEntry.Text);}
				if(orgEntry.Text != ""){data.setOrg(orgEntry.Text);}
				if(unitEntry.Text != ""){data.setUnitID(unitEntry.Text);}
				if(statusEntry.Text != ""){data.setStatus(statusEntry.Text);}

				if(picker.SelectedIndex > -1){
					data.setUnitType(CONSTANTS.uTypeOptions[picker.SelectedIndex]);
				}
				DisplayAlert("Saved!","Your Information Was Saved","OK");
			};
		}
		public void putExistingData(){
			var data = new DataManager ();

			if (data.getName() != "") nameEntry.Text = data.getName();
			if (data.getOrg() != "") orgEntry.Text = data.getOrg();
			if (data.getUnitID() != "") unitEntry.Text = data.getUnitID();
			if (data.getStatus() != "") statusEntry.Text = data.getStatus();
			if (data.getUnitType() != "")
				picker.SelectedIndex = Array.IndexOf (uTypeOptions, data.getUnitType());
		}

		public void buildUI(){
			Label header = new Label
			{
				Text = "EIMA",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};
			nameEntry = new Entry {
				Placeholder = "Name or Identifier",
				VerticalOptions = LayoutOptions.Center
			};
			orgEntry = new Entry { 
				Placeholder = "Organization",
				VerticalOptions = LayoutOptions.Center
			};

			unitEntry = new Entry { 
				Placeholder = "Unit #",
				VerticalOptions = LayoutOptions.Center
			};

			statusEntry = new Entry { 
				Placeholder = "Current Status",
				VerticalOptions = LayoutOptions.Center
			};

			picker = new Picker
			{
				Title = "Unit Type",
				VerticalOptions = LayoutOptions.Center
			};

			foreach (string element in uTypeOptions)
			{
				picker.Items.Add (element);
			}

			update = new Button
			{
				Text = "Update",
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};
			putExistingData ();
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (header);
			stackLayout.Children.Add (nameEntry);
			stackLayout.Children.Add (unitEntry);
			stackLayout.Children.Add (orgEntry);
			stackLayout.Children.Add (statusEntry);
			stackLayout.Children.Add (picker);
			stackLayout.Children.Add (update);
			Content = stackLayout;

			Title = "User Profile";
			Icon = "UserProfile.png";
		}
	}
	
}