#nullable enable
#pragma warning disable CS8604 //Null checks are present in Validate methods
#pragma warning disable CS8602
#pragma warning disable CS8600

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LabWork_CollectionsGenerics
{
    public class CustomLinkedList<T> : IEnumerable<T>
    {
        public CustomLinkedList() { }
        public CustomLinkedList(Node<T> node)
        {
            try
            {
                ValidateNewNode(node);
                head = node;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
        public CustomLinkedList(params T[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
        ~CustomLinkedList()
        {
            Clear();
        }

        internal Node<T>? head;

        public Node<T>? First { get => head; }
        public Node<T>? Last { get => head?.prev; }
        public int Count { get; private set; }
        public bool IsReadOnly { get => false; }

        public T this[int index]
        {
            get
            {
                if (head is null)
                    throw new ArgumentNullException("head of list is null.");

                if (index >= Count || index < 0)
                    throw new IndexOutOfRangeException("tried to access nonexisting node.");

                Node<T> node = head;
                for (int i = 0; i < index; i++)
                {
                    node = node.next;
                }

                return node.item;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException("Tried to assign null to value");
                if (head is null)
                    throw new ArgumentNullException("head of list is null.");
                if (index >= Count || index < 0)
                    throw new IndexOutOfRangeException("tried to access nonexisting node.");

                Node<T> node = head;
                for (int i = 0; i < index; i++)
                {
                    node = node.next;
                }
                node.item = value;
            }
        }
        public Node<T> Add(T item)
        {
            return AddLast(item);
        }
        public Node<T> AddAfter(Node<T> node, T value)
        {
            ValidateNode(node);

            Node<T> result = new Node<T>(node.list, value);
            InternalInsertNodeBefore(node.next, result);
            return result;
        }
        public Node<T> AddBefore(Node<T> node, T value)
        {
            ValidateNode(node);
            Node<T> result = new Node<T>(node.list, value);
            InternalInsertNodeBefore(node, result);
            if (node == head)
            {
                head = result;
            }
            return result;
        }
        public Node<T> AddFirst(T value)
        {
            Node<T> result = new Node<T>(this, value);
            if (head is null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
                head = result;
            }
            return result;
        }
        public Node<T> AddLast(T value)
        {
            Node<T> result = new Node<T>(this, value);
            if (head is null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
            }
            return result;
        }

        public void Add(Node<T> node)
        {
            AddLast(node);
        }
        public void AddAfter(Node<T> node, Node<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }
        public void AddBefore(Node<T> node, Node<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node == head)
            {
                head = newNode;
            }
        }
        public void AddFirst(Node<T> node)
        {
            ValidateNewNode(node);

            if (head is null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
                head = node;
            }
            node.list = this;
        }
        public void AddLast(Node<T> node)
        {
            ValidateNewNode(node);

            if (head is null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
            }
            node.list = this;
        }

        public void Clear()
        {
            Node<T> current = head;
            while (current != null)
            {
                Node<T> temp = current;
                current = current.Next;   // use Next the instead of "next", otherwise it will loop forever
                temp.Invalidate();
            }

            head = null;
            Count = 0;
        }
        public void RemoveFirstByValue(T value)
        {
            Node<T> node = Find(value);
            if (node != null)
            {
                InternalRemoveNode(node);
            }
        }
        public void RemoveLastByValue(T value)
        {
            Node<T> node = FindLast(value);
            if (node != null)
            {
                InternalRemoveNode(node);
            }
        }
        public void RemoveAllByValue(T value)
        {
            Node<T> node = Find(value);
            while (node != null)
            {
                RemoveFirstByValue(value);
                node = Find(value);
            }
        }
        public void Remove(Node<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }
        public void RemoveFirst()
        {
            if (head is null)
            {
                throw new InvalidOperationException("list is empty.");
            }
            InternalRemoveNode(head);
        }
        public void RemoveLast()
        {
            if (head is null)
            {
                throw new InvalidOperationException("list is empty.");
            }
            InternalRemoveNode(head.prev);
        }
        public bool Contains(T value)
        {
            return Find(value) != null;
        }
        public Node<T>? Find(T value)
        {
            Node<T>? node = head;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (Equals(node.item, value))
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
                else
                {
                    do
                    {
                        if (node.item is null)
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
            }
            return null;
        }
        public Node<T>? FindLast(T value)
        {
            if (head is null) return null;

            Node<T>? last = head.prev;
            Node<T>? node = last;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (Equals(node.item, value))
                        {
                            return node;
                        }

                        node = node.prev;
                    } while (node != last);
                }
                else
                {
                    do
                    {
                        if (node.item is null)
                        {
                            return node;
                        }
                        node = node.prev;
                    } while (node != last);
                }
            }
            return null;
        }
        public void BubbleSort()
        {
            if (head is null || Count <= 1)
                return;

            Node<T> node;
            int maxIndex = Count - 1;
            while (maxIndex > 0)
            {
                node = head;
                int index = 0;
                while (index < maxIndex)
                {
                    if (Comparer<T>.Default.Compare(node.item, node.next.item) > 0)
                    {
                        SwapValues(node, node.next);
                    }
                    node = node.next;
                    index++;
                }
                maxIndex--;
            }
        }
        public void SaveToFile(string fileName = "Linked List.io")
        {
            if (head is null || Count == 0)
            {
                throw new InvalidOperationException("Can't save null to file.");
            }
            using StreamWriter sw = File.CreateText(fileName);
            Node<T> node = head;
            int nodeIndex = 0;
            do
            {
                sw.WriteLine($"Node {nodeIndex}: {node}");
                nodeIndex++;
                node = node.next;
            }
            while (node != head);
        }
        public void DisplayToConsole()
        {
            if (head is null)
            {
                if (Count == 0)
                {
                    Console.WriteLine("List is empty.");
                }
                else
                {
                    Console.WriteLine("Head node is null. Can't display.");
                }
                return;
            }
            Node<T> node = head;
            int nodeIndex = 0;
            do
            {
                Console.WriteLine($"Node {nodeIndex}: {node}");
                nodeIndex++;
                node = node.next;
            }
            while (node != head);
        }
        private void InternalInsertNodeBefore(Node<T> node, Node<T> newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            Count++;
        }
        private void InternalInsertNodeToEmptyList(Node<T> newNode)
        {
            if (head != null && Count != 0)
            {
                throw new InvalidOperationException("list is not empty");
            }
            newNode.next = newNode;
            newNode.prev = newNode;
            head = newNode;
            Count++;
        }
        private void InternalRemoveNode(Node<T> node)
        {
            if (node.list != this)
            {
                throw new InvalidOperationException("node is linked up to another list.");
            }
            if (head is null && Count == 0)
            {
                throw new InvalidOperationException("list is empty.");
            }

            if (node.next == node)
            {
                head = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (head == node)
                {
                    head = node.next;
                }
            }
            node.Invalidate();
            Count--;
        }
        private void ValidateNewNode(Node<T> node)
        {
            if (node is null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != null)
            {
                throw new InvalidOperationException("node is already connected to another list");
            }
        }
        private void ValidateNode(Node<T> node)
        {
            if (node is null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != this)
            {
                throw new InvalidOperationException("node is already connected to another list");
            }
        }
        private void SwapValues(Node<T> node1, Node<T> node2)
        {
            if (node1.list != node2.list || node1.list != this || node2.list != this)
            {
                throw new InvalidOperationException("Can't swap values between nodes in different lists.");
            }

            T temp = node1.item;
            node1.item = node2.item;
            node2.item = temp;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly CustomLinkedList<T> list;
            private Node<T>? node;
            private T current;
            private int index;

            internal Enumerator(CustomLinkedList<T> list)
            {
                this.list = list;
                node = list.head;
                current = default;
                index = 0;
            }

            public T Current
            {
                get { return current; }
            }

            object? IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == list.Count + 1))
                    {
                        throw new InvalidOperationException("enum can't happen.");
                    }

                    return current;
                }
            }

            public bool MoveNext()
            {
                if (node is null)
                {
                    index = list.Count + 1;
                    return false;
                }

                index++;
                current = node.item;
                node = node.next;
                if (node == list.head)
                {
                    node = null;
                }
                return true;
            }

            void IEnumerator.Reset()
            {
                current = default;
                node = list.head;
                index = 0;
            }

            public void Dispose()
            {
            }
        }
    }
}
