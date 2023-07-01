using System.Collections.Generic;

namespace BookManagementClient.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BookModel> Books { get; set; }

    }
}
