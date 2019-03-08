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

using System;
using System.ComponentModel;
using System.Configuration;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Configuration;
using WebApplications.Utilities.Converters;

namespace WebApplications.Utilities.Scheduling.Configuration
{
    /// <summary>
    /// For use in <see cref="ConfigurationSection{T}"/>s, or <see cref="ScheduleCollection"/>s
    /// for easy specification of a schedule.
    /// </summary>
    /// <remarks></remarks>
    public class ScheduleElement : ConstructorConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        [StringValidator(MinLength = 0)]
        [NotNull]
        [PublicAPI]
        public string Name
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return GetProperty<string>("name"); }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                SetProperty("name", value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        [ConfigurationProperty("options", DefaultValue = ScheduleOptions.None, IsRequired = false)]
        [PublicAPI]
        public ScheduleOptions Options
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return GetProperty<ScheduleOptions>("options"); }
            set { SetProperty("options", value); }
        }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The logger type.</value>
        [ConfigurationProperty("type")]
        [TypeConverter(typeof(SimplifiedTypeNameConverter))]
        [SubclassTypeValidator(typeof(ISchedule))]
        [PublicAPI]
        public override Type Type
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return GetProperty<Type>("type"); }
            set { SetProperty("type", value); }
        }
    }
}