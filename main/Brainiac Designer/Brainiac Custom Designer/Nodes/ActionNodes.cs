//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace BrainiacCustomDesigner.Nodes
{
	public class Move : Action
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
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);
            Move node = (Move)newnode;
            node.steps = steps;
		}
	}

    public class Sleep : Action
    {
        protected int second = 0;
        [DesignerInteger("休眠时间", "休眠指定秒数", "Basic", DesignerProperty.DisplayMode.Parameter, 0, DesignerProperty.DesignerFlags.NoFlags, "Second", 0, 1000, 1, "s")]
        public int Second
        {
            get { return second; }
            set { second = value; }
        }

        public Sleep()
            : base("休眠节点", "休眠指定秒数")
        {
        }

        protected override void CloneProperties(Node newnode)
        {
            base.CloneProperties(newnode);
            Sleep node = (Sleep)newnode;
            node.second = second;
        }
    }
}
