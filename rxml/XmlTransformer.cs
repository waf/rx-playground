using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Xml.Xsl;

namespace rxml
{
    class XmlTransformer
    {
        private const string FileFilter = "split-*.xml";
        private static readonly XslCompiledTransform transform = new XslCompiledTransform();

        public static void Transform(string path, string transformPath)
        {
            transform.Load(transformPath);
            SetupDirectoryWatcher(path, transform);
        }

        private static void SetupDirectoryWatcher(string path, XslCompiledTransform transform)
        {
            FileSystemWatcher fsw = new FileSystemWatcher(path, FileFilter);
            fsw.EnableRaisingEvents = true;

            var fileCreated = Observable.FromEventPattern<FileSystemEventArgs>(fsw, "Changed");
            fileCreated
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(pattern =>
                    transform.Transform(pattern.EventArgs.FullPath, 
                                        pattern.EventArgs.FullPath.Replace("split-", "transformed-")));
        }
    }
}
