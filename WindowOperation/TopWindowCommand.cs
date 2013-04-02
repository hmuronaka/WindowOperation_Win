using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using h.mu.Logger;

namespace WindowOperation
{
    /// <summary>
    /// 選択されたウィンドウを最前面にするコマンド
    /// </summary>
    public class TopWindowCommand : WindowsControlCommand
    {
        private static LogFileWriter LOGGER = new LogFileWriter(Properties.Settings.Default, "log_");

        public TopWindowCommand()
        {
        }

        public bool execute(String data, StringBuilder response, ref int code)
        {
            Win32Api.foregroundWindows(data);
            List<String> windowList = Win32Api.getWindowListNow();
            String windowListAsStr = Utilities.listToStrings(windowList, ";");
            response.Append(windowListAsStr);
            code = 200;

            LOGGER.info("top window[" + data + "]");
            return true;
        }
    }
}
