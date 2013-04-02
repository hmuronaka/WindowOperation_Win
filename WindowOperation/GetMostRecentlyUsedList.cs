using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public class GetMostRecentlyUsedList : WindowsControlCommand
    {
        public GetMostRecentlyUsedList()
        {
        }

        public bool execute(String data, StringBuilder response, ref int code)
        {
            int numberOfItem = -1;
            if (!Int32.TryParse(data, out numberOfItem))
            {
                numberOfItem = -1;
            }
            List<String> filesList = getMostRecentlyUsedFiles(numberOfItem);
            String filesListAsStr = Utilities.listToStrings(filesList, ";");
            foreach (String filePath in filesList)
            {
                System.Diagnostics.Trace.TraceInformation(filePath);
            }
            response.Append(filesListAsStr);
            code = 200;
            return true;
        }


        private List<String> getMostRecentlyUsedFiles(int numberOfItems)
        {
            String dir = System.Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            var filesListQuery = (from path in System.IO.Directory.EnumerateFiles(dir)
                                 orderby System.IO.File.GetLastAccessTime(path) descending
                                 select System.IO.Path.GetFileName(path));
            if (numberOfItems != -1)
            {
                filesListQuery = filesListQuery.Take(numberOfItems);
            }
            return filesListQuery.ToList<String>();
        }
    }
}
