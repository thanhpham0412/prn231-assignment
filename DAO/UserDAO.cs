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
    public class UserDAO : IDAO<User>
    {
        private static UserDAO instance;
        private static readonly object instanceLock = new object();
        private static BookMgmtContext dbContext;

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new BookMgmtContext();
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public List<User> GetAll()
        {
            return dbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User GetByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username);
        }

        public void Insert(User user)
        {
            dbContext.Add(user);
            return;
        }

        public void Update(User user)
        {
            dbContext.Update(user);
            return;
        }

        public void Delete(User user)
        {
            dbContext.Remove(user);
            return;
        }

        public void Save()
        {
            dbContext.SaveChanges();
            return;
        }
    }
}
