using Brainiac.HTN.Nodes;
using Brainiac.Design.Attributes;
using Uprising.Enums;

namespace Uprising.Nodes.Preconditions
{
	public class FindPath : Precondition
	{
		public FindPath() : base("Find Path", "Checks if a certain path exists and stores it.")
		{
		}

		protected PathID _path;

		[DesignerEnum("Path", "The id under which the found path will be stored.", "CategoryBasic", DesignerProperty.DisplayMode.Parameter, 2, DesignerProperty.DesignerFlags.NoFlags, null)]
		public PathID Path
		{
			get { return _path; }
			set { _path= value; }
		}

		protected PositionID _start= PositionID.Current;

		[DesignerEnum("Start", "The position where the path starts.", "CategoryBasic", DesignerProperty.DisplayMode.Parameter, 0, DesignerProperty.DesignerFlags.NoFlags, null)]
		public PositionID Start
		{
			get { return _start; }
			set { _start= value; }
		}

		protected PositionID _destination= PositionID.Command;

		[DesignerEnum("Destination", "The position where the path ends.", "CategoryBasic", DesignerProperty.DisplayMode.Parameter, 1, DesignerProperty.DesignerFlags.NoFlags, null)]
		public PositionID Destination
		{
			get { return _destination; }
			set { _destination= value; }
		}
	}
}
