using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class GameManager : MonoBehaviour
{
    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern int SetWindowPos(IntPtr hWnd, IntPtr hPos, int x, int y, int cx, int cy, uint nflags);

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("Dwmapi.dll")]
    public static extern uint DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    public static void DragWindow(IntPtr window)
    {
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
    }

    private const int GWL_EXSTYLE = -20;
    private const int GWL_STYLE = -16;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;

    // Start is called before the first frame update
    void Awake()
    {
        IntPtr hwnd = FindWindow(null, Application.productName);
        SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
        //SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);

        IntPtr topMost = new IntPtr(-1);
        SetWindowPos(hwnd, topMost, 0, 0, 0, 0, 1 | 2 | 64);

        //var margins = new MARGINS() { cxLeftWidth = -1 };
        //DwmExtendFrameIntoClientArea(hwnd, ref margins);
    }
}
