using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Testovoe.Model
{
    public class ConnectionParamModel : INotifyPropertyChanged
    {
        private string host;
        private string port;
        private string dataBase;
        private string username;
        private string password;

        public string Host 
        {
            get => host;
            set { host = value; OnPropertyChanged("Host");}
        }
        public string Port
        {
            get => port;
            set { port = value; OnPropertyChanged("Port"); }
        }
        public string DataBase
        {
            get => dataBase;
            set
            {
                dataBase = value; OnPropertyChanged("DataBase");
            }
        }
        public string Username
        {
            get => username;
            set
            {
                username = value; OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged("Password"); }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
