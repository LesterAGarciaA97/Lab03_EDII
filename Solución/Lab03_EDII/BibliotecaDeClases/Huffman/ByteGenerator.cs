using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases.Huffman
{
	public class ByteGenerator
	{
		public static byte[] ConvertToBytes(string text)
		{
			return Encoding.ASCII.GetBytes(text);
		}

		public static string ConvertToString(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes);
		}

		public static byte[] ConvertToBytes(char[] text)
		{
			return Encoding.ASCII.GetBytes(text);
		}
	}
}
