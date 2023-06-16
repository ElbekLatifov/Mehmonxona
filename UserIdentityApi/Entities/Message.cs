using System.ComponentModel.DataAnnotations;


namespace ChatLibrary.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid FromId { get; set; }
        public Guid ChatGroupId { get; set; }
        public ChatGroup? ChatGroup { get; set; }

    }
}
