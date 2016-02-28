using Xamarin.Forms;
using System.Collections.Generic;

namespace EIMA
{

	public class MenuListView : ListView
	{
		public MenuListView ()
		{
			List<MenuItem> data = new MenuListData ();

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate (typeof(MenuCell));
			cell.SetBinding (TextCell.TextProperty, "Title");
			cell.SetBinding (ImageCell.ImageSourceProperty, "IconSource");

			ItemTemplate = cell;
		}
	}
}