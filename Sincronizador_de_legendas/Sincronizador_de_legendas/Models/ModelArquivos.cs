using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Sincronizador_de_legendas.Models
{
    public class ModelArquivos
    {
        public List<Arquivos> GetArquivos(string path)
        {
            List<Arquivos> lstArquivos = new List<Arquivos>();
            //DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Arquivos"));
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                lstArquivos.Add(new Arquivos()
                {
                    ID = i + 1,
                    Nome = item.Name,
                    Caminho = dirInfo.FullName + item.Name
                });
                i = i + 1;
            }
            return lstArquivos;
        }
    }
}
