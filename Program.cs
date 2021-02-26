using System;
using System.Collections;

namespace algoritmu_analize
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            Test_Array_List(seed);
        }


        public static void Test_Array_List(int seed)
        {
            const int n = 5600;
            var array = new MyDataArray(n, seed);
            Console.WriteLine("\n ARRAY \n");
            array.Print(n);
            RadixSort(array, n);
            Console.WriteLine("\n SORTED ARRAY \n");
            array.Print(n);

            var list = new MyDataList(n, seed);
            Console.WriteLine("\n LIST \n");
            list.Print(n);
            RadixSort(list, n);
            Console.WriteLine("\n SORTED LIST \n");
            list.Print(n);
        }


        public static int GetMax(DataArray arr, int n)
        {
            var mx = arr[0];
            for (var i = 1; i < n; i++)
                if (arr[i] > mx)
                    mx = arr[i];
            return mx;
        }


        public static void CountSort(MyDataList list, int n, int exp)
        {
            var output = new int[n]; // output array  
            int i;
            const int b = 10;
            var count = new int[b];


            foreach (int num in list)
            {
                count[(num / exp) % 10]++;
            }

            for (i = 1; i < b; i++)
                count[i] += count[i - 1];


            for (i = n - 1; i >= 0; i--)
            {
                output[count[(list.GetDataByIndex(i) / exp) % 10] - 1] = list.GetDataByIndex(i);
                count[(list.GetDataByIndex(i) / exp) % 10]--;
            }


            for (i = 0; i < n; i++)
                list.SetByIndex(i, output[i]);
        }

        public static void RadixSort(MyDataList list, int n)
        {
            var m = list.GetMax().ToString().Length;
            var exp = 1;

            for (var i = 0; i < m; i++)
            {
                CountSort(list, n, exp);
                exp *= 10;
            }
        }

        public static void CountSort(DataArray arr, int n, int exp)
        {
            var output = new int[n]; // output array  
            int i;
            const int b = 10;
            var count = new int[b];


            for (i = 0; i < n; i++)
                count[(arr[i] / exp) % 10]++;

            for (i = 1; i < b; i++)
                count[i] += count[i - 1];

            for (i = n - 1; i >= 0; i--)
            {
                output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                count[(arr[i] / exp) % 10]--;
            }

            for (i = 0; i < n; i++)
                arr[i] = output[i];
        }

        public static void RadixSort(DataArray arr, int n)
        {
            var m = GetMax(arr, n).ToString().Length;
            var exp = 1;

            for (var i = 0; i < m; i++)
            {
                CountSort(arr, n, exp);
                exp *= 10;
            }
        }

    }

    public abstract class DataArray
    {
        protected int _length;
        public int Length => _length;
        public abstract int this[int index] { get; set; }
        public void Print(int n)
        {
            for (var i = 0; i < n; i++)
            {
                Console.WriteLine(" {0} ", this[i]);
            }
        }
    }

    public abstract class DataList
    {
        protected int length;
        public int Length => length;
        public abstract int Head();
        public abstract int Next();
        public void Print(int n)
        {
            Console.WriteLine(" {0}", Head());
            for (int i = 1; i < n; i++)
            {
                Console.WriteLine(" {0} ", Next());
            }
            Console.WriteLine();
        }

    }

    public class MyDataArray : DataArray
    {
        private readonly int[] _data;
        public MyDataArray(int n, int seed)
        {
            _data = new int[n];
            _length = n;
            var rand = new Random(seed);
            for (var i = 0; i < _length; i++)
            {
                _data[i] = rand.Next();
            }
        }
        public override int this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

    }

    public class MyDataList : DataList, IEnumerable
    {
        public class MyLinkedListNode
        {
            public MyLinkedListNode NextNode { get; set; }
            public MyLinkedListNode PrevNode { get; set; }
            public int Data { get; }
            public MyLinkedListNode(int data)
            {
                this.Data = data;
            }
        }

        public MyLinkedListNode HeadNode;
        public MyLinkedListNode CurrentNode;

        public MyDataList(int n, int seed)
        {
            length = n;
            var rand = new Random(seed);
            HeadNode = new MyLinkedListNode(rand.Next());
            CurrentNode = HeadNode;
            for (var i = 1; i < length; i++)
            {
                CurrentNode.NextNode = new MyLinkedListNode(rand.Next());
                CurrentNode = CurrentNode.NextNode;
            }
            CurrentNode.NextNode = null;
        }
        public override int Head()
        {
            CurrentNode = HeadNode;
            return CurrentNode.Data;
        }

        public override int Next()
        {
            CurrentNode = CurrentNode.NextNode;
            return CurrentNode.Data;
        }

        public int GetMax()
        {
            CurrentNode = HeadNode;
            var max = int.MinValue;
            while (CurrentNode != null)
            {
                if (max < CurrentNode.Data)
                    max = CurrentNode.Data;
                CurrentNode = CurrentNode.NextNode;
            }
            return max;
        }

        public void SetByIndex(int index, int data)
        {
            var i = 0;
            for (var n = HeadNode; n != null; n = n.NextNode)
            {
                if (i == index)
                {
                    var newNode = new MyLinkedListNode(data) {NextNode = n.NextNode, PrevNode = n.PrevNode};
                    if (n.PrevNode != null)
                        n.PrevNode.NextNode = newNode;
                    else HeadNode = newNode;
                    if (n.NextNode != null)
                        n.NextNode.PrevNode = newNode;
                    break;
                }
                ++i;
            }
        }

        public int GetDataByIndex(int index)
        {
            var i = 0;
            for (var n = HeadNode; n != null; n = n.NextNode)
            {
                if (i == index)
                {
                    return n.Data;
                }
                ++i;
            }
            return i;
        }

        public IEnumerator GetEnumerator()
        {
            for (var node = HeadNode; node != null; node = node.NextNode)
            {
                yield return node.Data;
            }
        }
    }
}
