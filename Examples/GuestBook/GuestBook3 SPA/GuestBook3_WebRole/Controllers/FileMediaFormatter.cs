using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuestBook3_WebRole.Controllers
{
    public class FileMediaFormatter<T> : MediaTypeFormatter
    {
        public FileMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }
 
        public override bool CanReadType(Type type)
        {
            return type == typeof(FileUpload<T>);
        }
 
        public override bool CanWriteType(Type type)
        {
            return false;
        }
 
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
 
            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
 
            var Parts = content.ReadAsMultipartAsync().Result;
            var FileContent = Parts.Contents.First(x =>
                SupportedMediaTypes.Contains(x.Headers.ContentType));
 
            var DataString = "";
            foreach (var Part in Parts.Contents.Where(x => x.Headers.ContentDisposition.DispositionType == "form-data" 
                                                        && x.Headers.ContentDisposition.Name == "\"data\""))
            {
                var Data = Part.ReadAsStringAsync().Result;
                DataString = Data;
            }
 
            string FileName = FileContent.Headers.ContentDisposition.FileName;
            string MediaType = FileContent.Headers.ContentType.MediaType;
 
            using (var Imgstream = FileContent.ReadAsStreamAsync().Result)
            {
                byte[] Imagebuffer = ReadFully(Imgstream);
                //return new FileUpload<T>(Imagebuffer, MediaType, FileName, DataString);

                return null;
            }
        }
 
        private byte[] ReadFully(Stream input)
        {
            var Buffer = new byte[16 * 1024];
            using (var Ms = new MemoryStream())
            {
                int Read;
                while ((Read = input.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    Ms.Write(Buffer, 0, Read);
                }
                return Ms.ToArray();
            }
        }
 
 
    }
}