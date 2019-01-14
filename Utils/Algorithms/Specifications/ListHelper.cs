using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils.Algorithms.Specifications
{
    public static class ListHelper
    {
        public static IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }
    }
}
