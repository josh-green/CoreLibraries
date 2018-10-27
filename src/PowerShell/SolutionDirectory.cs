#region � Copyright Web Applications (UK) Ltd, 2014.  All rights reserved.
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.PowerShell
{
    /// <summary>
    /// Information about solutions in a directory.
    /// </summary>
    /// <remarks></remarks>
    [UsedImplicitly]
    public class SolutionDirectory : InitialisedSingleton<string, SolutionDirectory>, IDisposable
    {
        /// <summary>
        /// The task cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Task that retrieves solutions.
        /// </summary>
        private Task<ConcurrentBag<Project>> _projects;

        /// <summary>
        /// Task that retrieves solutions.
        /// </summary>
        private Task<ConcurrentBag<Solution>> _solutions;

        /// <summary>
        /// Task that retrieves solutions.
        /// </summary>
        private Task<ConcurrentBag<SolutionDirectory>> _subDirectories;

        /// <summary>
        /// Holds the file system watcher which monitors changes.
        /// </summary>
        private FileSystemWatcher _watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionDirectory"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <remarks></remarks>
        private SolutionDirectory([NotNull] string path)
            : base(path)
        {
        }

        /// <summary>
        /// The full path to the directory.
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public string FullPath
        {
            get { return Key; }
        }

        /// <summary>
        /// Whether the directory actually exists in the file system.
        /// </summary>
        public bool Exists { get; private set; }

        /// <summary>
        /// Solutions in the directory
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public IEnumerable<Solution> Solutions
        {
            get { return _solutions.Result ?? Enumerable.Empty<Solution>(); }
        }

        /// <summary>
        /// Gets all solutions recursively for the directory.
        /// </summary>
        /// <remarks></remarks>
        [NotNull]
        [UsedImplicitly]
        public IEnumerable<Solution> AllSolutions
        {
            get { return _solutions.Result.Union(_subDirectories.Result.SelectMany(d => d.AllSolutions)); }
        }

        /// <summary>
        /// Solutions in the directory
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public IEnumerable<Project> Projects
        {
            get { return _projects.Result ?? Enumerable.Empty<Project>(); }
        }

        /// <summary>
        /// Gets all projects recursively for the directory.
        /// </summary>
        /// <remarks></remarks>
        [NotNull]
        [UsedImplicitly]
        public IEnumerable<Project> AllProjects
        {
            get { return _projects.Result.Union(_subDirectories.Result.SelectMany(d => d.AllProjects)); }
        }

        /// <summary>
        /// Solutions in the directory
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public IEnumerable<SolutionDirectory> SubDirectories
        {
            get { return _subDirectories.Result ?? Enumerable.Empty<SolutionDirectory>(); }
        }

        /// <summary>
        /// Gets all known solutions.
        /// </summary>
        /// <remarks></remarks>
        [NotNull]
        public static IEnumerable<SolutionDirectory> All
        {
            get { return Singletons.Values; }
        }

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // If we've not cancelled tasks do so.
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            // Dispose watcher.
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }

            // Dispose tasks
            if (_projects != null)
            {
                _projects.Dispose();
                _projects = null;
            }

            if (_solutions != null)
            {
                _solutions.Dispose();
                _solutions = null;
            }

            if (_subDirectories != null)
            {
                _subDirectories.Dispose();
                _subDirectories = null;
            }

            // Dispose cancellation token source.
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
        #endregion

        /// <summary>
        /// Gets the solution directory for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [NotNull]
        [UsedImplicitly]
        public static SolutionDirectory Get([NotNull] string path)
        {
            return GetSingleton(path);
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Initialise()
        {
            // Check if the directory exists.
            Exists = Directory.Exists(FullPath);

            // If this is not a valid directory return empty.
            if (!Exists)
            {
                _solutions = Task<ConcurrentBag<Solution>>.Factory.StartNew(() => new ConcurrentBag<Solution>());
                _projects = Task<ConcurrentBag<Project>>.Factory.StartNew(() => new ConcurrentBag<Project>());
                _subDirectories =
                    Task<ConcurrentBag<SolutionDirectory>>.Factory.StartNew(
                        () => new ConcurrentBag<SolutionDirectory>());
                return;
            }

            // Create watcher for this directory
            _watcher = new FileSystemWatcher(FullPath, "*.sln|*proj") {IncludeSubdirectories = false};

            // Add watch event.
            _watcher.Changed += FileChanged;
            _watcher.Created += FileChanged;
            _watcher.Deleted += FileChanged;
            _watcher.Renamed += FileRenamed;
            _watcher.Error += WatcherError;

            // Begin raising events
            _watcher.EnableRaisingEvents = true;

            Reload();
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        /// <remarks></remarks>
        private void Reload()
        {
            // Create a new cancellation token source
            CancellationTokenSource newSource = new CancellationTokenSource();

            // Swap out old source
            CancellationTokenSource oldSource = Interlocked.Exchange(
                ref _cancellationTokenSource,
                new CancellationTokenSource());
            if (oldSource != null)
            {
                // Cancel old source.
                if (oldSource.IsCancellationRequested)
                    oldSource.Cancel();

                // Dispose old source
                oldSource.Dispose();
            }

            // Get cancellation token.
            CancellationToken ct = newSource.Token;

            // Create a ConcurrentBag of parsed solution files in this directory
            // Skips hidden files & folders automatically.
            // Supports cancellation
            Task<ConcurrentBag<Solution>> oldSolutionTask = Interlocked.Exchange(
                ref _solutions,
                Task<ConcurrentBag<Solution>>.Factory.StartNew(
                    () => new ConcurrentBag<Solution>(
                        Directory.EnumerateFiles(FullPath, "*.sln", SearchOption.TopDirectoryOnly)
                            .TakeWhile(fileInfo => !ct.IsCancellationRequested)
                            .Select(fn => new FileInfo(fn))
                            .Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
                            .Select(f => Solution.Get(f.FullName))),
                    ct));

            // If we had a task, dispose it.
            if (oldSolutionTask != null)
                oldSolutionTask.Dispose();

            // Create a ConcurrentBag of project files in this directory
            // Skips hidden files & folders automatically.
            // Supports cancellation
            Task<ConcurrentBag<Project>> oldProjectsTask = Interlocked.Exchange(
                ref _projects,
                Task<ConcurrentBag<Project>>.Factory.StartNew(
                    () => new ConcurrentBag<Project>(
                        Directory.EnumerateFiles(FullPath, "*proj", SearchOption.TopDirectoryOnly)
                            .TakeWhile(fileInfo => !ct.IsCancellationRequested)
                            .Select(fn => new FileInfo(fn))
                            .Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
                            .Select(f => Project.Get(f.FullName))),
                    ct));

            // If we had a task, dispose it.
            if (oldProjectsTask != null)
                oldProjectsTask.Dispose();

            // Asynchronously start loading all sub-directories.
            // Supports cancellation
            Task<ConcurrentBag<SolutionDirectory>> oldDirectoriesTask = Interlocked.Exchange(
                ref _subDirectories,
                Task<ConcurrentBag<SolutionDirectory>>.Factory.StartNew(
                    () => new ConcurrentBag<SolutionDirectory>(
                        Directory.EnumerateDirectories(FullPath, string.Empty, SearchOption.TopDirectoryOnly)
                            .TakeWhile(fileInfo => !ct.IsCancellationRequested)
                            .Select(dn => new DirectoryInfo(dn))
                            .Where(d => (d.Attributes & FileAttributes.Hidden) == 0)
                            .Select(d => Get(d.FullName))),
                    ct));

            // If we had a task, dispose it.
            if (oldDirectoriesTask != null)
                oldDirectoriesTask.Dispose();
        }

        /// <summary>
        /// Occurs when a file is renamed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.IO.RenamedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Catches errros from the watcher (buffer overflow)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.IO.ErrorEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void WatcherError(object sender, ErrorEventArgs e)
        {
            // If we get an error then reload
            Reload();
        }

        /// <summary>
        /// Files the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.IO.FileSystemEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}