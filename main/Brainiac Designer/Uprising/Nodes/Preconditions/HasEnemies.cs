using Brainiac.HTN.Nodes;

namespace Uprising.Nodes.Preconditions
{
	public class HasEnemies : Precondition
	{
		public HasEnemies() : base("Has Enemies", "Is true when the AI has any known enemies")
		{
		}
	}
}
