using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Translator.Domain.Commands;
using Translator.Domain.Dtos;
using Translator.Domain.Queries;

namespace Translator.API.Controllers
{
    public class TranslatorController : ApiControllerBase
    {
        public TranslatorController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get Translation as query parameteres (bad idea since  there is a limit of characters, just for educational purposes)
        /// </summary>
        [HttpGet("{targetLang}")]
        [ProducesResponseType(typeof(TranslationResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TranslationResponseDto>> GetTranslationAsync(string source, string sourceLang, string targetLang)
        {
            return Single(await QueryAsync(new GetTranslationQuery(source, sourceLang, targetLang)));
        }

        /// <summary>
        /// Get Translation Available Languages
        /// </summary>
        [HttpGet("availableLangs")]
        [ProducesResponseType(typeof(TranslationResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableLanguages()
        {
            // This could be diretly to the service, but IMHO, this should be via Query to decouple from 
            return Single(await QueryAsync(new GetAvailableLanguagesQuery()));
        }

        /// <summary>
        /// Translation in the right way using post
        /// </summary>
        /// <param name="command">Request command</param>
        /// <returns>A fully object with the requested parameters and the result</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateTranslationAsync([FromBody] TranslateRequestCommand command)
        {
            return Ok(await CommandAsync(command));
        }
    }
}
