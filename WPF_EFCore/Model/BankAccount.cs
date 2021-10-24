using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;

namespace WPF_EFCore.Model
{
    public class BankAccount : INotifyPropertyChanged
    {
        public static event Action<string, string> EventAccount;


        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        private int amount;
        public int Amount
        {
            get { return amount; }
            set
            {
                if (value == 0) return;
                int sum = value - amount;
                amount = value;
                OnPropertyChanged("Amount");

                Client client = GetClientByAcc();
                EventAccount?.Invoke($"{client}", $"Пополнение счета: \"{this}\" на {sum}");

            }
        }


        private int clientId;
        public int ClientId
        {
            get { return clientId; }
            set
            {
                clientId = value;
                OnPropertyChanged("ClientId");
            }
        }


        public Client Client { get; set; }

        public override string ToString()
        {
            return $"Недип.счет.  Id:{this.Id}, Счет:{this.Amount}";
        }


        #region реализация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        protected Client GetClientByAcc()
        {
            Client client = new Client();
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Client> clients = db.Clients.ToList();
                foreach (var item in clients)
                {
                    if (item.Id == this.ClientId)
                    {
                        client = item;
                        break;
                    }
                }
            }

            return client;
        }

    }
}
