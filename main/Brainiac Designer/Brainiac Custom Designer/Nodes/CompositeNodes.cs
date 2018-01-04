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
            : base("���нڵ�", "�����ӽڵ㲢��ִ��")
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
            : base("ѡ��ڵ�", "������һ�� True ���������� True")
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
            : base("���нڵ�", "������һ�� False ���������� False")
        {
        }

        public override string Type
        {
            get { return "Sequence"; }
        }
    }

}
