using System.Collections.Generic;

namespace EIMA
{

	public class MenuListData : List<MenuItem>
	{
		static readonly MenuItem map = new MenuItem () {Title = "Map", IconSource = "Map.png", TargetType = typeof(MapPage)};
		static readonly MenuItem alerts = new MenuItem () {Title = "Alerts", IconSource = "Alerts.png", TargetType = typeof(AlertsPage)};
		static readonly MenuItem userProf = new MenuItem () {Title = "User Profile", IconSource = "UserProfile.png", TargetType = typeof(UserProfilePage)};
		static readonly MenuItem settings = new MenuItem () {Title = "Settings", IconSource = "SettingsIcon.png", TargetType = typeof(SettingsPage)};
		static readonly MenuItem admin = new MenuItem () {Title = "Admin", IconSource = "Admin.png", TargetType = typeof(AdminPage)};
		static readonly MenuItem help = new MenuItem () {Title = "Help", IconSource = "Help.png", TargetType = typeof(HelpPage)};
		static readonly MenuItem logout = new MenuItem () {Title = "Logout", IconSource = "Logout.png", TargetType = typeof(LogoutPage)};

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
			Add (map);
			Add (settings);
			Add (help);

		}
		public void networkedAdmin(){
			Add (map);
			Add (alerts);
			Add (userProf);
			Add (settings);
			Add (admin);
			Add (help);
			Add (logout);
		}
		public void networkedMapEditor(){
			Add (map);
			Add (alerts);
			Add (userProf);
			Add (settings);
			Add (help);
			Add (logout);
		}
		public void networkedUserMenu(){
			Add (map);
			Add (alerts);
			Add (userProf);
			Add (settings);
			Add (help);
			Add (logout);
		}
	}
}