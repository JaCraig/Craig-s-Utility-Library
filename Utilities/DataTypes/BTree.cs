/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Binary tree
    /// </summary>
    /// <typeparam name="T">The type held by the nodes</typeparam>
    public class BinaryTree<T> : ICollection<T> where T : IComparable<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Root">Root of the binary tree</param>
        public BinaryTree(TreeNode<T> Root=null)
        {
            if (Root == null)
            {
                NumberOfNodes = 0;
                return;
            }
            this.Root = Root;
            foreach (TreeNode<T> Node in Traversal(Root))
            {
                ++NumberOfNodes;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// The root value
        /// </summary>
        public virtual TreeNode<T> Root { get; set; }

        /// <summary>
        /// The number of nodes in the tree
        /// </summary>
        protected virtual int NumberOfNodes { get; set; }

        /// <summary>
        /// Is the tree empty
        /// </summary>
        public virtual bool IsEmpty { get { return Root == null; } }

        /// <summary>
        /// Gets the minimum value of the tree
        /// </summary>
        public virtual T MinValue
        {
            get
            {
                if (IsEmpty)
                    throw new Exception("The tree is empty");
                TreeNode<T> TempNode = Root;
                while (TempNode.Left != null)
                    TempNode = TempNode.Left;
                return TempNode.Value;
            }
        }

        /// <summary>
        /// Gets the maximum value of the tree
        /// </summary>
        public virtual T MaxValue
        {
            get
            {
                if (IsEmpty)
                    throw new Exception("The tree is empty");
                TreeNode<T> TempNode = Root;
                while (TempNode.Right != null)
                    TempNode = TempNode.Right;
                return TempNode.Value;
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (TreeNode<T> TempNode in Traversal(Root))
            {
                yield return TempNode.Value;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (TreeNode<T> TempNode in Traversal(Root))
            {
                yield return TempNode.Value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public virtual void Add(T item)
        {
            if (Root == null)
            {
                Root = new TreeNode<T>(item);
                ++NumberOfNodes;
            }
            else
            {
                Insert(item);
            }
        }

        public virtual void Clear()
        {
            Root = null;
            NumberOfNodes = 0;
        }

        public virtual bool Contains(T item)
        {
            if (IsEmpty)
                return false;

            TreeNode<T> TempNode = Root;
            while (TempNode != null)
            {
                int ComparedValue = TempNode.Value.CompareTo(item);
                if (ComparedValue == 0)
                    return true;
                else if (ComparedValue < 0)
                    TempNode = TempNode.Left;
                else
                    TempNode = TempNode.Right;
            }
            return false;
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            T[] TempArray = new T[NumberOfNodes];
            int Counter = 0;
            foreach (T Value in this)
            {
                TempArray[Counter] = Value;
                ++Counter;
            }
            Array.Copy(TempArray, 0, array, arrayIndex, this.NumberOfNodes);
        }

        public virtual int Count
        {
            get { return NumberOfNodes; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            TreeNode<T> Item = Find(item);
            if (Item == null)
                return false;
            --NumberOfNodes;
            List<T> Values = new List<T>();
            foreach (TreeNode<T> TempNode in Traversal(Item.Left))
                Values.Add(TempNode.Value);
            foreach (TreeNode<T> TempNode in Traversal(Item.Right))
                Values.Add(TempNode.Value);
            if (Item.Parent != null)
            {
                if (Item.Parent.Left == Item)
                    Item.Parent.Left = null;
                else
                    Item.Parent.Right = null;
                Item.Parent = null;
            }
            else
            {
                Root = null;
            }
            foreach (T Value in Values)
                this.Add(Value);
            return true;
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Finds a specific object
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <returns>The node if it is found</returns>
        protected virtual TreeNode<T> Find(T item)
        {
            foreach (TreeNode<T> Item in Traversal(Root))
                if (Item.Value.Equals(item))
                    return Item;
            return null;
        }

        /// <summary>
        /// Traverses the list
        /// </summary>
        /// <param name="Node">The node to start the search from</param>
        /// <returns>The individual items from the tree</returns>
        protected virtual IEnumerable<TreeNode<T>> Traversal(TreeNode<T> Node)
        {
            if (Node != null)
            {
                if (Node.Left != null)
                {
                    foreach (TreeNode<T> LeftNode in Traversal(Node.Left))
                        yield return LeftNode;
                }
                yield return Node;
                if (Node.Right != null)
                {
                    foreach (TreeNode<T> RightNode in Traversal(Node.Right))
                        yield return RightNode;
                }
            }
        }

        /// <summary>
        /// Inserts a value
        /// </summary>
        /// <param name="item">item to insert</param>
        protected virtual void Insert(T item)
        {
            TreeNode<T> TempNode = Root;
            bool Found = false;
            while (!Found)
            {
                int ComparedValue = TempNode.Value.CompareTo(item);
                if (ComparedValue > 0)
                {
                    if (TempNode.Left == null)
                    {
                        TempNode.Left = new TreeNode<T>(item, TempNode);
                        ++NumberOfNodes;
                        return;
                    }
                    else
                    {
                        TempNode = TempNode.Left;
                    }
                }
                else if (ComparedValue < 0)
                {
                    if (TempNode.Right == null)
                    {
                        TempNode.Right = new TreeNode<T>(item, TempNode);
                        ++NumberOfNodes;
                        return;
                    }
                    else
                    {
                        TempNode = TempNode.Right;
                    }
                }
                else
                {
                    TempNode = TempNode.Right;
                }
            }
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            StringBuilder Builder=new StringBuilder();
            foreach (T Value in this)
                Builder.Append(Value.ToString() + " ");
            return Builder.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Node class for the Binary tree
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class TreeNode<T>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value of the node</param>
        /// <param name="Parent">Parent node</param>
        /// <param name="Left">Left node</param>
        /// <param name="Right">Right node</param>
        public TreeNode(T Value = default(T), TreeNode<T> Parent = null, TreeNode<T> Left = null, TreeNode<T> Right = null)
        {
            this.Value = Value;
            this.Right = Right;
            this.Left = Left;
            this.Parent = Parent;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Value of the node
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// Parent node
        /// </summary>
        public virtual TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Left node
        /// </summary>
        public virtual TreeNode<T> Left { get; set; }

        /// <summary>
        /// Right node
        /// </summary>
        public virtual TreeNode<T> Right { get; set; }

        /// <summary>
        /// Is this the root
        /// </summary>
        public virtual bool IsRoot { get { return Parent == null; } }

        /// <summary>
        /// Is this a leaf
        /// </summary>
        public virtual bool IsLeaf { get { return Left == null && Right == null; } }

        internal bool Visited { get; set; }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion
    }
}