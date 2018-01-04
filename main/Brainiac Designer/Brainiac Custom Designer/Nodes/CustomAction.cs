//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;

namespace BrainiacCustomDesigner.Nodes
{
	public class CustomAction : Action
	{
        public string className = "Custom.AI.ActionNode";

		public CustomAction(string label, string description)
            : base(label, description)
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);

            CustomAction node = (CustomAction)newnode;
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
