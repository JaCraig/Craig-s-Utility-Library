using System.Collections;
using System.Collections.Generic;

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
        /// Adds the vertex.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The new vertex</returns>
        public Vertex<T> AddVertex(T data)
        {
            Vertex<T> ReturnValue = new Vertex<T>(data);
            Vertices.Add(ReturnValue);
            return ReturnValue;
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
        public Vertex(T data)
        {
            Data = data;
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
        /// Adds an outgoing edge to the vertex specified
        /// </summary>
        /// <param name="sink">The sink.</param>
        /// <returns>The new edge</returns>
        public Edge<T> AddOutgoingEdge(Vertex<T> sink)
        {
            Edge<T> ReturnValue = new Edge<T>(this, sink);
            OutgoingEdges.Add(ReturnValue);
            sink.IncomingEdges.Add(ReturnValue);
            return ReturnValue;
        }
    }
}