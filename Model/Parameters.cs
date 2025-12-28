using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Testovoe.Model
{
    public class Parameters : INotifyPropertyChanged
    {
        private int paramId;
        private float speed_2_1000;
        private float speed_10_1000;
        private float accel_2_1000;
        private float accel_10_1000;
        private float movement_2_1000;
        private float movement_10_1000;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamId 
        { 
            get => paramId;
            set { paramId = value; OnPropertyChanged("ParamId"); }
        }
        [Column(TypeName = "real")]
        public float Speed_2_1000 
        { 
            get => speed_2_1000;
            set { speed_2_1000 = value; OnPropertyChanged("Speed_2_1000"); }
        }
        [Column(TypeName = "real")]
        public float Speed_10_1000 
        { 
            get => speed_10_1000;
            set { speed_10_1000 = value; OnPropertyChanged("Speed_10_1000"); }
        }
        [Column(TypeName = "real")]
        public float Accel_2_1000 
        { 
            get => accel_2_1000;
            set { accel_2_1000 = value; OnPropertyChanged("Accel_2_1000"); }
        }
        [Column(TypeName = "real")]
        public float Accel_10_1000 
        {
            get => accel_10_1000;
            set { accel_10_1000 = value; OnPropertyChanged("Accel_10_1000"); }
        }
        [Column(TypeName = "real")]
        public float Movement_2_1000
        {
            get => movement_2_1000;
            set { movement_2_1000 = value;OnPropertyChanged("Movement_2_1000"); }
        }
        [Column(TypeName = "real")]
        public float Movement_10_1000 
        {
            get => movement_10_1000;
            set { movement_10_1000 = value; OnPropertyChanged("Movement_10_1000"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
