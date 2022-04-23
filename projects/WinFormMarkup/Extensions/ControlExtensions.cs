using System.ComponentModel;

namespace WinFormMarkup.Extensions;

/// <summary>
///     Fluent Extensions for Controls
/// </summary>
public static class ControlExtensions
{
    /// <summary>
    ///     Sets the `Control.AccessibleDefaultActionDescription` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="defaultActionDescription"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AccessibleDefaultActionDescription<TControl>(
        this TControl control,
        string defaultActionDescription)
        where TControl : Control
    {
        control.AccessibleDefaultActionDescription = defaultActionDescription;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AccessibleDescription` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="accessibleDescription"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AccessibleDescription<TControl>(
        this TControl control,
        string accessibleDescription)
        where TControl : Control
    {
        control.AccessibleDescription = accessibleDescription;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AccessibleName` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="accessibleName"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AccessibleName<TControl>(
        this TControl control,
        string accessibleName)
        where TControl : Control
    {
        control.AccessibleName = accessibleName;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AccessibleRole` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="accessibleRole"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AccessibleRole<TControl>(
        this TControl control,
        AccessibleRole accessibleRole)
        where TControl : Control
    {
        control.AccessibleRole = accessibleRole;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AllowDrop` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="allowDrop"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AllowDrop<TControl>(
        this TControl control,
        bool allowDrop)
        where TControl : Control
    {
        control.AllowDrop = allowDrop;
        return control;
    }

    public static TControl Also<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        action(control);
        return control;
    }

    /// <summary>
    ///     Sets the `Control.Anchor` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="anchors"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Anchor<TControl>(
        this TControl control,
        AnchorStyles anchors)
        where TControl : Control
    {
        control.Anchor = anchors;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AutoScrollOffset` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="offset"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AutoScrollOffset<TControl>(
        this TControl control,
        Point offset)
        where TControl : Control
    {
        control.AutoScrollOffset = offset;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.AutoSize` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="autoSize"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl AutoSize<TControl>(
        this TControl control,
        bool autoSize)
        where TControl : Control
    {
        control.AutoSize = autoSize;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.BackColor` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="color"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl BackColor<TControl>(
        this TControl control,
        Color color)
        where TControl : Control
    {
        control.BackColor = color;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.BackgroundImage` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="image"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl BackgroundImage<TControl>(
        this TControl control,
        Image image)
        where TControl : Control
    {
        control.BackgroundImage = image;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.BackgroundImageLayout` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="layout"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl BackgroundImageLayout<TControl>(
        this TControl control,
        ImageLayout layout)
        where TControl : Control
    {
        control.BackgroundImageLayout = layout;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.Bounds` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="bounds">Either 2 values (`width`, `height`), ot 2 values (`let`, top`, `width`, `height`).</param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TControl Bounds<TControl>(
        this TControl control,
        params int[] bounds)
        where TControl : Control
    {
        if (bounds == null) throw new ArgumentNullException(nameof(bounds));

        switch (bounds.Length)
        {
            case 2:
                control.Bounds = new Rectangle(control.Left, control.Top, bounds[0], bounds[1]);
                break;
            case 4:
                control.Bounds = new Rectangle(bounds[0], bounds[1], bounds[2], bounds[3]);
                break;
            default:
                throw new ArgumentException(
                    "Bounds must be either 2 (width, height), or 4 (left, top, width, height)");
        }

        return control;
    }

    /// <summary>
    ///     Sets the `Control.Capture` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="capture"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Capture<TControl>(
        this TControl control,
        bool capture)
        where TControl : Control
    {
        control.Capture = capture;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.CausesValidation` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="causesValidation"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl CausesValidation<TControl>(
        this TControl control,
        bool causesValidation)
        where TControl : Control
    {
        control.CausesValidation = causesValidation;
        return control;
    }


    /// <summary>
    ///     Sets the `Control.ClientSize` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="clientSize"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl ClientSize<TControl>(
        this TControl control,
        Size clientSize)
        where TControl : Control
    {
        control.ClientSize = clientSize;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.ContextMenuStrip` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="contextMenu"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl ContextMenuStrip<TControl>(
        this TControl control,
        ContextMenuStrip contextMenu)
        where TControl : Control
    {
        control.ContextMenuStrip = contextMenu;
        return control;
    }

    /// <summary>
    ///     Adds all of the `children`to the control, and return the current control.
    /// </summary>
    /// <remarks>Any child control added with a DockStyle.Fill will be brought to front.</remarks>
    /// <param name="control"></param>
    /// <param name="children">`params` collection of controls to add.</param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Controls<TControl>(
        this TControl control,
        params Control[] children)
        where TControl : Control
    {
        control.SuspendLayout();
        control.Controls.AddRange(children);
        foreach (var c in children
                     .Where(c => c.Dock == DockStyle.Fill))
            c.BringToFront();

        control.ResumeLayout();
        return control;
    }

    public static TControl FontSize<TControl>(
        this TControl control,
        int size)
        where TControl : Control
    {
        control.Font = new Font(control.Font.FontFamily, size);
        return control;
    }
    
    /// <summary>
    ///     Sets the `Control.Cursor` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="cursor"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Cursor<TControl>(
        this TControl control,
        Cursor cursor)
        where TControl : Control
    {
        control.Cursor = cursor;
        return control;
    }

    /// <summary>
    ///     Sets the `Control.Dock` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="dockPosition"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Dock<TControl>(
        this TControl control,
        DockStyle dockPosition)
        where TControl : Control
    {
        control.Dock = dockPosition;
        return control;
    }

    public static TControl Enabled<TControl>(
        this TControl control,
        bool enabled)
        where TControl : Control
    {
        control.Enabled = enabled;
        return control;
    }

    public static TControl Font<TControl>(
        this TControl control,
        Font font)
        where TControl : Control
    {
        control.Font = font;
        return control;
    }

    public static TControl ForeColor<TControl>(
        this TControl control,
        Color foreColor)
        where TControl : Control
    {
        control.ForeColor = foreColor;
        return control;
    }

    public static TControl Height<TControl>(
        this TControl control,
        int height)
        where TControl : Control
    {
        control.Height = height;
        return control;
    }

    public static TControl ImeMode<TControl>(
        this TControl control,
        ImeMode mode)
        where TControl : Control
    {
        control.ImeMode = mode;
        return control;
    }


    public static TControl IsAccessible<TControl>(
        this TControl control,
        bool isAccessible)
        where TControl : Control
    {
        control.IsAccessible = isAccessible;
        return control;
    }

    public static TControl Leave<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Leave += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl Left<TControl>(
        this TControl control,
        int left)
        where TControl : Control
    {
        control.Left = left;
        return control;
    }

    public static TControl Location<TControl>(
        this TControl control,
        Point location)
        where TControl : Control
    {
        control.Location = location;
        return control;
    }

    public static TControl Location<TControl>(
        this TControl control,
        int left,
        int top)
        where TControl : Control
    {
        control.Location = new Point(left, top);

        return control;
    }

    /// <summary>
    ///     Sets the `Control.Margin` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="margin">Variable number of parameters 1 (all), 2 (horizontal, vertical), or 4 (left, top, right, bottom)</param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TControl Margin<TControl>(
        this TControl control,
        params int[] margin)
        where TControl : Control
    {
        if (margin == null) throw new ArgumentNullException(nameof(margin));

        switch (margin.Length)
        {
            case 1:
                control.Margin = new Padding(margin[0]);
                break;
            case 2:
                control.Margin = new Padding(margin[0], margin[1], margin[0], margin[1]);
                break;
            case 4:
                control.Margin = new Padding(margin[0], margin[1], margin[2], margin[3]);
                break;
            default:
                throw new ArgumentException(
                    "Margin must be either 1 (all), 2 (horizontal, vertical), or 4 (left, top, right, bottom)");
        }

        return control;
    }


    public static TControl MaximumSize<TControl>(
        this TControl control,
        Size size)
        where TControl : Control
    {
        control.MaximumSize = size;
        return control;
    }

    public static TControl MaximumSize<TControl>(
        this TControl control,
        int width,
        int height)
        where TControl : Control
    {
        return control.MaximumSize(new Size(width, height));
    }

    public static TControl MinimumSize<TControl>(
        this TControl control,
        Size size)
        where TControl : Control
    {
        control.MinimumSize = size;
        return control;
    }

    public static TControl MinimumSize<TControl>(
        this TControl control,
        int width,
        int height)
        where TControl : Control
    {
        return control.MinimumSize(new Size(width, height));
    }

    public static TControl Name<TControl>(
        this TControl control,
        string name)
        where TControl : Control
    {
        control.Name = name;
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.AutoSizeChanged` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnAutoSizeChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.AutoSizeChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }


    /// <summary>
    ///     Hooks the `Control.BackColorChanged` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnBackColorChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.BackColorChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.BackgroundImageChanged` event to call the provided `action`, and returns a reference to the
    ///     control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnBackgroundImageChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.BackgroundImageChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.BackgroundImageLayoutChanged` event to call the provided `action`, and returns a reference to
    ///     the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnBackgroundImageLayoutChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.BackgroundImageLayoutChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.BindingContextChanged` event to call the provided `action`, and returns a reference to the
    ///     control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnBindingContextChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.BindingContextChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.CausesValidationChanged` event to call the provided `action`, and returns a reference to the
    ///     control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnCausesValidationChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.CausesValidationChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }


    /// <summary>
    ///     Hooks the `Control.ChangeUICues` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    // ReSharper disable once InconsistentNaming
    public static TControl OnChangeUICues<TControl>(
        this TControl control,
        Action<TControl, UICuesEventArgs> action)
        where TControl : Control
    {
        control.ChangeUICues += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.Click` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnClick<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Click += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.ClientSizeChanged` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnClientSizeChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.ClientSizeChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.ContextMenuStripChanged` event to call the provided `action`, and returns a reference to the
    ///     control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnContextMenuStripChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.ContextMenuStripChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.ControlAdded` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnControlAdded<TControl>(
        this TControl control,
        Action<TControl, ControlEventArgs> action)
        where TControl : Control
    {
        control.ControlAdded += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.ControlRemoved` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnControlRemoved<TControl>(
        this TControl control,
        Action<TControl, ControlEventArgs> action)
        where TControl : Control
    {
        control.ControlRemoved += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.CursorChanged` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnCursorChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.CursorChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.DockChanged` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnDockChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.DockChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Hooks the `Control.DoubleClick` event to call the provided `action`, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="action"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl OnDoubleClick<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.DoubleClick += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }


    public static TControl OnDragDrop<TControl>(
        this TControl control,
        Action<TControl, DragEventArgs> action)
        where TControl : Control
    {
        control.DragDrop += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnDragEnter<TControl>(
        this TControl control,
        Action<TControl, DragEventArgs> action)
        where TControl : Control
    {
        control.DragEnter += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnDragLeave<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.DragLeave += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnDragOver<TControl>(
        this TControl control,
        Action<TControl, DragEventArgs> action)
        where TControl : Control
    {
        control.DragOver += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnEnabledChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.EnabledChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }


    public static TControl OnEnter<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Enter += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnFontChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.FontChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnForeColorChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.ForeColorChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnGiveFeedback<TControl>(
        this TControl control,
        Action<TControl, GiveFeedbackEventArgs> action)
        where TControl : Control
    {
        control.GiveFeedback += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnGotFocus<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.GotFocus += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }


    public static TControl OnHandleCreated<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.HandleCreated += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnHandleDestroyed<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.HandleDestroyed += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnHelpRequested<TControl>(
        this TControl control,
        Action<TControl, HelpEventArgs> action)
        where TControl : Control
    {
        control.HelpRequested += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnImeModeChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.ImeModeChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnInvalidated<TControl>(
        this TControl control,
        Action<TControl, InvalidateEventArgs> action)
        where TControl : Control
    {
        control.Invalidated += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnKeyDown<TControl>(
        this TControl control,
        Action<TControl, KeyEventArgs> action)
        where TControl : Control
    {
        control.KeyDown += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnKeyPress<TControl>(
        this TControl control,
        Action<TControl, KeyPressEventArgs> action)
        where TControl : Control
    {
        control.KeyPress += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnKeyUp<TControl>(
        this TControl control,
        Action<TControl, KeyEventArgs> action)
        where TControl : Control
    {
        control.KeyUp += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnLayout<TControl>(
        this TControl control,
        Action<TControl, LayoutEventArgs> action)
        where TControl : Control
    {
        control.Layout += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnLocationChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.LocationChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnLostFocus<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.LostFocus += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMarginChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.MarginChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMouseCaptureChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.MouseCaptureChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMouseClick<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseClick += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnMouseDoubleClick<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseDoubleClick += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnMouseDown<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseDown += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnMouseEnter<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.MouseEnter += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMouseHover<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.MouseHover += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMouseLeave<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.MouseLeave += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnMouseMove<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseMove += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnMouseUp<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseUp += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnMouseWheel<TControl>(
        this TControl control,
        Action<TControl, MouseEventArgs> action)
        where TControl : Control
    {
        control.MouseWheel += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnMove<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Move += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnPaddingChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.PaddingChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnPaint<TControl>(
        this TControl control,
        Action<TControl, PaintEventArgs> action)
        where TControl : Control
    {
        control.Paint += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnParentChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.ParentChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnPreviewKeyDown<TControl>(
        this TControl control,
        Action<TControl, PreviewKeyDownEventArgs> action)
        where TControl : Control
    {
        control.PreviewKeyDown += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnQueryAccessibilityHelp<TControl>(
        this TControl control,
        Action<TControl, QueryAccessibilityHelpEventArgs> action)
        where TControl : Control
    {
        control.QueryAccessibilityHelp += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl OnQueryContinueDrag<TControl>(
        this TControl control,
        Action<TControl, QueryContinueDragEventArgs> action)
        where TControl : Control
    {
        control.QueryContinueDrag += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }


    public static TControl OnRegionChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.RegionChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnResize<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Resize += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl ONRightToLeftChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.RightToLeftChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnSizeChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.SizeChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnStyleChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.StyleChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnSystemColorsChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.SystemColorsChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnTabIndexChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.TabIndexChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnTabStopChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.TabStopChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnTextChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.TextChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnValidated<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.Validated += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    public static TControl OnVisibleChanged<TControl>(
        this TControl control,
        Action<TControl> action)
        where TControl : Control
    {
        control.VisibleChanged += (sender, _) => action.Invoke((sender as TControl)!);
        return control;
    }

    /// <summary>
    ///     Sets the `Control.Padding` property, and returns a reference to the control.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="padding">Variable number of parameters 1 (all), 2 (horizontal, vertical), or 4 (left, top, right, bottom)</param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TControl Padding<TControl>(
        this TControl control,
        params int[] padding)
        where TControl : Control
    {
        if (padding == null) throw new ArgumentNullException(nameof(padding));

        switch (padding.Length)
        {
            case 1:
                control.Padding = new Padding(padding[0]);
                break;
            case 2:
                control.Padding = new Padding(padding[0], padding[1], padding[0], padding[1]);
                break;
            case 4:
                control.Padding = new Padding(padding[0], padding[1], padding[2], padding[3]);
                break;
            default:
                throw new ArgumentException(
                    "Padding must be either 1 (all), 2 (horizontal, vertical), or 4 (left, top, right, bottom)");
        }

        return control;
    }


    public static TControl Parent<TControl>(
        this TControl control,
        Control parent)
        where TControl : Control
    {
        control.Parent = parent;
        return control;
    }

    public static TControl Region<TControl>(
        this TControl control,
        Region region)
        where TControl : Control
    {
        control.Region = region;
        return control;
    }

    public static TControl RightToLeft<TControl>(
        this TControl control,
        RightToLeft rtl)
        where TControl : Control
    {
        control.RightToLeft = rtl;
        return control;
    }

    public static TControl Site<TControl>(
        this TControl control,
        ISite site)
        where TControl : Control
    {
        control.Site = site;
        return control;
    }


    public static TControl Size<TControl>(
        this TControl control,
        Size size)
        where TControl : Control
    {
        control.Size = size;
        return control;
    }


    public static TControl Size<TControl>(
        this TControl control,
        int width,
        int height)
        where TControl : Control
    {
        return control.Size(new Size(width, height));
    }

    public static TControl TabIndex<TControl>(
        this TControl control,
        int tabIndex)
        where TControl : Control
    {
        control.TabIndex = tabIndex;
        return control;
    }

    public static TControl TabStop<TControl>(
        this TControl control,
        bool tabStop)
        where TControl : Control
    {
        control.TabStop = tabStop;
        return control;
    }


    public static TControl Tag<TControl>(
        this TControl control,
        object tag)
        where TControl : Control
    {
        control.Tag = tag;
        return control;
    }

    public static TControl Text<TControl>(
        this TControl control,
        string text)
        where TControl : Control
    {
        control.Text = text;
        return control;
    }

    /// <summary>
    ///     Sends the current control to the back of the parent controls collection.
    ///     If the control has already been assigned to a parent, then this method calls `SendToBack()` immediately and returns
    ///     a reference to the control.
    ///     Otherwise, if hooks the `Control.ParentChanged` event, and then invokes `SendToBack()` when the parent is assigned.
    /// </summary>
    /// <remarks>
    ///     **NOTE:** Thread-safe.  Automatically unhooks from event after `ParentChanged` has fired.
    /// </remarks>
    /// <param name="control"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl ToBack<TControl>(
        this TControl control)
        where TControl : Control
    {
        void DoToBack(object? o, EventArgs eventArgs)
        {
            control.SendToBack();
            control.ParentChanged -= DoToBack;
        }

        if (control.Parent == null)
            control.ParentChanged += DoToBack;
        else
            control.BeginInvoke((EventHandler)DoToBack);

        return control;
    }

    /// <summary>
    ///     Brings the current control to the front of the parent controls collection.
    ///     If the control has already been assigned to a parent, then this method calls `BringToFront()` immediately and
    ///     returns a reference to the control.
    ///     Otherwise, if hooks the `Control.ParentChanged` event, and then invokes `BringToFront()` when the parent is
    ///     assigned.
    /// </summary>
    /// <remarks>
    ///     **NOTE:** Thread-safe.  Automatically unhooks from event after `ParentChanged` has fired.
    /// </remarks>
    /// <param name="control"></param>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl ToFront<TControl>(
        this TControl control)
        where TControl : Control
    {
        void DoToFront(object? o, EventArgs eventArgs)
        {
            control.BringToFront();
            control.ParentChanged -= DoToFront;
        }

        if (control.Parent == null)
            control.ParentChanged += DoToFront;
        else
            control.BeginInvoke((EventHandler)DoToFront);

        return control;
    }


    public static TControl Top<TControl>(
        this TControl control,
        int top)
        where TControl : Control
    {
        control.Top = top;
        return control;
    }

    public static TControl UseWaitCursor<TControl>(
        this TControl control,
        bool useWait)
        where TControl : Control
    {
        control.UseWaitCursor = useWait;
        return control;
    }


    public static TControl Validating<TControl>(
        this TControl control,
        Action<TControl, CancelEventArgs> action)
        where TControl : Control
    {
        control.Validating += (sender, args) => action.Invoke((sender as TControl)!, args);
        return control;
    }

    public static TControl Visible<TControl>(
        this TControl control,
        bool visible)
        where TControl : Control
    {
        control.Visible = visible;
        return control;
    }

    public static TControl Width<TControl>(
        this TControl control,
        int width)
        where TControl : Control
    {
        control.Width = width;
        return control;
    }


    public static TControl WindowTarget<TControl>(
        this TControl control,
        IWindowTarget target)
        where TControl : Control
    {
        control.WindowTarget = target;
        return control;
    }
}