using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingi
{
    internal class CLIOptions
    {
        [Option('h', "host", Required = true, HelpText = "Indica el nombre de host remoto.")]
        public string Host { get; set; }

        [Option('o', "outputfile", Required = false, HelpText = "Archivo de salida donde se guardan los resultados")]
        public string OutputFile { get; set; }

        [Option('p', "port", Required = false, HelpText = "Indica un puerto TCP específico, Pingi intentará comprobar si el puerto se encuentra abierto y lo considerará como \"responde\"")]
        public int Port { get; set; } = -1;
    }
}
