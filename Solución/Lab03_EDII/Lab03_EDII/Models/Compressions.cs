using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab03_EDII.Models
{
    public class Compressions
    {
        public string Nombre_Archivo_Original { get; set; }
        public string Ruta_Del_Archivo_Comprimido { get; set; }
        public string Razon_de_Compresion { get; set; }
        public string Factor_de_Compresion { get; set; }
        public string Porcentaje_de_Reduccion { get; set; }
        public Compressions(string nameFile, string path, string rc, string fc, string percentaje)
        {
            Nombre_Archivo_Original = nameFile;
            Ruta_Del_Archivo_Comprimido = path;
            Razon_de_Compresion = rc;
            Factor_de_Compresion = fc;
            Porcentaje_de_Reduccion = percentaje;
        }
        public Compressions()
        {

        }

        public List<Compressions> DataCompressions()
        {
            string folder = "c:\\Compressions\\";
            DirectoryInfo directory = Directory.CreateDirectory(folder);
            string ArchivoCompresiones = folder + "DataCompress.txt";
            string data = "";
            List<Compressions> ListCompress = new List<Compressions>();

            using (StreamReader reader = new StreamReader(ArchivoCompresiones))
            {
                data = reader.ReadToEnd();
            }
            string[] dataComplete = new string[100];
            dataComplete = data.Split("###");
            dataComplete = dataComplete.Where(val => val != "").ToArray();
            Array.Reverse(dataComplete);
            string[] dataCompleteV2 = new string[100];
            string value = String.Concat(dataComplete);
            dataCompleteV2 = value.Split("|");
            dataCompleteV2 = dataCompleteV2.Where(val => val != "").ToArray();//
            ListCompress = dataCompleteV2.OfType<Compressions>().ToList();
            for (int i = 0; i < dataComplete.Length - 1; i++)
            {
                for (int j = 0; j < dataComplete.Length; j++)
                {
                    Compressions newElement = new Compressions(dataCompleteV2[0], dataCompleteV2[1], dataCompleteV2[2], dataCompleteV2[3], dataCompleteV2[4]);
                    ListCompress.Add(newElement);
                    dataCompleteV2 = dataCompleteV2.Where(n => n != dataCompleteV2[4]).ToArray();
                    dataCompleteV2 = dataCompleteV2.Where(n => n != dataCompleteV2[3]).ToArray();
                    dataCompleteV2 = dataCompleteV2.Where(n => n != dataCompleteV2[2]).ToArray();
                    dataCompleteV2 = dataCompleteV2.Where(n => n != dataCompleteV2[1]).ToArray();
                    dataCompleteV2 = dataCompleteV2.Where(n => n != dataCompleteV2[0]).ToArray();
                    if (dataCompleteV2.Length == 0)
                    {
                        break;
                    }
                }
                
            }
            
            return ListCompress;
        }

        
    }
}
