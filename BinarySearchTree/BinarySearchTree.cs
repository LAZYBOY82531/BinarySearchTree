using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
	internal class BinarySearchTree<T> where T : IComparable<T>
	{
		private Node root;

		public BinarySearchTree()
        {
			this.root = null;
        }

		public void Add(T item)
		{
			Node newNode = new Node(item, null, null, null);

			if (root == null)
			{
				root = newNode;
				return;
			}
			Node current = root;
			while (current != null)
			{
				if (item.CompareTo(current.item) < 0)  //비교해서 더 작으면 왼쪽으로감
				{
					if (current.left != null)                //자리가 비어있지 않다면
					{
						current = current.left;          //왼쪽자식과 비교 하기 위해 둔다
					}
					else                                           //왼쪽자식이 없다면
					{
						current.left = newNode;       //왼쪽자식이 된다.
						newNode.parent = current;
						return;
					}
				}
				else if (item.CompareTo(current.item) > 0)  //비교해서 더 크다면 오른쪽으로감
				{
					if (current.right != null)          //오른쪽자식이 있다면
					{
						current = current.right;     //오른쪽 자식과 비교를 시작한다.
					}
					else                                        //자식이 없다면
					{
						current.right = newNode;   //오른쪽 자식이 된다.
						newNode.parent = current;
						return;
					}
				}
			}
		}
		public bool TryGetValue(T item, out T outValue)
		{
			Node findNode = FindNode(item);

			if(findNode == null)
            {
				outValue = default(T);
				return false;
            }
            else
            {
				outValue = findNode.item;
				return true;
            }
        }
		private Node FindNode(T item)
		{
			if (root == null)
				return null;

			Node current = root;
			while (current != null)
			{
				if (item.CompareTo(current.item) < 0)
				{
					current = current.left;
				}
				else if (item.CompareTo(current.item) > 0)
				{
					current = current.right;
				}
				else
				{
					return current;
				}
			}
			return null;
		}
		public bool Remove(T item)
		{
			if (root == null)
				return false;

			Node findNode = FindNode(item);
			if (findNode == null)
			{
				return false;
			}
			else
			{
				EraseNode(findNode);
				return true;
			}
		}
		private void EraseNode(Node node)
		{
			//자식 노드가 없는 노드일 경우
			if (node.hasNoChild)  
			{
				if (node.isLeftChild)
					node.parent.left = null;
				else if (node.isRightChild)
					node.parent.right = null;
				else  //루트 노드일 경우
					root = null;
			}
			//자식 노드가 한개인 노드일 경우
			else if (node.hasLeftChild || node.hasRightChild) 
			{
				Node parent = node.parent;
				Node child = node.hasLeftChild ? node.left : node.right;

				if (node.isLeftChild)
				{
					parent.left = child;
					child.parent = parent;
				}
				else if (node.isRightChild)
				{
					parent.right = child;
					child.parent = parent;
				}
				else
				{
					root = child;
					child.parent = null;
				}
			}
			//자식 노드가 2개인 노드인 경우
			//왼쪽자식 중 가장 큰 값과 데이터 교환한 후 그 노드를 지워주는 방식으로 대체
			else
			{
				Node replaceNode = node.left;
				while (replaceNode.right != null)
				{
					replaceNode = replaceNode.right;
				}
				node.item = replaceNode.item;
				EraseNode(replaceNode);
				/*
				Node replaceNode = node.right;
				while (replaceNode.left != null)
				{
					replaceNode = replaceNode.left;
				}
				node.item = replaceNode.item;
				EraseNode(replaceNode);    둘 다 같음*/
			}
		}

		private class Node
		{
			public T item;
			public Node parent;
			public Node left;
			public Node right;

			public Node(T item, Node parent, Node left, Node right)
			{
				this.item = item;
				this.parent = parent;
				this.left = left;
				this.right = right;
			}
			/*public bool isRootNode { get { return parent == null; } }*/
			public bool isLeftChild { get { return left != null && parent.left == this; } }
			public bool isRightChild { get { return right != null && parent.right == this; } }
			public bool hasNoChild { get { return left == null && right == null; } }
			public bool hasLeftChild { get { return left != null && right == null; } }
			public bool hasRightChild { get { return left == null && right != null; } }
			/*public bool HasBothChild { get { return left != null && right != null; } }*/
		}
	}
}
