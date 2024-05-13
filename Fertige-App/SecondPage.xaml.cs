using Schoen.BlackJAck.ViewModels;



namespace Schoen.BlackJAck;

public partial class SecondPage : ContentPage
{
    public  decimal einzahlWert;
    
    public SecondPage()
	{
		InitializeComponent();
        

    }

    
    private async void btnEinzahlen_Clicked(object sender, EventArgs e)
    {
        // Speichern Sie den eingegebenen Text in den Anwendungseigenschaften
        Preferences.Set("EnteredText", entryAdd.Text);

        // Navigieren Sie zurück zur MainPage
        await Navigation.PopAsync();



    }
}
