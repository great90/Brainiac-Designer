using System;
using System.Collections.Generic;
using System.Text;

using Brainiac.Design;
using BrainiacCustomDesigner.Properties;
using System.Reflection;

namespace BrainiacCustomDesigner
{
    public class BrainiacCustomDesigner : Brainiac.Design.Plugin
    {
        public BrainiacCustomDesigner()
        {
            AddResourceManager(Resources.ResourceManager);  // register resource manager
            _fileManagers.Add(new FileManagerInfo(typeof(Brainiac.Design.FileManagers.FileManagerXML), "XML文件 (*.xml)|*.xml", ".xml"));
            NewNodeGroup("行为节点", NodeIcon.Action, typeof(Brainiac.Design.Nodes.Action));
            NewNodeGroup("条件节点", NodeIcon.Condition, typeof(Brainiac.Design.Nodes.Condition));
            NewNodeGroup("装饰节点", NodeIcon.Decorator, typeof(Brainiac.Design.Nodes.Decorator));
            NewNodeGroup("组合节点", NodeIcon.CompoundTask, typeof(Nodes.Composite));
            _exporters.Add(new ExporterInfo(typeof(Exporters.LuaExporter), "导出Lua文件", true, "CustomLuaExporter"));
        }

        private void NewNodeGroup(string name, NodeIcon icon, Type baseType)
        {
            NodeGroup group = new NodeGroup(name, icon, null);
            _nodeGroups.Add(group);
            foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !baseType.IsAssignableFrom(type))
                    continue;
                group.Items.Add(type);
            }
        }
    }
}
