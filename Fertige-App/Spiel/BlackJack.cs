using Schoen.BlackJAck.Karten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.Spiel
{
    public class BlackJack
    {
        // Das private Deck und die Spieler für das aktuelle Deck vom Dealer und vom Spieler.
        private Deck deck;
        private Player dealer;
        private Player player;

        // Eigenschaft, die den aktuellen Spieler zurückgibt.
        public Player CurrentPlayer { get { return player; } }

        // Eigenschaft, die den Dealer zurückgibt.
        public Player Dealer { get { return dealer; } }

        // Eigenschaft, die das aktuelle Deck zurückgibt.
        public Deck CurrentDeck { get { return deck; } }

        // Konstruktor für die BlackJack-Klasse.
       
        public BlackJack(int startGuthaben)
        {
            // Initialisiert den Dealer und den Spieler.
            dealer = new Player();
            player = new Player(startGuthaben);
        }

        // Methode zum Austeilen eines neuen Spiels.
        public void DealNewGame()
        {
            // Erstellt und mischt ein neues Deck.
            deck = new Deck();
            deck.Shuffle();

            // Setzt die Hände des Spielers und des Dealers zurück, falls dies nicht das erste Spiel ist.
            player.NewHand();
            dealer.NewHand();

            // Teilt jedem Spieler zwei Karten aus.
            for (int i = 0; i < 2; i++)
            {
                Card c = deck.Draw();
                player.Hand.Cards.Add(c);

                Card d = deck.Draw();
                // Die zweite Karte des Dealers wird verdeckt ausgeteilt.
                if (i == 1)
                    d.IsCardUp = false;

                dealer.Hand.Cards.Add(d);
            }

            // Gibt dem Spieler und dem Dealer Zugriff auf das aktuelle Deck.
            player.CurrentDeck = deck;
            dealer.CurrentDeck = deck;
        }

        // Methode, die das Spiel des Dealers steuert.
        public void DealerPlay()
        {
            // Die zweite Karte des Dealers wird aufgedeckt.
            dealer.Hand.Cards[1].IsCardUp = true;

            // Überprüft, ob der Dealer eine Hand hat, die kleiner als 17 ist.
            // Wenn ja, muss der Dealer weiter Karten ziehen, bis seine Hand größer oder gleich 17 ist.
            if (dealer.Hand.GetSumOfHand() < 17)
            {
                dealer.Hit();
                DealerPlay();
            }
        }
    }
}
