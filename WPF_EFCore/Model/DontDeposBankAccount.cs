using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;

namespace WPF_EFCore.Model
{
    public class DontDeposBankAccount : BankAccount, IReplenishment<DontDeposBankAccount>
    {
        public DontDeposBankAccount()
        {
            Amount = 0;
        }


        // Пополнение счета
        public DontDeposBankAccount Replenishment(int sum)
        {
            this.Amount += sum;
            return this;
        }
    }
}
