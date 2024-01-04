﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CommonFuncion.Extensions
{
	public static class ByteExt
	{
		public static string Encode(
			this byte[] self
		)
		{
			return Encoding.Default.GetString(self);
		}

		public static string ToBase64(
			this byte[] self
		)
		{
			return Convert.ToBase64String(self);
		}

		public static byte[] Compress(
			this byte[] self, string fileName
		)
		{
			using MemoryStream outStream = new MemoryStream(self);
			using (ZipArchive archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
			{
				ZipArchiveEntry fileInArchive = archive.CreateEntry(fileName, CompressionLevel.Optimal);
				using Stream entryStream = fileInArchive.Open();
				using MemoryStream fileToCompressStream = new MemoryStream(self);
				
				fileToCompressStream.CopyTo(entryStream);
			}

			return outStream.ToArray();
		}

		public static void CompressAndWrite(
			this byte[] self, string path, string fileName, string extension
		)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				ZipArchiveEntry demoFile = archive.CreateEntry($"{fileName}.{extension}");

				using Stream entryStream = demoFile.Open();
				using StreamWriter streamWriter = new StreamWriter(entryStream);
				string stringValue = Encoding.UTF8.GetString(self);

				streamWriter.Write(stringValue);
			}

			using FileStream fileStream = new FileStream($"{path}{fileName}.zip", FileMode.Create);

			memoryStream.Seek(0, SeekOrigin.Begin);
			memoryStream.CopyTo(fileStream);
		}

		public static void WriteFile(
			this byte[] self, string path, string fileName, string extension
		)
		{
			using MemoryStream fileToCompressStream = new MemoryStream(self);
			using FileStream fileStream = new FileStream($"{path}{fileName}.{extension}", FileMode.Create);
			
			fileToCompressStream.Seek(0, SeekOrigin.Begin);
			fileToCompressStream.CopyTo(fileStream);
		}
	}
}