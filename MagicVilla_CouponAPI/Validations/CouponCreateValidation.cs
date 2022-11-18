using MagicVilla_CouponAPI.Models.DTD;

namespace MagicVilla_CouponAPI.Validations;

public class CouponCreateValidation:AbstractValidator<CouponCreateDTO>
{
    public CouponCreateValidation()
    {
        RuleFor(model => model.Name).NotEmpty();
        RuleFor(model => model.Percent).InclusiveBetween(1,100);
    }
}