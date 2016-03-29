using Xamarin.Forms;

namespace EIMA
{

	public class HelpOptionPage : ContentPage
	{
		public HelpOptionPage (string option)
		{
			var stackLayout = new StackLayout ();

			var option1 = new Label {
				Text = "Section 1 - QuickStart Guide\n(more can be added to this section)\n\nEIMA is " +
					"an asset visualization mobile application for incident commanders or other operations " +
					"coordinators at the scene of an ICS incident.\n\nThe goal of this mobile application " +
					"is to provide emergency responders in an incident command structure a high-level " +
					"understanding of current response efforts. This is accomplished by using a map that " +
					"is capable of showing geographic features, responder assets, and dangerous areas.\n",
				FontSize = 20
			};
			var option2 = new Label {
				Text = "Section 2 - Users and Login\n(more can be added to this section)\n\nWhen creating " +
					"a profile, users will be prompted to enter their name, unit #, organization, current " +
					"status, and unit type. This information can be edited at any time by tapping the user " +
					"profile icon from the side menu. Users can log into the application from the login " +
					"screen, by typing in their username and password. Users can be added to an incident " +
					"by the administrator of that incident.\n\nPrivileges\n\nThere are four types of " +
					"privilege levels that a user can have during an incident. The first type is No Access. " +
					"This the lowest privilege level where the user does not have access to the map. The " +
					"second type of privilege level is Standard User. Users of this privilege level are able " +
					"to view the map, but are unable to edit it. The third type of privilege level is Map " +
					"Editor. Users of this privilege level are able to do everything a Standard User can do, " +
					"but they can also edit the map with assets and danger zones. The top privilege level is " +
					"the Admin level. Users of this privilege level are able to do everything map editors can " +
					"do, and also have access to an Admin page which can be accessed from the side bar. From " +
					"this page, all users within the incident can be viewed, alerts can be sent, and the " +
					"privilege levels of the users within the incident can be changed.\n",
				FontSize = 20
			};
			var option3 = new Label {
				Text = "Section 3 - Event Creation and Login\n(need to add more to this section)\n\n" +
					"Administrators have the ability to create new incidents. When an incident is created, " +
					"it is given a unique incident ID. Users can be added to the incident by the administrator " +
					"of the incident. \n",
				FontSize = 20
			};
			var option4 = new Label {
				Text = "Section 4 - Map Editing\n\nAny user that has a Map Editor privilege level or higher " +
					"has the ability to add assets and danger zones to the map. This is done from the map " +
					"page, which can be accessed by tapping the map icon from the side menu. \n\nAssets\n\n" +
					"Assets can be added to the map by long tapping at any location on the map where the " +
					"asset is located, and selecting “Create Asset.” From here, enter the unit name, unit " +
					"type, unit number, organization, and current status, and select “Add to map.” This " +
					"will add an icon to the map, which will show the asset information when tapped. The " +
					"information of an asset can be modified by tapping the icon on the map, tapping the " +
					"information box above the icon, and selecting “Modify Asset Info.” From this menu the " +
					"asset can also be removed from the map by selecting “Delete Asset.”\n\nDanger Zones\n\n" +
					"Circular Area:\nDanger Zones can be added to the map by long tapping anywhere and " +
					"selecting “Create Danger Zone.” To create a danger zone that is circular in area, tap " +
					"the “Circular Area” option in the menu that will appear. From here enter the radius of " +
					"the area, the type of danger zone, and any info associated with the danger zone, and " +
					"select “Enter” to add the danger zone to the map. To modify the radius of the danger " +
					"zone, the type, or the info associated with it, tap the danger zone once from the map " +
					"screen and select the “Edit Danger Zone Size or Info” option. To move the danger zone " +
					"to another location, tap the danger zone once and select “Move Center Of Danger Zone”, " +
					"and tap a new location for the danger zone. To delete the danger zone, select the " +
					"“Delete Danger Zone” option. \n\nCustom Area:\nTo create a danger zone with a custom " +
					"area, long tap anywhere on the map, select “Create Danger Zone”, and then select “Custom " +
					"Area.” From here enter the danger zone type and other info and select “Enter”, and then " +
					"select “Start”. The area of the danger zone can be defined by single tapping locations " +
					"on the map, which will represent the vertices of the danger zone that will be created. " +
					"After three vertices are placed on the map, a menu will appear where more points can be " +
					"added by selecting “Add Another Point”. After all desired vertices are entered onto the " +
					"map, the danger zone can be created by selecting “Create Custom Area”. Similar to the " +
					"circular area, the danger zone can be modified or deleted by single tapping on the danger " +
					"zone. To edit the danger zone info, select “Edit Danger Zone Info”. To change the " +
					"location of the vertices, select “Modify Points”. To delete the danger zone, select " +
					"“Delete Danger Zone”.\n",
				FontSize = 20
			};
			var option5 = new Label {
				Text = "Section 5 - Messaging and Alerts\n\nThe Messages and Alerts page allows for communication " +
					"between Users and Administrators in networked event scenario. This function is disabled " +
					"when using the standalone function of EIMA. \n\nAll alerts from the command post are " +
					"viewable on the Messages and Alerts page. This is accessed by clicking the ! icon from the " +
					"side menu. \n\nViewing an Alert:\nFrom the alerts page, simply click the alert preview to " +
					"view the full text of the alert. Click the “Back” button in the lower-right corner to return " +
					"to the main Messages and Alerts page. All messages sent from the Messages and Alerts page by " +
					"a Map Editor or Standard User are routed directly to the Administrator of an event.  \n\nSending " +
					"a Message:\nTo send a message, click the “Send Message” button in the lower-right corner of " +
					"the Messages and Alerts page. This will open the Message window. When the message is finished, " +
					"click the “Send” button on the lower-left side of the window. To exit the message window, click " +
					"the “Cancel” button in the lower-right corner to return to the main Messages and Alerts page. \n",
				FontSize = 20
			};


			if (option == "Section 1 - QuickStart Guide") {
				stackLayout.Children.Add (option1);
			} else if (option == "Section 2 - Users and Login") {
				stackLayout.Children.Add (option2);
			} else if (option == "Section 3 - Event Creation and Login") {
				stackLayout.Children.Add (option3);
			} else if (option == "Section 4 - Map Editing") {
				stackLayout.Children.Add (option4);
			} else if (option == "Section 5 - Messaging and Alerts") {
				stackLayout.Children.Add (option5);
			} 

			var cancelButton = new Button {
				Text = "Back",
				Command = new Command (async o => await Application.Current.MainPage.Navigation.PopModalAsync ()),
			};

			stackLayout.Children.Add (cancelButton);
			var scrollView = new ScrollView { Content = stackLayout };
			Content = scrollView;

		}
	}

}