using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VRF.Utilities.Extensions
{
    public static class VrfClassExtensions
    {
        // https://stackoverflow.com/questions/1031023/how-can-i-copy-an-instance-of-a-class-in-c-sharp
        // возможна критическая ошибка remote code execution
        public static T DeepCopy<T>(this T other)
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }
}