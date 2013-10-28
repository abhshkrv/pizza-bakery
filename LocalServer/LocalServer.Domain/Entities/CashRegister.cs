using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class CashRegister
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cashRegisterID { get; set; }
        public byte status { get; set; }
    }
}
