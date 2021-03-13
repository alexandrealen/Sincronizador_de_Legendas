﻿using Microsoft.AspNetCore.Hosting;
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
        public IActionResult ProcessarArquivosDeLegenda(IEnumerable<IFormFile> arquivos, double offset)
        {
            string destino = $"{_appEnvironment.ContentRootPath}\\Resources\\";
            foreach (var arquivo in arquivos)
            {   
                //se o arquivo for inválido será retornado um erro
                if (arquivo == null || arquivo.Length == 0 || !arquivo.FileName.Contains(".srt"))
                {
                    ViewData["Erro"] = "Erro: Arquivo inválido ou não selecionado";
                    return View(ViewData);
                }

                //armazena o arquivo em um array, sendo cada indice uma linha
                string[] arquivoArray = System.IO.File.ReadAllLines($"{destino}{arquivo.FileName}");
                for(int i=0; i<arquivoArray.Length; i++)
                {
                    //verifica se a linha é de tempo ou não
                    if(arquivoArray[i].Contains(" --> "))
                    {
                        //separa os tempos da linha e armazena-os
                        string[] tempos = arquivoArray[i].Split(" --> ");
                        string linhaFormatada = default;

                        //ajuste do primeiro tempo da linha
                        string tempoProcessado = DateTime.ParseExact(tempos[0], "HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture).AddMilliseconds(offset - (2 * offset)).ToString("HH:mm:ss,fff");
                        linhaFormatada += $"{tempoProcessado} --> ";

                        //ajuste do segundo tempo da linha
                        tempoProcessado = DateTime.ParseExact(tempos[1], "HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture).AddMilliseconds(offset - (2 * offset)).ToString("HH:mm:ss,fff");
                        linhaFormatada += tempoProcessado;

                        //coloca a linha formatada no lugar da linha original
                        arquivoArray[i] = linhaFormatada;
                    }
                }
                //armazena o arquivo formatado na pasta resources com "offseted-" na frente do nome
                System.IO.File.WriteAllLines($"{destino}offseted-{arquivo.FileName}", arquivoArray);
            }
            return View(ViewData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}