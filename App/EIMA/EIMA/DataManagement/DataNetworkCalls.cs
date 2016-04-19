using System;

namespace EIMA
{
	public static class DataNetworkCalls
	{
		public static void updateNetworkedData(){
			var data = DataManager.getInstance();

			if (data.isNoAccess () || data.isStandAlone ()) {
				return;
			}






			if (data.isAdmin ()) {
			
			
			}




		}

		public static void sendMessage(){}
		public static void setPrivlege(){}

	}
}

