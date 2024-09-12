using Marten.Schema;
using Microsoft.AspNetCore.Http.Features;

namespace Basket.API.Models
{
    public class ShoppingCart
    {
        [Identity]
        public String UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(x => x.Quantity * x.Price);

        public ShoppingCart()
        {
                
        }
        public ShoppingCart(string username)
        {
            UserName = username;
        }

    }
}
