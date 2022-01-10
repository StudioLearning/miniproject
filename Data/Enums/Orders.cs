using System.ComponentModel.DataAnnotations;
public enum Orders
{
    [Display(Name = "รอการชำระเงิน")]
    PENDING,
    [Display(Name = "ชำระเงินแล้ว")]
    PAID,
}