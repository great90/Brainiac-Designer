//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;

namespace BrainiacCustomDesigner.Nodes
{
	public class CustomParallel : Parallel
	{
        public string className = "ParallelNode";

		public CustomParallel()
            : base("���нڵ�", "�����ӽڵ㲢��ִ��")
		{
		}

		protected override void CloneProperties(Node newnode)
		{
			base.CloneProperties(newnode);

            CustomParallel node = (CustomParallel)newnode;
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
