using System.Globalization;

namespace PPLogsdeLoots;

class Program
{
    const string REGULAREXPRESSION = @"^[A-ZÀ-Ÿ][a-zà-ÿ]+(_[A-Za-zÀ-ÿ]*)?_[A-ZÀ-Ÿ][a-zà-ÿ]+;[0-9]+[\.,][0-9]+;[A-ZÀ-Ÿ][a-zà-ÿ]+(_[A-Za-zÀ-ÿ]*)?_[A-ZÀ-Ÿ][a-zà-ÿ]+;[0-9]{4}-(0?[1-9]|1[0-2])-[0-9]{2}\s(:?[0-9]{2}){3}$";
    const double ALIQUOTA = 0.05d;
    static int validItemCount = 0;
    static double totalImpostos = 0.0d;
    static double totalImpostosParalelos = 0.0d;
    static object locker = new object();
    static List<Thread> threads = new List<Thread>();

    static List<double> LoadData(string filePath)
    {
        var dataList = new List<double>();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, REGULAREXPRESSION))
                {
                    validItemCount += 1;
                    
                    string[] colums = line.Split(";");
                    
                    dataList.Add(double.Parse(colums[1], CultureInfo.InvariantCulture));
                }
            }
        }
        
        return dataList;
    }
    
    static void DoSequential(List<double> dataList)
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

        foreach (var value in dataList)
        {
            totalImpostos += value * ALIQUOTA;
        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"=== SISTEMA SEQUENCIAL ===\nItens válidos: {validItemCount}\nTotal de impostos: {totalImpostos:F2}\nTempo de cálculo: {stopwatch.ElapsedMilliseconds} ms");
    }

    public static void ThreadProcess(int startPos, int endPos, List<double> dataList)
    {
        for (int i = startPos; i <= endPos; i++)
        {
            double tax = dataList[i] * ALIQUOTA;
            lock (locker)
            {
                totalImpostosParalelos += tax;
            }
        }
    }

    static void DoParalel(List<double> dataList, int threadsNumber)
    {
        int perThread = validItemCount / threadsNumber;
        int remainder = validItemCount % threadsNumber;

        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        for (int i = 0; i < threadsNumber; i++)
        {
            int startPos = i * perThread;
            int endPos = (i + 1) * perThread;

            if (i == 0)
            {
                startPos += i;
            }
            else if (i < threadsNumber - 1)
            {
                startPos += 1;
            }
            else
            {
                endPos += remainder;
            }
            
            Thread thread = new Thread(() => ThreadProcess(startPos, endPos, dataList));
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"=== SISTEMA PARALELO ({threadsNumber} threads) ===\nItens válidos: {validItemCount}\nTotal de impostos: {totalImpostosParalelos:F2}\nTempo de cálculo: {stopwatch.ElapsedMilliseconds} ms\nSpeedup: X");
    }
    
    static void Main(string[] args)
    {
        string filePath = "";
        int threadsNumber = 8;
        
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: PPLogsdeLoots.exe <filePath> <threadsNumber>");
            Environment.Exit(1);
        }
        else
        {
            if (File.Exists(args[0]))
            {
                filePath = args[0];
                
                if (args.Length > 1)
                {
                    threadsNumber = int.Parse(args[1]);
                }
            }
            else
            {
                Console.WriteLine("File not found: " + args[0]);
                Environment.Exit(1);
            }
        }
        
        var dataList = LoadData(filePath);
        
        DoSequential(dataList);
        
        Console.WriteLine("\n");
        
        DoParalel(dataList, threadsNumber);
    }
}