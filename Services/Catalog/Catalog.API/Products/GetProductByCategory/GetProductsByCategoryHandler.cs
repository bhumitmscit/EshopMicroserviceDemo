using Marten.Linq.QueryHandlers;
using Microsoft.VisualBasic;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    public class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var comparer = new CaseInsensitiveComparer();
            var result = await session.Query<Product>().Where(x =>  x.Category.Contains(query.Category, comparer)).ToListAsync();
            return new GetProductByCategoryResult(result);
        }
    }

    public class CaseInsensitiveComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj == null ? 0 : obj.ToLower().GetHashCode();
        }
    }
}
