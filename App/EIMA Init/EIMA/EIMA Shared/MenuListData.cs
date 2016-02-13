using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMAMaster
{

	public class MenuListData : List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Map", 
				IconSource = "Map.png", 
				TargetType = typeof(MapPage)
			});

			this.Add (new MenuItem () { 
				Title = "Alerts", 
				IconSource = "Alerts.png", 
				TargetType = typeof(AlertsPage)
			});

			this.Add (new MenuItem () { 
				Title = "User Profile", 
				IconSource = "UserProfile.png", 
				TargetType = typeof(UserProfilePage)
			});

			this.Add (new MenuItem () {
				Title = "Settings",
				IconSource = "SettingsIcon.png",
				TargetType = typeof(SettingsPage)
			});
			this.Add (new MenuItem () {
				Title = "Help",
				IconSource = "Help.png",
				TargetType = typeof(HelpPage)
			});
			this.Add (new MenuItem () {
				Title = "Logout",
				IconSource = "Logout.png",
				TargetType = typeof(LogoutPage)
			});
			//if this were networked then do a check for if has admin then add the admin page here.
			//no where close to this yet.
		}
	}
}