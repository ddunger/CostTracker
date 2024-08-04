using CommunityToolkit.Maui.Views;

namespace Miljokaz.Views;

public partial class PopupToSelectColor : Popup
{
	public PopupToSelectColor()
	{
		InitializeComponent();
		this.BindingContext = App.SharedMainPageViewModel;

	}

	private void OnItemTapped(object sender, ItemTappedEventArgs e)
	{
		if (e.Item != null)
		{
			Close();
		}
	}
}