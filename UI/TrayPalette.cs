using GColor = System.Drawing.Color;
using WColor = System.Windows.Media.Color;
using WSolidBrush = System.Windows.Media.SolidColorBrush;

namespace Clothesline.UI;

/// <summary>
/// The Linen theme palette — the app's single source of truth for colour.
/// Single light theme only: warm linen surfaces, amber accent, terracotta
/// for destructive actions. Hue stays in the 30–40° warm band; the accent
/// never covers more than ~5% of any surface.
/// <para/>
/// Rules the values encode:
/// <list type="bullet">
/// <item>Amber as a fill always pairs with <see cref="AccentInk"/> (#2A1B0C);
/// amber as text is always <see cref="AccentText"/> (#9E5B0C), never the
/// fill colour — that is what keeps it readable on light surfaces.</item>
/// <item>Destructive actions use <see cref="Danger"/> (#A8443A) only.</item>
/// <item>Every stroke is 1px; separation comes from whitespace, not shadows.</item>
/// </list>
/// WPF surfaces do not restate these values: <c>CardMenuTheme</c> fills its
/// XAML <c>@SLOT@</c> placeholders through <see cref="Hex"/>, and GDI+
/// surfaces read the fields directly, so the tray menu, the card menu, the
/// annotation editor and the tray icons cannot drift apart. Add a colour
/// here, not in a UI file.
/// </summary>
internal static class TrayPalette
{
    // ------------------------------------------------------------------
    // Surfaces (light → lighter)
    // ------------------------------------------------------------------
    public static readonly GColor BgBase     = GColor.FromArgb(0xEF, 0xE8, 0xDC); // #EFE8DC page / window base
    public static readonly GColor BgSurface  = GColor.FromArgb(0xFF, 0xFF, 0xFF); // #FFFFFF cards, chrome
    public static readonly GColor BgPanel    = GColor.FromArgb(0xF7, 0xF1, 0xE7); // #F7F1E7 panels, menu body
    public static readonly GColor BgCard     = GColor.FromArgb(0xF7, 0xF1, 0xE7); // #F7F1E7 (alias of BgPanel)
    public static readonly GColor BgElevated = GColor.FromArgb(0xF1, 0xEA, 0xDE); // #F1EADE raised / toggle-off track
    public static readonly GColor BgHover    = GColor.FromArgb(0xED, 0xE4, 0xD6); // #EDE4D6 hover wash
    public static readonly GColor BgPressed  = GColor.FromArgb(0xE2, 0xD7, 0xC7); // #E2D7C7 pressed
    public static readonly GColor BgCanvas   = GColor.FromArgb(0xDE, 0xD4, 0xC4); // #DED4C4 annotation canvas

    // ------------------------------------------------------------------
    // Strokes — always 1px
    // ------------------------------------------------------------------
    public static readonly GColor Stroke       = GColor.FromArgb(0xE2, 0xD7, 0xC7); // #E2D7C7 hairline
    public static readonly GColor StrokeStrong = GColor.FromArgb(0xD9, 0xCD, 0xBB); // #D9CDBB emphatic hairline

    // ------------------------------------------------------------------
    // Text — body ≥ 4.5:1, large ≥ 3:1 on the surfaces above
    // ------------------------------------------------------------------
    public static readonly GColor FgPrimary   = GColor.FromArgb(0x2B, 0x25, 0x20); // #2B2520 primary
    public static readonly GColor FgSecondary = GColor.FromArgb(0x6A, 0x60, 0x55); // #6A6055 secondary
    public static readonly GColor FgMuted     = GColor.FromArgb(0x9C, 0x91, 0x82); // #9C9182 muted / captions
    public static readonly GColor Disabled    = GColor.FromArgb(0xB7, 0xAF, 0x9F); // #B7AF9F disabled

