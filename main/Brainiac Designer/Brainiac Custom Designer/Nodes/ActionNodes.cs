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
        [DesignerInteger("�ƶ�����", "�ƶ�ָ��������ͣ��", "Basic", DesignerProperty.DisplayMode.Parameter, 0, DesignerProperty.DesignerFlags.NoFlags, "Steps", 0, 1000, 1, "steps")]
        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public Move()
            : base("�ƶ��ڵ�", "�ƶ���Ŀ���")
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
        [DesignerInteger("����ʱ��", "����ָ������", "Basic", DesignerProperty.DisplayMode.Parameter, 0, DesignerProperty.DesignerFlags.NoFlags, "Second", 0, 1000, 1, "s")]
        public int Second
        {
            get { return second; }
            set { second = value; }
        }

        public Sleep()
            : base("���߽ڵ�", "����ָ������")
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
