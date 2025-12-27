using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testovoe.Model
{
    public class Parameters
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamId { get; set; }
        [Column(TypeName = "real")]
        public float Speed_2_1000 { get; set; }
        [Column(TypeName = "real")]
        public float Speed_10_1000 { get;set; }
        [Column(TypeName = "real")]
        public float Accel_2_1000 { get; set; }
        [Column(TypeName = "real")]
        public float Accel_10_1000 { get; set; }
        [Column(TypeName = "real")]
        public float Movement_2_1000 { get;set;}
        [Column(TypeName = "real")]
        public float Movement_10_1000 { get; set; }
    }
}
