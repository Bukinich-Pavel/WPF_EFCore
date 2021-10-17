using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WPF_EFCore.Model;

namespace WPF_EFCore.ViewModel
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        #region селекты

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
                ObservableCollection<BankAccount> AccTemp = new ObservableCollection<BankAccount>();
                foreach (var item in BankAccounts)
                {
                    if (item.ClientId == value.Id) AccTemp.Add(item);
                }
                BankAccountsView = AccTemp;
                OnPropertyChanged("SelectedClient");
            }
        }


        /// <summary>
        /// Выбранный счет
        /// </summary>
        private BankAccount selectedAccount;
        public BankAccount SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                selectedAccount = value;
                OnPropertyChanged("SelectedAccount");
            }
        }


        // Выбор счета снятия транзакции
        private BankAccount selectedAccFrom;
        public BankAccount SelectedAccFrom
        {
            get { return selectedAccFrom; }
            set
            {
                selectedAccFrom = value;
                OnPropertyChanged("SelectedAccFrom");

            }
        }


        // Выбор счета зачисления транзакции
        private BankAccount selectedAccTo;
        public BankAccount SelectedAccTo
        {
            get { return selectedAccTo; }
            set
            {
                selectedAccTo = value;
                OnPropertyChanged("SelectedAccTo");

            }
        }

        #endregion


        #region команды

        // Транзакция средств между счетами
        private RelayCommand comandTransactionMoney;
        public RelayCommand ComandTransactionMoney
        {
            get
            {
                return comandTransactionMoney ?? (comandTransactionMoney = new RelayCommand(obj =>
                {
                    if (SelectedAccFrom == null || SelectedAccTo == null) return;
                    TransactionMoney(SelectedAccFrom, SelectedAccTo, 50);

                }));
            }
        }


        // Закрытие счета
        private RelayCommand comandCloseAccount;
        public RelayCommand ComandCloseAccount
        {
            get
            {
                return comandCloseAccount ?? (comandCloseAccount = new RelayCommand(obj =>
                {
                    BankAccount bankAccount = obj as BankAccount;
                    if (bankAccount != null)
                    {
                        CloseAccount(bankAccount);
                    }
                    BankAccounts.Remove(bankAccount);

                    var temp = BankAccountsView;
                    temp.Remove(bankAccount);
                    BankAccountsView = new ObservableCollection<BankAccount>();
                    BankAccountsView = temp;
                }));
            }
        }


        // Открытие депозитного счета
        private RelayCommand comandOpenDeposAccount;
        public RelayCommand ComandOpenDeposAccount
        {
            get
            {
                return comandOpenDeposAccount ?? (comandOpenDeposAccount = new RelayCommand(obj =>
                {
                    Client client = obj as Client;
                    DeposBankAccount newAcc = new DeposBankAccount() { DepositRate = 1, ClientId = client.Id, Amount = 0 };

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        List<DeposBankAccount> deposAcc = db.DeposBankAccount.ToList();
                        foreach (var item in deposAcc)
                        {
                            if (item.ClientId == client.Id) return;
                        }
                        db.DeposBankAccount.Add(newAcc);
                        db.SaveChanges();
                    }

                    BankAccounts.Add(newAcc);

                    var temp = BankAccountsView;
                    temp.Add(newAcc);
                    BankAccountsView = new ObservableCollection<BankAccount>();
                    BankAccountsView = temp;
                }));
            }
        }


        // Открытие недепозитного счета
        private RelayCommand comandOpenDontDeposAccount;
        public RelayCommand ComandOpenDontDeposAccount
        {
            get
            {
                return comandOpenDontDeposAccount ?? (comandOpenDontDeposAccount = new RelayCommand(obj =>
                {
                    Client client = obj as Client;
                    DontDeposBankAccount newAcc = new DontDeposBankAccount() { ClientId = client.Id, Amount = 0 };

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        List<DontDeposBankAccount> dontdeposAcc = db.DontDeposBankAccount.ToList();
                        foreach (var item in dontdeposAcc)
                        {
                            if (item.ClientId == client.Id) return;
                        }
                        db.DontDeposBankAccount.Add(newAcc);
                        db.SaveChanges();
                    }

                    BankAccounts.Add(newAcc);

                    var temp = BankAccountsView;
                    temp.Add(newAcc);
                    BankAccountsView = new ObservableCollection<BankAccount>();
                    BankAccountsView = temp;
                }));
            }
        }


        #endregion



        /// <summary>
        /// Коллекция клиентов банка
        /// </summary>
        public List<Client> Clients { get; set; }

        /// <summary>
        /// Коллекция всех счетов банка
        /// </summary>
        public ObservableCollection<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Коллекция счетов банка отображенных в ListBox
        /// </summary>
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



        // Конструктор
        public ApplicationViewModel()
        {
            Clients = GetClientsFromDB(); //получаем всех клиентов из бд
            BankAccounts = GetBankAccountsFromDB(); //получаем все счета из бд
            BankAccountsView = BankAccounts; //отображаем все счета в ListBox
        }



        #region Private Method

        /// <summary>
        /// Возвращает клиентов из бд
        /// </summary>
        /// <returns></returns>
        private List<Client> GetClientsFromDB()
        {
            List<Client> clients;
            using (ApplicationContext db = new ApplicationContext())
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
                IEnumerable<BankAccount> depos = db.DeposBankAccount.ToList();
                IEnumerable<BankAccount> dontdepos = db.DontDeposBankAccount.ToList();

                //bankAccounts = depos.Union(dontdepos).ToList();

                foreach (var item in depos) { bankAccounts.Add(item); }
                foreach (var item in dontdepos) { bankAccounts.Add(item); }
            }
            return bankAccounts;
        }


        /// <summary>
        /// Транзакция между счетами
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="accFrom"></param>
        /// <param name="accTo"></param>
        /// <param name="sum"></param>
        private void TransactionMoney<T, U>(T accFrom, U accTo, int sum)
            where T : BankAccount
            where U : BankAccount
        {
            if (accFrom.Amount < sum) return;
            accFrom.Amount -= sum;
            accTo.Amount += sum;

            UpdateDbBankAccount(accFrom);
            UpdateDbBankAccount(accTo);
        }


        /// <summary>
        /// Закрытие счетов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="acc">счет</param>
        private void CloseAccount<T>(T acc)
            where T : BankAccount
        {
            if (acc == null) return;
            using (ApplicationContext db = new ApplicationContext())
            {
                if (acc is DeposBankAccount)
                {
                    db.DeposBankAccount.Remove(acc as DeposBankAccount);
                    db.SaveChanges();
                }
                else if (acc is DontDeposBankAccount)
                {
                    db.DontDeposBankAccount.Remove(acc as DontDeposBankAccount);
                    db.SaveChanges();
                }
            }
        }


        /// <summary>
        /// Обновляет счет в бд
        /// </summary>
        /// <param name="acc">Счет</param>
        private static void UpdateDbBankAccount(BankAccount acc)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (acc is DeposBankAccount)
                {
                    db.DeposBankAccount.Update(acc as DeposBankAccount);
                    db.SaveChanges();
                }
                else if (acc is DontDeposBankAccount)
                {
                    db.DontDeposBankAccount.Update(acc as DontDeposBankAccount);
                    db.SaveChanges();
                }
            }

        }


        private ObservableCollection<T> ConverterToObsColl<T>(List<T> ts)
        {
            ObservableCollection<T> r = new ObservableCollection<T>();
            foreach (var item in ts)
            {
                r.Add(item);
            }
            return r;
        }

        #endregion



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
