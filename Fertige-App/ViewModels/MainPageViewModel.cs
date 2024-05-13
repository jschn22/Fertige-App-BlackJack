using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Schoen.BlackJAck.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        // Die Gesamtpunktzahl des Spielers.
        int playerTotal;

        // Die Gesamtpunktzahl des Dealers.
        int dealerTotal;

        // Eigenschaft, die die Gesamtpunktzahl des Spielers darstellt.
        public int PlayerTotal
        {
            get { return playerTotal; }
            set { SetProperty(ref playerTotal, value); } // Ruft die SetProperty-Methode auf, um die Eigenschaft zu setzen.
        }

        // Eigenschaft, die die Gesamtpunktzahl des Dealers darstellt.
        public int DealerTotal
        {
            get { return dealerTotal; }
            set { SetProperty(ref dealerTotal, value); } // Ruft die SetProperty-Methode auf, um die Eigenschaft zu setzen.
        }

        int guthabenGes;
        public int GuthabenGes
        {
            get { return guthabenGes; }
            set { SetProperty(ref guthabenGes, value); }
        }
        
        

        int myBets;
        public int MyBet
        {
            get { return myBets; }
            set { SetProperty(ref myBets, value); }
        }

        // Ein Flag, das anzeigt, ob das ViewModel beschäftigt ist.
        bool isBusy = false;

        // Eigenschaft, die anzeigt, ob das ViewModel beschäftigt ist.
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); } // Ruft die SetProperty-Methode auf, um die Eigenschaft zu setzen.
        }

        // Eine generische Methode zum Setzen von Eigenschaften, die das INotifyPropertyChanged-Event auslöst.
        protected bool SetProperty<T>(ref T backingStore,
            T value, [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            // Überprüft, ob der Wert geändert wurde.
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            // Setzt den neuen Wert und ruft die onChanged-Aktion auf.
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName); // Ruft die Methode zur Benachrichtigung über eine Eigenschaftsänderung auf.
            return true;
        }

        // Ereignis, das ausgelöst wird, wenn eine Eigenschaft geändert wird.
        public event PropertyChangedEventHandler PropertyChanged;

        // Methode zum Auslösen des PropertyChanged-Ereignisses.
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
