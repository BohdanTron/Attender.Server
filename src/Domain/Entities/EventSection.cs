namespace Attender.Server.Domain.Entities
{
    public class EventSection
    {
        public int RowId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LanguageId { get; set; }

        public virtual Language? Language { get; set; }
    }
}