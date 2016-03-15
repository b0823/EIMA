using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{
	public class AdminPage : ContentPage
	{
		public AdminPage ()
		{
			Title = "Administration";

			//User Data
			var data = DataManager.getInstance ();

			List<EIMAUser> userData = data.getUsers ();
		}
	}
}

