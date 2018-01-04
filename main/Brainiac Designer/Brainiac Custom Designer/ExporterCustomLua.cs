using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;
using BrainiacCustomDesigner.Nodes;

namespace BrainiacCustomDesigner.Exporters
{
    public class ExporterCustomLua : Brainiac.Design.Exporters.Exporter
    {
        public ExporterCustomLua(BehaviorNode node, string outputFolder, string filename)
            : base(node, outputFolder, filename + ".lua")
        {
        }

        protected virtual void ExportBehavior(StreamWriter file, BehaviorNode behavior)
        {
            string classname = Path.GetFileNameWithoutExtension(behavior.FileManager.Filename).Replace(" ", string.Empty);
            file.Write(string.Format("{0} = class(BTRoot)\r\n\r\n", classname));
            file.Write(string.Format("function {0}:_init_()\r\n", classname));
            int nodeID = 0;
            foreach (Node child in ((Node)behavior).Children)
                ExportNode(file, "self", child, 1, ref nodeID);
            file.Write("end\r\n\r\n");
            file.Write(string.Format("return {0}()\r\n", classname));

        }

        protected virtual void ExportConstructorAndProperties(StringWriter file, Node node, string indent, string nodeName, string classname)
        {
            string args = GetProperties(node);
            file.Write(string.Format("{0}\tlocal {1} = {2}:new({3});\r\n", indent, nodeName, classname, args));
        }

        protected void ExportNode(StreamWriter file, string parentName, Node node, int indentDepth, ref int nodeID)
        {
            string classname = node.ExportClass;
            string nodeName = string.Format("node{0}", ++nodeID);
            string indent = string.Empty;
            for (int i = 0; i < indentDepth; ++i)
                indent += "\n";
            string args = "";
            if (node is CustomAction || node is CustomCondition)
            {
                args = GetProperties(node);
                file.Write(string.Format("{0}{1}:addChild({2}({3}));\r\n", indent, parentName, classname, args));
            }
            else if (node is CustomSelector || node is CustomSequence || node is CustomParallel)
            {
                file.Write(string.Format("{0}local {1} = {2}();\r\n", indent, nodeName, classname));
                file.Write(string.Format("{0}{1}:addChild({2});\r\n", indent, parentName, nodeName));
                foreach (Node child in node.Children)
                    ExportNode(file, nodeName, child, indentDepth + 1, ref nodeID);
            }
        }

        protected string GetProperties(Node node)
        {
            string ret = "";
            IList<DesignerPropertyInfo> properties = node.GetDesignerProperties();
            for (int i = 0; i < properties.Count; ++i)
            {
                if (properties[i].Attribute.HasFlags(DesignerProperty.DesignerFlags.NoExport))
                    continue;
                ret += ',' + properties[i].GetExportValue(node);
            }
            if (ret.Length > 0)
                ret = ret.Substring(1);
            return ret;
        }

        public override void Export()
        {
            string folder = Path.GetDirectoryName(_outputFolder + "\\" + _filename);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            StreamWriter file = new StreamWriter(_outputFolder + "\\" + _filename);
            ExportBehavior(file, _node);
            file.Close();
        }
    }
}
