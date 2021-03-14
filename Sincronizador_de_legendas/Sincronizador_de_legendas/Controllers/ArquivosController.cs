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

        public IActionResult Index()
        {
            var arquivos = oModelArquivos.GetArquivos($"{_appEnvironment.ContentRootPath}\\Resources\\");
            return View(arquivos);
        }

        public FileResult Download(string id)
        {
            int arquivoId = Convert.ToInt32(id);
            var arquivos = oModelArquivos.GetArquivos($"{_appEnvironment.ContentRootPath}\\Resources\\");

            string nomeArquivo = arquivos.Where(x => x.ID == arquivoId).Select(x => x.Nome).First();
            string caminhoArquivo = arquivos.Where(x => x.ID == arquivoId).Select(x => x.Caminho).First();
            string contentType = "text/plain";
            byte[] bytesArquivo = System.IO.File.ReadAllBytes(caminhoArquivo);

            return File(bytesArquivo, contentType, nomeArquivo); 
        }
    }
}
