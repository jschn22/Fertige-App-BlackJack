using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.Karten
{

    // Enumeration, die die verschiedenen Symbole für die Kartensuiten definiert
public enum Suit
    {
        Diamonds, // Karo
        Spades,   // Pik
        Clubs,    // Kreuz
        Hearts    // Herz
    }

    // Enumeration, die die verschiedenen Werte für die Kartengesichter definiert
    public enum FaceValue
    {
        Two = 2,   // Zwei
        Three = 3, // Drei
        Four = 4,  // Vier
        Five = 5,  // Fünf
        Six = 6,   // Sechs
        Seven = 7, // Sieben
        Eight = 8, // Acht
        Nine = 9,  // Neun
        Ten = 10,  // Zehn
        Jack = 11, // Bube
        Queen = 12,// Dame
        King = 13, // König
        Ace = 14   // Ass
    }

    // Klasse, die eine einzelne Spielkarte darstellt
    public class Card
    {
        // Die Kartensuite
        private readonly Suit suit;

        // Der Wert der Karte
        private readonly FaceValue faceVal;

        // Gibt an, ob die Karte aufgedeckt ist oder nicht
        private bool isCardUp;

        // Eigenschaft, die die Kartensuite zurückgibt
        public Suit Suit { get { return suit; } }

        // Eigenschaft, die den Wert der Karte zurückgibt
        public FaceValue FaceVal { get { return faceVal; } }

        // Eigenschaft, die angibt, ob die Karte aufgedeckt ist oder nicht
        public bool IsCardUp { get { return isCardUp; } set { isCardUp = value; } }

        // Konstruktor für die Card-Klasse
        public Card(Suit suit, FaceValue faceVal, bool isCardUp)
        {
            // Initialisiert die Kartensuite, den Wert und den Status der Karte
            this.suit = suit;
            this.faceVal = faceVal;
            this.isCardUp = isCardUp;
        }

        // Überschreibt die ToString-Methode, um eine lesbare Darstellung der Karte zu liefern
        public override string ToString()
        {
            // Gibt den Wert und die Suite der Karte als lesbaren String zurück
            return "The " + faceVal.ToString() + " of " + suit.ToString();
        }
    }
}
