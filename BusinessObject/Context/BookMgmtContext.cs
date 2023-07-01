using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace BusinessObject.Context
{
    public class BookMgmtContext : DbContext
    {
        public BookMgmtContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BookMgmtDB"));
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder optionsBuilder)
        {
            /*optionsBuilder.Entity<Book>().HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);*/

            var cateList = new List<Category>()
            {
                new Category {Id = 1, Name = "Fiction" },
                new Category {Id = 2, Name = "Thriller" },
                new Category {Id = 3, Name = "Mystery" },
                new Category {Id = 4, Name = "Narrative" },
                new Category {Id = 5, Name = "Novel" }
            };

            var bookList = new List<Book>()
            {
                new Book {Id = 1, ISBN = "0684801221", Title = "The Old Man and the Sea", Author = "Ernest Hemingway", CategoryId = 1},
                new Book {Id = 2, ISBN = "B071XQ6H38", Title = "The Cruel Prince", Author = "Holly Black", CategoryId = 1},
                new Book {Id = 3, ISBN = "1416543805", Title = "The First Commandment", Author = "Brad Thor", CategoryId = 2},
                new Book {Id = 4, ISBN = "0141049219", Title = "Presumed Innocent", Author = "Turow Scott", CategoryId = 2},
                new Book {Id = 5, ISBN = "1476765650", Title = "All the Light we Cannot See", Author = "Anthony Doerr", CategoryId = 5}
            };

            var userList = new List<User>()
            {
                new User {Id = 1, Username = "admin", Password = "12345678", Role = Role.Admin},
                new User {Id = 2, Username = "user1", Password = "12345678", Role = Role.User},
                new User {Id = 3, Username = "user2", Password = "12345678", Role = Role.User}
            };

            optionsBuilder.Entity<Category>().HasData(cateList);
            optionsBuilder.Entity<Book>().HasData(bookList);
            optionsBuilder.Entity<User>().HasData(userList);
        }   
    }
}
