using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Interface;

namespace WPF_EFCore.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepAccId { get; set; }
        public int? DontDepAccId { get; set; }
        public List<DeposBankAccount> DeposBankAccount { get; set; }
        public List<DontDeposBankAccount> DontDeposBankAccount { get; set; }

    }
}
