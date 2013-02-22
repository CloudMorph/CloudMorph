using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuestBook3_WebRole.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(HttpPostedFileBase file)
        {
            //var FilePath = "Path";
            //upload.Save(FilePath); //save the buffer
            //upload.Value.Insert(); //save font object to DB

            //return Request.CreateResponse(HttpStatusCode.OK, upload.Value);

            return null;
        }

        public Task<HttpResponseMessage> PostMultipartStream(int z, int y) //HttpRequestMessage request)
        {
/*
            // Create a stream provider for setting up output streams
            var ss = new ContentWriter();

            // Read the MIME multipart content using the stream provider we just created.
            var bodyparts1 = Request.Content.ReadAsMultipartAsync(ss).Result;

            // grab the posted stream  
            request.Content.ReadAsByteArrayAsync().ContinueWith(t =>
                                                                    {
                                                                        var b = t.Result;
                                                                    }).Wait();
*/

            // write it to   
/*
            using (FileStream fileStream = File.Create(string.Format(@"c:\projects\step\{0}", filename), (int)stream.Length))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }  
*/

            if (Request.Content.IsMimeMultipartContent())
            {
                //var streamProvider = new ContentWriter();
                string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads"); 
                var providerZ = new MultipartFormDataStreamProvider(root); 
/*
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);

                        return new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
                    });

                return await task;
*/
/*
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<HttpResponseMessage>(o =>
                    {

                        //string file1 = provider.BodyPartFileNames.First().Value;
                        // this is the file name on the server where the file was saved 

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("File uploaded.")
                        };
                    });
*/

                Request.Content.ReadAsMultipartAsync(providerZ).Wait(); 

                return null;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                                       "This request is not properly formatted"));
            }
            return null;

            // Check we're uploading a file
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            // Create the stream provider, and tell it sort files in my c:\temp\uploads folder
            var provider = new MultipartFormDataStreamProvider("c:\\temp\\uploads");

            // Read using the stream
            var bodyparts = Request.Content.ReadAsMultipartAsync(provider);

            // Create response.
            //return provider.ExecutePostProcessingAsync(); //.BodyPartFileNames.Select(kv => kv.Value);

        }

        //public  Task<List<string>> PostMultipartStream1()
        public Task<List<string>> PostMultipartStream1(int i)
        {
            // Verify that this is an HTML Form file upload request
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Create a stream provider for setting up output streams that saves the output under c:\tmp\uploads
            // If you want full control over how the stream is saved then derive from MultipartFormDataStreamProvider
            // and override what you need.
            var streamProvider = new MultipartFormDataStreamProvider("c:\\Temp");

            // Read the MIME multipart content using the stream provider we just created.
            //IEnumerable<HttpContent> bodyparts = await Request.Content.ReadAsMultipartAsync(streamProvider);
            //var bodyparts = 

            // The submitter field is the entity with a Content-Disposition header field with a "name" parameter with value "submitter"
            string submitter;
            //if (!bodyparts.TryGetFormFieldValue("submitter", out submitter))
            {
                submitter = "unknown";
            }

            // Get a dictionary of local file names from stream provider.
            // The filename parameters provided in Content-Disposition header fields are the keys.
            // The local file names where the files are stored are the values.
            //IDictionary<string, string> bodyPartFileNames = streamProvider.BodyPartFileNames;

            // Create response containing information about the stored files. */
            var result = new List<string>();
/*            result.Add(submitter);

            IEnumerable<string> localFiles = bodyPartFileNames.Select(kv => kv.Value);
            result.AddRange(localFiles);
*/

            return null;
        }

        public HttpResponseMessage Post([FromUri]string filename)
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            try
            {
                Stream fileStream = File.Create(HttpContext.Current.Server.MapPath("~/" + filename));
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (IOException)
            {
                //throw new HttpResponseException("A generic error occured. Please try again later.", HttpStatusCode.InternalServerError);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "A generic error occured. Please try again later." });
            }

            var response = new HttpResponseMessage {StatusCode = HttpStatusCode.Created};
            return response;
        }
    }
}
