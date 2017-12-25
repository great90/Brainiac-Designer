////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2009, Daniel Kollmann
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list of conditions
//   and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list of
//   conditions and the following disclaimer in the documentation and/or other materials provided
//   with the distribution.
//
// - Neither the name of Daniel Kollmann nor the names of its contributors may be used to endorse
//   or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.Nodes;
using System.Drawing;

namespace Brainiac.Design
{
	/// <summary>
	/// Special NodeViewData for referenced behaviours
	/// </summary>
	public class NodeViewDataReferencedBehavior : NodeViewDataStyled
	{
		public NodeViewDataReferencedBehavior(NodeViewData parent, Nodes.BehaviorNode rootBehavior, Nodes.ReferencedBehaviorNode node, Pen borderPen, Brush backgroundBrush, string label, string description) :
			base(parent, rootBehavior, (Node)node, borderPen, backgroundBrush, label, description)
		{
		}

		/// <summary>
		/// Used to filter multiple requests to rebuild the subgraph so this happens only once.
		/// </summary>
		protected bool _subGraphNeedsToBeRebuilt= true;

		protected bool _isExpanded= false;

		/// <summary>
		/// Determines if the node is expanded or not.
		/// </summary>
		public bool IsExpanded
		{
			get { return _isExpanded; }

			set
			{
				_subGraphNeedsToBeRebuilt= true;
				_isExpanded= value;
			}
		}

		protected override bool NeedsToSynchronizeWithNode()
		{
			return _subGraphNeedsToBeRebuilt || base.NeedsToSynchronizeWithNode();
		}

		public override void DoSynchronizeWithNode(ProcessedBehaviors processedBehaviors)
		{
			// make all connectors changable
			for(int i= 0; i <_children.Connectors.Count; ++i)
				_children.Connectors[i].IsReadOnly= false;

			if(_isExpanded)
			{
				base.DoSynchronizeWithNode(processedBehaviors);
			}
			else
			{
				_subGraphNeedsToBeRebuilt= false;

				_children.ClearConnectors();
				RemoveAllConnectorSubItems();

				//foreach(Connector connector in _node.Connectors)
				//	connector.Clone(_children);

				new ConnectorMultiple(_children, string.Empty, "GenericChildren", 1, int.MaxValue);

				// generate the dummy nodes for the sub-referenced behaviours
				foreach(Node child in ((Node)((ReferencedBehaviorNode)_node).Reference).Children)
					GenerateReferencedBehaviorsTree(processedBehaviors, this, child);

				// make all connectors readonly
				for(int i= 0; i <_children.Connectors.Count; ++i)
					_children.Connectors[i].IsReadOnly= true;

				GenerateNewLabel();
			}
		}

		/// <summary>
		/// This function adapts the children of the view that they represent the children of the node this view is for.
		/// Children are added and removed.
		/// </summary>
		/// <param name="processedBehaviors">A list of previously processed behaviours to deal with circular references.</param>
		public override void SynchronizeWithNode(ProcessedBehaviors processedBehaviors)
		{
			// if we have a circular reference, we must skip it
			if(!processedBehaviors.MayProcessCheckOnly(_node))
			{
				_children.ClearChildren();
				return;
			}

			base.SynchronizeWithNode(processedBehaviors);
		}

		/// <summary>
		/// Returns the first NodeViewData which is associated with the given node. Notice that there might be other NodeViewDatas which are ignored.
		/// </summary>
		/// <param name="node">The node you want to get the NodeViewData for.</param>
		/// <returns>Returns the first NodeViewData found.</returns>
		public override NodeViewData FindNodeViewData(Node node)
		{
			if(node is ReferencedBehaviorNode)
			{
				ReferencedBehaviorNode refnode= (ReferencedBehaviorNode) _node;
				ReferencedBehaviorNode refnode2= (ReferencedBehaviorNode) node;

				if(refnode.Reference ==refnode2.Reference)
					return this;
			}

			return base.FindNodeViewData(node);
		}

