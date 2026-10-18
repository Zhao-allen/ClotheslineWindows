using System.ComponentModel;

namespace ClotheslineSetup;

/// <summary>
/// A flat button in the installer's palette. Owner-drawn because the stock
/// WinForms button is a 3D bevel that fights the flat Linen surface.
/// </summary>
internal sealed class FlatButton : Button
{
    // Colour mirrors Clothesline.UI.TrayPalette (Linen, single light theme).
    private static readonly Color BgWindow = Color.FromArgb(0xFF, 0xFF, 0xFF);
    private static readonly Color BgPage = Color.FromArgb(0xEF, 0xE8, 0xDC);
    private static readonly Color BgHover = Color.FromArgb(0xED, 0xE4, 0xD6);
    private static readonly Color FgPrimary = Color.FromArgb(0x2B, 0x25, 0x20);
    private static readonly Color FgDisabled = Color.FromArgb(0xB7, 0xAF, 0x9F);
    private static readonly Color Accent = Color.FromArgb(0xE9, 0xA2, 0x3B);
    private static readonly Color AccentInk = Color.FromArgb(0x2A, 0x1B, 0x0C);
    private static readonly Color AccentText = Color.FromArgb(0x9E, 0x5B, 0x0C);
    private static readonly Color AccentSoft = Color.FromArgb(0x33, 0xE9, 0xA2, 0x3B);
    private static readonly Color Stroke = Color.FromArgb(0xE2, 0xD7, 0xC7);

    private bool _hover;

    public FlatButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        // Match the painted default fill. OnPaint draws the rounded rect in
        // BgWindow over a BgWindow window; a tinted BackColor would fall
        // outside that path and show a fringe at the four rounded corners.
        BackColor = BgWindow;
        ForeColor = FgPrimary;
        Font = new Font("Segoe UI Variable Display", 9.5f);
        Cursor = Cursors.Hand;
        SetStyle(ControlStyles.AllPaintingInWmPaint
               | ControlStyles.UserPaint
               | ControlStyles.OptimizedDoubleBuffer, true);
    }

    /// <summary>The accent-filled variant, used for the one action that matters.</summary>
    [DefaultValue(false)]
    public bool Primary { get; set; }

    /// <summary>Language rows mark the active choice; defaults to the first.</summary>
    [DefaultValue(false)]
    public bool Selected { get; set; }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        int radius = 9;

        using var path = Rounded(r, radius);

        Color fill;
        if (!Enabled) fill = BgPage;
        else if (Primary) fill = Accent;
        else if (Selected) fill = AccentSoft;
        else if (_hover) fill = BgHover;
        else fill = BgWindow;

        using (var brush = new SolidBrush(fill)) g.FillPath(brush, path);

        // A 1px hairline: the primary button is a flat amber fill with no
        // border, the selected language row edges in accent, everything else
        // takes the hairline stroke.
        Color border = Primary && Enabled ? Color.Transparent
                     : Selected && Enabled ? Accent
                     : Stroke;
        using (var pen = new Pen(border))
            g.DrawPath(pen, path);

        var fg = !Enabled ? FgDisabled
                : Primary ? AccentInk
                : Selected && Enabled ? AccentText
                : FgPrimary;

        using (var brush = new SolidBrush(fg))
        using (var fmt = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap,
            Trimming = StringTrimming.EllipsisCharacter,
        })
        {
            g.DrawString(Text, Font, brush, r, fmt);
        }
    }

    internal static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

/// <summary>
/// A 6px progress bar. The stock one is chunky; this matches the installer's
/// hairline treatment and the design's fully-rounded (radius 3) bar.
/// </summary>
internal sealed class FlatProgressBar : Control
{
    // Colour mirrors Clothesline.UI.TrayPalette (Linen, single light theme).
    private static readonly Color BgField = Color.FromArgb(0xF1, 0xEA, 0xDE);
    private static readonly Color Accent = Color.FromArgb(0xE9, 0xA2, 0x3B);

    private int _value;

    public FlatProgressBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint
               | ControlStyles.UserPaint
               | ControlStyles.OptimizedDoubleBuffer, true);
        Height = 6;
        BackColor = BgField;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Value
    {
        get => _value;
        set
        {
            var next = Math.Max(0, Math.Min(100, value));
            if (next == _value) return;
            _value = next;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Both the track and the fill are fully rounded: a 6px bar with a 3px
        // radius is a pill.
        using (var trackPath = FlatButton.Rounded(ClientRectangle, 3))
        using (var track = new SolidBrush(BgField))
            g.FillPath(track, trackPath);

        if (_value <= 0) return;

        int w = ClientSize.Width * _value / 100;
        if (w <= 0) return;
        using (var fillPath = FlatButton.Rounded(new Rectangle(0, 0, w, ClientSize.Height), 3))
        using (var fill = new SolidBrush(Accent))
            g.FillPath(fill, fillPath);
    }
}
