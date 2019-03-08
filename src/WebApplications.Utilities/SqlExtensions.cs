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

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities
{
    /// <summary>
    ///   Extension methods for SQL Server functionalities.
    /// </summary>
    [PublicAPI]
    public static class SqlExtensions
    {
        /// <summary>
        ///   Sets the value of the specified column in a <see cref="SqlDataRecord"/>. This is an improvement
        ///   from <see cref="SqlDataRecord.SetValue"/> as it correctly handles <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/> to set.</typeparam>
        /// <param name="sqlDataRecord">A single row of data from a record.</param>
        /// <param name="ordinal">The zero based ordinal of the column.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="ordinal"/> is less than zero or greater than the number of columns in the record.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///   There is a type mismatch between the retrieved column and the specified <paramref name="value"/>.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="sqlDataRecord"/> is <see langword="null"/>.
        /// </exception>
        public static void Set<T>([NotNull] this SqlDataRecord sqlDataRecord, int ordinal, T value) where T : class
        {
            if (sqlDataRecord == null) throw new ArgumentNullException(nameof(sqlDataRecord));
            sqlDataRecord.SetValue(ordinal, value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Sets the value of the specified column in a <see cref="SqlDataRecord"/>. This is an improvement
        ///   from <see cref="SqlDataRecord.SetValue"/> as it correctly handles <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/> to set.</typeparam>
        /// <param name="sqlDataRecord">A single row of data from a record.</param>
        /// <param name="ordinal">The zero based ordinal of the column.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="ordinal"/> is less than zero or greater than the number of columns in the record.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="sqlDataRecord"/> is <see langword="null"/>.
        /// </exception>
        public static void Set<T>([NotNull] this SqlDataRecord sqlDataRecord, int ordinal, T? value) where T : struct
        {
            if (sqlDataRecord == null) throw new ArgumentNullException(nameof(sqlDataRecord));
            sqlDataRecord.SetValue(ordinal, value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Sets the value of the specified column in a <see cref="SqlDataRecord"/>. This is an improvement
        ///   from <see cref="SqlDataRecord.SetValue"/> as it correctly handles <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/> to set.</typeparam>
        /// <param name="sqlDataRecord">A single row of data from a record.</param>
        /// <param name="columnName">The name of the column to set.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="IndexOutOfRangeException">The column couldn't be found.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///   There is a type mismatch between the retrieved column and the specified <paramref name="value"/>.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="sqlDataRecord"/> is <see langword="null"/>.
        /// </exception>
        public static void Set<T>([NotNull] this SqlDataRecord sqlDataRecord, [NotNull] string columnName, T value)
            where T : class
        {
            if (sqlDataRecord == null) throw new ArgumentNullException(nameof(sqlDataRecord));
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            sqlDataRecord.SetValue(sqlDataRecord.GetOrdinal(columnName), value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Sets the value of the specified column in a <see cref="SqlDataRecord"/>. This is an improvement
        ///   from <see cref="SqlDataRecord.SetValue"/> as it correctly handles <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/> to set.</typeparam>
        /// <param name="sqlDataRecord">A single row of data from a record.</param>
        /// <param name="columnName">The name of the column to set.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="IndexOutOfRangeException">The column couldn't be found.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///   There is a type mismatch between the retrieved column and the <paramref name="value"/>.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="sqlDataRecord"/> is <see langword="null"/>.
        /// </exception>
        public static void Set<T>([NotNull] this SqlDataRecord sqlDataRecord, [NotNull] string columnName, T? value)
            where T : struct
        {
            if (sqlDataRecord == null) throw new ArgumentNullException(nameof(sqlDataRecord));
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            sqlDataRecord.SetValue(
                sqlDataRecord.GetOrdinal(columnName),
                value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Adds a value to the end of a <see cref="SqlParameterCollection"/>. If the <paramref name="value"/> is a
        ///   <see langword="null"/> then it will be passed as a <see cref="System.DBNull.Value"/>.
        /// </summary>
        /// <param name="parameters">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter to add.</param>
        public static void AddWithValueNull<T>(
            [NotNull] this SqlParameterCollection parameters,
            [NotNull] string name,
            T value)
            where T : class
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (name == null) throw new ArgumentNullException(nameof(name));
            parameters.AddWithValue(name, value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Adds a value to the end of a <see cref="SqlParameterCollection"/>. If the <paramref name="value"/> is
        ///   <see langword="null"/> then it will be passed as a <see cref="System.DBNull.Value"/>.
        /// </summary>
        /// <param name="parameters">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter to add.</param>
        public static void AddWithValueNull<T>(
            [NotNull] this SqlParameterCollection parameters,
            [NotNull] string name,
            T? value)
            where T : struct
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (name == null) throw new ArgumentNullException(nameof(name));
            parameters.AddWithValue(name, value ?? (object)DBNull.Value);
        }

        /// <summary>
        ///   Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="dataReader">The data reader object.</param>
        /// <param name="ordinal">The zero based ordinal of the column.</param>
        /// <param name="nullValue">
        ///   <para>The value to use if the data is null.</para>
        ///   <para>By default this gets the default value of <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>The value of the column specified by the <paramref name="ordinal"/>.</returns>
        /// <remarks>
        ///   Use this only if the ordinal is guaranteed not to change otherwise use the column name instead.
        /// </remarks>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="dataReader"/> is <see langword="null"/>.
        /// </exception>
        [ContractAnnotation("nullValue:null=>canbenull;nullValue:notnull=>notnull")]
        public static T GetValue<T>([NotNull] this IDataReader dataReader, int ordinal, T nullValue = default(T))
        {
            if (dataReader == null) throw new ArgumentNullException(nameof(dataReader));
            return dataReader.IsDBNull(ordinal) ? nullValue : (T)dataReader.GetValue(ordinal);
        }

        /// <summary>
        ///   Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="dataReader">The data reader object.</param>
        /// <param name="columnName">The name of the column to get.</param>
        /// <param name="nullValue">
        ///   <para>The value to use if the data is null.</para>
        ///   <para>By default this gets the default value of <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>The value of the column specified.</returns>
        /// <exception cref="IndexOutOfRangeException">
        ///   The <paramref name="columnName"/> provided is invalid.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="dataReader"/> is <see langword="null"/>.
        /// </exception>
        [ContractAnnotation("nullValue:null=>canbenull;nullValue:notnull=>notnull")]
        public static T GetValue<T>(
            [NotNull] this IDataReader dataReader,
            [NotNull] string columnName,
            T nullValue = default(T))
        {
            if (dataReader == null) throw new ArgumentNullException(nameof(dataReader));
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            int ordinal = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(ordinal) ? nullValue : (T)dataReader.GetValue(ordinal);
        }

        /// <summary>
        ///   Tries to get the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="dataReader">The data reader object.</param>
        /// <param name="columnName">The name of the column to get.</param>
        /// <param name="value">The retrieved value.</param>
        /// <param name="nullValue">
        ///   <para>The value to use if the data is null.</para>
        ///   <para>By default this gets the default value of <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        ///   Returns <see langword="true"/> if the value is found; otherwise returns <see langword="false"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///   There is no current connection to an instance of SQL Server.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///   <paramref name="dataReader"/> is <see langword="null"/>.
        /// </exception>
        public static bool TryGetValue<T>(
            [NotNull] this IDataReader dataReader,
            [NotNull] string columnName,
            out T value,
            T nullValue = default(T))
        {
            if (dataReader == null) throw new ArgumentNullException(nameof(dataReader));
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            int ordinal = -1;
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (!string.Equals(dataReader.GetName(i), columnName, StringComparison.CurrentCultureIgnoreCase))
                    continue;
                ordinal = i;
                break;
            }

            if (ordinal < 0)
            {
                value = nullValue;
                return false;
            }

            value = dataReader.IsDBNull(ordinal) ? nullValue : (T)dataReader.GetValue(ordinal);
            return true;
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="System.Int64"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<long> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));

            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.BigInt));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="byte"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<byte[]> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));

            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.VarBinary));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="bool"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<bool> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.Bit));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="string"/> type enumerable.</param>
        /// <param name="maxLength">
        ///   <para>The maximum length of the string.</para>
        ///   <para>By default this is -1, which is the default value of <see cref="SqlMetaData.Max"/>.</para>
        /// </param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentException">
        ///   The <paramref name="maxLength"/> specified is out of range.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<string> enumerable,
            int maxLength = -1,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.NVarChar, maxLength));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="System.DateTime"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<DateTime> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.DateTime));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="decimal"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<decimal> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.Decimal));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="double"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<double> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.Float));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="int"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<int> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.Int));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="System.Single"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<float> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.Real));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="System.Guid"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<Guid> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.UniqueIdentifier));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="System.Int16"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<short> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.SmallInt));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The <see cref="byte"/> type enumerable.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<byte> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, SqlDbType.TinyInt));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable item.</typeparam>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="dbType">The SQL server specific data type of the column.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentException">
        ///   An invalid <see cref="System.Data.SqlDbType"/> has been passed to <paramref name="dbType"/>.
        ///   See <see cref="Microsoft.SqlServer.Server.SqlMetaData"/> for which types are allowed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue<T>(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<T> enumerable,
            SqlDbType dbType,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, dbType));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable item.</typeparam>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="dbType">The SQL server specific data type of the column.</param>
        /// <param name="precision">The maximum number of decimal digits that can be stored (left and right of the decimal point).</param>
        /// <param name="scale">The maximum number of decimal digits that can be stored to the right of the decimal point.</param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <exception cref="ArgumentException">
        ///   <para>The only <see cref="System.Data.SqlDbType"/> allowed for <paramref name="dbType"/> is
        ///   <see cref="System.Data.SqlDbType.Decimal"/>.</para>
        ///   <para>-or-</para>
        ///   <para><paramref name="scale"/> was greater than <paramref name="precision"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue<T>(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<T> enumerable,
            SqlDbType dbType,
            byte precision,
            byte scale,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, dbType, precision, scale));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable item.</typeparam>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="dbType">The SQL server specific data type of the column.</param>
        /// <param name="maxLength">
        ///   <para>The maximum length of the specified type.</para>
        ///   <para>By default this is -1, which is the default value of <see cref="SqlMetaData.Max"/>.</para>
        /// </param>
        /// <param name="columnName">
        ///   <para>The name of the column in the table parameter.</para>
        ///   <para>By default this is set to "Value".</para>
        /// </param>
        /// <returns>The parameter that was added.</returns>
        /// <remarks>
        ///   Only <see cref="SqlMetaData.Max"/> or -1 is allowed for a <see cref="System.Data.SqlDbType"/> of Text, NText, or Image.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///   <para>The only <see cref="System.Data.SqlDbType"/>s allowed for <paramref name="dbType"/> are
        ///   Binary, Char, Image, NChar, Ntext, NVarChar, Text, VarBinary and VarChar.</para>
        ///   <para>-or-</para>
        ///   <para>The <paramref name="maxLength"/> specified is out of range.</para></exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static SqlParameter AddWithValue<T>(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<T> enumerable,
            SqlDbType dbType,
            long maxLength,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return AddWithValue(
                sqlParameterCollection,
                parameterName,
                enumerable,
                new SqlMetaData(columnName, dbType, maxLength));
        }

        /// <summary>
        ///   Adds a single column table parameter to a parameter collection,
        ///   based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable item.</typeparam>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="sqlMetaData">The SQL meta data describing the column.</param>
        /// <returns>The parameter that was added.</returns>
        [NotNull]
        public static SqlParameter AddWithValue<T>(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<T> enumerable,
            [NotNull] SqlMetaData sqlMetaData)
        {
            if (sqlParameterCollection == null) throw new ArgumentNullException(nameof(sqlParameterCollection));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            // Create the parameter
            SqlParameter sqlParameter = sqlParameterCollection.Add(
                new SqlParameter(parameterName, SqlDbType.Structured));
            Debug.Assert(sqlParameter != null);

            // Set the value
            sqlParameter.Value = enumerable.ToSqlParameterValue(sqlMetaData);

            return sqlParameter;
        }

        /// <summary>
        /// Adds a table parameter to a parameter collection, based on an <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable item.</typeparam>
        /// <param name="sqlParameterCollection">
        ///   The parameters associated with an <see cref="System.Data.SqlClient.SqlCommand"/>.
        /// </param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="convertor">The convertor function.</param>
        /// <param name="sqlMetaData">The SQL meta data describing the columns.</param>
        /// <returns>The <see cref="SqlParameter">parameter</see> that was added.</returns>
        /// <remarks>
        ///   <para>The <paramref name="convertor"/> function is supplied with a new <see cref="SqlDataRecord"/>
        ///   and an item of type <typeparamref name="T"/> from the supplied enumerable, which should be used to
        ///   populate the <see cref="SqlDataRecord"/>.</para>
        ///   <para>If you do not wish the item to be added to the table then return <see langword="false"/>; otherwise
        ///   <see langword="true"/> should be returned.</para>
        /// </remarks>
        [NotNull]
        public static SqlParameter AddWithValue<T>(
            [NotNull] this SqlParameterCollection sqlParameterCollection,
            [NotNull] string parameterName,
            [NotNull] IEnumerable<T> enumerable,
            [NotNull] Func<T, SqlDataRecord, bool> convertor,
            [NotNull] params SqlMetaData[] sqlMetaData)
        {
            if (sqlParameterCollection == null) throw new ArgumentNullException(nameof(sqlParameterCollection));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (sqlMetaData == null) throw new ArgumentNullException(nameof(sqlMetaData));

            // Create the parameter
            SqlParameter sqlParameter = sqlParameterCollection.Add(
                new SqlParameter(parameterName, SqlDbType.Structured));
            Debug.Assert(sqlParameter != null);

            // Set the value
            sqlParameter.Value = enumerable.ToSqlParameterValue(convertor, sqlMetaData);

            return sqlParameter;
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="Int64"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<long> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.BigInt));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable containing a <see cref="byte"/> array.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was a <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<byte[]> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.VarBinary));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="bool"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<bool> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.Bit));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="string"/>.</param>
        /// <param name="maxLength">
        ///   <para>The maximum length of the string.</para>
        ///   <para>By default this is -1, which is the default value of <see cref="SqlMetaData.Max"/>.</para>
        /// </param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<string> enumerable,
            int maxLength = -1,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.NVarChar, maxLength));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.DateTime"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<DateTime> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.DateTime));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Decimal"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<decimal> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.Decimal));
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Double"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<double> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.Float));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Int32"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<int> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.Int));
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Single"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<float> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.Real));
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Guid"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<Guid> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.UniqueIdentifier));
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Int16"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<short> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.SmallInt));
        }

        /// <summary>
        ///   Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable of type <see cref="System.Byte"/>.</param>
        /// <param name="columnName">
        ///   <para>The name of the column.</para>
        ///   <para>By default this is "Value".</para>
        /// </param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as an <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="columnName"/> was <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue(
            [NotNull] this IEnumerable<byte> enumerable,
            [NotNull] string columnName = "Value")
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            return ToSqlParameterValue(enumerable, new SqlMetaData(columnName, SqlDbType.TinyInt));
        }

        /// <summary>
        /// Converts the specified object to a single columned <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="sqlMetaData">The SQL meta data describing the column.</param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as a <see cref="SqlDataRecord"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable"/> or <paramref name="sqlMetaData"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [NotNull] SqlMetaData sqlMetaData)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (sqlMetaData == null) throw new ArgumentNullException(nameof(sqlMetaData));

            IEnumerable<SqlDataRecord> value = null;

            enumerable = enumerable.Enumerate();

            // Only set the parameters value 
            if ((enumerable.Any()))
            {
                bool checkNulls = !typeof(T).IsValueType;
                SqlMetaData[] metaData = { sqlMetaData };
                value = enumerable.Select(
                    o =>
                    {
                        object obj = o;
                        SqlDataRecord record = new SqlDataRecord(metaData);
                        // ReSharper disable once AssignNullToNotNullAttribute
                        record.SetValue(0, (checkNulls && obj == null) ? DBNull.Value : obj);
                        return record;
                    }).ToList();
            }

            return value;
        }

        /// <summary>
        ///   Converts the specified object to a corresponding <see cref="SqlDataRecord"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration items.</typeparam>
        /// <param name="enumerable">The enumerable of type <typeparamref name="T"/>.</param>
        /// <param name="convertor">The convertor function.</param>
        /// <param name="sqlMetaData">The SQL meta data describing the columns.</param>
        /// <returns>
        ///   The converted <paramref name="enumerable"/> as a <see cref="SqlDataRecord"/>.
        /// </returns>
        [CanBeNull]
        public static IEnumerable<SqlDataRecord> ToSqlParameterValue<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [NotNull] Func<T, SqlDataRecord, bool> convertor,
            [NotNull] params SqlMetaData[] sqlMetaData)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (sqlMetaData == null) throw new ArgumentNullException(nameof(sqlMetaData));

            enumerable = enumerable.Enumerate();

            IEnumerable<SqlDataRecord> value = null;
            // Only set the parameters value if we have columns and items in the enumerable.
            if ((sqlMetaData.Length > 0) &&
                (enumerable.Any()))
            {
                // Convert enumerable items into SqlDataRecords, where the returned value is not null.
                List<SqlDataRecord> convertedList = enumerable.Select(
                    o =>
                    {
                        SqlDataRecord record = new SqlDataRecord(sqlMetaData);
                        return convertor(o, record) ? record : null;
                    }).Where(r => r != null).ToList();

                // If we still have values set the parameter value.
                if (convertedList.Count > 0)
                    value = convertedList;
            }

            return value;
        }
    }
}