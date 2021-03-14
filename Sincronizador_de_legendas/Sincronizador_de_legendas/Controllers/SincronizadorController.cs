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
        private readonly string _pastaResources;
        //criei esse construtor para conseguir pegar o caminho onde a aplicação está rodando 
        //e posteriormente salvar os arquivos na pasta resources
        public SincronizadorController(IWebHostEnvironment env)
        {
            _pastaResources = $"{env.ContentRootPath}\\Resources\\";
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessarArquivosDeLegenda(IEnumerable<IFormFile> arquivos, double offset)
        {
            foreach (var arquivo in arquivos)
            {
                try
                {
                    var arquivoProcessado = AplicarOffsetEmSrt(arquivo, offset);

                    //armazena o arquivo formatado na pasta resources com "offseted-" na frente do nome
                    System.IO.File.WriteAllLines($"{_pastaResources}offseted-{arquivo.FileName}", arquivoProcessado);
                }
                catch (Exception e)
                {
                    //se houver arquivo(s) invalido(s) será retornado um erro
                    ViewData["Erro"] = e.Message;
                    return View(ViewData);
                }
            }

            ViewData["Resultado"] = "Legenda(s) formatada(s) com sucesso!";
            return View(ViewData);
        }

        private static IEnumerable<string> AplicarOffsetEmSrt(IFormFile arquivo, double offset)
        {
            if(!arquivo.FileName.Contains(".srt"))
            {
                throw new Exception("Arquivo(s) selecionado(s) não pertence ao formato 'srt'");
            }

            //armazena cada linha do arquivo em um indice do array
            var arquivoArray = new StreamReader(arquivo.OpenReadStream())
            .ReadToEnd()
            .Replace("\r", string.Empty)
            .Split("\n");

            var menorTempo = TimeSpan.MinValue;
            for (int i = 0; i < arquivoArray.Length; i++)
            {
                //verifica se a linha é de tempo ou não
                if (arquivoArray[i].Contains(" --> "))
                {
                    //separa os tempos da linha e armazena-os
                    string[] tempos = arquivoArray[i].Split(" --> ");

                    //verifica se o arquivo final não vai ter tempo negativo
                    if (menorTempo == TimeSpan.MinValue)
                    {
                        menorTempo = TimeSpan.Parse(tempos[0].Replace(",", "."));

                        if (offset < 0 && Math.Abs(offset) > menorTempo.TotalMilliseconds)
                        {
                            throw new Exception($"{arquivo.FileName} não foi formatado pois o tempo da primeira legenda é de {menorTempo.TotalMilliseconds}ms e o offset é de {offset}ms, resultando em tempo negativo.");
                        }
                    }

                    string linhaFormatada = default;

                    //ajuste do primeiro tempo da linha
                    string tempoProcessado = DateTime.ParseExact(tempos[0], "HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture).AddMilliseconds(offset).ToString("HH:mm:ss,fff");
                    linhaFormatada += $"{tempoProcessado} --> ";

                    //ajuste do segundo tempo da linha
                    tempoProcessado = DateTime.ParseExact(tempos[1], "HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture).AddMilliseconds(offset).ToString("HH:mm:ss,fff");
                    linhaFormatada += tempoProcessado;

                    //coloca a linha formatada no lugar da linha original
                    arquivoArray[i] = linhaFormatada;
                }
            }

            return arquivoArray;
        }
    }
}
