using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Model;

namespace WPF_EFCore.ViewModel
{
    class ApplicationViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Выбранный клиент
        /// </summary>
        private Client selectedClient;
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;

                BankAccountsView = new ObservableCollection<BankAccount>();
                foreach (var item in BankAccounts)
                {
                    if (item.ClientId == value.Id) BankAccountsView.Add(item);
                }

                OnPropertyChanged("SelectedClient");
            }
        }


        /// <summary>
        /// Выбранный счет
        /// </summary>
        private Client selectedBankAccount;
        public Client SelectedBankAccount
        {
            get { return selectedBankAccount; }
            set
            {
                selectedBankAccount = value;
                OnPropertyChanged("SelectedBankAccount");
            }
        }


        public List<Client> Clients { get; set; }


        public ObservableCollection<BankAccount> BankAccounts { get; set; }


        private ObservableCollection<BankAccount> bankAccountsView;
        public ObservableCollection<BankAccount> BankAccountsView
        {
            get { return bankAccountsView; }
            set
            {
                bankAccountsView = value;
                OnPropertyChanged("BankAccountsView");
            }
        }



        public ApplicationViewModel()
        {
            Clients = GetClientsFromDB();
            BankAccounts = GetBankAccountsFromDB();
            BankAccountsView = BankAccounts;
        }



        /// <summary>
        /// Возвращает клиентов из бд
        /// </summary>
        /// <returns></returns>
        private List<Client> GetClientsFromDB()
        {
            List<Client> clients;
            using(ApplicationContext db = new ApplicationContext())
            {
                clients = db.Clients.ToList();
            }
            return clients;
        }


        /// <summary>
        /// Возвращает счета из бд
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<BankAccount> GetBankAccountsFromDB()
        {
            ObservableCollection<BankAccount> bankAccounts = new ObservableCollection<BankAccount>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var depos = db.DeposBankAccount.ToList();
                var dontdepos = db.DontDeposBankAccount.ToList();

                foreach (var item in depos) { bankAccounts.Add(item); }

                foreach (var item in dontdepos) { bankAccounts.Add(item); }
            }
            return bankAccounts;
        }


        #region реализация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion


    }
}
