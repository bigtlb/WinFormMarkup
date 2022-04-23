namespace WinFormMarkup.Extensions;

/// <summary>
///     Fluent extensions for DateTimePicker controls
/// </summary>
public static class DateTimePickerExtensions
{
    /// <summary>
    ///     Sets the `DateTimePicker.CustomFormat` property, also sets the `DateTimePicker.Format` property to
    ///     `DateTimePickerFormat.Custom`, and then returns a reference to the control.
    /// </summary>
    /// <param name="dateTimePicker"></param>
    /// <param name="format"></param>
    /// <typeparam name="TDateTimePicker"></typeparam>
    /// <returns></returns>
    public static TDateTimePicker CustomFormat<TDateTimePicker>(
        this TDateTimePicker dateTimePicker,
        string format)
        where TDateTimePicker : DateTimePicker
    {
        dateTimePicker.Format = DateTimePickerFormat.Custom;
        dateTimePicker.CustomFormat = format;
        return dateTimePicker;
    }

    /// <summary>
    ///     Sets the `DateTimePicker.Format` property, and returns a reference to the control.
    /// </summary>
    /// <param name="dateTimePicker"></param>
    /// <param name="format"></param>
    /// <typeparam name="TDateTimePicker"></typeparam>
    /// <returns></returns>
    public static TDateTimePicker Format<TDateTimePicker>(
        this TDateTimePicker dateTimePicker,
        DateTimePickerFormat format)
        where TDateTimePicker : DateTimePicker
    {
        dateTimePicker.Format = format;
        return dateTimePicker;
    }
}