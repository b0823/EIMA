using System;

namespace EIMAMaster
{
	/**
	 * Outline for Items used in Yes/No listview. 
	 * Could potentially remove however this might require big changes.
	 */
	public class FilterObject
	{
		public string Name { get; set ; }
		public bool IsSelected { get; set ; }

		public FilterObject ()
		{
		}
	}
}

