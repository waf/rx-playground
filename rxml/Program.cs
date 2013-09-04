using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace rxml
{
    class Program
    {
        static void Main(string[] args)
        {
            // clear out past runs
            var oldFiles = new DirectoryInfo("data/output/").GetFiles();
            foreach (var file in oldFiles)
                file.Delete();
            
            // set up transform pipeline
            XmlTransformer.Transform("data/output", "data/book.xslt");

            // start file splitter
            Func<XmlReader, String> splitFileNamer = r => String.Format("data/output/split-{0}.xml", r.GetAttribute("id"));
            XmlSplitter.Split("data/books.xml", "book", splitFileNamer);

            Console.Write("done");
            Console.ReadKey();
        }
    }
}
