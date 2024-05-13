using Schoen.BlackJAck.Spiel;
using Schoen.BlackJAck.ViewModels;


namespace Schoen.BlackJAck;

public partial class StartPage : ContentPage
{
    private MainPage mainPage = new MainPage();
    private MainPageViewModel mainPageViewModel = new MainPageViewModel();
    public BlackJack game = new BlackJack(Player.PlayerInitialBalance);

    public StartPage()
	{
		InitializeComponent();
	}
    private async void btnStartPage_Clicked(object sender, EventArgs e)
    {
        mainPageViewModel.GuthabenGes = (int)game.CurrentPlayer.Guthaben;
        
        await Navigation.PushAsync(new MainPage());
        



    }
}