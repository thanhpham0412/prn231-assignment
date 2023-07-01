using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repository.Interface
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public void Insert(Category category);
        public void Update(Category category);
        public void Delete(Category category);
        public void Save();
    }
}
