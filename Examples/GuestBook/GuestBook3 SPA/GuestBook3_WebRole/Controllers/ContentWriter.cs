using System.IO;
using System.Net.Http;

namespace GuestBook3_WebRole.Controllers
{
    public class ContentWriter
    {
        private MemoryStream _stream;

        public ContentWriter() 
        {
        }

/*
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            return base.GetLocalFileName(headers);
        }

        public override System.IO.Stream GetStream(HttpContent parent, System.Net.Http.Headers.HttpContentHeaders headers)
        {
            //return base.GetStream(parent, headers);

            _stream = new MemoryStream();

            return _stream;
        }
*/
    }
}