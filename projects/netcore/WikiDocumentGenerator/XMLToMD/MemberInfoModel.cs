using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using WikiDocumentGenerator.Extensions;

namespace WikiDocumentGenerator.XMLToMD
{
    public class MemberInfoModel
    {
        private readonly MemberInfo _memberInfo;
        private readonly XmlNode _node;

        public Lazy<XmlNode?> CurrentNode;
        protected ICollection<MemberInfo> SeeAlso = new List<MemberInfo>();

        public MemberInfoModel(MemberInfo memberInfo, XmlNode node)
        {
            _memberInfo = memberInfo;
            _node = node;
            CurrentNode = new Lazy<XmlNode?>(() =>
            {
                try
                {
                    var result = _node?.SelectSingleNode($".//member[@name=\"{_memberInfo.MakeNodeName()}\"]");
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            });

        }

        public string Name => _memberInfo.Name;
        public string? Summary => CurrentNode.Value?.SelectSingleNode("summary")?.InnerXml.CleanLines();
        public string FullSignature => _memberInfo.GetFullSignature(SeeAlso);
        public bool HasSeeAlso => SeeAlso.Count>0;
        public bool HasParams => CurrentNode.Value?.SelectNodes("param")?.Count > 0;
        public bool HasRemarks => Remarks != null;
        public bool HasExample => Example != null;

        public string? Remarks => CurrentNode.Value?.SelectSingleNode("remarks")?.InnerXml.CleanLines(blockIndent:true);
        public string? Example => CurrentNode.Value?.SelectSingleNode("example")?.InnerXml.CleanLines();

        public IEnumerable<ParameterInfoModel>? Params => CurrentNode.Value?
                .SelectNodes("param")?.Cast<XmlNode>()
                .Select(p => new ParameterInfoModel(p));

        public IEnumerable<MemberInfoModel>? SeeAlsoMembers =>
            SeeAlso.OrderBy(m => m.Name).Distinct().Select(m => new MemberInfoModel(m, _node));
    }

    public class ParameterInfoModel
    {
        private readonly XmlNode _param;

        public ParameterInfoModel(XmlNode param)
        {
            _param = param;
        }

        public string? Name => _param?.Attributes?["name"]?.Value;
        public string? Summary => _param?.InnerText?.CleanLines();
    }
}
