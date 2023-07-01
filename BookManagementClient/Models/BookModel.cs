namespace BookManagementClient.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}
