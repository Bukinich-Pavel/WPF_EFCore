using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;
using WPF_EFCore.Model;

namespace WPF_EFCore
{
    class WorkBankAccount<T> 
        where T : BankAccount, new()
    {

        /// <summary>
        /// Закрытие счета
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="acc">счет</param>
        public static void CloseAccount(T acc)
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
        /// Открытие счета
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static T OpenAccount(Client client)
        {
            T newAcc = new T() { ClientId = client.Id };

            using (ApplicationContext db = new ApplicationContext())
            {
                Type type = newAcc.GetType();
                if (type.Name == "DeposBankAccount")
                {
                    List<DeposBankAccount> deposAcc = db.DeposBankAccount.ToList();

                    foreach (var item in deposAcc)
                    {
                        if (item.ClientId == client.Id) return null;
                    }

                    db.DeposBankAccount.Add(newAcc as DeposBankAccount);
                }
                else if (type.Name == "DontDeposBankAccount")
                {
                    List<DontDeposBankAccount> deposAcc = db.DontDeposBankAccount.ToList();

                    foreach (var item in deposAcc)
                    {
                        if (item.ClientId == client.Id) return null;
                    }

                    db.DontDeposBankAccount.Add(newAcc as DontDeposBankAccount);
                }
                db.SaveChanges();
            }
            return newAcc;
        }

    }
}
