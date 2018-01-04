//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;

namespace BrainiacCustomDesigner.Nodes
{
	public class CustomCondition : Condition
	{
        public string className = "Custom.AI.ConditionNode";

        public CustomCondition(string label, string description)
            : base(label, description)
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);

            CustomCondition node = (CustomCondition)newnode;
            node.className = className;
		}

        public override string ExportClass
        {
            get
            {
                return className;
            }
        }
	}
}
