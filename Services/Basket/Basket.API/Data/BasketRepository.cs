namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(UserName, cancellationToken);

            if (basket == null) throw new BasketNotFoundException(UserName);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart Cart, CancellationToken cancellationToken = default)
        {
            session.Store(Cart);
            await session.SaveChangesAsync(cancellationToken);
            return Cart;
        }

        public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default)
        {
            session.DeleteWhere<ShoppingCart>(x => x.UserName == UserName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
