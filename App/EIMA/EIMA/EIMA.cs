using TK.CustomMap.Api.Google;
using Xamarin.Forms;
using System;

namespace EIMA
{
	public class App : Application
	{
		public App ()
		{
			DataManager.getInstance ().dataStartup ();
			GmsPlace.Init("AIzaSyDGcU1OGx-VAwqn4s5fCgcmHkJJeWQjTJs"); //debug/testing key
			GmsDirection.Init("AIzaSyDGcU1OGx-VAwqn4s5fCgcmHkJJeWQjTJs"); //debug/testing key


			if (String.IsNullOrEmpty (DataManager.getInstance ().getSecret ())) {
				MainPage = new NavigationPage(new LoginPage ());
			} 
			else {
				MainPage = new NavigationPage(new IncidentPage ());
			}

		}
		public static Page GetMainPage ()
		{	
			if (String.IsNullOrEmpty (DataManager.getInstance ().getSecret ())) {
				return new LoginPage ();
			} else {
				return new IncidentPage ();
			}
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

