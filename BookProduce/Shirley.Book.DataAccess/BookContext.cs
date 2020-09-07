

using BookApi;
using BookApi.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Shirley.Book.DataAccess
{
   public class BookContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:/Book.db");
        }
        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<WeatherForecast> WeatherForecast { get; set; }


    }
}
