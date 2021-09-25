using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Windows.Forms
{
    public static class ComponentExtensions
    {
                
        public static TComponent ComponentBinding<TSource, TSourceProp, TComponent, TTargetProp>(
            this TComponent component,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp,
            Expression<Func<TComponent, TTargetProp>> targetProp,
            Func<TSourceProp, TTargetProp>? convert = null,
            Func<TTargetProp, TSourceProp>? convertBack = null)
            where TComponent : IComponent
            where TSource : INotifyPropertyChanged
        {
            var targetInfo = ((MemberExpression)targetProp.Body).Member as PropertyInfo;
            var targetSetter = targetInfo.GetSetMethod();
            var sourceInfo = ((MemberExpression)sourceProp.Body).Member as PropertyInfo;
            var sourcePropName = ((MemberExpression)sourceProp.Body).Member.Name;
            var sourceGetter = sourceInfo.GetGetMethod();
            source.PropertyChanged += sourceOnPropertyChanged;
            sourceOnPropertyChanged(component, new PropertyChangedEventArgs(null));
            component.Disposed += (sender, args) =>  source.PropertyChanged -= sourceOnPropertyChanged;
            return component;

            void sourceOnPropertyChanged(object? sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == sourcePropName || string.IsNullOrWhiteSpace(args.PropertyName))
                {
                    targetSetter.Invoke(component, new object[] { sourceGetter.Invoke(source, null) });
                }
            }
        }

    }
}