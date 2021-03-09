using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Lab1
{
    public class Thread
    {
        public int ThreadId { get; private set; }

        public int ProcessId { get; private set; }

        public int ThreadTime { get; private set; }

        public int IterationTime { get; private set; }

        FormMain formMain;

        public Thread(int ThreadId, int ProcessId, FormMain formMain) {
            this.ThreadId = ThreadId;
            this.ProcessId = ProcessId;
            this.formMain = formMain;

            Random random = new Random();
            ThreadTime = random.Next() % 25 + 5;
            IterationTime = random.Next() % 5 + 5;
        }

        public void SubtractThreadIterationTime()
        {
            ThreadTime -= IterationTime;
        }

        public void Start() {
            formMain.AddThread(ProcessId, ThreadId, IterationTime);
            formMain.Draw(formMain.g);
            formMain.Refresh();
        }
    }
}
