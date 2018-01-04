//using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace BrainiacCustomDesigner.Nodes
{
    public interface Composite
    {
    }

    public class CustomParallel : Parallel, Composite
    {
        public CustomParallel()
            : base("并行节点", "所有子节点并行执行")
        {
        }

        public override string Type
        {
            get { return "Parallel"; }
        }
    }

    public class CustomSelector : Selector, Composite
    {
        public CustomSelector()
            : base("选择节点", "遇到第一个 True 则立即返回 True")
        {
        }

        public override string Type
        {
            get { return "Selector"; }
        }
    }

    public class CustomSequence : Sequence, Composite
    {
        public CustomSequence()
            : base("序列节点", "遇到第一个 False 则立即返回 False")
        {
        }

        public override string Type
        {
            get { return "Sequence"; }
        }
    }

}
