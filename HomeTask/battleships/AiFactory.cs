using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
    public interface IAiFactory
    {
        Ai CreateAi();
    }

    class AiFactory : IAiFactory
    {
        private readonly string aiExePath;
        private readonly ProcessMonitor processMonitor;

        public AiFactory(string aiExePath, ProcessMonitor processMonitor)
        {
            this.aiExePath = aiExePath;
            this.processMonitor = processMonitor;
        }

        public Ai CreateAi()
        {
            return new Ai(aiExePath, processMonitor);
        }
    }
}
