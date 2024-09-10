using AutoMapper;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
using EStore.Domain.EntityDtos.NewFolder;
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
            CreateMap<OrderItemreq,OrderItem>();
            //Map from order to orderres
            CreateMap<Order, OrderRes>()
              .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));                         
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
        }
    }
}
