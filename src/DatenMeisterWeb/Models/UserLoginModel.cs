using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatenMeisterWeb.Models
{
    public class UserLoginModel
    {
        [Required]
        [DisplayName("Username")]
        [StringLength(20)]
        public string username
        {
            get;
            set;
        }

        [Required]
        [DisplayName("Password")]
        [StringLength(20)]
        public string password
        {
            get;
            set;
        }
    }
}