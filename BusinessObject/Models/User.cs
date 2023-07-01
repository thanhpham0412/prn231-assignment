using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Index(nameof(Username), IsUnique =true)]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, MinLength(8, ErrorMessage ="Password must containt more than 8 characters")]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }

    }
}
