using System;
using Convey.Types;

namespace MyApplication.Mongo
{
    public class MessageDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}