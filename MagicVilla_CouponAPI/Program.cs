using System.Net;
using AutoMapper;
using MagicVilla_CouponAPI;
using MagicVilla_CouponAPI.Data;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Models.DTD;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/coupon", (ILogger<Program> _logger) =>
    {
        var apiResponse = new ApiResponse();
        apiResponse.HttpStatusCode = HttpStatusCode.OK;
        apiResponse.IsSuccess = true;
        apiResponse.Result = CouponStore.couponStore;
        return Results.Ok(apiResponse);
    })
    .WithName("GetCoupons").Produces<ApiResponse>(201).Produces(400);

app.MapGet("api/coupon/{id:int}", (int id) =>
    {
        var apiResponse = new ApiResponse();
        apiResponse.HttpStatusCode = HttpStatusCode.OK;
        apiResponse.IsSuccess = true;
        apiResponse.Result = CouponStore.couponStore.FirstOrDefault(c => c.Id == id);
        return Results.Ok(apiResponse);
    })
    .WithName("GetCoupon").Produces<ApiResponse>(201).Produces(400);

app.MapPost("api/coupon/", async (IMapper _mapper,
        IValidator<CouponCreateDTO> _validator,
        [FromBody] CouponCreateDTO coupon_C_DTO) =>
    {
        var apiResponse = new ApiResponse() { IsSuccess = false, HttpStatusCode = HttpStatusCode.BadRequest };

        var validationResults = await _validator.ValidateAsync(coupon_C_DTO);
        if (!validationResults.IsValid)
        {
            apiResponse.ErrorMessages.AddRange(validationResults.Errors.Select(c => c.ToString()));
            return Results.BadRequest(apiResponse);
        }
        if (CouponStore.couponStore.FirstOrDefault(c => c.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
        {
            apiResponse.ErrorMessages.Add("Coupon name already exists");

            return Results.BadRequest(apiResponse);
        }

        Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);
        coupon.Id = CouponStore.couponStore.MaxBy(c => c.Id)!.Id + 1;
        CouponStore.couponStore.Add(coupon);
        CouponDTO couponDto = _mapper.Map<CouponDTO>(coupon);

        //return Results.CreatedAtRoute("GetCoupon", new { id = couponDto.Id }, couponDto);
        apiResponse.Result = couponDto;
        apiResponse.IsSuccess = true;
        apiResponse.HttpStatusCode = HttpStatusCode.Created;
        return Results.Ok(apiResponse);
    }).WithName("CreateCoupon")
    .Accepts<CouponCreateDTO>("application/json").Produces<ApiResponse>(201).Produces(400);

app.MapPut("api/coupon", async (
    IMapper _mapper,
        IValidator<CouponUpdateDTO> _validator,
    [FromBody] CouponUpdateDTO coupon_U_DTO) =>
{
    var apiResponse = new ApiResponse() { IsSuccess = false, HttpStatusCode = HttpStatusCode.BadRequest };

    var validationResults = await _validator.ValidateAsync(coupon_U_DTO);
    if (!validationResults.IsValid)
    {
        apiResponse.ErrorMessages.AddRange(validationResults.Errors.Select(c => c.ToString()));
        return Results.BadRequest(apiResponse);
    }

    Coupon couponFromStore = CouponStore.couponStore.FirstOrDefault(c => c.Id == coupon_U_DTO.Id);
    couponFromStore.Name = coupon_U_DTO.Name;
    couponFromStore.Percent = coupon_U_DTO.Percent;
    couponFromStore.IsActive = coupon_U_DTO.IsActive;
    couponFromStore.LastUpdate = DateTime.UtcNow;

    apiResponse.Result = _mapper.Map<CouponDTO>(couponFromStore);
    apiResponse.IsSuccess = true;
    apiResponse.HttpStatusCode = HttpStatusCode.OK;
    return Results.Ok(apiResponse);
}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<ApiResponse>(200).Produces(400); ;

app.MapDelete("api/coupon/{id:int}", (int id) =>
{
    var apiResponse = new ApiResponse() { IsSuccess = false, HttpStatusCode = HttpStatusCode.BadRequest };

    Coupon couponFromStore = CouponStore.couponStore.FirstOrDefault(c => c.Id == id);
    if (couponFromStore != null)
    {
        CouponStore.couponStore.Remove(couponFromStore);
        apiResponse.IsSuccess = true;
        apiResponse.HttpStatusCode = HttpStatusCode.NoContent;
        return Results.Ok(apiResponse);
    }
    else
    {
        apiResponse.ErrorMessages.Add("Coupon doesn't exist");
        return Results.BadRequest(apiResponse);
    }

}).WithName("DeleteCoupon");

app.UseHttpsRedirection();


app.Run();
