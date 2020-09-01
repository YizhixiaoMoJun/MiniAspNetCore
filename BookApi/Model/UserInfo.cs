using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model
{
    public class UserInfo
    {
        [Required]
        public string UserName { get; set; }

        public string Pwd { get; set; }
    
    }
}
