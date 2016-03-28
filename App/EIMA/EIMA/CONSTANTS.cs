using Xamarin.Forms;
using System;
using System.Collections;
using System.Linq;

namespace EIMA
{
	public static class CONSTANTS
	{
		//Universal constants go here. Most useful for lists used in various locations. 
		//also keeps things fairly easy to edit from once place

		public static string generateUID(){
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();

			var sim = "-" + new string(Enumerable.Repeat(chars, 12)
				.Select(s => s[random.Next(s.Length)]).ToArray());

			var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
			var result = timeSpan.TotalSeconds + sim;

			return result;
		}

		public static string[] helpOptions = {
			"Section 1 - QuickStart Guide",
			"Section 2 - Users and Login",
			"Section 3 - Event Creation and Login",
			"Section 4 - Map Editing",
			"Section 5 - Messaging and Alerts"
		};

		public static string[] messageTypes = {
			"Message", 
			"System Message"
		};

		public static string[] privLevels = {
			"No Access", 
			"User", 
			"Map Editor", 
			"Admin"
		};

		public static string[] uTypeOptions = {
			"Fire","Police", 
			"Hazmat",
			"EMS",
			"Triage",
			"Rescue",
			"Command Post",
			"Other"
		};

		public static string[] dzTypeOptions = {
			"Fire",
			"Biohazard", 
			"Gas",
			"Weather",
			"Other"
		};

		public static Color[] colorOptions = { 
			Color.FromRgba(125, 0, 0, 80),
			Color.FromRgba(255, 248, 61, 95),
			Color.FromRgba(32, 186, 35, 90),
			Color.FromRgba(27, 93, 186, 80),
			Color.FromRgba(178,178,178, 100)
		};
	}
}

