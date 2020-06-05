
namespace ABC.Prom.Protocol.Entities
{
	public struct Message
	{
		public string Name { get; set; }
		public int ID { get; set; }
		public MessageType Type { get; set; }
		public int SeqID { get; set; }

		public Message(string name, int id, MessageType type, int seqID)
		{
			Name = name;
			Type = type;
			ID = id;
			SeqID = seqID;
		}
	}
}
