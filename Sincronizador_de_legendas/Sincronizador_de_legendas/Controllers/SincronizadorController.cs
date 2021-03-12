using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sincronizador_de_legendas.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sincronizador_de_legendas.Controllers
{
    public class SincronizadorController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        //criei esse construtor para conseguir pegar o caminho onde a aplicação está rodando 
        //e posteriormente salvar os arquivos na pasta resources
        public SincronizadorController(IWebHostEnvironment env)
        {
            _appEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        //método para enviar os arquivos usando a interface IFormFile
        public async Task<IActionResult> Enviar(List<IFormFile> arquivos)
        { 
            var destino = $"{_appEnvironment.ContentRootPath}\\Resources\\";
            foreach (var arquivo in arquivos)
            {   
                if (arquivo == null || arquivo.Length == 0)
                {
                    //retorna a viewdata com erro
                    ViewData["Erro"] = "Erro: Arquivo inválido ou não selecionado";
                    return View(ViewData);
                }

                var destinoArquivo = $"{destino}{arquivo.FileName}";

                //copia o arquivo recebido na pasta resources
                using (var stream = new FileStream(destinoArquivo, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }
            }
            ViewData["Resultado"] = "Arquivo(s) enviado(s) com sucesso!";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
