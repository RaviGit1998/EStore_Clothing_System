using AutoMapper;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
using EStore.Domain.EntityDtos.NewFolder;
using EStore.Domain.EntityDtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()); 

            // Map from Product to ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SubCategoryId));

            // Map from CreateProductDto to Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            //Map from OrderReq to Order
            CreateMap<OrderReq, Order>()
                .ForMember(dest => dest.Coupon, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
                         

            
            //Map from order to orderres
            CreateMap<Order, OrderRes>()
              .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
              .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDto
              {
                  UserId = src.User.UserId,
                  FirstName = src.User.FirstName,
                  Email = src.User.Email
              }))
                .ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => new CouponDto
                {
                    CouponId = src.Coupon.CouponId,
                    CouponCode = src.Coupon.CouponCode,
                    DiscountedAmount = src.Coupon.DiscountedAmount
                }))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => new PaymentDto
                {
                    PaymentId = src.Payment.PaymentId,
                    Amount = src.Payment.Amount,
                    PaymentMode = src.Payment.PaymentMode
                }))
                .ForMember(dest => dest.Shipping, opt => opt.MapFrom(src => new ShippingDto
                {
                    ShippingId = src.Shipping.ShippingId,
                    TrackingNumber = src.Shipping.TrackingNumber,
                    EstimatedDeliveryDate = src.Shipping.EstimatedDeliveryDate
                }));

            CreateMap<OrderItemreq, OrderItem>();

            CreateMap<OrderItem, OrderItemRes>();
            //categoryReq to Category
            CreateMap<CategoryReq, Category>()
             .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
             .ForMember(dest => dest.Products, opt => opt.Ignore());
            //userReq to User
            CreateMap<UserReq, User>()
              .ForMember(dest => dest.ShippingAddresses, opt => opt.Ignore())
              .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
              .ForMember(dest => dest.Orders, opt => opt.Ignore())
              .ForMember(dest => dest.WishList, opt => opt.Ignore())
              .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
            //loginreq to User

            CreateMap<LoginReq, User>()
             .ForMember(dest => dest.Email,opt=>opt.MapFrom(src=>src.Email))
             .ForMember(dest=>dest.PasswordHash,opt=>opt.MapFrom(src=>src.PasswordHash))
             .ForMember(dest => dest.ShippingAddresses, opt => opt.Ignore())
             .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
             .ForMember(dest => dest.Orders, opt => opt.Ignore())
             .ForMember(dest => dest.WishList, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<ShippingAddress, ShippingAddressResponse>();

            // DTO to Entity
            CreateMap<ShippingAddressRequest, ShippingAddress>();
        }
    }
}
