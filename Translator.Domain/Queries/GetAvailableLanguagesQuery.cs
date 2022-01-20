using System.Collections.Generic;

namespace Translator.Domain.Queries
{
    public class GetAvailableLanguagesQuery : QueryBase<IEnumerable<string>>
    {
        public GetAvailableLanguagesQuery()
        {
        }
    }
}
