using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQLModels
{
    public class SQLUser
    {
        [Key]
        public int Id { get; set; }
        [Unique]
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Role role { get; set; }
    }
}
