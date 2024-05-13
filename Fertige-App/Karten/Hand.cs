using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.Karten
{
    public class Hand
    {
        // Erstellt eine Liste von Karten
        protected List<Card> cards = new List<Card>();

        // Gibt die Anzahl der Karten in der Hand zurück
        public int NumCards { get { return cards.Count; } }

        // Gibt die Liste der Karten in der Hand zurück
        public List<Card> Cards { get { return cards; } }

        // Überprüft, ob eine bestimmte Karte in der Hand enthalten ist
        public bool ContainsCard(FaceValue item)
        {
            foreach (Card c in cards)
            {
                if (c.FaceVal == item)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class BlackJackHand : Hand
    {
        // Vergleicht die Werte zweier Blackjack-Hände
        public int CompareFaceValue(object otherHand)
        {
            BlackJackHand aHand = otherHand as BlackJackHand;
            if (aHand != null)
            {
                return this.GetSumOfHand().CompareTo(aHand.GetSumOfHand());
            }
            else
            {
                throw new ArgumentException("Argument ist keine Hand");
            }
        }

        // Berechnet die Summe der Werte der Karten in der Hand
        public int GetSumOfHand()
        {
            int val = 0;
            int numAces = 0;

            foreach (Card c in cards)
            {
                if (c.FaceVal == FaceValue.Ace)
                {
                    numAces++;
                    val += 11;
                }
                else if (c.FaceVal == FaceValue.Jack || c.FaceVal == FaceValue.Queen || c.FaceVal == FaceValue.King)
                {
                    val += 10;
                }
                else
                {
                    val += (int)c.FaceVal;
                }
            }

            while (val > 21 && numAces > 0)
            {
                val -= 10;
                numAces--;
            }

            return val;
        }
    }

}
