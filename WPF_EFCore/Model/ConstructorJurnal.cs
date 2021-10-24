using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.ViewModel;

namespace WPF_EFCore.Model
{
    class ConstructorJurnal
    {
        public ConstructorJurnal()
        {
            BankAccount.EventAccount += SaveEventToDBJurnal;
            Client.EventClientTransaction += SaveEventToDBJurnal;
            ApplicationViewModel.Events += SaveEventToDBJurnal;
        }


        private void SaveEventToDBJurnal(string nameClient, string message)
        {
            Jurnal jurnal = new Jurnal() { Name = nameClient, Message = message, DateTime = DateTime.Now.ToString() };
            using(ApplicationContext db = new ApplicationContext())
            {
                db.Jurnal.Add(jurnal);
                db.SaveChanges();
            }
        }

    }
}
