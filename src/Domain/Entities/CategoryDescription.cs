namespace Attender.Server.Domain.Entities
{
    public class CategoryDescription
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int LanguageId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Language? Language { get; set; }
    }
}