		/// <summary>
		/// Returns if any of the node's parents is a given behaviour.
		/// </summary>
		/// <param name="behavior">The behavior we want to check if it is an ancestor of this node.</param>
		/// <returns>Returns true if this node is a descendant of the given behavior.</returns>
		public override bool HasParentBehavior(BehaviorNode behavior)
		{
			if(behavior ==null)
				return false;

			ReferencedBehaviorNode refb= (ReferencedBehaviorNode)_node;
			Debug.Check(refb.Reference !=null);

			if(refb.Reference ==behavior)
				return true;

			if(Parent ==null)
				return false;

			return Parent.HasParentBehavior(behavior);
		}

		/// <summary>
		/// Adds nodes to the referenced behaviour which represent sub-referenced behaviours.
		/// </summary>
		/// <param name="processedBehaviors">A list of processed behaviours to handle circular references.</param>
		/// <param name="parent">The node the sub-referenced behaviours will be added to.</param>
		/// <param name="node">The current node we are checking.</param>
		protected void GenerateReferencedBehaviorsTree(ProcessedBehaviors processedBehaviors, NodeViewData parent, Node node)
		{
			if(!processedBehaviors.MayProcess(node))
				return;

			// check if this is a referenced behaviour
			if(node is ReferencedBehaviorNode)
			{
				// create the dummy node and add it without marking the behaviour as being modified as these are no REAL nodes.
				NodeViewData rb= node.CreateNodeViewData(parent, _rootBehavior);

#if DEBUG
				rb.IsSubreferencedGraphNode();
#endif
				rb.DoSynchronizeWithNode(processedBehaviors);

				Connector conn= parent.GetConnector("GenericChildren");
				Debug.Check(conn !=null);

				Connector rbconn= parent.GetConnector("GenericChildren");
				Debug.Check(rbconn !=null);

				bool parentReadOnly= conn.IsReadOnly;

				conn.IsReadOnly= false;

				parent.AddChildNotModified(conn, rb);

				conn.IsReadOnly= parentReadOnly;

				// we have a circular reference here. Skip the children
				if(((ReferencedBehaviorNode)node).Reference ==_rootBehavior)
				{
					rbconn.IsReadOnly= true;
					return;
				}

				// do the same for all the children
				foreach(Node child in node.Children)
					GenerateReferencedBehaviorsTree(processedBehaviors.Branch(child), rb, child);

				rbconn.IsReadOnly= true;
			}
			else if(node is Impulse)
			{
				// create the dummy node and add it without marking the behaviour as being modified as these are no REAL nodes.
				NodeViewData ip= node.CreateNodeViewData(parent, _rootBehavior);

				ip.DoSynchronizeWithNode(processedBehaviors);

				// do the same for all the children
				foreach(Node child in node.Children)
					GenerateReferencedBehaviorsTree(processedBehaviors.Branch(child), ip, child);

				if(ip.Children.Count >0)
				{
					Connector conn= parent.GetConnector("GenericChildren");
					Debug.Check(conn !=null);

					Connector ipconn= ip.GetConnector("GenericChildren");
					Debug.Check(ipconn !=null);

					parent.AddChildNotModified(conn, ip);

					ipconn.IsReadOnly= true;
				}
			}
			else
			{
				// do the same for all the children
				foreach(Node child in node.Children)
					GenerateReferencedBehaviorsTree(processedBehaviors.Branch(child), parent, child);
			}
		}

		/// <summary>
		/// Is called when the node was double-clicked. Used for referenced behaviours.
		/// </summary>
		/// <param name="nvd">The view data of the node in the current view.</param>
		/// <param name="layoutChanged">Does the layout need to be recalculated?</param>
		/// <returns>Returns if the node handled the double click or not.</returns>
		public override bool OnDoubleClick(NodeViewData nvd, out bool layoutChanged)
		{
			NodeViewDataReferencedBehavior nvdrb= (NodeViewDataReferencedBehavior)nvd;
			nvdrb.IsExpanded= !nvdrb.IsExpanded;

			layoutChanged= true;
			return true;
		}
	}
}
