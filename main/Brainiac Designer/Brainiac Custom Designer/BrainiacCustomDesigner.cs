using System;
using System.Collections.Generic;
using System.Text;

using Brainiac.Design;
using BrainiacCustomDesigner.Properties;

namespace BrainiacCustomDesigner
{
    public class BrainiacCustomDesigner : Brainiac.Design.Plugin
    {
        public BrainiacCustomDesigner()
        {
            AddResourceManager(Resources.ResourceManager);  // register resource manager
            _fileManagers.Add(new FileManagerInfo(typeof(Brainiac.Design.FileManagers.FileManagerXML), "XML文件 (*.xml)|*.xml", ".xml"));
            InitActions();
            InitConditions();
            InitDecorators();
            InitContols();
            _exporters.Add(new ExporterInfo(typeof(Exporters.ExporterCustomLua), "导出Lua文件", true, "CustomLuaExporter"));
        }

        private void InitActions()
        {
            NodeGroup group = new NodeGroup("行为节点", NodeIcon.Action, null);
            _nodeGroups.Add(group);
            group.Items.Add(typeof(Nodes.Move));
        }

        private void InitConditions()
        {
            NodeGroup group = new NodeGroup("条件节点", NodeIcon.Condition, null);
            _nodeGroups.Add(group);
            group.Items.Add(typeof(Nodes.Probability));
        }

        private void InitDecorators()
        {
            NodeGroup group = new NodeGroup("装饰节点", NodeIcon.Decorator, null);
            _nodeGroups.Add(group);
            group.Items.Add(typeof(Nodes.Not));
        }

        private void InitContols()
        {
            NodeGroup group = new NodeGroup("控制节点", NodeIcon.Behavior, null);
            _nodeGroups.Add(group);
            group.Items.Add(typeof(Nodes.CustomSequence));
            group.Items.Add(typeof(Nodes.CustomSelector));
            group.Items.Add(typeof(Nodes.CustomParallel));
        }
    }
}
