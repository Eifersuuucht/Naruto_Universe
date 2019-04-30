using NarutoUniverseProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Extensions
{
    public static class IListExtensions
    {
        public static  Boolean IsAnyBoolInItemList(this IList<Item> list)
        {
            foreach (var item in list)
            {
                if (item.Value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
