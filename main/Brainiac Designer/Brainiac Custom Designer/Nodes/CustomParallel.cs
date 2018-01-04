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
            : base("并行节点", "所有子节点并行执行")
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
