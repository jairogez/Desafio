using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp3
{
  class Program
  {

    static void Main(string[] args)
    {
      try{
        if (args.Length == 0){
          Console.WriteLine("Faltou informar o caminho do arquivo.");
          return;
        }

        var path = args[0].StartsWith('.') ? Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + args[0].Substring(2,args[0].Length-2) : args[0];
        Loc(path);
      }
      catch(Exception e){
        Console.WriteLine(e.Message);
      }
    }

    public static void Loc(string path)
    {
      int loc = 0;
      using (StreamReader sr = new StreamReader(path)){
        string line;
        bool block = false;

        while ((line = sr.ReadLine()) != null){
          if (block)
            if (line.Contains(@"*/")){
              line = line.Substring(0, line.IndexOf(@"*/"));
              block = false;
            }
            else
              continue;

          line = Regex.Replace(line, @"\""(.*?)\""", "str").TrimStart();
          line = Regex.Replace(line, @"/\*(.*?)\*/", "").TrimStart();

          if (line.Contains(@"//")){
            line = line.Substring(0, line.IndexOf(@"//"));
            if (String.IsNullOrWhiteSpace(line))
              continue;
          }

          if (String.IsNullOrWhiteSpace(line) || line.StartsWith(@"//"))
            continue;

          if (line.Contains(@"/*")){
            block = true;
            continue;
          }
          // Console.WriteLine(line);
          /* se chegou aqui, contem instrucao */
          loc++;
        }
      }
      Console.WriteLine("LoC = " + loc);
    }
  }
}