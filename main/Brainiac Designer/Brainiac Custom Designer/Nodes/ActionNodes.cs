//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace BrainiacCustomDesigner.Nodes
{
	public class Move : CustomAction
	{
        protected int steps = 0;
        [DesignerInteger("移动步数", "移动指定步数后停顿", "Basic", DesignerProperty.DisplayMode.Parameter, 0, DesignerProperty.DesignerFlags.NoFlags, "Steps", 0, 1000, 1, "steps")]
        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public Move()
            : base("移动节点", "移动至目标点")
		{
            className = "Move";
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);
            Move node = (Move)newnode;
            node.steps = steps;
		}
	}
}
