using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;

namespace WPF_EFCore.Model
{
    public class DeposBankAccount : BankAccount, IReplenishment<DeposBankAccount>
    {
        public int DepositRate { get; set; }

        public override string ToString()
        {
            return $"Дип.счет.  Id:{this.Id}, Счет:{this.Amount}, Ставка:{this.DepositRate}";
        }

        // конструктор
        public DeposBankAccount()
        {
            DepositRate = 1;
            Amount = 0;
        }

        // пополнение счета
        public DeposBankAccount Replenishment(int sum)
        {
            this.Amount += sum;
            return this;
        }

    }
}
