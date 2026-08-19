using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetWindowTitleBar : MonoBehaviour
{
    //script sadly entirely by ai but tbh i understand how allat works LOL dont blame me
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttribute, int cbAttribute);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    void Start()
    {
        #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (Screen.fullScreenMode == FullScreenMode.Windowed) SetDarkModeTitleBar();
        #endif
    }

    private void SetDarkModeTitleBar()
    {
        IntPtr hWnd = GetActiveWindow();
        if (hWnd != IntPtr.Zero)
        {
            int useDarkMode = 1;
            DwmSetWindowAttribute(hWnd, 20, ref useDarkMode, sizeof(int));
        }
    }
}