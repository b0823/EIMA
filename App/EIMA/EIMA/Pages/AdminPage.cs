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


			EIMAUser user1 = new EIMAUser ();
			user1.username = "User 1's username";
			user1.name = "User 1";
			user1.unit = "User 1's unit";
			user1.unitType = "User 1's unit type";
			user1.organization = "User 1's organization";
			user1.status = "User 1's status";
			user1.level = "No Access";
			list.Add (user1);
			EIMAUser user2 = new EIMAUser ();
			user2.username = "User 2's username";
			user2.name = "User 2";
			user2.unit = "User 2's unit";
			user2.unitType = "User 2's unit type";
			user2.organization = "User 2's organization";
			user2.status = "User 2's status";
			user2.level = "Standard User";
			list.Add (user2);
			EIMAUser user3 = new EIMAUser ();
			user3.username = "User 3's username";
			user3.name = "User 3";
			user3.unit = "User 3's unit";
			user3.unitType = "User 3's unit type";
			user3.organization = "User 3's organization";
			user3.status = "User 3's status";
			user3.level = "No Access";
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
			user6.level = "Map Editor";
			list.Add (user6);
			data.setUsers (list);



			List<EIMAUser> userData = data.getUsers ();

			// Sort the users by privilege level
			List<EIMAUser> noAccessUsers = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "No Access") {
					noAccessUsers.Add (user);
				}
			}

			List<EIMAUser> standardUsers = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "Standard User") {
					standardUsers.Add (user);
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
			TableRoot root = new TableRoot ();
			var noAccess = new TableSection ("No Access");
			foreach (EIMAUser user in noAccessUsers) {
				var cell = new TextCell {
					Text = user.name, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user)))
				};
				noAccess.Add (cell);
			}

			var standardUser = new TableSection ("Standard Users");
			foreach (EIMAUser user in standardUsers) {
				var cell = new TextCell {
					Text = user.name, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				standardUser.Add (cell);
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

			var refreshButton = new Button();

			// big command to refresh the page with updated privilege levels
			Command command1 = new Command (c => {

				root.Remove (noAccess);
				root.Remove (standardUser);
				root.Remove (mapEditor);
				root.Remove (admin);


				foreach (EIMAUser user in Users.userList) {
					if (noAccessUsers.Contains(user)) {
						noAccessUsers.Remove (user);
					}
					else if (standardUsers.Contains(user)) {
						standardUsers.Remove (user);
					}
					else if (mapEditors.Contains(user)) {
						mapEditors.Remove (user);
					}
					else if (admins.Contains(user)) {
						admins.Remove (user);
					}
				}

				foreach (EIMAUser user in Users.userList){
					if (user.level == "No Access") { 
						noAccessUsers.Add(user);
					} else if (user.level == "Standard User") { 
						standardUsers.Add(user);
					}	else if (user.level == "Map Editor") { 
						mapEditors.Add(user);
					} else if (user.level == "Admin") {
						admins.Add(user);
					}
				}
				Users.userList.Clear();

				noAccess = new TableSection ("No Access");
				foreach (EIMAUser user in noAccessUsers) {
					var cell = new TextCell {
						Text = user.name, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user)))
					};
					noAccess.Add (cell);
				}

				standardUser = new TableSection ("Standard Users");
				foreach (EIMAUser user in standardUsers) {
					var cell = new TextCell {
						Text = user.name, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
					};

					standardUser.Add (cell);
				}

				mapEditor = new TableSection ("Map Editors");
				foreach (EIMAUser user in mapEditors) {
					var cell = new TextCell {
						Text = user.name, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
					};

					mapEditor.Add (cell);
				}

				admin = new TableSection ("Admins");
				foreach (EIMAUser user in admins) {
					var cell = new TextCell {
						Text = user.name, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
					};

					admin.Add (cell);
				}
					
				root.Add(noAccess);
				root.Add(standardUser);
				root.Add(mapEditor);
				root.Add(admin);

				var stackLayout2 = new StackLayout ();
				TableView tableView2 = new TableView (root);
				stackLayout2.Children.Add (tableView2);
				stackLayout2.Children.Add (refreshButton);
				Content = stackLayout2;
			});

			refreshButton = new Button {
				Text = "Refresh",
				HorizontalOptions = LayoutOptions.Center,
				Command = command1,
			};

			root.Add (noAccess);
			root.Add (standardUser);
			root.Add (mapEditor);
			root.Add (admin);
			TableView tableView = new TableView (root);
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (tableView);
			stackLayout.Children.Add (refreshButton);
			Content = stackLayout;

		}
	}
}
