using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Linq;

namespace EIMA
{
	public class UserInfoPage : ContentPage
	{
		public UserInfoPage (EIMAUser user)
		{
			var data = DataManager.getInstance ();
			var name = new TableSection ("Name");
			var cell1 = new TextCell {
				Text = user.name,
				TextColor = Color.White,
			};
			name.Add (cell1);

			var username = new TableSection ("Username");
			var cell2 = new TextCell {
				Text = user.username,
				TextColor = Color.White,
			};
			username.Add (cell2);

			var unit = new TableSection ("Unit");
			var cell3 = new TextCell {
				Text = user.unit,
				TextColor = Color.White,
			};
			unit.Add (cell3);

			var unitType = new TableSection ("Unit Type");
			var cell4 = new TextCell {
				Text = user.unitType,
				TextColor = Color.White,
			};
			unitType.Add (cell4);

			var organization = new TableSection ("Organization");
			var cell5 = new TextCell {
				Text = user.organization,
				TextColor = Color.White,
			};
			organization.Add (cell5);

			var status = new TableSection ("Status");
			var cell6 = new TextCell {
				Text = user.status,
				TextColor = Color.White,
			};
			status.Add (cell6);

			var level = new TableSection ("Privilege Level");
			var cell7 = new TextCell {
				Text = user.level,
				TextColor = Color.White,
			};
			level.Add (cell7);


			TableRoot root = new TableRoot ();
			root.Add (name);
			root.Add (username);
			root.Add (unit);
			root.Add (unitType);
			root.Add (organization);
			root.Add (status);
			root.Add (level);
			TableView tableView = new TableView (root);

			var command1 = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ());
			var command2 = new Command (async o => {
				var action = await Application.Current.MainPage.DisplayActionSheet (
					"Enter New Privilege Level",
					"Cancel",
					null,
					"No Access",
					"Standard User",
					"Map Editor",
					"Admin"
				);

				// Still need to finish this section. Need to store the new privilege level and update the page
				if (action == "No Access") { 
					user.level = "No Access";
					await Application.Current.MainPage.DisplayAlert ("(not fully implemented)", "The user is now a No Access User", "OK");
				} else if (action == "Standard User") { 
					user.level = "Standard User";
					await Application.Current.MainPage.DisplayAlert ("(not fully implemented)", "The user is now a Standard User", "OK");
				}	else if (action == "Map Editor") { 
					user.level = "Map Editor";
					await Application.Current.MainPage.DisplayAlert ("(not fully implemented)", "The user is now a Map Editor", "OK");
				} else if (action == "Admin") { 
					user.level = "Admin";
					await Application.Current.MainPage.DisplayAlert ("(not fully implemented)", "The user is now an Admin", "OK");
				}
			});


			var changePrivilege = new Button {
				Text = "Change Privilege",
				VerticalOptions = LayoutOptions.Start,
				Command = command2,
			};

			var backButton = new Button {
				Text = "Back",
				VerticalOptions = LayoutOptions.Start,
				Command = command1,
			};

			var label = new Label {
				Text = "Enter your message:",
				FontSize = 20
			};

			var entry = new Entry {
				Placeholder = "Message"
			};

			var sendButton = new Button {
				Text = "Send",

				// insert code to send message

				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
			};

			var cancelButton = new Button {
				Text = "Cancel",
				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
			};

			var stackLayout1 = new StackLayout ();
			stackLayout1.Children.Add (label);
			stackLayout1.Children.Add (entry);
			var stackLayout2 = new StackLayout ();
			stackLayout2.Children.Add (sendButton);
			stackLayout2.Children.Add (cancelButton);
			stackLayout2.HorizontalOptions = LayoutOptions.Center;
			stackLayout2.Orientation = StackOrientation.Horizontal;
			stackLayout1.Children.Add (stackLayout2);
			stackLayout1.VerticalOptions = LayoutOptions.Center;

			// make a new content page for sending messages
			ContentPage contentpage = new ContentPage ();
			contentpage.Content = stackLayout1;

			var command3 = new Command (async o => await Application.Current.MainPage.Navigation.PushModalAsync (contentpage));
			var messageButton = new Button {
				Text = "Send Message",
				VerticalOptions = LayoutOptions.Start,
				Command = command3,
			};

			var stackLayout3 = new StackLayout ();
			stackLayout3.Children.Add (tableView);
			var stackLayout4 = new StackLayout ();
			stackLayout4.Children.Add (changePrivilege);
			stackLayout4.Children.Add (messageButton);
			stackLayout4.Children.Add (backButton);
			stackLayout4.HorizontalOptions = LayoutOptions.Center;
			stackLayout4.Orientation = StackOrientation.Horizontal;
			stackLayout3.Children.Add (stackLayout4);
			Content = stackLayout3;

		}
	}
}