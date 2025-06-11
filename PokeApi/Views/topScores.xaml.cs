using PokeApi.ViewModels;

namespace PokeApi.Views;

public partial class topScores : ContentPage
{
	public topScores()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is topScoresVM vm)
        {
            await vm.recargarListado();
        }
    }
}