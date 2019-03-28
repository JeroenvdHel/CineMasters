using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetStaticStringSelectList(this IHtmlHelper htmlHelper, Type type)
        {
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                string value = fieldInfo.GetValue(null).ToString();

                yield return new SelectListItem()
                {
                    Value = value,
                    Text = value
                };
            }
        }
    }
}
