using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sincronizador_de_legendas.Models
{
    public class ArquivosModel
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public static IEnumerable<ArquivosModel> GetArquivos(string path)
        {
            List<ArquivosModel> lstArquivos = new List<ArquivosModel>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (var item in dirInfo.GetFiles().OrderBy(x => x.CreationTime))
            {
                lstArquivos.Add(new ArquivosModel()
                {
                    Nome = item.Name,
                    Caminho = dirInfo.FullName + item.Name
                });
            }
            return lstArquivos;
        }
    }
}
