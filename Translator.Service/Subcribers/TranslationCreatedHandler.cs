using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Translator.Data.IRepositories;
using Translator.Domain.Events;

namespace Translator.Service.Subcribers
{
    public class TranslationCreatedHandler : INotificationHandler<TranslationCreatedEvent>
    {
        private readonly ITranslationRepository _translationRepository;
        private readonly ILogger _logger;

        public TranslationCreatedHandler(ITranslationRepository translationRepository, ILogger<TranslationCreatedHandler> logger)
        {
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _logger = logger;
        }

        public async Task Handle(TranslationCreatedEvent notification, CancellationToken cancellationToken)
        {
            var translation = await _translationRepository.GetAsync(e => e.Id == notification.Id);

            if (translation == null)
            {
                //TODO: Handle next business logic if translation is not found
                _logger.LogWarning("Translation is not found");
            }
            else
            {
                //TODO: Do something more
                _logger.LogInformation($"A new Translation has been created! Id: {notification.Id}");
            }
        }
    }
}