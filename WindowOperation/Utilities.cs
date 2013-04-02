using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public static class Utilities
    {

        public static String listToStrings(List<String> aList, String delimiter)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < aList.Count; i++)
            {
                String windowName = aList[i].Replace(delimiter, "\\" + delimiter);
                builder.Append(windowName);
                builder.Append(delimiter);
            }
            return builder.ToString();
        }
    }
}
