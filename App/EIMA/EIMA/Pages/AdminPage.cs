using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Linq;

namespace EIMA
{
	public class AdminPage : ContentPage
	{
		public AdminPage ()
		{
			Title = "Administration";

			//User Data
			var data = DataManager.getInstance ();

			List<EIMAUser> list = new List<EIMAUser> ();


			// The following is user information for testing

			/*
			EIMAUser user1 = new EIMAUser ();
			user1.username = "User 1's username";
			user1.name = "User 1";
			user1.unit = "User 1's unit";
			user1.unitType = "User 1's unit type";
			user1.organization = "User 1's organization";
			user1.status = "User 1's status";
			user1.level = "Map Editor";
			list.Add (user1);

			EIMAUser user2 = new EIMAUser ();
			user2.username = "User 2's username";
			user2.name = "User 2";
			user2.unit = "User 2's unit";
			user2.unitType = "User 2's unit type";
			user2.organization = "User 2's organization";
			user2.status = "User 2's status";
			user2.level = "Map Viewer";
			list.Add (user2);

			EIMAUser user3 = new EIMAUser ();
			user3.username = "User 3's username";
			user3.name = "User 3";
			user3.unit = "User 3's unit";
			user3.unitType = "User 3's unit type";
			user3.organization = "User 3's organization";
			user3.status = "User 3's status";
			user3.level = "Map Viewer";
			list.Add (user3);

			EIMAUser user4 = new EIMAUser ();
			user4.username = "User 4's username";
			user4.name = "User 4";
			user4.unit = "User 4's unit";
			user4.unitType = "User 4's unit type";
			user4.organization = "User 4's organization";
			user4.status = "User 4's status";
			user4.level = "Admin";
			list.Add (user4);

			EIMAUser user5 = new EIMAUser ();
			user5.username = "User 5's username";
			user5.name = "User 5";
			user5.unit = "User 5's unit";
			user5.unitType = "User 5's unit type";
			user5.organization = "User 5's organization";
			user5.status = "User 5's status";
			user5.level = "Map Editor";
			list.Add (user5);

			EIMAUser user6 = new EIMAUser ();
			user6.username = "User 6's username";
			user6.name = "User 6";
			user6.unit = "User 6's unit";
			user6.unitType = "User 6's unit type";
			user6.organization = "User 6's organization";
			user6.status = "User 6's status";
			user6.level = "Map Viewer";
			list.Add (user6);

			data.setUsers (list);
			*/


			List<EIMAUser> userData = data.getUsers ();

			// Sort the users by privilege level
			List<EIMAUser> mapViewers = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "Map Viewer") {
					mapViewers.Add (user);
				}
			}

			List<EIMAUser> mapEditors = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "Map Editor") {
					mapEditors.Add (user);
				}
			}

			List<EIMAUser> admins = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "Admin") {
					admins.Add (user);
				}
			}

			var mapViewer = new TableSection ("Map Viewers");
			foreach (EIMAUser user in mapViewers) {
				var cell = new TextCell {
					Text = user.name, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				mapViewer.Add (cell);
			}

			var mapEditor = new TableSection ("Map Editors");
			foreach (EIMAUser user in mapEditors) {
				var cell = new TextCell {
					Text = user.name, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				mapEditor.Add (cell);
			}

			var admin = new TableSection ("Admins");
			foreach (EIMAUser user in admins) {
				var cell = new TextCell {
					Text = user.name, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				admin.Add (cell);
			}

			TableRoot root = new TableRoot ();
			root.Add (mapViewer);
			root.Add (mapEditor);
			root.Add (admin);
			TableView tableView = new TableView (root);

			Content = tableView;

		}
	}
}

