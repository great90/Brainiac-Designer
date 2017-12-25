////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2011, Daniel Kollmann
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
using Brainiac.Design;
using Uprising.Properties;
using HTNResources = Brainiac.HTN.Properties.Resources;

namespace Uprising
{
	public class Uprising : Plugin
	{
		public Uprising()
		{
			AllowReferencedBehaviors= false;

			SetUsedBehaviorNodeType(typeof(Brainiac.HTN.Nodes.Method));
			SetUsedReferencedBehaviorNodeType(typeof(Brainiac.HTN.Nodes.ReferencedMethod));

			Brainiac.Design.Attributes.DesignerEnum.ExportTextMode= Brainiac.Design.Attributes.DesignerEnum.ExportMode.Type_Value;
			Brainiac.Design.Attributes.DesignerFloat.ExportAppendix= string.Empty;

			AddResourceManager(Resources.ResourceManager);

			RegisterAdditionalAssembly("Brainiac.HTN.dll");

			_fileManagers.Add( new FileManagerInfo(typeof(Brainiac.Design.FileManagers.FileManagerXML), "HTN XML (*.xml)|*.xml", ".xml") );

			// primitive tasks
			NodeGroup primitiveTasks= new NodeGroup(HTNResources.NodeGroupPrimitiveTasks, NodeIcon.PrimitiveTask, null);
			_nodeGroups.Add(primitiveTasks);

			primitiveTasks.Items.Add(typeof(Nodes.PrimitiveTasks.Wait));
			primitiveTasks.Items.Add(typeof(Nodes.PrimitiveTasks.FollowPath));

			// compound tasks
			NodeGroup compoundTasks= new NodeGroup(HTNResources.NodeGroupCompoundTasks, NodeIcon.CompoundTask, null);
			_nodeGroups.Add(compoundTasks);

			compoundTasks.Items.Add(typeof(Brainiac.HTN.Nodes.CompoundTask));

			// methods
			NodeGroup methods= new NodeGroup(HTNResources.NodeGroupMethods, NodeIcon.Method, null);
			_nodeGroups.Add(methods);

			// preconditions
			NodeGroup preconditions= new NodeGroup(HTNResources.NodeGroupPreconditions, NodeIcon.Precondition, null);
			_nodeGroups.Add(preconditions);

			preconditions.Items.Add(typeof(Nodes.Preconditions.HasEnemies));
			preconditions.Items.Add(typeof(Nodes.Preconditions.FindPath));

			// operators
			NodeGroup operators= new NodeGroup(HTNResources.NodeGroupOperators, NodeIcon.Operator, null);
			_nodeGroups.Add(operators);

			operators.Items.Add(typeof(Brainiac.HTN.Nodes.And));
			operators.Items.Add(typeof(Brainiac.HTN.Nodes.Or));
			operators.Items.Add(typeof(Brainiac.HTN.Nodes.Any));
			operators.Items.Add(typeof(Brainiac.HTN.Nodes.If));

			// overrides
			NodeGroup overrides= new NodeGroup(HTNResources.NodeGroupOverrides, NodeIcon.Override, null);
			_nodeGroups.Add(overrides);

			overrides.Items.Add(typeof(Brainiac.Design.Attachments.Overrides.OverrideRandom));

			// register the exporter
			_exporters.Add( new ExporterInfo(typeof(Exporters.ExporterJavaScript), "JavaScript", true, "js") );
		}
	}
}
