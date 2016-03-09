using Xamarin.Forms;

namespace EIMA
{
	public static class CONSTANTS
	{
		//Universal constants go here. Most useful for lists used in various locations.
		public static string[] uTypeOptions = {"Fire","Police", "Hazmat","EMS","Triage","Rescue","Command Post","Other"};
		public static string[] dzTypeOptions = {"Fire","Biohazard", "Gas","Weather","Other"};
		public static Color[] colorOptions = { Color.FromRgba(100, 0, 0, 80),Color.FromRgba(88, 96, 0, 80)
			,Color.FromRgba(6, 96, 0, 80),Color.FromRgba(0, 77, 96, 80),Color.FromRgba(190,190,190, 80)};
	}
}

