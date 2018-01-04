//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace BrainiacCustomDesigner.Nodes
{
	public class Not : CustomDecorator
	{
        public Not()
            : base("ȡ���ڵ�", "���ӽڵ�Ľ������ȡ������")
		{
            className = "Not";
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);
		}
	}
}
