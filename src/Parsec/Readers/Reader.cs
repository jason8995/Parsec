﻿using System.IO;
using Newtonsoft.Json;
using Parsec.Common;
using Parsec.Helpers;
using Parsec.Shaiya.Core;

namespace Parsec.Readers
{
    public static class Reader
    {
        /// <summary>
        /// Reads a shaiya file format from a file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="options">Array of reading options</param>
        /// <typeparam name="T">Shaiya File Format Type</typeparam>
        /// <returns>T instance</returns>
        public static T ReadFromFile<T>(string path, params object[] options) where T : FileBase, new() =>
            FileBase.ReadFromFile<T>(path, options);

        /// <summary>
        /// Reads a shaiya file format from a buffer (byte array)
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="buffer">File buffer</param>
        /// <param name="options">Array of reading options</param>
        /// <typeparam name="T">Shaiya File Format Type</typeparam>
        /// <returns>T instance</returns>
        public static T ReadFromBuffer<T>(string name, byte[] buffer, params object[] options)
            where T : FileBase, new() => FileBase.ReadFromBuffer<T>(name, buffer, options);

        /// <summary>
        /// Reads a shaiya file format from a json file
        /// </summary>
        /// <param name="path">Path to json file</param>
        /// <typeparam name="T"><see cref="FileBase"/> type</typeparam>
        /// <returns><see cref="FileBase"/> instance</returns>
        public static T ReadFromJson<T>(string path) where T : FileBase, IJsonReadable
        {
            if (!FileHelper.FileExists(path))
                throw new FileNotFoundException($"File ${path} not found");

            if (path.Length < 6 || path.Substring(path.Length - 5, 5) != ".json")
                throw new FileLoadException("The provided file to deserialize must be a valid json file");

            // Read json file content
            string jsonContent = File.ReadAllText(path);

            // Deserialize into FileBase
            var deserializedObject = JsonConvert.DeserializeObject<T>(jsonContent);

            // Get file name without the ".json" extension
            string fileNameWithoutJsonExtension = Path.GetFileNameWithoutExtension(path);

            // Add proper Path to deserialized object
            if (deserializedObject != null)
            {
                var objectExtension = deserializedObject.Extension;

                // If file name is not long enough to have the extension, add it
                if (fileNameWithoutJsonExtension.Length < objectExtension.Length)
                {
                    deserializedObject.Path = $"{fileNameWithoutJsonExtension}.{objectExtension}";
                    return deserializedObject;
                }

                // Check if file extension matches the appropriate FileBase child extension
                // This is needed since a file could be called MobFox.3DC.json, meaning it already has
                // its extension after the ".json" part is removed
                var fileExtension = fileNameWithoutJsonExtension.Substring(
                    fileNameWithoutJsonExtension.Length - objectExtension.Length, objectExtension.Length);

                if (fileExtension != objectExtension)
                    deserializedObject.Path = $"{fileNameWithoutJsonExtension}.{objectExtension}";
                else
                    deserializedObject.Path = fileNameWithoutJsonExtension;
            }

            return deserializedObject;
        }
    }
}
