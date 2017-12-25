using Brainiac.Design.Attributes;
using Brainiac.HTN.Nodes;

namespace Uprising.Nodes.PrimitiveTasks
{
	public class Wait : PrimitiveTask
	{
		public Wait() : base("Wait", "The AI will wait for a given time.")
		{
			_exportName= "HTNNode_Wait";
		}

		protected float _time;

		[DesignerFloat("Delay", "The time the AI will wait.", "CategoryBasic", DesignerProperty.DisplayMode.NoDisplay, 0, DesignerProperty.DesignerFlags.NoFlags, null, -1.0f, 60.0f, 1.0f, 1, "UnitsSeconds")]
		public float Time
		{
			get { return _time; }
			set { _time= value; }
		}
	}
}
