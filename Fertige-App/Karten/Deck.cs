using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.Karten
{
    public class Deck
    {
        // Liste von Karten im Deck
        protected List<Card> cards = new List<Card>();

        // Indexer, der den Zugriff auf eine Karte im Deck ermöglicht
        public Card this[int position] { get { return (Card)cards[position]; } }

        // Konstruktor für die Deck-Klasse
        public Deck()
        {
            // Erstellt ein neues Deck, indem alle Karten hinzugefügt werden
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue faceVal in Enum.GetValues(typeof(FaceValue)))
                {
                    cards.Add(new Card(suit, faceVal, true));
                }
            }
        }

        // Methode zum Ziehen einer Karte aus dem Deck
        public Card Draw()
        {
            // Entfernt und gibt die oberste Karte im Deck zurück
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        // Methode zum Mischen des Decks
        public void Shuffle()
        {
            // Erstellt einen Zufallszahlengenerator
            Random random = new Random();

            // Geht durch jede Karte im Deck und tauscht sie mit einer zufällig gewählten anderen Karte
            for (int i = 0; i < cards.Count; i++)
            {
                int index1 = i;
                int index2 = random.Next(cards.Count);
                SwapCard(index1, index2);
            }
        }

        // Methode zum Austauschen von zwei Karten im Deck
        private void SwapCard(int index1, int index2)
        {
            // Speichert die Karte an Index1 temporär, setzt die Karte an Index1 auf die Karte an Index2 und umgekehrt
            Card card = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = card;
        }
    }
}
