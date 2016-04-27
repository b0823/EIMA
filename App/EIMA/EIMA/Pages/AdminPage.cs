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

			List<EIMAUser> list = data.getUsers ();


	
			//List<EIMAUser> userData = data.getUsers ();

			// Sort the users by privilege level
			List<EIMAUser> noAccessUsers = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "noAccess") {
					noAccessUsers.Add (user);
				}
			}

			List<EIMAUser> standardUsers = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "user") {
					standardUsers.Add (user);
				}
			}

			List<EIMAUser> mapEditors = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "mapEditor") {
					mapEditors.Add (user);
				}
			}

			List<EIMAUser> admins = new List<EIMAUser> ();
			foreach (EIMAUser user in list) {
				if (user.level == "admin") {
					admins.Add (user);
				}
			}
			admins.ForEach (Console.WriteLine);
			TableRoot root = new TableRoot ();
			var noAccess = new TableSection ("No Access");
			foreach (EIMAUser user in noAccessUsers) {
				var cell = new TextCell {
					Text = user.username, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user)))
				};
				noAccess.Add (cell);
			}

			var standardUser = new TableSection ("Standard Users");
			foreach (EIMAUser user in standardUsers) {
				var cell = new TextCell {
					Text = user.username, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				standardUser.Add (cell);
			}

			var mapEditor = new TableSection ("Map Editors");
			foreach (EIMAUser user in mapEditors) {
				var cell = new TextCell {
					Text = user.username, 
					TextColor = Color.White,
					Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
				};

				mapEditor.Add (cell);
			}

			var admin = new TableSection ("Admins");
			foreach (EIMAUser user in admins) {
				var cell = new TextCell {
					Text = user.username, 
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
					if (user.level == "noAccess") { 
						noAccessUsers.Add(user);
					} else if (user.level == "user") { 
						standardUsers.Add(user);
					}	else if (user.level == "mapEditor") { 
						mapEditors.Add(user);
					} else if (user.level == "admin") {
						admins.Add(user);
					}
				}
				Users.userList.Clear();

				noAccess = new TableSection ("No Access");
				foreach (EIMAUser user in noAccessUsers) {
					var cell = new TextCell {
						Text = user.username, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user)))
					};
					noAccess.Add (cell);
				}

				standardUser = new TableSection ("Standard Users");
				foreach (EIMAUser user in standardUsers) {
					var cell = new TextCell {
						Text = user.username, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
					};

					standardUser.Add (cell);
				}

				mapEditor = new TableSection ("Map Editors");
				foreach (EIMAUser user in mapEditors) {
					var cell = new TextCell {
						Text = user.username, 
						TextColor = Color.White,
						Command = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (new UserInfoPage (user))),
					};

					mapEditor.Add (cell);
				}

				admin = new TableSection ("Admins");
				foreach (EIMAUser user in admins) {
					var cell = new TextCell {
						Text = user.username, 
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
