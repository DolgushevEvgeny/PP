using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smokers
{
    public enum Material
    {
        Paper=1,
        Tobacco=2,
        Match=3
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("Wrong argument amount");
            }
            else if (args[0] == "table")
            {
                CTable table = new CTable();
                table.Start();
            } 
            else if (args[0] == "agent")
            {
                CAgent agent = new CAgent();
                agent.Start();
            } 
            else if (args[0] == "smoker")
            {
                if (args.Length < 2)
                {
                    throw new Exception("Wrong argument amount");
                }
                CSmoker smoker = new CSmoker(int.Parse(args[1]));
                smoker.Start();

            } else throw new Exception("Wrong argument: " + args[0]);
        }
    }
}
