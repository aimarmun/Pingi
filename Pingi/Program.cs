﻿using CommandLine;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Pingi
{
    class Program
    {
        public static IPStatus lastStatus;
        public static bool start = true;
        public static string lastError = "";
        public static bool running = true;
        public static int successCount = 0;
        public static int failCount = 0;
        public static long timeSumatory = 0;
        public static string Host { get; set; } = "";
        public static string File { get; set; } = "";
        public static int Port { get; set; } = -1;

        public static bool NoDelay { get; set; } = false;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            PrintLineMsg("Por Aimarmun 2023");

            var result = Parser.Default.ParseArguments<CLIOptions>(args)
                .WithParsed<CLIOptions>(option =>
                {
                    Host = option.Host;
                    Port = option.Port;
                    File = option.OutputFile;
                    NoDelay = option.NoDelay;
                });

            if (result.Errors.Count() > 0) return;

            while (running)
            {
                try
                {
                    
                    if (Port > 1)
                    {
                        PingToPort();
                    }
                    else
                    {
                        Ping();
                    }
                }
                catch (Exception ex)
                {
                    if (start)
                    {
                        PrintLineMsg($"ERROR: {ex.Message}\n" +
                            $"Escribe como primer argumento la IP o nombre de Host y como segundo argumento el archivo Log destino. " +
                            $"Opcional argumento 3 puerto. By Aimarmun 03/09/2021", false);
                        Console.ReadKey();
                        Environment.Exit(1);
                        break;
                    }
                    else
                    {
                        if (!lastError.Equals(ex.Message))
                        {
                            lastError = ex.Message;
                            PrintLineMsg($"[{DateTime.Now}]\t Estado: ERROR!  Mensaje: {ex.Message}{Environment.NewLine}");

                        }
                    }
                    failCount++;
                }
            }
            string statistics = "No hubo respuestas";
            if(successCount > 0)
            {
                statistics = $"{((successCount * 100) / (successCount + failCount))}% a una media de {timeSumatory / successCount}ms.";
            }
            string temp = $"[{DateTime.Now}]\t Finalizado!{Environment.NewLine}" +
                $"\tEstadisticas: {Environment.NewLine}" +
                $"\tAciertos: {successCount}{Environment.NewLine}" +
                $"\tFallos:   {failCount}{Environment.NewLine}" +
                $"\t{statistics} {Environment.NewLine}";
            PrintLineMsg (temp);
            Console.Beep();
            System.Threading.Thread.Sleep(3000);

        }

        private static void PrintLineMsg(string msg, bool writeToFile = true)
        {
            Console.WriteLine(msg);
            if (string.IsNullOrEmpty(File) || !writeToFile) return;
            try
            {
                System.IO.File.AppendAllText(File, msg + Environment.NewLine);
            }
            catch { }
        }

        private static void Ping()
        {
            
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(Host, 3000);
                if (reply != null)
                {
                    Console.WriteLine("Estado:  " + reply.Status + " Tiempo: " + reply.RoundtripTime.ToString() + "ms. Dirección: " + reply.Address);
                    if (start)
                    {
                        start = false;
                        PrintLineMsg( $"[{DateTime.Now}]\t Estado: {reply.Status}  Tiempo: {reply.RoundtripTime}ms Dirección: {reply.Address}");
                        lastStatus = reply.Status;
                    }
                    else
                    {
                        if (!reply.Status.Equals(lastStatus))
                        {
                            PrintLineMsg($"[{DateTime.Now}]\t Estado: {reply.Status}  Tiempo: {reply.RoundtripTime}ms. Dirección: {reply.Address}");
                            lastStatus = reply.Status;
                        }
                    }
                    if (reply.Status.Equals(IPStatus.Success))
                    {
                        successCount++;
                        timeSumatory += reply.RoundtripTime;
                    }
                    else
                        failCount++;
                }

            if(!NoDelay)
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private static Stopwatch stopwatch = new Stopwatch();

        private static void PingToPort()
        {

            stopwatch.Restart();
            bool portOpen = IsPortOpen();
            stopwatch.Stop();

            int millis = (int)Math.Round(stopwatch.Elapsed.TotalMilliseconds);

            if (IsPortOpen())
            {
                Console.WriteLine("Estado:  el puerto responde. Tiempo: " + millis + "ms. Dirección: " + Host);
                if (start)
                {
                    start = false;
                    PrintLineMsg($"[{DateTime.Now}]\t Estado: el puerto responde Tiempo: {millis}ms Dirección: {Host}");
                    lastStatus = IPStatus.Success;
                }
                else if(lastStatus != IPStatus.Success)
                {
                    PrintLineMsg($"[{DateTime.Now}]\t Estado: el puerto responde Tiempo: {millis}ms Dirección: {Host}");
                    lastStatus = IPStatus.Success;
                }
                successCount++;
                timeSumatory += millis;
            }
            else
            {
                Console.WriteLine("Estado: el puerto "+ Port +" NO responde. Tiempo de corte: " + millis + "ms. Dirección: " + Host);
                if (lastStatus != IPStatus.TimedOut)
                {
                    PrintLineMsg($"[{DateTime.Now}]\t Estado: el puerto {Port} NO responde. Tiempo de corte: {millis}ms. Dirección: {Host}");
                    lastStatus =IPStatus.TimedOut;
                }
                failCount++;
            }
                
            if(!NoDelay)
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private static bool IsPortOpen()
        {
            bool isPortOpen = false;
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {                    
                    isPortOpen = tcpClient.ConnectAsync(Host, Port).Wait(3000);
                }
                catch {}
            }
            return isPortOpen;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            PrintLineMsg("Parando...", false);
            running = false;
        }
    }
}
