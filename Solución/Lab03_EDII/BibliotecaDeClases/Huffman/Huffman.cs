using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
    public class Huffman
    {
        public Dictionary<byte, string> huffCod = new Dictionary<byte, string>();
        public Node root;
        private bool[] bitF = { false };
        public void Encode(Node root, string str, Dictionary<byte, string> huffman)
        {
            if (root == null)
            {
                return;
            }
            if (root.Left == null && root.Right == null)
            {
                huffman.Add(root.byt, str);
            }
            BitArray bit = new BitArray(bitF);
            Encode(root.Left, str + "0", huffman);
            Encode(root.Right, str + "1", huffman);
        }

        void WriteTree(List<Node> PList, string fullPath)
        {
            foreach (var item in PList)
            {
                byte[] temp = { item.byt };
                using (FileStream writer = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    writer.Seek(0, SeekOrigin.End);
                    if (ByteGenerator.ConvertToString(temp) == '\n'.ToString())
                    {
                        writer.Write(ByteGenerator.ConvertToBytes("NTR"), 0, 3);
                    }
                    else if (ByteGenerator.ConvertToString(temp) == '\r'.ToString())
                    {
                        writer.Write(ByteGenerator.ConvertToBytes("NDL"), 0, 3);
                    }
                    else
                    {
                        writer.Write(temp, 0, 1);
                    }
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(ByteGenerator.ConvertToBytes(item.freq.ToString()), 0, item.freq.ToString().Length);
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(ByteGenerator.ConvertToBytes("@@@"), 0, 3);
                }
            }

        }

        public void BuildHuffman(byte[] texto, string newName) {
            Dictionary<byte, int> frecuency = new Dictionary<byte, int>();
            for (int i = 0; i < texto.Length; i++)
            {
                if (!frecuency.ContainsKey(texto[i]))
                {
                    frecuency.Add(texto[i], 0);
                }
                frecuency.TryGetValue(texto[i], out int x);
                frecuency[texto[i]] = x + 1;
            }
            List<Node> PList = new List<Node>();

            for (int i = 0; i < frecuency.Count; i++)
            {
                KeyValuePair<byte, int> pair = frecuency.ElementAt(i);
                PList.Add(new Node(pair.Key, pair.Value, null, null));
            }
            PList.Sort();
            string folder = "c:\\Compressions\\";
            DirectoryInfo directory = Directory.CreateDirectory(folder);
            string fullPath = folder + newName + ".huff";
            WriteTree(PList, fullPath);

            while (PList.Count != 1)
            {
                Node Pair = PList[0];
                PList.RemoveAt(0);
                Node Left = Pair;

                Pair = PList.ElementAt(0);
                PList.RemoveAt(0);
                Node Right = Pair;

                int Suma = Left.freq + Right.freq;
                PList.Add(new Node(0, Suma, Left, Right));
                PList.Sort();
            }
            this.root = PList.ElementAt(0);
            Encode(this.root, "", huffCod);
        }

       /// <summary>
       /// Construye el Huffman a partir de la frecuencia
       /// </summary>
       /// <param name="frecuency"></param>
        public void BuildHuffman(Dictionary<byte, int> frecuency) {
            List<Node> PList = new List<Node>();
            for (int i = 0; i < frecuency.Count; i++)
            {
                KeyValuePair<byte, int> Pair = frecuency.ElementAt(i);
                PList.Add(new Node(Pair.Key, Pair.Value, null, null));
            }
            while (PList.Count != 1)
            {
                Node pair = PList[0];
                PList.RemoveAt(0);
                Node left = pair;

                pair = PList.ElementAt(0);
                PList.RemoveAt(0);
                Node right = pair;

                int sum = left.freq + right.freq;
                PList.Add(new Node(0, sum, left, right));
                PList.Sort();
            }
            this.root = PList.ElementAt(0);
        }
        byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }
        byte[] ConvertToByte(string b)
        {
            BitArray bits = new BitArray(b.Select(x => x == '1').ToArray());

            byte[] ret = ToByteArray(bits);

            return ret;
        }

        public void WriteFile(byte[] text, string newName, string name)
        {
            //escribir archivo binario 
            string folder = "c:\\Compressions\\";
            string fullPath = folder + newName + ".huff";
            string ArchivoCompresiones = folder + "DataCompress.txt";

            // crear el directorio
            DirectoryInfo directory = Directory.CreateDirectory(folder);

            string content = "";
            foreach (var item in text)
            {
                content += huffCod[item];
            }

            byte[] compressed = ConvertToByte(content);
            using (FileStream writer = new FileStream(fullPath, FileMode.Open))
            {
                byte[] ToWrite = ConvertToByte(content);
                for (int i = 0; i < ToWrite.Length; i++)
                {
                    byte[] temp = { ToWrite[i] };
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(temp, 0, 1);

                }
            }
            double compressedBytes = compressed.Length;
            double originalBytes = text.Length;
            double rc = compressedBytes / originalBytes;
            double fc = originalBytes / compressedBytes;
            double percentage = rc * 100;



            string[] data = new string[5];
            data[0] = name;
            data[1] = fullPath;
            data[2] = rc.ToString();
            data[3] = fc.ToString();
            data[4] = percentage.ToString("N2") + "%";
            string ifContent = "";

            if (!File.Exists(ArchivoCompresiones))
            {
                using (StreamWriter writer = new StreamWriter(ArchivoCompresiones))
                {
                    writer.Write("");
                }
            }
            using (StreamReader reader = new StreamReader(ArchivoCompresiones))
            {
                ifContent =  reader.ReadToEnd();
            }
            using (StreamWriter writer = new StreamWriter(ArchivoCompresiones))
            {
                writer.Write(ifContent);
                writer.Write(data[0] + "|");
                writer.Write(data[1] + "|");
                writer.Write(data[2] + "|");
                writer.Write(data[3] + "|");
                writer.Write(data[4] + "|");
                writer.Write("###");
            }
        }


        /// <summary>
        /// Convierte el bit a binario
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private BitArray ToBitArray(byte[] bytes)
        {
            string strAllbin = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                byte byteindx = bytes[i];

                string strBin = Convert.ToString(byteindx, 2);
                strBin = strBin.PadLeft(8, '0'); // rellena los espacios vacios con cero si no llega a tenerlos

                strAllbin += strBin;
            }

            BitArray ba = new BitArray(strAllbin.Select(x => x == '1').ToArray());
            return ba;
        }

        public void decode(Node root, ref int index, BitArray str, ref string result)
        {
            if (root == null)
            {
                return;
            }
            if (root.Left == null && root.Right == null)
            {
                byte[] toStr = { root.byt };
                result += ByteGenerator.ConvertToString(toStr);
                return;
            }

            index++;

            if (str[index] == false)
            {
                decode(root.Left, ref index, str, ref result);
            }
            else
            {
                decode(root.Right, ref index, str, ref result);
            }
        }
        public void DecodeFile(byte[] text, string newName, string name)
        {
            string txt = ByteGenerator.ConvertToString(text);
            string[] nodes = txt.Split("@@@");

            int start = (nodes.Length - 1) * 3;

            Dictionary<byte, int> freq = new Dictionary<byte, int>();

            for (int i = 0; i < nodes.Length - 1; i++)
            {
                string val = nodes[i];

                string character = "";
                string integer = "";

                if (int.TryParse(val[0].ToString(), out _))
                {
                    character += val[0];
                    for (int j = 1; j < val.Length; j++)
                    {
                        integer += val[j];
                        start++;
                    }
                }
                else
                {
                    for (int j = 0; j < val.Length; j++)
                    {
                        if (!int.TryParse(val[j].ToString(), out _))
                        {
                            character += val[j];
                        }
                        else
                        {
                            integer += val[j];
                            start++;
                        }
                    }
                }

                byte[] bt;
                if (character == "NTR")
                {
                    bt = ByteGenerator.ConvertToBytes('\n'.ToString());
                }
                else if (character == "NDL")
                {
                    bt = ByteGenerator.ConvertToBytes('\r'.ToString());
                }
                else
                {
                    bt = ByteGenerator.ConvertToBytes(character.ToString());
                }
                start += character.Length;

                if (!freq.ContainsKey(bt[0]))
                {
                    freq.Add(bt[0], Convert.ToInt32(integer));
                }
            }

            BuildHuffman(freq);

            BitArray result = ToBitArray(text.Skip(start).ToArray());
            int index = -1;
            string decoded = "";
            while (index < result.Length - 1)
            {
                decode(this.root, ref index, result, ref decoded);
            }

            string folder = @"C:\Compressions\";
            string fullPath = folder + newName;

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                sw.Write(decoded);
            }
        }

        public string OldName() {
            string folder = "c:\\Compressions\\";
            string ArchivoCompresiones = folder + "DataCompress.txt";
            string data = "";
            string result = "";

            using (StreamReader reader = new StreamReader(ArchivoCompresiones))
            {
                data = reader.ReadToEnd();
            }
            string[] dataComplete = new string[100];
            dataComplete = data.Split("###");
            Array.Reverse(dataComplete);
            string[] dataCompleteV2 = new string[100];
            string value = String.Concat(dataComplete);
            dataCompleteV2 = value.Split("|");
            result = dataCompleteV2[0];
            return result;
        }
        
    }
}
