using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Edge pointing from vertex source to vertex sink
    /// </summary>
    /// <typeparam name="T">Data type of the data</typeparam>
    public class Edge<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sink">The sink.</param>
        public Edge(Vertex<T> source, Vertex<T> sink)
        {
            Source = source;
            Sink = sink;
        }

        /// <summary>
        /// Gets the sink vertex.
        /// </summary>
        /// <value>The sink vertex.</value>
        public Vertex<T> Sink { get; private set; }

        /// <summary>
        /// Gets the source vertex
        /// </summary>
        /// <value>The source vertex</value>
        public Vertex<T> Source { get; private set; }

        /// <summary>
        /// Removes this edge from the sink and source vertices.
        /// </summary>
        /// <returns>This</returns>
        public Edge<T> Remove()
        {
            Sink.RemoveEdge(this);
            Source.RemoveEdge(this);
            Sink = null;
            Source = null;
            return this;
        }
    }

    /// <summary>
    /// Class used to represent a graph
    /// </summary>
    /// <typeparam name="T">The data type stored in the graph</typeparam>
    public class Graph<T> : IEnumerable<Vertex<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{T}"/> class.
        /// </summary>
        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public List<Vertex<T>> Vertices { get; private set; }

        /// <summary>
        /// Adds the edge.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sink">The sink.</param>
        /// <returns>The new edge</returns>
        public Edge<T> AddEdge(Vertex<T> source, Vertex<T> sink)
        {
            return source.AddOutgoingEdge(sink);
        }

        /// <summary>
        /// Adds the vertex.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The new vertex</returns>
        public Vertex<T> AddVertex(T data)
        {
            var ReturnValue = new Vertex<T>(data, this);
            Vertices.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>A copy of this graph</returns>
        public Graph<T> Copy()
        {
            var Result = new Graph<T>();
            foreach (var Vertex in Vertices)
            {
                Result.AddVertex(Vertex.Data);
            }
            foreach (var Vertex in Vertices)
            {
                var TempSource = Result.Vertices.First(x => x.Data.Equals(Vertex.Data));
                foreach (var Edge in Vertex.OutgoingEdges)
                {
                    var TempSink = Result.Vertices.First(x => x.Data.Equals(Edge.Sink.Data));
                    Result.AddEdge(TempSource, TempSink);
                }
            }
            return Result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Vertex<T>> GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        /// <summary>
        /// Removes the vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>This</returns>
        public Graph<T> RemoveVertex(Vertex<T> vertex)
        {
            vertex.Remove();
            return this;
        }
    }

    /// <summary>
    /// Vertex within the graph
    /// </summary>
    /// <typeparam name="T">Data type saved in the vertex</typeparam>
    public class Vertex<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="graph">The graph.</param>
        public Vertex(T data, Graph<T> graph)
        {
            Data = data;
            Graph = graph;
            IncomingEdges = new List<Edge<T>>();
            OutgoingEdges = new List<Edge<T>>();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets the incoming edges.
        /// </summary>
        /// <value>The incoming edges.</value>
        public List<Edge<T>> IncomingEdges { get; private set; }

        /// <summary>
        /// Gets the outgoing edges.
        /// </summary>
        /// <value>The outgoing edges.</value>
        public List<Edge<T>> OutgoingEdges { get; private set; }

        /// <summary>
        /// Gets or sets the graph.
        /// </summary>
        /// <value>The graph.</value>
        private Graph<T> Graph { get; set; }

        /// <summary>
        /// Adds an outgoing edge to the vertex specified
        /// </summary>
        /// <param name="sink">The sink.</param>
        /// <returns>The new edge</returns>
        public Edge<T> AddOutgoingEdge(Vertex<T> sink)
        {
            var ReturnValue = new Edge<T>(this, sink);
            OutgoingEdges.Add(ReturnValue);
            sink.IncomingEdges.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Removes all edges from this vertex and removes it from the graph.
        /// </summary>
        /// <returns>This</returns>
        public Vertex<T> Remove()
        {
            IncomingEdges.ForEach(x => x.Remove());
            OutgoingEdges.ForEach(x => x.Remove());
            IncomingEdges.Clear();
            OutgoingEdges.Clear();
            Graph.Vertices.Remove(this);
            return this;
        }

        /// <summary>
        /// Removes the edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>This</returns>
        public Vertex<T> RemoveEdge(Edge<T> edge)
        {
            if (edge.Sink == this)
                IncomingEdges.Remove(edge);
            else
                OutgoingEdges.Remove(edge);
            return this;
        }
    }
}