using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_EFCore.Interface
{
    interface IReplenishment<out T>
    {
        T Replenishment(int sum);
    }
}
