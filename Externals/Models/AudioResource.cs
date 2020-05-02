namespace Externals.Models
{
    public class AudioResource
    {
        public AudioResource(string fileName, byte[] audioBytes)
        {
            FileName = fileName;
            AudioBytes = audioBytes;
        }

        public string FileName { get; set; }
        public byte[] AudioBytes { get; set; }
    }
}