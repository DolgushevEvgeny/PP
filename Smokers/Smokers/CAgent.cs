using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace Smokers
{
    class CAgent
    {
        private const string PipeName = "Table";

        public CAgent() {}

        private NamedPipeClientStream CreatePipeClientStream()
        {
            return new NamedPipeClientStream(
                ".",
                PipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous
            );
        }

        public void Start()
        {
            Console.WriteLine("Посредник создан. Pipe: {0}.", PipeName);
            while (true)
            {
                Thread.Sleep(1000);
                var random = new Random();

                try
                {
                    var response = SendCommand(Command.LetAgentCraft);
                    while (response != Response.YouCanCraft)
                    {
                        Console.WriteLine("Я прячусь от копов.");
                        Thread.Sleep(1000);
                        response = SendCommand(Command.LetAgentCraft);
                    }

                    Console.WriteLine("Посредник собирает посылку.");
                    Thread.Sleep(2000);
                    int material = random.Next(1, 4);
                    Console.WriteLine("Посредник собрал посылку без {0}.", (Command)material);
                    Thread.Sleep(2000);
                    Console.WriteLine("Посредник сбросил посылку.");
                    SendCommand((Command)material);
                    Thread.Sleep(2000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void TryConnect(NamedPipeClientStream pipeStream)
        {
            try
            {
                pipeStream.Connect();
            }
            catch (FileNotFoundException)
            {
                Thread.Sleep(1000);
                TryConnect(pipeStream);
            }
        }

        private Response SendCommand(Command command)
        {
            var pipeStream = CreatePipeClientStream();
            TryConnect(pipeStream);
            pipeStream.WriteByte((byte)command);
            pipeStream.WaitForPipeDrain();
            var response = (Response)pipeStream.ReadByte();
            pipeStream.WaitForPipeDrain();
            return response;
        }
    }
}
