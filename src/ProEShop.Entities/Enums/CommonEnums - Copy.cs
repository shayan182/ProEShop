﻿ using System.ComponentModel.DataAnnotations;

namespace ProEShop.Entities.Enums;
public enum Dimension : byte
{
    [Display(Name = "عادی")]
    Normal,

    [Display(Name = "کارت های بزرگ و سنگین")]
    Heavy,

    [Display(Name = "کالا های فوق سنگین")]
    UltraHeavy
}