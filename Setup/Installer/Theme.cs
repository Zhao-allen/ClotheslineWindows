namespace ClotheslineSetup;

/// <summary>
/// The installer's mirror of the Linen palette (see TrayPalette in the
/// application project). Single light theme only: warm linen surfaces,
/// amber accent, terracotta for destructive actions.
///
/// The values are restated here — never referenced across the project
/// boundary — so the installer still builds as one self-contained exe with
/// no dependency on the app it installs. If a colour changes in TrayPalette,
/// copy the new value here; do not add colours in UI files.
///
/// Rules the values encode (same as the app):
/// <list type="bullet">
/// <item>Amber as a fill always pairs with <see cref="AccentInk"/> (#2A1B0C);
/// amber as text is always <see cref="AccentText"/> (#9E5B0C), never the
/// fill colour — that is what keeps it readable on light surfaces.</item>
/// <item>Destructive actions use <see cref="Danger"/> (#A8443A) only.</item>
/// <item>Every stroke is 1px; separation comes from whitespace, not shadows.</item>
/// </list>
/// </summary>
internal static class Theme
{
    // ------------------------------------------------------------------
    // Surfaces (light → lighter)
    // ------------------------------------------------------------------
    public static readonly Color BgBase     = Color.FromArgb(0xEF, 0xE8, 0xDC); // #EFE8DC window base
    public static readonly Color BgSurface  = Color.FromArgb(0xFF, 0xFF, 0xFF); // #FFFFFF cards, chrome
    public static readonly Color BgPanel    = Color.FromArgb(0xF7, 0xF1, 0xE7); // #F7F1E7 panels
    public static readonly Color BgElevated = Color.FromArgb(0xF1, 0xEA, 0xDE); // #F1EADE path field, tracks
    public static readonly Color BgHover    = Color.FromArgb(0xED, 0xE4, 0xD6); // #EDE4D6 hover wash
    public static readonly Color BgPressed  = Color.FromArgb(0xE2, 0xD7, 0xC7); // #E2D7C7 pressed

    // ------------------------------------------------------------------
    // Strokes — always 1px
    // ------------------------------------------------------------------
    public static readonly Color Stroke       = Color.FromArgb(0xE2, 0xD7, 0xC7); // #E2D7C7 hairline
    public static readonly Color StrokeStrong = Color.FromArgb(0xD9, 0xCD, 0xBB); // #D9CDBB emphatic hairline

    // ------------------------------------------------------------------
    // Text — body ≥ 4.5:1, large ≥ 3:1 on the surfaces above
    // ------------------------------------------------------------------
    public static readonly Color FgPrimary   = Color.FromArgb(0x2B, 0x25, 0x20); // #2B2520 primary
    public static readonly Color FgSecondary = Color.FromArgb(0x6A, 0x60, 0x55); // #6A6055 secondary
    public static readonly Color FgMuted     = Color.FromArgb(0x9C, 0x91, 0x82); // #9C9182 muted / captions
    public static readonly Color Disabled    = Color.FromArgb(0xB7, 0xAF, 0x9F); // #B7AF9F disabled

    // ------------------------------------------------------------------
    // Accent — amber. Fill pairs with AccentInk; text uses AccentText.
    // ------------------------------------------------------------------
    public static readonly Color Accent     = Color.FromArgb(0xE9, 0xA2, 0x3B); // #E9A23B fill
    public static readonly Color AccentSoft = Color.FromArgb(33, 0xE9, 0xA2, 0x3B);
    public static readonly Color AccentInk  = Color.FromArgb(0x2A, 0x1B, 0x0C); // #2A1B0C text on amber fill
    public static readonly Color AccentText = Color.FromArgb(0x9E, 0x5B, 0x0C); // #9E5B0C amber as text

    // ------------------------------------------------------------------
    // Status
    // ------------------------------------------------------------------
    public static readonly Color Danger  = Color.FromArgb(0xA8, 0x44, 0x3A); // #A8443A terracotta
    public static readonly Color Success = Color.FromArgb(0x3E, 0x7A, 0x4E); // #3E7A4E

    // ------------------------------------------------------------------
    // Metrics — 4pt ladder, single light theme
    // ------------------------------------------------------------------
    public const int RadiusButton = 6;   // buttons
    public const int RadiusBar = 3;      // progress bar
    public const int RadiusBox = 4;      // checkbox box

    public const string FontFamily = "Segoe UI Variable";
}
