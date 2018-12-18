using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Trial.Models
{
    [Table("InstallApp")]
    public partial class InstallApp
    {
        [Key]
        public int Id { get; set; }

        public string AppName { get; set; }

        public string MachineName { get; set; }
    }
}
