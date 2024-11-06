namespace TestEF.Entities
{
    public class MessageEntity
    {
        public Guid Uid { get; set; }

        public string Text { get; set; } = null!;

        public Guid ChatUid { get; set; }

        public ChatEntity? Chat { get; set; }
    }
}
