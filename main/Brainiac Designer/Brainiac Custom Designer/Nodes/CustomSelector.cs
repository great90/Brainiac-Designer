//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;

namespace BrainiacCustomDesigner.Nodes
{
	public class CustomSelector : Selector
	{
        public string className = "SelectorNode";

		public CustomSelector()
            : base("ѡ��ڵ�", "������һ�� True ���������� True")
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);

            CustomSelector node = (CustomSelector)newnode;
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
