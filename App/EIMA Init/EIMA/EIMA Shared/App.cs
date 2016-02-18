using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EIMAMaster
{
	public class App : Application
	{
		public App ()
		{
			new DataManager().setData();
			MainPage = new NavigationPage(new LoginPage ());
		}
		public static Page GetMainPage ()
		{	
			return new LoginPage ();
		}
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}