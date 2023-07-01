using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using BusinessObject.Context;
using DAO.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class BookDAO : IDAO<Book>
    {
        private static BookDAO instance;
        private static readonly object instanceLock = new object();
        private static BookMgmtContext dbContext;

        public static BookDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new BookMgmtContext();
                        instance = new BookDAO();
                    }
                    return instance;
                }
            }
        }


        public void Delete(Book book)
        {
            dbContext.Books.Remove(book);
            return;
        }

        public List<Book> GetAll()
        {
            return dbContext.Books.Include(b => b.Category).ToList();
        }

        public Book GetById(int id)
        {
            return dbContext.Books.FirstOrDefault(b => b.Id == id);
        }

        public Book GetByISBN(string isbn)
        {
            return dbContext.Books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public void Insert(Book book)
        {
            dbContext.Books.Add(book);
            return;
        }

        public void Save()
        {
            dbContext.SaveChanges();
            return;
        }

        public void Update(Book book)
        {
            dbContext.Books.Update(book);
            return;
        }
    }
}
