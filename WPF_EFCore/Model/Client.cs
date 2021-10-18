using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;
using WPF_EFCore.ViewModel;

namespace WPF_EFCore.Model
{
    public class Client : ITransaction<Client>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepAccId { get; set; }
        public int? DontDepAccId { get; set; }
        public List<DeposBankAccount> DeposBankAccount { get; set; }
        public List<DontDeposBankAccount> DontDeposBankAccount { get; set; }


        public override string ToString()
        {
            return $"{Name}";
        }

        /// <summary>
        /// Транзакция между клиентами
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sum"></param>
        public void Transaction(Client client, int sum)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var accounts = ApplicationViewModel.GetBankAccountsFromDB();
                ObservableCollection<BankAccount> accountsThisClient = new ObservableCollection<BankAccount>();
                ObservableCollection<BankAccount> accountsAnotherClient = new ObservableCollection<BankAccount>();
                foreach (var item in accounts)
                {
                    if (item.ClientId == this.Id) accountsThisClient.Add(item);
                    if (item.ClientId == client.Id) accountsAnotherClient.Add(item);
                }

                if (accountsThisClient == null || accountsAnotherClient == null) return;

                foreach (var item in accountsThisClient)
                {
                    if (item.Amount >= sum)
                    {
                        item.Amount -= sum;
                        accountsAnotherClient[0].Amount += sum;

                        UpdateDBBankAccount(db, item);
                        UpdateDBBankAccount(db, accountsAnotherClient[0]);

                        break;
                    }
                }


            }
        }

        /// <summary>
        /// Обновить счета в бд
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acc"></param>
        private static void UpdateDBBankAccount(ApplicationContext db, BankAccount acc)
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
}
