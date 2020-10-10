using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDeClases.Huffman;
using Lab03_EDII.Models;

namespace Lab03_EDII.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiController : ControllerBase
    {
        /// <summary>
        /// Metodo que recibe el archivo de texto para ser compreso
        /// </summary>
        /// <param name="file">Archivo a comprimir</param>
        /// <param name="name">Nuevo nombre del archivo a comprimir</param>
        [HttpPost("compress/{name}")]
        public string Post([FromForm] IFormFile file, string name) {
            var Contenido = new StringBuilder();
            using (var Lector = new StreamReader(file.OpenReadStream())) {
                while (Lector.Peek() >= 0)
                    Contenido.AppendLine(Lector.ReadLine());
            }
            byte[] TextoEnBytes = Encoding.ASCII.GetBytes(Contenido.ToString());
            Huffman CompressHuffman = new Huffman();
            CompressHuffman.BuildHuffman(TextoEnBytes, name);
            CompressHuffman.WriteFile(TextoEnBytes, name, file.FileName);
            return ("Archivo Compreso se encuentra en C:\\Compressions");
        }

        /// <summary>
        /// Metodo que recibe un archivo de texto a ser compreso
        /// </summary>
        /// <param name="file">archivo a descomprimir</param>
        [HttpPost("decompress")]
        public string Post([FromForm] IFormFile file) {
            Huffman huffmanMethods = new Huffman();
            string folder = "C:\\Compressions\\";
            string OldName = huffmanMethods.OldName();
            string fullPath = folder + file.FileName;

            byte[] txt = new byte[file.Length];
            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
            {
                int count;
                int sum = 0;
                while ((count = fs.Read(txt, sum, txt.Length - sum)) > 0)
                    sum += count;
            }

            huffmanMethods.DecodeFile(txt, OldName, file.FileName);
            return ("Archivo Compreso se encuentra en C:\\Compressions");
        }

        /// <summary>
        /// Metodo que devuelve todas las compresiones "diferentes" realizadas
        /// </summary> 
        /// <returns>JSON con las compresiones</returns>
        [HttpGet("compressions")]
        public List<Compressions> Get() {
            Compressions ResultCompress = new Compressions();
            return (ResultCompress.DataCompressions());
        }

        [HttpGet]
        public ActionResult GetResult() {
            return Ok();
        }
    }
}
