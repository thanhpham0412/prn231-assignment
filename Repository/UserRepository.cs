using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.Interface;
using DAO;


namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public void Delete(User user) => UserDAO.Instance.Delete(user);

        public List<User> GetAll() => UserDAO.Instance.GetAll();


        public User GetById(int id) => UserDAO.Instance.GetById(id);

        public User GetByUsername(string username) => UserDAO.Instance.GetByUsername(username);

        public User GetByUsernameAndPassword(string username, string password) => UserDAO.Instance.GetByUsernameAndPassword(username, password);

        public void Insert(User user) => UserDAO.Instance.Insert(user);


        public void Save() => UserDAO.Instance.Save();


        public void Update(User user) => UserDAO.Instance.Update(user);

    }
}
