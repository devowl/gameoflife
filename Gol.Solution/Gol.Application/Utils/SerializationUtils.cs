using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Gol.Core.Controls.Models;

namespace Gol.Application.Utils
{
    /// <summary>
    /// Serialization methods.
    /// </summary>
    public class SerializationUtils
    {
        /// <summary>
        /// Save object to file.
        /// </summary>
        /// <param name="fileStream">File stream.</param>
        /// <param name="sourceObject"></param>
        public static void Save(Stream fileStream, object sourceObject)
        {
            var type = sourceObject.GetType();
            var serializer = new DataContractJsonSerializer(type);
            serializer.WriteObject(fileStream, sourceObject);
        }
        
        /// <summary>
        /// Read object from stream.
        /// </summary>
        /// <typeparam name="TValue">Object type.</typeparam>
        /// <param name="fileStream">File stream.</param>
        /// <returns>Deserialized object.</returns>
        public static TValue Read<TValue>(Stream fileStream)
        {
            var type = typeof(MonoLifeGrid<bool>);
            var serializer = new DataContractJsonSerializer(type);
            return (TValue)serializer.ReadObject(fileStream);
        }
    }
}