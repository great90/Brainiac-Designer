//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;
using BrainiacCustomDesigner.Nodes;
using System.Reflection;

namespace BrainiacCustomDesigner.Exporters
{
    public class LuaExporter : Brainiac.Design.Exporters.Exporter
    {
        public LuaExporter(BehaviorNode node, string outputFolder, string filename)
            : base(node, outputFolder, filename + ".lua")
        {
        }

        public override void Export()
        {
            string folder = Path.GetDirectoryName(_outputFolder + "\\" + _filename);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            StreamWriter file = new StreamWriter(_outputFolder + "\\" + _filename);
            //ExportBehavior(file, _node);
            file.WriteLine("return {");
            ExportNode(file, (Node)_node, "\t");
            file.WriteLine("}");
            file.Close();
        }

        protected void ExportNode(StreamWriter file, Node node, string linePrefix = "")
        {
            int id = node.ID;
            file.WriteLine("{0}[{1}] = {{", linePrefix, node.ID);
            file.WriteLine("{0}\tID = {1},", linePrefix, node.ID);
            if (node.Parent != null)
                file.WriteLine("{0}\tParent = {1},", linePrefix, node.Parent.ID);
            file.WriteLine("{0}\tType = \"{1}\",", linePrefix, node.Type);
            bool isCompositeNode = (node is Selector || node is Sequence || node is Parallel);
            if (node.Children.Count > 0)
            {
                file.Write("{0}\tChildren = {{", linePrefix);
                string children = "";
                foreach (Node child in node.Children)
                    children += child.ID + ", ";
                if (children.EndsWith(", "))
                    children = children.Remove(children.Length - 2);
                file.WriteLine(children + "},");
            }
            ExportNodeProperties(file, node, linePrefix + "\t");
            file.WriteLine(linePrefix + "},");
            foreach (Node child in node.Children)
                ExportNode(file, child, "\t");
        }

        protected void ExportNodeProperties(StreamWriter file, Node node, string linePrefix = "")
        {
            System.Console.WriteLine("===========================================");
            foreach (DesignerPropertyInfo info in node.GetDesignerProperties())
                System.Console.WriteLine("{0} : {1}", info.Property.Name, info.GetValue(node));
            System.Console.WriteLine("-------------------------------------------");
            foreach (string s in node.GetNodePropertyExcludedProperties())
                System.Console.WriteLine(s);
            System.Console.WriteLine("-------------------------------------------");

            /*System.Type type = node.GetType();
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                if (info.DeclaringType == type)
                    System.Console.WriteLine("{0} : {1} {2} {3}", info.Name, info.GetValue(node, null), info.DeclaringType, type);
            */
            foreach (System.Reflection.FieldInfo fi in node.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                System.Console.WriteLine("{0} : {1} {2} {3}", fi.Name, fi.GetValue(node), fi.DeclaringType, node.GetType());
             
            System.Console.WriteLine("===========================================");
            IList<DesignerPropertyInfo> properties = new List<DesignerPropertyInfo>();
            foreach (DesignerPropertyInfo info in node.GetDesignerProperties())
            {
                if (!info.Attribute.HasFlags(DesignerProperty.DesignerFlags.NoExport))
                    properties.Add(info);
            }
            if (properties.Count > 0)
            {
                file.WriteLine("{0}Params = {{", linePrefix);
                foreach (DesignerPropertyInfo info in properties)
                    file.WriteLine("{0}\t{1} = {2},\t-- {3}", linePrefix, info.Property.Name, info.GetExportValue(node), info.Attribute.DisplayName);
                file.WriteLine(linePrefix + "},");
            }
        }

    }
}
