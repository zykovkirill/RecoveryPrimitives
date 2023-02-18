#region Lock
int x = 0;
object locker = new object();
for (int i = 1; i < 6; i++)
{
    Thread thread = new(Print);
    thread.Name = $"Поток(lock) - {i}";
    thread.Start();
}

void Print()
{
    lock (locker)
    {
        x = 1;
        for (int i = 1; i < 6; i++)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - {x}");
            x++;
            Thread.Sleep(1000);
        }
    }
}
#endregion

#region Monitor
int y = 0;
object lockerMonitor = new();
for(int i = 1; i< 6;i++)
{
    Thread thread= new(PrintMonitor);
    thread.Name = $"Поток(Monitor) -{i}";
    thread.Start();
}

void PrintMonitor()
{
    bool acquiredLock = false;
    try
    {
        Monitor.Enter(lockerMonitor, ref acquiredLock);
        y = 1;
        for(int i=1;i <6; i++)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} - {x}");
            y++;
            Thread.Sleep(1000);
        }
    }
    finally
    {
        if(acquiredLock)
        {
            Monitor.Exit(lockerMonitor);
        }
    }
}
#endregion

#region AutoResetEvent
int z = 0;
var autoResetEvent = new AutoResetEvent(true);
for (int i = 1; i < 6; i++)
{
    Thread thread = new(PrintAutoResetEvent);
    thread.Name = $"Поток(AutoResetEvent) - {i}";
    thread.Start();
}

void PrintAutoResetEvent()
{
    autoResetEvent.WaitOne();
    z = 1;
    for (int i = 1; i < 6; i++)
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} - {x}");
        z++;
        Thread.Sleep(1000);
    }
    autoResetEvent.Set();
}
#endregion

#region Mutex
int w = 0;
var mutex = new Mutex();
for (int i = 1; i < 6; i++)
{
    Thread thread = new(PrintMutex);
    thread.Name = $"Поток(Mutex) - {i}";
    thread.Start();
}

void PrintMutex()
{
    mutex.WaitOne();
    w = 1;
    for (int i = 1; i < 6; i++)
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} - {x}");
        w++;
        Thread.Sleep(1000);
    }
    mutex.ReleaseMutex();
}
#endregion

#region Semaphore

for (int i = 1; i < 6; i++)
{
    Driver reader = new Driver(i);
}
class Driver
{

    static Semaphore sem = new Semaphore(2, 2);
    Thread myThread;
    int count = 3;

    public Driver(int i)
    {
        myThread = new Thread(Drive);
        myThread.Name = $"Водитель {i}";
        myThread.Start();
    }

    public void Drive()
    {
        while (count > 0)
        {
            sem.WaitOne();  

            Console.WriteLine($"{Thread.CurrentThread.Name} заезжает на платную дорогу");

            Console.WriteLine($"{Thread.CurrentThread.Name} едет");
            Thread.Sleep(1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} покидает дорогу");

            sem.Release();  

            count--;
            Thread.Sleep(1000);
        }
    }
}
#endregion