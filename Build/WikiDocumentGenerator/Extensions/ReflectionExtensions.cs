using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using WikiDocumentGenerator.XMLToMD;

namespace WikiDocumentGenerator.Extensions;

public static class ReflectionExtensions
{
    private static readonly HashSet<string> recordBuiltInMethods = new()
    {
        "<Clone>$",
        "Deconstruct",
        "Equals",
        "GetHashCode",
        "op_Equality",
        "op_Inequality",
        "ToString"
    };

    private static readonly HashSet<string> s_keywords = new()
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while"
    };

    private static readonly XmlDocVisibilityLevel Visibility = XmlDocVisibilityLevel.Protected;

    public static string GetFullSignature(this MemberInfo memberInfo, ICollection<MemberInfo> seeAlsoMembers)
    {
        var stringBuilder = new StringBuilder();
        var lineBuilder = new StringBuilder();
        var segmentBuilder = new StringBuilder();
        const int maxLineLength = 100;

        void WrapLineIfNecessary(bool hard)
        {
            const string indent = "    ";

            if (lineBuilder.Length > indent.Length &&
                (hard || lineBuilder.Length + segmentBuilder.Length > maxLineLength))
            {
                lineBuilder.Append(Environment.NewLine);
                stringBuilder.Append(lineBuilder);
                lineBuilder.Clear();

                if (!hard)
                    lineBuilder.Append(indent);
            }
        }

        foreach (var part in GetFullSignatureParts(memberInfo, seeAlsoMembers))
            if (part.Length == 0 || part == Environment.NewLine)
            {
                WrapLineIfNecessary(false);

                lineBuilder.Append(segmentBuilder);
                segmentBuilder.Clear();

                WrapLineIfNecessary(part == Environment.NewLine);
            }
            else
            {
                segmentBuilder.Append(part);
            }

        WrapLineIfNecessary(false);

        lineBuilder.Append(segmentBuilder);
        stringBuilder.Append(lineBuilder);
        return stringBuilder.ToString();
    }

    public static IEnumerable<string> GetFullSignatureParts(this MemberInfo memberInfo,
        ICollection<MemberInfo> seeAlsoMembers)
    {
        var typeInfo = memberInfo as TypeInfo;
        var typeKind = typeInfo == null ? default(TypeKind?) : GetTypeKind(typeInfo);

        var obsoleteAttribute = memberInfo.GetCustomAttribute<ObsoleteAttribute>();
        if (obsoleteAttribute != null)
        {
            var message = obsoleteAttribute.Message;
            if (string.IsNullOrWhiteSpace(message))
            {
                yield return "[Obsolete]";
            }
            else
            {
                yield return "[Obsolete(";
                yield return RenderConstant(message);
                yield return ")]";
            }

            yield return Environment.NewLine;
        }

        var browsableAttribute = memberInfo.GetCustomAttribute<EditorBrowsableAttribute>();
        if (browsableAttribute != null && browsableAttribute.State != EditorBrowsableState.Always)
        {
            yield return "[EditorBrowsable(";
            yield return RenderConstant(browsableAttribute.State);
            yield return ")]";
            yield return Environment.NewLine;
        }

        var attributeUsage = memberInfo.GetCustomAttribute<AttributeUsageAttribute>();
        if (attributeUsage != null)
        {
            yield return "[AttributeUsage(";

            yield return RenderConstant(attributeUsage.ValidOn);

            if (!attributeUsage.Inherited)
            {
                yield return ", ";
                yield return "";
                yield return "Inherited = false";
            }

            if (attributeUsage.AllowMultiple)
            {
                yield return ", ";
                yield return "";
                yield return "AllowMultiple = true";
            }

            yield return ")]";
            yield return Environment.NewLine;
        }

        if (IsFlagsEnum(memberInfo))
        {
            yield return "[Flags]";
            yield return Environment.NewLine;
        }

        yield return $"{GetAccessModifier(memberInfo)} ";

        if (IsStatic(memberInfo))
            yield return "static ";
        else if (typeKind == TypeKind.Class && typeInfo!.IsSealed)
            yield return "sealed ";
        else if (IsAbstract(memberInfo))
            yield return "abstract ";
        else if (IsVirtual(memberInfo))
            yield return "virtual ";
        else if (IsOverride(memberInfo))
            yield return "override ";

        if (IsConst(memberInfo))
            yield return "const ";
        if (IsReadOnly(memberInfo))
            yield return "readonly ";

        if (memberInfo is EventInfo)
            yield return "event ";

        switch (typeKind)
        {
            case TypeKind.Record:
                yield return "record ";
                break;
            case TypeKind.Class:
                yield return "class ";
                break;
            case TypeKind.Interface:
                yield return "interface ";
                break;
            case TypeKind.Struct:
                yield return "struct ";
                break;
            case TypeKind.Enum:
                yield return "enum ";
                break;
            case TypeKind.Delegate:
                yield return "delegate ";
                break;
        }

        var shortName = GetOperatorKeywordName(GetShortName(memberInfo));
        if (shortName == "Item" && memberInfo is PropertyInfo)
            shortName = "this";

        var isConversion = shortName == "explicit operator" || shortName == "implicit operator";

        var nullableContextFlags = GetNullableContextFlags(memberInfo.GetCustomAttributes());
        if (nullableContextFlags.Length == 0)
        {
            var ancestor = memberInfo;
            while (true)
            {
                ancestor = ancestor.DeclaringType?.GetTypeInfo();
                if (ancestor is null)
                    break;
                nullableContextFlags = GetNullableContextFlags(ancestor.GetCustomAttributes());
                if (nullableContextFlags.Length != 0)
                    break;
            }
        }

        var (valueType, valueAttributes) = GetValueType(memberInfo);
        if (valueType != null && !isConversion)
        {
            yield return RenderTypeName(valueType, seeAlsoMembers, valueAttributes, nullableContextFlags);
            yield return " ";
        }

        if (valueType != null && isConversion)
        {
            yield return shortName;
            yield return " ";
            yield return RenderTypeName(valueType, seeAlsoMembers, valueAttributes, nullableContextFlags);
        }
        else
        {
            yield return "";
            yield return shortName;
        }

        var genericParameters = GetGenericArguments(memberInfo);
        if (genericParameters.Length != 0)
            yield return RenderGenericParameters(genericParameters);

        if (typeKind == TypeKind.Class || typeKind == TypeKind.Struct || typeKind == TypeKind.Interface)
        {
            var isFirstBase = true;
            if (typeKind == TypeKind.Class && typeInfo!.BaseType != typeof(object))
            {
                yield return " : ";
                yield return "";
                if (typeInfo.BaseType != null)
                    yield return RenderTypeName(typeInfo.BaseType.GetTypeInfo(), seeAlsoMembers);
                isFirstBase = false;
            }

            var baseInterfaces = typeInfo!.ImplementedInterfaces.Select(x => x.GetTypeInfo())
                .Where(x => x.IsPublic)
                .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.IsGenericType ? x.GenericTypeArguments.Length : 0)
                .ThenBy(x => x.IsGenericType ? RenderGenericArguments(x.GenericTypeArguments) : "",
                    StringComparer.OrdinalIgnoreCase)
                .ToList();
            var baseTypeInterfaces = typeInfo.BaseType?.GetTypeInfo().ImplementedInterfaces
                .Select(x => x.GetTypeInfo()).ToList();
            foreach (var baseInterface in baseInterfaces)
                if (!(typeKind == TypeKind.Class && baseTypeInterfaces!.Contains(baseInterface)) &&
                    !baseInterfaces.Any(
                        x => /*XmlDocUtility.GetXmlDocRef(x) != XmlDocUtility.GetXmlDocRef(baseInterface) &&*/
                            IsLessDerived(baseInterface, x)))
                {
                    yield return isFirstBase ? " : " : ", ";
                    yield return "";
                    yield return RenderTypeName(baseInterface, seeAlsoMembers);
                    isFirstBase = false;
                }
        }

        if (typeKind == TypeKind.Enum && Enum.GetUnderlyingType(typeInfo!.AsType()) != typeof(int))
        {
            yield return " : ";
            yield return RenderTypeName(Enum.GetUnderlyingType(typeInfo.AsType()).GetTypeInfo(), seeAlsoMembers);
        }

        ParameterInfo[]? parameterInfos = null;

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
        {
            parameterInfos = GetParameters(propertyInfo);
            if (parameterInfos.Length == 0)
                parameterInfos = null;
        }

        var methodInfo = memberInfo as MethodBase ?? TryGetDelegateInvoke(memberInfo);
        if (methodInfo != null)
            parameterInfos = GetParameters(methodInfo);

        if (parameterInfos != null)
        {
            yield return propertyInfo != null ? "[" : "(";
            yield return "";

            var isFirstParameter = true;
            foreach (var (parameterInfo, index) in parameterInfos.Select((x, i) => (x, i)))
            {
                if (!isFirstParameter)
                {
                    yield return ", ";
                    yield return "";
                }

                if (parameterInfo.GetCustomAttributes<CallerFilePathAttribute>().Any())
                    yield return "[CallerFilePath] ";
                if (parameterInfo.GetCustomAttributes<CallerLineNumberAttribute>().Any())
                    yield return "[CallerLineNumber] ";
                if (parameterInfo.GetCustomAttributes<CallerMemberNameAttribute>().Any())
                    yield return "[CallerMemberName] ";

                if (isFirstParameter && IsStatic(memberInfo) &&
                    memberInfo.GetCustomAttributes<ExtensionAttribute>().Any())
                    yield return "this ";

                if (parameterInfo.ParameterType.IsByRef)
                    yield return parameterInfo.IsOut ? "out " : "ref ";
                if (parameterInfo.GetCustomAttributes<ParamArrayAttribute>().Any())
                    yield return "params ";

                yield return RenderTypeName(parameterInfo.ParameterType.GetTypeInfo(),
                    seeAlsoMembers,
                    parameterInfo.GetCustomAttributes().ToList(),
                    nullableContextFlags);

                yield return " ";
                if (IsKeyword(parameterInfo.Name))
                    yield return "@";
                yield return
                    parameterInfo.Name ??
                    "P_" + index.ToString(CultureInfo.InvariantCulture); // default as per ILSpy if null

                if (ParameterHasDefaultValue(parameterInfo))
                {
                    yield return " = ";
                    if (CanRenderParameterConstant(parameterInfo))
                        yield return RenderConstant(parameterInfo.DefaultValue);
                    else if (parameterInfo.ParameterType.GetTypeInfo().IsValueType ||
                             parameterInfo.ParameterType.IsGenericParameter)
                        yield return "default";
                    else
                        yield return "null";
                }

                isFirstParameter = false;
            }

            yield return propertyInfo != null ? "]" : ")";
        }

        if (propertyInfo != null)
            yield return GetPropertyGetSet(propertyInfo);

        foreach (var genericParameter in genericParameters)
        {
            var isFirstPart = true;

            if (genericParameter.GetTypeInfo().GenericParameterAttributes
                .HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
            {
                yield return Environment.NewLine;
                yield return $"    where {genericParameter.Name} : ";

                yield return "class";
                isFirstPart = false;
            }

            var isStruct = genericParameter.GetTypeInfo().GenericParameterAttributes
                .HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint);
            if (isStruct)
            {
                if (isFirstPart)
                {
                    yield return Environment.NewLine;
                    yield return $"    where {genericParameter.Name} : ";
                }
                else
                {
                    yield return ", ";
                }

                yield return "struct";
                isFirstPart = false;
            }

            var genericConstraints = genericParameter.GetTypeInfo().GetGenericParameterConstraints();
            foreach (var genericConstraint in genericConstraints.Where(x => x != typeof(ValueType)))
            {
                if (isFirstPart)
                {
                    yield return Environment.NewLine;
                    yield return $"    where {genericParameter.Name} : ";
                }
                else
                {
                    yield return ", ";
                }

                yield return RenderTypeName(genericConstraint.GetTypeInfo(), seeAlsoMembers);
                isFirstPart = false;
            }

            if (!isStruct && genericParameter.GetTypeInfo().GenericParameterAttributes
                    .HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
            {
                if (isFirstPart)
                {
                    yield return Environment.NewLine;
                    yield return $"    where {genericParameter.Name} : ";
                }
                else
                {
                    yield return ", ";
                }

                yield return "new()";
            }
        }

        if (typeKind == TypeKind.Delegate || memberInfo is EventInfo || memberInfo is FieldInfo)
            yield return ";";
    }

    public static bool IsBuiltInRecordMethod(this MemberInfo memberInfo)
    {
        var declaringType = memberInfo.DeclaringType;
        if (declaringType == null) return false;
        if (!IsRecord(declaringType)) return false;
        return recordBuiltInMethods.Contains(memberInfo.Name);
    }

    public static bool IsRecord(Type type)
    {
        return type.GetMethod("<Clone>$") != null;
    }

    public static string MakeNodeName(this object obj)
    {
        StringBuilder GetDeclaringName(MemberInfo info, out Type[] classArgs)
        {
            var dt = info.DeclaringType ?? throw new ArgumentNullException("info.DeclaringType");
            var dn = new StringBuilder(dt.FullName);
            classArgs = dt.GetGenericArguments();
            if (classArgs.Length > 0) dn.Append('`').Append(classArgs.Length);

            return dn;
        }

        StringBuilder MakeParamList(MethodInfo info, Type[] classArgs, Type[] methodArgs)
        {
            StringBuilder GetParamID(Type paramType)
            {
                var pi = new StringBuilder();
                if (paramType.IsGenericType)
                {
                    var tickPos = paramType.Name.IndexOf('`');
                    pi.Append(paramType.Namespace).Append('.')
                        .Append(tickPos >= 0 ? paramType.Name[..tickPos] : paramType.Name)
                        .Append('{');
                    var idx = 0;
                    foreach (var argument in paramType.GenericTypeArguments)
                    {
                        if (idx++ > 0) pi.Append(',');
                        pi.Append(GetParamID(argument));
                    }

                    pi.Append('}');
                }
                else
                {
                    if (paramType.IsGenericMethodParameter || paramType.IsGenericTypeParameter)
                    {
                        if (classArgs != null && classArgs.Any(t => t.Name == paramType.Name))
                        {
                            var pos = Array.IndexOf(classArgs, classArgs.First(t => t.Name == paramType.Name));
                            pi.Append('`').Append(pos);
                        }
                        else
                        {
                            if (methodArgs != null && methodArgs.Any(t => t.Name == paramType.Name))
                            {
                                var pos = Array.IndexOf(methodArgs,
                                    methodArgs.First(t => t.Name == paramType.Name));
                                pi.Append("``").Append(pos);
                            }
                            else
                            {
                                pi.Append('T');
                            }
                        }
                    }
                    else
                    {
                        pi.Append(paramType.FullName?.Replace('&', '@'));
                    }
                }

                return pi;
            }

            var isExt = info.IsDefined(typeof(ExtensionAttribute), true);
            var hasGenericTypes = classArgs is { Length: > 0 } || methodArgs is { Length: > 0 };
            var pl = new StringBuilder();
            var parInfo = info.GetParameters();
            foreach (var p in parInfo)
            {
                if (pl.Length > 0) pl.Append(',');
                pl.Append(GetParamID(p.ParameterType));
            }

            return pl;
        }

        var s = new StringBuilder();
        Type[] classArgs;
        switch (obj)
        {
            case MethodInfo mi:
                s.Append("M:").Append(GetDeclaringName(mi, out classArgs)).Append('.')
                    .Append(mi.IsConstructor ? "#ctor" : mi.Name);
                var methodArgs = mi.GetGenericArguments();
                if (methodArgs.Length > 0) s.Append("``").Append(methodArgs.Length);

                s.Append('(').Append(MakeParamList(mi, classArgs, methodArgs)).Append(')');
                break;
            case FieldInfo fi:
                s.Append("F:").Append(GetDeclaringName(fi, out classArgs)).Append('.').Append(fi.Name);
                break;
            case PropertyInfo pi:
                s.Append("P:").Append(GetDeclaringName(pi, out classArgs)).Append('.').Append(pi.Name);
                break;
            case Type t:
                s.Append("T:").Append(t.FullName);
                break;
        }

        return s.ToString();
    }

    private static bool CanRenderParameterConstant(ParameterInfo parameterInfo)
    {
        return TryGetBuiltInTypeName(parameterInfo.ParameterType) != null ||
               TryGetBuiltInTypeName(Nullable.GetUnderlyingType(parameterInfo.ParameterType)) != null ||
               parameterInfo.ParameterType.GetTypeInfo().IsEnum;
    }

    private static int CountTupleItems(TypeInfo typeInfo)
    {
        return typeInfo.GenericTypeArguments.Length < 8
            ? typeInfo.GenericTypeArguments.Length
            : 7 + CountTupleItems(typeInfo.GenericTypeArguments[7].GetTypeInfo());
    }

    private static string EscapeChar(char ch)
    {
        return ch switch
        {
            '\'' => @"\'",
            '\"' => @"\""",
            '\\' => @"\\",
            '\a' => @"\a",
            '\b' => @"\b",
            '\f' => @"\f",
            '\n' => @"\n",
            '\r' => @"\r",
            '\t' => @"\t",
            '\v' => @"\v",
            _ => char.IsControl(ch) ? $"\\u{ch:x4}" : ch.ToString()
        };
    }

    private static string GetAccessModifier(MemberInfo memberInfo)
    {
        var visibility = GetVisibility(memberInfo, XmlDocVisibilityLevel.ProtectedInternal);
        return visibility switch
        {
            XmlDocVisibilityLevel.Public => "public",
            XmlDocVisibilityLevel.ProtectedInternal => "protected internal",
            XmlDocVisibilityLevel.Protected => "protected",
            XmlDocVisibilityLevel.Internal => "internal",
            XmlDocVisibilityLevel.Private => "private",
            _ => throw new InvalidOperationException()
        };
    }

    private static MemberOrder GetEventOrder(EventInfo eventInfo)
    {
        var method = eventInfo.AddMethod ?? eventInfo.RemoveMethod;
        if (method is not null && !method.IsStatic)
            return MemberOrder.InstanceEvent;
        return MemberOrder.StaticEvent;
    }

    private static MemberOrder GetFieldOrder(FieldInfo fieldInfo)
    {
        if (!fieldInfo.IsStatic)
            return MemberOrder.InstanceField;
        if (fieldInfo.FieldType == fieldInfo.DeclaringType)
            return MemberOrder.LifetimeField;
        return MemberOrder.StaticField;
    }

    private static XmlDocVisibilityLevel GetFieldVisibility(FieldInfo fieldInfo,
        XmlDocVisibilityLevel protectedInternal = XmlDocVisibilityLevel.Protected)
    {
        if (fieldInfo.IsPublic)
            return XmlDocVisibilityLevel.Public;
        if (fieldInfo.IsFamilyOrAssembly)
            return protectedInternal;
        if (fieldInfo.IsFamily)
            return XmlDocVisibilityLevel.Protected;
        if (fieldInfo.IsAssembly || fieldInfo.IsFamilyAndAssembly)
            return XmlDocVisibilityLevel.Internal;
        return XmlDocVisibilityLevel.Private;
    }

    private static Type[] GetGenericArguments(MemberInfo memberInfo)
    {
        var type = memberInfo as TypeInfo;
        if (type != null)
            return type.GenericTypeParameters;

        var method = memberInfo as MethodInfo;
        return method?.GetGenericArguments() ?? Array.Empty<Type>();
    }

    private static MemberOrder GetMemberOrder(MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            TypeInfo => MemberOrder.Type,
            ConstructorInfo => MemberOrder.Constructor,
            PropertyInfo propertyInfo => GetPropertyOrder(propertyInfo),
            EventInfo eventInfo => GetEventOrder(eventInfo),
            MethodInfo methodInfo => GetMethodOrder(methodInfo),
            FieldInfo fieldInfo => GetFieldOrder(fieldInfo),
            _ => MemberOrder.Unknown
        };
    }

    private static MemberOrder GetMethodOrder(MethodInfo methodInfo)
    {
        if (methodInfo.Name.StartsWith("op_", StringComparison.Ordinal))
            return MemberOrder.Operator;
        if (!methodInfo.IsStatic)
            return MemberOrder.InstanceMethod;
        if (methodInfo.ReturnType == methodInfo.DeclaringType)
            return MemberOrder.LifetimeMethod;
        return MemberOrder.StaticMethod;
    }

    private static XmlDocVisibilityLevel GetMethodVisibility(MethodBase methodBase,
        XmlDocVisibilityLevel protectedInternal = XmlDocVisibilityLevel.Protected)
    {
        if (methodBase.IsPublic)
            return XmlDocVisibilityLevel.Public;
        if (methodBase.IsFamilyOrAssembly)
            return protectedInternal;
        if (methodBase.IsFamily)
            return XmlDocVisibilityLevel.Protected;
        if (methodBase.IsAssembly || methodBase.IsFamilyAndAssembly)
            return XmlDocVisibilityLevel.Internal;
        return XmlDocVisibilityLevel.Private;
    }

    private static XmlDocVisibilityLevel GetMostPrivate(params XmlDocVisibilityLevel[] visibilityLevels)
    {
        return (XmlDocVisibilityLevel)visibilityLevels.Min(x => (int)x);
    }

    private static XmlDocVisibilityLevel GetMostPublic(params XmlDocVisibilityLevel[] visibilityLevels)
    {
        return (XmlDocVisibilityLevel)visibilityLevels.Max(x => (int)x);
    }

    private static byte[] GetNullableContextFlags(IEnumerable<Attribute> attributes)
    {
        var attribute = attributes.FirstOrDefault(x =>
            x.GetType().FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
        return attribute is null
            ? Array.Empty<byte>()
            : new[] { (byte)attribute?.GetType().GetField("Flag")?.GetValue(attribute)! };
    }

    private static byte[] GetNullableFlags(IEnumerable<Attribute> attributes)
    {
        var attribute = attributes.FirstOrDefault(x =>
            x.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");
        return (byte[]?)attribute?.GetType().GetField("NullableFlags")?.GetValue(attribute) ?? Array.Empty<byte>();
    }

    private static string GetOperatorKeywordName(string name)
    {
        return name switch
        {
            "op_Addition" => "operator +",
            "op_BitwiseAnd" => "operator &",
            "op_BitwiseOr" => "operator |",
            "op_Decrement" => "operator --",
            "op_Division" => "operator /",
            "op_Equality" => "operator ==",
            "op_ExclusiveOr" => "operator ^",
            "op_Explicit" => "explicit operator",
            "op_False" => "operator false",
            "op_GreaterThan" => "operator >",
            "op_GreaterThanOrEqual" => "operator >=",
            "op_Implicit" => "implicit operator",
            "op_Increment" => "operator ++",
            "op_Inequality" => "operator !=",
            "op_LeftShift" => "operator <<",
            "op_LessThan" => "operator <",
            "op_LessThanOrEqual" => "operator <=",
            "op_LogicalNot" => "operator !",
            "op_Modulus" => "operator %",
            "op_Multiply" => "operator *",
            "op_OnesComplement" => "operator ~",
            "op_RightShift" => "operator >>",
            "op_Subtraction" => "operator -",
            "op_True" => "operator true",
            "op_UnaryNegation" => "operator -",
            "op_UnaryPlus" => "operator +",
            _ => name
        };
    }

    private static ParameterInfo[] GetParameters(MemberInfo memberInfo)
    {
        var delegateInvoke = TryGetDelegateInvoke(memberInfo);
        if (delegateInvoke != null)
            return GetParameters(delegateInvoke);

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return propertyInfo.GetIndexParameters();

        var method = memberInfo as MethodBase;
        return method?.GetParameters() ?? Array.Empty<ParameterInfo>();
    }

    private static string GetParameterShortNames(MemberInfo memberInfo)
    {
        return string.Join(", ",
            GetParameters(memberInfo).Select(x => RenderTypeName(x.ParameterType.GetTypeInfo())));
    }

    private static string GetPropertyGetSet(PropertyInfo propertyInfo)
    {
        var getMethod = propertyInfo.GetMethod;
        var setMethod = propertyInfo.SetMethod;
        if (getMethod == null && setMethod == null)
            throw new InvalidOperationException();

        var getVisibility = getMethod == null ? XmlDocVisibilityLevel.Private : GetMethodVisibility(getMethod);
        var setVisibility = setMethod == null ? XmlDocVisibilityLevel.Private : GetMethodVisibility(setMethod);

        if (getMethod != null && (setMethod == null || IsMorePrivateThan(setVisibility, Visibility)))
            return " { get; }";
        if (getMethod == null || IsMorePrivateThan(getVisibility, Visibility))
            return " { set; }";

        if (getVisibility == setVisibility)
            return " { get; set; }";
        if (IsMorePrivateThan(getVisibility, setVisibility))
            return $" {{ {GetAccessModifier(getMethod)} get; set; }}";
        return $" {{ get; {GetAccessModifier(setMethod!)} set; }}";
    }

    private static MemberOrder GetPropertyOrder(PropertyInfo propertyInfo)
    {
        var method = propertyInfo.GetMethod ?? propertyInfo.SetMethod;
        if (method is not null && !method.IsStatic)
            return MemberOrder.InstanceProperty;
        if (propertyInfo.PropertyType == propertyInfo.DeclaringType)
            return MemberOrder.LifetimeProperty;
        return MemberOrder.StaticProperty;
    }

    private static XmlDocVisibilityLevel GetPropertyVisibility(PropertyInfo propertyInfo,
        XmlDocVisibilityLevel protectedInternal = XmlDocVisibilityLevel.Protected)
    {
        var getMethod = propertyInfo.GetMethod;
        var setMethod = propertyInfo.SetMethod;
        if (getMethod == null && setMethod == null)
            throw new InvalidOperationException();

        if (getMethod != null && setMethod == null)
            return GetMethodVisibility(getMethod);
        if (getMethod == null)
            return GetMethodVisibility(setMethod!);

        return GetMostPublic(
            GetMethodVisibility(propertyInfo.GetMethod!, protectedInternal),
            GetMethodVisibility(propertyInfo.SetMethod!, protectedInternal));
    }

    private static string GetShortName(MemberInfo memberInfo)
    {
        var name = memberInfo.Name;

        var tickIndex = name.IndexOf('`');
        if (tickIndex != -1)
            name = name.Substring(0, tickIndex);

        if (name == ".ctor" || name == ".cctor")
            name = GetShortName(memberInfo.DeclaringType!.GetTypeInfo());
        else if (name == "op_UnaryPlus")
            name = "op_Addition";
        else if (name == "op_UnaryNegation")
            name = "op_Subtraction";

        return name;
    }

    private static IReadOnlyList<string?> GetTupleNames(IEnumerable<Attribute> attributes)
    {
        return attributes.OfType<TupleElementNamesAttribute>().FirstOrDefault()?.TransformNames as
            IReadOnlyList<string?> ?? Array.Empty<string?>();
    }

    private static TypeKind GetTypeKind(TypeInfo typeInfo)
    {
        if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(typeInfo))
            return TypeKind.Delegate;
        if (IsRecord(typeInfo))
            return TypeKind.Record;
        if (typeInfo.IsClass)
            return TypeKind.Class;
        if (typeInfo.IsInterface)
            return TypeKind.Interface;
        if (typeInfo.IsEnum)
            return TypeKind.Enum;
        if (typeInfo.IsValueType)
            return TypeKind.Struct;
        return TypeKind.Unknown;
    }

    private static XmlDocVisibilityLevel GetTypeVisibility(TypeInfo typeInfo,
        XmlDocVisibilityLevel protectedInternal = XmlDocVisibilityLevel.Protected)
    {
        if (typeInfo.IsPublic || typeInfo.IsNestedPublic)
            return XmlDocVisibilityLevel.Public;
        if (typeInfo.IsNestedFamORAssem)
            return protectedInternal;
        if (typeInfo.IsNestedFamily)
            return XmlDocVisibilityLevel.Protected;
        if (typeInfo.IsNestedAssembly || typeInfo.IsNestedFamANDAssem)
            return XmlDocVisibilityLevel.Internal;
        return XmlDocVisibilityLevel.Private;
    }

    private static (TypeInfo? Type, IReadOnlyList<Attribute>? Attributes) GetValueType(MemberInfo member)
    {
        var eventInfo = member as EventInfo;
        if (eventInfo != null)
            return (eventInfo?.EventHandlerType?.GetTypeInfo(), eventInfo?.GetCustomAttributes().ToList());

        var propertyInfo = member as PropertyInfo;
        if (propertyInfo != null)
            return (propertyInfo.PropertyType.GetTypeInfo(), propertyInfo.GetCustomAttributes().ToList());

        var fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
            return (fieldInfo.FieldType.GetTypeInfo(), fieldInfo.GetCustomAttributes().ToList());

        var methodInfo = member as MethodInfo ?? TryGetDelegateInvoke(member);
        if (methodInfo != null)
            return (methodInfo.ReturnType.GetTypeInfo(),
                methodInfo.ReturnTypeCustomAttributes.GetCustomAttributes(false).OfType<Attribute>().ToList());

        return default;
    }

    private static XmlDocVisibilityLevel GetVisibility(MemberInfo memberInfo)
    {
        return GetVisibility(memberInfo, XmlDocVisibilityLevel.Protected);
    }

    private static XmlDocVisibilityLevel GetVisibility(MemberInfo memberInfo,
        XmlDocVisibilityLevel protectedInternal)
    {
        var typeInfo = memberInfo as TypeInfo;
        if (typeInfo != null)
        {
            var visibility = GetTypeVisibility(typeInfo);
            return typeInfo.IsNested
                ? GetMostPrivate(visibility,
                    GetTypeVisibility(typeInfo.DeclaringType!.GetTypeInfo(), protectedInternal))
                : visibility;
        }

        var eventInfo = memberInfo as EventInfo;
        if (eventInfo != null)
            return GetMethodVisibility(eventInfo.AddMethod!, protectedInternal);

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return GetPropertyVisibility(propertyInfo, protectedInternal);

        var fieldInfo = memberInfo as FieldInfo;
        if (fieldInfo != null)
            return GetFieldVisibility(fieldInfo, protectedInternal);

        var methodBase = memberInfo as MethodBase;
        if (methodBase != null)
            return GetMethodVisibility(methodBase, protectedInternal);

        return XmlDocVisibilityLevel.Private;
    }

    private static bool IsAbstract(MemberInfo memberInfo)
    {
        var typeInfo = memberInfo as TypeInfo;
        if (typeInfo != null && !typeInfo.IsInterface)
            return typeInfo.IsAbstract;

        if (memberInfo.DeclaringType?.GetTypeInfo().IsInterface == true)
            return false;

        var eventInfo = memberInfo as EventInfo;
        if (eventInfo != null)
            return eventInfo.AddMethod != null && IsAbstract(eventInfo.AddMethod);

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return propertyInfo.GetMethod != null && IsAbstract(propertyInfo.GetMethod) ||
                   propertyInfo.SetMethod != null && IsAbstract(propertyInfo.SetMethod);

        var methodBase = memberInfo as MethodBase;
        if (methodBase != null)
            return methodBase.IsAbstract;

        return false;
    }

    private static bool IsConst(MemberInfo memberInfo)
    {
        return (memberInfo as FieldInfo)?.IsLiteral ?? false;
    }

    private static bool IsFlagsEnum(MemberInfo memberInfo)
    {
        var type = memberInfo as TypeInfo;
        return type != null && type.IsEnum && type.GetCustomAttributes<FlagsAttribute>().Any();
    }

    private static bool IsKeyword(string? value)
    {
        return s_keywords.Contains(value ?? string.Empty);
    }

    private static bool IsLessDerived(TypeInfo a, TypeInfo b)
    {
        if (!a.IsAssignableFrom(b))
            return false;
        if (a.IsGenericType && b.IsGenericType && a.GetGenericTypeDefinition().GetTypeInfo() ==
            b.GetGenericTypeDefinition().GetTypeInfo())
        {
            var parameters = a.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters;
            for (var i = 0; i < parameters.Length; i++)
            {
                var paramAttributes = parameters[i].GetTypeInfo().GenericParameterAttributes;
                var aArgInfo = a.GenericTypeArguments[i].GetTypeInfo();
                var bArgInfo = b.GenericTypeArguments[i].GetTypeInfo();
                if (paramAttributes.HasFlag(GenericParameterAttributes.Contravariant) &&
                    !aArgInfo.IsAssignableFrom(bArgInfo))
                    return false;
                if (paramAttributes.HasFlag(GenericParameterAttributes.Covariant) &&
                    aArgInfo.IsAssignableFrom(bArgInfo))
                    return false;
            }
        }

        return true;
    }

    private static bool IsMorePrivateThan(XmlDocVisibilityLevel visibility1, XmlDocVisibilityLevel visibility2)
    {
        return (int)visibility1 < (int)visibility2;
    }

    private static bool IsOverride(MemberInfo memberInfo)
    {
        if (memberInfo.DeclaringType?.GetTypeInfo().IsInterface == true)
            return false;

        var eventInfo = memberInfo as EventInfo;
        if (eventInfo != null)
            return eventInfo.AddMethod != null && IsOverride(eventInfo.AddMethod);

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return propertyInfo.GetMethod != null && IsOverride(propertyInfo.GetMethod) ||
                   propertyInfo.SetMethod != null && IsOverride(propertyInfo.SetMethod);

        var methodInfo = memberInfo as MethodInfo;
        if (methodInfo != null)
            return methodInfo.IsVirtual && !methodInfo.IsFinal &&
                   methodInfo.GetRuntimeBaseDefinition()!.DeclaringType != methodInfo.DeclaringType;

        return false;
    }

    private static bool IsReadOnly(MemberInfo memberInfo)
    {
        return (memberInfo as FieldInfo)?.IsInitOnly ?? false;
    }

    private static bool IsStatic(MemberInfo memberInfo)
    {
        var typeInfo = memberInfo as TypeInfo;
        if (typeInfo != null)
            return typeInfo.IsClass && typeInfo.IsAbstract && typeInfo.IsSealed;

        var eventInfo = memberInfo as EventInfo;
        if (eventInfo != null)
            return eventInfo.AddMethod!.IsStatic;

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return (propertyInfo.GetMethod ?? propertyInfo.SetMethod)?.IsStatic ?? false;

        var fieldInfo = memberInfo as FieldInfo;
        if (fieldInfo != null)
            return fieldInfo.IsStatic && !fieldInfo.IsLiteral;

        var methodBase = memberInfo as MethodBase;
        if (methodBase != null)
            return methodBase.IsStatic;

        return false;
    }

    private static bool IsVirtual(MemberInfo memberInfo)
    {
        if (memberInfo.DeclaringType?.GetTypeInfo().IsInterface == true)
            return false;

        var eventInfo = memberInfo as EventInfo;
        if (eventInfo != null)
            return eventInfo.AddMethod != null && IsVirtual(eventInfo.AddMethod);

        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo != null)
            return propertyInfo.GetMethod != null && IsVirtual(propertyInfo.GetMethod) ||
                   propertyInfo.SetMethod != null && IsVirtual(propertyInfo.SetMethod);

        var methodInfo = memberInfo as MethodInfo;
        if (methodInfo != null)
            return methodInfo.IsVirtual && !methodInfo.IsFinal &&
                   methodInfo.GetRuntimeBaseDefinition()!.DeclaringType == methodInfo.DeclaringType;

        return false;
    }

    private static IEnumerable<T> OrderMembers<T>(IEnumerable<T> items, Func<T, MemberInfo> getMemberInfo)
    {
        return items.OrderBy(x => (int)GetMemberOrder(getMemberInfo(x)))
            .ThenBy(x => GetShortName(getMemberInfo(x)).ToString(), StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => GetGenericArguments(getMemberInfo(x)).Length)
            .ThenBy(x => GetParameters(getMemberInfo(x)).Length)
            .ThenBy(x => GetParameterShortNames(getMemberInfo(x)), StringComparer.OrdinalIgnoreCase);
    }

    private static bool ParameterHasDefaultValue(ParameterInfo parameterInfo)
    {
        if (parameterInfo.Attributes.HasFlag(ParameterAttributes.HasDefault))
            return true;

        if (parameterInfo.ParameterType == typeof(decimal) || parameterInfo.ParameterType == typeof(decimal?))
            return parameterInfo.HasDefaultValue;

        return false;
    }

    private static string RenderChar(char ch)
    {
        return "'" + (ch == '\"' ? "\"" : EscapeChar(ch)) + "'";
    }

    private static string RenderConstant(object? value)
    {
        if (value == null)
            return "null";

        if (value is bool valueAsBool)
            return valueAsBool ? "true" : "false";

        if (value is char valueAsChar)
            return RenderChar(valueAsChar);

        if (value is string valueAsString)
            return RenderString(valueAsString);

        var type = value.GetType();
        if (type.GetTypeInfo().IsEnum)
            return string.Join(" | ", (value.ToString() ?? "")
                .Split(new[] { ", " }, StringSplitOptions.None)
                .Select(x => $"{type.Name}.{x}"));

        var rendered = Convert.ToString(value, CultureInfo.InvariantCulture);

        if (value is double)
            rendered += "m";

        return rendered ?? "";
    }

    private static string RenderGenericArguments(Type[]? genericArguments, ICollection<MemberInfo>? seeAlso = null)
    {
        var tupleNameIndex = 0;
        var nullableFlagIndex = 0;
        return RenderGenericArguments(genericArguments, seeAlso, Array.Empty<string?>(), ref tupleNameIndex,
            Array.Empty<byte>(), ref nullableFlagIndex);
    }

    private static string RenderGenericArguments(Type[]? genericArguments, ICollection<MemberInfo>? seeAlso,
        IReadOnlyList<string?> tupleNames, ref int tupleNameIndex, byte[] nullableFlags, ref int nullableFlagIndex)
    {
        if (genericArguments == null)
            return "";

        var stringBuilder = new StringBuilder();
        for (var index = 0; index < genericArguments.Length; index++)
        {
            var genericArgument = genericArguments[index];
            stringBuilder.Append((index == 0 ? "<" : "") +
                                 RenderTypeName(genericArgument.GetTypeInfo(), seeAlso, tupleNames,
                                     ref tupleNameIndex, nullableFlags, ref nullableFlagIndex) +
                                 (index < genericArguments.Length - 1 ? ", " : "") +
                                 (index == genericArguments.Length - 1 ? ">" : ""));
        }

        return stringBuilder.ToString();
    }

    private static string RenderGenericParameters(Type[] genericParameters)
    {
        var stringBuilder = new StringBuilder();
        for (var index = 0; index < genericParameters.Length; index++)
        {
            var genericParameter = genericParameters[index];
            stringBuilder.Append((index == 0 ? "<" : "") +
                                 (genericParameter.GetTypeInfo().GenericParameterAttributes
                                     .HasFlag(GenericParameterAttributes.Covariant)
                                     ? "out "
                                     : "") +
                                 (genericParameter.GetTypeInfo().GenericParameterAttributes
                                     .HasFlag(GenericParameterAttributes.Contravariant)
                                     ? "in "
                                     : "") +
                                 genericParameter.Name +
                                 (index < genericParameters.Length - 1 ? ", " : "") +
                                 (index == genericParameters.Length - 1 ? ">" : ""));
        }

        return stringBuilder.ToString();
    }

    private static string RenderShortGenericParameters(Type[] genericParameters)
    {
        if (genericParameters == null)
            return "";

        var stringBuilder = new StringBuilder();
        for (var index = 0; index < genericParameters.Length; index++)
        {
            var genericParameter = genericParameters[index];
            stringBuilder.Append((index == 0 ? "<" : "") +
                                 genericParameter.Name +
                                 (index < genericParameters.Length - 1 ? "," : "") +
                                 (index == genericParameters.Length - 1 ? ">" : ""));
        }

        return stringBuilder.ToString();
    }

    private static string RenderString(string value)
    {
        var builder = new StringBuilder("\"");
        foreach (var ch in value)
            builder.Append(ch == '\'' ? "'" : EscapeChar(ch));
        return builder.Append('"').ToString();
    }

    private static IReadOnlyList<string> RenderTupleTypes(TypeInfo typeInfo, ICollection<MemberInfo>? seeAlso,
        IReadOnlyList<string?> tupleNames, ref int tupleNameIndex, byte[] nullableFlags, ref int nullableFlagIndex)
    {
        var renderedTupleTypes = new List<string>();

        if (typeInfo.Namespace == "System" && typeInfo.Name.StartsWith("ValueTuple`", StringComparison.Ordinal))
        {
            var ourTupleNameIndex = tupleNameIndex;
            tupleNameIndex += CountTupleItems(typeInfo);

            var genericTypeArguments = typeInfo.GenericTypeArguments;
            while (true)
            {
                foreach (var genericTypeArgument in genericTypeArguments.Take(7))
                {
                    var itemType = genericTypeArgument.GetTypeInfo();
                    var renderedTupleType = RenderTypeName(itemType, seeAlso, tupleNames, ref tupleNameIndex,
                        nullableFlags, ref nullableFlagIndex);
                    var itemName = tupleNames.ElementAtOrDefault(ourTupleNameIndex++);
                    if (itemName is not null)
                        renderedTupleType += $" {itemName}";
                    renderedTupleTypes.Add(renderedTupleType);
                }

                if (genericTypeArguments.Length < 8)
                    break;

                genericTypeArguments = genericTypeArguments[7].GenericTypeArguments;
                tupleNameIndex++;
            }
        }

        return renderedTupleTypes;
    }

    private static string RenderTypeName(TypeInfo typeInfo, ICollection<MemberInfo>? seeAlso = null,
        IReadOnlyList<Attribute>? attributes = null, byte[]? nullableContextFlags = null)
    {
        attributes ??= Array.Empty<Attribute>();
        var tupleNames = GetTupleNames(attributes);
        var tupleNameIndex = 0;
        var nullableFlags = GetNullableFlags(attributes);
        if (nullableFlags.Length == 0)
            nullableFlags = nullableContextFlags ?? Array.Empty<byte>();
        var nullableFlagIndex = 0;
        return RenderTypeName(typeInfo, seeAlso, tupleNames, ref tupleNameIndex, nullableFlags,
            ref nullableFlagIndex);
    }

    private static string RenderTypeName(TypeInfo typeInfo, ICollection<MemberInfo>? seeAlso,
        IReadOnlyList<string?> tupleNames, ref int tupleNameIndex, byte[] nullableFlags, ref int nullableFlagIndex)
    {
        if (typeInfo.IsByRef)
            return RenderTypeName(typeInfo.GetElementType()!.GetTypeInfo(), seeAlso, tupleNames, ref tupleNameIndex,
                nullableFlags, ref nullableFlagIndex);

        var nullableOfType = Nullable.GetUnderlyingType(typeInfo.AsType());
        if (nullableOfType != null)
            return
                $"{RenderTypeName(nullableOfType.GetTypeInfo(), seeAlso, tupleNames, ref tupleNameIndex, nullableFlags, ref nullableFlagIndex)}?";

        var nullableSuffix = "";
        if (!typeInfo.IsValueType || typeInfo.IsGenericType)
        {
            if (nullableFlagIndex < nullableFlags.Length && nullableFlags[nullableFlagIndex] == 2)
                nullableSuffix = "?";
            if (nullableFlagIndex < nullableFlags.Length - 1)
                nullableFlagIndex++;
        }

        if (typeInfo.IsArray)
            return
                $"{RenderTypeName(typeInfo.GetElementType()!.GetTypeInfo(), seeAlso, tupleNames, ref tupleNameIndex, nullableFlags, ref nullableFlagIndex)}[]" +
                nullableSuffix;

        var builtIn = TryGetBuiltInTypeName(typeInfo.AsType());
        if (builtIn != null)
            return builtIn + nullableSuffix;

        var renderedTupleTypes = RenderTupleTypes(typeInfo, seeAlso, tupleNames, ref tupleNameIndex, nullableFlags,
            ref nullableFlagIndex);
        if (renderedTupleTypes.Count > 1)
            return $"({string.Join(", ", renderedTupleTypes)})";

        seeAlso?.Add(typeInfo);

        return GetShortName(typeInfo) + RenderGenericArguments(typeInfo.GenericTypeArguments, seeAlso, tupleNames,
            ref tupleNameIndex, nullableFlags, ref nullableFlagIndex) + nullableSuffix;
    }

    private static string? TryGetBuiltInTypeName(Type? type)
    {
        if (type == typeof(void))
            return "void";
        if (type == typeof(bool))
            return "bool";
        if (type == typeof(byte))
            return "byte";
        if (type == typeof(sbyte))
            return "sbyte";
        if (type == typeof(char))
            return "char";
        if (type == typeof(decimal))
            return "decimal";
        if (type == typeof(double))
            return "double";
        if (type == typeof(float))
            return "float";
        if (type == typeof(int))
            return "int";
        if (type == typeof(uint))
            return "uint";
        if (type == typeof(long))
            return "long";
        if (type == typeof(ulong))
            return "ulong";
        if (type == typeof(object))
            return "object";
        if (type == typeof(short))
            return "short";
        if (type == typeof(ushort))
            return "ushort";
        if (type == typeof(string))
            return "string";
        return null;
    }

    private static MethodInfo? TryGetDelegateInvoke(MemberInfo memberInfo)
    {
        var typeInfo = memberInfo as TypeInfo;
        return typeInfo != null && typeof(Delegate).GetTypeInfo().IsAssignableFrom(typeInfo)
            ? typeInfo.DeclaredMethods.FirstOrDefault(x => x.Name == "Invoke")
            : null;
    }

    private enum TypeKind
    {
        Unknown,
        Class,
        Interface,
        Struct,
        Enum,
        Delegate,
        Record
    }

    private enum MemberOrder
    {
        Constructor,
        LifetimeProperty,
        LifetimeField,
        LifetimeMethod,
        InstanceProperty,
        InstanceField,
        InstanceEvent,
        InstanceMethod,
        StaticProperty,
        StaticField,
        StaticEvent,
        StaticMethod,
        Operator,
        Type,
        Unknown
    }
}