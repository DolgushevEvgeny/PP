﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;

namespace Smokers
{
    enum Command
    {
        Paper = 1,
        Tobacco = 2,
        Match = 3,
        LetAgentCraft = 4,
        CanSmokerSmokePaper = 5,
        CanSmokerSmokeMatch = 6,
        CanSmokerSmokeTobacco = 7,
        EndSmokerSmoking = 8,
        PaperOnReady = 9,
        MatchOnReady = 10,
        TobaccoOnReady = 11
    }

    enum Response
    {
        Ok = 1,
        YouCanCraft = 2,
        None = 3,
    }

    class CTable
    {
        enum Material
        {
            Paper = 1,
            Tobacco = 2,
            Match = 3,
            None = 4
        }

        private NamedPipeServerStream pipeServer;
        private const string PipeName = "Table";
        private Material material = Material.None;
        private bool isPaperOnReady = false;
        private bool isMatchOnReady = false;
        private bool isTobaccoOnReady_ = false;

        private static NamedPipeServerStream CreatePipeServer()
        {
            return new NamedPipeServerStream(
                PipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous
            );
        }

        public void Start()
        {
            Console.WriteLine("Товар закончился.Курильщики выстроились.Ждут раздачи.");
            while (true)
            {
                pipeServer = CreatePipeServer();
                pipeServer.WaitForConnection();
                try
                {
                    var command = (Command)pipeServer.ReadByte();
                    switch (command)
                    {
                        case Command.Paper:
                            AgentDropPaper();
                            break;
                        case Command.Match:
                            AgentDropMatch();
                            break;
                        case Command.Tobacco:
                            AgentDropTobacco();
                            break;
                        case Command.CanSmokerSmokeMatch:
                            CanSmokerSmokeMatch();
                            break;
                        case Command.CanSmokerSmokePaper:
                            CanSmokerSmokePaper();
                            break;
                        case Command.CanSmokerSmokeTobacco:
                            CanSmokerSmokeTobacco();
                            break;
                        case Command.LetAgentCraft:
                            CanAgentCraft();
                            break;
                        case Command.EndSmokerSmoking:
                            EndingSmokerSmoking();
                            break;
                        case Command.PaperOnReady:
                            PaperOnReady();
                            break;
                        case Command.MatchOnReady:
                            MatchOnReady();
                            break;
                        case Command.TobaccoOnReady:
                            TobaccoOnReady();
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                    pipeServer.WaitForPipeDrain();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void EndingSmokerSmoking()
        {
            material = Material.None;
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void AgentDropPaper()
        {
            material = Material.Paper;
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void AgentDropMatch()
        {
            material = Material.Match;
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void AgentDropTobacco()
        {
            material = Material.Tobacco;
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void CanSmokerSmokeMatch()
        {
            if (material == Material.Match)
            {
                pipeServer.WriteByte((byte)Response.Ok);
            }
            else
            {
                pipeServer.WriteByte((byte)Response.None);
            }
        }
        private void CanSmokerSmokePaper()
        {
            if (material == Material.Paper)
            {
                pipeServer.WriteByte((byte)Response.Ok);
            }
            else
            {
                pipeServer.WriteByte((byte)Response.None);
            }
        }
        private void CanSmokerSmokeTobacco()
        {
            if (material == Material.Tobacco)
            {
                pipeServer.WriteByte((byte)Response.Ok);
            }
            else
            {
                pipeServer.WriteByte((byte)Response.None);
            }
        }

        private void CanAgentCraft()
        {
            if ((material == Material.None) && isPaperOnReady && isMatchOnReady && isTobaccoOnReady_)
            {
                pipeServer.WriteByte((byte)Response.YouCanCraft);
            }
            else
            {
                pipeServer.WriteByte((byte)Response.None);
            }
        }

        private void PaperOnReady()
        {
            isPaperOnReady = true;
            Console.WriteLine("Значение курильщика 1 - {0}", isPaperOnReady);
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void MatchOnReady()
        {
            isMatchOnReady = true;
            Console.WriteLine("Значение курильщика 2 - {0}", isMatchOnReady);
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void TobaccoOnReady()
        {
            isTobaccoOnReady_ = true;
            Console.WriteLine("Значение курильщика 3 - {0}", isTobaccoOnReady_);
            //SetIsTobaccoOnReady(true);
            pipeServer.WriteByte((byte)Response.Ok);
        }

        private void SetIsTobaccoOnReady(bool value)
        {
            Console.WriteLine("Значение {0}", value);
            isTobaccoOnReady_ = value;
        }
    }
}
