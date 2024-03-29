﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using WikiDocumentGenerator.Extensions;

namespace WikiDocumentGenerator.XMLToMD;

public class ClassInfoModel
{
    private readonly Type _classType;
    private readonly XmlNode _node;
    protected ICollection<MemberInfo> SeeAlso = new List<MemberInfo>();

    public ClassInfoModel(Type classType, XmlNode node)
    {
        _classType = classType;
        _node = node;
    }

    public string Name => _classType.Name.LastIndexOf('`') > -1
        ? _classType.Name[.._classType.Name.LastIndexOf('`')]
        : _classType.Name;

    public string? Summary => GetNode()?.SelectSingleNode("summary")?.InnerText.CleanLines();
    public bool HasSeeAlso => SeeAlso.Count > 0;
    public string FullSignature => _classType.GetFullSignature(SeeAlso);

    public IEnumerable<MemberInfoModel> SeeAlsoMembers =>
        SeeAlso.OrderBy(m => m.Name).Distinct().Select(m => new MemberInfoModel(m, _node));


    public IEnumerable<MemberInfoModel> Members => _classType.GetMembers(
            BindingFlags.Instance
            | BindingFlags.Static
            | BindingFlags.Public
        ).Where(x =>
        {
            if (x.DeclaringType != _classType)
                return false;
            var attr = x.GetCustomAttributes(true).ToArray();
            if (attr != null &&
                attr.FirstOrDefault(y => y is CompilerGeneratedAttribute) !=
                default)
                return false;
            if (x.IsBuiltInRecordMethod())
                return false;
            return true;
        })
        .OrderBy(t => t.Name)
        .Select(t => new MemberInfoModel(t, _node));

    public XmlNode? GetNode()
    {
        try
        {
            var result = _node?.SelectSingleNode($".//member[@name=\"{_classType.MakeNodeName()}\"]");
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
}