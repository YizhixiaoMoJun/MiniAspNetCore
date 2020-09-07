using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model
{
    public class UserInfo
    {

        public int Id { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Pwd { get; set; }
    
    }
}
