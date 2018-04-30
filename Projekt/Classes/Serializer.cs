using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Projekt
{
    public class Serializer
    {
        public JsonSerializerSettings Settings { get; set; }= new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };


       

        /// <summary>
        ///     Writes the given object instance to a Json file.
        ///     <para>Object type must have a parameterless constructor.</para>
        ///     <para>
        ///         Only Public properties and variables will be written to the file. These can be any type though, even other
        ///         classes.
        ///     </para>
        ///     <para>
        ///         If there are public properties/variables that you do not want written to the file, decorate them with the
        ///         [JsonIgnore] attribute.
        ///     </para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">
        ///     If false the file will be overwritten if it already exists. If true the contents will be appended
        ///     to the file.
        /// </param>
        public void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            File.Create(filePath).Dispose();
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite, Settings);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        ///     Reads an object instance from an Json file.
        ///     <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents, Settings);
            }
            finally
            {
                reader?.Close();
            }
        }

    }
}