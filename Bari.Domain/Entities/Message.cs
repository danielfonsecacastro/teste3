using System;

namespace Bari.Domain.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public string ServiceName { get; set; }
    }
}
