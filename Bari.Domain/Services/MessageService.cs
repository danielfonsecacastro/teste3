using Bari.Domain.Entities;
using Bari.Domain.Interfaces;
using Newtonsoft.Json;
using Serilog;
using System;

namespace Bari.Domain.Services
{
    public class MessageService : IMessageService
    {
        public MessageService(ILogger logger,ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public void Proccess(string body)
        {
            var message = JsonConvert.DeserializeObject<Message>(body);
            _logger.Information("ServiceName:{ServiceName}, Id:{Id}, Date:{Date}, Value:{Value}", message.ServiceName, message.Id, message.Date, message.Value);
        }

        public Message Create(string value)
            => new Message
            {
                Date = DateTime.UtcNow.ToLocalTime(),
                Id = Guid.NewGuid().ToString(),
                ServiceName = _settings.ServiceName,
                Value = value 
            };
    }
}
