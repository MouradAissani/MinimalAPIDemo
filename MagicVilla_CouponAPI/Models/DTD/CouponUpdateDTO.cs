namespace MagicVilla_CouponAPI.Models.DTD;

public class CouponUpdateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Percent { get; set; }
    public bool IsActive { get; set; }
}