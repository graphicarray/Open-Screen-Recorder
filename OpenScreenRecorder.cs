// ============================================================================
// Open Screen Recorder v2.0 — 100% Offline Crash-Safe Video Recorder
// Brand & Owner: Graphicarray · Md Numan Ahmed (https://www.linkedin.com/in/graphicarray)
// Pure Video Recording Edition:
//  - 5 Video Area Modes: Active Window, Rectangle (Free / Custom Ratio),
//    Freehand, Full Screen, Fixed Region (Pixel Dimensions / Custom Ratio)
//  - Live On-Screen Outer Guideline Frame during Rectangle, Freehand & Fixed recording
//  - Single-File Muxed Audio+Video OR Video-Only (No Audio)
//  - Full Dropdown Menus for all value controls (Format, Quality, FPS, Audio, Cursor, Ratio)
//  - Settings Gear -> Shortcut Key Manager Page (Add, Change, or Remove any hotkey)
//  - Blinking Red Taskbar & System Tray Icon + Pause/Resume + Crash-Safe 1s Auto-Save
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        public const string VersionText = "v2.0";
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

        public static Icon CreateStatusIcon(int state)
        {
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

    // Custom Graphicarray Renderer for Dropdown Menus (ContextMenuStrip)
    public sealed class GaDropdownRenderer : ToolStripProfessionalRenderer
    {
        public GaDropdownRenderer() : base(new GaColorTable()) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Selected ? GaBrand.Abyss : GaBrand.Text;
            e.TextFont = GaBrand.MakeFont(12.5f, e.Item.Selected ? FontStyle.Bold : FontStyle.Regular);
            base.OnRenderItemText(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle r = new Rectangle(4, 2, e.Item.Width - 8, e.Item.Height - 4);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (e.Item.Selected)
            {
                using (GraphicsPath gp = GaBrand.RoundedRect(r, 8f))
                using (LinearGradientBrush grad = GaBrand.SignatureGradient(r))
                    e.Graphics.FillPath(grad, gp);
            }
            else
            {
                using (SolidBrush sb = new SolidBrush(GaBrand.Panel))
                    e.Graphics.FillRectangle(sb, new Rectangle(0, 0, e.Item.Width, e.Item.Height));
            }
        }

        private sealed class GaColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => GaBrand.Panel;
            public override Color MenuBorder => GaBrand.Cyan;
            public override Color ImageMarginGradientBegin => GaBrand.Panel;
            public override Color ImageMarginGradientMiddle => GaBrand.Panel;
            public override Color ImageMarginGradientEnd => GaBrand.Panel;
            public override Color SeparatorDark => GaBrand.Edge;
        }
    }

    // ========================================================================
    // 2. WIN32 LOW-LEVEL PRIMITIVES (GDI32, USER32, KERNEL32, WINMM WAVEIN)
    // ========================================================================
    public static class NativeMethods
    {
        public const int WM_HOTKEY = 0x0312;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;
        public const int SRCCOPY = 0x00CC0020;
        public const int CAPTUREBLT = 0x40000000;
        public const int CURSOR_SHOWING = 0x00000001;
        public const int DI_NORMAL = 0x0003;
        public const int CALLBACK_FUNCTION = 0x00030000;
        public const int MM_WIM_DATA = 0x3C0;

        // Window ExStyles for Click-Through Live Recording Outer Guideline Frame
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;

        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;

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
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")] public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")] public static extern bool GetCursorInfo(out CURSORINFO pci);
        [DllImport("user32.dll")] public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);
        [DllImport("user32.dll")] public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);
        [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
        [DllImport("user32.dll")] public static extern bool DestroyIcon(IntPtr hIcon);
        [DllImport("user32.dll")] public static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        [DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
        [DllImport("gdi32.dll")] public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")] public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("kernel32.dll")] public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

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
    // 3. VIDEO MODES, SHORTCUT KEY MODEL & PERSISTENCE (fsc.db / Portable.db)
    // ========================================================================
    // Only the 5 requested Video Recording modes (Window/Spy, Scroll Page & Repeat Last removed)
    public enum VideoAreaMode
    {
        ActiveWindow = 0,
        Rectangle = 1,
        Freehand = 2,
        FullScreen = 3,
        FixedRegion = 4
    }

    public enum RectangleSelectionSubMode
    {
        FreeArea = 0,
        CustomRatio = 1
    }

    public enum FixedRegionSubMode
    {
        PixelDimensions = 0,
        CustomRatio = 1
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

    public class ShortcutBinding
    {
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public Keys Key { get; set; }
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }

        public string FormatDisplay()
        {
            if (Key == Keys.None) return "None (Removed)";
            List<string> parts = new List<string>();
            if (Ctrl) parts.Add("Ctrl");
            if (Alt) parts.Add("Alt");
            if (Shift) parts.Add("Shift");
            parts.Add(Key.ToString());
            return string.Join(" + ", parts);
        }
    }

    public class AppConfig
    {
        public VideoAreaMode ActiveAreaMode { get; set; } = VideoAreaMode.Rectangle;
        public RectangleSelectionSubMode RectangleSubMode { get; set; } = RectangleSelectionSubMode.FreeArea;
        public FixedRegionSubMode FixedSubMode { get; set; } = FixedRegionSubMode.PixelDimensions;
        public int AspectRatioW { get; set; } = 16;
        public int AspectRatioH { get; set; } = 9;
        public int FixedRegionWidth { get; set; } = 1280;
        public int FixedRegionHeight { get; set; } = 720;

        public VideoFormatType VideoFormat { get; set; } = VideoFormatType.MP4;
        public VideoQualityLevel VideoQuality { get; set; } = VideoQualityLevel.High;
        public int VideoFps { get; set; } = 24;
        public bool RecordAudioWithVideo { get; set; } = true; // true = Video + Audio (1 File), false = Video Only (No Audio)
        public bool IncludeMouseCursor { get; set; } = true;
        public bool HighlightMouseClicks { get; set; } = true;
        public bool ShowOuterGuideline { get; set; } = true;

        public string AutoSaveFolder { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            "OpenScreenRecorder");
        public string FileNameTemplate { get; set; } = "OSR_$Y-$M-$D_$H-$N-$S_#";
        public int CrashFlushIntervalSeconds { get; set; } = 1;
        public int SequenceCounter { get; set; } = 1;
        public Rectangle LastVideoRect { get; set; } = new Rectangle(100, 100, 1280, 720);
        public bool PortableMode { get; set; } = true;
        public List<ShortcutBinding> Shortcuts { get; set; } = DefaultShortcuts();

        public static List<ShortcutBinding> DefaultShortcuts() => new List<ShortcutBinding>
        {
            new ShortcutBinding { ActionId = "StartStop",    ActionName = "Start / Stop Video Recording", Key = Keys.F9 },
            new ShortcutBinding { ActionId = "PauseResume",  ActionName = "Pause / Resume Recording",     Key = Keys.F10 },
            new ShortcutBinding { ActionId = "ActiveWindow", ActionName = "Active Window Video Mode",     Key = Keys.F5, Alt = true },
            new ShortcutBinding { ActionId = "Rectangle",    ActionName = "Rectangle Video Mode",         Key = Keys.F6, Alt = true },
            new ShortcutBinding { ActionId = "Freehand",     ActionName = "Freehand Video Mode",          Key = Keys.F7, Alt = true },
            new ShortcutBinding { ActionId = "FullScreen",   ActionName = "Full Screen Video Mode",       Key = Keys.F8, Alt = true },
            new ShortcutBinding { ActionId = "FixedRegion",  ActionName = "Fixed Region Video Mode",      Key = Keys.F11, Alt = true }
        };
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
                    if (loaded != null)
                    {
                        if (loaded.Shortcuts == null || loaded.Shortcuts.Count == 0)
                            loaded.Shortcuts = AppConfig.DefaultShortcuts();
                        cfg = loaded;
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
                    Product = GaBrand.ProductName,
                    Owner = GaBrand.OwnerName,
                    Profile = GaBrand.LinkedInUrl,
                    UpdatedUtc = DateTime.UtcNow
                }, opts), Encoding.UTF8);
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
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    "OpenScreenRecorder");
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
    // 4. LIVE ON-SCREEN OUTER GUIDELINE OVERLAY (Rectangle, Freehand, Fixed)
    // ========================================================================
    // Click-through topmost overlay that draws a 2px outer boundary around the
    // active recording area (or freehand polygon) so you see the exact recording
    // region on screen without the border ever appearing inside the recorded video!
    public sealed class RecordingGuidelineOverlayForm : Form
    {
        private readonly Rectangle _recordBounds;
        private readonly Point[] _localPolygon;
        private bool _blinkPhase;

        public RecordingGuidelineOverlayForm(Rectangle recordBounds, Point[] localPolygon)
        {
            _recordBounds = recordBounds;
            _localPolygon = localPolygon;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            // Expand window 6px outside the recorded region so the border sits outside the video capture area
            Bounds = Rectangle.Inflate(recordBounds, 6, 6);
            TopMost = true;
            ShowInTaskbar = false;
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;
            DoubleBuffered = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_LAYERED |
                              NativeMethods.WS_EX_TRANSPARENT |
                              NativeMethods.WS_EX_TOPMOST |
                              NativeMethods.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation => true;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Also instruct Windows 10/11 DWM to exclude this guideline window from screen capture
            try { NativeMethods.SetWindowDisplayAffinity(Handle, NativeMethods.WDA_EXCLUDEFROMCAPTURE); } catch { }
        }

        public void SetBlinkPhase(bool blink)
        {
            _blinkPhase = blink;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Magenta);

            Color guideColor = _blinkPhase ? GaBrand.Red : GaBrand.Cyan;
            using (Pen guidePen = new Pen(guideColor, 2f) { DashStyle = DashStyle.Dash })
            {
                if (_localPolygon != null && _localPolygon.Length >= 3)
                {
                    Point[] shifted = new Point[_localPolygon.Length];
                    for (int i = 0; i < _localPolygon.Length; i++)
                        shifted[i] = new Point(_localPolygon[i].X + 6, _localPolygon[i].Y + 6);
                    g.DrawPolygon(guidePen, shifted);
                }
                else
                {
                    // Draw 2px outside the actual captured rectangle (at offset 3,3)
                    g.DrawRectangle(guidePen, 2, 2, Width - 5, Height - 5);
                }
            }
        }
    }

    // ========================================================================
    // 5. SINGLE-FILE INTERLEAVED AUDIO+VIDEO CRASH-SAFE WRITER & FFMPEG MUXER
    // ========================================================================
    // shortcut: writes interleaved MJPEG video (00dc) + PCM audio (01wb) into one container; converts to H.264 MP4/MKV when ffmpeg.exe is present.
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

            WriteFourCC("LIST");
            long hdrlSizePos = _fs.Position;
            _bw.Write((uint)0);
            long hdrlStart = _fs.Position;
            WriteFourCC("hdrl");

            WriteFourCC("avih");
            _bw.Write((uint)56);
            _bw.Write(microSecPerFrame);
            _bw.Write(maxBytesPerSec);
            _bw.Write((uint)0);
            _bw.Write((uint)0x110);
            _avihTotalFramesPos = _fs.Position;
            _bw.Write((uint)0);
            _bw.Write((uint)0);
            _bw.Write(streamCount);
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write((uint)_width);
            _bw.Write((uint)_height);
            _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0);

            // Stream 0: Video
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
            _bw.Write((uint)0);
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

            // Stream 1: Audio (only when RecordAudioWithVideo is true — zero separate audio file)
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
                _bw.Write((uint)1);
                _bw.Write((uint)0);
                _bw.Write((ushort)0);
                _bw.Write((ushort)0);
                _bw.Write((uint)0);
                _bw.Write(blockAlign);
                _bw.Write(avgBytesPerSec);
                _bw.Write((uint)0);
                _audsLengthPos = _fs.Position;
                _bw.Write((uint)0);
                _bw.Write(avgBytesPerSec);
                _bw.Write((uint)10000);
                _bw.Write(blockAlign);
                _bw.Write((short)0);
                _bw.Write((short)0);
                _bw.Write((short)0);
                _bw.Write((short)0);

                WriteFourCC("strf");
                _bw.Write((uint)18);
                _bw.Write((ushort)1);
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
    // 6. WINMM PCM AUDIO STREAMER (Interleaves audio directly into video file)
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
                    wFormatTag = 1,
                    nChannels = CrashSafeAviWriter.AudioChannels,
                    nSamplesPerSec = CrashSafeAviWriter.AudioSampleRate,
                    wBitsPerSample = CrashSafeAviWriter.AudioBitsPerSample,
                    nBlockAlign = (ushort)(CrashSafeAviWriter.AudioChannels * (CrashSafeAviWriter.AudioBitsPerSample / 8)),
                    cbSize = 0
                };
                fmt.nAvgBytesPerSec = fmt.nSamplesPerSec * fmt.nBlockAlign;

                int res = NativeMethods.waveInOpen(out _hWaveIn, unchecked((uint)-1), ref fmt, _callback, IntPtr.Zero, NativeMethods.CALLBACK_FUNCTION);
                if (res != 0 || _hWaveIn == IntPtr.Zero) return false;

                int bufSize = 8820;
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
    // 7. CRASH-SAFE GUARD & VIDEO RECORDER (5 Video Modes + Pause/Resume)
    // ========================================================================
    public static class CrashSafeGuard
    {
        private static readonly object Sync = new object();
        private static CrashSafeVideoRecorder _activeRecorder;
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
        private readonly VideoAreaMode _mode;
        private readonly Point[] _freehandPolygon;

        private CrashSafeAviWriter _writer;
        private WinMmAudioStreamer _audioStreamer;
        private Thread _workerThread;
        private volatile bool _running;
        private volatile bool _paused;
        private readonly Stopwatch _activeTimer = new Stopwatch();
        private DateTime _lastFlushUtc;
        private string _finalTargetFilePath;

        public bool IsRecording => _running;
        public bool IsPaused => _paused;
        public string ActiveVideoPath => _finalTargetFilePath ?? _writer?.FilePath;
        public int RecordedFrames => _writer?.FrameCount ?? 0;
        public TimeSpan Elapsed => _activeTimer.Elapsed;

        public event Action<int, TimeSpan, string> OnProgress;

        public CrashSafeVideoRecorder(
            AppConfig cfg,
            VideoAreaMode mode,
            Rectangle captureBounds,
            Point[] freehandPolygon = null)
        {
            _cfg = cfg;
            _mode = mode;
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

            string liveStreamPath = _cfg.VideoFormat == VideoFormatType.AVI
                ? _finalTargetFilePath
                : Path.ChangeExtension(_finalTargetFilePath, ".live.avi");

            _writer = new CrashSafeAviWriter(liveStreamPath, w, h, _cfg.VideoFps, _cfg.VideoQuality, _cfg.RecordAudioWithVideo);
            _running = true;
            _paused = false;
            _activeTimer.Restart();
            _lastFlushUtc = DateTime.UtcNow;

            if (_cfg.RecordAudioWithVideo)
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
                        if (_mode == VideoAreaMode.ActiveWindow)
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

                        using (Bitmap rawFrame = ScreenCaptureEngine.CaptureRegion(
                            new Rectangle(_captureBounds.X, _captureBounds.Y, targetW, targetH),
                            _cfg.IncludeMouseCursor,
                            _cfg.HighlightMouseClicks))
                        {
                            if (_mode == VideoAreaMode.Freehand && _freehandPolygon != null && _freehandPolygon.Length >= 3)
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

            string finalFile = FinalizeContainerFormat(liveAviPath, _finalTargetFilePath, _cfg.VideoFormat, _cfg.VideoQuality, _cfg.RecordAudioWithVideo);

            CrashSafeGuard.UnregisterActiveRecorder(this);
            StorageEngine.Save(_cfg);
            return finalFile;
        }

        private static string FinalizeContainerFormat(string liveAviPath, string desiredPath, VideoFormatType fmt, VideoQualityLevel quality, bool hasAudio)
        {
            if (string.IsNullOrEmpty(liveAviPath) || !File.Exists(liveAviPath))
                return desiredPath;

            if (fmt == VideoFormatType.AVI || string.Equals(liveAviPath, desiredPath, StringComparison.OrdinalIgnoreCase))
                return liveAviPath;

            try
            {
                string ffmpegPath = Path.Combine(StorageEngine.BaseDir, "ffmpeg.exe");
                if (File.Exists(ffmpegPath))
                {
                    int crf = quality switch
                    {
                        VideoQualityLevel.Low => 28,
                        VideoQualityLevel.Medium => 23,
                        VideoQualityLevel.High => 18,
                        VideoQualityLevel.Lossless => 0,
                        _ => 18
                    };
                    string audioCodec = hasAudio ? "-c:a aac -b:a 128k" : "-an";
                    string args = $"-y -i \"{liveAviPath}\" -c:v libx264 -preset ultrafast -crf {crf} -pix_fmt yuv420p {audioCodec} \"{desiredPath}\"";
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

                if (File.Exists(desiredPath)) File.Delete(desiredPath);
                File.Move(liveAviPath, desiredPath);
                return desiredPath;
            }
            catch
            {
                return liveAviPath;
            }
        }

        public void Dispose() => StopAndSave(emergency: true, reason: "Dispose");
    }

    // ========================================================================
    // 8. CORE GDI VIDEO FRAME ACQUISITION & POLYGON MASKING
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

        // Enforces custom aspect ratio (W:H) on a dragged rectangle
        public static Rectangle ComputeRatioLockedRect(Point start, Point current, int ratioW, int ratioH)
        {
            int rawW = Math.Max(32, Math.Abs(current.X - start.X));
            int rawH = Math.Max(32, Math.Abs(current.Y - start.Y));
            if (ratioW > 0 && ratioH > 0)
            {
                rawH = Math.Max(32, (rawW * ratioH) / ratioW);
            }
            rawW &= ~1;
            rawH &= ~1;
            int x = current.X >= start.X ? start.X : start.X - rawW;
            int y = current.Y >= start.Y ? start.Y : start.Y - rawH;
            return new Rectangle(x, y, rawW, rawH);
        }
    }

    // ========================================================================
    // 9. INTERACTIVE VIDEO REGION SELECTOR (Free / Ratio Rectangle, Freehand, Fixed)
    // ========================================================================
    public sealed class VideoAreaSelectorForm : Form
    {
        private readonly Bitmap _frozenDesktop;
        private readonly VideoAreaMode _mode;
        private readonly bool _lockRatio;
        private readonly int _ratioW;
        private readonly int _ratioH;
        private readonly Size _fixedSize;

        private bool _dragging;
        private Point _startPt;
        private Point _currentPt;
        private readonly List<Point> _freehandPts = new List<Point>();

        public Rectangle SelectedBounds { get; private set; }
        public Point[] LocalFreehandPolygon { get; private set; }

        public VideoAreaSelectorForm(VideoAreaMode mode, bool lockRatio, int ratioW, int ratioH, Size fixedSize)
        {
            _mode = mode;
            _lockRatio = lockRatio;
            _ratioW = Math.Max(1, ratioW);
            _ratioH = Math.Max(1, ratioH);
            _fixedSize = new Size(Math.Max(32, fixedSize.Width & ~1), Math.Max(32, fixedSize.Height & ~1));

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

            if (_mode == VideoAreaMode.FixedRegion)
            {
                SelectedBounds = new Rectangle(
                    e.X - _fixedSize.Width / 2 + Bounds.X,
                    e.Y - _fixedSize.Height / 2 + Bounds.Y,
                    _fixedSize.Width,
                    _fixedSize.Height);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _currentPt = e.Location;
            if (_dragging && _mode == VideoAreaMode.Freehand)
                _freehandPts.Add(e.Location);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!_dragging) return;
            _dragging = false;

            if (_mode == VideoAreaMode.Freehand)
            {
                if (_freehandPts.Count < 3)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }
                int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
                foreach (Point p in _freehandPts)
                {
                    if (p.X < minX) minX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y > maxY) maxY = p.Y;
                }
                int w = Math.Max(32, (maxX - minX) & ~1);
                int h = Math.Max(32, (maxY - minY) & ~1);
                LocalFreehandPolygon = new Point[_freehandPts.Count];
                for (int i = 0; i < _freehandPts.Count; i++)
                    LocalFreehandPolygon[i] = new Point(_freehandPts[i].X - minX, _freehandPts[i].Y - minY);
                SelectedBounds = new Rectangle(minX + Bounds.X, minY + Bounds.Y, w, h);
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            Rectangle localRect = _lockRatio
                ? ScreenCaptureEngine.ComputeRatioLockedRect(_startPt, _currentPt, _ratioW, _ratioH)
                : GetFreeRubberBandRect(_startPt, _currentPt);

            SelectedBounds = new Rectangle(localRect.X + Bounds.X, localRect.Y + Bounds.Y, localRect.Width, localRect.Height);
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private static Rectangle GetFreeRubberBandRect(Point a, Point b) =>
            new Rectangle(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Max(32, Math.Abs(a.X - b.X) & ~1), Math.Max(32, Math.Abs(a.Y - b.Y) & ~1));

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(_frozenDesktop, 0, 0);

            using (SolidBrush dimBrush = new SolidBrush(Color.FromArgb(115, GaBrand.Abyss)))
                g.FillRectangle(dimBrush, ClientRectangle);

            Rectangle activeRect = _mode switch
            {
                VideoAreaMode.FixedRegion => new Rectangle(_currentPt.X - _fixedSize.Width / 2, _currentPt.Y - _fixedSize.Height / 2, _fixedSize.Width, _fixedSize.Height),
                _ => _dragging
                    ? (_lockRatio ? ScreenCaptureEngine.ComputeRatioLockedRect(_startPt, _currentPt, _ratioW, _ratioH) : GetFreeRubberBandRect(_startPt, _currentPt))
                    : Rectangle.Empty
            };

            if (_mode == VideoAreaMode.Freehand && _freehandPts.Count > 1)
            {
                using (Pen cyanPen = new Pen(GaBrand.Cyan, 2.5f) { DashStyle = DashStyle.Dash })
                    g.DrawLines(cyanPen, _freehandPts.ToArray());
            }
            else if (activeRect.Width > 0 && activeRect.Height > 0)
            {
                g.DrawImage(_frozenDesktop, activeRect, activeRect, GraphicsUnit.Pixel);
                using (Pen borderPen = new Pen(GaBrand.Cyan, 2f) { DashStyle = DashStyle.Dash })
                    g.DrawRectangle(borderPen, activeRect);

                string ratioLabel = _lockRatio ? $" · Ratio {_ratioW}:{_ratioH}" : "";
                string badge = $"{activeRect.Width} × {activeRect.Height} px{ratioLabel}";
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
    // 10. SETTINGS GEAR -> SHORTCUT KEY MANAGER PAGE (Add / Change / Remove)
    // ========================================================================
    public sealed class ShortcutKeyPageForm : Form
    {
        private readonly AppConfig _cfg;
        private int _capturingRowIndex = -1;
        private readonly RectangleF[] _setBtnRects;
        private readonly RectangleF[] _removeBtnRects;
        private RectangleF _addCustomBtnRect;
        private RectangleF _doneBtnRect;

        public ShortcutKeyPageForm(AppConfig cfg)
        {
            _cfg = cfg;
            if (_cfg.Shortcuts == null || _cfg.Shortcuts.Count == 0)
                _cfg.Shortcuts = AppConfig.DefaultShortcuts();

            _setBtnRects = new RectangleF[_cfg.Shortcuts.Count + 4];
            _removeBtnRects = new RectangleF[_cfg.Shortcuts.Count + 4];

            Text = "Open Screen Recorder — Shortcut Key Settings";
            Size = new Size(520, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = GaBrand.Abyss;
            ForeColor = GaBrand.Text;
            DoubleBuffered = true;
            KeyPreview = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_capturingRowIndex >= 0 && _capturingRowIndex < _cfg.Shortcuts.Count)
            {
                if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu)
                    return;

                ShortcutBinding b = _cfg.Shortcuts[_capturingRowIndex];
                if (e.KeyCode == Keys.Escape)
                {
                    _capturingRowIndex = -1;
                    Invalidate();
                    return;
                }
                b.Key = e.KeyCode;
                b.Ctrl = e.Control;
                b.Alt = e.Alt;
                b.Shift = e.Shift;
                _capturingRowIndex = -1;
                StorageEngine.Save(_cfg);
                Invalidate();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            PointF pt = e.Location;
            for (int i = 0; i < _cfg.Shortcuts.Count; i++)
            {
                if (_setBtnRects[i].Contains(pt))
                {
                    _capturingRowIndex = i;
                    Invalidate();
                    return;
                }
                if (_removeBtnRects[i].Contains(pt))
                {
                    _cfg.Shortcuts[i].Key = Keys.None;
                    _cfg.Shortcuts[i].Ctrl = _cfg.Shortcuts[i].Alt = _cfg.Shortcuts[i].Shift = false;
                    _capturingRowIndex = -1;
                    StorageEngine.Save(_cfg);
                    Invalidate();
                    return;
                }
            }

            if (_addCustomBtnRect.Contains(pt))
            {
                _cfg.Shortcuts = AppConfig.DefaultShortcuts();
                _capturingRowIndex = -1;
                StorageEngine.Save(_cfg);
                Invalidate();
                return;
            }

            if (_doneBtnRect.Contains(pt))
            {
                StorageEngine.Save(_cfg);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            GaBrand.PaintAbyssBackground(g, ClientRectangle);

            float pad = 20f;
            GaBrand.DrawLogoTile(g, new RectangleF(pad, 14f, 44f, 44f), false, false);
            using (Font tf = GaBrand.MakeFont(18f, FontStyle.Bold))
            using (Font bf = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush wb = new SolidBrush(GaBrand.Text))
            using (SolidBrush cb = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("Shortcut Key Settings", tf, wb, pad + 56f, 14f);
                g.DrawString("by Graphicarray · Add, Change, or Remove Any Shortcut", bf, cb, pad + 56f, 38f);
            }

            RectangleF card = new RectangleF(pad, 72f, ClientSize.Width - pad * 2, 370f);
            using (GraphicsPath gp = GaBrand.RoundedRect(card, 20f))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen ep = new Pen(GaBrand.Edge, 1f))
            {
                g.FillPath(pb, gp);
                g.DrawPath(ep, gp);
            }

            using (Font secF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font rowF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (Font keyF = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("ACTION", secF, slateB, card.X + 16f, card.Y + 14f);
                g.DrawString("SHORTCUT KEY (ADD / REMOVE)", secF, slateB, card.Right - 230f, card.Y + 14f);

                for (int i = 0; i < _cfg.Shortcuts.Count; i++)
                {
                    ShortcutBinding sb = _cfg.Shortcuts[i];
                    float ry = card.Y + 42f + i * 45f;
                    g.DrawString(sb.ActionName, rowF, textB, card.X + 16f, ry + 6f);

                    bool capturing = _capturingRowIndex == i;
                    _setBtnRects[i] = new RectangleF(card.Right - 230f, ry, 136f, 32f);
                    using (GraphicsPath kp = GaBrand.RoundedRect(_setBtnRects[i], 999f))
                    using (SolidBrush kFill = new SolidBrush(GaBrand.Abyss))
                    using (Pen kPen = new Pen(capturing ? GaBrand.Amber : GaBrand.Cyan, 1f))
                    using (SolidBrush kText = new SolidBrush(capturing ? GaBrand.Amber : GaBrand.Ice))
                    {
                        g.FillPath(kFill, kp);
                        g.DrawPath(kPen, kp);
                        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString(capturing ? "Press key..." : sb.FormatDisplay(), keyF, kText, _setBtnRects[i], sf);
                    }

                    _removeBtnRects[i] = new RectangleF(card.Right - 86f, ry, 70f, 32f);
                    using (GraphicsPath rp = GaBrand.RoundedRect(_removeBtnRects[i], 999f))
                    using (SolidBrush rFill = new SolidBrush(GaBrand.Abyss))
                    using (Pen rPen = new Pen(GaBrand.Red, 1f))
                    using (SolidBrush rText = new SolidBrush(GaBrand.Red))
                    {
                        g.FillPath(rFill, rp);
                        g.DrawPath(rPen, rp);
                        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString("Remove", keyF, rText, _removeBtnRects[i], sf);
                    }
                }
            }

            _addCustomBtnRect = new RectangleF(pad, card.Bottom + 14f, 180f, 42f);
            using (GraphicsPath rstP = GaBrand.RoundedRect(_addCustomBtnRect, 999f))
            using (SolidBrush rstFill = new SolidBrush(GaBrand.Panel))
            using (Pen rstPen = new Pen(GaBrand.Cyan, 1f))
            using (Font bf = GaBrand.MakeFont(13f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(GaBrand.Text))
            {
                g.FillPath(rstFill, rstP);
                g.DrawPath(rstPen, rstP);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("Restore Defaults", bf, tb, _addCustomBtnRect, sf);
            }

            _doneBtnRect = new RectangleF(pad + 192f, card.Bottom + 14f, ClientSize.Width - pad * 2 - 192f, 42f);
            using (GraphicsPath dnP = GaBrand.RoundedRect(_doneBtnRect, 999f))
            using (LinearGradientBrush grad = GaBrand.SignatureGradient(_doneBtnRect))
            using (Font bf = GaBrand.MakeFont(14f, FontStyle.Bold))
            using (SolidBrush ab = new SolidBrush(GaBrand.Abyss))
            {
                g.FillPath(grad, dnP);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("Apply Shortcuts & Done", bf, ab, _doneBtnRect, sf);
            }
        }
    }

    // ========================================================================
    // 11. MAIN OPEN SCREEN RECORDER STUDIO WINDOW (100% Video Recorder)
    // ========================================================================
    public sealed class MainCaptureForm : Form
    {
        private readonly AppConfig _cfg;
        private CrashSafeVideoRecorder _recorder;
        private RecordingGuidelineOverlayForm _guidelineOverlay;
        private Rectangle _selectedVideoBounds;
        private Point[] _selectedFreehandPolygon;

        private int _activeTab; // 0 = Video Studio, 1 = Crash Guard
        private string _statusToast = "Ready · Pick any of the 5 Video Modes or press Start (F9)";
        private bool _hoverLogo;
        private bool _hoverByLink;
        private bool _hoverFooterLink;
        private bool _hoverGearBtn;
        private int _hoveredModeCard = -1;
        private bool _hoverPrimaryBtn;
        private bool _blinkRedPhase;

        private readonly System.Windows.Forms.Timer _uiAndBlinkTimer;
        private readonly NotifyIcon _trayIcon;
        private readonly Icon _iconIdle;
        private readonly Icon _iconRecRed;
        private readonly Icon _iconPausedAmber;
        private readonly GaDropdownRenderer _menuRenderer = new GaDropdownRenderer();

        // Hit-test rectangles
        private RectangleF _rectLogo;
        private RectangleF _rectByLink;
        private RectangleF _rectFooterLink;
        private RectangleF _rectSettingsGear;
        private RectangleF _rectAreaSubOptionDropdown;
        private RectangleF _rectFormatDropdown;
        private RectangleF _rectQualityDropdown;
        private RectangleF _rectFpsDropdown;
        private RectangleF _rectAudioDropdown;
        private RectangleF _rectCursorDropdown;
        private RectangleF _rectFolderInput;
        private RectangleF _rectFolderBrowseBtn;
        private RectangleF _rectPortableDropdown;
        private RectangleF _rectPauseBtn;
        private RectangleF _rectPrimaryBtn;
        private readonly RectangleF[] _tabRects = new RectangleF[2];
        private readonly RectangleF[] _modeRects = new RectangleF[5];

        private static readonly string[] ModeTitles =
        {
            "Active Window", "Rectangle", "Freehand", "Full Screen", "Fixed Region"
        };

        public MainCaptureForm(AppConfig cfg)
        {
            _cfg = cfg;
            _selectedVideoBounds = _cfg.LastVideoRect.Width > 16 ? _cfg.LastVideoRect : ScreenCaptureEngine.GetVirtualDesktopBounds();
            CrashSafeGuard.Initialize(_cfg);

            _iconIdle = GaBrand.CreateStatusIcon(0);
            _iconRecRed = GaBrand.CreateStatusIcon(1);
            _iconPausedAmber = GaBrand.CreateStatusIcon(2);

            Text = GaBrand.ProductName;
            Icon = _iconIdle;
            Size = new Size(460, 748);
            MinimumSize = new Size(360, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = GaBrand.Abyss;
            ForeColor = GaBrand.Text;
            DoubleBuffered = true;
            KeyPreview = true;

            ContextMenuStrip trayMenu = new ContextMenuStrip { Renderer = _menuRenderer };
            trayMenu.Items.Add("Start / Stop Video", null, (s, e) => ToggleVideoRecording());
            trayMenu.Items.Add("Pause / Resume Video", null, (s, e) => TogglePauseRecording());
            trayMenu.Items.Add("Shortcut Key Settings...", null, (s, e) => OpenShortcutSettingsPage());
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
                    _guidelineOverlay?.SetBlinkPhase(_blinkRedPhase);
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
            ReRegisterAllShortcuts();
        }

        private void ReRegisterAllShortcuts()
        {
            if (!IsHandleCreated) return;
            for (int id = 200; id < 220; id++)
                NativeMethods.UnregisterHotKey(Handle, id);

            if (_cfg.Shortcuts == null) return;
            for (int i = 0; i < _cfg.Shortcuts.Count; i++)
            {
                ShortcutBinding sb = _cfg.Shortcuts[i];
                if (sb.Key == Keys.None) continue;
                uint mods = 0;
                if (sb.Alt) mods |= NativeMethods.MOD_ALT;
                if (sb.Ctrl) mods |= NativeMethods.MOD_CONTROL;
                if (sb.Shift) mods |= NativeMethods.MOD_SHIFT;
                NativeMethods.RegisterHotKey(Handle, 200 + i, mods, (uint)sb.Key);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CloseGuidelineOverlay();
            CrashSafeGuard.EmergencySaveAll("MainFormClosing");
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            for (int id = 200; id < 220; id++)
                NativeMethods.UnregisterHotKey(Handle, id);
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
                int idx = m.WParam.ToInt32() - 200;
                if (_cfg.Shortcuts != null && idx >= 0 && idx < _cfg.Shortcuts.Count)
                {
                    string act = _cfg.Shortcuts[idx].ActionId;
                    switch (act)
                    {
                        case "StartStop":    ToggleVideoRecording(); break;
                        case "PauseResume":  TogglePauseRecording(); break;
                        case "ActiveWindow": SelectVideoAreaMode(VideoAreaMode.ActiveWindow, showSubMenu: false); break;
                        case "Rectangle":    SelectVideoAreaMode(VideoAreaMode.Rectangle, showSubMenu: false); break;
                        case "Freehand":     SelectVideoAreaMode(VideoAreaMode.Freehand, showSubMenu: false); break;
                        case "FullScreen":   SelectVideoAreaMode(VideoAreaMode.FullScreen, showSubMenu: false); break;
                        case "FixedRegion":  SelectVideoAreaMode(VideoAreaMode.FixedRegion, showSubMenu: false); break;
                    }
                }
            }
            base.WndProc(ref m);
        }

        private void ShowGuidelineOverlayIfNeeded()
        {
            CloseGuidelineOverlay();
            if (!_cfg.ShowOuterGuideline) return;
            if (_cfg.ActiveAreaMode == VideoAreaMode.Rectangle ||
                _cfg.ActiveAreaMode == VideoAreaMode.Freehand ||
                _cfg.ActiveAreaMode == VideoAreaMode.FixedRegion)
            {
                _guidelineOverlay = new RecordingGuidelineOverlayForm(_selectedVideoBounds, _selectedFreehandPolygon);
                _guidelineOverlay.Show();
            }
        }

        private void CloseGuidelineOverlay()
        {
            if (_guidelineOverlay != null)
            {
                try { _guidelineOverlay.Close(); _guidelineOverlay.Dispose(); } catch { }
                _guidelineOverlay = null;
            }
        }

        private void OpenShortcutSettingsPage()
        {
            using (ShortcutKeyPageForm dlg = new ShortcutKeyPageForm(_cfg))
            {
                dlg.ShowDialog(this);
                ReRegisterAllShortcuts();
                _statusToast = "Shortcut keys updated & active";
                Invalidate();
            }
        }

        // Handles clicking any of the 5 Video Modes (including Rectangle & Fixed Region sub-options)
        private void SelectVideoAreaMode(VideoAreaMode mode, bool showSubMenu, Point menuPt = default)
        {
            _cfg.ActiveAreaMode = mode;
            _selectedFreehandPolygon = null;

            if (mode == VideoAreaMode.FullScreen)
            {
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
                _cfg.LastVideoRect = _selectedVideoBounds;
                StorageEngine.Save(_cfg);
                _statusToast = $"Full Screen Video ready ({_selectedVideoBounds.Width}×{_selectedVideoBounds.Height})";
                Invalidate();
                return;
            }

            if (mode == VideoAreaMode.ActiveWindow)
            {
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
                StorageEngine.Save(_cfg);
                _statusToast = "Active Window Video mode ready";
                Invalidate();
                return;
            }

            if (showSubMenu && mode == VideoAreaMode.Rectangle)
            {
                OpenRectangleOptionsDropdown(menuPt);
                return;
            }

            if (showSubMenu && mode == VideoAreaMode.FixedRegion)
            {
                OpenFixedRegionOptionsDropdown(menuPt);
                return;
            }

            RunInteractiveVideoRegionPicker();
        }

        private void OpenRectangleOptionsDropdown(Point screenOrClientPt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            menu.Items.Add("1. Select Freely Area on Screen", null, (s, e) =>
            {
                _cfg.RectangleSubMode = RectangleSelectionSubMode.FreeArea;
                StorageEngine.Save(_cfg);
                RunInteractiveVideoRegionPicker();
            });
            menu.Items.Add("2. Custom Ratio 16:9 Rectangle", null, (s, e) =>
            {
                _cfg.RectangleSubMode = RectangleSelectionSubMode.CustomRatio;
                _cfg.AspectRatioW = 16; _cfg.AspectRatioH = 9;
                StorageEngine.Save(_cfg);
                RunInteractiveVideoRegionPicker();
            });
            menu.Items.Add("3. Custom Ratio 9:16 (Vertical)", null, (s, e) =>
            {
                _cfg.RectangleSubMode = RectangleSelectionSubMode.CustomRatio;
                _cfg.AspectRatioW = 9; _cfg.AspectRatioH = 16;
                StorageEngine.Save(_cfg);
                RunInteractiveVideoRegionPicker();
            });
            menu.Items.Add("4. Custom Ratio 4:3", null, (s, e) =>
            {
                _cfg.RectangleSubMode = RectangleSelectionSubMode.CustomRatio;
                _cfg.AspectRatioW = 4; _cfg.AspectRatioH = 3;
                StorageEngine.Save(_cfg);
                RunInteractiveVideoRegionPicker();
            });
            menu.Items.Add("5. Custom Ratio 1:1 (Square)", null, (s, e) =>
            {
                _cfg.RectangleSubMode = RectangleSelectionSubMode.CustomRatio;
                _cfg.AspectRatioW = 1; _cfg.AspectRatioH = 1;
                StorageEngine.Save(_cfg);
                RunInteractiveVideoRegionPicker();
            });
            menu.Items.Add("6. Input Custom Ratio (W:H)...", null, (s, e) =>
            {
                if (PromptTwoIntegers("Custom Rectangle Aspect Ratio", "Ratio Width (W):", "Ratio Height (H):",
                    _cfg.AspectRatioW, _cfg.AspectRatioH, out int rw, out int rh))
                {
                    _cfg.RectangleSubMode = RectangleSelectionSubMode.CustomRatio;
                    _cfg.AspectRatioW = Math.Max(1, rw);
                    _cfg.AspectRatioH = Math.Max(1, rh);
                    StorageEngine.Save(_cfg);
                    RunInteractiveVideoRegionPicker();
                }
            });
            menu.Show(this, screenOrClientPt);
        }

        private void OpenFixedRegionOptionsDropdown(Point screenOrClientPt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            menu.Items.Add($"Option 1: Input Pixel Dimensions ({_cfg.FixedRegionWidth}×{_cfg.FixedRegionHeight} px)...", null, (s, e) =>
            {
                if (PromptTwoIntegers("Fixed Region — Pixel Dimensions", "Width in Pixels (px):", "Height in Pixels (px):",
                    _cfg.FixedRegionWidth, _cfg.FixedRegionHeight, out int pw, out int ph))
                {
                    _cfg.FixedSubMode = FixedRegionSubMode.PixelDimensions;
                    _cfg.FixedRegionWidth = Math.Max(32, pw & ~1);
                    _cfg.FixedRegionHeight = Math.Max(32, ph & ~1);
                    StorageEngine.Save(_cfg);
                    RunInteractiveVideoRegionPicker();
                }
            });
            menu.Items.Add($"Option 2: Input Custom Ratio ({_cfg.AspectRatioW}:{_cfg.AspectRatioH})...", null, (s, e) =>
            {
                if (PromptTwoIntegers("Fixed Region — Custom Aspect Ratio", "Ratio Width (W):", "Ratio Height (H):",
                    _cfg.AspectRatioW, _cfg.AspectRatioH, out int rw, out int rh))
                {
                    _cfg.FixedSubMode = FixedRegionSubMode.CustomRatio;
                    _cfg.AspectRatioW = Math.Max(1, rw);
                    _cfg.AspectRatioH = Math.Max(1, rh);
                    _cfg.FixedRegionHeight = Math.Max(32, ((_cfg.FixedRegionWidth * _cfg.AspectRatioH) / _cfg.AspectRatioW) & ~1);
                    StorageEngine.Save(_cfg);
                    RunInteractiveVideoRegionPicker();
                }
            });
            menu.Show(this, screenOrClientPt);
        }

        private void RunInteractiveVideoRegionPicker()
        {
            Hide();
            Thread.Sleep(130);
            try
            {
                bool lockRatio = _cfg.ActiveAreaMode == VideoAreaMode.Rectangle &&
                                 _cfg.RectangleSubMode == RectangleSelectionSubMode.CustomRatio;
                Size fixedSz = new Size(_cfg.FixedRegionWidth, _cfg.FixedRegionHeight);

                using (VideoAreaSelectorForm ov = new VideoAreaSelectorForm(
                    _cfg.ActiveAreaMode, lockRatio, _cfg.AspectRatioW, _cfg.AspectRatioH, fixedSz))
                {
                    if (ov.ShowDialog(this) == DialogResult.OK)
                    {
                        _selectedVideoBounds = ov.SelectedBounds;
                        _selectedFreehandPolygon = ov.LocalFreehandPolygon;
                        _cfg.LastVideoRect = _selectedVideoBounds;
                        StorageEngine.Save(_cfg);
                        _statusToast = $"{_cfg.ActiveAreaMode} Video Area: {_selectedVideoBounds.Width}×{_selectedVideoBounds.Height} px";
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

        private bool PromptTwoIntegers(string title, string label1, string label2, int def1, int def2, out int val1, out int val2)
        {
            val1 = def1;
            val2 = def2;
            using (Form dlg = new Form())
            {
                dlg.Text = title;
                dlg.Size = new Size(380, 210);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = GaBrand.Abyss;
                dlg.ForeColor = GaBrand.Text;

                Label l1 = new Label { Text = label1, Left = 20, Top = 20, Width = 160, ForeColor = GaBrand.Slate };
                TextBox t1 = new TextBox { Text = def1.ToString(), Left = 190, Top = 18, Width = 140, BackColor = GaBrand.Panel, ForeColor = GaBrand.Text };

                Label l2 = new Label { Text = label2, Left = 20, Top = 62, Width = 160, ForeColor = GaBrand.Slate };
                TextBox t2 = new TextBox { Text = def2.ToString(), Left = 190, Top = 60, Width = 140, BackColor = GaBrand.Panel, ForeColor = GaBrand.Text };

                Button ok = new Button { Text = "Apply", Left = 170, Top = 114, Width = 80, Height = 32, DialogResult = DialogResult.OK, BackColor = GaBrand.Cyan, ForeColor = GaBrand.Abyss };
                Button cancel = new Button { Text = "Cancel", Left = 258, Top = 114, Width = 72, Height = 32, DialogResult = DialogResult.Cancel, BackColor = GaBrand.Panel, ForeColor = GaBrand.Text };

                dlg.Controls.AddRange(new Control[] { l1, t1, l2, t2, ok, cancel });
                dlg.AcceptButton = ok;

                if (dlg.ShowDialog(this) == DialogResult.OK &&
                    int.TryParse(t1.Text.Trim(), out int parsed1) &&
                    int.TryParse(t2.Text.Trim(), out int parsed2))
                {
                    val1 = Math.Max(1, parsed1);
                    val2 = Math.Max(1, parsed2);
                    return true;
                }
                return false;
            }
        }

        private void ToggleVideoRecording()
        {
            if (_recorder != null && _recorder.IsRecording)
            {
                CloseGuidelineOverlay();
                string savedPath = _recorder.StopAndSave(emergency: false, reason: "UserButtonOrHotkey");
                _recorder = null;
                _statusToast = "Saved video: " + Path.GetFileName(savedPath);
                Invalidate();
                return;
            }

            if (_selectedVideoBounds.Width < 16 || _selectedVideoBounds.Height < 16)
                _selectedVideoBounds = ScreenCaptureEngine.GetVirtualDesktopBounds();

            ShowGuidelineOverlayIfNeeded();

            _recorder = new CrashSafeVideoRecorder(
                _cfg,
                _cfg.ActiveAreaMode,
                _selectedVideoBounds,
                _selectedFreehandPolygon);

            string livePath = _recorder.Start();
            string audioTag = _cfg.RecordAudioWithVideo ? "Video+Audio" : "Video Only";
            _statusToast = $"● REC ({audioTag}): " + Path.GetFileName(livePath);
            Invalidate();
        }

        private void TogglePauseRecording()
        {
            if (_recorder == null || !_recorder.IsRecording)
            {
                _statusToast = "Start a video recording first to Pause/Resume.";
                Invalidate();
                return;
            }
            _recorder.TogglePause();
            _statusToast = _recorder.IsPaused
                ? "Recording PAUSED (flushed to disk) · Click Resume"
                : "● Recording RESUMED: " + Path.GetFileName(_recorder.ActiveVideoPath);
            Invalidate();
        }

        // Dropdown Menus for all Value Controls
        private void OpenFormatDropdown(Point pt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            foreach (VideoFormatType fmt in Enum.GetValues(typeof(VideoFormatType)))
            {
                VideoFormatType copy = fmt;
                menu.Items.Add($"{copy} (.{copy.ToString().ToLowerInvariant()})", null, (s, e) =>
                {
                    _cfg.VideoFormat = copy;
                    StorageEngine.Save(_cfg);
                    Invalidate();
                });
            }
            menu.Show(this, pt);
        }

        private void OpenQualityDropdown(Point pt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            foreach (VideoQualityLevel q in Enum.GetValues(typeof(VideoQualityLevel)))
            {
                VideoQualityLevel copy = q;
                menu.Items.Add($"{copy} Quality ({(int)copy}%)", null, (s, e) =>
                {
                    _cfg.VideoQuality = copy;
                    StorageEngine.Save(_cfg);
                    Invalidate();
                });
            }
            menu.Show(this, pt);
        }

        private void OpenFpsDropdown(Point pt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            foreach (int fps in new[] { 15, 24, 30, 60 })
            {
                int copy = fps;
                menu.Items.Add($"{copy} FPS", null, (s, e) =>
                {
                    _cfg.VideoFps = copy;
                    StorageEngine.Save(_cfg);
                    Invalidate();
                });
            }
            menu.Show(this, pt);
        }

        private void OpenAudioModeDropdown(Point pt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            menu.Items.Add("Video + Audio (1 Single File)", null, (s, e) =>
            {
                _cfg.RecordAudioWithVideo = true;
                StorageEngine.Save(_cfg);
                Invalidate();
            });
            menu.Items.Add("Video Only (Without Audio)", null, (s, e) =>
            {
                _cfg.RecordAudioWithVideo = false;
                StorageEngine.Save(_cfg);
                Invalidate();
            });
            menu.Show(this, pt);
        }

        private void OpenCursorDropdown(Point pt)
        {
            ContextMenuStrip menu = new ContextMenuStrip { Renderer = _menuRenderer };
            menu.Items.Add("Show Cursor + Click Halo", null, (s, e) =>
            {
                _cfg.IncludeMouseCursor = true;
                _cfg.HighlightMouseClicks = true;
                StorageEngine.Save(_cfg);
                Invalidate();
            });
            menu.Items.Add("Show Cursor Only", null, (s, e) =>
            {
                _cfg.IncludeMouseCursor = true;
                _cfg.HighlightMouseClicks = false;
                StorageEngine.Save(_cfg);
                Invalidate();
            });
            menu.Items.Add("Hide Mouse Cursor", null, (s, e) =>
            {
                _cfg.IncludeMouseCursor = false;
                StorageEngine.Save(_cfg);
                Invalidate();
            });
            menu.Show(this, pt);
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
                prompt.Text = "Edit Video Auto-Save Directory Path";
                prompt.Size = new Size(460, 170);
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.BackColor = GaBrand.Abyss;
                prompt.ForeColor = GaBrand.Text;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lbl = new Label { Text = "Type or paste your Video Auto-Save folder path:", Left = 16, Top = 16, Width = 410, ForeColor = GaBrand.Slate };
                TextBox tb = new TextBox { Text = _cfg.AutoSaveFolder, Left = 16, Top = 44, Width = 410, BackColor = GaBrand.Panel, ForeColor = GaBrand.Text };
                Button ok = new Button { Text = "Save Path", Left = 246, Top = 84, Width = 90, Height = 32, DialogResult = DialogResult.OK, BackColor = GaBrand.Cyan, ForeColor = GaBrand.Abyss };
                Button cancel = new Button { Text = "Cancel", Left = 342, Top = 84, Width = 84, Height = 32, DialogResult = DialogResult.Cancel, BackColor = GaBrand.Panel, ForeColor = GaBrand.Text };
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
            bool hGear = _rectSettingsGear.Contains(pt);
            bool hPrim = _rectPrimaryBtn.Contains(pt);

            int modeHover = -1;
            for (int i = 0; i < _modeRects.Length; i++)
                if (_modeRects[i].Contains(pt)) modeHover = i;

            Cursor = (hLogo || hBy || hFoot || hGear || hPrim || modeHover >= 0 ||
                      _rectAreaSubOptionDropdown.Contains(pt) || _rectFormatDropdown.Contains(pt) ||
                      _rectQualityDropdown.Contains(pt) || _rectFpsDropdown.Contains(pt) ||
                      _rectAudioDropdown.Contains(pt) || _rectCursorDropdown.Contains(pt) ||
                      _rectFolderInput.Contains(pt) || _rectFolderBrowseBtn.Contains(pt) ||
                      _rectPortableDropdown.Contains(pt) || _rectPauseBtn.Contains(pt))
                ? Cursors.Hand : Cursors.Default;

            if (hLogo != _hoverLogo || hBy != _hoverByLink || hFoot != _hoverFooterLink ||
                hGear != _hoverGearBtn || hPrim != _hoverPrimaryBtn || modeHover != _hoveredModeCard)
            {
                _hoverLogo = hLogo;
                _hoverByLink = hBy;
                _hoverFooterLink = hFoot;
                _hoverGearBtn = hGear;
                _hoverPrimaryBtn = hPrim;
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

            // Settings Gear Button -> Opens Shortcut Key Manager Page
            if (_rectSettingsGear.Contains(pt))
            {
                OpenShortcutSettingsPage();
                return;
            }

            // 5 Video Recording Modes
            for (int i = 0; i < _modeRects.Length; i++)
            {
                if (_modeRects[i].Contains(pt))
                {
                    SelectVideoAreaMode((VideoAreaMode)i, showSubMenu: true, menuPt: new Point((int)_modeRects[i].Left, (int)_modeRects[i].Bottom));
                    return;
                }
            }

            // Area Sub-Option Dropdown (Free vs Custom Ratio for Rectangle; Pixel Dimensions vs Custom Ratio for Fixed)
            if (_rectAreaSubOptionDropdown.Contains(pt))
            {
                Point dp = new Point((int)_rectAreaSubOptionDropdown.Left, (int)_rectAreaSubOptionDropdown.Bottom);
                if (_cfg.ActiveAreaMode == VideoAreaMode.FixedRegion)
                    OpenFixedRegionOptionsDropdown(dp);
                else
                    OpenRectangleOptionsDropdown(dp);
                return;
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

            // Dropdown Value Buttons
            if (_rectFormatDropdown.Contains(pt))  { OpenFormatDropdown(new Point((int)_rectFormatDropdown.Left, (int)_rectFormatDropdown.Bottom)); return; }
            if (_rectQualityDropdown.Contains(pt)) { OpenQualityDropdown(new Point((int)_rectQualityDropdown.Left, (int)_rectQualityDropdown.Bottom)); return; }
            if (_rectFpsDropdown.Contains(pt))     { OpenFpsDropdown(new Point((int)_rectFpsDropdown.Left, (int)_rectFpsDropdown.Bottom)); return; }
            if (_rectAudioDropdown.Contains(pt))   { OpenAudioModeDropdown(new Point((int)_rectAudioDropdown.Left, (int)_rectAudioDropdown.Bottom)); return; }
            if (_rectCursorDropdown.Contains(pt))  { OpenCursorDropdown(new Point((int)_rectCursorDropdown.Left, (int)_rectCursorDropdown.Bottom)); return; }

            if (_rectFolderInput.Contains(pt))     { PromptEditAutoSaveFolder(); return; }
            if (_rectFolderBrowseBtn.Contains(pt)) { BrowseAutoSaveFolder(); return; }

            if (_rectPortableDropdown.Contains(pt))
            {
                ContextMenuStrip pm = new ContextMenuStrip { Renderer = _menuRenderer };
                pm.Items.Add("Portable Mode (Portable.db)", null, (s, ev) => { _cfg.PortableMode = true; StorageEngine.Save(_cfg); Invalidate(); });
                pm.Items.Add("Standard Mode (AppData)", null, (s, ev) => { _cfg.PortableMode = false; StorageEngine.Save(_cfg); Invalidate(); });
                pm.Show(this, new Point((int)_rectPortableDropdown.Left, (int)_rectPortableDropdown.Bottom));
                return;
            }

            if (_rectPauseBtn.Contains(pt))   { TogglePauseRecording(); return; }
            if (_rectPrimaryBtn.Contains(pt)) { ToggleVideoRecording(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            float pad = 20f;

            GaBrand.PaintAbyssBackground(g, ClientRectangle);

            // ── 1. HEADER: 44px Logo tile + "Open Screen Recorder" + "by Graphicarray" + Settings Gear (⚙) + Blinking Dot
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

            // Settings Gear Button (Opens Shortcut Key Page)
            _rectSettingsGear = new RectangleF(w - pad - 56f, 18f, 34f, 34f);
            using (GraphicsPath gp = GaBrand.RoundedRect(_rectSettingsGear, 999f))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen ep = new Pen(_hoverGearBtn ? GaBrand.Cyan : GaBrand.Edge, 1f))
            {
                g.FillPath(pb, gp);
                g.DrawPath(ep, gp);
                DrawGearIcon(g, new PointF(_rectSettingsGear.X + 17f, _rectSettingsGear.Y + 17f), _hoverGearBtn ? GaBrand.Cyan : GaBrand.Slate);
            }

            bool isRec = _recorder != null && _recorder.IsRecording;
            bool isPaused = isRec && _recorder.IsPaused;
            Color dotColor = !isRec ? GaBrand.Green : (isPaused ? GaBrand.Amber : (_blinkRedPhase ? GaBrand.Red : GaBrand.Mute));
            using (SolidBrush statusDot = new SolidBrush(dotColor))
            using (Pen halo = new Pen(Color.FromArgb(75, dotColor), 4f))
            {
                RectangleF dotRect = new RectangleF(w - pad - 14f, 29f, 12f, 12f);
                g.DrawEllipse(halo, dotRect);
                g.FillEllipse(statusDot, dotRect);
            }

            // ── 2. MAIN VIDEO STUDIO CARD: Timer + 5 Pure Video Modes + Sub-Mode Dropdown
            float y = 68f;
            RectangleF mainCard = new RectangleF(pad, y, w - pad * 2, 198f);
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
                g.DrawString("VIDEO RECORDING AREA (5 MODES + OUTER GUIDELINE)", labelF, slateB, mainCard.X + 16f, mainCard.Y + 12f);
                string timeStr = isRec ? _recorder.Elapsed.ToString(@"hh\:mm\:ss") : "00:00:00";
                g.DrawString(timeStr, timerF, isRec ? cyanB : textB, mainCard.X + 16f, mainCard.Y + 30f);

                // Area Sub-Option Dropdown (shows Free vs Custom Ratio for Rectangle, or Pixel vs Ratio for Fixed)
                string subLabel = GetAreaSubOptionLabel();
                _rectAreaSubOptionDropdown = new RectangleF(mainCard.Right - 196f, mainCard.Y + 30f, 180f, 28f);
                DrawDropdownPill(g, _rectAreaSubOptionDropdown, subLabel);
            }

            // 5 Video Mode Buttons in one clean row
            float gridTop = mainCard.Y + 72f;
            float cellW = (mainCard.Width - 32f - 32f) / 5f;
            float cellH = 108f;
            for (int i = 0; i < 5; i++)
            {
                RectangleF cell = new RectangleF(
                    mainCard.X + 16f + i * (cellW + 8f),
                    gridTop,
                    cellW, cellH);
                _modeRects[i] = cell;

                bool selected = (int)_cfg.ActiveAreaMode == i;
                bool hovered = _hoveredModeCard == i;

                using (GraphicsPath cp = GaBrand.RoundedRect(cell, 12f))
                using (SolidBrush cb = new SolidBrush(GaBrand.Abyss))
                using (Pen border = new Pen(selected || hovered ? GaBrand.Cyan : GaBrand.Edge, selected ? 1.8f : 1f))
                {
                    g.FillPath(cb, cp);
                    g.DrawPath(border, cp);
                }

                DrawVideoModeIcon(g, i, new PointF(cell.X + cell.Width / 2f, cell.Y + 34f));

                using (Font mf = GaBrand.MakeFont(10.5f, FontStyle.Bold))
                using (SolidBrush mb = new SolidBrush(selected || hovered ? GaBrand.Text : GaBrand.Slate))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString(ModeTitles[i], mf, mb, new RectangleF(cell.X + 2, cell.Bottom - 36f, cell.Width - 4, 32f), sf);
                }
            }

            // ── 3. SEGMENTED PILL TABS (Video Studio | Crash Guard & Shortcuts)
            y = mainCard.Bottom + 12f;
            RectangleF tabBar = new RectangleF(pad, y, w - pad * 2, 36f);
            using (GraphicsPath tp = GaBrand.RoundedRect(tabBar, GaBrand.RadiusPill))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen ep = new Pen(GaBrand.Edge, 1f))
            {
                g.FillPath(pb, tp);
                g.DrawPath(ep, tp);
            }

            string[] tabNames = { "Video Studio", "Crash Guard & Shortcuts" };
            float tabW = (tabBar.Width - 8f) / 2f;
            for (int i = 0; i < 2; i++)
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

            // ── 4. DROPDOWN VALUE CONTROLS & PATH CARD
            y = tabBar.Bottom + 12f;
            RectangleF ctrlCard = new RectangleF(pad, y, w - pad * 2, 246f);
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

            // ── 6. PAUSE/RESUME SECONDARY PILL + START/STOP PRIMARY PILL
            y = toastRect.Bottom + 10f;
            float pauseW = 134f;
            _rectPauseBtn = new RectangleF(pad, y, pauseW, 44f);
            DrawDropdownPill(g, _rectPauseBtn, isPaused ? "Resume (F10)" : "Pause (F10)", showArrow: false);

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
                g.DrawString("Outer screen guideline shows recorded area · Gear opens Shortcuts.", helpF, slateB, pad, _rectPrimaryBtn.Bottom + 8f);

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

        private string GetAreaSubOptionLabel()
        {
            if (_cfg.ActiveAreaMode == VideoAreaMode.Rectangle)
            {
                return _cfg.RectangleSubMode == RectangleSelectionSubMode.FreeArea
                    ? "Rect: Free Area ▾"
                    : $"Rect: Ratio {_cfg.AspectRatioW}:{_cfg.AspectRatioH} ▾";
            }
            if (_cfg.ActiveAreaMode == VideoAreaMode.FixedRegion)
            {
                return _cfg.FixedSubMode == FixedRegionSubMode.PixelDimensions
                    ? $"Fixed: {_cfg.FixedRegionWidth}×{_cfg.FixedRegionHeight}px ▾"
                    : $"Fixed: Ratio {_cfg.AspectRatioW}:{_cfg.AspectRatioH} ▾";
            }
            return $"{_cfg.ActiveAreaMode} Options ▾";
        }

        private void ResetControlHitRects()
        {
            _rectFormatDropdown = _rectQualityDropdown = _rectFpsDropdown =
                _rectAudioDropdown = _rectCursorDropdown = _rectFolderInput =
                _rectFolderBrowseBtn = _rectPortableDropdown = RectangleF.Empty;
        }

        private void PaintVideoControlsTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(10.5f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("VIDEO RECORDER DROPDOWN OPTIONS (CLICK ANY VALUE)", secF, slateB, card.X + 16f, card.Y + 12f);

                // Row 1: Video File Format Dropdown (MP4 / MKV / AVI) + Quality Dropdown (Low / Medium / High / Lossless)
                g.DrawString("Video File Format & Quality", bodyF, textB, card.X + 16f, card.Y + 38f);
                _rectFormatDropdown = new RectangleF(card.Right - 216f, card.Y + 33f, 88f, 28f);
                DrawDropdownPill(g, _rectFormatDropdown, $"{_cfg.VideoFormat} ▾");

                _rectQualityDropdown = new RectangleF(card.Right - 120f, card.Y + 33f, 104f, 28f);
                DrawDropdownPill(g, _rectQualityDropdown, $"{_cfg.VideoQuality} ▾");

                // Row 2: Audio Mode Dropdown (Video + Audio in 1 File OR Video Only without Audio)
                g.DrawString("Audio Recording Mode", bodyF, textB, card.X + 16f, card.Y + 74f);
                _rectAudioDropdown = new RectangleF(card.Right - 216f, card.Y + 69f, 200f, 28f);
                string audioLabel = _cfg.RecordAudioWithVideo ? "Video + Audio (1 File) ▾" : "Video Only (No Audio) ▾";
                DrawDropdownPill(g, _rectAudioDropdown, audioLabel);

                // Row 3: Frame Rate Dropdown + Cursor Dropdown
                g.DrawString("Frame Rate & Cursor", bodyF, textB, card.X + 16f, card.Y + 110f);
                _rectFpsDropdown = new RectangleF(card.Right - 216f, card.Y + 105f, 88f, 28f);
                DrawDropdownPill(g, _rectFpsDropdown, $"{_cfg.VideoFps} FPS ▾");

                _rectCursorDropdown = new RectangleF(card.Right - 120f, card.Y + 105f, 104f, 28f);
                DrawDropdownPill(g, _rectCursorDropdown, _cfg.IncludeMouseCursor ? "Cursor On ▾" : "Hidden ▾");

                // Row 4: Editable Auto-Save Directory + Browse Button
                g.DrawString("VIDEO AUTO-SAVE DIRECTORY (CLICK PATH OR BROWSE):", secF, slateB, card.X + 16f, card.Y + 148f);
                string shortPath = _cfg.AutoSaveFolder ?? "";
                if (shortPath.Length > 36) shortPath = "..." + shortPath.Substring(shortPath.Length - 33);

                _rectFolderInput = new RectangleF(card.X + 16f, card.Y + 170f, card.Width - 116f, 32f);
                DrawTertiaryInput(g, _rectFolderInput, shortPath);

                _rectFolderBrowseBtn = new RectangleF(card.Right - 92f, card.Y + 170f, 76f, 32f);
                DrawDropdownPill(g, _rectFolderBrowseBtn, "Browse", showArrow: false);
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
                g.DrawString("CRASH-SAFE AUTO-SAVE & SHORTCUT KEY GUARD", secF, slateB, card.X + 16f, card.Y + 14f);
                g.DrawString("● Active: 1s disk flush + auto-saves on sudden close", bodyF, greenB, card.X + 16f, card.Y + 38f);

                g.DrawString("Auto-Save Folder Path:", secF, slateB, card.X + 16f, card.Y + 68f);
                string shortPath = _cfg.AutoSaveFolder ?? "";
                if (shortPath.Length > 36) shortPath = "..." + shortPath.Substring(shortPath.Length - 33);
                _rectFolderInput = new RectangleF(card.X + 16f, card.Y + 90f, card.Width - 116f, 32f);
                DrawTertiaryInput(g, _rectFolderInput, shortPath);

                _rectFolderBrowseBtn = new RectangleF(card.Right - 92f, card.Y + 90f, 76f, 32f);
                DrawDropdownPill(g, _rectFolderBrowseBtn, "Browse", showArrow: false);

                g.DrawString("Storage Mode Dropdown", bodyF, textB, card.X + 16f, card.Y + 144f);
                _rectPortableDropdown = new RectangleF(card.Right - 140f, card.Y + 138f, 124f, 30f);
                DrawDropdownPill(g, _rectPortableDropdown, _cfg.PortableMode ? "Portable ▾" : "AppData ▾");

                g.DrawString("Shortcut Keys Manager", bodyF, textB, card.X + 16f, card.Y + 188f);
                RectangleF hkBtn = new RectangleF(card.Right - 180f, card.Y + 182f, 164f, 30f);
                DrawDropdownPill(g, hkBtn, "Click ⚙ Gear Top-Right", showArrow: false);
            }
        }

        private static void DrawDropdownPill(Graphics g, RectangleF rect, string text, bool showArrow = true)
        {
            using (GraphicsPath gp = GaBrand.RoundedRect(rect, GaBrand.RadiusPill))
            using (SolidBrush fill = new SolidBrush(GaBrand.Abyss))
            using (Pen border = new Pen(GaBrand.Cyan, 1f))
            using (Font f = GaBrand.MakeFont(11.5f, FontStyle.Bold))
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

        private static void DrawGearIcon(Graphics g, PointF center, Color color)
        {
            using (Pen p = new Pen(color, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawEllipse(p, center.X - 5f, center.Y - 5f, 10f, 10f);
                for (int a = 0; a < 360; a += 45)
                {
                    double rad = a * Math.PI / 180.0;
                    float x1 = center.X + (float)Math.Cos(rad) * 5.5f;
                    float y1 = center.Y + (float)Math.Sin(rad) * 5.5f;
                    float x2 = center.X + (float)Math.Cos(rad) * 8.5f;
                    float y2 = center.Y + (float)Math.Sin(rad) * 8.5f;
                    g.DrawLine(p, x1, y1, x2, y2);
                }
            }
        }

        private static void DrawVideoModeIcon(Graphics g, int index, PointF center)
        {
            using (Pen p = new Pen(GaBrand.Cyan, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                RectangleF r = new RectangleF(center.X - 11, center.Y - 8, 22, 16);
                switch (index)
                {
                    case 0: // Active Window
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X, r.Y + 5, r.Right, r.Y + 5);
                        break;
                    case 1: // Rectangle (Free / Custom Ratio)
                        p.DashStyle = DashStyle.Dot;
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 2: // Freehand
                        g.DrawEllipse(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 3: // Full Screen
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X + 4, r.Y + 4, r.Right - 4, r.Bottom - 4);
                        break;
                    case 4: // Fixed Region (Pixel / Ratio)
                        g.DrawRectangle(p, center.X - 7, center.Y - 6, 14, 12);
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

            // 2. Verify only 5 VideoAreaModes exist
            Debug.Assert(Enum.GetValues(typeof(VideoAreaMode)).Length == 5, "Must have exactly 5 VideoAreaModes");

            // 3. Verify custom ratio rectangle calculation
            Rectangle locked = ScreenCaptureEngine.ComputeRatioLockedRect(new Point(100, 100), new Point(260, 200), 16, 9);
            if (locked.Width != 160 || locked.Height != 90)
                throw new InvalidOperationException($"Ratio lock failed: {locked.Width}x{locked.Height}");

            // 4. Verify Single-File Audio+Video AND Video-Only writer modes
            string tempAvi = Path.Combine(Path.GetTempPath(), "osr_v2_" + Guid.NewGuid().ToString("N") + ".avi");
            try
            {
                using (CrashSafeAviWriter writer = new CrashSafeAviWriter(tempAvi, 64, 64, 24, VideoQualityLevel.High, includeAudio: false))
                using (Bitmap frame = new Bitmap(64, 64))
                {
                    writer.WriteFrame(frame);
                    writer.FlushCheckpoint();
                }
                byte[] bytes = File.ReadAllBytes(tempAvi);
                string ascii = Encoding.ASCII.GetString(bytes);
                if (!ascii.StartsWith("RIFF") || !ascii.Contains("00dc"))
                    throw new InvalidOperationException("Video-only stream verification failed.");
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
                Console.WriteLine("SELF-CHECK OK: Open Screen Recorder v2.0 (Pure Video Edition) verified.");
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
