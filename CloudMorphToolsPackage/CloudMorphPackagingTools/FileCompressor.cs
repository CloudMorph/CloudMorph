using System.IO;
using System.IO.Compression;

namespace CloudMorphPackagingTools
{
    public class FileCompressor
    {
        public static void Compress(string sourcePath, string targetpath)
        {
            // Get the stream of the source file.
            using (var inFile = File.OpenRead(sourcePath))
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(sourcePath)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & Path.GetExtension(sourcePath) != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile = File.Create(targetpath))
                    {
                        using (var compress = new GZipStream(outFile, CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(compress);

                            //Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                            //    fi.Name, fi.Length.ToString(), outFile.Length.ToString());
                        }
                    }
                }
            }
        }

        public static void Decompress(string sourcePath, string targetpath)
        {
            // Get the stream of the source file.
            using (FileStream inFile = File.OpenRead(sourcePath))
            {
                //Create the decompressed file.
                using (FileStream outFile = File.Create(targetpath))
                {
                    using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))
                    {
                        // Copy the decompression stream 
                        // into the output file.
                        Decompress.CopyTo(outFile);

                        //Console.WriteLine("Decompressed: {0}", fi.Name);

                    }
                }
            }
        }

    }
}