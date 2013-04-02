using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using h.mu.Logger;

namespace WindowOperation
{
    /// <summary>
    /// 選択されたファイルを開くコマンド
    /// </summary>
    public class OpenFileCommand : WindowsControlCommand
    {
        private static LogFileWriter LOGGER = new LogFileWriter(Properties.Settings.Default, "log_");

        public bool execute(String data, StringBuilder response, ref int code)
        {
            String dir = System.Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            String file = System.IO.Path.Combine(dir, data);
            System.Diagnostics.Process.Start(file);

            LOGGER.info("opend file[" + file.ToString() + "]");
            return true;
        }

    }
}
