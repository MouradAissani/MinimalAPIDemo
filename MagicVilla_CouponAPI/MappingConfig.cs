using AutoMapper;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Models.DTD;

namespace MagicVilla_CouponAPI;

public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
        CreateMap<Coupon, CouponDTO>().ReverseMap();

    }
    
}