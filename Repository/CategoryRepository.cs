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
    public class CategoryRepository : ICategoryRepository
    {
        public void Delete(Category category) => CategoryDAO.Instance.Delete(category);

        public List<Category> GetAll() => CategoryDAO.Instance.GetAll();

        public Category GetById(int id) => CategoryDAO.Instance.GetById(id);

        public void Insert(Category category) => CategoryDAO.Instance.Insert(category);

        public void Save() => CategoryDAO.Instance.Save();

        public void Update(Category category) => CategoryDAO.Instance.Update(category);
    }
}
