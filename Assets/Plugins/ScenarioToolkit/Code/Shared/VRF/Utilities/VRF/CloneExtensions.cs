using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ScenarioToolkit.Shared.VRF.Utilities.VRF
{
    /// <summary>
    /// Расширения для копирования классов
    /// </summary>
    public static class CloneExtensions
    {
        // https://stackoverflow.com/questions/1031023/how-can-i-copy-an-instance-of-a-class-in-c-sharp
        // возможна критическая ошибка remote code execution
        public static T SerializableClone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", nameof(source));

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default;

            using var stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
}