    // ------------------------------------------------------------------
    // Accent — amber. Fill pairs with AccentInk; text uses AccentText.
    // ------------------------------------------------------------------
    public static readonly GColor Accent     = GColor.FromArgb(0xE9, 0xA2, 0x3B); // #E9A23B fill
    public static readonly GColor AccentSoft = GColor.FromArgb(33, 0xE9, 0xA2, 0x3B);
    public static readonly GColor AccentRing = GColor.FromArgb(73, 0xE9, 0xA2, 0x3B);
    public static readonly GColor AccentInk  = GColor.FromArgb(0x2A, 0x1B, 0x0C); // #2A1B0C text on amber fill
    public static readonly GColor AccentText = GColor.FromArgb(0x9E, 0x5B, 0x0C); // #9E5B0C amber as text

    // ------------------------------------------------------------------
    // Brand — the clothesline motif: rope + wooden peg
    // ------------------------------------------------------------------
    public static readonly GColor Rope = GColor.FromArgb(0x9C, 0x7A, 0x46); // #9C7A46
    public static readonly GColor Peg  = GColor.FromArgb(0xC2, 0x9A, 0x64); // #C29A64

    // ------------------------------------------------------------------
    // Status
    // ------------------------------------------------------------------
    public static readonly GColor Danger       = GColor.FromArgb(0xA8, 0x44, 0x3A); // #A8443A terracotta
    public static readonly GColor DangerHover  = GColor.FromArgb(0xC4, 0x2B, 0x1C); // #C42B1C close-button hover
    public static readonly GColor DangerHoverInk = GColor.FromArgb(0xFF, 0xFF, 0xFF); // white glyph on DangerHover
    public static readonly GColor Success      = GColor.FromArgb(0x3E, 0x7A, 0x4E); // #3E7A4E
    public static readonly GColor MenuShadow   = GColor.FromArgb(0x5A, 0x46, 0x28); // #5A4628 card-menu shadow

    // ------------------------------------------------------------------
    // Compatibility — kept so existing paint code keeps compiling; new
    // code should prefer the named tokens above.
    // ------------------------------------------------------------------
    /// <summary>Toggle knob on an amber (on-state) track. Reads white.</summary>
    public static readonly GColor KnobDark = GColor.FromArgb(0xFF, 0xFF, 0xFF);

    // ------------------------------------------------------------------
    // Metrics — 4pt ladder, single light theme
    // ------------------------------------------------------------------
    public const int TrayRowHeight = 36;   // tray menu rows (was 34)
    public const int CardRowHeight = 34;   // card context-menu rows
    public const int MenuPadding = 16;     // menu outer padding
    public const int RowPadding = 11;      // row inner padding
    public const int SectionTopMargin = 11;
    public const int SubmenuLeftPad = 11;  // shared by both menu levels

    public const int RadiusRow = 7;        // rows
    public const int RadiusControl = 9;    // controls
    public const int RadiusFlyout = 12;    // flyouts
    public const int RadiusWindow = 16;    // windows
    public const int RadiusCapsule = 999;  // capsules / pills
    public const int RadiusCardMenuShell = 10;
    public const int RadiusHoverCapsule = 6;

    public const string FontFamily = "Segoe UI Variable";
    public const double FontDisplay = 21;   // hero titles
    public const double FontTitle = 14.5;   // menu / dialog titles
    public const double FontBody = 13.3;    // row text
    public const double FontCaption = 11.5; // values, hints
    public const double FontLabel = 11;     // section labels

    // ------------------------------------------------------------------
    // Bridges
    // ------------------------------------------------------------------
    /// <summary>#RRGGBB for XAML <c>@SLOT@</c> placeholders in theme builders.</summary>
    public static string Hex(GColor c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    /// <summary>Same colour as a WPF <see cref="WColor"/>.</summary>
    public static WColor W(GColor c) => WColor.FromArgb(c.A, c.R, c.G, c.B);

    /// <summary>Frozen WPF brush for a palette colour.</summary>
    public static WSolidBrush WBrush(GColor c)
    {
        var b = new WSolidBrush(W(c));
        b.Freeze();
        return b;
    }
}
