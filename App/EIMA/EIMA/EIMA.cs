using TK.CustomMap.Api.Google;
using Xamarin.Forms;

namespace EIMA
{
	public class App : Application
	{
		public App ()
		{
			DataManager.getInstance ().dataStartup ();
			GmsPlace.Init("AIzaSyDGcU1OGx-VAwqn4s5fCgcmHkJJeWQjTJs"); //debug/testing key
			GmsDirection.Init("AIzaSyDGcU1OGx-VAwqn4s5fCgcmHkJJeWQjTJs"); //debug/testing key
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

