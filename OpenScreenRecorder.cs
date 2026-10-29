// ============================================================================
// Open Screen Recorder v1.0 — Offline Screen Capture & Crash-Safe Video Studio
// Brand & Owner: Graphicarray · Md Numan Ahmed (https://www.linkedin.com/in/graphicarray)
// Blueprint: FastStone Capture v7.4 architecture + Crash-Safe Auto-Save Guard
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GraphicarrayCapture
{
    // ========================================================================
    // 1. GRAPHICARRAY BRAND DESIGN TOKENS & VECTOR LOGO RENDERER
    // ========================================================================
    public static class GaBrand
    {
        public const string BrandName = "Graphicarray";
        public const string OwnerName = "Md Numan Ahmed";
        public const string FooterCredit = "Graphicarray · Numan Ahmed";
        public const string VersionText = "v1.0";
        public const string LinkedInUrl = "https://www.linkedin.com/in/graphicarray";

        // Strict Graphicarray palette tokens (only these colors are used)
        public static readonly Color Abyss = ColorTranslator.FromHtml("#001E36"); // --ga-abyss
        public static readonly Color Panel = ColorTranslator.FromHtml("#062847"); // --ga-panel
        public static readonly Color Edge  = ColorTranslator.FromHtml("#0F4A7A"); // --ga-edge
        public static readonly Color Cyan  = ColorTranslator.FromHtml("#00CDFF"); // --ga-cyan
        public static readonly Color Azure = ColorTranslator.FromHtml("#0083FF"); // --ga-azure
        public static readonly Color Ice   = ColorTranslator.FromHtml("#DDF6FF"); // --ga-ice
        public static readonly Color Text  = ColorTranslator.FromHtml("#FFFFFF"); // --ga-text
        public static readonly Color Slate = ColorTranslator.FromHtml("#8FA7BF"); // --ga-slate
        public static readonly Color Mute  = ColorTranslator.FromHtml("#5F7C99"); // --ga-mute
        public static readonly Color Amber = ColorTranslator.FromHtml("#F5A94A"); // --ga-amber
        public static readonly Color Green = ColorTranslator.FromHtml("#3DD68C"); // --ga-green
        public static readonly Color Red   = ColorTranslator.FromHtml("#FF5D6C"); // --ga-red
        public static readonly Color TopGlow = ColorTranslator.FromHtml("#06406E");

        public const int RadiusCard = 20;
        public const int RadiusInput = 12;
        public const int RadiusPill = 999;

        // Official brand/logo.svg content (exact 100x100 viewBox from brand guidelines)
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
                Process.Start(new ProcessStartInfo
                {
                    FileName = LinkedInUrl,
                    UseShellExecute = true
                });
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

        // Renders the exact 3-wing Graphicarray "A" vector mark on an Abyss rounded tile
        public static void DrawLogoTile(Graphics g, RectangleF tileRect, bool hovered, bool focused)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float radius = tileRect.Width * 0.25f; // 25% of width (12px on 44px tile)

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
                // Wing 1: bottom chevron polygon
                PointF[] poly1 = new PointF[]
                {
                    new PointF(49.78f, 61.93f),
                    new PointF(39.27f, 63.71f),
                    new PointF(33.89f, 73.04f),
                    new PointF(49.78f, 70.35f),
                    new PointF(66.11f, 73.15f),
                    new PointF(60.72f, 63.81f)
                };
                g.FillPolygon(wingGrad1, poly1);

                // Wing 2: upper-left & apex wing
                PointF[] poly2 = new PointF[]
                {
                    new PointF(39.27f, 46.88f),
                    new PointF(44.65f, 37.55f),
                    new PointF(47.64f, 32.36f),
                    new PointF(49.95f, 31.00f),
                    new PointF(52.27f, 32.36f),
                    new PointF(55.31f, 37.63f),
                    new PointF(66.09f, 39.48f),
                    new PointF(59.55f, 28.14f),
                    new PointF(50.00f, 22.60f),
                    new PointF(40.35f, 28.14f),
                    new PointF(33.89f, 39.17f),
                    new PointF(24.30f, 55.78f),
                    new PointF(26.15f, 69.41f),
                    new PointF(28.51f, 65.32f),
                    new PointF(33.89f, 56.00f)
                };
                g.FillPolygon(wingGrad2, poly2);

                // Wing 3: middle-right wing
                PointF[] poly3 = new PointF[]
                {
                    new PointF(75.70f, 56.10f),
                    new PointF(71.50f, 48.83f),
                    new PointF(60.72f, 46.98f),
                    new PointF(49.99f, 45.14f),
                    new PointF(44.66f, 54.38f),
                    new PointF(49.79f, 53.51f),
                    new PointF(55.34f, 54.46f),
                    new PointF(66.12f, 56.31f),
                    new PointF(71.51f, 65.65f),
                    new PointF(73.86f, 69.72f)
                };
                g.FillPolygon(wingGrad3, poly3);
            }

            g.Restore(state);
        }
    }

    // ========================================================================
    // 2. WIN32 LOW-LEVEL PRIMITIVES (GDI32, USER32, KERNEL32, WINMM)
    // ========================================================================
    public static class NativeMethods
    {
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int SB_PAGEDOWN = 3;
        public const int SB_LINEDOWN = 1;
        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;
        public const int SRCCOPY = 0x00CC0020;
        public const int CAPTUREBLT = 0x40000000;
        public const int CURSOR_SHOWING = 0x00000001;
        public const int DI_NORMAL = 0x0003;
        public const uint CF_BITMAP = 2;

        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;

        public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate bool ConsoleCtrlDelegate(int sig);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X; public int Y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

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

        [DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
        [DllImport("gdi32.dll")] public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")] public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")] public static extern IntPtr CreatePolygonRgn(POINT[] lppt, int cPoints, int fnPolyFillMode);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)] public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll")] public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        // WinMM waveIn for offline microphone audio capture
        [DllImport("winmm.dll")] public static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);
    }

    // ========================================================================
    // 3. PERSISTENCE & CONFIG ENGINE (fsc.db, TBSettings.db, Portable.db)
    // ========================================================================
    public enum CaptureMode
    {
        ActiveWindow,
        WindowObject,
        Rectangle,
        Freehand,
        FullScreen,
        ScrollingWindow,
        FixedRegion,
        VideoRecorder
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
        public CaptureMode ActiveCaptureMode { get; set; } = CaptureMode.VideoRecorder;
        public OutputTarget ActiveOutputTarget { get; set; } = OutputTarget.ToEditor;
        public string AutoSaveFolder { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "OpenScreenRecorder", "AutoSave");
        public string FileNameTemplate { get; set; } = "GA_$Y-$M-$D_$H-$N-$S_#";
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
        public Rectangle LastCaptureRect { get; set; } = new Rectangle(100, 100, 800, 600);
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
                    File.WriteAllText(PortableFlagPath, "PORTABLE=1\nBRAND=Graphicarray\n", Encoding.UTF8);
                else if (!cfg.PortableMode && File.Exists(PortableFlagPath))
                    File.Delete(PortableFlagPath);

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
                    Owner = GaBrand.OwnerName,
                    Profile = GaBrand.LinkedInUrl,
                    UpdatedUtc = DateTime.UtcNow
                }, opts), Encoding.UTF8);

                if (!cfg.PortableMode)
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                    {
                        key?.SetValue("AutoSaveFolder", cfg.AutoSaveFolder ?? "");
                        key?.SetValue("Template", cfg.FileNameTemplate ?? "GA_$Y-$M-$D_$H-$N-$S_#");
                    }
                }
            }
            catch { }
        }

        // FastStone template token engine ($Y, $M, $D, $H, $N, $S, #)
        public static string ExpandTemplate(string template, DateTime dt, int sequence)
        {
            if (string.IsNullOrWhiteSpace(template))
                template = "GA_$Y-$M-$D_$H-$N-$S_#";

            return template
                .Replace("$Y", dt.ToString("yyyy"))
                .Replace("$M", dt.ToString("MM"))
                .Replace("$D", dt.ToString("dd"))
                .Replace("$H", dt.ToString("HH"))
                .Replace("$N", dt.ToString("mm"))
                .Replace("$S", dt.ToString("ss"))
                .Replace("#", sequence.ToString("D3"));
        }

        public static string BuildAutoSaveFilePath(AppConfig cfg, string ext, string prefixOverride = null)
        {
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
    // 4. CRASH-SAFE AUTO-SAVE GUARD & LIVE DISK VIDEO STREAM WRITER
    // ========================================================================
    // Writes a valid MJPEG RIFF/AVI file directly into the user's selected folder,
    // patching frame counts and flushing OS file buffers every 1 second so that
    // unexpected app closure, crash, or power loss leaves a playable video file.
    // shortcut: uses MJPEG-in-AVI container for zero-dependency crash-safe streaming; upgrade to MediaFoundation H.264 Fragmented MP4 if hardware compression is needed.
    public sealed class CrashSafeAviWriter : IDisposable
    {
        private FileStream _fs;
        private BinaryWriter _bw;
        private readonly int _width;
        private readonly int _height;
        private readonly int _fps;
        private int _frameCount;
        private long _riffSizePos;
        private long _avihTotalFramesPos;
        private long _strhLengthPos;
        private long _moviSizePos;
        private long _moviDataStart;
        private readonly List<Tuple<uint, uint>> _indexEntries = new List<Tuple<uint, uint>>();
        private readonly object _sync = new object();
        private bool _finalized;

        public string FilePath { get; }
        public int FrameCount => _frameCount;

        public CrashSafeAviWriter(string filePath, int width, int height, int fps)
        {
            FilePath = filePath;
            _width = Math.Max(16, width);
            _height = Math.Max(16, height);
            _fps = Math.Clamp(fps, 1, 60);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
            _fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            _bw = new BinaryWriter(_fs, Encoding.ASCII, leaveOpen: true);
            WriteInitialHeader();
            FlushCheckpoint();
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
            _bw.Write((uint)0x10); // AVIF_HASINDEX
            _avihTotalFramesPos = _fs.Position;
            _bw.Write((uint)0); // dwTotalFrames
            _bw.Write((uint)0);
            _bw.Write((uint)1); // 1 stream
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write((uint)_width);
            _bw.Write((uint)_height);
            _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0); _bw.Write((uint)0);

            // strl LIST
            WriteFourCC("LIST");
            long strlSizePos = _fs.Position;
            _bw.Write((uint)0);
            long strlStart = _fs.Position;
            WriteFourCC("strl");

            // strh (56 bytes)
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
            _strhLengthPos = _fs.Position;
            _bw.Write((uint)0); // dwLength
            _bw.Write((uint)(_width * _height * 3));
            _bw.Write((uint)10000);
            _bw.Write((uint)0);
            _bw.Write((short)0);
            _bw.Write((short)0);
            _bw.Write((short)_width);
            _bw.Write((short)_height);

            // strf (BITMAPINFOHEADER 40 bytes)
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

            long strlEnd = _fs.Position;
            _fs.Position = strlSizePos;
            _bw.Write((uint)(strlEnd - strlStart));

            long hdrlEnd = strlEnd;
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
                frame.Save(ms, ImageFormat.Jpeg);
                WriteJpegBytes(ms.ToArray());
            }
        }

        public void WriteJpegBytes(byte[] jpegBytes)
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
                if ((len & 1) == 1) _bw.Write((byte)0); // word align
                _indexEntries.Add(Tuple.Create((uint)chunkOffset, len));
                _frameCount++;
            }
        }

        // Updates header sizes in-place and flushes to physical storage
        public void FlushCheckpoint()
        {
            lock (_sync)
            {
                if (_finalized || _fs == null) return;
                long endPos = _fs.Position;
                uint frames = (uint)Math.Max(1, _frameCount);

                _fs.Position = _avihTotalFramesPos;
                _bw.Write(frames);

                _fs.Position = _strhLengthPos;
                _bw.Write(frames);

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

                    // Write idx1 chunk for instant seeking in players
                    WriteFourCC("idx1");
                    _bw.Write((uint)(_indexEntries.Count * 16));
                    foreach (var entry in _indexEntries)
                    {
                        WriteFourCC("00dc");
                        _bw.Write((uint)0x10); // AVIIF_KEYFRAME
                        _bw.Write(entry.Item1);
                        _bw.Write(entry.Item2);
                    }

                    long fileEnd = _fs.Position;
                    uint frames = (uint)Math.Max(1, _frameCount);

                    _fs.Position = _avihTotalFramesPos;
                    _bw.Write(frames);

                    _fs.Position = _strhLengthPos;
                    _bw.Write(frames);

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
                        string path = StorageEngine.BuildAutoSaveFilePath(_config, ".png", "GA_RecoveredEdit_$Y-$M-$D_$H-$N-$S_#");
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

    // ========================================================================
    // 5. SCREEN VIDEO RECORDER ENGINE (FSRecorder + Crash-Safe Auto-Save)
    // ========================================================================
    public sealed class CrashSafeVideoRecorder : IDisposable
    {
        private readonly AppConfig _cfg;
        private readonly Rectangle _captureBounds;
        private CrashSafeAviWriter _writer;
        private Thread _workerThread;
        private volatile bool _running;
        private volatile bool _paused;
        private DateTime _startUtc;
        private DateTime _lastFlushUtc;

        public bool IsRecording => _running;
        public bool IsPaused => _paused;
        public string ActiveVideoPath => _writer?.FilePath;
        public string ActiveAudioPath { get; private set; }
        public int RecordedFrames => _writer?.FrameCount ?? 0;
        public TimeSpan Elapsed => _running ? DateTime.UtcNow - _startUtc : TimeSpan.Zero;

        public event Action<int, TimeSpan, string> OnProgress;

        public CrashSafeVideoRecorder(AppConfig cfg, Rectangle captureBounds)
        {
            _cfg = cfg;
            _captureBounds = captureBounds.Width > 16 && captureBounds.Height > 16
                ? captureBounds
                : ScreenCaptureEngine.GetVirtualDesktopBounds();
        }

        public string Start()
        {
            if (_running) return ActiveVideoPath;

            // Force even dimensions for video encoders
            int w = _captureBounds.Width & ~1;
            int h = _captureBounds.Height & ~1;
            string videoPath = StorageEngine.BuildAutoSaveFilePath(_cfg, ".avi", "GA_Video_$Y-$M-$D_$H-$N-$S_#");

            _writer = new CrashSafeAviWriter(videoPath, w, h, _cfg.VideoFps);
            _running = true;
            _paused = false;
            _startUtc = DateTime.UtcNow;
            _lastFlushUtc = DateTime.UtcNow;

            if (_cfg.RecordAudio)
            {
                try
                {
                    ActiveAudioPath = Path.ChangeExtension(videoPath, ".wav");
                    NativeMethods.mciSendString("open new Type waveaudio Alias ga_rec", null, 0, IntPtr.Zero);
                    NativeMethods.mciSendString("record ga_rec", null, 0, IntPtr.Zero);
                }
                catch { }
            }

            CrashSafeGuard.RegisterActiveRecorder(this);

            _workerThread = new Thread(CaptureLoop)
            {
                IsBackground = false, // Foreground thread guarantees flush before process exit
                Name = "Graphicarray_CrashSafeRecorder"
            };
            _workerThread.Start();

            return videoPath;
        }

        private void CaptureLoop()
        {
            int intervalMs = Math.Max(15, 1000 / Math.Clamp(_cfg.VideoFps, 1, 60));
            Stopwatch sw = new Stopwatch();

            while (_running)
            {
                sw.Restart();
                if (!_paused)
                {
                    try
                    {
                        using (Bitmap frame = ScreenCaptureEngine.CaptureRegion(
                            _captureBounds, _cfg.IncludeMouseCursor, _cfg.HighlightMouseClicks))
                        {
                            _writer?.WriteFrame(frame);
                        }

                        if ((DateTime.UtcNow - _lastFlushUtc).TotalSeconds >= Math.Max(1, _cfg.CrashFlushIntervalSeconds))
                        {
                            _writer?.FlushCheckpoint();
                            _lastFlushUtc = DateTime.UtcNow;
                        }

                        OnProgress?.Invoke(RecordedFrames, Elapsed, ActiveVideoPath);
                    }
                    catch { }
                }

                int sleep = intervalMs - (int)sw.ElapsedMilliseconds;
                if (sleep > 1) Thread.Sleep(sleep);
            }
        }

        public void TogglePause() => _paused = !_paused;

        public string StopAndSave(bool emergency = false, string reason = "UserStop")
        {
            if (!_running && _writer == null) return null;
            _running = false;
            try
            {
                if (_workerThread != null && _workerThread.IsAlive && Thread.CurrentThread != _workerThread)
                    _workerThread.Join(1500);
            }
            catch { }

            string finalPath = _writer?.FilePath;
            try { _writer?.FinalizeStream(); } catch { }
            _writer = null;

            if (_cfg.RecordAudio && !string.IsNullOrEmpty(ActiveAudioPath))
            {
                try
                {
                    NativeMethods.mciSendString($"save ga_rec \"{ActiveAudioPath}\"", null, 0, IntPtr.Zero);
                    NativeMethods.mciSendString("close ga_rec", null, 0, IntPtr.Zero);
                }
                catch { }
            }

            CrashSafeGuard.UnregisterActiveRecorder(this);
            StorageEngine.Save(_cfg);
            return finalPath;
        }

        public void Dispose() => StopAndSave(emergency: true, reason: "Dispose");
    }

    // ========================================================================
    // 6. CORE GDI SCREEN ACQUISITION & SCROLLING STITCH ENGINE
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

        // Direct desktop BitBlt via GetDC(NULL) -> CreateCompatibleDC -> CreateDIBSection -> BitBlt
        public static Bitmap CaptureRegion(Rectangle rect, bool includeCursor, bool highlightCursorHalo = false)
        {
            int w = Math.Max(1, rect.Width);
            int h = Math.Max(1, rect.Height);

            IntPtr hdcScreen = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr hdcMem = NativeMethods.CreateCompatibleDC(hdcScreen);

            NativeMethods.BITMAPINFO bmi = new NativeMethods.BITMAPINFO();
            bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = w;
            bmi.bmiHeader.biHeight = -h; // top-down DIB
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

        // BntActiveWindowClick
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

        // BntFreehandClick: polygon clipping mask
        public static Bitmap CaptureFreehandPolygon(Bitmap fullScreen, Point[] polygonPoints, out Rectangle bounds)
        {
            if (polygonPoints == null || polygonPoints.Length < 3)
            {
                bounds = new Rectangle(0, 0, fullScreen.Width, fullScreen.Height);
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
                new Rectangle(minX, minY, Math.Max(1, maxX - minX), Math.Max(1, maxY - minY)));

            Bitmap cropped = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(cropped))
            using (GraphicsPath path = new GraphicsPath())
            {
                g.Clear(GaBrand.Abyss);
                Point[] localPts = new Point[polygonPoints.Length];
                for (int i = 0; i < polygonPoints.Length; i++)
                    localPts[i] = new Point(polygonPoints[i].X - bounds.X, polygonPoints[i].Y - bounds.Y);
                path.AddPolygon(localPts);
                g.SetClip(path);
                g.DrawImage(fullScreen, new Rectangle(0, 0, bounds.Width, bounds.Height), bounds, GraphicsUnit.Pixel);
            }
            return cropped;
        }

        // BntWebClick: Scrolling Window Capture via WM_VSCROLL + scanline correlation stitching
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
                            break; // Reached bottom of scrollable container
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

        // Finds how many pixels frameB scrolled down relative to frameA using scanline correlation
        public static int ComputeVerticalScrollShift(Bitmap frameA, Bitmap frameB)
        {
            if (frameA == null || frameB == null || frameA.Width != frameB.Width || frameA.Height != frameB.Height)
                return 0;

            int h = frameA.Height;
            int w = frameA.Width;
            int stripHeight = Math.Min(24, h / 4);
            if (stripHeight < 4) return 0;

            int refY = h - stripHeight - 16;
            if (refY < 0) refY = 0;

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

        // Box-blur / pixelate filter for Editor Obscure tool ('O' key)
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
    // 7. OFFLINE OUTPUT DISPATCHER (OutputBtn Menu — 100% Offline)
    // ========================================================================
    public static class OutputDispatcher
    {
        public static string Dispatch(Bitmap captured, AppConfig cfg, Form owner)
        {
            if (captured == null) return null;

            // Always write an emergency recovery backup first so no capture is ever lost
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
                    hBitmap = IntPtr.Zero; // Owned by clipboard now
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
                    dynamic doc = app.Documents.Add();
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
                    dynamic slide = pres.Slides.Add(1, 12); // ppLayoutBlank
                    slide.Shapes.AddPicture(imagePath, 0, -1, 40, 40);
                }
            }
            catch { }
        }
    }

    // ========================================================================
    // 8. INTERACTIVE OVERLAY FORMS (Spy Window, Rectangle, Freehand, Fixed)
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
                Opacity = 0.01; // Allow WindowFromPoint to see underlying HWND
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
                            Math.Max(1, r.Right - r.Left), Math.Max(1, r.Bottom - r.Top));
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
                CapturedBitmap = ScreenCaptureEngine.CaptureFreehandPolygon(_frozenDesktop, _freehandPts.ToArray(), out Rectangle b);
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
            if (crop.Width < 4 || crop.Height < 4)
                crop = new Rectangle(0, 0, _frozenDesktop.Width, _frozenDesktop.Height);
            CapturedBitmap = _frozenDesktop.Clone(crop, PixelFormat.Format32bppArgb);
            SelectedBounds = new Rectangle(crop.X + Bounds.X, crop.Y + Bounds.Y, crop.Width, crop.Height);
            DialogResult = DialogResult.OK;
            Close();
        }

        private static Rectangle GetRubberBandRect(Point a, Point b) =>
            new Rectangle(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Max(4, Math.Abs(a.X - b.X)), Math.Max(4, Math.Abs(a.Y - b.Y)));

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

                // Floating Graphicarray pill toast showing exact coordinates
                string badge = $"{activeRect.Width} × {activeRect.Height} px";
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
    // 9. BUILT-IN EDITOR & ANNOTATION BOARD (TMainWin & TTextBoard)
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
            // Crash-Safe Guard: if editor closes without explicit save, auto-save to selected folder
            if (!_isSaved && _canvasBitmap != null)
            {
                string autoPath = StorageEngine.BuildAutoSaveFilePath(_cfg, _cfg.DefaultImageExtension, "GA_AutoSavedEdit_$Y-$M-$D_$H-$N-$S_#");
                OutputDispatcher.SaveImageByExtension(_canvasBitmap, autoPath);
                _isSaved = true;
            }
            CrashSafeGuard.SetUnsavedEditorBitmap(null);
            base.OnFormClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveCurrent(selectionOnly: false);
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.S)
            {
                SaveCurrent(selectionOnly: true);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                ResizeCanvas(0.75f);
                e.Handled = true;
            }
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
            // Check header/footer LinkedIn clicks or tool pill clicks
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
                        string s = (_stepCounter++).ToString();
                        g.DrawString(s, f, textB, badge.X + 8, badge.Y + 4);
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
            {
                _selectionRect = rect;
            }
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

            // Header bar with Official Graphicarray Logo + linked title
            GaBrand.DrawLogoTile(g, new RectangleF(24, 10, 44, 44), false, false);
            using (Font titleF = GaBrand.MakeFont(18f, FontStyle.Bold))
            using (Font byF = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("Open Screen Recorder Editor", titleF, textB, 78, 10);
                g.DrawString("by Graphicarray", byF, cyanB, 78, 32);
            }

            // Segmented tool bar pills
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

            // Image viewport card
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

            // Footer with LinkedIn credit & version chip
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
    // 10. MAIN GRAPHICARRAY CAPTURE & VIDEO STUDIO WINDOW (TMainPanel)
    // ========================================================================
    public sealed class MainCaptureForm : Form
    {
        private readonly AppConfig _cfg;
        private CrashSafeVideoRecorder _recorder;
        private int _activeTab; // 0 = Video Studio, 1 = Capture & Output, 2 = Crash Guard
        private string _statusToast = "Ready · Offline Crash-Safe Auto-Save Active";
        private bool _hoverLogo;
        private bool _hoverByLink;
        private bool _hoverFooterLink;
        private int _hoveredModeCard = -1;
        private bool _hoverPrimaryBtn;
        private System.Windows.Forms.Timer _uiTimer;

        // UI hit-test rectangles
        private RectangleF _rectLogo;
        private RectangleF _rectByLink;
        private RectangleF _rectFooterLink;
        private RectangleF _rectFolderBtn;
        private RectangleF _rectFpsCycle;
        private RectangleF _rectOutputCycle;
        private RectangleF _rectCursorToggle;
        private RectangleF _rectAudioToggle;
        private RectangleF _rectPortableToggle;
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
            CrashSafeGuard.Initialize(_cfg);

            Text = "Open Screen Recorder";
            Size = new Size(440, 690);
            MinimumSize = new Size(340, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = GaBrand.Abyss;
            ForeColor = GaBrand.Text;
            DoubleBuffered = true;
            KeyPreview = true;

            _uiTimer = new System.Windows.Forms.Timer { Interval = 250 };
            _uiTimer.Tick += (s, e) =>
            {
                if (_recorder != null && _recorder.IsRecording)
                    Invalidate();
            };
            _uiTimer.Start();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Register global hotkeys (F9 = Record/Stop Video, PrtSc = Capture, F11 = Repeat Last)
            NativeMethods.RegisterHotKey(Handle, 101, 0, (uint)Keys.F9);
            NativeMethods.RegisterHotKey(Handle, 102, 0, (uint)Keys.PrintScreen);
            NativeMethods.RegisterHotKey(Handle, 103, 0, (uint)Keys.F11);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // CRITICAL USER REQUIREMENT: When app unexpectedly or accidentally closes,
            // automatically finalize and save the recording/capture to the selected PC path!
            CrashSafeGuard.EmergencySaveAll("MainFormClosing");
            NativeMethods.UnregisterHotKey(Handle, 101);
            NativeMethods.UnregisterHotKey(Handle, 102);
            NativeMethods.UnregisterHotKey(Handle, 103);
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
                else if (id == 102) ExecuteScreenCapture(_cfg.ActiveCaptureMode);
                else if (id == 103) ExecuteRepeatLastCapture();
            }
            base.WndProc(ref m);
        }

        private void ToggleVideoRecording()
        {
            if (_recorder != null && _recorder.IsRecording)
            {
                string savedPath = _recorder.StopAndSave(emergency: false, reason: "UserButtonOrF9");
                _recorder = null;
                _statusToast = "Saved video: " + Path.GetFileName(savedPath);
                Invalidate();
                return;
            }

            Rectangle bounds = ScreenCaptureEngine.GetVirtualDesktopBounds();
            if (_cfg.ActiveCaptureMode == CaptureMode.Rectangle)
            {
                Hide();
                Thread.Sleep(120);
                using (RegionOverlayForm ov = new RegionOverlayForm(CaptureMode.Rectangle, Size.Empty))
                {
                    if (ov.ShowDialog(this) == DialogResult.OK)
                    {
                        bounds = ov.SelectedBounds;
                        _cfg.LastCaptureRect = bounds;
                    }
                    else
                    {
                        Show();
                        return;
                    }
                }
                Show();
            }

            _recorder = new CrashSafeVideoRecorder(_cfg, bounds);
            string livePath = _recorder.Start();
            _statusToast = "Recording live to: " + Path.GetFileName(livePath);
            Invalidate();
        }

        private void ExecuteScreenCapture(CaptureMode mode)
        {
            if (mode == CaptureMode.VideoRecorder)
            {
                ToggleVideoRecording();
                return;
            }

            if (_cfg.CaptureDelaySeconds > 0)
                Thread.Sleep(_cfg.CaptureDelaySeconds * 1000);

            Hide();
            Thread.Sleep(140);

            Bitmap captured = null;
            Rectangle rect = ScreenCaptureEngine.GetVirtualDesktopBounds();

            try
            {
                switch (mode)
                {
                    case CaptureMode.ActiveWindow:
                        captured = ScreenCaptureEngine.CaptureActiveWindow(_cfg.IncludeMouseCursor, out rect);
                        break;

                    case CaptureMode.FullScreen:
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
                    _statusToast = "Saved: " + Path.GetFileName(saved);
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

        private void ExecuteRepeatLastCapture()
        {
            Hide();
            Thread.Sleep(120);
            try
            {
                using (Bitmap bmp = ScreenCaptureEngine.CaptureRegion(_cfg.LastCaptureRect, _cfg.IncludeMouseCursor))
                {
                    string saved = OutputDispatcher.Dispatch(bmp, _cfg, this);
                    _statusToast = "Repeated capture: " + Path.GetFileName(saved);
                }
            }
            finally
            {
                Show();
                Invalidate();
            }
        }

        private void ChooseAutoSaveFolder()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Auto-Save & Crash Recovery Folder on your PC";
                fbd.SelectedPath = _cfg.AutoSaveFolder;
                if (fbd.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _cfg.AutoSaveFolder = fbd.SelectedPath;
                    StorageEngine.Save(_cfg);
                    _statusToast = "Auto-Save path updated";
                    Invalidate();
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

            int modeHover = -1;
            for (int i = 0; i < _modeRects.Length; i++)
                if (_modeRects[i].Contains(pt)) modeHover = i;

            Cursor = (hLogo || hBy || hFoot || hPrim || modeHover >= 0 ||
                      _rectFolderBtn.Contains(pt) || _rectFpsCycle.Contains(pt) ||
                      _rectOutputCycle.Contains(pt) || _rectCursorToggle.Contains(pt) ||
                      _rectAudioToggle.Contains(pt) || _rectPortableToggle.Contains(pt))
                ? Cursors.Hand : Cursors.Default;

            if (hLogo != _hoverLogo || hBy != _hoverByLink || hFoot != _hoverFooterLink ||
                hPrim != _hoverPrimaryBtn || modeHover != _hoveredModeCard)
            {
                _hoverLogo = hLogo;
                _hoverByLink = hBy;
                _hoverFooterLink = hFoot;
                _hoverPrimaryBtn = hPrim;
                _hoveredModeCard = modeHover;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            PointF pt = e.Location;

            // 1. LinkedIn Brand Links (Logo, "by Graphicarray", Footer "Graphicarray · Numan Ahmed")
            if (_rectLogo.Contains(pt) || _rectByLink.Contains(pt) || _rectFooterLink.Contains(pt))
            {
                GaBrand.OpenBrandProfile();
                return;
            }

            // 2. Capture Mode Grid Cards (8 FastStone buttons)
            for (int i = 0; i < _modeRects.Length; i++)
            {
                if (_modeRects[i].Contains(pt))
                {
                    if (i == 7) { ExecuteRepeatLastCapture(); return; }
                    _cfg.ActiveCaptureMode = (CaptureMode)i;
                    ExecuteScreenCapture(_cfg.ActiveCaptureMode);
                    return;
                }
            }

            // 3. Segmented Tabs
            for (int i = 0; i < _tabRects.Length; i++)
            {
                if (_tabRects[i].Contains(pt))
                {
                    _activeTab = i;
                    Invalidate();
                    return;
                }
            }

            // 4. Interactive Controls inside active tab card
            if (_rectFolderBtn.Contains(pt)) { ChooseAutoSaveFolder(); return; }
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

            // 5. Primary Bottom Pill Button
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

            // Background: radial blue light from top fading into Abyss (#001E36)
            GaBrand.PaintAbyssBackground(g, ClientRectangle);

            // ── 1. HEADER: 44px Logo tile (left, linked) + Product Title + "by Graphicarray" (linked)
            _rectLogo = new RectangleF(pad, 16f, 44f, 44f);
            GaBrand.DrawLogoTile(g, _rectLogo, _hoverLogo, false);

            using (Font titleFont = GaBrand.MakeFont(20f, FontStyle.Bold))
            using (Font byFont = GaBrand.MakeFont(12f, FontStyle.Bold))
            using (SolidBrush whiteBrush = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanBrush = new SolidBrush(_hoverByLink ? GaBrand.Ice : GaBrand.Cyan))
            {
                g.DrawString("Open Screen Recorder", titleFont, whiteBrush, pad + 56f, 15f);
                _rectByLink = new RectangleF(pad + 56f, 40f, 120f, 18f);
                g.DrawString("by Graphicarray", byFont, cyanBrush, _rectByLink.Location);
            }

            // Top-right status dot (Green when idle/protected, Amber/Red when recording)
            bool isRec = _recorder != null && _recorder.IsRecording;
            using (SolidBrush statusDot = new SolidBrush(isRec ? GaBrand.Red : GaBrand.Green))
            using (Pen halo = new Pen(Color.FromArgb(65, isRec ? GaBrand.Red : GaBrand.Cyan), 4f))
            {
                RectangleF dotRect = new RectangleF(w - pad - 14f, 30f, 12f, 12f);
                g.DrawEllipse(halo, dotRect);
                g.FillEllipse(statusDot, dotRect);
            }

            // ── 2. MAIN VISUAL CONTROL: Live Recorder Studio HUD + 8 FastStone Capture Mode Buttons
            float y = 74f;
            RectangleF mainCard = new RectangleF(pad, y, w - pad * 2, 232f);
            using (GraphicsPath cardPath = GaBrand.RoundedRect(mainCard, GaBrand.RadiusCard))
            using (SolidBrush panelBrush = new SolidBrush(GaBrand.Panel))
            using (Pen edgePen = new Pen(isRec ? GaBrand.Cyan : GaBrand.Edge, 1f))
            {
                g.FillPath(panelBrush, cardPath);
                g.DrawPath(edgePen, cardPath);
            }

            // Live Timer / Status Header inside Main Visual Card
            using (Font labelF = GaBrand.MakeFont(11f, FontStyle.Bold))
            using (Font timerF = GaBrand.MakeFont(22f, FontStyle.Bold))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush cyanB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("VIDEO & CAPTURE STUDIO", labelF, slateB, mainCard.X + 16f, mainCard.Y + 14f);
                string timeStr = isRec ? _recorder.Elapsed.ToString(@"hh\:mm\:ss") : "00:00:00";
                g.DrawString(timeStr, timerF, isRec ? cyanB : textB, mainCard.X + 16f, mainCard.Y + 32f);

                string fpsBadge = $"{_cfg.VideoFps} FPS · AUTO-SAVE ON";
                SizeF fsz = g.MeasureString(fpsBadge, labelF);
                g.DrawString(fpsBadge, labelF, cyanB, mainCard.Right - fsz.Width - 16f, mainCard.Y + 38f);
            }

            // 8 FastStone Capture Mode Pills (2 rows × 4 columns)
            float gridTop = mainCard.Y + 72f;
            float cellW = (mainCard.Width - 32f - 24f) / 4f;
            float cellH = 64f;
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
                using (Pen border = new Pen(selected || hovered ? GaBrand.Cyan : GaBrand.Edge, 1f))
                {
                    g.FillPath(cb, cp);
                    g.DrawPath(border, cp);
                }

                // 2px stroke outline icon in Cyan (24px grid)
                DrawCaptureIcon(g, i, new PointF(cell.X + cell.Width / 2f, cell.Y + 22f));

                using (Font mf = GaBrand.MakeFont(10.5f, FontStyle.Bold))
                using (SolidBrush mb = new SolidBrush(selected || hovered ? GaBrand.Text : GaBrand.Slate))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString(ModeTitles[i], mf, mb, new RectangleF(cell.X, cell.Bottom - 20f, cell.Width, 18f), sf);
                }
            }

            // ── 3. SEGMENTED PILL TABS (Video Studio | Output | Crash Guard)
            y = mainCard.Bottom + 14f;
            RectangleF tabBar = new RectangleF(pad, y, w - pad * 2, 38f);
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
            y = tabBar.Bottom + 14f;
            RectangleF ctrlCard = new RectangleF(pad, y, w - pad * 2, 166f);
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
            y = ctrlCard.Bottom + 12f;
            RectangleF toastRect = new RectangleF(pad, y, w - pad * 2, 28f);
            using (GraphicsPath toastPath = GaBrand.RoundedRect(toastRect, GaBrand.RadiusPill))
            using (SolidBrush pb = new SolidBrush(GaBrand.Panel))
            using (Pen cp = new Pen(GaBrand.Cyan, 1f))
            using (Font tf = GaBrand.MakeFont(11.5f, FontStyle.Regular))
            using (SolidBrush cb = new SolidBrush(GaBrand.Cyan))
            {
                g.FillPath(pb, toastPath);
                g.DrawPath(cp, toastPath);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(_statusToast, tf, cb, toastRect, sf);
            }

            // ── 6. SINGLE PRIMARY PILL BUTTON (Full width at bottom, 135deg gradient, Abyss bold text)
            y = toastRect.Bottom + 12f;
            _rectPrimaryBtn = new RectangleF(pad, y, w - pad * 2, 46f);
            if (_hoverPrimaryBtn)
            {
                using (GraphicsPath glow = GaBrand.RoundedRect(RectangleF.Inflate(_rectPrimaryBtn, 2, 2), GaBrand.RadiusPill))
                using (Pen gp = new Pen(Color.FromArgb(150, GaBrand.Cyan), 3f))
                    g.DrawPath(gp, glow);
            }
            using (GraphicsPath btnPath = GaBrand.RoundedRect(_rectPrimaryBtn, GaBrand.RadiusPill))
            using (LinearGradientBrush btnGrad = GaBrand.SignatureGradient(_rectPrimaryBtn))
            using (Font bf = GaBrand.MakeFont(15f, FontStyle.Bold))
            using (SolidBrush abyssText = new SolidBrush(GaBrand.Abyss))
            {
                g.FillPath(btnGrad, btnPath);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                string label = isRec ? "Stop & Save Recording (F9)" : "Start Video Recording (F9)";
                g.DrawString(label, bf, abyssText, _rectPrimaryBtn, sf);
            }

            // ── 7. HELPER TEXT & BRAND FOOTER (pinned near bottom)
            using (Font helpF = GaBrand.MakeFont(12f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush hoverB = new SolidBrush(GaBrand.Cyan))
            {
                g.DrawString("Select a capture mode above, or press F9 to record crash-safe video.", helpF, slateB, pad, _rectPrimaryBtn.Bottom + 10f);

                float fy = h - 32f;
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
            _rectFolderBtn = _rectFpsCycle = _rectOutputCycle =
                _rectCursorToggle = _rectAudioToggle = _rectPortableToggle = RectangleF.Empty;
        }

        private void PaintVideoControlsTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(11f, FontStyle.Bold))
            using (Font valF = GaBrand.MakeFont(14f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(13f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("VIDEO RECORDER SETTINGS", secF, slateB, card.X + 16f, card.Y + 14f);

                // Row 1: Frame Rate numeric pill
                g.DrawString("Frame Rate (FPS)", bodyF, textB, card.X + 16f, card.Y + 42f);
                _rectFpsCycle = new RectangleF(card.Right - 116f, card.Y + 36f, 100f, 30f);
                DrawSecondaryPill(g, _rectFpsCycle, $"{_cfg.VideoFps} FPS");

                // Row 2: Include Mouse Cursor & Click Halo
                g.DrawString("Mouse Cursor + Click Halo", bodyF, textB, card.X + 16f, card.Y + 82f);
                _rectCursorToggle = new RectangleF(card.Right - 116f, card.Y + 76f, 100f, 30f);
                DrawSecondaryPill(g, _rectCursorToggle, _cfg.IncludeMouseCursor ? "Enabled" : "Hidden");

                // Row 3: Offline Microphone Audio
                g.DrawString("Offline Audio Capture", bodyF, textB, card.X + 16f, card.Y + 122f);
                _rectAudioToggle = new RectangleF(card.Right - 116f, card.Y + 116f, 100f, 30f);
                DrawSecondaryPill(g, _rectAudioToggle, _cfg.RecordAudio ? "Recording" : "Muted");
            }
        }

        private void PaintOutputControlsTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(11f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(13f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            {
                g.DrawString("OFFLINE OUTPUT DESTINATION (NO CLOUD/FTP)", secF, slateB, card.X + 16f, card.Y + 14f);

                g.DrawString("Post-Capture Target", bodyF, textB, card.X + 16f, card.Y + 44f);
                _rectOutputCycle = new RectangleF(card.Right - 150f, card.Y + 38f, 134f, 30f);
                DrawSecondaryPill(g, _rectOutputCycle, _cfg.ActiveOutputTarget.ToString());

                g.DrawString("Filename Template", bodyF, textB, card.X + 16f, card.Y + 84f);
                RectangleF tplBox = new RectangleF(card.Right - 190f, card.Y + 78f, 174f, 30f);
                DrawTertiaryInput(g, tplBox, _cfg.FileNameTemplate);

                g.DrawString("Fixed / Scroll Pacing", bodyF, textB, card.X + 16f, card.Y + 124f);
                RectangleF fixBox = new RectangleF(card.Right - 190f, card.Y + 118f, 174f, 30f);
                DrawTertiaryInput(g, fixBox, $"{_cfg.FixedRegionWidth}×{_cfg.FixedRegionHeight} · {_cfg.AutoScrollIntervalMs}ms");
            }
        }

        private void PaintCrashGuardTab(Graphics g, RectangleF card)
        {
            using (Font secF = GaBrand.MakeFont(11f, FontStyle.Bold))
            using (Font bodyF = GaBrand.MakeFont(12.5f, FontStyle.Regular))
            using (SolidBrush slateB = new SolidBrush(GaBrand.Slate))
            using (SolidBrush textB = new SolidBrush(GaBrand.Text))
            using (SolidBrush greenB = new SolidBrush(GaBrand.Green))
            {
                g.DrawString("CRASH-SAFE AUTO-SAVE RECOVERY GUARD", secF, slateB, card.X + 16f, card.Y + 14f);
                g.DrawString("● Active: Auto-saves every 1s & on unexpected close", bodyF, greenB, card.X + 16f, card.Y + 36f);

                string shortPath = _cfg.AutoSaveFolder ?? "";
                if (shortPath.Length > 38) shortPath = "..." + shortPath.Substring(shortPath.Length - 35);
                RectangleF pathInput = new RectangleF(card.X + 16f, card.Y + 64f, card.Width - 122f, 32f);
                DrawTertiaryInput(g, pathInput, shortPath);

                _rectFolderBtn = new RectangleF(card.Right - 98f, card.Y + 64f, 82f, 32f);
                DrawSecondaryPill(g, _rectFolderBtn, "Browse");

                g.DrawString("100% Portable Mode (Portable.db)", bodyF, textB, card.X + 16f, card.Y + 116f);
                _rectPortableToggle = new RectangleF(card.Right - 116f, card.Y + 110f, 100f, 30f);
                DrawSecondaryPill(g, _rectPortableToggle, _cfg.PortableMode ? "Portable" : "Registry");
            }
        }

        private static void DrawSecondaryPill(Graphics g, RectangleF rect, string text)
        {
            using (GraphicsPath gp = GaBrand.RoundedRect(rect, GaBrand.RadiusPill))
            using (SolidBrush fill = new SolidBrush(GaBrand.Abyss))
            using (Pen border = new Pen(GaBrand.Cyan, 1f))
            using (Font f = GaBrand.MakeFont(12.5f, FontStyle.Bold))
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
                    case 0: // Active Window
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X, r.Y + 4, r.Right, r.Y + 4);
                        break;
                    case 1: // Window / Object Spy
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawEllipse(p, center.X - 3, center.Y - 3, 6, 6);
                        break;
                    case 2: // Rectangular Region
                        p.DashStyle = DashStyle.Dot;
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 3: // Freehand Lasso
                        g.DrawEllipse(p, r.X, r.Y, r.Width, r.Height);
                        break;
                    case 4: // Full Screen
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, r.X + 3, r.Y + 3, r.Right - 3, r.Bottom - 3);
                        break;
                    case 5: // Scrolling Window
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
                        g.DrawLine(p, center.X, r.Y + 3, center.X, r.Bottom - 3);
                        break;
                    case 6: // Fixed Region
                        g.DrawRectangle(p, center.X - 6, center.Y - 5, 12, 10);
                        break;
                    case 7: // Repeat Last
                        g.DrawArc(p, r, 30, 290);
                        break;
                }
            }
        }
    }

    // ========================================================================
    // 11. ASSERT-BASED SELF-CHECK SUITE (Ponytail Quality Gate)
    // ========================================================================
    public static class SelfTests
    {
        public static void RunAll()
        {
            // 1. Verify Graphicarray Brand hex colors match guidelines
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Abyss) == "#001E36", "Abyss mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Panel) == "#062847", "Panel mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Cyan)  == "#00CDFF", "Cyan mismatch");
            Debug.Assert(ColorTranslator.ToHtml(GaBrand.Azure) == "#0083FF", "Azure mismatch");
            Debug.Assert(GaBrand.LinkedInUrl == "https://www.linkedin.com/in/graphicarray", "LinkedIn URL mismatch");

            // 2. Verify FastStone filename token expansion ($Y, $M, $D, $H, $N, $S, #)
            DateTime sampleDt = new DateTime(2026, 10, 30, 14, 5, 9);
            string expanded = StorageEngine.ExpandTemplate("GA_$Y-$M-$D_$H-$N-$S_#", sampleDt, 7);
            if (expanded != "GA_2026-10-30_14-05-09_007")
                throw new InvalidOperationException("Template expansion check failed: " + expanded);

            // 3. Verify Crash-Safe AVI Writer flushes and finalizes valid RIFF header on sudden close
            string tempAvi = Path.Combine(Path.GetTempPath(), "ga_selfcheck_" + Guid.NewGuid().ToString("N") + ".avi");
            try
            {
                using (CrashSafeAviWriter writer = new CrashSafeAviWriter(tempAvi, 64, 64, 24))
                using (Bitmap frame = new Bitmap(64, 64))
                {
                    writer.WriteFrame(frame);
                    writer.FlushCheckpoint();
                }
                byte[] header = File.ReadAllBytes(tempAvi);
                if (header.Length < 64 || Encoding.ASCII.GetString(header, 0, 4) != "RIFF")
                    throw new InvalidOperationException("CrashSafeAviWriter RIFF header invalid.");
            }
            finally
            {
                if (File.Exists(tempAvi)) File.Delete(tempAvi);
            }
        }
    }

    // ========================================================================
    // 12. PROGRAM ENTRY POINT
    // ========================================================================
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            // Run self-check assertions automatically
            SelfTests.RunAll();
            if (args != null && Array.Exists(args, a => a == "--self-check"))
            {
                Console.WriteLine("SELF-CHECK OK: Open Screen Recorder verified.");
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