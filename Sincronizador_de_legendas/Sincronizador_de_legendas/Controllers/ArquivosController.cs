using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sincronizador_de_legendas.Models;
using System.Linq;

namespace Sincronizador_de_legendas.Controllers
{
    public class ArquivosController : Controller
    {
        private readonly string _pastaResources;
        private const string _contentType = "text/plain";
        //criei esse construtor para conseguir pegar o caminho onde a aplicação está rodando 
        //e posteriormente salvar os arquivos na pasta resources
        public ArquivosController(IWebHostEnvironment env)
        {
            _pastaResources = $"{env.ContentRootPath}\\Resources";
        }

        [HttpGet]
        public IActionResult Index()
        {
            var arquivos = ArquivosModel.GetArquivos($"{_pastaResources}\\Resources\\");
            return View(arquivos);
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            if (id < 1)
                return View();
            var arquivos = ArquivosModel.GetArquivos($"{_pastaResources}\\Resources\\");

            if(arquivos == null || !arquivos.Any())
                return View();

            var arquivo = arquivos
                .Where(x => x.ID == id)
                .FirstOrDefault();

            if (arquivo == null)
                return View();

            byte[] bytesArquivo = System.IO.File.ReadAllBytes(arquivo.Caminho);

            return File(bytesArquivo, _contentType, arquivo.Nome);
        }
    }
}
