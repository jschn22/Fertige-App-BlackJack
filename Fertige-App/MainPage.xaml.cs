
using Schoen.BlackJAck.ViewModels;
using Schoen.BlackJAck.Spiel;
using Schoen.BlackJAck.Karten;


namespace Schoen.BlackJAck;

public partial class MainPage : ContentPage

{
    // Pfad zur Datei, in der die Notizen gespeichert werden
    private string guthabenPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "guthaben.txt");

    // Das BlackJack-Spielobjekt
    public BlackJack game = new BlackJack(Player.PlayerInitialBalance);

    // Das ViewModel für die MainPage
    private MainPageViewModel mainPageViewModel = new MainPageViewModel();
    

    // Arrays für die Spieler- und Dealer-Karten
    private Image[] playerCards;
    private Image[] dealerCards;

    // Konstruktor der MainPage-Klasse
    public MainPage()
    {

        InitializeComponent();

        //ShowBankValue();
        //OnAppearing();
        LoadFile();
        

        // Lädt die Bilddateien für die Spielkarten
        LoadCardSkinImages();

        // Setzt ein neues Spiel auf
        SetUpNewGame();

        // Legt das Binding-Kontext auf das MainPageViewModel fest
        BindingContext = mainPageViewModel;

        




    }

    // Methode zum Einrichten eines neuen Spiels
    private void SetUpNewGame()
    {
        // Start-Button aktivieren, Stand- und Hit-Buttons deaktivieren, Labels unsichtbar machen
        Start.IsEnabled = true;
        Stand.IsEnabled = false;
        Hit.IsEnabled = false;
        btnClear.IsEnabled = true;

        btn10.IsEnabled = true;
        btn25.IsEnabled = true;
        btn50.IsEnabled = true;
        btn100.IsEnabled = true;
        lblPlayerTotal.IsVisible = false;
        lblDealerTotal.IsVisible = false;
        
        ShowBankValue();
    }

    // Methode zum Anzeigen des Bankwerts im UI
    public void ShowBankValue()
    {

        mainPageViewModel.GuthabenGes = (int)game.CurrentPlayer.Guthaben;
        
        mainPageViewModel.MyBet = (int)game.CurrentPlayer.Bet;
    }

    // Methode zum Laden der Bilder für die Spielkarten
    private void LoadCardSkinImages()
    {
        try
        {
            // Initialisiert Arrays für Spieler- und Dealer-Karten
            playerCards = new Image[] { playerCard1, playerCard2, playerCard3, playerCard4, playerCard5, playerCard6 };
            dealerCards = new Image[] { dealerCard1, dealerCard2, dealerCard3, dealerCard4, dealerCard5, dealerCard6 };

            // Setzt alle Kartenbilder vorerst unsichtbar
            foreach (var card in playerCards)
            {
                card.IsVisible = false;
            }

            foreach (var card in dealerCards)
            {
                card.IsVisible = false;
            }
        }
        catch (OutOfMemoryException)
        {
            // Behandelt den Fall, dass die Bilddateien nicht geladen werden können
            DisplayAlertAndExit("Fehler beim Laden der Kartenbildern. Stelle sicher, dass die Bilddateien an der richtigen Stelle sind.");
        }
    }

    // Event-Handler, der aufgerufen wird, wenn der "Hit" Button geklickt wird
    private void OnHitClicked(object sender, EventArgs e)
    {
        // Spieler nimmt eine weitere Karte und aktualisiert das UI
        game.CurrentPlayer.Hit();
        UpdateUIPlayerCards();

        // Überprüfe, ob der Spieler überkauft hat
        if (game.CurrentPlayer.HasBust())
        {
            EndGame(Ergebnis.PlayerBust);
        }
    }



    // Event-Handler, der aufgerufen wird, wenn der "Stand" Button geklickt wird
    private void OnStandClicked(object sender, EventArgs e)
    {
        // Der Dealer beendet sein Spiel und das UI wird aktualisiert
        game.DealerPlay();
        UpdateUIPlayerCards();
        

        // Überprüfe, wer das Spiel gewonnen hat
        EndGame(GetGameResult());
    }

    // Event-Handler, der aufgerufen wird, wenn der "Start" Button geklickt wird
    private async void OnStartClicked(object sender, EventArgs e)
    {
        try
        {
            // Wenn der aktuelle Einsatz gleich 0 ist, fordere den Spieler auf, einen Einsatz zu platzieren
            if ((game.CurrentPlayer.Bet == 0) && (game.CurrentPlayer.Guthaben > 0))
            {
                await DisplayAlert("Hinweis", "Sie müssen einen Einsatz platzieren, bevor der Dealer austeilt.", "OK");
            }
            else
            {
                // Platziere den Einsatz
                game.CurrentPlayer.PlaceBet();
                ShowBankValue();

                // Lösche den Tisch, richte das UI für ein neues Spiel ein und teile ein neues Spiel aus
                ClearTable();
                SetUpGameInPlay();
                game.DealNewGame();
                UpdateUIPlayerCards();

                // Überprüfe, ob der aktuelle Spieler einen Blackjack hat
                if (game.CurrentPlayer.HasBlackJack())
                {
                    EndGame(Ergebnis.PlayerBlackJack);
                }
            }
        }
        catch (Exception NotEnoughMoneyException)
        {
            DisplayAlertAndExit(NotEnoughMoneyException.Message);
        }
    }

    // Methode zur Ermittlung des Spielstands
    private Ergebnis GetGameResult()
    {
        Ergebnis endState;

        // Überprüfe auf Blackjack
        if (game.Dealer.Hand.NumCards == 2 && game.Dealer.HasBlackJack())
        {
            endState = Ergebnis.DealerBlackJack;
        }
        // Überprüfe, ob der Dealer überkauft hat
        else if (game.Dealer.HasBust())
        {
            endState = Ergebnis.DealerBust;
        }
        else if (game.Dealer.Hand.CompareFaceValue(game.CurrentPlayer.Hand) > 0)
        {
            // Dealer gewinnt
            endState = Ergebnis.DealerWin;
        }
        else if (game.Dealer.Hand.CompareFaceValue(game.CurrentPlayer.Hand) == 0)
        {
            // Push
            endState = Ergebnis.Push;
        }
        else
        {
            // Spieler gewinnt
            endState = Ergebnis.PlayerWin;
        }
        return endState;
        
    }

    // Methode zum Beenden des Spiels
    private async void EndGame(Ergebnis endState)
    {
        lblDealerTotal.IsVisible = true;
        lblErgebnis.Opacity = 100;

        // Je nach Ergebnis wird eine entsprechende Nachricht angezeigt
        switch (endState)
        {
            case Ergebnis.DealerBust:
                lblErgebnis.Text = "Gewonnen, Dealer hat sich überkauft!";
                game.CurrentPlayer.Guthaben += (game.CurrentPlayer.Bet * 2);
                break;
            case Ergebnis.DealerBlackJack:
                lblErgebnis.Text = "Verloren, Dealer BlackJack!";
                break;
            case Ergebnis.DealerWin:
                lblErgebnis.Text = "Dealer hat gewonnen!";
                break;
            case Ergebnis.PlayerBlackJack:
                lblErgebnis.Text = "Gewonnen, BlackJack!";
                game.CurrentPlayer.Guthaben += (game.CurrentPlayer.Bet * (decimal)2.5);
                break;
            case Ergebnis.PlayerBust:
                lblErgebnis.Text = "Du hast dich überkauft!";
                break;
            case Ergebnis.PlayerWin:
                lblErgebnis.Text = "Gewonnen!";
                game.CurrentPlayer.Guthaben += (game.CurrentPlayer.Bet * 2);
                break;
            case Ergebnis.Push:
                lblErgebnis.Text = "Gleichstand";
                game.CurrentPlayer.Guthaben += game.CurrentPlayer.Bet;
                break;
        }

        SaveFile();
        // Spiel wird zurückgesetzt und Labels werden sichtbar gemacht
        SetUpNewGame();
        ShowBankValue();
        lblDealerTotal.IsVisible = true;
        lblPlayerTotal.IsVisible = true;

        // Überprüfe, ob das Spielerkonto leer ist
        if (game.CurrentPlayer.Guthaben == 0)
        {
            
            await DisplayAlert("Ihr Guthaben ist aufgebraucht!", "Bitte zahlen Sie erneut ein, um spielen zu können.", "OK");
            //await Shell.Current.Navigation.PopAsync();
        }
        
    }


    // Aktualisiert das UI der Spielerkarten und des Gesamtwerts der Hand
    private void UpdateUIPlayerCards()
    {
        // Aktualisiere den Wert der Spielerhand
        mainPageViewModel.PlayerTotal = game.CurrentPlayer.Hand.GetSumOfHand();
        mainPageViewModel.DealerTotal = game.Dealer.Hand.GetSumOfHand();

        // Aktualisiere die Spielerkarten
        List<Card> pcards = game.CurrentPlayer.Hand.Cards;
        for (int i = 0; i < pcards.Count; i++)
        {
            // Lade jedes Bild für die Karte aus der Datei
            LoadCard(playerCards[i], pcards[i]);
            playerCards[i].IsVisible = true;
        }

        // Aktualisiere die Dealerkarten
        List<Card> dcards = game.Dealer.Hand.Cards;
        for (int i = 0; i < dcards.Count; i++)
        {
            LoadCard(dealerCards[i], dcards[i]);
            dealerCards[i].IsVisible = true;
        }
    }

    // Lädt das Bild für eine Spielkarte basierend auf deren Farbe und Wert
    private void LoadCard(Image image, Card card)
    {
        ShowBankValue();
        try
        {
            string imageSource = string.Empty;

            switch (card.Suit)
            {
                case Suit.Diamonds:
                    imageSource += "diamonds_";
                    break;
                case Suit.Hearts:
                    imageSource += "hearts_";
                    break;
                case Suit.Spades:
                    imageSource += "spades_";
                    break;
                case Suit.Clubs:
                    imageSource += "clubs_";
                    break;
            }

            switch (card.FaceVal)
            {
                case FaceValue.Ace:
                    imageSource += "ace";
                    break;
                case FaceValue.King:
                    imageSource += "king";
                    break;
                case FaceValue.Queen:
                    imageSource += "queen";
                    break;
                case FaceValue.Jack:
                    imageSource += "jack";
                    break;
                case FaceValue.Ten:
                    imageSource += "10";
                    break;
                case FaceValue.Nine:
                    imageSource += "9";
                    break;
                case FaceValue.Eight:
                    imageSource += "8";
                    break;
                case FaceValue.Seven:
                    imageSource += "7";
                    break;
                case FaceValue.Six:
                    imageSource += "6";
                    break;
                case FaceValue.Five:
                    imageSource += "5";
                    break;
                case FaceValue.Four:
                    imageSource += "4";
                    break;
                case FaceValue.Three:
                    imageSource += "3";
                    break;
                case FaceValue.Two:
                    imageSource += "2";
                    break;
            }

            imageSource += ".png";

            // Überprüfe, ob die 2. Dealer Karte verdeckt ist
            if (!card.IsCardUp)
            {
                imageSource = "red2.png";
            }

            image.Source = imageSource;
        }
        catch (ArgumentOutOfRangeException)
        {
            DisplayAlertAndExit("Fehler beim Laden der Kartenbilder. Stelle sicher, dass alle Kartenbilder am richtigen Ort sind.");
        }
    }

    // Bereitet das UI für ein laufendes Spiel vor
    private void SetUpGameInPlay()
    {
        // Aktiviere die Stand- und Hit-Buttons, deaktiviere den Start-Button
        Stand.IsEnabled = true;
        Hit.IsEnabled = true;
        Start.IsEnabled = false;
        btn10.IsEnabled = false;
        btn25.IsEnabled = false;
        btn50.IsEnabled = false;
        btn100.IsEnabled = false;
        btnClear.IsEnabled = false;

        // Setze die Sichtbarkeit der Ergebnis- und Hand-Wert-Labels
        lblErgebnis.Opacity = 0; // Unsichtbar
        lblPlayerTotal.IsVisible = true;
        lblDealerTotal.IsVisible = false;
    }

    // Löscht den Tisch, indem alle Kartenbilder entfernt werden
    private void ClearTable()
    {
        for (int i = 0; i < 6; i++)
        {
            dealerCards[i].Source = null;
            dealerCards[i].IsVisible = false;

            playerCards[i].Source = null;
            playerCards[i].IsVisible = false;
        }
    }

    // Zeigt eine Fehlermeldung an und beendet die Seite
    private async void DisplayAlertAndExit(string message)
    {
        await DisplayAlert("Fehler", message, "OK");
        //await Shell.Current.Navigation.PopAsync();
    }

    // Event-Handler für das Klicken des "Clear" Buttons
    private void btnClear_Clicked(object sender, EventArgs e)
    {
        // Lösche den Einsatzbetrag
        game.CurrentPlayer.ClearBet();
        ShowBankValue();
    }

    // Event-Handler für das Klicken der Buttons für Einsätze
    private void Bet(int betValue)
    {
        try
        {
            // Aktualisiere den Einsatzbetrag
            game.CurrentPlayer.IncreaseBet(betValue);

            // Aktualisiere die "My Bet" und "My Account" Werte
            ShowBankValue();
        }
        catch (Exception NotEnoughMoneyException)
        {
            DisplayAlertAndExit(NotEnoughMoneyException.Message);
        }
    }

    // Methode für das Klicken des "10" Chips
    private void btn10_Clicked(object sender, EventArgs e)
    {
        Bet(10);
    }

    // Methode für das Klicken des "25" Chips
    private void btn25_Clicked(object sender, EventArgs e)
    {
        Bet(25);
    }

    // Methode für das Klicken des "50" Chips
    private void btn50_Clicked(object sender, EventArgs e)
    {
        Bet(50);
    }

    // Methode für das Klicken des "100" Chips
    private void btn100_Clicked(object sender, EventArgs e)
    {
        Bet(100);
        
    }
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {






        await Navigation.PushAsync(new SecondPage());
        

    }
    //Tut das Guthaben auf den letztrigen Wert
    public async void LoadFile()
    {
        if (File.Exists(guthabenPath))
        {
            using (StreamReader leser = new StreamReader(guthabenPath))
            {
                string text = await leser.ReadToEndAsync();
                game.CurrentPlayer.Guthaben = decimal.Parse(text);
            }
        }
        if(game.CurrentPlayer.Guthaben == 0)
        {
            game.CurrentPlayer.Guthaben = 1000;
        }
        
    }
    //Schreibt das Guthaben auf den letztrigen Wert
    public async void SaveFile()
    {
        using (StreamWriter schreiber = new StreamWriter(guthabenPath))
        {
            await schreiber.WriteAsync(game.CurrentPlayer.Guthaben.ToString());
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Überprüfen Sie, ob der gespeicherte Wert vorhanden ist
        if (Preferences.ContainsKey("EnteredText"))
        {
            // Fügen Sie den gespeicherten Wert zum bestehenden Text hinzu
           //decimal enteredText = decimal.Parse(Preferences.Get("EnteredText",""));
           //game.CurrentPlayer.Guthaben += enteredText;
            if (Preferences.ContainsKey("EnteredText") && !string.IsNullOrWhiteSpace(Preferences.Get("EnteredText", "")))
            {
                // Versuchen Sie, den gespeicherten Text in ein Decimal umzuwandeln
                if (decimal.TryParse(Preferences.Get("EnteredText", ""), out decimal enteredDecimal))
                {
                    game.CurrentPlayer.Guthaben += enteredDecimal;
                    ShowBankValue();
                }
                else
                {
                    
                }
            }
        }
        
        
    }

    
    







}