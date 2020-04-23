using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RicoClient.Scripts.Utils
{
    /// <summary>
    /// Only Windows solution to make the application blink (or bring it to the front) after authorize through the browser
    /// </summary>
    public static class WindowsHelper
    {
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static IntPtr _unityId;

        public static void GetUnityId()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                _unityId = GetActiveWindow();
        }

        public static void BringAppToFront()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                SetForegroundWindow(_unityId);
        }
    }
}
