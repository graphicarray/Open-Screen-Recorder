// ============================================================================
// Open Screen Recorder v1.1 — Offline Screen Capture & Crash-Safe Video Studio
// Brand & Owner: Graphicarray · Md Numan Ahmed (https://www.linkedin.com/in/graphicarray)
// Blueprint: FastStone Capture v7.4 + Crash-Safe Single-File A/V Muxer
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GraphicarrayCapture
{
    // ========================================================================
    // 1. GRAPHICARRAY BRAND DESIGN TOKENS & VECTOR LOGO RENDERER
    // ========================================================================
    public static class GaBrand
    {
        public const string ProductName = "Open Screen Recorder";
        public const string BrandName = "Graphicarray";
        public const string OwnerName = "Md Numan Ahmed";
        public const string FooterCredit = "Graphicarray · Numan Ahmed";
        public const string VersionText = "v1.1";
        public const string LinkedInUrl = "https://www.linkedin.com/in/graphicarray";

        // Strict Graphicarray palette tokens (only these colors are used)
        public static readonly Color Abyss   = ColorTranslator.FromHtml("#001E36"); // --ga-abyss
        public static readonly Color Panel   = ColorTranslator.FromHtml("#062847"); // --ga-panel
        public static readonly Color Edge    = ColorTranslator.FromHtml("#0F4A7A"); // --ga-edge
        public static readonly Color Cyan    = ColorTranslator.FromHtml("#00CDFF"); // --ga-cyan
        public static readonly Color Azure   = ColorTranslator.FromHtml("#0083FF"); // --ga-azure
        public static readonly Color Ice     = ColorTranslator.FromHtml("#DDF6FF"); // --ga-ice
        public static readonly Color Text    = ColorTranslator.FromHtml("#FFFFFF"); // --ga-text
        public static readonly Color Slate   = ColorTranslator.FromHtml("#8FA7BF"); // --ga-slate
        public static readonly Color Mute    = ColorTranslator.FromHtml("#5F7C99"); // --ga-mute
        public static readonly Color Amber   = ColorTranslator.FromHtml("#F5A94A"); // --ga-amber
        public static readonly Color Green   = ColorTranslator.FromHtml("#3DD68C"); // --ga-green
        public static readonly Color Red     = ColorTranslator.FromHtml("#FF5D6C"); // --ga-red
        public static readonly Color TopGlow = ColorTranslator.FromHtml("#06406E");

        public const int RadiusCard = 20;
        public const int RadiusInput = 12;
        public const int RadiusPill = 999;

        public const string OfficialLogoSvg = @"<svg id=""ga-Layer_1"" xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" viewBox=""0 0 100 100"">
  <defs>
    <style>
      .ga-st0 { fill: #001e36; }
      .ga-st1 { fill: url(#ga-radial-gradient); }
      .ga-st2 { fill: url(#ga-radial-gradient1); }
      .ga-st3 { fill: url(#ga-radial-gradient2); }
    </style>
    <radialGradient id=""ga-radial-gradient"" cx=""49.12"" cy=""57.59"" fx=""49.12"" fy=""57.59"" r=""20.9"" gradientUnits=""userSpaceOnUse"">
      <stop offset=""0"" stop-color=""#00cdff""/>
      <stop offset="".2"" stop-color=""#00c6ff""/>
      <stop offset="".48"" stop-color=""#00b5ff""/>
      <stop offset="".79"" stop-color=""#09f""/>
      <stop offset=""1"" stop-color=""#0083ff""/>
    </radialGradient>
    <radialGradient id=""ga-radial-gradient1"" cx=""28.88"" cy=""26.12"" fx=""28.88"" fy=""26.12"" r=""41.83"" gradientUnits=""userSpaceOnUse"">
      <stop offset=""0"" stop-color=""#00cdff""/>
      <stop offset="".23"" stop-color=""#00c9ff""/>
      <stop offset="".47"" stop-color=""#00bdff""/>
      <stop offset="".7"" stop-color=""#00a9ff""/>
      <stop offset="".93"" stop-color=""#008dff""/>
      <stop offset=""1"" stop-color=""#0083ff""/>
    </radialGradient>
    <radialGradient id=""ga-radial-gradient2"" cx=""59"" cy=""46.76"" fx=""59"" fy=""46.76"" r=""33.78"" gradientUnits=""userSpaceOnUse"">
      <stop offset=""0"" stop-color=""#00cdff""/>
      <stop offset="".2"" stop-color=""#00c6ff""/>
      <stop offset="".48"" stop-color=""#00b5ff""/>
      <stop offset="".79"" stop-color=""#09f""/>
      <stop offset=""1"" stop-color=""#0083ff""/>
    </radialGradient>
  </defs>
  <rect class=""ga-st0"" x=""0"" y=""0"" width=""100"" height=""100""/>
  <polygon class=""ga-st1"" points=""49.78 61.93 39.27 63.71 39.27 63.71 33.89 73.04 49.78 70.35 66.11 73.15 60.72 63.81 49.78 61.93""/>
  <path class=""ga-st2"" d=""M39.27,46.88l5.38-9.33.45-.79,2.54-4.4c1.03-1.78,3.6-1.78,4.63,0l2.54,4.4.5.87,10.78,1.85h0l-5.39-9.34-1.04-1.8-.11-.2c-4.27-7.39-14.94-7.39-19.2,0l-.11.2-.98,1.7-5.38,9.33-5.38,9.32-4.21,7.29c-2.78,4.82-1.54,10.27,1.85,13.63l2.36-4.09,5.38-9.32,5.38-9.33Z""/>
  <path class=""ga-st3"" d=""M75.7,56.1l-4.2-7.27-10.78-1.85-10.73-1.84-.02.04-5.31,9.2,5.13-.87,5.55.95,10.78,1.85h0s5.39,9.34,5.39,9.34l2.35,4.07c3.39-3.37,4.63-8.81,1.84-13.63Z""/>
</svg>";

        public static Font MakeFont(float sizePx, FontStyle style = FontStyle.Regular)
        {
            string[] families = { "Inter", "Segoe UI", "Verdana", "Arial" };
            foreach (string name in families)
            {
                try
                {
                    Font f = new Font(name, sizePx, style, GraphicsUnit.Pixel);
                    if (string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase))
                        return f;
                    f.Dispose();
                }
                catch { }
            }
            return new Font(SystemFonts.DefaultFont.FontFamily, sizePx, style, GraphicsUnit.Pixel);
        }

        public static void OpenBrandProfile()
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = LinkedInUrl, UseShellExecute = true });
            }
            catch { }
        }

        public static GraphicsPath RoundedRect(RectangleF bounds, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (bounds.Width <= 0 || bounds.Height <= 0) return path;
            float d = Math.Min(radius * 2f, Math.Min(bounds.Width, bounds.Height));
            if (d <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static LinearGradientBrush SignatureGradient(RectangleF rect)
        {
            RectangleF safe = rect.Width > 1 && rect.Height > 1 ? rect : new RectangleF(0, 0, 100, 40);
            return new LinearGradientBrush(safe, Cyan, Azure, 135f);
        }

        public static void PaintAbyssBackground(Graphics g, Rectangle bounds)
        {
            g.Clear(Abyss);
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            using (GraphicsPath glowPath = new GraphicsPath())
            {
                Rectangle glowRect = new Rectangle(bounds.Width / 2 - 450, -250, 900, 500);
                glowPath.AddEllipse(glowRect);
                using (PathGradientBrush pgb = new PathGradientBrush(glowPath))
                {
                    pgb.CenterColor = TopGlow;
                    pgb.SurroundColors = new Color[] { Abyss };
                    g.FillRectangle(pgb, bounds);
                }
            }
        }

        public static void DrawLogoTile(Graphics g, RectangleF tileRect, bool hovered, bool focused)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float radius = tileRect.Width * 0.25f;

            if (hovered)
            {
                using (GraphicsPath glowPath = RoundedRect(RectangleF.Inflate(tileRect, 3, 3), radius + 3))
                using (Pen glowPen = new Pen(Color.FromArgb(140, Cyan), 3f))
                    g.DrawPath(glowPen, glowPath);
            }

            using (GraphicsPath tilePath = RoundedRect(tileRect, radius))
            using (SolidBrush abyssBrush = new SolidBrush(Abyss))
            using (Pen borderPen = new Pen(focused || hovered ? Cyan : Edge, 1f))
            {
                g.FillPath(abyssBrush, tilePath);
                g.DrawPath(borderPen, tilePath);
            }

            GraphicsState state = g.Save();
            g.TranslateTransform(tileRect.X, tileRect.Y);
            g.ScaleTransform(tileRect.Width / 100f, tileRect.Height / 100f);

            using (LinearGradientBrush wingGrad1 = new LinearGradientBrush(new PointF(33f, 61f), new PointF(66f, 74f), Cyan, Azure))
            using (LinearGradientBrush wingGrad2 = new LinearGradientBrush(new PointF(20f, 24f), new PointF(66f, 70f), Cyan, Azure))
            using (LinearGradientBrush wingGrad3 = new LinearGradientBrush(new PointF(40f, 44f), new PointF(78f, 70f), Cyan, Azure))
            {
                PointF[] poly1 = new PointF[]
                {
                    new PointF(49.78f, 61.93f), new PointF(39.27f, 63.71f),
                    new PointF(33.89f, 73.04f), new PointF(49.78f, 70.35f),
                    new PointF(66.11f, 73.15f), new PointF(60.72f, 63.81f)
                };
                g.FillPolygon(wingGrad1, poly1);

                PointF[] poly2 = new PointF[]
                {
                    new PointF(39.27f, 46.88f), new PointF(44.65f, 37.55f),
                    new PointF(47.64f, 32.36f), new PointF(49.95f, 31.00f),
                    new PointF(52.27f, 32.36f), new PointF(55.31f, 37.63f),
                    new PointF(66.09f, 39.48f), new PointF(59.55f, 28.14f),
                    new PointF(50.00f, 22.60f), new PointF(40.35f, 28.14f),
                    new PointF(33.89f, 39.17f), new PointF(24.30f, 55.78f),
                    new PointF(26.15f, 69.41f), new PointF(28.51f, 65.32f),
                    new PointF(33.89f, 56.00f)
                };
                g.FillPolygon(wingGrad2, poly2);

                PointF[] poly3 = new PointF[]
                {
                    new PointF(75.70f, 56.10f), new PointF(71.50f, 48.83f),
                    new PointF(60.72f, 46.98f), new PointF(49.99f, 45.14f),
                    new PointF(44.66f, 54.38f), new PointF(49.79f, 53.51f),
                    new PointF(55.34f, 54.46f), new PointF(66.12f, 56.31f),
                    new PointF(71.51f, 65.65f), new PointF(73.86f, 69.72f)
                };
                g.FillPolygon(wingGrad3, poly3);
            }
            g.Restore(state);
        }

        // Creates dynamic Taskbar & System Tray icon (Normal Cyan logo vs Blinking Red recording badge vs Amber Paused)
        public static Icon CreateStatusIcon(int state)
        {
            // state: 0 = Idle (Logo), 1 = Blinking Red Recording, 2 = Amber Paused
            using (Bitmap bmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                DrawLogoTile(g, new RectangleF(0, 0, 32, 32), false, false);
                if (state == 1)
                {
                    using (SolidBrush redGlow = new SolidBrush(Color.FromArgb(190, Red)))
                    using (SolidBrush redCore = new SolidBrush(Red))
                    using (Pen whiteRing = new Pen(Text, 2f))
                    {
                        g.FillEllipse(redGlow, 2, 2, 28, 28);
                        g.FillEllipse(redCore, 6, 6, 20, 20);
                        g.DrawEllipse(whiteRing, 6, 6, 20, 20);
                    }
                }
                else if (state == 2)
                {
                    using (SolidBrush amberCore = new SolidBrush(Amber))
                    using (Pen abyssPen = new Pen(Abyss, 2.5f))
                    {
                        g.FillEllipse(amberCore, 16, 16, 14, 14);
                        g.DrawLine(abyssPen, 21, 19, 21, 27);
                        g.DrawLine(abyssPen, 25, 19, 25, 27);
                    }
                }
                IntPtr hIcon = bmp.GetHicon();
                Icon cloned = (Icon)Icon.FromHandle(hIcon).Clone();
                NativeMethods.DestroyIcon(hIcon);
                return cloned;
            }
        }
    }

    // ========================================================================
    // 2. WIN32 LOW-LEVEL PRIMITIVES (GDI32, USER32, KERNEL32, WINMM WAVEIN)
    // ========================================================================
    public static class NativeMethods
    {
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int SB_PAGEDOWN = 3;
        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;
        public const int SRCCOPY = 0x00CC0020;
        public const int CAPTUREBLT = 0x40000000;
        public const int CURSOR_SHOWING = 0x00000001;
        public const int DI_NORMAL = 0x0003;
        public const uint CF_BITMAP = 2;
        public const int CALLBACK_FUNCTION = 0x00030000;
        public const int MM_WIM_DATA = 0x3C0;

        public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate bool ConsoleCtrlDelegate(int sig);
        public delegate void WaveInProc(IntPtr hwi, int uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

        [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X; public int Y; }
        [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public uint bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEFORMATEX
        {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEHDR
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesRecorded;
            public IntPtr dwUser;
            public uint dwFlags;
            public uint dwLoops;
            public IntPtr lpNext;
            public IntPtr reserved;
        }

        [DllImport("user32.dll")] public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")] public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")] public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")] public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")] public static extern IntPtr WindowFromPoint(POINT Point);
        [DllImport("user32.dll")] public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, POINT pt, uint uFlags);
        [DllImport("user32.dll")] public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")] public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")] public static extern bool GetCursorInfo(out CURSORINFO pci);
        [DllImport("user32.dll")] public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);
        [DllImport("user32.dll")] public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);
        [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")] public static extern bool EmptyClipboard();
        [DllImport("user32.dll")] public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
        [DllImport("user32.dll")] public static extern bool CloseClipboard();
        [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
        [DllImport("user32.dll")] public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
        [DllImport("gdi32.dll")] public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")] public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")] public static extern IntPtr CreatePolygonRgn(POINT[] lppt, int cPoints, int fnPolyFillMode);

        [DllImport("kernel32.dll")] public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        // WinMM low-level PCM capture so Audio & Video are interleaved into ONE single file
        [DllImport("winmm.dll")] public static extern int waveInOpen(out IntPtr phwi, uint uDeviceID, ref WAVEFORMATEX pwfx, WaveInProc dwCallback, IntPtr dwInstance, int fdwOpen);
        [DllImport("winmm.dll")] public static extern int waveInPrepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);
        [DllImport("winmm.dll")] public static extern int waveInUnprepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);
        [DllImport("winmm.dll")] public static extern int waveInAddBuffer(IntPtr hwi, IntPtr pwh, int cbwh);
        [DllImport("winmm.dll")] public static extern int waveInStart(IntPtr hwi);
        [DllImport("winmm.dll")] public static extern int waveInStop(IntPtr hwi);
        [DllImport("winmm.dll")] public static extern int waveInReset(IntPtr hwi);
        [DllImport("winmm.dll")] public static extern int waveInClose(IntPtr hwi);
    }

    // ========================================================================
    // 3. PERSISTENCE & CONFIG ENGINE (fsc.db, TBSettings.db, Portable.db)
    // ========================================================================
    public enum CaptureMode
    {
        ActiveWindow = 0,
        WindowObject = 1,
        Rectangle = 2,
        Freehand = 3,
        FullScreen = 4,
        ScrollingWindow = 5,
        FixedRegion = 6,
        RepeatLast = 7
    }

    public enum VideoFormatType
    {
        MP4,
        MKV,
        AVI
    }

    public enum VideoQualityLevel
    {
        Low = 50,
        Medium = 75,
        High = 90,
        Lossless = 100
    }

    public enum OutputTarget
    {
        ToEditor,
        ToDraw,
        ToClipboard,
        ToFile,
        ToAutoSave,
        ToPrinter,
        ToWord,
        ToExcel,
        ToPowerPoint
    }

    public class AppConfig
    {
        public CaptureMode ActiveCaptureMode { get; set; } = CaptureMode.FullScreen;
        public VideoFormatType VideoFormat { get; set; } = VideoFormatType.MP4;
        public VideoQualityLevel VideoQuality { get; set; } = VideoQualityLevel.High;
        public OutputTarget ActiveOutputTarget { get; set; } = OutputTarget.ToEditor;
        public string AutoSaveFolder { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "OpenScreenRecorder", "AutoSave");
        public string FileNameTemplate { get; set; } = "OSR_$Y-$M-$D_$H-$N-$S_#";
        public string DefaultImageExtension { get; set; } = ".png";
        public bool IncludeMouseCursor { get; set; } = true;
        public bool HighlightMouseClicks { get; set; } = true;
        public bool RecordAudio { get; set; } = true;
        public int VideoFps { get; set; } = 24;
        public int AutoScrollIntervalMs { get; set; } = 250;
        public int FixedRegionWidth { get; set; } = 800;
        public int FixedRegionHeight { get; set; } = 600;
        public int CaptureDelaySeconds { get; set; } = 0;
        public int CrashFlushIntervalSeconds { get; set; } = 1;
        public int SequenceCounter { get; set; } = 1;
        public Rectangle LastCaptureRect { get; set; } = new Rectangle(0, 0, 1920, 1080);
        public CaptureMode LastCaptureMode { get; set; } = CaptureMode.FullScreen;
        public bool PortableMode { get; set; } = true;
        public string ToolbarSkin { get; set; } = "Graphicarray Abyss";
    }

    public static class StorageEngine
    {
        public static string BaseDir => AppDomain.CurrentDomain.BaseDirectory;
        public static string FscDbPath => Path.Combine(BaseDir, "fsc.db");
        public static string TbSettingsPath => Path.Combine(BaseDir, "TBSettings.db");
        public static string PortableFlagPath => Path.Combine(BaseDir, "Portable.db");
        public static string BrandLogoPath => Path.Combine(BaseDir, "brand", "logo.svg");
        public const string RegistryKeyPath = @"Software\Graphicarray\Open Screen Recorder";

        public static void EnsureBrandAndPortableFiles(AppConfig cfg)
        {
            try
            {
                string brandDir = Path.Combine(BaseDir, "brand");
                Directory.CreateDirectory(brandDir);
                if (!File.Exists(BrandLogoPath))
                    File.WriteAllText(BrandLogoPath, GaBrand.OfficialLogoSvg, Encoding.UTF8);

                if (cfg.PortableMode && !File.Exists(PortableFlagPath))
                    File.WriteAllText(PortableFlagPath, "PORTABLE=1\nBRAND=Graphicarray\nPRODUCT=Open Screen Recorder\n", Encoding.UTF8);
                else if (!cfg.PortableMode && File.Exists(PortableFlagPath))
                    File.Delete(PortableFlagPath);

                if (!string.IsNullOrWhiteSpace(cfg.AutoSaveFolder))
                    Directory.CreateDirectory(cfg.AutoSaveFolder);
            }
            catch { }
        }

        public static AppConfig Load()
        {
            AppConfig cfg = new AppConfig();
            try
            {
                cfg.PortableMode = File.Exists(PortableFlagPath) || !File.Exists(FscDbPath);
                if (File.Exists(FscDbPath))
                {
                    string json = File.ReadAllText(FscDbPath, Encoding.UTF8);
                    AppConfig loaded = JsonSerializer.Deserialize<AppConfig>(json);
                    if (loaded != null) cfg = loaded;
                }
                else if (!cfg.PortableMode)
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                    {
                        if (key != null)
                        {
                            object folder = key.GetValue("AutoSaveFolder");
                            if (folder != null) cfg.AutoSaveFolder = folder.ToString();
                        }
                    }
                }
            }
            catch { }
            EnsureBrandAndPortableFiles(cfg);
            return cfg;
        }

        public static void Save(AppConfig cfg)
        {
            try
            {
                EnsureBrandAndPortableFiles(cfg);
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(FscDbPath, JsonSerializer.Serialize(cfg, opts), Encoding.UTF8);
                File.WriteAllText(TbSettingsPath, JsonSerializer.Serialize(new
                {
                    Skin = cfg.ToolbarSkin,
                    Product = GaBrand.ProductName,
                    Owner = GaBrand.OwnerName,
                    Profile = GaBrand.LinkedInUrl,
                    UpdatedUtc = DateTime.UtcNow
                }, opts), Encoding.UTF8);

                if (!cfg.PortableMode)
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                    {
                        key?.SetValue("AutoSaveFolder", cfg.AutoSaveFolder ?? "");
                        key?.SetValue("Template", cfg.FileNameTemplate ?? "OSR_$Y-$M-$D_$H-$N-$S_#");
                    }
                }
            }
            catch { }
        }

        public static string ExpandTemplate(string template, DateTime dt, int sequence)
        {
            if (string.IsNullOrWhiteSpace(template))
                template = "OSR_$Y-$M-$D_$H-$N-$S_#";

            return template
                .Replace("$Y", dt.ToString("yyyy"))
                .Replace("$M", dt.ToString("MM"))
                .Replace("$D", dt.ToString("dd"))
                .Replace("$H", dt.ToString("HH"))
                .Replace("$N", dt.ToString("mm"))
                .Replace("$S", dt.ToString("ss"))
                .Replace("#", sequence.ToString("D3"));
        }

        public static string GetExtensionForFormat(VideoFormatType fmt) => fmt switch
        {
            VideoFormatType.MP4 => ".mp4",
            VideoFormatType.MKV => ".mkv",
            _ => ".avi"
        };

        public static string BuildAutoSaveFilePath(AppConfig cfg, string ext, string prefixOverride = null)
        {
            if (string.IsNullOrWhiteSpace(cfg.AutoSaveFolder))
            {
                cfg.AutoSaveFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "OpenScreenRecorder", "AutoSave");
            }
            Directory.CreateDirectory(cfg.AutoSaveFolder);
            string baseName = ExpandTemplate(prefixOverride ?? cfg.FileNameTemplate, DateTime.Now, cfg.SequenceCounter++);
            foreach (char c in Path.GetInvalidFileNameChars())
                baseName = baseName.Replace(c, '_');
            if (!ext.StartsWith(".")) ext = "." + ext;
            string fullPath = Path.Combine(cfg.AutoSaveFolder, baseName + ext);
            int dup = 1;
            while (File.Exists(fullPath))
            {
                fullPath = Path.Combine(cfg.AutoSaveFolder, $"{baseName}_{dup++}{ext}");
            }
            return fullPath;
        }
    }

    // ========================================================================
    // 4. SINGLE-FILE INTERLEAVED AUDIO+VIDEO CRASH-SAFE WRITER & FFMPEG MUXER
    // ========================================================================
    // Writes BOTH Video (Stream 00dc MJPG) AND Audio (Stream 01wb 44.1kHz 16-bit PCM)
    // into ONE single file on disk, flushing every 1 second for crash safety.
    // When MP4 or MKV is selected and ffmpeg.exe is available, remuxes/encodes
    // into a single .mp4 or .mkv container and removes the intermediate buffer.
    // shortcut: native fallback writes interleaved MJPEG+PCM in one container; bundled ffmpeg.exe converts to H.264 MP4/MKV.
    public sealed class CrashSafeAviWriter : IDisposable
    {
        private FileStream _fs;
        private BinaryWriter _bw;
        private readonly int _width;
        private readonly int _height;
        private readonly int _fps;
        private readonly bool _hasAudio;
        private readonly long _jpegQuality;
        private readonly ImageCodecInfo _jpegEncoder;

        private int _frameCount;
        private uint _audioBytesWritten;
        private long _riffSizePos;
        private long _avihTotalFramesPos;
        private long _vidsLengthPos;
        private long _audsLengthPos;
        private long _moviSizePos;
        private long _moviDataStart;

        private struct IndexItem
        {
            public string FourCC;
            public uint Flags;
            public uint Offset;
            public uint Length;
        }

        private readonly List<IndexItem> _indexEntries = new List<IndexItem>();
        private readonly object _sync = new object();
        private bool _finalized;

        public const int AudioSampleRate = 44100;
        public const int AudioChannels = 1;
        public const int AudioBitsPerSample = 16;

        public string FilePath { get; }
        public int FrameCount => _frameCount;

        public CrashSafeAviWriter(string filePath, int width, int height, int fps, VideoQualityLevel quality = VideoQualityLevel.High, bool includeAudio = true)
        {
            FilePath = filePath;
            _width = Math.Max(16, width);
            _height = Math.Max(16, height);
            _fps = Math.Clamp(fps, 1, 60);
            _hasAudio = includeAudio;
            _jpegQuality = (long)quality;
            _jpegEncoder = GetJpegCodec();

            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
            _fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            _bw = new BinaryWriter(_fs, Encoding.ASCII, leaveOpen: true);
            WriteInitialHeader();
            FlushCheckpoint();
        }

        private static ImageCodecInfo GetJpegCodec()
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                if (codec.FormatID == ImageFormat.Jpeg.Guid) return codec;
            return null;
        }

        private void WriteFourCC(string fourcc)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(fourcc.PadRight(4, ' ').Substring(0, 4));
            _bw.Write(bytes);
        }

        private void WriteInitialHeader()
        {
            uint microSecPerFrame = (uint)(1000000 / _fps);
            uint maxBytesPerSec = (uint)(_width * _height * 3 * _fps);
            uint streamCount = _hasAudio ? 2u : 1u;

            WriteFourCC("RIFF");
            _riffSizePos = _fs.Position;
            _bw.Write((uint)0);
            WriteFourCC("AVI ");

            // hdrl LIST
            WriteFourCC("LIST");
            long hdrlSizePos = _fs.Position;
            _bw.Write((uint)0);
            long hdrlStart = _fs.Position;
            WriteFourCC("hdrl");

            // avih chunk (56 bytes)
            WriteFourCC("avih");
            _bw.Write((uint)56);
            _bw.Write(microSecPerFrame);
            _bw.Write(maxBytesPerSec);
            _bw.Write((uint)0);
            _bw.Write((uint)0x110); // AVIF_HASINDEX | AVIF_ISINTERLEAVED
            _avihTotalFramesPos = _fs.Position;
            _bw.Write((uint)0); // dwTotalFrames
            _bw.Write((uint)0);
            _bw.Write(streamCount);
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write((uint)_width);
            _bw.Write((uint)_height);
            _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0);

            // Stream 0: Video strl LIST
            WriteFourCC("LIST");
            long vStrlSizePos = _fs.Position;
            _bw.Write((uint)0);
            long vStrlStart = _fs.Position;
            WriteFourCC("strl");

            WriteFourCC("strh");
            _bw.Write((uint)56);
            WriteFourCC("vids");
            WriteFourCC("MJPG");
            _bw.Write((uint)0);
            _bw.Write((ushort)0);
            _bw.Write((ushort)0);
            _bw.Write((uint)0);
            _bw.Write((uint)1);
            _bw.Write((uint)_fps);
            _bw.Write((uint)0);
            _vidsLengthPos = _fs.Position;
            _bw.Write((uint)0); // dwLength
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write((uint)_jpegQuality * 100);
            _bw.Write((uint)0);
            _bw.Write((short)0);
            _bw.Write((short)0);
            _bw.Write((short)_width);
            _bw.Write((short)_height);

            WriteFourCC("strf");
            _bw.Write((uint)40);
            _bw.Write((uint)40);
            _bw.Write(_width);
            _bw.Write(_height);
            _bw.Write((ushort)1);
            _bw.Write((ushort)24);
            WriteFourCC("MJPG");
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write(0);
            _bw.Write(0);
            _bw.Write((uint)0);
            _bw.Write((uint)0);

            long vStrlEnd = _fs.Position;
            _fs.Position = vStrlSizePos;
            _bw.Write((uint)(vStrlEnd - vStrlStart));
            _fs.Position = vStrlEnd;

            // Stream 1: Audio strl LIST (interleaved in the exact same file!)
            if (_hasAudio)
            {
                uint blockAlign = (uint)(AudioChannels * (AudioBitsPerSample / 8));
                uint avgBytesPerSec = AudioSampleRate * blockAlign;

                WriteFourCC("LIST");
                long aStrlSizePos = _fs.Position;
                _bw.Write((uint)0);
                long aStrlStart = _fs.Position;
                WriteFourCC("strl");

                WriteFourCC("strh");
                _bw.Write((uint)56);
                WriteFourCC("auds");
                _bw.Write((uint)1); // PCM
                _bw.Write((uint)0);
                _bw.Write((ushort)0);
                _bw.Write((ushort)0);
                _bw.Write((uint)0);
                _bw.Write(blockAlign);
                _bw.Write(avgBytesPerSec);
                _bw.Write((uint)0);
                _audsLengthPos = _fs.Position;
                _bw.Write((uint)0); // dwLength in samples
                _bw.Write(avgBytesPerSec);
                _bw.Write((uint)10000);
                _bw.Write(blockAlign);
                _bw.Write((short)0);
                _bw.Write((short)0);
                _bw.Write((short)0);
                _bw.Write((short)0);

                // WAVEFORMATEX (18 bytes)
                WriteFourCC("strf");
                _bw.Write((uint)18);
                _bw.Write((ushort)1); // WAVE_FORMAT_PCM
                _bw.Write((ushort)AudioChannels);
                _bw.Write((uint)AudioSampleRate);
                _bw.Write(avgBytesPerSec);
                _bw.Write((ushort)blockAlign);
                _bw.Write((ushort)AudioBitsPerSample);
                _bw.Write((ushort)0);

                long aStrlEnd = _fs.Position;
                _fs.Position = aStrlSizePos;
                _bw.Write((uint)(aStrlEnd - aStrlStart));
                _fs.Position = aStrlEnd;
            }

            long hdrlEnd = _fs.Position;
            _fs.Position = hdrlSizePos;
            _bw.Write((uint)(hdrlEnd - hdrlStart));
            _fs.Position = hdrlEnd;

            // movi LIST
            WriteFourCC("LIST");
            _moviSizePos = _fs.Position;
            _bw.Write((uint)0);
            _moviDataStart = _fs.Position;
            WriteFourCC("movi");
        }

        public void WriteFrame(Bitmap frame)
        {
            if (frame == null) return;
            using (MemoryStream ms = new MemoryStream())
            {
                if (_jpegEncoder != null)
                {
                    using (EncoderParameters ep = new EncoderParameters(1))
                    {
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _jpegQuality);
                        frame.Save(ms, _jpegEncoder, ep);
                    }
                }
                else
                {
                    frame.Save(ms, ImageFormat.Jpeg);
                }
                WriteVideoJpegBytes(ms.ToArray());
            }
        }

        public void WriteVideoJpegBytes(byte[] jpegBytes)
        {
            if (jpegBytes == null || jpegBytes.Length == 0) return;
            lock (_sync)
            {
                if (_finalized || _fs == null) return;
                long chunkOffset = _fs.Position - _moviDataStart;
                WriteFourCC("00dc");
                uint len = (uint)jpegBytes.Length;
                _bw.Write(len);
                _bw.Write(jpegBytes);
                if ((len & 1) == 1) _bw.Write((byte)0);
                _indexEntries.Add(new IndexItem { FourCC = "00dc", Flags = 0x10u, Offset = (uint)chunkOffset, Length = len });
                _frameCount++;
            }
        }

        // Writes captured microphone/system PCM audio directly into the SAME video file
        public void WriteAudioPcmBytes(byte[] pcmBytes, int count)
        {
            if (!_hasAudio || pcmBytes == null || count <= 0) return;
            lock (_sync)
            {
                if (_finalized || _fs == null) return;
                long chunkOffset = _fs.Position - _moviDataStart;
                WriteFourCC("01wb");
                uint len = (uint)count;
                _bw.Write(len);
                _bw.Write(pcmBytes, 0, count);
                if ((len & 1) == 1) _bw.Write((byte)0);
                _indexEntries.Add(new IndexItem { FourCC = "01wb", Flags = 0x00u, Offset = (uint)chunkOffset, Length = len });
                _audioBytesWritten += len;
            }
        }

        public void FlushCheckpoint()
        {
            lock (_sync)
            {
                if (_finalized || _fs == null) return;
                long endPos = _fs.Position;
                uint frames = (uint)Math.Max(1, _frameCount);

                _fs.Position = _avihTotalFramesPos;
                _bw.Write(frames);

                _fs.Position = _vidsLengthPos;
                _bw.Write(frames);

                if (_hasAudio && _audsLengthPos > 0)
                {
                    uint sampleCount = _audioBytesWritten / 2u;
                    _fs.Position = _audsLengthPos;
                    _bw.Write(sampleCount);
                }

                _fs.Position = _moviSizePos;
                _bw.Write((uint)(endPos - _moviDataStart));

                _fs.Position = _riffSizePos;
                _bw.Write((uint)(endPos - 8));

                _fs.Position = endPos;
                _bw.Flush();
                _fs.Flush(true);
            }
        }

        public void FinalizeStream()
        {
            lock (_sync)
            {
                if (_finalized || _fs == null) return;
                _finalized = true;
                try
                {
                    long moviEnd = _fs.Position;

                    WriteFourCC("idx1");
                    _bw.Write((uint)(_indexEntries.Count * 16));
                    foreach (var entry in _indexEntries)
                    {
                        WriteFourCC(entry.FourCC);
                        _bw.Write(entry.Flags);
                        _bw.Write(entry.Offset);
                        _bw.Write(entry.Length);
                    }

                    long fileEnd = _fs.Position;
                    uint frames = (uint)Math.Max(1, _frameCount);

                    _fs.Position = _avihTotalFramesPos;
                    _bw.Write(frames);

                    _fs.Position = _vidsLengthPos;
                    _bw.Write(frames);

                    if (_hasAudio && _audsLengthPos > 0)
                    {
                        uint sampleCount = _audioBytesWritten / 2u;
                        _fs.Position = _audsLengthPos;
                        _bw.Write(sampleCount);
                    }

                    _fs.Position = _moviSizePos;
                    _bw.Write((uint)(moviEnd - _moviDataStart));

                    _fs.Position = _riffSizePos;
                    _bw.Write((uint)(fileEnd - 8));

                    _bw.Flush();
                    _fs.Flush(true);
                }
                catch { }
                finally
                {
                    try { _bw?.Dispose(); } catch { }
                    try { _fs?.Dispose(); } catch { }
                    _bw = null;
                    _fs = null;
                }
            }
        }

        public void Dispose() => FinalizeStream();
    }

    // ========================================================================
    // 5. WINMM PCM AUDIO STREAMER (Feeds directly into CrashSafeAviWriter)
    // ========================================================================
    public sealed class WinMmAudioStreamer : IDisposable
    {
        private IntPtr _hWaveIn = IntPtr.Zero;
        private readonly NativeMethods.WaveInProc _callback;
        private readonly List<IntPtr> _headers = new List<IntPtr>();
        private readonly List<IntPtr> _buffers = new List<IntPtr>();
        private readonly Action<byte[], int> _onAudioChunk;
        private volatile bool _recording;
        private volatile bool _paused;

        public WinMmAudioStreamer(Action<byte[], int> onAudioChunk)
        {
            _onAudioChunk = onAudioChunk;
            _callback = WaveInCallback;
        }

        public bool Start()
        {
            try
            {
                NativeMethods.WAVEFORMATEX fmt = new NativeMethods.WAVEFORMATEX
                {
                    wFormatTag = 1, // WAVE_FORMAT_PCM
                    nChannels = CrashSafeAviWriter.AudioChannels,
                    nSamplesPerSec = CrashSafeAviWriter.AudioSampleRate,
                    wBitsPerSample = CrashSafeAviWriter.AudioBitsPerSample,
                    nBlockAlign = (ushort)(CrashSafeAviWriter.AudioChannels * (CrashSafeAviWriter.AudioBitsPerSample / 8)),
                    cbSize = 0
                };
                fmt.nAvgBytesPerSec = fmt.nSamplesPerSec * fmt.nBlockAlign;

                int res = NativeMethods.waveInOpen(out _hWaveIn, unchecked((uint)-1), ref fmt, _callback, IntPtr.Zero, NativeMethods.CALLBACK_FUNCTION);
                if (res != 0 || _hWaveIn == IntPtr.Zero) return false;

                int bufSize = 8820; // 0.1s of 44.1kHz 16-bit mono PCM
                for (int i = 0; i < 4; i++)
                {
                    IntPtr dataPtr = Marshal.AllocHGlobal(bufSize);
                    NativeMethods.WAVEHDR hdr = new NativeMethods.WAVEHDR
                    {
                        lpData = dataPtr,
                        dwBufferLength = (uint)bufSize
                    };
                    IntPtr hdrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.WAVEHDR)));
                    Marshal.StructureToPtr(hdr, hdrPtr, false);

                    NativeMethods.waveInPrepareHeader(_hWaveIn, hdrPtr, Marshal.SizeOf(typeof(NativeMethods.WAVEHDR)));
                    NativeMethods.waveInAddBuffer(_hWaveIn, hdrPtr, Marshal.SizeOf(typeof(NativeMethods.WAVEHDR)));

                    _buffers.Add(dataPtr);
                    _headers.Add(hdrPtr);
                }

                _recording = true;
                _paused = false;
                NativeMethods.waveInStart(_hWaveIn);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetPaused(bool paused) => _paused = paused;

        private void WaveInCallback(IntPtr hwi, int uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
        {
            if (uMsg != NativeMethods.MM_WIM_DATA || !_recording || dwParam1 == IntPtr.Zero) return;
            try
            {
                NativeMethods.WAVEHDR hdr = Marshal.PtrToStructure<NativeMethods.WAVEHDR>(dwParam1);
                if (!_paused && hdr.dwBytesRecorded > 0)
                {
                    byte[] pcm = new byte[hdr.dwBytesRecorded];
                    Marshal.Copy(hdr.lpData, pcm, 0, (int)hdr.dwBytesRecorded);
                    _onAudioChunk?.Invoke(pcm, (int)hdr.dwBytesRecorded);
                }
                if (_recording && _hWaveIn != IntPtr.Zero)
                {
                    NativeMethods.waveInAddBuffer(_hWaveIn, dwParam1, Marshal.SizeOf(typeof(NativeMethods.WAVEHDR)));
                }
            }
            catch { }
        }

        public void Stop()
        {
            if (!_recording && _hWaveIn == IntPtr.Zero) return;
            _recording = false;
            try
            {
                if (_hWaveIn != IntPtr.Zero)
                {
                    NativeMethods.waveInStop(_hWaveIn);
                    NativeMethods.waveInReset(_hWaveIn);
                    foreach (IntPtr hdrPtr in _headers)
                        NativeMethods.waveInUnprepareHeader(_hWaveIn, hdrPtr, Marshal.SizeOf(typeof(NativeMethods.WAVEHDR)));
                    NativeMethods.waveInClose(_hWaveIn);
                    _hWaveIn = IntPtr.Zero;
                }
            }
            catch { }
            finally
            {
                foreach (IntPtr hdrPtr in _headers) Marshal.FreeHGlobal(hdrPtr);
                foreach (IntPtr bufPtr in _buffers) Marshal.FreeHGlobal(bufPtr);
                _headers.Clear();
                _buffers.Clear();
            }
        }

        public void Dispose() => Stop();
    }

    // ========================================================================
    // 6. CRASH-SAFE GUARD & VIDEO RECORDER (Supports All 8 Modes + Pause/Resume)
    // ========================================================================
    public static class CrashSafeGuard
    {
        private static readonly object Sync = new object();
        private static CrashSafeVideoRecorder _activeRecorder;
        private static Bitmap _unsavedEditorBitmap;
        private static AppConfig _config;
        private static NativeMethods.ConsoleCtrlDelegate _consoleHandler;
        private static bool _hooksInstalled;
        public static string LastEmergencySavedPath { get; private set; }

        public static void Initialize(AppConfig config)
        {
            _config = config;
            if (_hooksInstalled) return;
            _hooksInstalled = true;

            Application.ApplicationExit += (s, e) => EmergencySaveAll("ApplicationExit");
            Application.ThreadException += (s, e) => EmergencySaveAll("ThreadException");
            AppDomain.CurrentDomain.UnhandledException += (s, e) => EmergencySaveAll("UnhandledException");
            AppDomain.CurrentDomain.ProcessExit += (s, e) => EmergencySaveAll("ProcessExit");
            SystemEvents.SessionEnding += (s, e) => EmergencySaveAll("SessionEnding");

            try
            {
                _consoleHandler = sig =>
                {
                    EmergencySaveAll("ConsoleCtrlSignal_" + sig);
                    return false;
                };
                NativeMethods.SetConsoleCtrlHandler(_consoleHandler, true);
            }
            catch { }
        }

        public static void RegisterActiveRecorder(CrashSafeVideoRecorder recorder)
        {
            lock (Sync) { _activeRecorder = recorder; }
        }

        public static void UnregisterActiveRecorder(CrashSafeVideoRecorder recorder)
        {
            lock (Sync)
            {
                if (ReferenceEquals(_activeRecorder, recorder))
                    _activeRecorder = null;
            }
        }

        public static void SetUnsavedEditorBitmap(Bitmap bmp)
        {
            lock (Sync) { _unsavedEditorBitmap = bmp; }
        }

        public static void EmergencySaveAll(string reason)
        {
            lock (Sync)
            {
                try
                {
                    if (_activeRecorder != null && _activeRecorder.IsRecording)
                    {
                        string savedVideo = _activeRecorder.StopAndSave(emergency: true, reason: reason);
                        if (!string.IsNullOrEmpty(savedVideo))
                            LastEmergencySavedPath = savedVideo;
                        _activeRecorder = null;
                    }

                    if (_unsavedEditorBitmap != null && _config != null)
                    {
                        string path = StorageEngine.BuildAutoSaveFilePath(_config, ".png", "OSR_RecoveredEdit_$Y-$M-$D_$H-$N-$S_#");
                        _unsavedEditorBitmap.Save(path, ImageFormat.Png);
                        LastEmergencySavedPath = path;
                        _unsavedEditorBitmap = null;
                    }

                    if (_config != null)
                        StorageEngine.Save(_config);
                }
                catch { }
            }
        }
    }

    public sealed class CrashSafeVideoRecorder : IDisposable
    {
        private readonly AppConfig _cfg;
        private Rectangle _captureBounds;
        private readonly CaptureMode _mode;
        private readonly IntPtr _targetWindowHwnd;
        private readonly Point[] _freehandPolygon;

        private CrashSafeAviWriter _writer;
        private WinMmAudioStreamer _audioStreamer;
        private Thread _workerThread;
        private volatile bool _running;
        private volatile bool _paused;
        private readonly Stopwatch _activeTimer = new Stopwatch();
        private DateTime _lastFlushUtc;
        private DateTime _lastAutoScrollUtc;
        private string _finalTargetFilePath;

        public bool IsRecording => _running;
        public bool IsPaused => _paused;
        public string ActiveVideoPath => _finalTargetFilePath ?? _writer?.FilePath;
        public int RecordedFrames => _writer?.FrameCount ?? 0;
        public TimeSpan Elapsed => _activeTimer.Elapsed;

        public event Action<int, TimeSpan, string> OnProgress;

        public CrashSafeVideoRecorder(
            AppConfig cfg,
            CaptureMode mode,
            Rectangle captureBounds,
            IntPtr targetWindowHwnd = default,
            Point[] freehandPolygon = null)
        {
            _cfg = cfg;
            _mode = mode;
            _targetWindowHwnd = targetWindowHwnd;
            _freehandPolygon = freehandPolygon;
            _captureBounds = captureBounds.Width > 16 && captureBounds.Height > 16
                ? captureBounds
                : ScreenCaptureEngine.GetVirtualDesktopBounds();
        }

        public string Start()
        {
            if (_running) return ActiveVideoPath;

            int w = _captureBounds.Width & ~1;
            int h = _captureBounds.Height & ~1;
            string desiredExt = StorageEngine.GetExtensionForFormat(_cfg.VideoFormat);
            _finalTargetFilePath = StorageEngine.BuildAutoSaveFilePath(_cfg, desiredExt, "OSR_Video_$Y-$M-$D_$H-$N-$S_#");

            // Write live crash-safe interleaved A/V stream directly in the user's selected folder
            string liveStreamPath = _cfg.VideoFormat == VideoFormatType.AVI
                ? _finalTargetFilePath
                : Path.ChangeExtension(_finalTargetFilePath, ".live.avi");

            _writer = new CrashSafeAviWriter(liveStreamPath, w, h, _cfg.VideoFps, _cfg.VideoQuality, _cfg.RecordAudio);
            _running = true;
            _paused = false;
            _activeTimer.Restart();
            _lastFlushUtc = DateTime.UtcNow;
            _lastAutoScrollUtc = DateTime.UtcNow;

            if (_cfg.RecordAudio)
            {
                _audioStreamer = new WinMmAudioStreamer((pcm, count) => _writer?.WriteAudioPcmBytes(pcm, count));
                _audioStreamer.Start();
            }

            CrashSafeGuard.RegisterActiveRecorder(this);

            _workerThread = new Thread(CaptureLoop)
            {
                IsBackground = false,
                Name = "OpenScreenRecorder_CrashSafeWorker"
            };
            _workerThread.Start();

            return _finalTargetFilePath;
        }

        private void CaptureLoop()
        {
            int intervalMs = Math.Max(15, 1000 / Math.Clamp(_cfg.VideoFps, 1, 60));
            Stopwatch frameSw = new Stopwatch();
            int targetW = _captureBounds.Width & ~1;
            int targetH = _captureBounds.Height & ~1;

            while (_running)
            {
                frameSw.Restart();
                if (!_paused)
                {
                    try
                    {
                        // Dynamic window tracking for ActiveWindow / WindowObject video recording
                        if (_mode == CaptureMode.ActiveWindow)
                        {
                            IntPtr fg = NativeMethods.GetForegroundWindow();
                            if (fg != IntPtr.Zero && NativeMethods.GetWindowRect(fg, out NativeMethods.RECT r))
                            {
                                int rw = r.Right - r.Left;
                                int rh = r.Bottom - r.Top;
                                if (rw > 32 && rh > 32)
                                    _captureBounds = new Rectangle(r.Left, r.Top, targetW, targetH);
                            }
                        }
                        else if ((_mode == CaptureMode.WindowObject || _mode == CaptureMode.ScrollingWindow) && _targetWindowHwnd != IntPtr.Zero)
                        {
                            if (NativeMethods.GetWindowRect(_targetWindowHwnd, out NativeMethods.RECT r))
                            {
                                _captureBounds = new Rectangle(r.Left, r.Top, targetW, targetH);
                            }
                            if (_mode == CaptureMode.ScrollingWindow &&
                                (DateTime.UtcNow - _lastAutoScrollUtc).TotalMilliseconds >= Math.Max(200, _cfg.AutoScrollIntervalMs))
                            {
                                NativeMethods.PostMessage(_targetWindowHwnd, NativeMethods.WM_VSCROLL, (IntPtr)NativeMethods.SB_PAGEDOWN, IntPtr.Zero);
                                _lastAutoScrollUtc = DateTime.UtcNow;
                            }
                        }

                        using (Bitmap rawFrame = ScreenCaptureEngine.CaptureRegion(
                            new Rectangle(_captureBounds.X, _captureBounds.Y, targetW, targetH),
                            _cfg.IncludeMouseCursor,
                            _cfg.HighlightMouseClicks))
                        {
                            if (_mode == CaptureMode.Freehand && _freehandPolygon != null && _freehandPolygon.Length >= 3)
                            {
                                using (Bitmap masked = ScreenCaptureEngine.ApplyPolygonMaskInPlace(rawFrame, _freehandPolygon))
                                    _writer?.WriteFrame(masked);
                            }
                            else
                            {
                                _writer?.WriteFrame(rawFrame);
                            }
                        }

                        if ((DateTime.UtcNow - _lastFlushUtc).TotalSeconds >= Math.Max(1, _cfg.CrashFlushIntervalSeconds))
                        {
                            _writer?.FlushCheckpoint();
                            _lastFlushUtc = DateTime.UtcNow;
                        }

                        OnProgress?.Invoke(RecordedFrames, Elapsed, _finalTargetFilePath);
                    }
                    catch { }
                }

                int sleep = intervalMs - (int)frameSw.ElapsedMilliseconds;
                if (sleep > 1) Thread.Sleep(sleep);
            }
        }

        public void TogglePause()
        {
            if (!_running) return;
            _paused = !_paused;
            if (_paused)
            {
                _activeTimer.Stop();
                _audioStreamer?.SetPaused(true);
                _writer?.FlushCheckpoint();
            }
            else
            {
                _audioStreamer?.SetPaused(false);
                _activeTimer.Start();
            }
        }

        public string StopAndSave(bool emergency = false, string reason = "UserStop")
        {
            if (!_running && _writer == null) return _finalTargetFilePath;
            _running = false;
            _activeTimer.Stop();

            try { _audioStreamer?.Stop(); } catch { }
            _audioStreamer = null;

            try
            {
                if (_workerThread != null && _workerThread.IsAlive && Thread.CurrentThread != _workerThread)
                    _workerThread.Join(1500);
            }
            catch { }

            string liveAviPath = _writer?.FilePath;
            try { _writer?.FinalizeStream(); } catch { }
            _writer = null;

            string finalFile = FinalizeContainerFormat(liveAviPath, _finalTargetFilePath, _cfg.VideoFormat, _cfg.VideoQuality);

            CrashSafeGuard.UnregisterActiveRecorder(this);
            StorageEngine.Save(_cfg);
            return finalFile;
        }

        private static string FinalizeContainerFormat(string liveAviPath, string desiredPath, VideoFormatType fmt, VideoQualityLevel quality)
        {
            if (string.IsNullOrEmpty(liveAviPath) || !File.Exists(liveAviPath))
                return desiredPath;

            if (fmt == VideoFormatType.AVI || string.Equals(liveAviPath, desiredPath, StringComparison.OrdinalIgnoreCase))
                return liveAviPath;

            // Convert/Mux single-file AVI (video+audio) into single-file .mp4 or .mkv via local ffmpeg.exe if present
            try
            {
                string ffmpegPath = FindLocalFfmpeg();
                if (!string.IsNullOrEmpty(ffmpegPath))
                {
                    int crf = quality switch
                    {
                        VideoQualityLevel.Low => 28,
                        VideoQualityLevel.Medium => 23,
                        VideoQualityLevel.High => 18,
                        VideoQualityLevel.Lossless => 0,
                        _ => 18
                    };
                    string args = $"-y -i \"{liveAviPath}\" -c:v libx264 -preset ultrafast -crf {crf} -pix_fmt yuv420p -c:a aac -b:a 128k \"{desiredPath}\"";
                    using (Process p = Process.Start(new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = args,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }))
                    {
                        if (p != null && p.WaitForExit(15000) && File.Exists(desiredPath) && new FileInfo(desiredPath).Length > 0)
                        {
                            try { File.Delete(liveAviPath); } catch { }
                            return desiredPath;
                        }
                    }
                }

                // Fallback if ffmpeg.exe is not in folder: rename stream directly to requested target path so file is never lost
                if (File.Exists(desiredPath)) File.Delete(desiredPath);
                File.Move(liveAviPath, desiredPath);
                return desiredPath;
            }
            catch
            {
                return liveAviPath;
            }
        }

        private static string FindLocalFfmpeg()
        {
            string local = Path.Combine(StorageEngine.BaseDir, "ffmpeg.exe");
            if (File.Exists(local)) return local;
            return null;
        }

        public void Dispose() => StopAndSave(emergency: true, reason: "Dispose");
    }

    // ========================================================================
    // 7. CORE GDI SCREEN ACQUISITION & SCROLLING STITCH ENGINE
    // ========================================================================
    public static class ScreenCaptureEngine
    {
        public static Rectangle GetVirtualDesktopBounds()
        {
            int x = NativeMethods.GetSystemMetrics(NativeMethods.SM_XVIRTUALSCREEN);
            int y = NativeMethods.GetSystemMetrics(NativeMethods.SM_YVIRTUALSCREEN);
            int w = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXVIRTUALSCREEN);
            int h = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYVIRTUALSCREEN);
            if (w <= 0 || h <= 0)
                return Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1920, 1080);
            return new Rectangle(x, y, w, h);
        }

        public static Bitmap CaptureRegion(Rectangle rect, bool includeCursor, bool highlightCursorHalo = false)
        {
            int w = Math.Max(1, rect.Width);
            int h = Math.Max(1, rect.Height);

            IntPtr hdcScreen = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr hdcMem = NativeMethods.CreateCompatibleDC(hdcScreen);

            NativeMethods.BITMAPINFO bmi = new NativeMethods.BITMAPINFO();
            bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = w;
            bmi.bmiHeader.biHeight = -h;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = 32;

            IntPtr ppvBits;
            IntPtr hDib = NativeMethods.CreateDIBSection(hdcScreen, ref bmi, 0, out ppvBits, IntPtr.Zero, 0);
            IntPtr hOld = NativeMethods.SelectObject(hdcMem, hDib);

            NativeMethods.BitBlt(hdcMem, 0, 0, w, h, hdcScreen, rect.X, rect.Y,
                NativeMethods.SRCCOPY | NativeMethods.CAPTUREBLT);

            if (includeCursor)
            {
                NativeMethods.CURSORINFO ci = new NativeMethods.CURSORINFO();
                ci.cbSize = Marshal.SizeOf(typeof(NativeMethods.CURSORINFO));
                if (NativeMethods.GetCursorInfo(out ci) && (ci.flags & NativeMethods.CURSOR_SHOWING) != 0)
                {
                    int cx = ci.ptScreenPos.X - rect.X;
                    int cy = ci.ptScreenPos.Y - rect.Y;
                    if (NativeMethods.GetIconInfo(ci.hCursor, out NativeMethods.ICONINFO ii))
                    {
                        cx -= ii.xHotspot;
                        cy -= ii.yHotspot;
                        if (ii.hbmMask != IntPtr.Zero) NativeMethods.DeleteObject(ii.hbmMask);
                        if (ii.hbmColor != IntPtr.Zero) NativeMethods.DeleteObject(ii.hbmColor);
                    }
                    NativeMethods.DrawIconEx(hdcMem, cx, cy, ci.hCursor, 0, 0, 0, IntPtr.Zero, NativeMethods.DI_NORMAL);
                }
            }

            Bitmap result = Image.FromHbitmap(hDib);

            NativeMethods.SelectObject(hdcMem, hOld);
            NativeMethods.DeleteObject(hDib);
            NativeMethods.DeleteDC(hdcMem);
            NativeMethods.ReleaseDC(IntPtr.Zero, hdcScreen);

            if (includeCursor && highlightCursorHalo)
            {
                Point cursorPt = Cursor.Position;
                int rx = cursorPt.X - rect.X;
                int ry = cursorPt.Y - rect.Y;
                if (rx >= 0 && ry >= 0 && rx < w && ry < h)
                {
                    using (Graphics g = Graphics.FromImage(result))
                    using (Pen haloPen = new Pen(Color.FromArgb(160, GaBrand.Cyan), 3f))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawEllipse(haloPen, rx - 14, ry - 14, 28, 28);
                    }
                }
            }

            return result;
        }

        public static Bitmap ApplyPolygonMaskInPlace(Bitmap frame, Point[] localPolygon)
        {
            Bitmap masked = new Bitmap(frame.Width, frame.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(masked))
            using (GraphicsPath path = new GraphicsPath())
            {
                g.Clear(GaBrand.Abyss);
                path.AddPolygon(localPolygon);
                g.SetClip(path);
                g.DrawImage(frame, 0, 0);
            }
            return masked;
        }

        public static Bitmap CaptureActiveWindow(bool includeCursor, out Rectangle capturedRect)
        {
            IntPtr hwnd = NativeMethods.GetForegroundWindow();
            if (hwnd != IntPtr.Zero && NativeMethods.GetWindowRect(hwnd, out NativeMethods.RECT r))
            {
                capturedRect = new Rectangle(r.Left, r.Top, Math.Max(1, r.Right - r.Left), Math.Max(1, r.Bottom - r.Top));
                return CaptureRegion(capturedRect, includeCursor);
            }
            capturedRect = GetVirtualDesktopBounds();
            return CaptureRegion(capturedRect, includeCursor);
        }

        public static Bitmap CaptureFreehandPolygon(Bitmap fullScreen, Point[] polygonPoints, out Rectangle bounds, out Point[] localPolygon)
        {
            if (polygonPoints == null || polygonPoints.Length < 3)
            {
                bounds = new Rectangle(0, 0, fullScreen.Width, fullScreen.Height);
                localPolygon = Array.Empty<Point>();
                return (Bitmap)fullScreen.Clone();
            }

            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (Point p in polygonPoints)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            bounds = Rectangle.Intersect(new Rectangle(0, 0, fullScreen.Width, fullScreen.Height),
                new Rectangle(minX, minY, Math.Max(16, (maxX - minX) & ~1), Math.Max(16, (maxY - minY) & ~1)));

            localPolygon = new Point[polygonPoints.Length];
            for (int i = 0; i < polygonPoints.Length; i++)
                localPolygon[i] = new Point(polygonPoints[i].X - bounds.X, polygonPoints[i].Y - bounds.Y);

            Bitmap cropped = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(cropped))
            using (GraphicsPath path = new GraphicsPath())
            {
                g.Clear(GaBrand.Abyss);
                path.AddPolygon(localPolygon);
                g.SetClip(path);
                g.DrawImage(fullScreen, new Rectangle(0, 0, bounds.Width, bounds.Height), bounds, GraphicsUnit.Pixel);
            }
            return cropped;
        }

        public static Bitmap CaptureScrollingWindow(IntPtr targetHwnd, Rectangle viewportRect, int scrollIntervalMs, int maxPages = 8)
        {
            List<Bitmap> slices = new List<Bitmap>();
            try
            {
                NativeMethods.SetForegroundWindow(targetHwnd);
                Thread.Sleep(120);

                for (int i = 0; i < maxPages; i++)
                {
                    Bitmap frame = CaptureRegion(viewportRect, includeCursor: false);
                    if (slices.Count > 0)
                    {
                        int shift = ComputeVerticalScrollShift(slices[slices.Count - 1], frame);
                        if (shift <= 4)
                        {
                            frame.Dispose();
                            break;
                        }
                    }
                    slices.Add(frame);
                    NativeMethods.PostMessage(targetHwnd, NativeMethods.WM_VSCROLL, (IntPtr)NativeMethods.SB_PAGEDOWN, IntPtr.Zero);
                    Thread.Sleep(Math.Clamp(scrollIntervalMs, 100, 2000));
                }

                return StitchVerticalSlices(slices);
            }
            finally
            {
                foreach (Bitmap b in slices) b.Dispose();
            }
        }

        public static int ComputeVerticalScrollShift(Bitmap frameA, Bitmap frameB)
        {
            if (frameA == null || frameB == null || frameA.Width != frameB.Width || frameA.Height != frameB.Height)
                return 0;

            int h = frameA.Height;
            int w = frameA.Width;
            int stripHeight = Math.Min(24, h / 4);
            if (stripHeight < 4) return 0;

            int refY = Math.Max(0, h - stripHeight - 16);
            int bestMatchY = refY;
            long bestScore = long.MaxValue;

            for (int candidateY = 0; candidateY <= refY; candidateY += 2)
            {
                long diff = 0;
                for (int dy = 0; dy < stripHeight; dy += 4)
                {
                    for (int x = w / 6; x < (w * 5) / 6; x += 16)
                    {
                        Color cA = frameA.GetPixel(x, refY + dy);
                        Color cB = frameB.GetPixel(x, candidateY + dy);
                        diff += Math.Abs(cA.R - cB.R) + Math.Abs(cA.G - cB.G) + Math.Abs(cA.B - cB.B);
                    }
                }
                if (diff < bestScore)
                {
                    bestScore = diff;
                    bestMatchY = candidateY;
                }
            }

            return refY - bestMatchY;
        }

        public static Bitmap StitchVerticalSlices(IList<Bitmap> slices)
        {
            if (slices == null || slices.Count == 0)
                return new Bitmap(1, 1);
            if (slices.Count == 1)
                return (Bitmap)slices[0].Clone();

            int w = slices[0].Width;
            int h = slices[0].Height;
            List<int> shifts = new List<int> { h };
            int totalHeight = h;

            for (int i = 1; i < slices.Count; i++)
            {
                int shift = ComputeVerticalScrollShift(slices[i - 1], slices[i]);
                if (shift <= 0 || shift > h) shift = h / 2;
                shifts.Add(shift);
                totalHeight += shift;
            }

            Bitmap stitched = new Bitmap(w, totalHeight, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(stitched))
            {
                g.DrawImage(slices[0], 0, 0);
                int yCursor = h;
                for (int i = 1; i < slices.Count; i++)
                {
                    int shift = shifts[i];
                    int srcY = h - shift;
                    g.DrawImage(slices[i],
                        new Rectangle(0, yCursor, w, shift),
                        new Rectangle(0, srcY, w, shift),
                        GraphicsUnit.Pixel);
                    yCursor += shift;
                }
            }
            return stitched;
        }

        public static void ApplyObscureBlur(Bitmap bmp, Rectangle region, int blockSize = 10)
        {
            if (bmp == null) return;
            Rectangle safe = Rectangle.Intersect(new Rectangle(0, 0, bmp.Width, bmp.Height), region);
            if (safe.Width <= 0 || safe.Height <= 0) return;
            blockSize = Math.Max(2, blockSize);

            for (int y = safe.Top; y < safe.Bottom; y += blockSize)
            {
                for (int x = safe.Left; x < safe.Right; x += blockSize)
                {
                    int bw = Math.Min(blockSize, safe.Right - x);
                    int bh = Math.Min(blockSize, safe.Bottom - y);
                    Color sample = bmp.GetPixel(x + bw / 2, y + bh / 2);
                    for (int py = y; py < y + bh; py++)
                        for (int px = x; px < x + bw; px++)
                            bmp.SetPixel(px, py, sample);
                }
            }
        }
    }

    // ========================================================================
    // 8. OFFLINE OUTPUT DISPATCHER (OutputBtn Menu — 100% Offline)
    // ========================================================================
    public static class OutputDispatcher
    {
        public static string Dispatch(Bitmap captured, AppConfig cfg, Form owner)
        {
            if (captured == null) return null;

            string recoveryPath = StorageEngine.BuildAutoSaveFilePath(cfg, cfg.DefaultImageExtension);
            SaveImageByExtension(captured, recoveryPath);

            switch (cfg.ActiveOutputTarget)
            {
                case OutputTarget.ToEditor:
                    using (EditorForm editor = new EditorForm(captured, cfg, startInDrawMode: false))
                        editor.ShowDialog(owner);
                    return recoveryPath;

                case OutputTarget.ToDraw:
                    using (EditorForm drawBoard = new EditorForm(captured, cfg, startInDrawMode: true))
                        drawBoard.ShowDialog(owner);
                    return recoveryPath;

                case OutputTarget.ToClipboard:
                    CopyBitmapToClipboardWin32(captured);
                    return recoveryPath;

                case OutputTarget.ToFile:
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Title = "Open Screen Recorder — Save Image Offline";
                        sfd.InitialDirectory = cfg.AutoSaveFolder;
                        sfd.FileName = Path.GetFileName(recoveryPath);
                        sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif|PDF Document (*.pdf)|*.pdf";
                        if (sfd.ShowDialog(owner) == DialogResult.OK)
                        {
                            SaveImageByExtension(captured, sfd.FileName);
                            return sfd.FileName;
                        }
                    }
                    return recoveryPath;

                case OutputTarget.ToAutoSave:
                    return recoveryPath;

                case OutputTarget.ToPrinter:
                    using (PrintDocument pd = new PrintDocument())
                    {
                        pd.DocumentName = "Open Screen Recorder Print";
                        pd.PrintPage += (s, e) =>
                        {
                            e.Graphics.DrawImage(captured, e.MarginBounds);
                            using (Font f = GaBrand.MakeFont(10f))
                            using (SolidBrush sb = new SolidBrush(GaBrand.Slate))
                                e.Graphics.DrawString("linkedin.com/in/graphicarray", f, sb, e.MarginBounds.Left, e.MarginBounds.Bottom + 6);
                        };
                        using (PrintDialog dlg = new PrintDialog { Document = pd })
                        {
                            if (dlg.ShowDialog(owner) == DialogResult.OK)
                                pd.Print();
                        }
                    }
                    return recoveryPath;

                case OutputTarget.ToWord:
                    SendToOfficeOffline("Word.Application", recoveryPath);
                    return recoveryPath;

                case OutputTarget.ToExcel:
                    SendToOfficeOffline("Excel.Application", recoveryPath);
                    return recoveryPath;

                case OutputTarget.ToPowerPoint:
                    SendToOfficeOffline("PowerPoint.Application", recoveryPath);
                    return recoveryPath;

                default:
                    return recoveryPath;
            }
        }

        public static void CopyBitmapToClipboardWin32(Bitmap bmp)
        {
            IntPtr hBitmap = bmp.GetHbitmap();
            try
            {
                if (NativeMethods.OpenClipboard(IntPtr.Zero))
                {
                    NativeMethods.EmptyClipboard();
                    NativeMethods.SetClipboardData(NativeMethods.CF_BITMAP, hBitmap);
                    NativeMethods.CloseClipboard();
                    hBitmap = IntPtr.Zero;
                }
            }
            finally
            {
                if (hBitmap != IntPtr.Zero) NativeMethods.DeleteObject(hBitmap);
            }
        }

        public static void SaveImageByExtension(Bitmap bmp, string filePath)
        {
            string ext = (Path.GetExtension(filePath) ?? ".png").ToLowerInvariant();
            if (ext == ".pdf")
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                    pd.PrinterSettings.PrintToFile = true;
                    pd.PrinterSettings.PrintFileName = filePath;
                    pd.PrintPage += (s, e) => e.Graphics.DrawImage(bmp, e.PageBounds);
                    try { pd.Print(); return; } catch { }
                }
                bmp.Save(filePath, ImageFormat.Png);
                return;
            }

            ImageFormat fmt = ext switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tif" or ".tiff" => ImageFormat.Tiff,
                _ => ImageFormat.Png
            };
            bmp.Save(filePath, fmt);
        }

        private static void SendToOfficeOffline(string progId, string imagePath)
        {
            try
            {
                Type comType = Type.GetTypeFromProgID(progId);
                if (comType == null) return;
                dynamic app = Activator.CreateInstance(comType);
                app.Visible = true;
                if (progId.StartsWith("Word"))
                {
                    app.Documents.Add();
                    app.Selection.InlineShapes.AddPicture(imagePath);
                }
                else if (progId.StartsWith("Excel"))
                {
                    dynamic wb = app.Workbooks.Add();
                    wb.ActiveSheet.Pictures.Insert(imagePath);
                }
                else if (progId.StartsWith("PowerPoint"))
                {
                    dynamic pres = app.Presentations.Add();
                    dynamic slide = pres.Slides.Add(1, 12);
                    slide.Shapes.AddPicture(imagePath, 0, -1, 40, 40);
                }
            }
            catch { }
        }
    }

    // ========================================================================
    // 9. INTERACTIVE OVERLAY FORMS (Works for BOTH Video Area & Screenshot)
    // ========================================================================
    public sealed class RegionOverlayForm : Form
    {
        private readonly Bitmap _frozenDesktop;
        private readonly CaptureMode _mode;
        private readonly Size _fixedSize;
        private bool _dragging;
        private Point _startPt;
        private Point _currentPt;
        private readonly List<Point> _freehandPts = new List<Point>();
        private Rectangle _spyWindowRect;

        public Bitmap CapturedBitmap { get; private set; }
        public Rectangle SelectedBounds { get; private set; }
        public IntPtr SelectedWindowHandle { get; private set; }
        public Point[] LocalFreehandPolygon { get; private set; }

        public RegionOverlayForm(CaptureMode mode, Size fixedSize)
        {
            _mode = mode;
            _fixedSize = fixedSize;
            Rectangle virtualBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
            _frozenDesktop = ScreenCaptureEngine.CaptureRegion(virtualBounds, includeCursor: false);

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = virtualBounds;
            TopMost = true;
            DoubleBuffered = true;
            Cursor = Cursors.Cross;
            KeyPreview = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }
            _dragging = true;
            _startPt = e.Location;
            _currentPt = e.Location;
            _freehandPts.Clear();
            _freehandPts.Add(e.Location);

            if (_mode == CaptureMode.FixedRegion)
            {
                SelectedBounds = new Rectangle(e.X - _fixedSize.Width / 2, e.Y - _fixedSize.Height / 2, _fixedSize.Width, _fixedSize.Height);
                FinishSelection();
            }
            else if (_mode == CaptureMode.WindowObject || _mode == CaptureMode.ScrollingWindow)
            {
                SelectedBounds = _spyWindowRect.Width > 4 ? _spyWindowRect : new Rectangle(0, 0, Width, Height);
                FinishSelection();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _currentPt = e.Location;
            if (_mode == CaptureMode.WindowObject || _mode == CaptureMode.ScrollingWindow)
            {
                NativeMethods.POINT pt = new NativeMethods.POINT { X = Cursor.Position.X, Y = Cursor.Position.Y };
                Opacity = 0.01;
                IntPtr hwnd = NativeMethods.WindowFromPoint(pt);
                if (hwnd != IntPtr.Zero)
                {
                    NativeMethods.POINT clientPt = pt;
                    NativeMethods.ScreenToClient(hwnd, ref clientPt);
                    IntPtr child = NativeMethods.ChildWindowFromPointEx(hwnd, clientPt, 0);
                    if (child != IntPtr.Zero) hwnd = child;
                    SelectedWindowHandle = hwnd;
                    if (NativeMethods.GetWindowRect(hwnd, out NativeMethods.RECT r))
                    {
                        _spyWindowRect = new Rectangle(
                            r.Left - Bounds.X, r.Top - Bounds.Y,
                            Math.Max(16, (r.Right - r.Left) & ~1), Math.Max(16, (r.Bottom - r.Top) & ~1));
                    }
                }
                Opacity = 1.0;
            }
            else if (_dragging && _mode == CaptureMode.Freehand)
            {
                _freehandPts.Add(e.Location);
            }
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!_dragging) return;
            _dragging = false;

            if (_mode == CaptureMode.Freehand)
            {
                CapturedBitmap = ScreenCaptureEngine.CaptureFreehandPolygon(_frozenDesktop, _freehandPts.ToArray(), out Rectangle b, out Point[] localPoly);
                LocalFreehandPolygon = localPoly;
                SelectedBounds = new Rectangle(b.X + Bounds.X, b.Y + Bounds.Y, b.Width, b.Height);
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            SelectedBounds = GetRubberBandRect(_startPt, _currentPt);
            FinishSelection();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void FinishSelection()
        {
            Rectangle crop = Rectangle.Intersect(new Rectangle(0, 0, _frozenDesktop.Width, _frozenDesktop.Height), SelectedBounds);
            if (crop.Width < 16 || crop.Height < 16)
                crop = new Rectangle(0, 0, _frozenDesktop.Width, _frozenDesktop.Height);
            CapturedBitmap = _frozenDesktop.Clone(crop, PixelFormat.Format32bppArgb);
            SelectedBounds = new Rectangle(crop.X + Bounds.X, crop.Y + Bounds.Y, crop.Width & ~1, crop.Height & ~1);
            DialogResult = DialogResult.OK;
            Close();
        }

        private static Rectangle GetRubberBandRect(Point a, Point b) =>
            new Rectangle(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Max(16, Math.Abs(a.X - b.X) & ~1), Math.Max(16, Math.Abs(a.Y - b.Y) & ~1));

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(_frozenDesktop, 0, 0);

            using (SolidBrush dimBrush = new SolidBrush(Color.FromArgb(110, GaBrand.Abyss)))
                g.FillRectangle(dimBrush, ClientRectangle);

            Rectangle activeRect = _mode switch
            {
                CaptureMode.FixedRegion => new Rectangle(_currentPt.X - _fixedSize.Width / 2, _currentPt.Y - _fixedSize.Height / 2, _fixedSize.Width, _fixedSize.Height),
                CaptureMode.WindowObject or CaptureMode.ScrollingWindow => _spyWindowRect,
                _ => _dragging ? GetRubberBandRect(_startPt, _currentPt) : Rectangle.Empty
            };

            if (_mode == CaptureMode.Freehand && _freehandPts.Count > 1)
            {
                using (Pen cyanPen = new Pen(GaBrand.Cyan, 2f))
                    g.DrawLines(cyanPen, _freehandPts.ToArray());
            }
            else if (activeRect.Width > 0 && activeRect.Height > 0)
            {
                g.DrawImage(_frozenDesktop, activeRect, activeRect, GraphicsUnit.Pixel);
                using (Pen borderPen = new Pen(GaBrand.Cyan, 2f))
                    g.DrawRectangle(borderPen, activeRect);

                string badge = $"{activeRect.Width} × {activeRect.Height} px · {_mode}";
                using (Font f = GaBrand.MakeFont(12f, FontStyle.Bold))
                {
                    SizeF sz = g.MeasureString(badge, f);
                    RectangleF pill = new RectangleF(activeRect.X, Math.Max(8, activeRect.Y - 32), sz.Width + 20, 26);
                    using (GraphicsPath path = GaBrand.RoundedRect(pill, 999f))
                    using (SolidBrush fill = new SolidBrush(GaBrand.Panel))
                    using (Pen outline = new Pen(GaBrand.Cyan, 1f))
                    using (SolidBrush textB = new SolidBrush(GaBrand.Cyan))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.FillPath(fill, path);
                        g.DrawPath(outline, path);
                        g.DrawString(badge, f, textB, pill.X + 10, pill.Y + 4);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _frozenDesktop?.Dispose();
            base.Dispose(disposing);
        }
    }

    // ========================================================================
    // 10. BUILT-IN EDITOR & ANNOTATION BOARD (TMainWin & TTextBoard)
    // ========================================================================
    public enum AnnotationTool { SelectCrop, TextLabel, ArrowLine, HighlightRect, StepStamp, ObscureBlur, EdgeWatermark }

    public sealed class EditorForm : Form
    {
        private Bitmap _canvasBitmap;
        private readonly AppConfig _cfg;
        private AnnotationTool _activeTool;
        private float _zoom = 1.0f;
        private int _stepCounter = 1;
        private bool _dragging;
        private Point _dragStart;
        private Point _dragCurrent;
        private Rectangle _selectionRect;
        private bool _isSaved;

        public EditorForm(Bitmap initialBitmap, AppConfig cfg, bool startInDrawMode)
        {
            _canvasBitmap = (Bitmap)initialBitmap.Clone();
            _cfg = cfg;
            _activeTool = startInDrawMode ? AnnotationTool.ArrowLine : AnnotationTool.SelectCrop;
            CrashSafeGuard.SetUnsavedEditorBitmap(_canvasBitmap);

            Text = "Open Screen Recorder — Studio Editor & Annotation Board";
            Size = new Size(1020, 720);
            MinimumSize = new Size(640, 480);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = GaBrand.Abyss;
            ForeColor = GaBrand.Text;
            DoubleBuffered = true;
            KeyPreview = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_isSaved && _canvasBitmap != null)
            {
                string autoPath = StorageEngine.BuildAutoSaveFilePath(_cfg, _cfg.DefaultImageExtension, "OSR_AutoSavedEdit_$Y-$M-$D_$H-$N-$S_#");
                OutputDispatcher.SaveImageByExtension(_canvasBitmap, autoPath);
                _isSaved = true;
            }
            CrashSafeGuard.SetUnsavedEditorBitmap(null);
            base.OnFormClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { SaveCurrent(false); e.Handled = true; }
            else if (e.Alt && e.KeyCode == Keys.S) { SaveCurrent(true); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.R) { ResizeCanvas(0.75f); e.Handled = true; }
            else if (e.KeyCode == Keys.X) { CropToSelection(); }
            else if (e.KeyCode == Keys.D) { _activeTool = AnnotationTool.ArrowLine; Invalidate(); }
            else if (e.KeyCode == Keys.O) { _activeTool = AnnotationTool.ObscureBlur; Invalidate(); }
            else if (e.KeyCode == Keys.G) { ApplyEdgeAndWatermark(); }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Space) { _zoom = 1.0f; Invalidate(); }
            else if (e.KeyCode == Keys.B)
            {
                float zx = (Width - 80f) / _canvasBitmap.Width;
                float zy = (Height - 180f) / _canvasBitmap.Height;
                _zoom = Math.Clamp(Math.Min(zx, zy), 0.25f, 4.0f);
                Invalidate();
            }
            else if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
            {
                _zoom = (e.KeyCode - Keys.D0);
                Invalidate();
            }
        }

        private void SaveCurrent(bool selectionOnly)
        {
            Bitmap target = _canvasBitmap;
            if (selectionOnly && _selectionRect.Width > 4 && _selectionRect.Height > 4)
            {
                Rectangle safe = Rectangle.Intersect(new Rectangle(0, 0, _canvasBitmap.Width, _canvasBitmap.Height), _selectionRect);
                if (safe.Width > 4 && safe.Height > 4)
                    target = _canvasBitmap.Clone(safe, PixelFormat.Format32bppArgb);
            }
            string path = StorageEngine.BuildAutoSaveFilePath(_cfg, _cfg.DefaultImageExtension);
            OutputDispatcher.SaveImageByExtension(target, path);
            _isSaved = true;
            Invalidate();
        }

        private void CropToSelection()
        {
            if (_selectionRect.Width <= 8 || _selectionRect.Height <= 8) return;
            Rectangle safe = Rectangle.Intersect(new Rectangle(0, 0, _canvasBitmap.Width, _canvasBitmap.Height), _selectionRect);
            if (safe.Width <= 8 || safe.Height <= 8) return;
            Bitmap cropped = _canvasBitmap.Clone(safe, PixelFormat.Format32bppArgb);
            _canvasBitmap.Dispose();
            _canvasBitmap = cropped;
            _selectionRect = Rectangle.Empty;
            _isSaved = false;
            CrashSafeGuard.SetUnsavedEditorBitmap(_canvasBitmap);
            Invalidate();
        }

        private void ResizeCanvas(float scale)
        {
            int nw = Math.Max(16, (int)(_canvasBitmap.Width * scale));
            int nh = Math.Max(16, (int)(_canvasBitmap.Height * scale));
            Bitmap resized = new Bitmap(nw, nh, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_canvasBitmap, 0, 0, nw, nh);
            }
            _canvasBitmap.Dispose();
            _canvasBitmap = resized;
            _isSaved = false;
            CrashSafeGuard.SetUnsavedEditorBitmap(_canvasBitmap);
            Invalidate();
        }

        private void ApplyEdgeAndWatermark()
        {
            using (Graphics g = Graphics.FromImage(_canvasBitmap))
            using (Pen edgePen = new Pen(GaBrand.Cyan, 3f))
            using (Font wmFont = GaBrand.MakeFont(13f, FontStyle.Bold))
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(210, GaBrand.Abyss)))
            using (SolidBrush fg = new SolidBrush(GaBrand.Slate))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawRectangle(edgePen, 1, 1, _canvasBitmap.Width - 3, _canvasBitmap.Height - 3);
                string wm = "Graphicarray · linkedin.com/in/graphicarray";
                SizeF sz = g.MeasureString(wm, wmFont);
                RectangleF pill = new RectangleF(_canvasBitmap.Width - sz.Width - 28, _canvasBitmap.Height - 36, sz.Width + 20, 26);
                using (GraphicsPath gp = GaBrand.RoundedRect(pill, 999f))
                {
                    g.FillPath(bg, gp);
                    g.DrawString(wm, wmFont, fg, pill.X + 10, pill.Y + 4);
                }
            }
            _isSaved = false;
            Invalidate();
        }

        private Point ScreenToImagePoint(Point pt) =>
            new Point((int)((pt.X - 32) / _zoom), (int)((pt.Y - 118) / _zoom));

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Y < 60 && e.X < 320) { GaBrand.OpenBrandProfile(); return; }
            if (e.Y > Height - 44 && e.X < 280) { GaBrand.OpenBrandProfile(); return; }

            if (e.Y >= 64 && e.Y <= 104)
            {
                int idx = (e.X - 24) / 128;
                if (idx >= 0 && idx <= 6)
                {
                    _activeTool = (AnnotationTool)idx;
                    if (_activeTool == AnnotationTool.EdgeWatermark)
                        ApplyEdgeAndWatermark();
                    Invalidate();
                }
                return;
            }

            if (e.Y > 118 && e.Y < Height - 48)
            {
                _dragging = true;
                _dragStart = ScreenToImagePoint(e.Location);
                _dragCurrent = _dragStart;

                if (_activeTool == AnnotationTool.StepStamp)
                {
                    using (Graphics g = Graphics.FromImage(_canvasBitmap))
                    using (SolidBrush fill = new SolidBrush(GaBrand.Cyan))
                    using (SolidBrush textB = new SolidBrush(GaBrand.Abyss))
                    using (Font f = GaBrand.MakeFont(14f, FontStyle.Bold))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        Rectangle badge = new Rectangle(_dragStart.X - 14, _dragStart.Y - 14, 28, 28);
                        g.FillEllipse(fill, badge);
                        g.DrawString((_stepCounter++).ToString(), f, textB, badge.X + 8, badge.Y + 4);
                    }
                    _isSaved = false;
                    Invalidate();
                }
                else if (_activeTool == AnnotationTool.TextLabel)
                {
                    using (Graphics g = Graphics.FromImage(_canvasBitmap))
                    using (Font f = GaBrand.MakeFont(16f, FontStyle.Bold))
                    using (SolidBrush bg = new SolidBrush(GaBrand.Panel))
                    using (Pen border = new Pen(GaBrand.Cyan, 1.5f))
                    using (SolidBrush fg = new SolidBrush(GaBrand.Text))
                    {
                        string note = $"Note #{_stepCounter}";
                        SizeF sz = g.MeasureString(note, f);
                        RectangleF r = new RectangleF(_dragStart.X, _dragStart.Y, sz.Width + 16, sz.Height + 8);
                        using (GraphicsPath gp = GaBrand.RoundedRect(r, 12f))
                        {
                            g.FillPath(bg, gp);
                            g.DrawPath(border, gp);
                            g.DrawString(note, f, fg, r.X + 8, r.Y + 4);
                        }
                    }
                    _isSaved = false;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_dragging) return;
            _dragCurrent = ScreenToImagePoint(e.Location);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!_dragging) return;
            _dragging = false;
            _dragCurrent = ScreenToImagePoint(e.Location);
            Rectangle rect = new Rectangle(
                Math.Min(_dragStart.X, _dragCurrent.X),
                Math.Min(_dragStart.Y, _dragCurrent.Y),
                Math.Abs(_dragStart.X - _dragCurrent.X),
                Math.Abs(_dragStart.Y - _dragCurrent.Y));

            if (_activeTool == AnnotationTool.SelectCrop)
                _selectionRect = rect;
            else if (_activeTool == AnnotationTool.ArrowLine)
            {
                using (Graphics g = Graphics.FromImage(_canvasBitmap))
                using (Pen p = new Pen(GaBrand.Cyan, 3.5f))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    p.CustomEndCap = new AdjustableArrowCap(6, 6);
                    g.DrawLine(p, _dragStart, _dragCurrent);
                }
                _isSaved = false;
            }
            else if (_activeTool == AnnotationTool.HighlightRect)
            {
                using (Graphics g = Graphics.FromImage(_canvasBitmap))
                using (SolidBrush hb = new SolidBrush(Color.FromArgb(80, GaBrand.Cyan)))
                using (Pen hp = new Pen(GaBrand.Cyan, 1.5f))
                {
                    g.FillRectangle(hb, rect);
                    g.DrawRectangle(hp, rect);
                }
                _isSaved = false;
            }
            else if (_activeTool == AnnotationTool.ObscureBlur)
            {
                ScreenCaptureEngine.ApplyObscureBlur(_canvasBitmap, rect, 12);
                _isSaved = false;
            }
            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e) => CropToSelection();

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            GaBrand.PaintAbyssBackground(g, ClientRectangle);

            GaBrand.DrawLogoTile(g, new RectangleF(24, 10, 44, 44), false, false);
            using (Font titleF = GaBrand.MakeFont(18f, FontStyle.Bold))
            using (Font byF = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("Open Screen Recorder Editor", titleF, textB, 78, 10);
                g.DrawString("by Graphicarray", byF, cyanB, 78, 32);
            }

            string[] tools = { "Crop (X)", "Text (T)", "Arrow (D)", "Highlight", "Step (1..9)", "Blur (O)", "Watermark (G)" };
            for (int i = 0; i < tools.Length; i++)
            {
                RectangleF pill = new RectangleF(24 + i * 128, 64, 120, 34);
                bool active = (int)_activeTool == i;
                using (GraphicsPath gp = GaBrand.RoundedRect(pill, 999f))
                {
                    if (active)
                    {
                        using (LinearGradientBrush grad = GaBrand.SignatureGradient(pill))
                            g.FillPath(grad, gp);
                    }
                    else
                    {
                        using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
                        using (Pen ep = new Pen(GaBrand.Edge, 1f))
                        {
                            g.FillPath(pb, gp);
                            g.DrawPath(ep, gp);
                        }
                    }
                    using (Font tf = GaBrand.MakeFont(12f, FontStyle.Bold))
                    using (SolidBrush tb = new SolidBrush(active ? GaBrand.Abyss : GaBrand.Slate))
                    {
                        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString(tools[i], tf, tb, pill, sf);
                    }
                }
            }

            RectangleF canvasCard = new RectangleF(24, 110, Width - 48, Height - 166);
            using (GraphicsPath cardPath = GaBrand.RoundedRect(canvasCard, 20f))
            using (SolidBrush cardFill = new SolidBrush(GaBrand.Panel))
            using (Pen cardEdge = new Pen(GaBrand.Edge, 1f))
            {
                g.FillPath(cardFill, cardPath);
                g.DrawPath(cardEdge, cardPath);
            }

            if (_canvasBitmap != null)
            {
                g.SetClip(Rectangle.Round(canvasCard));
                g.DrawImage(_canvasBitmap, 32, 118, _canvasBitmap.Width * _zoom, _canvasBitmap.Height * _zoom);
                if (_selectionRect.Width > 0 && _selectionRect.Height > 0)
                {
                    Rectangle sr = new Rectangle(
                        32 + (int)(_selectionRect.X * _zoom),
                        118 + (int)(_selectionRect.Y * _zoom),
                        (int)(_selectionRect.Width * _zoom),
                        (int)(_selectionRect.Height * _zoom));
                    using (Pen sp = new Pen(GaBrand.Cyan, 1.5f) { DashStyle = DashStyle.Dash })
                        g.DrawRectangle(sp, sr);
                }
                g.ResetClip();
            }

            using (Font ff = GaBrand.MakeFont(12f, FontStyle.Regular))
            using (SolidBrush sb = new SolidBrush(GaBrand.Slate))
            {
                g.DrawString(GaBrand.FooterCredit + "  ·  Ctrl+S Save  ·  Auto-saves on close", ff, sb, 24, Height - 34);
                RectangleF chip = new RectangleF(Width - 84, Height - 38, 60, 24);
                using (GraphicsPath cp = GaBrand.RoundedRect(chip, 999f))
                using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
                using (Pen ep = new Pen(GaBrand.Edge, 1f))
                {
                    g.FillPath(pb, cp);
                    g.DrawPath(ep, cp);
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(GaBrand.VersionText, ff, sb, chip, sf);
                }
            }
        }
    }

    // ========================================================================
    // 11. MAIN OPEN SCREEN RECORDER STUDIO WINDOW (TMainPanel)
    // ========================================================================
    public sealed class MainCaptureForm : Form
    {
        private readonly AppConfig _cfg;
        private CrashSafeVideoRecorder _recorder;
        private Rectangle _selectedVideoBounds;
        private IntPtr _selectedVideoWindow;
        private Point[] _selectedFreehandPolygon;

        private int _activeTab; // 0 = Video Studio, 1 = Output Target, 2 = Crash Guard
        private string _statusToast = "Ready · All 8 Modes Work for Video & Screenshot";
        private bool _hoverLogo;
        private bool _hoverByLink;
        private bool _hoverFooterLink;
        private int _hoveredModeCard = -1;
        private bool _hoverPrimaryBtn;
        private bool _hoverPauseBtn;
        private bool _blinkRedPhase;

        private readonly System.Windows.Forms.Timer _uiAndBlinkTimer;
        private readonly NotifyIcon _trayIcon;
        private readonly Icon _iconIdle;
        private readonly Icon _iconRecRed;
        private readonly Icon _iconPausedAmber;

        // Hit-test rectangles
        private RectangleF _rectLogo;
        private RectangleF _rectByLink;
        private RectangleF _rectFooterLink;
        private RectangleF _rectFolderInput;
        private RectangleF _rectFolderBrowseBtn;
        private RectangleF _rectFormatCycle;
        private RectangleF _rectQualityCycle;
        private RectangleF _rectFpsCycle;
        private RectangleF _rectOutputCycle;
        private RectangleF _rectCursorToggle;
        private RectangleF _rectAudioToggle;
        private RectangleF _rectPortableToggle;
        private RectangleF _rectScreenshotBtn;
        private RectangleF _rectPauseBtn;
        private RectangleF _rectPrimaryBtn;
        private readonly RectangleF[] _tabRects = new RectangleF[3];
        private readonly RectangleF[] _modeRects = new RectangleF[8];

        private static readonly string[] ModeTitles =
        {
            "Active Window", "Window / Spy", "Rectangle", "Freehand",
            "Full Screen", "Scroll Page", "Fixed Region", "Repeat Last"
        };

        public MainCaptureForm(AppConfig cfg)
        {
            _cfg = cfg;
            _selectedVideoBounds = _cfg.LastCaptureRect.Width > 16 ? _cfg.LastCaptureRect : ScreenCaptureEngine.GetVirtualDesktopBounds();
            CrashSafeGuard.Initialize(_cfg);

            _iconIdle = GaBrand.CreateStatusIcon(0);
            _iconRecRed = GaBrand.CreateStatusIcon(1);
            _iconPausedAmber = GaBrand.CreateStatusIcon(2);

            Text = GaBrand.ProductName;
            Icon = _iconIdle;
            Size = new Size(460, 760);
            MinimumSize = new Size(360, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = GaBrand.Abyss;
            ForeColor = GaBrand.Text;
            DoubleBuffered = true;
            KeyPreview = true;

            // System Tray Icon with Blinking Red Recording Alert & Context Menu
            ContextMenuStrip trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Start / Stop Video (F9)", null, (s, e) => ToggleVideoRecording());
            trayMenu.Items.Add("Pause / Resume Video (F10)", null, (s, e) => TogglePauseRecording());
            trayMenu.Items.Add("Capture Screenshot (PrtSc)", null, (s, e) => ExecuteScreenshotCapture(_cfg.ActiveCaptureMode));
            trayMenu.Items.Add("Show Window", null, (s, e) => { Show(); WindowState = FormWindowState.Normal; Activate(); });
            trayMenu.Items.Add("Exit (Auto-Save Active)", null, (s, e) => Close());

            _trayIcon = new NotifyIcon
            {
                Icon = _iconIdle,
                Text = "Open Screen Recorder by Graphicarray",
                ContextMenuStrip = trayMenu,
                Visible = true
            };
            _trayIcon.DoubleClick += (s, e) => { Show(); WindowState = FormWindowState.Normal; Activate(); };

            // 500ms timer drives live HUD timer AND blinking red Taskbar + System Tray icon during recording
            _uiAndBlinkTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _uiAndBlinkTimer.Tick += OnTimerTick;
            _uiAndBlinkTimer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            bool isRec = _recorder != null && _recorder.IsRecording;
            if (isRec)
            {
                if (_recorder.IsPaused)
                {
                    Icon = _iconPausedAmber;
                    _trayIcon.Icon = _iconPausedAmber;
                    _trayIcon.Text = $"PAUSED · Open Screen Recorder ({_recorder.Elapsed:hh\\:mm\\:ss})";
                }
                else
                {
                    _blinkRedPhase = !_blinkRedPhase;
                    Icon activeIcon = _blinkRedPhase ? _iconRecRed : _iconIdle;
                    Icon = activeIcon;
                    _trayIcon.Icon = activeIcon;
                    _trayIcon.Text = $"● REC {_recorder.Elapsed:hh\\:mm\\:ss} · Open Screen Recorder";
                }
                Invalidate();
            }
            else if (Icon != _iconIdle)
            {
                Icon = _iconIdle;
                _trayIcon.Icon = _iconIdle;
                _trayIcon.Text = "Open Screen Recorder by Graphicarray";
                Invalidate();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Global hotkeys: F9 = Start/Stop Video, F10 = Pause/Resume Video, PrtSc = Screenshot, F11 = Repeat Last
            NativeMethods.RegisterHotKey(Handle, 101, 0, (uint)Keys.F9);
            NativeMethods.RegisterHotKey(Handle, 102, 0, (uint)Keys.F10);
            NativeMethods.RegisterHotKey(Handle, 103, 0, (uint)Keys.PrintScreen);
            NativeMethods.RegisterHotKey(Handle, 104, 0, (uint)Keys.F11);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CrashSafeGuard.EmergencySaveAll("MainFormClosing");
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            NativeMethods.UnregisterHotKey(Handle, 101);
            NativeMethods.UnregisterHotKey(Handle, 102);
            NativeMethods.UnregisterHotKey(Handle, 103);
            NativeMethods.UnregisterHotKey(Handle, 104);
            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_QUERYENDSESSION)
            {
                CrashSafeGuard.EmergencySaveAll("WindowsShutdownOrLogoff");
            }
            else if (m.Msg == NativeMethods.WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == 101) ToggleVideoRecording();
                else if (id == 102) TogglePauseRecording();
                else if (id == 103) ExecuteScreenshotCapture(_cfg.ActiveCaptureMode);
                else if (id == 104) SelectCaptureModeAndArea(CaptureMode.RepeatLast);
            }
            base.WndProc(ref m);
        }

        // All 8 Capture Mode Buttons work for BOTH Video Recording AND Screenshot
        private void SelectCaptureModeAndArea(CaptureMode mode)
        {
            _cfg.ActiveCaptureMode = mode;
            _selectedFreehandPolygon = null;
            _selectedVideoWindow = IntPtr.Zero;

            if (mode == CaptureMode.RepeatLast)
            {
                _selectedVideoBounds = _cfg.LastCaptureRect.Width > 16
                    ? _cfg.LastCaptureRect
                    : ScreenCaptureEngine.GetVirtualDesktopBounds();
                _statusToast = $"Repeat Last Area ready ({_selectedVideoBounds.Width}×{_selectedVideoBounds.Height})";
                Invalidate();
                return;
            }

            if (mode == CaptureMode.FullScreen)
            {
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
                _cfg.LastCaptureRect = _selectedVideoBounds;
                StorageEngine.Save(_cfg);
                _statusToast = $"Full Screen ready ({_selectedVideoBounds.Width}×{_selectedVideoBounds.Height})";
                Invalidate();
                return;
            }

            if (mode == CaptureMode.ActiveWindow)
            {
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
                _statusToast = "Active Window mode set for Video & Screenshot";
                Invalidate();
                return;
            }

            // Interactive selection for Window / Spy, Rectangle, Freehand, Scroll Page, Fixed Region
            Hide();
            Thread.Sleep(130);
            try
            {
                using (RegionOverlayForm ov = new RegionOverlayForm(
                    mode, new Size(_cfg.FixedRegionWidth, _cfg.FixedRegionHeight)))
                {
                    if (ov.ShowDialog(this) == DialogResult.OK)
                    {
                        _selectedVideoBounds = ov.SelectedBounds;
                        _selectedVideoWindow = ov.SelectedWindowHandle;
                        _selectedFreehandPolygon = ov.LocalFreehandPolygon;
                        _cfg.LastCaptureRect = _selectedVideoBounds;
                        _cfg.LastCaptureMode = mode;
                        StorageEngine.Save(_cfg);
                        _statusToast = $"{mode} Area locked: {_selectedVideoBounds.Width}×{_selectedVideoBounds.Height} px";
                    }
                }
            }
            finally
            {
                Show();
                Activate();
                Invalidate();
            }
        }

        private void ToggleVideoRecording()
        {
            if (_recorder != null && _recorder.IsRecording)
            {
                string savedPath = _recorder.StopAndSave(emergency: false, reason: "UserButtonOrF9");
                _recorder = null;
                _statusToast = "Saved single A/V file: " + Path.GetFileName(savedPath);
                Invalidate();
                return;
            }

            if (_selectedVideoBounds.Width < 16 || _selectedVideoBounds.Height < 16)
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();

            _recorder = new CrashSafeVideoRecorder(
                _cfg,
                _cfg.ActiveCaptureMode,
                _selectedVideoBounds,
                _selectedVideoWindow,
                _selectedFreehandPolygon);

            string livePath = _recorder.Start();
            _statusToast = "● REC (Single A/V): " + Path.GetFileName(livePath);
            Invalidate();
        }

        private void TogglePauseRecording()
        {
            if (_recorder == null || !_recorder.IsRecording)
            {
                _statusToast = "Start a recording (F9) first to use Pause/Resume (F10).";
                Invalidate();
                return;
            }
            _recorder.TogglePause();
            _statusToast = _recorder.IsPaused
                ? "Recording PAUSED (flushed to disk) · Press Resume (F10)"
                : "● Recording RESUMED: " + Path.GetFileName(_recorder.ActiveVideoPath);
            Invalidate();
        }

        private void ExecuteScreenshotCapture(CaptureMode mode)
        {
            if (_cfg.CaptureDelaySeconds > 0)
                Thread.Sleep(_cfg.CaptureDelaySeconds * 1000);

            Hide();
            Thread.Sleep(140);

            Bitmap captured = null;
            Rectangle rect = _selectedVideoBounds.Width > 16 ? _selectedVideoBounds : ScreenCaptureEngine.GetVirtualDesktopBounds();

            try
            {
                switch (mode)
                {
                    case CaptureMode.ActiveWindow:
                        captured = ScreenCaptureEngine.CaptureActiveWindow(_cfg.IncludeMouseCursor, out rect);
                        break;

                    case CaptureMode.FullScreen:
                        rect = ScreenCaptureEngine.GetVirtualDesktopBounds();
                        captured = ScreenCaptureEngine.CaptureRegion(rect, _cfg.IncludeMouseCursor);
                        break;

                    case CaptureMode.RepeatLast:
                        rect = _cfg.LastCaptureRect.Width > 16 ? _cfg.LastCaptureRect : ScreenCaptureEngine.GetVirtualDesktopBounds();
                        captured = ScreenCaptureEngine.CaptureRegion(rect, _cfg.IncludeMouseCursor);
                        break;

                    case CaptureMode.Rectangle:
                    case CaptureMode.Freehand:
                    case CaptureMode.FixedRegion:
                    case CaptureMode.WindowObject:
                        using (RegionOverlayForm ov = new RegionOverlayForm(
                            mode, new Size(_cfg.FixedRegionWidth, _cfg.FixedRegionHeight)))
                        {
                            if (ov.ShowDialog(this) == DialogResult.OK)
                            {
                                captured = (Bitmap)ov.CapturedBitmap.Clone();
                                rect = ov.SelectedBounds;
                            }
                        }
                        break;

                    case CaptureMode.ScrollingWindow:
                        using (RegionOverlayForm ov = new RegionOverlayForm(CaptureMode.ScrollingWindow, Size.Empty))
                        {
                            if (ov.ShowDialog(this) == DialogResult.OK && ov.SelectedWindowHandle != IntPtr.Zero)
                            {
                                rect = ov.SelectedBounds;
                                captured = ScreenCaptureEngine.CaptureScrollingWindow(
                                    ov.SelectedWindowHandle, rect, _cfg.AutoScrollIntervalMs);
                            }
                        }
                        break;
                }

                if (captured != null)
                {
                    _cfg.LastCaptureRect = rect;
                    _cfg.LastCaptureMode = mode;
                    StorageEngine.Save(_cfg);
                    string saved = OutputDispatcher.Dispatch(captured, _cfg, this);
                    _statusToast = "Saved screenshot: " + Path.GetFileName(saved);
                }
            }
            finally
            {
                captured?.Dispose();
                Show();
                Activate();
                Invalidate();
            }
        }

        private void BrowseAutoSaveFolder()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Auto-Save & Crash Recovery Folder on your PC";
                fbd.SelectedPath = _cfg.AutoSaveFolder;
                if (fbd.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _cfg.AutoSaveFolder = fbd.SelectedPath;
                    StorageEngine.Save(_cfg);
                    _statusToast = "Save folder updated: " + _cfg.AutoSaveFolder;
                    Invalidate();
                }
            }
        }

        private void PromptEditAutoSaveFolder()
        {
            using (Form prompt = new Form())
            {
                prompt.Text = "Edit Auto-Save Directory Path";
                prompt.Size = new Size(460, 170);
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.BackColor = GaBrand.Abyss;
                prompt.ForeColor = GaBrand.Text;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lbl = new Label
                {
                    Text = "Type or paste your Auto-Save folder path:",
                    Left = 16, Top = 16, Width = 410,
                    ForeColor = GaBrand.Slate
                };
                TextBox tb = new TextBox
                {
                    Text = _cfg.AutoSaveFolder,
                    Left = 16, Top = 44, Width = 410,
                    BackColor = GaBrand.Panel,
                    ForeColor = GaBrand.Text
                };
                Button ok = new Button
                {
                    Text = "Save Path",
                    Left = 246, Top = 84, Width = 90, Height = 32,
                    DialogResult = DialogResult.OK,
                    BackColor = GaBrand.Cyan,
                    ForeColor = GaBrand.Abyss
                };
                Button cancel = new Button
                {
                    Text = "Cancel",
                    Left = 342, Top = 84, Width = 84, Height = 32,
                    DialogResult = DialogResult.Cancel,
                    BackColor = GaBrand.Panel,
                    ForeColor = GaBrand.Text
                };
                prompt.Controls.AddRange(new Control[] { lbl, tb, ok, cancel });
                prompt.AcceptButton = ok;

                if (prompt.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(tb.Text))
                {
                    try
                    {
                        Directory.CreateDirectory(tb.Text.Trim());
                        _cfg.AutoSaveFolder = tb.Text.Trim();
                        StorageEngine.Save(_cfg);
                        _statusToast = "Save folder set: " + _cfg.AutoSaveFolder;
                        Invalidate();
                    }
                    catch (Exception ex)
                    {
                        _statusToast = "Invalid folder path: " + ex.Message;
                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            PointF pt = e.Location;
            bool hLogo = _rectLogo.Contains(pt);
            bool hBy = _rectByLink.Contains(pt);
            bool hFoot = _rectFooterLink.Contains(pt);
            bool hPrim = _rectPrimaryBtn.Contains(pt);
            bool hPause = _rectPauseBtn.Contains(pt);

            int modeHover = -1;
            for (int i = 0; i < _modeRects.Length; i++)
                if (_modeRects[i].Contains(pt)) modeHover = i;

            Cursor = (hLogo || hBy || hFoot || hPrim || hPause || modeHover >= 0 ||
                      _rectFolderInput.Contains(pt) || _rectFolderBrowseBtn.Contains(pt) ||
                      _rectFormatCycle.Contains(pt) || _rectQualityCycle.Contains(pt) ||
                      _rectFpsCycle.Contains(pt) || _rectOutputCycle.Contains(pt) ||
                      _rectCursorToggle.Contains(pt) || _rectAudioToggle.Contains(pt) ||
                      _rectPortableToggle.Contains(pt) || _rectScreenshotBtn.Contains(pt))
                ? Cursors.Hand : Cursors.Default;

            if (hLogo != _hoverLogo || hBy != _hoverByLink || hFoot != _hoverFooterLink ||
                hPrim != _hoverPrimaryBtn || hPause != _hoverPauseBtn || modeHover != _hoveredModeCard)
            {
                _hoverLogo = hLogo;
                _hoverByLink = hBy;
                _hoverFooterLink = hFoot;
                _hoverPrimaryBtn = hPrim;
                _hoverPauseBtn = hPause;
                _hoveredModeCard = modeHover;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            PointF pt = e.Location;

            if (_rectLogo.Contains(pt) || _rectByLink.Contains(pt) || _rectFooterLink.Contains(pt))
            {
                GaBrand.OpenBrandProfile();
                return;
            }

            // 8 Capture Modes: sets the Video Recording Area (and triggers interactive picker when needed)
            for (int i = 0; i < _modeRects.Length; i++)
            {
                if (_modeRects[i].Contains(pt))
                {
                    SelectCaptureModeAndArea((CaptureMode)i);
                    return;
                }
            }

            // Segmented Tabs
            for (int i = 0; i < _tabRects.Length; i++)
            {
                if (_tabRects[i].Contains(pt))
                {
                    _activeTab = i;
                    Invalidate();
                    return;
                }
            }

            // Save Directory Path (Editable text input OR Folder Browser dialog)
            if (_rectFolderInput.Contains(pt)) { PromptEditAutoSaveFolder(); return; }
            if (_rectFolderBrowseBtn.Contains(pt)) { BrowseAutoSaveFolder(); return; }

            // Video Format (MP4 -> MKV -> AVI)
            if (_rectFormatCycle.Contains(pt))
            {
                VideoFormatType[] fmts = { VideoFormatType.MP4, VideoFormatType.MKV, VideoFormatType.AVI };
                int idx = Array.IndexOf(fmts, _cfg.VideoFormat);
                _cfg.VideoFormat = fmts[(idx + 1) % fmts.Length];
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            // Video Quality (Low -> Medium -> High -> Lossless)
            if (_rectQualityCycle.Contains(pt))
            {
                VideoQualityLevel[] quals = { VideoQualityLevel.Low, VideoQualityLevel.Medium, VideoQualityLevel.High, VideoQualityLevel.Lossless };
                int idx = Array.IndexOf(quals, _cfg.VideoQuality);
                _cfg.VideoQuality = quals[(idx + 1) % quals.Length];
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            // Frame Rate (15 -> 24 -> 30 -> 60 FPS)
            if (_rectFpsCycle.Contains(pt))
            {
                int[] fpsOpts = { 15, 24, 30, 60 };
                int idx = Array.IndexOf(fpsOpts, _cfg.VideoFps);
                _cfg.VideoFps = fpsOpts[(idx + 1) % fpsOpts.Length];
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_rectOutputCycle.Contains(pt))
            {
                int next = ((int)_cfg.ActiveOutputTarget + 1) % Enum.GetValues(typeof(OutputTarget)).Length;
                _cfg.ActiveOutputTarget = (OutputTarget)next;
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_rectCursorToggle.Contains(pt))
            {
                _cfg.IncludeMouseCursor = !_cfg.IncludeMouseCursor;
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_rectAudioToggle.Contains(pt))
            {
                _cfg.RecordAudio = !_cfg.RecordAudio;
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_rectPortableToggle.Contains(pt))
            {
                _cfg.PortableMode = !_cfg.PortableMode;
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_rectScreenshotBtn.Contains(pt))
            {
                ExecuteScreenshotCapture(_cfg.ActiveCaptureMode);
                return;
            }

            if (_rectPauseBtn.Contains(pt))
            {
                TogglePauseRecording();
                return;
            }

            if (_rectPrimaryBtn.Contains(pt))
            {
                ToggleVideoRecording();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            float pad = 20f;

            GaBrand.PaintAbyssBackground(g, ClientRectangle);

            // ── 1. HEADER: 44px Logo tile + "Open Screen Recorder" + "by Graphicarray" + Blinking Red Indicator
            _rectLogo = new RectangleF(pad, 14f, 44f, 44f);
            GaBrand.DrawLogoTile(g, _rectLogo, _hoverLogo, false);

            using (Font titleFont = GaBrand.MakeFont(20f, FontStyle.Bold))
            using (Font byFont = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush whiteBrush = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanBrush = new SolidBrush(_hoverByLink ? GaBrand.Ice : GaBrand.Cyan))
            {
                g.DrawString(GaBrand.ProductName, titleFont, whiteBrush, pad + 56f, 13f);
                _rectByLink = new RectangleF(pad + 56f, 38f, 120f, 18f);
                g.DrawString("by Graphicarray", byFont, cyanBrush, _rectByLink.Location);
            }

            bool isRec = _recorder != null && _recorder.IsRecording;
            bool isPaused = isRec && _recorder.IsPaused;
            Color dotColor = !isRec ? GaBrand.Green : (isPaused ? GaBrand.Amber : (_blinkRedPhase ? GaBrand.Red : GaBrand.Mute));
            using (SolidBrush statusDot = new SolidBrush(dotColor))
            using (Pen halo = new Pen(Color.FromArgb(75, dotColor), 4f))
            {
                RectangleF dotRect = new RectangleF(w - pad - 14f, 28f, 12f, 12f);
                g.DrawEllipse(halo, dotRect);
                g.FillEllipse(statusDot, dotRect);
            }

            // ── 2. MAIN VISUAL STUDIO CARD: HUD + 8 Capture Modes (Video & Screenshot)
            float y = 68f;
            RectangleF mainCard = new RectangleF(pad, y, w - pad * 2, 232f);
            using (GraphicsPath cardPath = GaBrand.RoundedRect(mainCard, GaBrand.RadiusCard))
            using (SolidBrush panelBrush = new SolidBrush(GaBrand.Panel))
            using (Pen edgePen = new Pen(isRec ? GaBrand.Cyan : GaBrand.Edge, 1f))
            {
                g.FillPath(panelBrush, cardPath);
                g.DrawPath(edgePen, cardPath);
            }

            using (Font labelF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font timerF = GaBrand.MakeFont(22f, FontStyle.Bold))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("VIDEO & SCREENSHOT CAPTURE MODES (ALL 8 ACTIVE)", labelF, slateB, mainCard.X + 16f, mainCard.Y + 12f);
                string timeStr = isRec ? _recorder.Elapsed.ToString(@"hh\:mm\:ss") : "00:00:00";
                g.DrawString(timeStr, timerF, isRec ? cyanB : textB, mainCard.X + 16f, mainCard.Y + 30f);

                _rectScreenshotBtn = new RectangleF(mainCard.Right - 142f, mainCard.Y + 30f, 126f, 28f);
                DrawSecondaryPill(g, _rectScreenshotBtn, "Screenshot (PrtSc)");
            }

            float gridTop = mainCard.Y + 70f;
            float cellW = (mainCard.Width - 32f - 24f) / 4f;
            float cellH = 66f;
            for (int i = 0; i < 8; i++)
            {
                int row = i / 4;
                int col = i % 4;
                RectangleF cell = new RectangleF(
                    mainCard.X + 16f + col * (cellW + 8f),
                    gridTop + row * (cellH + 10f),
                    cellW, cellH);
                _modeRects[i] = cell;

                bool selected = (int)_cfg.ActiveCaptureMode == i;
                bool hovered = _hoveredModeCard == i;

                using (GraphicsPath cp = GaBrand.RoundedRect(cell, 12f))
                using (SolidBrush cb = new SolidBrush(GaBrand.Abyss))
                using (Pen border = new Pen(selected || hovered ? GaBrand.Cyan : GaBrand.Edge, selected ? 1.5f : 1f))
                {
                    g.FillPath(cb, cp);
                    g.DrawPath(border, cp);
                }

                DrawCaptureIcon(g, i, new PointF(cell.X + cell.Width / 2f, cell.Y + 22f));

                using (Font mf = GaBrand.MakeFont(10.5f, FontStyle.Bold))
                using (SolidBrush mb = new SolidBrush(selected || hovered ? GaBrand.Text : GaBrand.Slate))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString(ModeTitles[i], mf, mb, new RectangleF(cell.X, cell.Bottom - 20f, cell.Width, 18f), sf);
                }
            }

            // ── 3. SEGMENTED PILL TABS (Video Studio | Output Target | Crash Guard)
            y = mainCard.Bottom + 12f;
            RectangleF tabBar = new RectangleF(pad, y, w - pad * 2, 36f);
            using (GraphicsPath tp = GaBrand.RoundedRect(tabBar, GaBrand.RadiusPill))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen ep = new Pen(GaBrand.Edge, 1f))
            {
                g.FillPath(pb, tp);
                g.DrawPath(ep, tp);
            }

            string[] tabNames = { "Video Studio", "Output Target", "Crash Guard" };
            float tabW = (tabBar.Width - 8f) / 3f;
            for (int i = 0; i < 3; i++)
            {
                RectangleF tr = new RectangleF(tabBar.X + 4f + i * tabW, tabBar.Y + 4f, tabW, tabBar.Height - 8f);
                _tabRects[i] = tr;
                bool active = _activeTab == i;
                if (active)
                {
                    using (GraphicsPath atp = GaBrand.RoundedRect(tr, GaBrand.RadiusPill))
                    using (LinearGradientBrush grad = GaBrand.SignatureGradient(tr))
                        g.FillPath(grad, atp);
                }
                using (Font tf = GaBrand.MakeFont(12f, FontStyle.Bold))
                using (SolidBrush tb = new SolidBrush(active ? GaBrand.Abyss : GaBrand.Slate))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(tabNames[i], tf, tb, tr, sf);
                }
            }

            // ── 4. NUMERIC & PATH CONTROLS CARD
            y = tabBar.Bottom + 12f;
            RectangleF ctrlCard = new RectangleF(pad, y, w - pad * 2, 224f);
            using (GraphicsPath ccp = GaBrand.RoundedRect(ctrlCard, GaBrand.RadiusCard))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen ep = new Pen(GaBrand.Edge, 1f))
            {
                g.FillPath(pb, ccp);
                g.DrawPath(ep, ccp);
            }

            ResetControlHitRects();
            if (_activeTab == 0)
                PaintVideoControlsTab(g, ctrlCard);
            else if (_activeTab == 1)
                PaintOutputControlsTab(g, ctrlCard);
            else
                PaintCrashGuardTab(g, ctrlCard);

            // ── 5. FLOATING STATUS PILL TOAST
            y = ctrlCard.Bottom + 10f;
            RectangleF toastRect = new RectangleF(pad, y, w - pad * 2, 26f);
            using (GraphicsPath toastPath = GaBrand.RoundedRect(toastRect, GaBrand.RadiusPill))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen cp = new Pen(GaBrand.Cyan, 1f))
            using (Font tf = GaBrand.MakeFont(11f, FontStyle.Regular))
            using (SolidBrush cb = new SolidBrush(GaBrand.Cyan))
            {
                g.FillPath(pb, toastPath);
                g.DrawPath(cp, toastPath);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(_statusToast, tf, cb, toastRect, sf);
            }

            // ── 6. PAUSE/RESUME (F10) SECONDARY PILL + START/STOP (F9) PRIMARY PILL
            y = toastRect.Bottom + 10f;
            float pauseW = 132f;
            _rectPauseBtn = new RectangleF(pad, y, pauseW, 44f);
            DrawSecondaryPill(g, _rectPauseBtn, isPaused ? "Resume (F10)" : "Pause (F10)");

            _rectPrimaryBtn = new RectangleF(pad + pauseW + 10f, y, w - pad * 2 - pauseW - 10f, 44f);
            if (_hoverPrimaryBtn)
            {
                using (GraphicsPath glow = GaBrand.RoundedRect(RectangleF.Inflate(_rectPrimaryBtn, 2, 2), GaBrand.RadiusPill))
                using (Pen gp = new Pen(Color.FromArgb(150, GaBrand.Cyan), 3f))
                    g.DrawPath(gp, glow);
            }
            using (GraphicsPath btnPath = GaBrand.RoundedRect(_rectPrimaryBtn, GaBrand.RadiusPill))
            using (LinearGradientBrush btnGrad = GaBrand.SignatureGradient(_rectPrimaryBtn))
            using (Font bf = GaBrand.MakeFont(14.5f, FontStyle.Bold))
            using (SolidBrush abyssText = new SolidBrush(GaBrand.Abyss))
            {
                g.FillPath(btnGrad, btnPath);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                string label = isRec ? "Stop & Save Video (F9)" : "Start Video Recording (F9)";
                g.DrawString(label, bf, abyssText, _rectPrimaryBtn, sf);
            }

            // ── 7. HELPER TEXT & BRAND FOOTER
            using (Font helpF = GaBrand.MakeFont(11.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush hoverB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("Tray & taskbar icon blinks red while recording · 1s crash-safe flush.", helpF, slateB, pad, _rectPrimaryBtn.Bottom + 8f);

                float fy = h - 28f;
                _rectFooterLink = new RectangleF(pad, fy, 200f, 20f);
                g.DrawString(GaBrand.FooterCredit, helpF, _hoverFooterLink ? hoverB : slateB, _rectFooterLink.Location);

                RectangleF vChip = new RectangleF(w - pad - 54f, fy - 2f, 54f, 22f);
                using (GraphicsPath vp = GaBrand.RoundedRect(vChip, GaBrand.RadiusPill))
                using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
                using (Pen ep = new Pen(GaBrand.Edge, 1f))
                {
                    g.FillPath(pb, vp);
                    g.DrawPath(ep, vp);
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(GaBrand.VersionText, helpF, slateB, vChip, sf);
                }
            }
        }

        private void ResetControlHitRects()
        {
            _rectFolderInput = _rectFolderBrowseBtn = _rectFormatCycle = _rectQualityCycle =
                _rectFpsCycle = _rectOutputCycle = _rectCursorToggle =
                _rectAudioToggle = _rectPortableToggle = RectangleF.Empty;
        }

        private void PaintVideoControlsTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("SINGLE-FILE AUDIO+VIDEO RECORDER SETTINGS", secF, slateB, card.X + 16f, card.Y + 12f);

                // Row 1: Video Format (MP4 / MKV / AVI) + Quality (Low / Medium / High / Lossless)
                g.DrawString("Format & Quality", bodyF, textB, card.X + 16f, card.Y + 38f);
                _rectFormatCycle = new RectangleF(card.Right - 212f, card.Y + 33f, 86f, 28f);
                DrawSecondaryPill(g, _rectFormatCycle, _cfg.VideoFormat.ToString());

                _rectQualityCycle = new RectangleF(card.Right - 118f, card.Y + 33f, 102f, 28f);
                DrawSecondaryPill(g, _rectQualityCycle, _cfg.VideoQuality.ToString());

                // Row 2: Frame Rate + Single-File Audio+Video
                g.DrawString("FPS & Muxed Audio", bodyF, textB, card.X + 16f, card.Y + 74f);
                _rectFpsCycle = new RectangleF(card.Right - 212f, card.Y + 69f, 86f, 28f);
                DrawSecondaryPill(g, _rectFpsCycle, $"{_cfg.VideoFps} FPS");

                _rectAudioToggle = new RectangleF(card.Right - 118f, card.Y + 69f, 102f, 28f);
                DrawSecondaryPill(g, _rectAudioToggle, _cfg.RecordAudio ? "1-File A/V" : "Video Only");

                // Row 3: Mouse Cursor & Click Halo
                g.DrawString("Mouse Cursor + Click Halo", bodyF, textB, card.X + 16f, card.Y + 110f);
                _rectCursorToggle = new RectangleF(card.Right - 118f, card.Y + 105f, 102f, 28f);
                DrawSecondaryPill(g, _rectCursorToggle, _cfg.IncludeMouseCursor ? "Enabled" : "Hidden");

                // Row 4: Editable Save Directory Path + Browse Button right inside Video Studio!
                g.DrawString("Auto-Save Directory (Click path to edit or Browse):", secF, slateB, card.X + 16f, card.Y + 146f);
                string shortPath = _cfg.AutoSaveFolder ?? "";
                if (shortPath.Length > 36) shortPath = "..." + shortPath.Substring(shortPath.Length - 33);

                _rectFolderInput = new RectangleF(card.X + 16f, card.Y + 168f, card.Width - 116f, 32f);
                DrawTertiaryInput(g, _rectFolderInput, shortPath);

                _rectFolderBrowseBtn = new RectangleF(card.Right - 92f, card.Y + 168f, 76f, 32f);
                DrawSecondaryPill(g, _rectFolderBrowseBtn, "Browse");
            }
        }

        private void PaintOutputControlsTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("OFFLINE SCREENSHOT & VIDEO OUTPUT TARGET", secF, slateB, card.X + 16f, card.Y + 14f);

                g.DrawString("Post-Capture Target", bodyF, textB, card.X + 16f, card.Y + 46f);
                _rectOutputCycle = new RectangleF(card.Right - 150f, card.Y + 40f, 134f, 30f);
                DrawSecondaryPill(g, _rectOutputCycle, _cfg.ActiveOutputTarget.ToString());

                g.DrawString("Filename Template", bodyF, textB, card.X + 16f, card.Y + 90f);
                RectangleF tplBox = new RectangleF(card.Right - 196f, card.Y + 84f, 180f, 30f);
                DrawTertiaryInput(g, tplBox, _cfg.FileNameTemplate);

                g.DrawString("Fixed / Scroll Pacing", bodyF, textB, card.X + 16f, card.Y + 134f);
                RectangleF fixBox = new RectangleF(card.Right - 196f, card.Y + 128f, 180f, 30f);
                DrawTertiaryInput(g, fixBox, $"{_cfg.FixedRegionWidth}×{_cfg.FixedRegionHeight} · {_cfg.AutoScrollIntervalMs}ms");

                g.DrawString("Save Directory", bodyF, textB, card.X + 16f, card.Y + 178f);
                _rectFolderBrowseBtn = new RectangleF(card.Right - 150f, card.Y + 172f, 134f, 30f);
                DrawSecondaryPill(g, _rectFolderBrowseBtn, "Change Folder...");
            }
        }

        private void PaintCrashGuardTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush greenB = new SolidBrush(GaBrand.Green))
            {
                g.DrawString("CRASH-SAFE AUTO-SAVE RECOVERY GUARD", secF, slateB, card.X + 16f, card.Y + 14f);
                g.DrawString("● Active: 1s disk flush + auto-saves on sudden close", bodyF, greenB, card.X + 16f, card.Y + 38f);

                g.DrawString("Click path to type custom directory, or click Browse:", secF, slateB, card.X + 16f, card.Y + 68f);
                string shortPath = _cfg.AutoSaveFolder ?? "";
                if (shortPath.Length > 36) shortPath = "..." + shortPath.Substring(shortPath.Length - 33);
                _rectFolderInput = new RectangleF(card.X + 16f, card.Y + 90f, card.Width - 116f, 32f);
                DrawTertiaryInput(g, _rectFolderInput, shortPath);

                _rectFolderBrowseBtn = new RectangleF(card.Right - 92f, card.Y + 90f, 76f, 32f);
                DrawSecondaryPill(g, _rectFolderBrowseBtn, "Browse");

                g.DrawString("100% Portable Mode (Portable.db)", bodyF, textB, card.X + 16f, card.Y + 144f);
                _rectPortableToggle = new RectangleF(card.Right - 116f, card.Y + 138f, 100f, 30f);
                DrawSecondaryPill(g, _rectPortableToggle, _cfg.PortableMode ? "Portable" : "Registry");

                g.DrawString("Taskbar & Tray Red Blink on Record", bodyF, textB, card.X + 16f, card.Y + 184f);
                RectangleF blinkBadge = new RectangleF(card.Right - 116f, card.Y + 178f, 100f, 30f);
                DrawTertiaryInput(g, blinkBadge, "Active (500ms)");
            }
        }

        private static void DrawSecondaryPill(Graphics g, RectangleF rect, string text)
        {
            using (GraphicsPath gp = GaBrand.RoundedRect(rect, GaBrand.RadiusPill))
            using (SolidBrush fill = new SolidBrush(GaBrand.Abyss))
            using (Pen border = new Pen(GaBrand.Cyan, 1f))
            using (Font f = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(GaBrand.Text))
            {
                g.FillPath(fill, gp);
                g.DrawPath(border, gp);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(text, f, tb, rect, sf);
            }
        }

        private static void DrawTertiaryInput(Graphics g, RectangleF rect, string text)
        {
            using (GraphicsPath gp = GaBrand.RoundedRect(rect, GaBrand.RadiusInput))
            using (SolidBrush fill = new SolidBrush(GaBrand.Abyss))
            using (Pen border = new Pen(GaBrand.Edge, 1f))
            using (Font f = GaBrand.MakeFont(11.5f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(GaBrand.Ice))
            {
                g.FillPath(fill, gp);
                g.DrawPath(border, gp);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(text, f, tb, rect, sf);
            }
        }

        private static void DrawCaptureIcon(Graphics g, int index, PointF center)
        {
            using (Pen p = new Pen(GaBrand.Cyan, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                RectangleF r = new RectangleF(center.X - 9, center.Y - 7, 18, 14);
                switch (index)
                {
                    case 0:
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X, r.Y + 4, r.Right, r.Y + 4);
                        break;
                    case 1:
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawEllipse(p, center.X - 3, center.Y - 3, 6, 6);
                        break;
                    case 2:
                        p.DashStyle = DashStyle.Dot;
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 3:
                        g.DrawEllipse(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 4:
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X + 3, r.Y + 3, r.Right - 3, r.Bottom - 3);
                        break;
                    case 5:
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, center.X, r.Y + 3, center.X, r.Bottom - 3);
                        break;
                    case 6:
                        g.DrawRectangle(p, center.X - 6, center.Y - 5, 12, 10);
                        break;
                    case 7:
                        g.DrawArc(p, r, 30, 290);
                        break;
                }
            }
        }
    }

    // ========================================================================
    // 12. ASSERT-BASED SELF-CHECK SUITE (Ponytail Quality Gate)
    // ========================================================================
    public static class SelfTests
    {
        public static void RunAll()
        {
            // 1. Brand palette & product identity assertions
            Debug.Assert(GaBrand.ProductName == "Open Screen Recorder", "Product name mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Abyss) == "#001E36", "Abyss mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Cyan)  == "#00CDFF", "Cyan mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Red)   == "#FF5D6C", "Red mismatch");

            // 2. Template token expansion check
            DateTime sampleDt = new DateTime(2026, 10, 30, 14, 5, 9);
            string expanded = StorageEngine.ExpandTemplate("OSR_$Y-$M-$D_$H-$N-$S_#", sampleDt, 7);
            if (expanded != "OSR_2026-10-30_14-05-09_007")
                throw new InvalidOperationException("Template expansion check failed: " + expanded);

            // 3. Single-file interleaved Audio + Video RIFF writer verification
            string tempAvi = Path.Combine(Path.GetTempPath(), "osr_selfcheck_" + Guid.NewGuid().ToString("N") + ".avi");
            try
            {
                using (CrashSafeAviWriter writer = new CrashSafeAviWriter(tempAvi, 64, 64, 24, VideoQualityLevel.High, includeAudio: true))
                using (Bitmap frame = new Bitmap(64, 64))
                {
                    writer.WriteFrame(frame);
                    byte[] dummyPcm = new byte[1024];
                    writer.WriteAudioPcmBytes(dummyPcm, dummyPcm.Length);
                    writer.FlushCheckpoint();
                }
                byte[] bytes = File.ReadAllBytes(tempAvi);
                string ascii = Encoding.ASCII.GetString(bytes);
                if (!ascii.StartsWith("RIFF") || !ascii.Contains("00dc") || !ascii.Contains("01wb"))
                    throw new InvalidOperationException("Single-file interleaved audio+video verification failed.");
            }
            finally
            {
                if (File.Exists(tempAvi)) File.Delete(tempAvi);
            }
        }
    }

    // ========================================================================
    // 13. PROGRAM ENTRY POINT
    // ========================================================================
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            SelfTests.RunAll();
            if (args != null && Array.Exists(args, a => a == "--self-check"))
            {
                Console.WriteLine("SELF-CHECK OK: Open Screen Recorder v1.1 verified.");
                return 0;
            }

            try { NativeMethods.SetProcessDPIAware(); } catch { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppConfig cfg = StorageEngine.Load();
            CrashSafeGuard.Initialize(cfg);

            using (MainCaptureForm mainForm = new MainCaptureForm(cfg))
            {
                Application.Run(mainForm);
            }
            return 0;
        }
    }
}
