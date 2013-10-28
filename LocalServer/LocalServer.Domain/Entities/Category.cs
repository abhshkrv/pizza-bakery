using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LocalServer.Domain.Entities
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int categoryID { get; set; }
        public string categoryName { get; set; }
    }
}
