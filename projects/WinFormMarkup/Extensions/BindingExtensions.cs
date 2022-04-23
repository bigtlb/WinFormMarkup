using System.Linq.Expressions;

namespace WinFormMarkup.Extensions;

public static class BindingExtensions
{
    /// <summary>
    ///     Sets up a `Binding` for a `Control.DataBindings` collection, and returns a reference to the control.
    /// </summary>
    /// <remarks>
    ///     UpdateMode is OnPropertyChanged, formattingEnabled=false
    /// </remarks>
    /// <param name="control">Target of the data binding</param>
    /// <param name="source">Source of the data binding</param>
    /// <param name="sourceProp">
    ///     A lambda expression accessing the source property (path derived through reflection and can be
    ///     several levels deep)
    /// </param>
    /// <param name="targetProp">A lambda expression accessing the direct target property (must be a member accessor).</param>
    /// <param name="convert">If present, hooks the `Binding.Format` event.</param>
    /// <param name="convertBack">If present, hooks the `Binding.Parse` event.</param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSourceProp"></typeparam>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TTargetProp"></typeparam>
    /// <returns></returns>
    public static TControl Binding<TSource, TSourceProp, TControl, TTargetProp>(
        this TControl control,
        TSource source,
        Expression<Func<TSource, TSourceProp>> sourceProp,
        Expression<Func<TControl, TTargetProp>> targetProp,
        Func<TSourceProp, TTargetProp>? convert = null,
        Func<TTargetProp, TSourceProp>? convertBack = null)
        where TControl : Control
    {
        var propertyName = ((MemberExpression)targetProp.Body).Member.Name;
        var sourceChain = sourceProp.Body.ToString();
        sourceChain = sourceChain.Substring(sourceChain.IndexOf(".", StringComparison.Ordinal) + 1);
        var b = new Binding(propertyName, source, sourceChain, false, DataSourceUpdateMode.OnPropertyChanged);
        if (convert != null)
        {
            // ReSharper disable once ConvertToLocalFunction
            ConvertEventHandler doConvert = (_, args) => args.Value = convert((TSourceProp)args.Value);
            b.Format += doConvert;
        }

        if (convertBack != null)
        {
            // ReSharper disable once ConvertToLocalFunction
            ConvertEventHandler doConvert = (_, args) => args.Value = convertBack((TTargetProp)args.Value);
            b.Parse += doConvert;
        }

        control.DataBindings.Add(b);
        return control;
    }

    /// <summary>
    ///     Sets up a `Binding` to the `Control.Text` property, and returns a reference to the control.
    /// </summary>
    /// <remarks>
    ///     UpdateMode is OnPropertyChanged, formattingEnabled=false
    /// </remarks>
    /// <param name="control"></param>
    /// <param name="source">Source of the data binding</param>
    /// <param name="sourceProp">
    ///     A lambda expression accessing the source property (path derived through reflection and can be
    ///     several levels deep)
    /// </param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSourceProp"></typeparam>
    /// <typeparam name="TControl"></typeparam>
    /// <returns></returns>
    public static TControl Binding<TSource, TSourceProp, TControl>(
        this TControl control,
        TSource source,
        Expression<Func<TSource, TSourceProp>> sourceProp)
        where TControl : Control
    {
        var sourceChain = sourceProp.Body.ToString();
        sourceChain = sourceChain.Substring(sourceChain.IndexOf(".", StringComparison.Ordinal) + 1);
        control.DataBindings.Add(new Binding("Text", source, sourceChain, false,
            DataSourceUpdateMode.OnPropertyChanged));
        return control;
    }

    /// <summary>
    ///     Sets up a `Binding` for a `Control.DataBindings` collection, and returns a reference to the control.
    /// </summary>
    /// <remarks>
    ///     UpdateMode is OnPropertyChanged, formattingEnabled=false
    /// </remarks>
    /// <param name="control">Target of the data binding</param>
    /// <param name="source">Source of the data binding</param>
    /// <param name="sourceProp">Data binding path on source</param>
    /// <param name="targetProp">Data binding path on target (default to "Text")</param>
    /// <param name="convert">If present, hooks the `Binding.Format` event.</param>
    /// <param name="convertBack">If present, hooks the `Binding.Parse` event.</param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSourceProp"></typeparam>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TTargetProp"></typeparam>
    /// <returns></returns>
    public static TControl Binding<TSource, TSourceProp, TControl, TTargetProp>(
        this TControl control,
        TSource source,
        string sourceProp,
        string targetProp = "Text",
        Func<TSourceProp, TTargetProp>? convert = null,
        Func<TTargetProp, TSourceProp>? convertBack = null)
        where TControl : Control
    {
        var b = new Binding(targetProp, source, sourceProp, false, DataSourceUpdateMode.OnPropertyChanged);
        if (convert != null)
        {
            // ReSharper disable once ConvertToLocalFunction
            ConvertEventHandler doConvert = (_, args) => args.Value = convert((TSourceProp)args.Value);
            b.Format += doConvert;
        }

        if (convertBack != null)
        {
            // ReSharper disable once ConvertToLocalFunction
            ConvertEventHandler doConvert = (_, args) => args.Value = convertBack((TTargetProp)args.Value);
            b.Parse += doConvert;
        }

        control.DataBindings.Add(b);
        return control;
    }

    //
    // /// <summary>Bind to a specified property</summary>
    // public static TBindable Bind<TBindable>(
    //     this TBindable bindable,
    //     BindableProperty targetProperty,
    //     string path = bindingContextPath,
    //     BindingMode mode = BindingMode.Default,
    //     IValueConverter? converter = null,
    //     object? converterParameter = null,
    //     string? stringFormat = null,
    //     object? source = null,
    //     object? targetNullValue = null,
    //     object? fallbackValue = null) where TBindable : BindableObject
    // {
    //     bindable.SetBinding(
    //         targetProperty,
    //         new Binding(path, mode, converter, converterParameter, stringFormat, source)
    //         {
    //             TargetNullValue = targetNullValue,
    //             FallbackValue = fallbackValue
    //         });
    //     return bindable;
    // }
}