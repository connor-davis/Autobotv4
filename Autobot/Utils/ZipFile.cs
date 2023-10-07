﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Autobot.Utils;

internal static class ZipFileUtil
{
    public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, IProgress<double> progress)
    {
        sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);

        var sourceFiles =
            new DirectoryInfo(sourceDirectoryName).GetFiles("*", SearchOption.AllDirectories);
        double totalBytes = sourceFiles.Sum(f => f.Length);
        long currentBytes = 0;

        using var archive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Create);
        
        foreach (var file in sourceFiles)
        {
            // NOTE: naive method to get sub-path from file name, relative to
            // input directory. Production code should be more robust than this.
            // Either use Path class or similar to parse directory separators and
            // reconstruct output file name, or change this entire method to be
            // recursive so that it can follow the sub-directories and include them
            // in the entry name as they are processed.
            var entryName = file.FullName.Substring(sourceDirectoryName.Length + 1);
            var entry = archive.CreateEntry(entryName);

            entry.LastWriteTime = file.LastWriteTime;

            using Stream inputStream = File.OpenRead(file.FullName);
            using var outputStream = entry.Open();
                
            Stream progressStream = new StreamWithProgress(inputStream,
                new BasicProgress<int>(i =>
                {
                    currentBytes += i;
                    progress.Report(currentBytes / totalBytes);
                }), null);

            progressStream.CopyTo(outputStream);
        }
    }

    public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, IProgress<double> progress)
    {
        using var archive = ZipFile.OpenRead(sourceArchiveFileName);
        double totalBytes = archive.Entries.Sum(e => e.Length);
        long currentBytes = 0;

        foreach (var entry in archive.Entries)
        {
            var fileName = Path.Combine(destinationDirectoryName, entry.FullName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            catch
            {
                // ignored
            }

            using (var inputStream = entry.Open())
            using(Stream outputStream = File.OpenWrite(fileName))
            {
                Stream progressStream = new StreamWithProgress(outputStream, null,
                    new BasicProgress<int>(i =>
                    {
                        currentBytes += i;
                        progress.Report(currentBytes / totalBytes);
                    }));

                inputStream.CopyTo(progressStream);
            }

            File.SetLastWriteTime(fileName, entry.LastWriteTime.LocalDateTime);
        }
    }
}