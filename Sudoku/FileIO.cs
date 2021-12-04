using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sudoku
{
	class FileIO
	{
		public static void Save(string filePath, Object obj)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, obj);
			stream.Close();
		}

		public static T Load<T>(string filePath)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			T obj = (T)formatter.Deserialize(stream);
			stream.Close();

			return obj;
		}

		public static async Task SaveText(string text, string filePath)
		{
			await File.WriteAllTextAsync(filePath, text);
		}

		public static async Task SaveText(string[] lines, string filePath)
		{
			await File.WriteAllLinesAsync(filePath, lines);
		}

		public static string LoadText(string filePath)
		{
			return System.IO.File.ReadAllText(filePath);
		}
	}
}
