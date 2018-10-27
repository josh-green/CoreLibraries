﻿#region © Copyright Web Applications (UK) Ltd, 2014.  All rights reserved.
// Copyright (c) 2014, Web Applications UK Ltd
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Web Applications UK Ltd nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL WEB APPLICATIONS UK LTD BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.PowerShell
{
    /// <summary>
    /// Provide zip utilities.
    /// </summary>
    [UsedImplicitly]
    public static class Zip
    {
        /// <summary>
        /// Compresses the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="zipFile">The zip file.</param>
        /// <param name="force">if set to <see langword="true"/> overwrites zip if present.</param>
        /// <param name="backSlash">if set to <see langword="true"/> file paths will be encoded with a back-slash (\) separator; 
        /// otherwise they will be encoded with a forward-slash (/).</param>
        [UsedImplicitly]
        public static void Compress([NotNull] string directory, [NotNull] string zipFile, bool force, bool backSlash)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(
                    String.Format(
                        Resources.Zip_Compress_DirectoryNotFound,
                        directory));

            if (File.Exists(zipFile))
            {
                if (!force)
                    throw new InvalidOperationException(
                        String.Format(
                            Resources.Zip_Compress_CannotOverwriteExistingFile,
                            zipFile));

                File.Delete(zipFile);
            }

            ZipFile.CreateFromDirectory(directory, zipFile, CompressionLevel.Optimal, false, backSlash ? SlashEncoder.Back : SlashEncoder.Forward);
        }

        /// <summary>
        /// A UTF8 Encoder that converts all slashes to a target slash.
        /// </summary>
        private class SlashEncoder : UTF8Encoding
        {
            public static readonly SlashEncoder Forward = new SlashEncoder("/");
            public static readonly SlashEncoder Back = new SlashEncoder("\\");

            [NotNull]
            private readonly string _fromSlash;

            [NotNull]
            private readonly string _toSlash;

            private SlashEncoder([NotNull] string targetSlash)
            {
                _toSlash = targetSlash;
                _fromSlash = _toSlash == "\\" ? "/" : "\\";
            }

            public override byte[] GetBytes(string s)
            {
                s = s.Replace(_fromSlash, _toSlash);
                return base.GetBytes(s);
            }
        }

        /// <summary>
        /// Compresses the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="zipFile">The zip file.</param>
        /// <param name="force">if set to <see langword="true"/> overwrites files in the directory if present.</param>
        [UsedImplicitly]
        public static void Decompress([NotNull] string zipFile, [NotNull] string directory, bool force)
        {
            if (!File.Exists(zipFile))
                throw new FileNotFoundException(string.Format(Resources.Zip_Decompress_FileNotFound, zipFile));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (ZipArchive archive = ZipFile.Open(zipFile, ZipArchiveMode.Read))
            {
                Debug.Assert(archive != null);

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Debug.Assert(entry != null);

                    //Identifies the destination file name and path
                    string fileName = Path.Combine(directory, entry.FullName);
                    string fileDirectory = Path.GetDirectoryName(fileName);

                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);

                    Trace.WriteLine(fileName);

                    entry.ExtractToFile(fileName, force);
                }
            }
        }
    }
}