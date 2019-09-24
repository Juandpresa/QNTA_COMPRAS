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
    [Display(Name = "Veronica Gonzalez")]
    Veronica_Gonzalez,
    [Display(Name = "Arely Palacios")]
    Arely_Palacios,
    [Display(Name = "Margarita Contreras")]
    Margarita_Contreras,
    [Display(Name = "Daniel Davalos")]
    Daniel_Davalos,
    [Display(Name = "Diana Portillo")]
    Diana_Portillo,
    [Display(Name = "Ulises Sorchini")]
    Ulises_Sorchini,
    [Display(Name = "Angeles León")]
    Angeles_Leon,
    [Display(Name = "Eduardo Gatica")]
    Eduardo_Gatica,
    Juand,
    berna
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
        var attribute = (DisplayAttribute)
          fieldInfo.GetCustomAttribute(typeof(DisplayAttribute));
        return attribute.Name;
      }
    }
  }
}