using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    [Serializable]
    internal sealed class AVLTree
    {
        private Node root;

        internal int GetHeight()
        {
            return Node.GetHeight(root);
        }

        public bool Search(int key)
        {
            return Search(root, key);
        }

        private static bool Search(Node node, int key)
        {
            if (node == null)
            {
                return false;
            }
            if (key > node.key)
            {
                return Search(node.Right, key);
            }
            else if (key < node.key)
            {
                return Search(node.Left, key);
            }
            else
            {
                return true;
            }
        }

        public void Insert(int key)
        {
            if (root == null)
            {
                root = new Node(key);
                return;
            }
            root = Insert(root, key);
        }

        public void Remove(int key)
        {
            root = Remove(root, key);
        }

        private static Node Remove(Node node, int key)
        {
            if (node == null)
            {
                return null;
            }
            else if (key > node.key)
            {
                node.Right = Remove(node.Right, key);
            }
            else if (key < node.key)
            {
                node.Left = Remove(node.Left, key);
            }
            else
            {
                if (node.Right == null)
                {
                    return node.Left;
                }
                Node min = node.Right.FindMin();
                min.Right = node.Right.RemoveMin();
                min.Left = node.Left;
                return DoBalance(min);
            }
            return DoBalance(node);
        }

        private static Node Insert(Node node, int key)
        {
            if (node == null)
            {
                return new Node(key);
            }
            if (key >= node.key)
            {
                node.Right = Insert(node.Right, key);
            }
            else
            {
                node.Left = Insert(node.Left, key);
            }
            return DoBalance(node);
        }

        private static Node DoBalance(Node node)
        {
            node.FixHeight();
            if (node.BalanceFactor() == 2)
            {
                if (node.Right.BalanceFactor() < 0)
                {
                    node.Right = node.Right.RightRotate();
                }
                return node.LeftRotate();
            }
            if (node.BalanceFactor() == -2)
            {
                if (node.Left.BalanceFactor() > 0)
                {
                    node.Left = node.Left.LeftRotate();
                }
                return node.RightRotate();
            }
            return node;
        }

        [Serializable]
        private class Node
        {
            public readonly int key;
            private sbyte height;
            public Node Left;
            public Node Right;

            public Node(int key)
            {
                this.key = key;
                height = 1;
            }

            public int BalanceFactor()
            {
                return GetHeight(Right) - GetHeight(Left);
            }

            public void FixHeight()
            {
                sbyte hl = GetHeight(Left);
                sbyte hr = GetHeight(Right);
                height = (sbyte)((hl > hr ? hl : hr) + 1);
            }

            public Node FindMin()
            {
                return Left != null ? Left.FindMin() : this;
            }

            public Node RemoveMin()
            {
                if (Left == null)
                {
                    return Right;
                }
                Left = Left.RemoveMin();
                return DoBalance(this);
            }

            public Node RightRotate()
            {
                Node q = Left;
                Left = q.Right;
                q.Right = this;
                this.FixHeight();
                q.FixHeight();
                return q;
            }

            public Node LeftRotate()
            {
                Node p = Right;
                Right = p.Left;
                p.Left = this;
                this.FixHeight();
                p.FixHeight();
                return p;
            }

            public static sbyte GetHeight(Node node)
            {
                return node != null ? node.height : (sbyte)0;
            }
        }
    }
}
