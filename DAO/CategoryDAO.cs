using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using BusinessObject.Context;
using DAO.Interface;


namespace DAO
{
    public class CategoryDAO :IDAO<Category>
    {
        private static CategoryDAO instance;
        private static readonly object instanceLock = new object();
        private static BookMgmtContext dbContext;

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new BookMgmtContext();
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }


        public void Delete(Category category)
        {
            dbContext.Categories.Remove(category);
            return;
        }

        public List<Category> GetAll()
        {
            return dbContext.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return dbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Insert(Category category)
        {
            dbContext.Add(category);
            return;
        }

        public void Save()
        {
            dbContext.SaveChanges();
            return;
        }

        public void Update(Category category)
        {
            dbContext.Update(category);
            return;
        }
    }
}
