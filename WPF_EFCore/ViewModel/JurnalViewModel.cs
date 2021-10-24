using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Model;

namespace WPF_EFCore.ViewModel
{
    class JurnalViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Jurnal> messageFromJurnal;
        public ObservableCollection<Jurnal> MessageFromJurnal
        {
            get { return messageFromJurnal; }
            set
            {
                messageFromJurnal = value;
                OnPropertyChanged("MessageFromJurnal");
            }
        }


        public JurnalViewModel()
        {
            MessageFromJurnal = new ObservableCollection<Jurnal>();
            List<Jurnal> temp = new List<Jurnal>();

            using (ApplicationContext db = new ApplicationContext())
            {
                temp = db.Jurnal.ToList();
            }

            foreach (var item in temp)
            {
                MessageFromJurnal.Add(item);
            }
        }


        #region реализация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}
