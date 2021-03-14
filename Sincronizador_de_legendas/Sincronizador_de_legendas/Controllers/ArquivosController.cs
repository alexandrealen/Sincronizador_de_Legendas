using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sincronizador_de_legendas.Models;
using Microsoft.AspNetCore.Hosting;

namespace Sincronizador_de_legendas.Controllers
{
    public class ArquivosController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        ModelArquivos oModelArquivos = new ModelArquivos();
        //criei esse construtor para conseguir pegar o caminho onde a aplicação está rodando 
        //e posteriormente salvar os arquivos na pasta resources
        public ArquivosController(IWebHostEnvironment env)
        {
            _appEnvironment = env;
        }

        // GET: /Reports/
        public ActionResult Index()
        {
            var _arquivos = oModelArquivos.GetArquivos($"{_appEnvironment.ContentRootPath}/Resources\\");
            return View(_arquivos);
        }

        public FileResult Download(string id)
        {
            int _arquivoId = Convert.ToInt32(id);
            var arquivos = oModelArquivos.GetArquivos($"{_appEnvironment.ContentRootPath}/Resources\\");

            string nomeArquivo = (from arquivo in arquivos
                                  where arquivo.ID == _arquivoId
                                  select arquivo.Caminho).First();

            string contentType = "text/srt";
            return File(nomeArquivo, contentType, $"Subtitle.srt");
        }
    }
}
