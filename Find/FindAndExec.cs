using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

public class FindAndExec
{

  static List<string> files = new List<string>();

  static void DirSearch(string sDir, string pattern)
  {
      try
      {
        
          foreach (string d in Directory.GetDirectories(sDir))
          {
              foreach (string f in Directory.GetFiles(d))
              {
                  files.Add(f);
              }
              DirSearch(d, pattern);
          }
      }
      catch (System.Exception excpt)
      {
          Console.WriteLine(excpt.Message);
      }
  }

  public static void Main(string[] args)
  {

    if(args.Length == 4){
      
      string path = args[0];
      string pattern = args[1];
      string cmd = args[2];
      string arguments = args[3];
      
      if(Directory.Exists(path)){

        DirSearch(path, pattern);
        //numbers.ForEach(num => Console.WriteLine(num + ", "));

        foreach(var f in files){
         try {

            if(Regex.IsMatch(f, pattern, RegexOptions.IgnoreCase)){

              var process = new Process();
              var startInfo = new ProcessStartInfo();

              startInfo.WindowStyle = ProcessWindowStyle.Hidden;
              startInfo.FileName = cmd;
              startInfo.Arguments = arguments.Replace("{}",f);
              startInfo.RedirectStandardInput = true;
              startInfo.RedirectStandardOutput = true;
              startInfo.CreateNoWindow = true;
              startInfo.UseShellExecute = false;

              process.StartInfo = startInfo;
              process.Start();
              //Console.WriteLine("{0} {1}", process.ProcessName, process.Id);
              process.StandardInput.Flush();
              process.StandardInput.Close();
              process.WaitForExit();
              
              //Console.WriteLine(process.ExitCode);
              //Console.WriteLine("======");
              Console.WriteLine(process.StandardOutput.ReadToEnd().Replace("\n","").Replace("\r",""));
              //Console.WriteLine("===>{0}",arguments.Replace("{}",f));
            }

          } catch (RegexMatchTimeoutException e) {
            Console.WriteLine("Timeout after {0} seconds matching {1}.", e.MatchTimeout, e.Input);
          }
        }

      } else {
        Console.WriteLine("{0} not exists.", path);
      }

    } else {
      string appName = Thread.GetDomain().FriendlyName;
      Console.WriteLine("{0} path pattern cmd cmd-arguments",appName);
    }

  }
}