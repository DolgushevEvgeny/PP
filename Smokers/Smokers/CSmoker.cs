using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace Smokers
{
    class CSmoker
    {
        private const string PipeName = "Table";
        private int smokerNumber;

        public CSmoker(int number) { smokerNumber = number; }

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
            Console.WriteLine("Курильщик {0} создан. Pipe: {1}.", smokerNumber, PipeName);
            while (true)
            {
                try
                {
                    switch (smokerNumber)
                    {
                        case 1:
                            SendCommand(Command.PaperOnReady);
                            var response1 = SendCommand(Command.CanSmokerSmokePaper);
                            Waiting(smokerNumber, Command.CanSmokerSmokePaper, response1);                           
                            TwistAndSmoking(smokerNumber);
                            break;
                        case 2:
                            SendCommand(Command.MatchOnReady);
                            var response2 = SendCommand(Command.CanSmokerSmokeMatch);
                            Waiting(smokerNumber, Command.CanSmokerSmokeMatch, response2);
                            TwistAndSmoking(smokerNumber);
                            break;
                        case 3:
                            SendCommand(Command.TobaccoOnReady);
                            var response3 = SendCommand(Command.CanSmokerSmokeTobacco);
                            Waiting(smokerNumber, Command.CanSmokerSmokeTobacco, response3);
                            TwistAndSmoking(smokerNumber);
                            break;
                    }
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

        private void Waiting(int smokerNumber, Command command, Response response)
        {
            while (response != Response.Ok)
            {
                Console.WriteLine("Курильщик {0} ждет", smokerNumber);
                Thread.Sleep(1000);
                response = SendCommand(command);
            }
        }

        private void TwistAndSmoking(int smokerNumber)
        {
            Console.WriteLine("Курильщик {0} начинает курить.", smokerNumber);
            Thread.Sleep(1000);
            Console.WriteLine("Курильщик {0} выкурил.", smokerNumber);
            SendCommand(Command.EndSmokerSmoking);
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
