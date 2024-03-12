using CommandLine;

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

        [Option('n', "nodelay", Required = false, HelpText = "Indica si se tiene que enviar inmediatamente otro Ping despues de terminar otro.")]
        public bool NoDelay { get; set; } = false;
    }
}
