using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases.Huffman
{

    public class CompressionsCollection
    {
        public string originalName { get; set; }
        public string path { get; set; }
        public double compressionReason { get; set; } // razon de compresion 
        public double compressionFactor { get; set; }  //factor de compresion 
        public string reduction { get; set; }

        public CompressionsCollection(string name, string path, double cr, double cf, string redPercentage)
        {
            originalName = name;
            this.path = path;
            compressionReason = cr;
            compressionFactor = cf;
            reduction = redPercentage;
        }
    }
}
