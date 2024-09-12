
using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductbyIdQuery(Guid Id): IQuery<GetProductbyIdResult>;
    public record GetProductbyIdResult(Product Product);
    internal class GetProductByIdQueryHandler (IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
        : IQueryHandler<GetProductbyIdQuery, GetProductbyIdResult>
    {
        public async Task<GetProductbyIdResult> Handle(GetProductbyIdQuery query, CancellationToken cancellationToken)
        {
             var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundExceptioncs(query.Id);
            }
            return new GetProductbyIdResult(product);
        }
    }
}
