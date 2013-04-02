using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowOperation
{
    public static class Win32Api
    {
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, int lParam);
        [DllImport("user32.dll")]
        private static extern int IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        private delegate int EnumWindowsDelegate(IntPtr hWnd, int lParam);


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void foregroundWindows(String name)
        {
            EnumWindows(new EnumWindowsDelegate(delegate(IntPtr hWnd, int lParam)
            {
                StringBuilder sb = new StringBuilder(0x1024);
                if (IsWindowVisible(hWnd) != 0 && GetWindowText(hWnd, sb, sb.Capacity) != 0)
                {
                    string title = sb.ToString();
                    int pid;
                    GetWindowThreadProcessId(hWnd, out pid);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pid);
                    String myName = title + "," + p.ProcessName;
                    if (myName == name)
                    {
                        SetForegroundWindow(hWnd);
                    }
                }
                return 1;
            }), 0);
        }

        public static List<String> getWindowListNow()
        {
            List<String> newWindowList = new List<String>();
            EnumWindows(new EnumWindowsDelegate(delegate(IntPtr hWnd, int lParam)
            {
                StringBuilder sb = new StringBuilder(0x1024);
                if (IsWindowVisible(hWnd) != 0 && GetWindowText(hWnd, sb, sb.Capacity) != 0)
                {
                    string title = sb.ToString();
                    int pid;
                    GetWindowThreadProcessId(hWnd, out pid);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pid);
                    newWindowList.Add(title + "," + p.ProcessName);
                }
                return 1;
            }), 0);
            return newWindowList;
        }

    }
}
