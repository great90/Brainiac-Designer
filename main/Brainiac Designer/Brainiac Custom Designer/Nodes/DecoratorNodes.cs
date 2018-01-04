//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace BrainiacCustomDesigner.Nodes
{
	public class Not : Decorator
	{
        public Not()
            : base("取反节点", "对子节点的结果进行取反运算")
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);
		}
	}
}
