using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_EFCore.Interface
{
    interface ITransaction<in T>
    {
        void Transaction(T acc, int sum);
    }
}
