using System;
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
                if (value == null) return;
                BankAccountsView = new ObservableCollection<BankAccount>();
                ObservableCollection<BankAccount> AccTemp = new ObservableCollection<BankAccount>();
                foreach (var item in BankAccounts)
                {
                    if (item.ClientId == value.Id) AccTemp.Add(item);
                }
                BankAccountsView = AccTemp;
                ForSelectTransaction = AccTemp;
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


        // Выбор клиента снятия транзакции
        private Client selectedClientFrom;
        public Client SelectedClientFrom
        {
            get { return selectedClientFrom; }
            set
            {
                selectedClientFrom = value;
                OnPropertyChanged("SelectedClientFrom");

            }
        }


        // Выбор клиента зачисления транзакции
        private Client selectedClientTo;
        public Client SelectedClientTo
        {
            get { return selectedClientTo; }
            set
            {
                selectedClientTo = value;
                OnPropertyChanged("SelectedClientTo");

            }
        }


        // Сумма пополнения
        private string sumReplenishment;
        public string SumReplenishment
        {
            get { return sumReplenishment; }
            set
            {
                sumReplenishment = value;
                OnPropertyChanged("SumReplenishment");
            }
        }


        // Сумма пополнения
        private string sumTransactionBetweenClients;
        public string SumTransactionBetweenClients
        {
            get { return sumTransactionBetweenClients; }
            set
            {
                sumTransactionBetweenClients = value;
                OnPropertyChanged("SumTransactionBetweenClients");
            }
        }


        // Сумма пополнения
        private string sumTransactionBetweenAccounts;
        public string SumTransactionBetweenAccounts
        {
            get { return sumTransactionBetweenAccounts; }
            set
            {
                sumTransactionBetweenAccounts = value;
                OnPropertyChanged("SumTransactionBetweenAccounts");
            }
        }


        // Имя нового клиента
        private string nameNewClient;
        public string NameNewClient
        {
            get { return nameNewClient; }
            set
            {
                nameNewClient = value;
                OnPropertyChanged("NameNewClient");
            }
        }


        #endregion


        
        #region Команды

        // Транзакция средств между счетами
        private RelayCommand comandTransactionMoney;
        public RelayCommand ComandTransactionMoney
        {
            get
            {
                return comandTransactionMoney ?? (comandTransactionMoney = new RelayCommand(obj =>
                {
                    int sum;
                    try
                    {
                        sum = Convert.ToInt32(SumTransactionBetweenAccounts);
                    }
                    catch 
                    {
                        return;
                    }
                    if (SelectedAccFrom == null || SelectedAccTo == null) return;
                    TransactionMoney(SelectedAccFrom, SelectedAccTo, sum);


                }));
            }
        }


        // Транзакция средств между клиентами
        private RelayCommand сomandTransactionBetweenClients;
        public RelayCommand ComandTransactionBetweenClients
        {
            get
            {
                return сomandTransactionBetweenClients ?? (сomandTransactionBetweenClients = new RelayCommand(obj =>
                {
                    int sum;
                    try
                    {
                        sum = Convert.ToInt32(SumTransactionBetweenClients);
                    }
                    catch 
                    {
                        return;
                    }


                    if (SelectedClientFrom == null || SelectedClientTo == null) return;
                    SelectedClientFrom.Transaction(SelectedClientTo, sum);

                    BankAccounts = GetBankAccountsFromDB(); 
                    var temp = this.BankAccountsView;
                    this.BankAccountsView = new ObservableCollection<BankAccount>();
                    this.BankAccountsView = BankAccounts;

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
                    if (obj == null) return;
                    BankAccount bankAccount = obj as BankAccount;
                    if (bankAccount != null)
                    {
                        WorkBankAccount<BankAccount>.CloseAccount(bankAccount);
                        //CloseAccount(bankAccount);
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
                    if (obj == null) return;
                    Client client = obj as Client;
                    DeposBankAccount newAcc = WorkBankAccount<DeposBankAccount>.OpenAccount(client);
                    if (newAcc == null) return;

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
                    if (obj == null) return;
                    Client client = obj as Client;
                    DontDeposBankAccount newAcc = WorkBankAccount<DontDeposBankAccount>.OpenAccount(client);
                    if (newAcc == null) return;

                    BankAccounts.Add(newAcc);

                    var temp = BankAccountsView;
                    temp.Add(newAcc);
                    BankAccountsView = new ObservableCollection<BankAccount>();
                    BankAccountsView = temp;
                }));
            }
        }


        // Пополнение счета
        private RelayCommand сomandReplenishmentAccount;
        public RelayCommand ComandReplenishmentAccount
        {
            get
            {
                return сomandReplenishmentAccount ?? (сomandReplenishmentAccount = new RelayCommand(obj =>
                {

                    int sum;
                    try
                    {
                        sum = Convert.ToInt32(SumReplenishment);
                    }
                    catch
                    {
                        return;
                    }

                    if (obj is DeposBankAccount)
                    {
                        DeposBankAccount account = obj as DeposBankAccount;
                        account.Replenishment(sum);
                        UpdateDbBankAccount(account);
                    }
                    else if (obj is DontDeposBankAccount)
                    {
                        DontDeposBankAccount account = obj as DontDeposBankAccount;
                        account.Replenishment(sum);
                        UpdateDbBankAccount(account);
                    }

                    

                }));
            }
        }


        // Добавление нового клиента
        private RelayCommand comandAddClient;
        public RelayCommand ComandAddClient
        {
            get
            {
                return comandAddClient ?? (comandAddClient = new RelayCommand(obj =>
                {
                    if (obj == null) return;
                    string name = obj as string;

                    Client newClient = new Client() { Name = name };

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.Clients.Add(newClient);
                        db.SaveChanges();
                    }
                    Clients.Add(newClient);
                }));
            }
        }


        // Удаление клиента
        private RelayCommand commandDeleteClient;
        public RelayCommand CommandDeleteClient
        {
            get
            {
                return commandDeleteClient ?? (commandDeleteClient = new RelayCommand(obj =>
                {
                    if (obj == null) return;
                    Client client = obj as Client;

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.Clients.Remove(client);
                        db.SaveChanges();
                    }
                    Clients.Remove(client);
                }));
            }
        }



        #endregion



        /// <summary>
        /// Коллекция клиентов банка
        /// </summary>
        private ObservableCollection<Client> сlients;
        public ObservableCollection<Client> Clients
        {
            get { return сlients; }
            set
            {
                сlients = value;
                OnPropertyChanged("Clients");
            }
        }

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


        private ObservableCollection<BankAccount> forSelectTransaction;
        public ObservableCollection<BankAccount> ForSelectTransaction
        {
            get { return forSelectTransaction; }
            set
            {
                forSelectTransaction = value;
                OnPropertyChanged("ForSelectTransaction");
            }
        }





        // Конструктор
        public ApplicationViewModel()
        {
            Clients = GetClientsFromDB(); //получаем всех клиентов из бд
            BankAccounts = GetBankAccountsFromDB(); //получаем все счета из бд
            BankAccountsView = BankAccounts; //отображаем все счета в ListBox
            ForSelectTransaction = new ObservableCollection<BankAccount>();
        }



        #region Private Method


        /// <summary>
        /// Возвращает клиентов из бд
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Client> GetClientsFromDB()
        {
            ObservableCollection<Client> clients;
            List<Client> temp;
            using (ApplicationContext db = new ApplicationContext())
            {
                temp = db.Clients.ToList();
            }
            clients = new ObservableCollection<Client>();
            foreach (var item in temp)
            {
                clients.Add(item);
            }

            return clients;
        }


        /// <summary>
        /// Возвращает счета из бд
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<BankAccount> GetBankAccountsFromDB()
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
        /// Обновляет счет в бд
        /// </summary>
        /// <param name="acc">Счет</param>
        private void UpdateDbBankAccount(BankAccount acc)
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

            var temp = this.BankAccountsView;
            this.BankAccountsView = new ObservableCollection<BankAccount>();
            this.BankAccountsView = temp;

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
