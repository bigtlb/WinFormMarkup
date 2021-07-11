using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using RazorEngineCore;

namespace WikiDocumentGenerator.XMLToMD
{
    public class Generator : IDisposable
    {
        private static Assembly? _dllAssembly;
        private readonly IRazorEngineCompiledTemplate<RazorEngineTemplateBase<ClassInfoModel>> _classTemplate;
        private readonly IRazorEngineCompiledTemplate<RazorEngineTemplateBase<ClassInfoModel>> _classTemplateName;
        private readonly IEnumerable<string> _filterNamespaces;
        private readonly string _outputPath;
        private readonly XmlDocument _xmlNode;

        public Generator(string dllPath, string xmlPath, string outputPath,
            IEnumerable<string>? filterNamespaces = null)
        {
            var engine = new RazorEngine();
            _filterNamespaces = filterNamespaces ?? Enumerable.Empty<string>();
            _outputPath = outputPath;
            _classTemplate =
                engine.Compile<RazorEngineTemplateBase<ClassInfoModel>>(
                    File.ReadAllText("Templates/ClassTemplate.cshtml"),
                    builder => builder.AddAssemblyReference(typeof(IEnumerable<>)));
            _classTemplateName =
                engine.Compile<RazorEngineTemplateBase<ClassInfoModel>>(
                    File.ReadAllText("Templates/ClassTemplateName.cshtml"),
                    builder => builder.AddAssemblyReference(typeof(IEnumerable<>)));
            _dllAssembly = Assembly.LoadFrom(dllPath);
            _xmlNode = new XmlDocument();
            _xmlNode.Load(xmlPath);
            _xmlNode.SelectSingleNode("doc/members");
        }

        public void Dispose()
        {
        }

        public void Generate()
        {
            InitOutputDirectory();
            foreach (var t in EnumerateClasses())
                File.WriteAllText(Path.Combine($"{_outputPath}/{GenerateClassFileName(t)}"), GenerateClassFile(t));
        }


        public static string MakeNodeName(object obj)
        {
            string ret = "";
            if (obj is Type) ret = "T:" + ((Type) obj).FullName;

            if (obj is MethodBase)
            {
                ret = "M:" + ((MethodBase) obj).DeclaringType?.FullName;
                if (((MethodBase) obj).IsConstructor)
                    ret += ".#ctor";
                else
                    ret += "." + ((MethodBase) obj).Name;
                if (obj is MethodInfo)
                {
                    Type[] ptypes = ((MethodInfo) obj).GetGenericArguments();
                    if (ptypes != null && ptypes.Length > 0)
                        ret += "``" + ptypes.Length;
                }

                if (((MethodBase) obj).GetParameters().Length > 0)
                    ret += "(" + MakeParametersListInNode((MethodBase) obj, false, false) + ")";
            }

            if (obj is FieldInfo)
            {
                ret = "F:" + ((FieldInfo) obj).DeclaringType?.FullName;
                ret += "." + ((FieldInfo) obj).Name;
            }

            if (obj is PropertyInfo)
            {
                ret = "P:" + ((PropertyInfo) obj).DeclaringType?.FullName;
                ret += "." + ((PropertyInfo) obj).Name;
            }

            return ret;
        }

        private IEnumerable<Type> EnumerateClasses()
        {
            Debug.Assert(_dllAssembly != null, nameof(_dllAssembly) + " != null");
            return _dllAssembly
                .GetTypes()
                .Where(t => !_filterNamespaces.Any() ||
                            _filterNamespaces.Contains(t.Namespace, StringComparer.OrdinalIgnoreCase))
                .Where(t => !t.Name.StartsWith("<"))
                .OrderBy(t => t.Namespace).ThenBy(t => t.Name);
        }

        private string GenerateClassFile(Type classType)
        {
            var output = _classTemplate.Run(i => { i.Model = new ClassInfoModel(classType, _xmlNode); });
            return output;
        }

        private string GenerateClassFileName(Type classType)
        {
            var output = _classTemplateName.Run(i => { i.Model = new ClassInfoModel(classType, _xmlNode); });
            return output;
        }

        private void InitOutputDirectory()
        {
            if (Directory.Exists(_outputPath))
            {
                var di = new DirectoryInfo(_outputPath);
                foreach (var fi in di.EnumerateFiles("*.md").ToArray()) fi.Delete();
            }
            else
            {
                Directory.CreateDirectory(_outputPath);
            }
        }

        private static string MakedName(Type type, bool prefix = false, bool linked = true)
        {
            string ret = type.Name;
            if (type.GetGenericArguments().Length > 0)
            {
                ret = ret.Split("`")[0];
                ret += MakeGenericArgs(type);
            }

            if (prefix) ret = type.Namespace + "." + ret;
            if (type.Assembly == _dllAssembly && !type.IsGenericMethodParameter && !type.IsGenericTypeParameter &&
                linked)
            {
                string linkname = MakeMDFileName(type).Replace(".md", "");
                ret = "[" + ret + "](" + linkname + ")";
            }

            return ret;
        }

        private static string MakeGenericArgs(MethodInfo info)
        {
            string Content = "";
            Type[] ptypes = info.GetGenericArguments();
            if (ptypes.Length > 0)
            {
                Content += "< ";
                var begin = true;
                foreach (Type ptype in ptypes)
                {
                    if (!begin) Content += ", ";
                    Content += MakedName(ptype);
                    begin = false;
                }

                Content += " >";
            }

            return Content;
        }

        private static string MakeGenericArgs(Type info)
        {
            string Content = "";
            Type[] ptypes = info.GetGenericArguments();
            if (ptypes.Length > 0)
            {
                Content += "< ";
                var begin = true;
                foreach (Type ptype in ptypes)
                {
                    if (!begin) Content += ", ";
                    Content += MakedName(ptype);
                    begin = false;
                }

                Content += " >";
            }

            return Content;
        }


        private static string MakeMDFileName(Type type)
        {
            string name = type.Name;

            if (type.FullName == null)
                name = (type.Assembly.FullName + "-" + type.Name).Replace(".", "-").Replace("+", "-").Replace(">", "-")
                    .Replace("<", "-") + ".md";
            else
                name = type.FullName.Replace(".", "-").Replace("+", "-").Replace(">", "-").Replace("<", "-") + ".md";
            return name;
        }

        private static string MakeParametersListInNode(MethodBase info, bool link = true, bool paramName = true)
        {
            string Content = "";
            var isExt = info.IsDefined(typeof(ExtensionAttribute), true);
            ParameterInfo[] parameters = info.GetParameters();
            foreach (ParameterInfo pinfo in parameters)
            {
                if (pinfo.Position > 0)
                    Content += ",";

                var formatedName = "";
                if (link)
                    formatedName = MakedName(pinfo.ParameterType);
                else
                    formatedName = pinfo.ParameterType.FullName;

                if (formatedName == null) formatedName = "T";

                formatedName = formatedName.Replace("&", "@");

                Content += formatedName;

                if (paramName)
                {
                    Content += " " + pinfo.Name;
                    var valuestr = pinfo.DefaultValue?.ToString();
                    if (valuestr != "")
                    {
                        if (pinfo.DefaultValue is string ||
                            pinfo.DefaultValue?.GetType() == typeof(string))
                            valuestr = $"\"{valuestr}\"";
                        Content += " = " + valuestr;
                    }
                }
            }

            return Content;
        }
    }
}