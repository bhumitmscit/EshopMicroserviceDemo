using BuildingBlock.Exceptions;

namespace Catalog.API.Exceptions
{
    public class ProductNotFoundExceptioncs : NotFoundException
    {
        public ProductNotFoundExceptioncs(Guid Id) : base ("Product not found",Id.ToString() )
        {
            
        }
    }
}
