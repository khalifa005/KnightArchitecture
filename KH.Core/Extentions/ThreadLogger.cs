using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Application.Extentions
{
    public class ThreadLogger
    {
        public ThreadLogger()
        {
        }
        private static Object lockObj = new Object();
        private static Object rndLock = new Object();

        public static void ShowThreadInformation(String taskName)
        {
            String msg = null;
            Thread thread = Thread.CurrentThread;
            //lock (lockObj)
            //{
                msg = String.Format("{0} thread information\n", taskName) +
                      String.Format("   Background: {0}\n", thread.IsBackground) +
                      String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                      String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            //}
            Console.WriteLine(msg);
            //_logger.LogError("Thread ID: {0}", thread.ManagedThreadId);
        }
    }

}
