
namespace Catalog.API.Products.GetProductById
{
    //public record GetProductbyIdRequest(Guid Id)  
    public record GetProductbyIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductbyIdQuery(Id));

                var response = result.Adapt<GetProductbyIdResponse>();  
                return Results.Ok(response);

            });
        }
    }
}
