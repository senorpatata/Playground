using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Translator.Data.IRepositories;
using Translator.Domain.Commands;
using Translator.Domain.Dtos;
using Translator.Domain.Events;
using Translator.Service.Dxos;
using Translator.Service.Services;

namespace Translator.MSTest.service
{
    [TestClass]
    public class TranslateUserRequestHandlerTest
    {
        private TranslateUserRequestHandler Handler { get; set; }
        private Mock<ITranslationRepository> Repo { get; set; }
        private Mock<IMediator> Mediator { get; set; }
        private ITranslationDxos Mapper { get; set; }
        private Mock<ITranslatorService> TranslatorService { get; set; }

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<ITranslationRepository>();
            Mediator = new Mock<IMediator>();
            Mapper = new TranslationDxos();
            TranslatorService = new Mock<ITranslatorService>();
            Handler = new TranslateUserRequestHandler(Repo.Object, TranslatorService.Object, Mapper, Mediator.Object);
        }

        [TestMethod]
        public void Handle_WhenIsNotValidLanguage_ShouldReturnError()
        {
            // Arrange
            Repo.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            Mediator.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()));

            TranslatorService.Setup(x => x.GetAvailableLanguages()).Returns(new string[] { "Suahili", "Greek"});
            var translateRequestCommand = new TranslateRequestCommand("Test", "French", "Spanish");

            // Act
            var handleResult = Handler.Handle(translateRequestCommand, new CancellationToken());

            // Assert    
            handleResult.Result.Success.ShouldBe(false);
            handleResult.Result.ErrorCode.ShouldBe("NOT_AVAILABLE_LANG");
        }

        [TestMethod]
        public void Handle_WhenIsValidLanguage_ShouldReturnTranslation()
        {
            // Arrange
            Repo.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            Mediator.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()));

            TranslatorService.Setup(x => x.GetAvailableLanguages()).Returns(new string[] { "French", "Spanish" });
            TranslatorService.Setup(x => x.Translate(It.IsAny<TranslationServiceInput>()))
                .Returns(new TranslationServiceResult() { Success = true, Result = "Translation" });
            var translateRequestCommand = new TranslateRequestCommand("Test", "French", "Spanish");

            // Act
            var handleResult = Handler.Handle(translateRequestCommand, new CancellationToken());

            // Assert
            Mediator.Verify(v => v.Publish(It.IsAny<TranslationCreatedEvent>(), It.IsAny<CancellationToken>()));
            handleResult.Result.Success.ShouldBe(true);
        }

        [TestMethod]
        public void Handle_WhenIsCached_ShouldReturnCachedQuery()
        {
            // Arrange
            Repo.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

            Mediator.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()));

            TranslatorService.Setup(x => x.GetAvailableLanguages()).Returns(new string[] { "French", "Spanish" });
            TranslatorService.Setup(x => x.Translate(It.IsAny<TranslationServiceInput>()))
                .Returns(new TranslationServiceResult() { Success = true, Result = "Translation" });
            var translateRequestCommand = new TranslateRequestCommand("Test", "French", "Spanish");

            // Act
            var handleResult = Handler.Handle(translateRequestCommand, new CancellationToken());

            // Assert first call
            Mediator.Verify(v => v.Publish(It.IsAny<TranslationCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            handleResult.Result.Success.ShouldBe(true);


            // Arrange second call
            Repo.Setup(x =>
                 x.TranslationAlreadyExists(translateRequestCommand.Source, translateRequestCommand.SourceLanguage, translateRequestCommand.TargetLanguage))
                .Returns(Task.FromResult(true));

            // Act second call
            var handleCachedResult = Handler.Handle(translateRequestCommand, new CancellationToken());

            // Assert second call
            Mediator.Verify(v => v.Publish(It.IsAny<TranslationCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            handleResult.Result.Success.ShouldBe(true);
        }
    }
}
