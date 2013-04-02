using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    /// <summary>
    /// Windowsを処理するための1件のコマンドを表現する.
    /// </summary>
    public interface WindowsControlCommand
    {
        bool execute(String data, StringBuilder response, ref int code);
    }
}
