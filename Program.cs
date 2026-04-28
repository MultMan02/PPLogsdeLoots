using System.Globalization;

namespace PPLogsdeLoots;

class Program
{
    const string REGULAREXPRESSION = @"^[A-ZÀ-Ÿ][a-zà-ÿ]+(_[A-Za-zÀ-ÿ]*)?_[A-ZÀ-Ÿ][a-zà-ÿ]+;[0-9]+[\.,][0-9]+;[A-ZÀ-Ÿ][a-zà-ÿ]+(_[A-Za-zÀ-ÿ]*)?_[A-ZÀ-Ÿ][a-zà-ÿ]+;[0-9]{4}-(0?[1-9]|1[0-2])-[0-9]{2}\s(:?[0-9]{2}){3}$";
    const double ALIQUOTA = 0.05d;
    static int validItemCount = 0;
    static double totalImpostos = 0.0d;
    
    static void DoSequential(string filePath)
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        using (StreamReader reader = new(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, REGULAREXPRESSION))
                {
                    validItemCount += 1;
                    
                    string temp = "";
                    double itemValue = 0.0d;
                    int index = 0;

                    string[] colums = line.Split(";");
                    
                    itemValue = double.Parse(colums[1], CultureInfo.InvariantCulture);
                    
                    totalImpostos += itemValue * ALIQUOTA;
                }
                else
                {
                    break;
                }
            }
        } 
        stopwatch.Stop();
        
        Console.WriteLine($"=== SISTEMA SEQUENCIAL ===\nItens válidos: {validItemCount}\nTotal de impostos: {totalImpostos:F4}\nTempo de cálculo: {stopwatch.ElapsedMilliseconds} ms");
    }

    static void DoParalel(string filePath, int threadsNumber)
    {
        
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
        
        DoSequential(filePath);
        
        DoParalel(filePath, threadsNumber);
    }
}