using System.ComponentModel.DataAnnotations;

public enum CargoStatusEnum
{
    [Display(Name = "Bekliyor")]
    Bekliyor = 0,

    [Display(Name = "Hazırlanıyor")]
    Hazirlaniyor = 1,

    [Display(Name = "Kargoya Verildi")]
    KargoyaVerildi = 2,

    [Display(Name = "Teslim Edildi")]
    TeslimEdildi = 3
}