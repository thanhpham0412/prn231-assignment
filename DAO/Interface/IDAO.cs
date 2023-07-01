using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interface
{
    public interface IDAO<T>
    {
        public List<T> GetAll();
        public T GetById(int id);
        public void Insert(T t);
        public void Update(T t);
        public void Delete(T t);
        public void Save();
    }
}
