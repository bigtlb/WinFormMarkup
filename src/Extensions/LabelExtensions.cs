using System;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class LabelExtensions
    {
        public static TLabel AutoSize<TLabel>(
            this TLabel label,
            bool autoSize)
            where TLabel : Label
        {
            label.AutoSize = autoSize;
            return label;
        }

        public static TLabel Bind<TSource, TSourceProp, TLabel>(
            this TLabel label,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp)
            where TLabel : Label
        {
            var sourceChain = sourceProp.Body.ToString();
            sourceChain = sourceChain.Substring(sourceChain.IndexOf(".") + 1);
            label.DataBindings.Add(new Binding("Text", source, sourceChain));
            return label;
        }
    }
}