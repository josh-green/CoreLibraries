﻿#region © Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
// Copyright (c) 2015, Web Applications UK Ltd
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

using System.Configuration;
using WebApplications.Utilities.Annotations;
using ConfigurationElement = WebApplications.Utilities.Configuration.ConfigurationElement;

namespace WebApplications.Utilities.Database.Configuration
{
    /// <summary>
    ///   An element that represents a single connection.
    /// </summary>
    [PublicAPI]
    public class ConnectionElement : ConfigurationElement
    {
        /// <summary>
        ///   Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///   The property is read-only or locked.
        /// </exception>
        [ConfigurationProperty("connectionString", IsRequired = true, IsKey = true)]
        [NotNull]
        public string ConnectionString
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return GetProperty<string>("connectionString"); }
            set { SetProperty("connectionString", value); }
        }

        /// <summary>
        ///   Gets or sets a <see cref="bool"/> value indicating whether the connection is enabled.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the connection is enabled; otherwise <see langword="false"/>.
        /// </value>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///   The property is read-only or locked.
        /// </exception>
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get { return GetProperty<bool>("enabled"); }
            set { SetProperty("enabled", value); }
        }

        /// <summary>
        ///   Gets or sets a <see cref="double"/> indicating the relative weight of the connection.
        /// </summary>
        /// <value>
        ///   <para>The weight of the connection.</para>
        ///   <para>The default weighting is 1.0.</para>
        /// </value>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///   The property is read-only or locked.
        /// </exception>
        [ConfigurationProperty("weight", DefaultValue = 1.0D, IsRequired = false)]
        public double Weight
        {
            get { return GetProperty<double>("weight"); }
            set { SetProperty("weight", value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of concurrent program executions that are allowed in the connection.
        /// </summary>
        /// <value>
        /// The maximum concurrency.
        /// </value>
        /// <remarks>A negative value indicates no limit.</remarks>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        ///   The property is read-only or locked.
        /// </exception>
        [ConfigurationProperty("maxConcurrency", DefaultValue = -1, IsRequired = false)]
        public int MaximumConcurrency
        {
            get { return GetProperty<int>("maxConcurrency"); }
            set { SetProperty("maxConcurrency", value); }
        }
    }
}