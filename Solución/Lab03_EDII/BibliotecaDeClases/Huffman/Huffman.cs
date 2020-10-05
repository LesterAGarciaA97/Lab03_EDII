using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases.Huffman
{
    public class Node : IComparable {
        public byte byt;
        public int freq;
        public Node Left = null;
        public Node Right = null;
        public Node(byte _byt, int _freq, Node _Left , Node _Right)
        {
            this.byt = _byt;
            this.freq = _freq;
            this.Left = _Left;
            this.Right = _Right;
        }
        Node(byte _byt, int _freq) {
            this.byt = _byt;
            this.freq = _freq;
        }

        public int CompareTo(object obj)
        {
            var obj2 = (Node)obj;
            return freq.CompareTo(obj2.freq);
        }
    }
    class Huffman
    {
    }
}
