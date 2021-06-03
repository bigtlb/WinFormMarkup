using System;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class ControlExtensions
    {
        public static TControl Text<TControl>(
            this TControl control,
            string text)
            where TControl : Control
        {
            control.Text = text;
            return control;
        }

        public static TControl Dock<TControl>(
            this TControl control,
            DockStyle dockPosition)
            where TControl : Control
        {
            control.Dock = dockPosition;
            return control;
        }

        /// <summary>
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

        /// <summary>
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
                        "Margin must be either 2 (width, height), or 4 (left, top, width, height)");
            }

            return control;
        }
        
        public static TControl Position<TControl>(
            this TControl control,
            int left,
            int top)
            where TControl : Control
        {
            control.Left = left;
            control.Top = top;

            return control;
        }
        
        public static TControl ToFront<TControl>(
            this TControl control)
            where TControl : Control
        {
            EventHandler doToFront = (s,e) => control.BringToFront();
            if (control.Parent == null)
            {
                control.ParentChanged += doToFront;
            }
            else
            {
                control.BeginInvoke(doToFront);
            }
            return control;
        }
        
        public static TControl Anchor<TControl>(
            this TControl control,
            AnchorStyles anchors)
            where TControl : Control
        {
            control.Anchor = anchors;
            return control;
        }

        public static TControl AutoSize<TControl>(
            this TControl control,
            bool autoSize)
            where TControl : Control
        {
            control.AutoSize = autoSize;
            return control;
        }

        public static TControl BackColor<TControl>(
            this TControl control,
            Color color)
            where TControl : Control
        {
            control.BackColor = color;
            return control;
        }

        public static TControl Binding<TSource, TSourceProp, TControl, TTargetProp>(
            this TControl control,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp,
            Expression<Func<TControl, TTargetProp>> targetProp,
            Func<TSourceProp, TTargetProp> convert = null,
            Func<TTargetProp, TSourceProp> convertBack = null)
            where TControl : Control
        {
            var propertyName = ((MemberExpression) targetProp.Body).Member.Name;
            var sourceChain = sourceProp.Body.ToString();
            sourceChain = sourceChain.Substring(sourceChain.IndexOf(".") + 1);
            //TODO Deal with converters
            //control.DataBindings.Add(propertyName, source, sourceChain, false, DataSourceUpdateMode.OnPropertyChanged);
            control.DataBindings.Add(new Binding(propertyName, source, sourceChain));
            return control;
        }
        
        public static TControl Binding<TSource, TSourceProp, TControl>(
            this TControl control,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp)
            where TControl : Control
        {
            var sourceChain = sourceProp.Body.ToString();
            sourceChain = sourceChain.Substring(sourceChain.IndexOf(".") + 1);
            control.DataBindings.Add(new Binding("Text", source, sourceChain));
            return control;
        }

        public static TControl Controls<TControl>(
            this TControl control,
            params Control[] children)
            where TControl : Control
        {
            control.Controls.AddRange(children);
            foreach (var c in children
                .Where(c => c.Dock == DockStyle.Fill))
                c.BringToFront();

            return control;
        }


        public static TControl Clicked<TControl>(
            this TControl control,
            Action<TControl> action)
            where TControl : Control
        {
            control.Click += (sender, args) => action(sender as TControl);
            return control;
        }
    }
}