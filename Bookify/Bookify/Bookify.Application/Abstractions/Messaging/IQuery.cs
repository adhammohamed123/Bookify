using Bookify.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse>:IRequest<Result<TResponse>>
    {
    }

    public interface IQueryHandler<TQuery,TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }


    public interface IBaseCommand { }
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
    }
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }


    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {
    }
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}
