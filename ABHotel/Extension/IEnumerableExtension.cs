using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Extension
{
    public static class IEnumerableExtension
    {
        //int
        public static IEnumerable <SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("ID"),
                       Selected = item.GetPropertyValue("ID").Equals(selectedValue.ToString())
                   };
        }

        //string
        public static IEnumerable<SelectListItem> ToSelectListItemString<T>(this IEnumerable<T> items, string selectedValue)
        {
            if (selectedValue == null)
            {
                selectedValue = "";
            }

            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                   };
        }
    }
}
