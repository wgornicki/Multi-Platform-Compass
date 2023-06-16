namespace Multi_Platform_Compass;

using Microsoft.Maui.Controls.Maps;

public partial class MapPage : ContentPage
{
	public MapPage()
	{
		InitializeComponent();
		map = new Map();
	}
}