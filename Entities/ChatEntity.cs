namespace TestEF.Entities
{
    public class ChatEntity
    {
        public Guid Uid { get; set; }

        public string Name { get; set; } = null!;

        public List<MessageEntity> Messages { get; set; } = [];
    }
}
