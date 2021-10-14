using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_EFCore.Model
{
    class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int  DepAccId{ get; set; }
        public int  DontDepAccId{ get; set; }


    }
}
