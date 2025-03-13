using System.ComponentModel.DataAnnotations;

namespace OzdamarDepo.Domain.MediaItems;

public enum MediaDurumEnum 
{
    [Display(Name ="Bekliyor")]
    Bekliyor=0,

    [Display(Name ="Kargoya Teslim Edildi")]
    KargoyaTeslimEdildi=1
   
}