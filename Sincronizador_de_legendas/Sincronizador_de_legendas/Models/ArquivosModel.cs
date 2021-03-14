using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sincronizador_de_legendas.Models
{
    public class ArquivosModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public static IEnumerable<ArquivosModel> GetArquivos(string path)
        {
            List<ArquivosModel> lstArquivos = new List<ArquivosModel>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            int i = 0;
            foreach (var item in dirInfo.GetFiles().OrderBy(x => x.CreationTime))
            {
                i++;
                lstArquivos.Add(new ArquivosModel()
                {
                    ID = i,
                    Nome = item.Name,
                    Caminho = dirInfo.FullName + item.Name
                });
            }
            return lstArquivos;
        }
    }
}
