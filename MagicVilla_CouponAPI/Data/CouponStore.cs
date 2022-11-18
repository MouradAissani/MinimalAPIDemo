﻿using MagicVilla_CouponAPI.Models;

namespace MagicVilla_CouponAPI.Data;

public static class CouponStore
{
    public static List<Coupon> couponStore = new()
    {
        new Coupon { Id = 1, Name = "10OFF", Percent = 10, IsActive = true },
        new Coupon { Id = 2, Name = "20OFF", Percent = 20, IsActive = false },
        new Coupon { Id = 3, Name = "30OFF", Percent = 30, IsActive = true }
    };

}