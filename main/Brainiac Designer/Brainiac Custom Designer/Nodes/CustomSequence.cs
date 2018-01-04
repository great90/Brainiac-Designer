//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;

namespace BrainiacCustomDesigner.Nodes
{
	public class CustomSequence : Sequence
	{
        public string className = "SequenceNode";

		public CustomSequence()
            : base("序列节点", "遇到第一个 False 则立即返回 False")
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);
            CustomSequence node = (CustomSequence)newnode;
            node.className = className;
		}

        public override string ExportClass
        {
            get { return className; }
        }
	}
}
