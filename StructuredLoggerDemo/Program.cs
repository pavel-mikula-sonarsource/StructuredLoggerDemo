using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using System.Linq;

namespace StructuredLoggerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "build.binlog");
            var root = BinaryLog.ReadBuild(path);
            // Find interesting node
            //root.VisitAllChildren<TreeNode>(x =>
            //{
            //    if (x.ToString().Contains(".Rules."))
            //    {
            //        Console.WriteLine(x.Parent.GetType().Name + " / " + x.GetType().Name + ": " + x.ToString());
            //    }
            //    if (x.ToString().Contains("Total analyzer execution time"))
            //    {
            //        Console.WriteLine(x.Parent.GetType().Name + " / " + x.GetType().Name + ": " + x.ToString());
            //    }
            //});
            // Read/Parse
            root.VisitAllChildren<CscTask>(csc =>
            {
                var folder = CscTaskAnalyzer.Analyze(csc);
                foreach(var analyzer in folder.Children.OfType<Folder>().Where(x => x.Name.Contains("SonarAnalyzer.")))
                {
                    foreach (var message in analyzer.Children.OfType<Message>())
                    {
                        Console.WriteLine(message.Text); // All message, including ones that we're not interested in
                    }
                }
                // Manual way
                //foreach (var message in x.Children.OfType<Message>())
                //{
                //    Console.WriteLine(message.Text); // All message, including ones that we're not interested in
                //}
            });
            Console.ReadKey();
        }
    }
}
