using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetWindowTitleBar : MonoBehaviour
{
    //script sadly entirely by ai but tbh i understand how allat works LOL dont blame me
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttribute, int cbAttribute);
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    void Start()
    {
        #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (Screen.fullScreenMode == FullScreenMode.Windowed) SetDarkModeTitleBar();
        #endif
    }

    private void SetDarkModeTitleBar()
    {
        try
        {
            IntPtr hWnd = GetActiveWindow();
            
            if (hWnd != IntPtr.Zero)
            {
                int useDarkMode = 1;
                DwmSetWindowAttribute(hWnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to set custom title bar color: " + e.Message);
        }
    }
}