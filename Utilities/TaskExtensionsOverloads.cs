﻿#region © Copyright Web Applications (UK) Ltd, 2012.  All rights reserved.
// Copyright (c) 2012, Web Applications UK Ltd
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
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebApplications.Utilities
{
    ///<summary>
    /// Extensions to the reflection namespace.
    ///</summary>
    public static partial class TaskExtensions
    {
        #region 0 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TResult>(
            [NotNull] this Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 1 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TResult>(
            [NotNull] this Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 2 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TResult>(
            [NotNull] this Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 3 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(
            [NotNull] this Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 4 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TResult>(
            [NotNull] this Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 5 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
            [NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 6 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
            [NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult>
                beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 7 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
            [NotNull] this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, AsyncCallback, object, IAsyncResult>
                beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 8 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
            [NotNull] this
                Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, AsyncCallback, object, IAsyncResult>
                beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 9 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
            [NotNull] this
                Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, AsyncCallback, object, IAsyncResult>
                beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 10 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TArg10">The type of argument 10.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="arg10">Argument 10 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync
            <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
            [NotNull] this
                Func
                <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, AsyncCallback, object,
                IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            TArg10 arg10,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 11 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TArg10">The type of argument 10.</typeparam>
        /// <typeparam name="TArg11">The type of argument 11.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="arg10">Argument 10 of the begin call.</param>
        /// <param name="arg11">Argument 11 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync
            <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
            [NotNull] this
                Func
                <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, AsyncCallback, object,
                IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            TArg10 arg10,
            TArg11 arg11,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 12 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TArg10">The type of argument 10.</typeparam>
        /// <typeparam name="TArg11">The type of argument 11.</typeparam>
        /// <typeparam name="TArg12">The type of argument 12.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="arg10">Argument 10 of the begin call.</param>
        /// <param name="arg11">Argument 11 of the begin call.</param>
        /// <param name="arg12">Argument 12 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync
            <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
            [NotNull] this
                Func
                <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, AsyncCallback,
                object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            TArg10 arg10,
            TArg11 arg11,
            TArg12 arg12,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, null,
                               asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 13 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TArg10">The type of argument 10.</typeparam>
        /// <typeparam name="TArg11">The type of argument 11.</typeparam>
        /// <typeparam name="TArg12">The type of argument 12.</typeparam>
        /// <typeparam name="TArg13">The type of argument 13.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="arg10">Argument 10 of the begin call.</param>
        /// <param name="arg11">Argument 11 of the begin call.</param>
        /// <param name="arg12">Argument 12 of the begin call.</param>
        /// <param name="arg13">Argument 13 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync
            <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
            [NotNull] this
                Func
                <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13,
                AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            TArg10 arg10,
            TArg11 arg11,
            TArg12 arg12,
            TArg13 arg13,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, null,
                               asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

        #region 14 parameters.
        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TArg2">The type of argument 2.</typeparam>
        /// <typeparam name="TArg3">The type of argument 3.</typeparam>
        /// <typeparam name="TArg4">The type of argument 4.</typeparam>
        /// <typeparam name="TArg5">The type of argument 5.</typeparam>
        /// <typeparam name="TArg6">The type of argument 6.</typeparam>
        /// <typeparam name="TArg7">The type of argument 7.</typeparam>
        /// <typeparam name="TArg8">The type of argument 8.</typeparam>
        /// <typeparam name="TArg9">The type of argument 9.</typeparam>
        /// <typeparam name="TArg10">The type of argument 10.</typeparam>
        /// <typeparam name="TArg11">The type of argument 11.</typeparam>
        /// <typeparam name="TArg12">The type of argument 12.</typeparam>
        /// <typeparam name="TArg13">The type of argument 13.</typeparam>
        /// <typeparam name="TArg14">The type of argument 14.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">Argument 1 of the begin call.</param>
        /// <param name="arg2">Argument 2 of the begin call.</param>
        /// <param name="arg3">Argument 3 of the begin call.</param>
        /// <param name="arg4">Argument 4 of the begin call.</param>
        /// <param name="arg5">Argument 5 of the begin call.</param>
        /// <param name="arg6">Argument 6 of the begin call.</param>
        /// <param name="arg7">Argument 7 of the begin call.</param>
        /// <param name="arg8">Argument 8 of the begin call.</param>
        /// <param name="arg9">Argument 9 of the begin call.</param>
        /// <param name="arg10">Argument 10 of the begin call.</param>
        /// <param name="arg11">Argument 11 of the begin call.</param>
        /// <param name="arg12">Argument 12 of the begin call.</param>
        /// <param name="arg13">Argument 13 of the begin call.</param>
        /// <param name="arg14">Argument 14 of the begin call.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync
            <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14,
             TResult>(
            [NotNull] this
                Func
                <TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14,
                AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4,
            TArg5 arg5,
            TArg6 arg6,
            TArg7 arg7,
            TArg8 arg8,
            TArg9 arg9,
            TArg10 arg10,
            TArg11 arg11,
            TArg12 arg12,
            TArg13 arg13,
            TArg14 arg14,
            object asyncState = null,
            [CanBeNull] Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull] TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                               null, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion

/*

        /// <summary>
        /// Converts APM to TPL, but supports cancellation.
        /// </summary>
        /// <typeparam name="TArg1">The type of argument 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="beginMethod">The begin method.</param>
        /// <param name="endMethod">The end method.</param>
        /// <param name="arg1">The argument 1.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="cancellationMethod">The cancellation method.</param>
        /// <param name="creationOptions">The creation options.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [UsedImplicitly]
        public static Task<TResult> FromAsync<TArg1, TResult>([NotNull]this Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull]Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1,
            AsyncCallback callback,
            object asyncState,
            [CanBeNull]Action<IAsyncResult> cancellationMethod = null,
            TaskCreationOptions creationOptions = TaskCreationOptions.None,
            [CanBeNull]TaskScheduler scheduler = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return beginMethod(arg1, callback, asyncState)
                .FromAsync(endMethod, cancellationMethod, creationOptions, scheduler,
                           cancellationToken);
            // ReSharper restore AssignNullToNotNullAttribute
        }
*/
    }
}