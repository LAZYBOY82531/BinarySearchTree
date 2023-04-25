using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
	internal class MyBinarySearchTree<T> where T : IComparable<T>
	{
		private Node root;

		public MyBinarySearchTree()
		{
			this.root = null;
		}

		public void Add(T item)                                              //값을 추가하는 함수
		{
			Node newNode = new Node(item, null, null, null);

			if (root == null)                                        //트리가 비어있다면
			{
				root = newNode;
				return;
			}
			Node current = root;
			while (current != null)                               //맞는 자리를 찾는 반복을 함
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
		public bool TryGetValue(T item, out T outValue)      //자리에 있는 값을 반환하는 함수
		{
			Node findNode = FindNode(item);        //값을 찾는 함수를 실행

			if (findNode == null)                            //값이 없다면 false
			{
				outValue = default(T);
				return false;
			}
			else                                                    //값이 있다면 true
			{
				outValue = findNode.item;
				return true;
			}
		}
		private Node FindNode(T item)             //값을 찾는 함수
		{
			if (root == null)                                  //이진탐색트리가 비어있다면
				return null;

			Node current = root;                         //루트부터 찾기 시작
			while (current != null)                        //값을 찾을 때 까지 반복
			{
				if (item.CompareTo(current.item) < 0)         //찾아야 될 값이 트리값보다 작다면
				{
					current = current.left;                            //왼쪽으로 진행
				}
				else if (item.CompareTo(current.item) > 0)  //찾아야 될 값이 트리값보다 큰 경우
				{
					current = current.right;                           //오른쪽으로 진행
				}
				else                                                              //찾아야 될 값이 트리값과 같은 경우
				{
					return current;            //트리 값을 반환
				}
			}
			return null;                          //이진탐색트리에 값이 없음.
		}
		//값을 삭제하는데 값이 없어서 삭제할 수 없다면 false 삭제할 수 있다면 true값을 반환하는 함수
		public bool Remove(T item)     
		{
			if (root == null)                   //이진탐색트리가 비어있는 경우
				return false;

			Node findNode = FindNode(item);  //값을 찾는 함수를 진행 하여 변수
			if (findNode == null)                       //이진탐색트리에 값이 없다면
			{
				return false;
			}
			else                                                //이진탐색트리에 있다면
			{
				EraseNode(findNode);               //변수를 삭제하는 함수 진행
				return true;
			}
		}
		private void EraseNode(Node node)  //값을 삭제하는 함수 진행
		{
			if (node.hasNoChild)                     //자식 노드가 없는 노드일 경우
			{
				if (node.isLeftChild)                 //본인이 왼쪽자식값이라면
					node.parent.left = null;         //값의 부모의 왼쪽값 즉 본인을 지움
				else if (node.isRightChild)        //본인이 오른쪽자식값이라면
					node.parent.right = null;       //값의 부모의 오른쪽값 즉 본인을 지움
				else                                          //루트 노드일 경우
					root = null;
			}
			//자식 노드가 한개인 노드일 경우
			else if (node.hasLeftChild || node.hasRightChild) 
			{
				Node parent = node.parent;                                                      //값의 부모값을 변수지정
				Node child = node.hasLeftChild ? node.left : node.right;          //본인의 자식값을 변수지정

				if (node.isLeftChild)                 //본인이 왼쪽자식값이라면
				{
					parent.left = child;               //본인 자식값을 본인자리로 올림
					child.parent = parent;          //본인 자식값의 부모값을 본인의 부모값으로 지정
				}
				else if (node.isRightChild)       //본인이 오른쪽자식값이라면
				{
					parent.right = child;            //똑같이 진행
					child.parent = parent;
				}
				else                                        //본인이 루트값이라면
				{
					root = child;                       //자신의 자식값을 루트값으로 지정
					child.parent = null;             //자식값의 부모값을 null로 지정
				}
			}
			//자식 노드가 2개인 노드인 경우
			//오른쪽자식 중 가장 작은 값과 데이터 교환한 후 그 노드를 지워주는 방식으로 대체
			else
			{
				Node eraseNode = node.right;           //지울 값의 오른쪽자식값을 불러옴
				while (eraseNode.left != null)             //오른쪽자식값의 왼쪽자식값을 계속 불러오는것을 반복함 null값이 될떄까지
				{
					eraseNode = eraseNode.left;
				}
				node.item = eraseNode.item;             //지울값의 값을 본인의 값에 넣음
				EraseNode(eraseNode);                    //지울값으로 지정한 노드를 지움
			}
		}

		private class Node
		{
			public T item;                    //노드의 값
			public Node parent;          //노드의 부모 노드
			public Node left;              //노드의 왼쪽자식노드
			public Node right;            //노드의 오른쪽자식노드

			public Node(T item, Node parent, Node left, Node right)
			{
				this.item = item;
				this.parent = parent;
				this.left = left;
				this.right = right;
			}
			public bool isLeftChild { get { return left != null && parent.left == this; } }          //노드가 왼쪽자식노드인지 판별하는 함수
			public bool isRightChild { get { return right != null && parent.right == this; } }     //노드가 오른쪽자식노드인지 판별하는 함수
			public bool hasNoChild { get { return left == null && right == null; } }                   //노드가 자식이 없는지 판별하는 함수
			public bool hasLeftChild { get { return left != null && right == null; } }                 //노드가 왼쪽자식이 없는지 판별하는 함수
			public bool hasRightChild { get { return left == null && right != null; } }                //노드가 오른쪽자식이 없는지 판별하는 함수
		}
	}
}
