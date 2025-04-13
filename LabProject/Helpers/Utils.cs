using System.Text.Json;
using System.Text;

namespace LabProject.Helpers
{
    public class Utils
    {
        private static readonly Utils _instance = new Utils();
        public static Utils Instance => _instance;

        private Utils() { }

        public byte[] ExportToJson<T>(IEnumerable<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return Encoding.UTF8.GetBytes(json);
        }
    }
}