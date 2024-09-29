namespace KH.BuildingBlocks.Extentions.Methods;

public class ThreadLogger
{
  public ThreadLogger()
  {
  }
  private static object lockObj = new object();
  private static object rndLock = new object();

  public static void ShowThreadInformation(string taskName)
  {
    string msg = null;
    Thread thread = Thread.CurrentThread;
    //lock (lockObj)
    //{
    msg = string.Format("{0} thread information\n", taskName) +
          string.Format("   Background: {0}\n", thread.IsBackground) +
          string.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
          string.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
    //}
    Console.WriteLine(msg);
    //_logger.LogError("Thread ID: {0}", thread.ManagedThreadId);
  }
}
