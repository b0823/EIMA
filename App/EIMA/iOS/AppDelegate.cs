using Foundation;
using TK.CustomMap.iOSUnified;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace EIMA.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init();
			TKCustomMapRenderer.InitMapRenderer();
			NativePlacesApi.Init();
			FormsMaps.Init();
			LoadApplication(new App());

			return base.FinishedLaunching (app, options);
		}
	}
}

