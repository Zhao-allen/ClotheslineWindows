using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace ClotheslineSetup;

/// <summary>
/// A Linen button. Owner-drawn because the stock WinForms button is a
/// light-themed 3D bevel that fights the warm window.
///
/// Variants, in painting order: disabled → primary (solid amber, the one
/// action that matters) → selected (amber, the active language capsule) →
/// ghost (transparent hairline, the Cancel-style secondary) → pressed →
/// hover → resting card.
/// </summary>
internal sealed class FlatButton : Button
{
    private bool _hover;
    private bool _pressed;
    private bool _primary;
    private bool _selected;
    private bool _ghost;
    private bool _capsule;

    public FlatButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Theme.BgSurface;
        ForeColor = Theme.FgPrimary;
        Font = new Font(Theme.FontFamily, 9.5f);
        Cursor = Cursors.Hand;
        SetStyle(ControlStyles.AllPaintingInWmPaint
               | ControlStyles.UserPaint
               | ControlStyles.OptimizedDoubleBuffer, true);
    }

    /// <summary>The accent-filled variant, used for the one action that matters.</summary>
    [DefaultValue(false)]
    public bool Primary
    {
        get => _primary;
        set { if (_primary != value) { _primary = value; Invalidate(); } }
    }

    /// <summary>Language capsules mark the active choice; defaults to the first.</summary>
    [DefaultValue(false)]
    public bool Selected
    {
        get => _selected;
        set { if (_selected != value) { _selected = value; Invalidate(); } }
    }

    /// <summary>Transparent hairline variant for secondary actions like Cancel.</summary>
    [DefaultValue(false)]
    public bool Ghost
    {
        get => _ghost;
        set { if (_ghost != value) { _ghost = value; Invalidate(); } }
    }

    /// <summary>Pill shape (radius = half height), used for the language row.</summary>
    [DefaultValue(false)]
    public bool Capsule
    {
        get => _capsule;
        set { if (_capsule != value) { _capsule = value; Invalidate(); } }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _pressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _pressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        int radius = Capsule ? r.Height / 2 : Theme.RadiusButton;

        using var path = Rounded(r, radius);

        // Amber as a fill always pairs with AccentInk; amber as text is
        // always AccentText. That pairing is what keeps the amber readable.
        Color fill, line, fg;
        if (!Enabled)
        {
            fill = Theme.BgElevated; line = Theme.Stroke; fg = Theme.Disabled;
        }
        else if (Primary || Selected)
        {
            fill = Theme.Accent; line = Theme.AccentText; fg = Theme.AccentInk;
        }
        else if (Ghost)
        {
            // Transparent: show the window through, ringed by a hairline.
            fill = _pressed ? Theme.BgPressed
                 : _hover ? Theme.BgHover
                 : Parent?.BackColor ?? Theme.BgBase;
            line = Theme.Stroke;
            fg = _hover || _pressed ? Theme.FgPrimary : Theme.FgSecondary;
        }
        else if (_pressed)
        {
            fill = Theme.BgPressed; line = Theme.Stroke; fg = Theme.FgPrimary;
        }
        else if (_hover)
        {
            fill = Theme.BgHover; line = Theme.Stroke; fg = Theme.FgPrimary;
        }
        else
        {
            fill = Theme.BgSurface; line = Theme.Stroke; fg = Theme.FgPrimary;
        }

        using (var brush = new SolidBrush(fill)) g.FillPath(brush, path);

        // A 1px hairline everywhere; the filled amber one gets the deep-amber
        // edge so it does not wash out against light surfaces.
        using (var pen = new Pen(line)) g.DrawPath(pen, path);

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

    private static GraphicsPath Rounded(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

/// <summary>
/// A 6px rounded progress bar: warm elevated track, amber fill, 3px radius.
/// The stock one is chunky and light-themed; this matches the installer's
/// hairline treatment.
/// </summary>
internal sealed class FlatProgressBar : Control
{
    private int _value;

    public FlatProgressBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint
               | ControlStyles.UserPaint
               | ControlStyles.OptimizedDoubleBuffer, true);
        Height = 6;
        BackColor = Theme.BgBase;
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
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var trackPath = Rounded(ClientRectangle, Theme.RadiusBar);
        using (var track = new SolidBrush(Theme.BgElevated)) g.FillPath(track, trackPath);

        if (_value <= 0) return;

        int w = ClientSize.Width * _value / 100;
        if (w <= 0) return;

        // Clip to the rounded track so the fill's square end never pokes out
        // of the capsule, even at 1%.
        g.SetClip(trackPath);
        using (var fill = new SolidBrush(Theme.Accent))
            g.FillRectangle(fill, 0, 0, w, ClientSize.Height);
        g.ResetClip();
    }

    private static GraphicsPath Rounded(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

/// <summary>
/// A Linen checkbox: 1px hairline box, amber fill with a dark-ink check when
/// on. The stock WinForms box is light-themed and cannot be tinted, so the
/// whole row is owner-drawn. Sizes itself to its text.
/// </summary>
internal sealed class AmberCheckBox : CheckBox
{
    private const int BoxSize = 16;
    private const int BoxTextGap = 8;

    public AmberCheckBox()
    {
        FlatStyle = FlatStyle.Flat;
        AutoSize = false;
        ForeColor = Theme.FgPrimary;
        Font = new Font(Theme.FontFamily, 9.5f);
        Cursor = Cursors.Hand;
        SetStyle(ControlStyles.AllPaintingInWmPaint
               | ControlStyles.UserPaint
               | ControlStyles.OptimizedDoubleBuffer, true);
        FitToText();
    }

    protected override void OnTextChanged(EventArgs e)
    {
        FitToText();
        base.OnTextChanged(e);
    }

    protected override void OnFontChanged(EventArgs e)
    {
        FitToText();
        base.OnFontChanged(e);
    }

    protected override void OnCheckedChanged(EventArgs e)
    {
        Invalidate();
        base.OnCheckedChanged(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    private void FitToText()
    {
        var ts = TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue),
            TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        // 1px left inset + box + gap + text + a little air on the right.
        Width = 1 + BoxSize + BoxTextGap + ts.Width + 4;
        Height = Math.Max(22, ts.Height + 6);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.BgBase);

        var box = new Rectangle(1, (Height - BoxSize) / 2, BoxSize, BoxSize);
        using var path = Rounded(box, Theme.RadiusBox);

        Color fill = !Enabled ? Theme.BgElevated
                   : Checked ? Theme.Accent
                   : Theme.BgSurface;
        using (var brush = new SolidBrush(fill)) g.FillPath(brush, path);
        using (var pen = new Pen(Checked && Enabled ? Theme.AccentText : Theme.Stroke))
            g.DrawPath(pen, path);

        if (Checked)
        {
            // Dark ink on amber; muted when disabled. Hand-tuned for a 16px box.
            using var pen = new Pen(!Enabled ? Theme.Disabled : Theme.AccentInk, 2f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round,
            };
            g.DrawLines(pen, new[]
            {
                new PointF(box.X + 3.5f, box.Y + 8.5f),
                new PointF(box.X + 7f, box.Y + 12f),
                new PointF(box.X + 12.5f, box.Y + 5f),
            });
        }

        var textColor = !Enabled ? Theme.Disabled : ForeColor;
        TextRenderer.DrawText(g, Text, Font,
            new Rectangle(box.Right + BoxTextGap, 0, Width - box.Right - BoxTextGap, Height),
            textColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter
            | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix
            | TextFormatFlags.EndEllipsis);
    }

    private static GraphicsPath Rounded(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
