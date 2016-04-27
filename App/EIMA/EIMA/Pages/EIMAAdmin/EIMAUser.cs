using System;

namespace EIMA
{
	public class EIMAUser
	{
		public string username;
		public string name;
		public string unit;
		public string unitType;
		public string organization;
		public string status;
		public string level;

		public override string ToString()
		{
			return username + " " + name;
		}
	}
}

