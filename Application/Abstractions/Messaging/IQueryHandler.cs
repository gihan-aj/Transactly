using MediatR;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
        //Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
