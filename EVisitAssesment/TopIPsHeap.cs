//-----------------------------------------------------------------------------------
//Sorce based on: https://egorikas.com/max-and-min-heap-implementation-with-csharp/
//-----------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVisitAssesment
{
    public class Node: IComparable
    {
        public int Index { get; set; } = -1;
        public string IP { get; set; }
        public int Counter { get; set; }

        public Node(int counter, string iP)
        {
            IP = iP;
            Counter = counter;
        }

        public int CompareTo(object obj)
        {
            return this.Counter.CompareTo(((Node)obj).Counter);
        }
    }

    public class TopIPsHeap
    {
        private Node[] _elements;
        private int _size;
        private Dictionary<string, Node> _hashMap = new Dictionary<string, Node>();

        public TopIPsHeap(int size)
        {
            _elements = new Node[size];
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private Node GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private Node GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private Node GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

        private void Swap(int firstIndex, int secondIndex)
        {
            var temp = _elements[firstIndex];
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = temp;

            _elements[firstIndex].Index = firstIndex;
            _elements[secondIndex].Index = secondIndex;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        public Node Peek()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            return _elements[0];
        }

        public Node Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];

            _hashMap.Remove(result.IP);
            _elements[0].Index = 0;

            _size--;

            ReCalculateDown();

            return result;
        }

        public List<string> GetSortedCollection()
        {
            //clone state
            var cloneElements = _elements.Select(a => a).ToArray();
            var cloneSize = _size;
            var cloneHashMap = new Dictionary<string, Node>();
            
            foreach (string key in _hashMap.Keys)
            {
                cloneHashMap.Add(key, _hashMap[key]);
            }

            var res = new List<string>();
            while(_size>0)
            {
                res.Add(Pop().IP);
            }

            //restore state
            _size = cloneSize;
            _elements = cloneElements;
            _hashMap = cloneHashMap;
            return res;
        }

        public void Clear()
        {
            _size = 0;
            _hashMap.Clear();
        }

        public void Add(int counter, string ip)
        {
            Add(new Node(counter, ip));
        }

        public void Add(Node element)
        {
            //if this IP already exists in the heap update its counter value and rebalance the heap
            if (_hashMap.ContainsKey(element.IP))
            {
                var existedElement = _hashMap[element.IP];
                if (existedElement.CompareTo(element)<0)
                {
                    existedElement.Counter = element.Counter;
                    ReCalculateDown(existedElement.Index);
                }
            }
            else if (_size == _elements.Length)
            {
                //replace min element in heap with the new one
                if (Peek().CompareTo(element) < 0)
                {
                    _hashMap.Remove(Peek().IP);

                    _elements[0] = element;
                    element.Index = 0;

                    ReCalculateDown(element.Index);
                }
            }
            else
            {
                //if heap is empty we add new element
                _elements[_size] = element;
                element.Index = _size;
                _size++;

                this._hashMap[element.IP] = element;

                ReCalculateUp();
            }
        }

        private void ReCalculateDown(int index =0)
        {
            while (HasLeftChild(index))
            {
                var smallerIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && GetRightChild(index).CompareTo(GetLeftChild(index))<0)
                {
                    smallerIndex = GetRightChildIndex(index);
                }

                if (_elements[smallerIndex].CompareTo(_elements[index])>=0)
                {
                    break;
                }

                Swap(smallerIndex, index);
                index = smallerIndex;
            }
        }

        private void ReCalculateUp()
        {
            var index = _size - 1;
            while (!IsRoot(index) && _elements[index].CompareTo(GetParent(index))<0)
            {
                var parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }
    }
}
