using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Domains
{
    public class DomainException : Exception
    {
        /// <summary>
        /// status code
        /// </summary>
        public int Status { get; }

        /// <summary>
        /// domain exception
        /// </summary>
        /// <param name="status"> status code</param>
        /// <param name="message">error message</param>
        public DomainException(int status,string message) : base(message)
        {
            Status = status;
        }

        /// <summary>
        /// domain exception with default status code : 400
        /// </summary>
        /// <param name="message"></param>
        public DomainException(string message) : this (400,message)
        {

        }

    }
}
