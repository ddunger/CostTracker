using CommunityToolkit.Maui.Views;

namespace Miljokaz.Views;

public partial class NewItem : ContentPage
{
	public NewItem()
	{
		InitializeComponent();
		this.BindingContext = App.SharedMainPageViewModel;

		

	}
	public void DisplayPopup(object sender, EventArgs e)
	{
		var popup = new PopupToSelectColor();

		this.ShowPopup(popup);
	}
}