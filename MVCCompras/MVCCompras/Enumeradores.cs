using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MVCCompras
{
  public enum USolicitante
  {
    Recepcion,
    [Display(Name = "Veronica Gonzalez")]VGonzalez,
    [Display(Name = "Arely Palacios")] APalacios,
    [Display(Name = "Margarita Contreras")] MContreras,
    [Display(Name = "Daniel Davalos")] DDavalos,
    [Display(Name = "Diana Portillo")] DPortillo,
    [Display(Name = "Ulises Sorchini")] USorchini,
    [Display(Name = "Angeles León")] ALeon,
    [Display(Name = "Eduardo Gatica")] EGatica
  }
  public static class EnumExtensions
  {
    public static string GetDescripcion(this Enum value)
    {
      FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
      if (fieldInfo.CustomAttributes.Count() == 0)
      {
        return value.ToString();

      }
      else
      {
        var attribute = (DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute));
        return attribute.Name;
      }
    }
  }
}