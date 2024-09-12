
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IDistributedCache cache, IBasketRepository repository)
        : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(UserName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket)) return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await repository.GetBasket(UserName, cancellationToken);

            await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart Cart, CancellationToken cancellationToken = default)
        {
            var basket = await repository.StoreBasket(Cart, cancellationToken);

            await cache.SetStringAsync(Cart.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;

        }

        public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(UserName, cancellationToken);
            
            await cache.RemoveAsync(UserName, cancellationToken);
            
            return true;
        }
    }
}
