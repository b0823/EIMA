using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{

	public class MenuListData : List<MenuItem>
	{
		private static MenuItem map = new MenuItem () {Title = "Map", IconSource = "Map.png", TargetType = typeof(MapPage)};
		private static MenuItem alerts = new MenuItem () {Title = "Alerts", IconSource = "Alerts.png", TargetType = typeof(AlertsPage)};
		private static MenuItem userProf = new MenuItem () {Title = "User Profile", IconSource = "UserProfile.png", TargetType = typeof(UserProfilePage)};
		private static MenuItem settings = new MenuItem () {Title = "Settings", IconSource = "SettingsIcon.png", TargetType = typeof(SettingsPage)};
		private static MenuItem admin = new MenuItem () {Title = "Admin", IconSource = "Admin.png", TargetType = typeof(AdminPage)};
		private static MenuItem help = new MenuItem () {Title = "Help", IconSource = "Help.png", TargetType = typeof(HelpPage)};
		private static MenuItem logout = new MenuItem () {Title = "Logout", IconSource = "Logout.png", TargetType = typeof(LogoutPage)};

		public MenuListData ()
		{
			var data = new DataManager ();
			//See marie's Spec for spreadsheet on why things are this way.
			if (data.isStandAlone ()) {
				standAloneMenu ();
			} else if (data.isNetworked()){

				if (data.isAdmin ()) {
					networkedAdmin ();
				} else if (data.isMapEditor ()) {
					networkedMapEditor ();
				} else if (data.isUser ()) {
					networkedUserMenu ();
				}
			}
				
		}

		public void standAloneMenu(){
//			this.Add (map);
//			this.Add (settings);
//			this.Add (help);
			this.Add (map);
			this.Add (alerts);
			this.Add (userProf);
			this.Add (settings);
			this.Add (admin);
			this.Add (help);
			this.Add (logout);
		}
		public void networkedAdmin(){
			this.Add (map);
			this.Add (alerts);
			this.Add (userProf);
			this.Add (settings);
			this.Add (admin);
			this.Add (help);
			this.Add (logout);
		}
		public void networkedMapEditor(){
			this.Add (map);
			this.Add (alerts);
			this.Add (userProf);
			this.Add (settings);
			this.Add (help);
			this.Add (logout);
		}
		public void networkedUserMenu(){
			this.Add (map);
			this.Add (alerts);
			this.Add (userProf);
			this.Add (settings);
			this.Add (help);
			this.Add (logout);
		}
	}
}