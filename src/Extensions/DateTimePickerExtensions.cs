using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class DateTimePickerExtensions
    {
        public static TDateTimePicker CustomFormat<TDateTimePicker>(
            this TDateTimePicker dateTimePicker,
            string format)
            where TDateTimePicker : DateTimePicker
        {
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = format;
            return dateTimePicker;
        }

        public static TDateTimePicker Format<TDateTimePicker>(
            this TDateTimePicker dateTimePicker,
            DateTimePickerFormat format)
            where TDateTimePicker : DateTimePicker
        {
            dateTimePicker.Format = format;
            return dateTimePicker;
        }
    }
}