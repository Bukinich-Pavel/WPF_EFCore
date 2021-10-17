using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_EFCore.Model
{
    public class DeposBankAccount : BankAccount
    {
        public int DepositRate { get; set; }

        public override string ToString()
        {
            return $"Дип.счет.  Id:{this.Id}, Счет:{this.Amount}, Ставка:{this.DepositRate}";
        }
    }
}
