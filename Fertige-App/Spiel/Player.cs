using Schoen.BlackJAck.Karten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.Spiel
{
    public class Player
    {
        // Objekte zur Speicherung von Spielerinformationen
        private decimal guthaben; // Guthaben des Spielers
        private BlackJackHand hand; // Hand des Spielers im Spiel
        private decimal bet; // Betrag des vom Spieler platzierten Einsatzes
        private decimal einzahlen; // Betrag des vom Spieler Eingezahlten Betrag
        static int playerInitialBalance = 5000; // Anfangsguthaben für alle Spieler
        private int pushes; // Anzahl der Unentschieden des Spielers

        private Deck currentDeck; // Aktuelles Deck, das im Spiel verwendet wird
        private List<Card> cards = new List<Card>(); // Liste der Karten in der Hand des Spielers

        // Eigenschaft zum Abrufen und Festlegen des aktuellen Decks
        public Deck CurrentDeck { get { return currentDeck; } set { currentDeck = value; } }

        // Eigenschaft zum Abrufen der Hand des Spielers
        public BlackJackHand Hand { get { return hand; } }

        // Eigenschaft zum Abrufen und Festlegen des Einsatzbetrags
        public decimal Bet { get { return bet; } set { bet = value; } }
        public decimal Einzahlen { get { return einzahlen; } set { einzahlen = value; } }

        // Eigenschaft zum Abrufen und Festlegen des Spielerkontostands
        public decimal Guthaben { get { return guthaben; } set { guthaben = value; } }

        // Eigenschaft zum Abrufen und Festlegen der Anzahl der Unentschieden
        public int Push { get { return pushes; } set { pushes = value; } }

        // Eigenschaft zum Abrufen und Festlegen des Anfangsguthabens für alle Spieler
        public static int PlayerInitialBalance { get { return playerInitialBalance; } set { playerInitialBalance = value; } }

        // Standardkonstruktor
        public Player() : this(-1) { }

        // Konstruktor mit Parameter zum Festlegen des Anfangsguthabens
        public Player(int newGuthaben)
        {
            // Initialisiere die Hand des Spielers und das Guthaben
            this.hand = new BlackJackHand();
            this.guthaben = newGuthaben;
        }

        // Methode zum Erhöhen des Einsatzbetrags
        public void IncreaseBet(decimal amt)
        {
            // Überprüfe, ob der Spieler genügend Guthaben hat, um den Einsatz zu erhöhen
            if ((guthaben - (bet + amt)) >= 0)
            {
                // Erhöhe den Einsatzbetrag
                bet += amt;
            }
            else
            {
                // Wirf eine Ausnahme, wenn der Spieler nicht genügend Guthaben hat
                throw new Exception("Sie haben nicht genügend Geld, um diesen Einsatz zu platzieren.");
            }
        }

        


        // Methode zum Platzieren des Einsatzes
        public void PlaceBet()
        {
            // Überprüfe, ob der Spieler genügend Guthaben hat, um den Einsatz zu platzieren
            if ((guthaben - bet) >= 0)
            {
                // Ziehe den Einsatzbetrag vom Spielerkonto ab
                guthaben = guthaben - bet;
            }
            else
            {
                // Wirf eine Ausnahme, wenn der Spieler nicht genügend Guthaben hat
                throw new Exception("Sie haben nicht genügend Geld, um diesen Einsatz zu platzieren.");
            }
        }

        // Methode zum Erstellen einer neuen Hand für den Spieler
        public BlackJackHand NewHand()
        {
            // Erstelle eine neue Hand für den Spieler
            this.hand = new BlackJackHand();
            return this.hand;
        }

        // Methode zum Zurücksetzen des Einsatzbetrags
        public void ClearBet()
        {
            // Setze den Einsatzbetrag auf Null zurück
            bet = 0;
        }

        // Methode zum Überprüfen, ob der Spieler einen Blackjack hat
        public bool HasBlackJack()
        {
            // Überprüfe, ob die Summe der Hand des Spielers 21 ist
            if (hand.GetSumOfHand() == 21)
                return true;
            else return false;
        }

        // Methode zum Überprüfen, ob der Spieler überkauft hat
        public bool HasBust()
        {
            // Überprüfe, ob die Summe der Hand des Spielers größer als 21 ist
            if (hand.GetSumOfHand() > 21)
                return true;
            else return false;
        }

        // Methode für den Spieler, um eine Karte zu ziehen
        public void Hit()
        {
            // Ziehe eine Karte aus dem aktuellen Deck und füge sie der Hand des Spielers hinzu
            Card c = currentDeck.Draw();
            hand.Cards.Add(c);
        }
    }


    // Enumeration für die möglichen Ergebnisse eines Blackjack-Spiels
    public enum Ergebnis
    {
        DealerBlackJack, // Der Dealer hat einen Blackjack
        PlayerBlackJack, // Der Spieler hat einen Blackjack
        PlayerBust, // Der Spieler hat überkauft
        DealerBust, // Der Dealer hat überkauft
        Push, // Unentschieden
        PlayerWin, // Der Spieler gewinnt
        DealerWin // Der Dealer gewinnt
    }
}
