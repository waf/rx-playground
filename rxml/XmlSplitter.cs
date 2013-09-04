using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace rxml
{
    class XmlSplitter
    {
        private static Func<XmlReader, String> fileNamer;

        public static async void Split(String filename, String splitElement, Func<XmlReader, String> splitFileNamer)
        {
            fileNamer = splitFileNamer;

            var result = XmlReader.Create(filename).ToObservable()
                    .Where(reader => reader.NodeType == XmlNodeType.Element && reader.Name == splitElement)
                    .Subscribe(WriteFile);
        }

        private static void WriteFile(XmlReader nodeReader) 
        {
            var fs = new FileStream(fileNamer(nodeReader), FileMode.Create, FileAccess.Write, FileShare.Write, 1, true);
            var bytes = Encoding.UTF8.GetBytes(nodeReader.ReadOuterXml());
            using (fs)
            {
                fs.WriteAsync(bytes, 0, bytes.Length).ToObservable();
            }
        }
    }
}
