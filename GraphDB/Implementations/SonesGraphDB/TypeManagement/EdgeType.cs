/*
* sones GraphDB - Community Edition - http://www.sones.com
* Copyright (C) 2007-2011 sones GmbH
*
* This file is part of sones GraphDB Community Edition.
*
* sones GraphDB is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB. If not, see <http://www.gnu.org/licenses/>.
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using sones.GraphDB.TypeSystem;
using sones.Library.PropertyHyperGraph;
using sones.Library.LanguageExtensions;
using sones.GraphDB.TypeManagement.Base;
using sones.GraphDB.Manager.BaseGraph;
using System.Collections;

namespace sones.GraphDB.TypeManagement
{
    internal class EdgeType: BaseType, IEdgeType
    {

        #region Constants

        /// <summary>
        /// This is the initialization count of the result list of a GetChildVertices method
        /// </summary>
        private const int ExpectedChildTypes = 50;

        /// <summary>
        /// This is the initialization count of the result list of a GetAttributes method
        /// </summary>
        private const int ExpectedAttributes = 50;

        #endregion

        #region Data

        private IEnumerable<IEdgeType> _childs;
        private bool _hasChilds;

        #endregion

        #region c'tor

        public EdgeType(IVertex myVertex): base(myVertex)
        {
            _hasChilds = HasIncomingVertices(BaseTypes.EdgeType, AttributeDefinitions.EdgeTypeDotParent);
        }

        #endregion


        public override bool HasParentType
        {
            get { return ID != (long)BaseTypes.Edge; }
        }

        public override bool HasChildTypes
        {
            get { return _hasChilds; }
        }

        protected override BaseType GetParentType()
        {
            if (HasParentType)
                return new EdgeType(GetOutgoingSingleEdge(AttributeDefinitions.EdgeTypeDotParent).GetTargetVertex());

            return null;
        }

        protected override IDictionary<string, IAttributeDefinition> RetrieveAttributes()
        {
            return BaseGraphStorageManager.GetPropertiesFromFS(_vertex, this).Cast<IAttributeDefinition>().ToDictionary(x => x.Name);
        }

        #region IEdgeType Members

        public IEdgeType ParentEdgeType
        {
            get 
            {
                return GetParentType() as IEdgeType;
            }
        }

        public IEnumerable<IEdgeType> GetChildEdgeTypes(bool myRecursive = true, bool myIncludeSelf = false)
        {
            if (myIncludeSelf)
                yield return this;

            foreach (var aChildEdgeType in GetChilds())
            {
                yield return aChildEdgeType;

                if (myRecursive)
                {
                    foreach (var aVertex in aChildEdgeType.GetChildEdgeTypes(true))
                    {
                        yield return aVertex;
                    }
                }
            }

            yield break;
        }

        #endregion

        #region private methods

        private IEnumerable<IEdgeType> GetChilds()
        {
            if (_childs == null)
                lock (_lock)
                {
                    if (_childs == null)
                        _childs = RetrieveChilds();
                }

            return _childs;
        }

        private IEnumerable<IEdgeType> RetrieveChilds()
        {
            if (!HasChildTypes)
                return Enumerable.Empty<IEdgeType>();

            var vertices = GetIncomingVertices(BaseTypes.EdgeType, AttributeDefinitions.EdgeTypeDotParent);

            return vertices.Select(vertex => new EdgeType(vertex)).ToArray();
        }


        #endregion
    }
}