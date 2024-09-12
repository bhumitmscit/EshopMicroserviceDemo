using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService
        : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly DiscountContext dbcontext;
        public DiscountService(DiscountContext objdbcontext)
        {
            dbcontext = objdbcontext;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var objcoupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

                if (objcoupon == null)
                {
                    objcoupon = new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No discount found" };
                }

                var model = objcoupon.Adapt<CouponModel>();
                return model;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            dbcontext.Coupons.Add(coupon);
            await dbcontext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            dbcontext.Coupons.Update(coupon);
            await dbcontext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, "coupon not found"));

            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();

            return new DeleteDiscountResponse { Success = true };

        }

    }
}
