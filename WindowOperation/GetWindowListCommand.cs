using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    /// <summary>
    /// 現在表示されているウィンドウの一覧を応答として返すコマンド
    /// </summary>
    public class GetWindowListCommand : WindowsControlCommand
    {
        public GetWindowListCommand()
        {
        }

        public bool execute(String data, StringBuilder response, ref int code)
        {
            List<String> windowList = Win32Api.getWindowListNow();
            String windowListAsStr = Utilities.listToStrings(windowList, ";");
            response.Append(windowListAsStr);
            code = 200;
            return true;
        }
    }
}
