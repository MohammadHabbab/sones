/*
* sones GraphDB - Open Source Edition - http://www.sones.com
* Copyright (C) 2007-2010 sones GmbH
*
* This file is part of sones GraphDB Open Source Edition (OSE).
*
* sones GraphDB OSE is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB OSE is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB OSE. If not, see <http://www.gnu.org/licenses/>.
* 
*/

/*
 * sones GraphDS API - IVertex
 * (c) Achim 'ahzf' Friedland, 2010
 */

#region Usings

using System;
using System.Collections.Generic;

using sones.GraphFS.DataStructures;
using sones.Lib.ErrorHandling;

#endregion

namespace sones.GraphDB.NewAPI
{

    public interface IVertex : IEquatable<IVertex>
    {

        #region IVertex Properties

        ObjectUUID           UUID                   { get; set; }
        String               TYPE                   { get; }
        String               EDITION                { get; set; }
        ObjectRevisionID     REVISIONID             { get; set; }
        String               Comment                { get; set; }

        #endregion

        #region Attributes ( == Properties + Edges)

        Boolean              HasAttribute           (String myAttributeName);
        Boolean              HasAttribute           (Func<String, Boolean> myAttributeNameFilter = null);

        IEnumerable<KeyValuePair<String, Object>>
                             Attributes             (Func<String, Object, Boolean> myAttributeFilter = null);

        UInt64               Count                  { get; }

        #endregion

        #region Properties

        Boolean              HasProperty            (String myPropertyName);
        Boolean              HasProperty            (Func<String, Object, Boolean> myPropertyFilter = null);

        Object               GetProperty            (String myPropertyName);
        IEnumerable<Object>  GetProperties          (Func<String, Object, Boolean> myPropertyFilter = null);

        T                    GetProperty<T>         (String myPropertyName);

        IEnumerable<T>       GetProperties<T>       (Func<String, Object, Boolean> myPropertyFilter = null);

        String               GetStringProperty      (String myPropertyName);
        IEnumerable<String>  GetStringProperty      (Func<String, String, Boolean> myPropertyFilter = null);

        #endregion

        #region Edges

        Boolean              HasEdge                (String myEdgeName);
        Boolean              HasEdge                (Func<String, IEdge, Boolean> myEdgeFilter = null);

        IEdge                GetEdge                (String myEdgeName);
        IEnumerable<IEdge>   GetEdges               (String myEdgeName);
        IEnumerable<IEdge>   GetEdges               (Func<String, IEdge, Boolean> myEdgeFilter = null);

        TEdge                GetEdge<TEdge>         (String myEdgeName) where TEdge : class, IEdge;
        IEnumerable<TEdge>   GetEdges<TEdge>        (String myEdgeName) where TEdge : class, IEdge;
        IEnumerable<TEdge>   GetEdges<TEdge>        (Func<String, IEdge, Boolean> myEdgeFilter = null) where TEdge : class, IEdge;

        #endregion

        #region Neighbors

        IVertex              GetNeighbor            (String myEdgeName);
        IEnumerable<IVertex> GetNeighbors           (String myEdgeName);
        IEnumerable<IVertex> GetNeighbors           (Func<String, IVertex, Boolean> myVertexFilter = null);
        IEnumerable<IVertex> GetNeighbors           (Func<String, IEdge,   Boolean> myEdgeFilter   = null);

        TVertex              GetNeighbor<TVertex>   (String myEdgeName) where TVertex : class, IVertex;
        IEnumerable<TVertex> GetNeighbors<TVertex>  (String myEdgeName) where TVertex : class, IVertex;
        IEnumerable<TVertex> GetNeighbors<TVertex>  (Func<String, IVertex, Boolean> myVertexFilter = null) where TVertex : class, IVertex;
        IEnumerable<TVertex> GetNeighbors<TVertex>  (Func<String, IEdge,   Boolean> myEdgeFilter   = null) where TVertex : class, IVertex;

        #endregion

        #region Link/Unlink

        Exceptional Link     (IVertex              myTargetVertex);
        Exceptional Link     (params IVertex[]     myTargetVertices);
        Exceptional Link     (IEnumerable<IVertex> myTargetVertices);


        Exceptional Unlink   (IVertex              myTargetVertex);
        Exceptional Unlink   (params IVertex[]     myTargetVertices);
        Exceptional Unlink   (IEnumerable<IVertex> myTargetVertices);

        #endregion

        #region Traverse

        T TraversePath<T>   (TraversalOperation                    TraversalOperation  = TraversalOperation.BreathFirst,
                             Func<DBPath, IEdge, Boolean>          myFollowThisEdge    = null,
                             Func<DBPath, IEdge, IVertex, Boolean> myFollowThisPath    = null,
                             Func<DBPath, Boolean>                 myMatchEvaluator    = null,
                             Action<DBPath>                        myMatchAction       = null,
                             Func<TraversalState, Boolean>         myStopEvaluator     = null,
                             Func<IEnumerable<DBPath>, T>          myWhenFinished      = null);

        T TraverseVertex<T> (TraversalOperation                    TraversalOperation  = TraversalOperation.BreathFirst,
                             Func<IVertex, IEdge, Boolean>         myFollowThisEdge    = null,
                             Func<IVertex, Boolean>                myMatchEvaluator    = null,
                             Action<IVertex>                       myMatchAction       = null,
                             Func<TraversalState, Boolean>         myStopEvaluator     = null,
                             Func<IEnumerable<IVertex>, T>         myWhenFinished      = null);

        #endregion
 
    }

}