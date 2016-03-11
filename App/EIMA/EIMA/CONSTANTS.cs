using Xamarin.Forms;

namespace EIMA
{
	public static class CONSTANTS
	{
		//Universal constants go here. Most useful for lists used in various locations.
		public static string[] uTypeOptions = {"Fire","Police", "Hazmat","EMS","Triage","Rescue","Command Post","Other"};
		public static string[] dzTypeOptions = {"Fire","Biohazard", "Gas","Weather","Other"};
		public static Color[] colorOptions = { Color.FromRgba(125, 0, 0, 80),Color.FromRgba(255, 248, 61, 95)
			,Color.FromRgba(32, 186, 35, 90),Color.FromRgba(27, 93, 186, 80),Color.FromRgba(178,178,178, 100)};
	}
}

