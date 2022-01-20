using System;
using MediatR;

namespace Translator.Domain.Events
{
    public class TranslationCreatedEvent : INotification
    {
        public Guid Id { get; set; }
        public TranslationCreatedEvent(Guid id)
        {
            Id = id;
        }
    }
}