using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace rxml
{
    public static class ReactiveXml
    {
        public static IObservable<XmlReader> ToObservable(this XmlReader reader)
        {
            return Observable.Create<XmlReader>(observer =>
            {
                try
                {
                    while (reader.Read())
                        observer.OnNext(reader);
                    observer.OnCompleted();
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return reader;
            });
        }
    }

}
