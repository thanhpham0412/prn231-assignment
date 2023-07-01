using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repository.Interface
{
    public interface IBookRepository
    {
        public List<Book> GetAll();
        public Book GetById(int id);
        public void Insert(Book book);
        public void Update(Book book);
        public void Delete(Book book);
        public void Save();
    }
}
