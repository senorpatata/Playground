using MediatR;

namespace Translator.Domain.Commands
{
    public class CommandBase<T> : IRequest<T> where T : class
    {      
    }
}