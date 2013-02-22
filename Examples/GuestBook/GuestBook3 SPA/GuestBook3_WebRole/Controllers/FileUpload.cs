using System.IO;
using Newtonsoft.Json;

namespace GuestBook3_WebRole.Controllers
{
    public class FileUpload<T>
    {
        private readonly string _RawValue;
 
        public T Value { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public byte[] Buffer { get; set; }
 
        public FileUpload(byte[] buffer, string mediaType, string fileName, string value)
        {
            Buffer = buffer;
            MediaType = mediaType;
            FileName = fileName.Replace("\"","");
            _RawValue = value;
 
            Value = JsonConvert.DeserializeObject<T>(_RawValue);
        }
 
        public void Save(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var NewPath = Path.Combine(path, FileName);
            if (File.Exists(NewPath))
            {
                File.Delete(NewPath);
            }
 
            File.WriteAllBytes(NewPath, Buffer);
 
            var Property = Value.GetType().GetProperty("FileName");
            Property.SetValue(Value,FileName, null);
        }
    }
}