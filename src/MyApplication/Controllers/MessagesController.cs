using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Mvc;
using MyApplication.Messaging;
using MyApplication.Mongo;

namespace MyApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMongoRepository<MessageDocument, Guid> _repository;
        private readonly IBusPublisher _publisher;

        public MessagesController(IMongoRepository<MessageDocument, Guid> repository, IBusPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var messages = await _repository.FindAsync(m => true);

            return messages.Select(m => m.Value);
        }
        
        [HttpPost]
        public Task Post([FromBody] Message message)
            => _publisher.PublishAsync(message, new CorrelationContext());
    }
}
