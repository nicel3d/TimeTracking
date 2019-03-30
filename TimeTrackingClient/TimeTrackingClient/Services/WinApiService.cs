using TimeTrackingClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TimeTrackingClient.Services
{
    public class WinApiService
    {
        private const int _bufferSize = 1024 * 512; // 0.5Mb

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        private static string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                    if (mo != null)
                    {
                        return (string)mo["ExecutablePath"];
                    }
                }
            }
            return null;
        }

        public string DecodeFromUtf8(string utf8String)
        {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }

        public ApplicationStreamingData GetActiveWindow()
        {
            StringBuilder Buff = new StringBuilder(_bufferSize);
            IntPtr hWnd = GetForegroundWindow();
            int handle = (int)hWnd;

            if (GetWindowText(handle, Buff, _bufferSize) > 0)
            {
                uint procId = 0;
                GetWindowThreadProcessId(hWnd, out procId);

                //var proc = Process.GetProcessById((int)procId); // Работает только для 32bit win
                //Console.WriteLine(proc.MainModule);
                string base64image = new PrintScreenService().base64String;
                string processFilePath = GetMainModuleFilepath((int)procId);

                FileVersionInfo processFileVersionInfo = FileVersionInfo.GetVersionInfo(processFilePath);

                return new ApplicationStreamingData()
                {
                    // Regex.Replace(processPath, @".*\\", ""),
                    ApplicationAlias = processFileVersionInfo.ProductName,
                    ApplicationTitle = Buff.ToString(),
                    ApplicationImage = base64image
                };
            }

            return null;
        }
    }
}
