using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetById(int id);
        public User GetByUsernameAndPassword(string username, string password);
        public User GetByUsername(string username);
        public void Insert(User user);
        public void Update(User user);
        public void Delete(User user);
        public void Save();
    }
}
