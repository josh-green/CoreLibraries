﻿#region © Copyright Web Applications (UK) Ltd, 2016.  All rights reserved.
// Copyright (c) 2016, Web Applications UK Ltd
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

#region Designer generated code
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SqlServer.Server;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Configuration;
using WebApplications.Utilities.Database.Exceptions;
using WebApplications.Utilities.Database.Schema;
using WebApplications.Utilities.Logging;

namespace WebApplications.Utilities.Database
{
    #region Extensions to SqlProgramCommand
    /// <summary>
    /// A specialised command that allows finer grained control when using SqlPrograms.
    /// </summary>
    public partial class SqlProgramCommand
    {
        /// <summary>
        /// Sets the parameters in ordinal order.
        /// </summary>
        /// <typeparam name="T1">The type of parameter 1.</typeparam>
        /// <typeparam name="T2">The type of parameter 2.</typeparam>
        /// <typeparam name="T3">The type of parameter 3.</typeparam>
        /// <typeparam name="T4">The type of parameter 4.</typeparam>
        /// <typeparam name="T5">The type of parameter 5.</typeparam>
        /// <typeparam name="T6">The type of parameter 6.</typeparam>
        /// <typeparam name="T7">The type of parameter 7.</typeparam>
        /// <typeparam name="T8">The type of parameter 8.</typeparam>
        /// <typeparam name="T9">The type of parameter 9.</typeparam>
        /// <typeparam name="T10">The type of parameter 10.</typeparam>
        /// <typeparam name="T11">The type of parameter 11.</typeparam>
        /// <typeparam name="T12">The type of parameter 12.</typeparam>
        /// <typeparam name="T13">The type of parameter 13.</typeparam>
        /// <typeparam name="T14">The type of parameter 14.</typeparam>
        /// <typeparam name="T15">The type of parameter 15.</typeparam>
        /// <typeparam name="T16">The type of parameter 16.</typeparam>
        /// <typeparam name="T17">The type of parameter 17.</typeparam>
        /// <typeparam name="T18">The type of parameter 18.</typeparam>
        /// <typeparam name="T19">The type of parameter 19.</typeparam>
        /// <typeparam name="T20">The type of parameter 20.</typeparam>
        /// <typeparam name="T21">The type of parameter 21.</typeparam>
        /// <typeparam name="T22">The type of parameter 22.</typeparam>
        /// <typeparam name="T23">The type of parameter 23.</typeparam>
        /// <typeparam name="T24">The type of parameter 24.</typeparam>
        /// <typeparam name="T25">The type of parameter 25.</typeparam>
        /// <typeparam name="T26">The type of parameter 26.</typeparam>
        /// <typeparam name="T27">The type of parameter 27.</typeparam>
        /// <typeparam name="T28">The type of parameter 28.</typeparam>
        /// <typeparam name="T29">The type of parameter 29.</typeparam>
        /// <typeparam name="T30">The type of parameter 30.</typeparam>
        /// <typeparam name="T31">The type of parameter 31.</typeparam>
        /// <typeparam name="T32">The type of parameter 32.</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="mode">The constraint mode.</param>
        /// <returns>The parameters that were set</returns>
        [NotNull]
        public IEnumerable<SqlParameter> SetParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(T1 p1Value, T2 p2Value, T3 p3Value, T4 p4Value, T5 p5Value, T6 p6Value, T7 p7Value, T8 p8Value, T9 p9Value, T10 p10Value, T11 p11Value, T12 p12Value, T13 p13Value, T14 p14Value, T15 p15Value, T16 p16Value, T17 p17Value, T18 p18Value, T19 p19Value, T20 p20Value, T21 p21Value, T22 p22Value, T23 p23Value, T24 p24Value, T25 p25Value, T26 p26Value, T27 p27Value, T28 p28Value, T29 p29Value, T30 p30Value, T31 p31Value, T32 p32Value, TypeConstraintMode mode = TypeConstraintMode.Warn)
        {
            SqlProgramParameter[] parameters = _mapping.Parameters.ToArray();
            int pCount = parameters.GetLength(0);
            if (pCount < 32)
                throw new LoggingException(
                        LoggingLevel.Critical,
                        () => Resources.SqlProgramCommand_SetParameters_Too_Many_Parameters,
                        _program.Name,
                        pCount,
                        32);

            List<SqlParameter> sqlParameters = new List<SqlParameter>(32);
            SqlParameter parameter;
            SqlProgramParameter programParameter;
            int index;
            lock (_parameters)
            {
                // Find or create SQL Parameter 1.
                programParameter = parameters[0];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p1Value, mode);
                AddOutParameter(programParameter, parameter, p1Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 2.
                programParameter = parameters[1];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p2Value, mode);
                AddOutParameter(programParameter, parameter, p2Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 3.
                programParameter = parameters[2];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p3Value, mode);
                AddOutParameter(programParameter, parameter, p3Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 4.
                programParameter = parameters[3];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p4Value, mode);
                AddOutParameter(programParameter, parameter, p4Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 5.
                programParameter = parameters[4];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p5Value, mode);
                AddOutParameter(programParameter, parameter, p5Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 6.
                programParameter = parameters[5];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p6Value, mode);
                AddOutParameter(programParameter, parameter, p6Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 7.
                programParameter = parameters[6];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p7Value, mode);
                AddOutParameter(programParameter, parameter, p7Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 8.
                programParameter = parameters[7];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p8Value, mode);
                AddOutParameter(programParameter, parameter, p8Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 9.
                programParameter = parameters[8];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p9Value, mode);
                AddOutParameter(programParameter, parameter, p9Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 10.
                programParameter = parameters[9];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p10Value, mode);
                AddOutParameter(programParameter, parameter, p10Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 11.
                programParameter = parameters[10];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p11Value, mode);
                AddOutParameter(programParameter, parameter, p11Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 12.
                programParameter = parameters[11];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p12Value, mode);
                AddOutParameter(programParameter, parameter, p12Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 13.
                programParameter = parameters[12];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p13Value, mode);
                AddOutParameter(programParameter, parameter, p13Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 14.
                programParameter = parameters[13];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p14Value, mode);
                AddOutParameter(programParameter, parameter, p14Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 15.
                programParameter = parameters[14];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p15Value, mode);
                AddOutParameter(programParameter, parameter, p15Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 16.
                programParameter = parameters[15];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p16Value, mode);
                AddOutParameter(programParameter, parameter, p16Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 17.
                programParameter = parameters[16];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p17Value, mode);
                AddOutParameter(programParameter, parameter, p17Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 18.
                programParameter = parameters[17];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p18Value, mode);
                AddOutParameter(programParameter, parameter, p18Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 19.
                programParameter = parameters[18];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p19Value, mode);
                AddOutParameter(programParameter, parameter, p19Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 20.
                programParameter = parameters[19];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p20Value, mode);
                AddOutParameter(programParameter, parameter, p20Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 21.
                programParameter = parameters[20];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p21Value, mode);
                AddOutParameter(programParameter, parameter, p21Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 22.
                programParameter = parameters[21];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p22Value, mode);
                AddOutParameter(programParameter, parameter, p22Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 23.
                programParameter = parameters[22];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p23Value, mode);
                AddOutParameter(programParameter, parameter, p23Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 24.
                programParameter = parameters[23];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p24Value, mode);
                AddOutParameter(programParameter, parameter, p24Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 25.
                programParameter = parameters[24];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p25Value, mode);
                AddOutParameter(programParameter, parameter, p25Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 26.
                programParameter = parameters[25];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p26Value, mode);
                AddOutParameter(programParameter, parameter, p26Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 27.
                programParameter = parameters[26];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p27Value, mode);
                AddOutParameter(programParameter, parameter, p27Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 28.
                programParameter = parameters[27];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p28Value, mode);
                AddOutParameter(programParameter, parameter, p28Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 29.
                programParameter = parameters[28];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p29Value, mode);
                AddOutParameter(programParameter, parameter, p29Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 30.
                programParameter = parameters[29];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p30Value, mode);
                AddOutParameter(programParameter, parameter, p30Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 31.
                programParameter = parameters[30];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p31Value, mode);
                AddOutParameter(programParameter, parameter, p31Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 32.
                programParameter = parameters[31];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p32Value, mode);
                AddOutParameter(programParameter, parameter, p32Value as IOut);
                sqlParameters.Add(parameter);
            }

            // Return parameters that were set
            return sqlParameters;
        }

        /// <summary>
        /// Sets the parameters in ordinal order.
        /// </summary>
        /// <typeparam name="T1">The type of parameter 1.</typeparam>
        /// <typeparam name="T2">The type of parameter 2.</typeparam>
        /// <typeparam name="T3">The type of parameter 3.</typeparam>
        /// <typeparam name="T4">The type of parameter 4.</typeparam>
        /// <typeparam name="T5">The type of parameter 5.</typeparam>
        /// <typeparam name="T6">The type of parameter 6.</typeparam>
        /// <typeparam name="T7">The type of parameter 7.</typeparam>
        /// <typeparam name="T8">The type of parameter 8.</typeparam>
        /// <typeparam name="T9">The type of parameter 9.</typeparam>
        /// <typeparam name="T10">The type of parameter 10.</typeparam>
        /// <typeparam name="T11">The type of parameter 11.</typeparam>
        /// <typeparam name="T12">The type of parameter 12.</typeparam>
        /// <typeparam name="T13">The type of parameter 13.</typeparam>
        /// <typeparam name="T14">The type of parameter 14.</typeparam>
        /// <typeparam name="T15">The type of parameter 15.</typeparam>
        /// <typeparam name="T16">The type of parameter 16.</typeparam>
        /// <typeparam name="T17">The type of parameter 17.</typeparam>
        /// <typeparam name="T18">The type of parameter 18.</typeparam>
        /// <typeparam name="T19">The type of parameter 19.</typeparam>
        /// <typeparam name="T20">The type of parameter 20.</typeparam>
        /// <typeparam name="T21">The type of parameter 21.</typeparam>
        /// <typeparam name="T22">The type of parameter 22.</typeparam>
        /// <typeparam name="T23">The type of parameter 23.</typeparam>
        /// <typeparam name="T24">The type of parameter 24.</typeparam>
        /// <typeparam name="T25">The type of parameter 25.</typeparam>
        /// <typeparam name="T26">The type of parameter 26.</typeparam>
        /// <typeparam name="T27">The type of parameter 27.</typeparam>
        /// <typeparam name="T28">The type of parameter 28.</typeparam>
        /// <typeparam name="T29">The type of parameter 29.</typeparam>
        /// <typeparam name="T30">The type of parameter 30.</typeparam>
        /// <typeparam name="T31">The type of parameter 31.</typeparam>
        /// <typeparam name="T32">The type of parameter 32.</typeparam>
        /// <param name="names">The enumeration of parameters names.</param>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="mode">The constraint mode.</param>
        /// <returns>The parameters that were set</returns>
        [NotNull]
        public IEnumerable<SqlParameter> SetParameters<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(IEnumerable<string> names, T1 p1Value, T2 p2Value, T3 p3Value, T4 p4Value, T5 p5Value, T6 p6Value, T7 p7Value, T8 p8Value, T9 p9Value, T10 p10Value, T11 p11Value, T12 p12Value, T13 p13Value, T14 p14Value, T15 p15Value, T16 p16Value, T17 p17Value, T18 p18Value, T19 p19Value, T20 p20Value, T21 p21Value, T22 p22Value, T23 p23Value, T24 p24Value, T25 p25Value, T26 p26Value, T27 p27Value, T28 p28Value, T29 p29Value, T30 p30Value, T31 p31Value, T32 p32Value, TypeConstraintMode mode = TypeConstraintMode.Warn)
        {
            if ((names == null) || (names.Count() != 32))
                throw new LoggingException(
                        LoggingLevel.Critical,
                        () => Resources.SqlProgramCommand_SetParameters_Wrong_Number_Of_Parameters,
                        _program.Name,
                        32,
                        names == null ? 0 : names.Count());

            SqlProgramParameter[] parameters = names.Select(
                    n =>
                        {
                            n = n.ToLower(); // Find parameter definition
                            SqlProgramParameter parameterDefinition;
                            if (!_mapping.Definition.TryGetParameter(n, out parameterDefinition))
                                throw new LoggingException(
                                        LoggingLevel.Critical,
                                        () => Resources.SqlProgramCommand_SetParameters_Unknown_Parameter,
                                        _program.Name,
                                        n);
                            return parameterDefinition;
                        }).ToArray();

            int pCount = parameters.GetLength(0);
            if (pCount < 32)
                throw new LoggingException(
                        LoggingLevel.Critical,
                        () => Resources.SqlProgramCommand_SetParameters_Too_Many_Parameters,
                        _program.Name,
                        pCount,
                        32);

            List<SqlParameter> sqlParameters = new List<SqlParameter>(2);
            SqlParameter parameter;
            SqlProgramParameter programParameter;
            int index;
            lock (_parameters)
            {
                // Find or create SQL Parameter 1.
                programParameter = parameters[0];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p1Value, mode);
                AddOutParameter(programParameter, parameter, p1Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 2.
                programParameter = parameters[1];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p2Value, mode);
                AddOutParameter(programParameter, parameter, p2Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 3.
                programParameter = parameters[2];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p3Value, mode);
                AddOutParameter(programParameter, parameter, p3Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 4.
                programParameter = parameters[3];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p4Value, mode);
                AddOutParameter(programParameter, parameter, p4Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 5.
                programParameter = parameters[4];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p5Value, mode);
                AddOutParameter(programParameter, parameter, p5Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 6.
                programParameter = parameters[5];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p6Value, mode);
                AddOutParameter(programParameter, parameter, p6Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 7.
                programParameter = parameters[6];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p7Value, mode);
                AddOutParameter(programParameter, parameter, p7Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 8.
                programParameter = parameters[7];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p8Value, mode);
                AddOutParameter(programParameter, parameter, p8Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 9.
                programParameter = parameters[8];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p9Value, mode);
                AddOutParameter(programParameter, parameter, p9Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 10.
                programParameter = parameters[9];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p10Value, mode);
                AddOutParameter(programParameter, parameter, p10Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 11.
                programParameter = parameters[10];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p11Value, mode);
                AddOutParameter(programParameter, parameter, p11Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 12.
                programParameter = parameters[11];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p12Value, mode);
                AddOutParameter(programParameter, parameter, p12Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 13.
                programParameter = parameters[12];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p13Value, mode);
                AddOutParameter(programParameter, parameter, p13Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 14.
                programParameter = parameters[13];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p14Value, mode);
                AddOutParameter(programParameter, parameter, p14Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 15.
                programParameter = parameters[14];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p15Value, mode);
                AddOutParameter(programParameter, parameter, p15Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 16.
                programParameter = parameters[15];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p16Value, mode);
                AddOutParameter(programParameter, parameter, p16Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 17.
                programParameter = parameters[16];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p17Value, mode);
                AddOutParameter(programParameter, parameter, p17Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 18.
                programParameter = parameters[17];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p18Value, mode);
                AddOutParameter(programParameter, parameter, p18Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 19.
                programParameter = parameters[18];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p19Value, mode);
                AddOutParameter(programParameter, parameter, p19Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 20.
                programParameter = parameters[19];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p20Value, mode);
                AddOutParameter(programParameter, parameter, p20Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 21.
                programParameter = parameters[20];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p21Value, mode);
                AddOutParameter(programParameter, parameter, p21Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 22.
                programParameter = parameters[21];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p22Value, mode);
                AddOutParameter(programParameter, parameter, p22Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 23.
                programParameter = parameters[22];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p23Value, mode);
                AddOutParameter(programParameter, parameter, p23Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 24.
                programParameter = parameters[23];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p24Value, mode);
                AddOutParameter(programParameter, parameter, p24Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 25.
                programParameter = parameters[24];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p25Value, mode);
                AddOutParameter(programParameter, parameter, p25Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 26.
                programParameter = parameters[25];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p26Value, mode);
                AddOutParameter(programParameter, parameter, p26Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 27.
                programParameter = parameters[26];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p27Value, mode);
                AddOutParameter(programParameter, parameter, p27Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 28.
                programParameter = parameters[27];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p28Value, mode);
                AddOutParameter(programParameter, parameter, p28Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 29.
                programParameter = parameters[28];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p29Value, mode);
                AddOutParameter(programParameter, parameter, p29Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 30.
                programParameter = parameters[29];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p30Value, mode);
                AddOutParameter(programParameter, parameter, p30Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 31.
                programParameter = parameters[30];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p31Value, mode);
                AddOutParameter(programParameter, parameter, p31Value as IOut);
                sqlParameters.Add(parameter);
                // Find or create SQL Parameter 32.
                programParameter = parameters[31];
                index = _parameters.IndexOf(programParameter.FullName);
                parameter = index < 0 ? _parameters.Add(programParameter.CreateSqlParameter()) : _parameters[index];
                programParameter.SetSqlParameterValue(parameter, p32Value, mode);
                AddOutParameter(programParameter, parameter, p32Value as IOut);
                sqlParameters.Add(parameter);
            }

            // Return parameters that were set
            return sqlParameters;
        }
    }
    #endregion

    #region SqlProgam<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>
    /// <summary>
    ///   Used to create an object for easy calling of stored procedures or functions in a database.
    /// </summary>
    public class SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> : SqlProgram
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProgram" /> class.
        /// </summary>
        /// <param name="connection">The load balanced connection.</param>
        /// <param name="name">The <see cref="SqlProgram.Name">name</see> of the program.</param>
        /// <param name="parameters">The program <see cref="SqlProgram.Parameters">parameters</see>.</param>
        /// <param name="defaultCommandTimeout"><para>The <see cref="SqlProgram.DefaultCommandTimeout">default command timeout</see></para>
        /// <para>This is the time to wait for the command to execute.</para>
        /// <para>If set to <see langword="null" /> then the timeout will be 30 seconds.</para></param>
        /// <param name="constraintMode"><para>The type constraint mode.</para>
        /// <para>By default this is set to log a warning if truncation/loss of precision occurs.</para></param>
        protected SqlProgram(
            [NotNull] LoadBalancedConnection connection,
            [NotNull] string name,
            [CanBeNull] IEnumerable<KeyValuePair<string, Type>> parameters = null,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode constraintMode = TypeConstraintMode.Warn)
            : base(connection, name, parameters, defaultCommandTimeout, constraintMode)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (name == null) throw new ArgumentNullException("name");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProgram" /> class.
        /// </summary>
        /// <param name="program">The base program (stored procedure/function).</param>
        /// <param name="parameters">The program <see cref="SqlProgram.Parameters">parameters</see>.</param>
        /// <param name="defaultCommandTimeout"><para>The <see cref="SqlProgram.DefaultCommandTimeout">default command timeout</see></para>
        /// <para>This is the time to wait for the command to execute.</para>
        /// <para>If set to <see langword="null" /> then the timeout the default timeout from the base program.</para></param>
        /// <param name="constraintMode">The type constraint mode, this defined the behavior when truncation/loss of precision occurs.</param>
        protected SqlProgram(
            [NotNull] SqlProgram program,
            [NotNull] IEnumerable<KeyValuePair<string, Type>> parameters,
            TimeSpan? defaultCommandTimeout,
            TypeConstraintMode constraintMode)
            : base(program, parameters, defaultCommandTimeout, constraintMode)
        {
            if (program == null) throw new ArgumentNullException("program");
            if (parameters == null) throw new ArgumentNullException("parameters");
        }
        #endregion
        
        #region Create overloads
        /// <summary>
        /// Creates a new instance of the <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> does not throw validation errors (records them instead).</param>
        /// <param name="defaultCommandTimeout">The optional default command timeout, which will be used whenever this command is executed synchronously.
        /// Defaults to 30s.</param>
        /// <param name="constraintMode">The constraint mode.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram"/>.</returns>
        public static async Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> Create(
            [NotNull] LoadBalancedConnection connection,
            [NotNull] string name,
            bool ignoreValidationErrors = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode constraintMode = TypeConstraintMode.Warn,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (connection == null) throw new ArgumentNullException("connection");
            SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> newProgram = new SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
                connection,
                name,
                new[] { new KeyValuePair<string, Type>(null, typeof(T1)), new KeyValuePair<string, Type>(null, typeof(T2)), new KeyValuePair<string, Type>(null, typeof(T3)), new KeyValuePair<string, Type>(null, typeof(T4)), new KeyValuePair<string, Type>(null, typeof(T5)), new KeyValuePair<string, Type>(null, typeof(T6)), new KeyValuePair<string, Type>(null, typeof(T7)), new KeyValuePair<string, Type>(null, typeof(T8)), new KeyValuePair<string, Type>(null, typeof(T9)), new KeyValuePair<string, Type>(null, typeof(T10)), new KeyValuePair<string, Type>(null, typeof(T11)), new KeyValuePair<string, Type>(null, typeof(T12)), new KeyValuePair<string, Type>(null, typeof(T13)), new KeyValuePair<string, Type>(null, typeof(T14)), new KeyValuePair<string, Type>(null, typeof(T15)), new KeyValuePair<string, Type>(null, typeof(T16)), new KeyValuePair<string, Type>(null, typeof(T17)), new KeyValuePair<string, Type>(null, typeof(T18)), new KeyValuePair<string, Type>(null, typeof(T19)), new KeyValuePair<string, Type>(null, typeof(T20)), new KeyValuePair<string, Type>(null, typeof(T21)), new KeyValuePair<string, Type>(null, typeof(T22)), new KeyValuePair<string, Type>(null, typeof(T23)), new KeyValuePair<string, Type>(null, typeof(T24)), new KeyValuePair<string, Type>(null, typeof(T25)), new KeyValuePair<string, Type>(null, typeof(T26)), new KeyValuePair<string, Type>(null, typeof(T27)), new KeyValuePair<string, Type>(null, typeof(T28)), new KeyValuePair<string, Type>(null, typeof(T29)), new KeyValuePair<string, Type>(null, typeof(T30)), new KeyValuePair<string, Type>(null, typeof(T31)), new KeyValuePair<string, Type>(null, typeof(T32)) },
                defaultCommandTimeout,
                constraintMode);

            // Validate
            await newProgram.Validate(true, false, !ignoreValidationErrors, cancellationToken).ConfigureAwait(false);

            return newProgram;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/> class.
        /// </summary>
        /// <param name="sqlProgram">The SQL program.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> does not throw validation errors (records them instead).</param>
        /// <param name="defaultCommandTimeout">The optional default command timeout, which will be used whenever this command is executed synchronously.
        /// Defaults to existing programs default.</param>
        /// <param name="constraintMode">The constraint mode.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram"/>.</returns>
        public static async Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> Create(
            [NotNull] SqlProgram sqlProgram,
            bool ignoreValidationErrors = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode constraintMode = TypeConstraintMode.Warn,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (sqlProgram == null) throw new ArgumentNullException("sqlProgram");
            SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> newProgram = new SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
                sqlProgram,
                new[] { new KeyValuePair<string, Type>(null, typeof(T1)), new KeyValuePair<string, Type>(null, typeof(T2)), new KeyValuePair<string, Type>(null, typeof(T3)), new KeyValuePair<string, Type>(null, typeof(T4)), new KeyValuePair<string, Type>(null, typeof(T5)), new KeyValuePair<string, Type>(null, typeof(T6)), new KeyValuePair<string, Type>(null, typeof(T7)), new KeyValuePair<string, Type>(null, typeof(T8)), new KeyValuePair<string, Type>(null, typeof(T9)), new KeyValuePair<string, Type>(null, typeof(T10)), new KeyValuePair<string, Type>(null, typeof(T11)), new KeyValuePair<string, Type>(null, typeof(T12)), new KeyValuePair<string, Type>(null, typeof(T13)), new KeyValuePair<string, Type>(null, typeof(T14)), new KeyValuePair<string, Type>(null, typeof(T15)), new KeyValuePair<string, Type>(null, typeof(T16)), new KeyValuePair<string, Type>(null, typeof(T17)), new KeyValuePair<string, Type>(null, typeof(T18)), new KeyValuePair<string, Type>(null, typeof(T19)), new KeyValuePair<string, Type>(null, typeof(T20)), new KeyValuePair<string, Type>(null, typeof(T21)), new KeyValuePair<string, Type>(null, typeof(T22)), new KeyValuePair<string, Type>(null, typeof(T23)), new KeyValuePair<string, Type>(null, typeof(T24)), new KeyValuePair<string, Type>(null, typeof(T25)), new KeyValuePair<string, Type>(null, typeof(T26)), new KeyValuePair<string, Type>(null, typeof(T27)), new KeyValuePair<string, Type>(null, typeof(T28)), new KeyValuePair<string, Type>(null, typeof(T29)), new KeyValuePair<string, Type>(null, typeof(T30)), new KeyValuePair<string, Type>(null, typeof(T31)), new KeyValuePair<string, Type>(null, typeof(T32)) },
                defaultCommandTimeout,
                constraintMode);

            // Validate
            await newProgram.Validate(true, false, !ignoreValidationErrors, cancellationToken).ConfigureAwait(false);

            return newProgram;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        /// <param name="p1Name">Name of parameter 1.</param>
        /// <param name="p2Name">Name of parameter 2.</param>
        /// <param name="p3Name">Name of parameter 3.</param>
        /// <param name="p4Name">Name of parameter 4.</param>
        /// <param name="p5Name">Name of parameter 5.</param>
        /// <param name="p6Name">Name of parameter 6.</param>
        /// <param name="p7Name">Name of parameter 7.</param>
        /// <param name="p8Name">Name of parameter 8.</param>
        /// <param name="p9Name">Name of parameter 9.</param>
        /// <param name="p10Name">Name of parameter 10.</param>
        /// <param name="p11Name">Name of parameter 11.</param>
        /// <param name="p12Name">Name of parameter 12.</param>
        /// <param name="p13Name">Name of parameter 13.</param>
        /// <param name="p14Name">Name of parameter 14.</param>
        /// <param name="p15Name">Name of parameter 15.</param>
        /// <param name="p16Name">Name of parameter 16.</param>
        /// <param name="p17Name">Name of parameter 17.</param>
        /// <param name="p18Name">Name of parameter 18.</param>
        /// <param name="p19Name">Name of parameter 19.</param>
        /// <param name="p20Name">Name of parameter 20.</param>
        /// <param name="p21Name">Name of parameter 21.</param>
        /// <param name="p22Name">Name of parameter 22.</param>
        /// <param name="p23Name">Name of parameter 23.</param>
        /// <param name="p24Name">Name of parameter 24.</param>
        /// <param name="p25Name">Name of parameter 25.</param>
        /// <param name="p26Name">Name of parameter 26.</param>
        /// <param name="p27Name">Name of parameter 27.</param>
        /// <param name="p28Name">Name of parameter 28.</param>
        /// <param name="p29Name">Name of parameter 29.</param>
        /// <param name="p30Name">Name of parameter 30.</param>
        /// <param name="p31Name">Name of parameter 31.</param>
        /// <param name="p32Name">Name of parameter 32.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> does not throw validation errors (records them instead).</param>
        /// <param name="checkOrder">if set to <c>true</c> checks the parameter order matches.</param>
        /// <param name="defaultCommandTimeout">The optional default command timeout, which will be used whenever this command is executed synchronously.
        /// Defaults to 30s.</param>
        /// <param name="constraintMode">The constraint mode.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram"/>.</returns>
        public static async Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> Create(
            [NotNull] LoadBalancedConnection connection,
            [NotNull] string name,
            [NotNull] string p1Name, 
            [NotNull] string p2Name, 
            [NotNull] string p3Name, 
            [NotNull] string p4Name, 
            [NotNull] string p5Name, 
            [NotNull] string p6Name, 
            [NotNull] string p7Name, 
            [NotNull] string p8Name, 
            [NotNull] string p9Name, 
            [NotNull] string p10Name, 
            [NotNull] string p11Name, 
            [NotNull] string p12Name, 
            [NotNull] string p13Name, 
            [NotNull] string p14Name, 
            [NotNull] string p15Name, 
            [NotNull] string p16Name, 
            [NotNull] string p17Name, 
            [NotNull] string p18Name, 
            [NotNull] string p19Name, 
            [NotNull] string p20Name, 
            [NotNull] string p21Name, 
            [NotNull] string p22Name, 
            [NotNull] string p23Name, 
            [NotNull] string p24Name, 
            [NotNull] string p25Name, 
            [NotNull] string p26Name, 
            [NotNull] string p27Name, 
            [NotNull] string p28Name, 
            [NotNull] string p29Name, 
            [NotNull] string p30Name, 
            [NotNull] string p31Name, 
            [NotNull] string p32Name,
            bool ignoreValidationErrors = false,
            bool checkOrder = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode constraintMode = TypeConstraintMode.Warn,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (p1Name == null) throw new ArgumentNullException("p1Name");
            if (p2Name == null) throw new ArgumentNullException("p2Name");
            if (p3Name == null) throw new ArgumentNullException("p3Name");
            if (p4Name == null) throw new ArgumentNullException("p4Name");
            if (p5Name == null) throw new ArgumentNullException("p5Name");
            if (p6Name == null) throw new ArgumentNullException("p6Name");
            if (p7Name == null) throw new ArgumentNullException("p7Name");
            if (p8Name == null) throw new ArgumentNullException("p8Name");
            if (p9Name == null) throw new ArgumentNullException("p9Name");
            if (p10Name == null) throw new ArgumentNullException("p10Name");
            if (p11Name == null) throw new ArgumentNullException("p11Name");
            if (p12Name == null) throw new ArgumentNullException("p12Name");
            if (p13Name == null) throw new ArgumentNullException("p13Name");
            if (p14Name == null) throw new ArgumentNullException("p14Name");
            if (p15Name == null) throw new ArgumentNullException("p15Name");
            if (p16Name == null) throw new ArgumentNullException("p16Name");
            if (p17Name == null) throw new ArgumentNullException("p17Name");
            if (p18Name == null) throw new ArgumentNullException("p18Name");
            if (p19Name == null) throw new ArgumentNullException("p19Name");
            if (p20Name == null) throw new ArgumentNullException("p20Name");
            if (p21Name == null) throw new ArgumentNullException("p21Name");
            if (p22Name == null) throw new ArgumentNullException("p22Name");
            if (p23Name == null) throw new ArgumentNullException("p23Name");
            if (p24Name == null) throw new ArgumentNullException("p24Name");
            if (p25Name == null) throw new ArgumentNullException("p25Name");
            if (p26Name == null) throw new ArgumentNullException("p26Name");
            if (p27Name == null) throw new ArgumentNullException("p27Name");
            if (p28Name == null) throw new ArgumentNullException("p28Name");
            if (p29Name == null) throw new ArgumentNullException("p29Name");
            if (p30Name == null) throw new ArgumentNullException("p30Name");
            if (p31Name == null) throw new ArgumentNullException("p31Name");
            if (p32Name == null) throw new ArgumentNullException("p32Name");
            SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> newProgram = new SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
                connection,
                name,
                new[] { new KeyValuePair<string, Type>(p1Name, typeof(T1)), new KeyValuePair<string, Type>(p2Name, typeof(T2)), new KeyValuePair<string, Type>(p3Name, typeof(T3)), new KeyValuePair<string, Type>(p4Name, typeof(T4)), new KeyValuePair<string, Type>(p5Name, typeof(T5)), new KeyValuePair<string, Type>(p6Name, typeof(T6)), new KeyValuePair<string, Type>(p7Name, typeof(T7)), new KeyValuePair<string, Type>(p8Name, typeof(T8)), new KeyValuePair<string, Type>(p9Name, typeof(T9)), new KeyValuePair<string, Type>(p10Name, typeof(T10)), new KeyValuePair<string, Type>(p11Name, typeof(T11)), new KeyValuePair<string, Type>(p12Name, typeof(T12)), new KeyValuePair<string, Type>(p13Name, typeof(T13)), new KeyValuePair<string, Type>(p14Name, typeof(T14)), new KeyValuePair<string, Type>(p15Name, typeof(T15)), new KeyValuePair<string, Type>(p16Name, typeof(T16)), new KeyValuePair<string, Type>(p17Name, typeof(T17)), new KeyValuePair<string, Type>(p18Name, typeof(T18)), new KeyValuePair<string, Type>(p19Name, typeof(T19)), new KeyValuePair<string, Type>(p20Name, typeof(T20)), new KeyValuePair<string, Type>(p21Name, typeof(T21)), new KeyValuePair<string, Type>(p22Name, typeof(T22)), new KeyValuePair<string, Type>(p23Name, typeof(T23)), new KeyValuePair<string, Type>(p24Name, typeof(T24)), new KeyValuePair<string, Type>(p25Name, typeof(T25)), new KeyValuePair<string, Type>(p26Name, typeof(T26)), new KeyValuePair<string, Type>(p27Name, typeof(T27)), new KeyValuePair<string, Type>(p28Name, typeof(T28)), new KeyValuePair<string, Type>(p29Name, typeof(T29)), new KeyValuePair<string, Type>(p30Name, typeof(T30)), new KeyValuePair<string, Type>(p31Name, typeof(T31)), new KeyValuePair<string, Type>(p32Name, typeof(T32)) },
                defaultCommandTimeout,
                constraintMode);

            // Validate
            await newProgram.Validate(checkOrder, false, !ignoreValidationErrors, cancellationToken).ConfigureAwait(false);

            return newProgram;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/> class.
        /// </summary>
        /// <param name="sqlProgram">The SQL program.</param>
        /// <param name="p1Name">Name of parameter 1.</param>
        /// <param name="p2Name">Name of parameter 2.</param>
        /// <param name="p3Name">Name of parameter 3.</param>
        /// <param name="p4Name">Name of parameter 4.</param>
        /// <param name="p5Name">Name of parameter 5.</param>
        /// <param name="p6Name">Name of parameter 6.</param>
        /// <param name="p7Name">Name of parameter 7.</param>
        /// <param name="p8Name">Name of parameter 8.</param>
        /// <param name="p9Name">Name of parameter 9.</param>
        /// <param name="p10Name">Name of parameter 10.</param>
        /// <param name="p11Name">Name of parameter 11.</param>
        /// <param name="p12Name">Name of parameter 12.</param>
        /// <param name="p13Name">Name of parameter 13.</param>
        /// <param name="p14Name">Name of parameter 14.</param>
        /// <param name="p15Name">Name of parameter 15.</param>
        /// <param name="p16Name">Name of parameter 16.</param>
        /// <param name="p17Name">Name of parameter 17.</param>
        /// <param name="p18Name">Name of parameter 18.</param>
        /// <param name="p19Name">Name of parameter 19.</param>
        /// <param name="p20Name">Name of parameter 20.</param>
        /// <param name="p21Name">Name of parameter 21.</param>
        /// <param name="p22Name">Name of parameter 22.</param>
        /// <param name="p23Name">Name of parameter 23.</param>
        /// <param name="p24Name">Name of parameter 24.</param>
        /// <param name="p25Name">Name of parameter 25.</param>
        /// <param name="p26Name">Name of parameter 26.</param>
        /// <param name="p27Name">Name of parameter 27.</param>
        /// <param name="p28Name">Name of parameter 28.</param>
        /// <param name="p29Name">Name of parameter 29.</param>
        /// <param name="p30Name">Name of parameter 30.</param>
        /// <param name="p31Name">Name of parameter 31.</param>
        /// <param name="p32Name">Name of parameter 32.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> does not throw validation errors (records them instead).</param>
        /// <param name="checkOrder">if set to <c>true</c> checks the parameter order matches.</param>
        /// <param name="defaultCommandTimeout">The optional default command timeout, which will be used whenever this command is executed synchronously.
        /// Defaults to existing programs default.</param>
        /// <param name="constraintMode">The constraint mode.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram"/>.</returns>
        public static async Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> Create(
            [NotNull] SqlProgram sqlProgram,
            [NotNull] string p1Name, 
            [NotNull] string p2Name, 
            [NotNull] string p3Name, 
            [NotNull] string p4Name, 
            [NotNull] string p5Name, 
            [NotNull] string p6Name, 
            [NotNull] string p7Name, 
            [NotNull] string p8Name, 
            [NotNull] string p9Name, 
            [NotNull] string p10Name, 
            [NotNull] string p11Name, 
            [NotNull] string p12Name, 
            [NotNull] string p13Name, 
            [NotNull] string p14Name, 
            [NotNull] string p15Name, 
            [NotNull] string p16Name, 
            [NotNull] string p17Name, 
            [NotNull] string p18Name, 
            [NotNull] string p19Name, 
            [NotNull] string p20Name, 
            [NotNull] string p21Name, 
            [NotNull] string p22Name, 
            [NotNull] string p23Name, 
            [NotNull] string p24Name, 
            [NotNull] string p25Name, 
            [NotNull] string p26Name, 
            [NotNull] string p27Name, 
            [NotNull] string p28Name, 
            [NotNull] string p29Name, 
            [NotNull] string p30Name, 
            [NotNull] string p31Name, 
            [NotNull] string p32Name,
            bool ignoreValidationErrors = false,
            bool checkOrder = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode constraintMode = TypeConstraintMode.Warn,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (sqlProgram == null) throw new ArgumentNullException("sqlProgram");
            if (p1Name == null) throw new ArgumentNullException("p1Name");
            if (p2Name == null) throw new ArgumentNullException("p2Name");
            if (p3Name == null) throw new ArgumentNullException("p3Name");
            if (p4Name == null) throw new ArgumentNullException("p4Name");
            if (p5Name == null) throw new ArgumentNullException("p5Name");
            if (p6Name == null) throw new ArgumentNullException("p6Name");
            if (p7Name == null) throw new ArgumentNullException("p7Name");
            if (p8Name == null) throw new ArgumentNullException("p8Name");
            if (p9Name == null) throw new ArgumentNullException("p9Name");
            if (p10Name == null) throw new ArgumentNullException("p10Name");
            if (p11Name == null) throw new ArgumentNullException("p11Name");
            if (p12Name == null) throw new ArgumentNullException("p12Name");
            if (p13Name == null) throw new ArgumentNullException("p13Name");
            if (p14Name == null) throw new ArgumentNullException("p14Name");
            if (p15Name == null) throw new ArgumentNullException("p15Name");
            if (p16Name == null) throw new ArgumentNullException("p16Name");
            if (p17Name == null) throw new ArgumentNullException("p17Name");
            if (p18Name == null) throw new ArgumentNullException("p18Name");
            if (p19Name == null) throw new ArgumentNullException("p19Name");
            if (p20Name == null) throw new ArgumentNullException("p20Name");
            if (p21Name == null) throw new ArgumentNullException("p21Name");
            if (p22Name == null) throw new ArgumentNullException("p22Name");
            if (p23Name == null) throw new ArgumentNullException("p23Name");
            if (p24Name == null) throw new ArgumentNullException("p24Name");
            if (p25Name == null) throw new ArgumentNullException("p25Name");
            if (p26Name == null) throw new ArgumentNullException("p26Name");
            if (p27Name == null) throw new ArgumentNullException("p27Name");
            if (p28Name == null) throw new ArgumentNullException("p28Name");
            if (p29Name == null) throw new ArgumentNullException("p29Name");
            if (p30Name == null) throw new ArgumentNullException("p30Name");
            if (p31Name == null) throw new ArgumentNullException("p31Name");
            if (p32Name == null) throw new ArgumentNullException("p32Name");
            SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> newProgram = new SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
                sqlProgram,
                new[] { new KeyValuePair<string, Type>(p1Name, typeof(T1)), new KeyValuePair<string, Type>(p2Name, typeof(T2)), new KeyValuePair<string, Type>(p3Name, typeof(T3)), new KeyValuePair<string, Type>(p4Name, typeof(T4)), new KeyValuePair<string, Type>(p5Name, typeof(T5)), new KeyValuePair<string, Type>(p6Name, typeof(T6)), new KeyValuePair<string, Type>(p7Name, typeof(T7)), new KeyValuePair<string, Type>(p8Name, typeof(T8)), new KeyValuePair<string, Type>(p9Name, typeof(T9)), new KeyValuePair<string, Type>(p10Name, typeof(T10)), new KeyValuePair<string, Type>(p11Name, typeof(T11)), new KeyValuePair<string, Type>(p12Name, typeof(T12)), new KeyValuePair<string, Type>(p13Name, typeof(T13)), new KeyValuePair<string, Type>(p14Name, typeof(T14)), new KeyValuePair<string, Type>(p15Name, typeof(T15)), new KeyValuePair<string, Type>(p16Name, typeof(T16)), new KeyValuePair<string, Type>(p17Name, typeof(T17)), new KeyValuePair<string, Type>(p18Name, typeof(T18)), new KeyValuePair<string, Type>(p19Name, typeof(T19)), new KeyValuePair<string, Type>(p20Name, typeof(T20)), new KeyValuePair<string, Type>(p21Name, typeof(T21)), new KeyValuePair<string, Type>(p22Name, typeof(T22)), new KeyValuePair<string, Type>(p23Name, typeof(T23)), new KeyValuePair<string, Type>(p24Name, typeof(T24)), new KeyValuePair<string, Type>(p25Name, typeof(T25)), new KeyValuePair<string, Type>(p26Name, typeof(T26)), new KeyValuePair<string, Type>(p27Name, typeof(T27)), new KeyValuePair<string, Type>(p28Name, typeof(T28)), new KeyValuePair<string, Type>(p29Name, typeof(T29)), new KeyValuePair<string, Type>(p30Name, typeof(T30)), new KeyValuePair<string, Type>(p31Name, typeof(T31)), new KeyValuePair<string, Type>(p32Name, typeof(T32)) },
                defaultCommandTimeout,
                constraintMode);

            // Validate
            await newProgram.Validate(checkOrder, false, !ignoreValidationErrors, cancellationToken).ConfigureAwait(false);

            return newProgram;
        }
        #endregion

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <returns>The scalar value.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public TOut ExecuteScalar<TOut>(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            return this.ExecuteScalar<TOut>(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)));
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The scalar value.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<TOut> ExecuteScalarAsync<TOut>(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ExecuteScalarAsync<TOut>(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), cancellationToken);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <returns>The scalar value for each connection.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<TOut> ExecuteScalarAll<TOut>(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteScalarAll<TOut>(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)));
        } 

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The scalar value for each connection.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<TOut>> ExecuteScalarAllAsync<TOut>(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteScalarAllAsync<TOut>(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), cancellationToken);
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public int ExecuteNonQuery(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            return this.ExecuteNonQuery(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)));
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<int> ExecuteNonQueryAsync(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ExecuteNonQueryAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), cancellationToken);
        }

        /// <summary>
        /// Executes a Transact-SQL statement against all the connections and returns the number of rows affected.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <returns>Number of rows affected for each connection.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<int> ExecuteNonQueryAll(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteNonQueryAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)));
        }

        /// <summary>
        /// Executes a Transact-SQL statement against all the connections and returns the number of rows affected.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of rows affected for each connection.</returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<int>> ExecuteNonQueryAllAsync(T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteNonQueryAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteReader([NotNull] ResultDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ExecuteReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteReader([NotNull] ResultDisposableDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ExecuteReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteReaderAsync([NotNull] ResultDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            return this.ExecuteReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteReaderAsync([NotNull] ResultDisposableDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            return this.ExecuteReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteReaderAll([NotNull] ResultDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            this.ExecuteReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteReaderAll([NotNull] ResultDisposableDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            this.ExecuteReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteReaderAllAsync([NotNull] ResultDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteReaderAllAsync([NotNull] ResultDisposableDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public TOut ExecuteReader<TOut>([NotNull] ResultDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public TOut ExecuteReader<TOut>([NotNull] ResultDisposableDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<TOut> ExecuteReaderAsync<TOut>([NotNull] ResultDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<TOut> ExecuteReaderAsync<TOut>([NotNull] ResultDisposableDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<TOut> ExecuteReaderAll<TOut>([NotNull] ResultDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<TOut> ExecuteReaderAll<TOut>([NotNull] ResultDisposableDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<TOut>> ExecuteReaderAllAsync<TOut>([NotNull] ResultDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="behavior">The behaviour.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<TOut>> ExecuteReaderAllAsync<TOut>([NotNull] ResultDisposableDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), CommandBehavior behavior = CommandBehavior.Default, TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteXmlReader([NotNull] XmlResultDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            this.ExecuteXmlReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteXmlReader([NotNull] XmlResultDisposableDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            this.ExecuteXmlReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteXmlReaderAsync([NotNull] XmlResultDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            return this.ExecuteXmlReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteXmlReaderAsync([NotNull] XmlResultDisposableDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            return this.ExecuteXmlReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteXmlReaderAll([NotNull] XmlResultDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            this.ExecuteXmlReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public void ExecuteXmlReaderAll([NotNull] XmlResultDisposableDelegate resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            this.ExecuteXmlReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteXmlReaderAllAsync([NotNull] XmlResultDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultAction">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task ExecuteXmlReaderAllAsync([NotNull] XmlResultDisposableDelegateAsync resultAction, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultAction == null) throw new ArgumentNullException("resultAction");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultAction, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public TOut ExecuteXmlReader<TOut>([NotNull] XmlResultDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteXmlReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public TOut ExecuteXmlReader<TOut>([NotNull] XmlResultDisposableDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteXmlReader(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<TOut> ExecuteXmlReaderAsync<TOut>([NotNull] XmlResultDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteXmlReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut">The type to return from the result function</typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<TOut> ExecuteXmlReaderAsync<TOut>([NotNull] XmlResultDisposableDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return this.ExecuteXmlReaderAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<TOut> ExecuteXmlReaderAll<TOut>([NotNull] XmlResultDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public IEnumerable<TOut> ExecuteXmlReaderAll<TOut>([NotNull] XmlResultDisposableDelegate<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null)
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAll(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc);
        }

        /// <summary>
        /// Executes the program with the specified parameters.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<TOut>> ExecuteXmlReaderAllAsync<TOut>([NotNull] XmlResultDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, cancellationToken);
        }

        /// <summary>
        /// Executes the program with the specified parameters, requires manual disposal.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configured default for this program.</param>
        /// <param name="resultFunc">The result function.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="SqlProgramExecutionException">An error occurred executing the program.</exception>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [NotNull]
        public Task<IEnumerable<TOut>> ExecuteXmlReaderAllAsync<TOut>([NotNull] XmlResultDisposableDelegateAsync<TOut> resultFunc, T1 p1Value = default(T1), T2 p2Value = default(T2), T3 p3Value = default(T3), T4 p4Value = default(T4), T5 p5Value = default(T5), T6 p6Value = default(T6), T7 p7Value = default(T7), T8 p8Value = default(T8), T9 p9Value = default(T9), T10 p10Value = default(T10), T11 p11Value = default(T11), T12 p12Value = default(T12), T13 p13Value = default(T13), T14 p14Value = default(T14), T15 p15Value = default(T15), T16 p16Value = default(T16), T17 p17Value = default(T17), T18 p18Value = default(T18), T19 p19Value = default(T19), T20 p20Value = default(T20), T21 p21Value = default(T21), T22 p22Value = default(T22), T23 p23Value = default(T23), T24 p24Value = default(T24), T25 p25Value = default(T25), T26 p26Value = default(T26), T27 p27Value = default(T27), T28 p28Value = default(T28), T29 p29Value = default(T29), T30 p30Value = default(T30), T31 p31Value = default(T31), T32 p32Value = default(T32), TypeConstraintMode? constraintMode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            ValidateExecuteAllOutParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value);
            return this.ExecuteXmlReaderAllAsync(c => c.SetParameters(p1Value, p2Value, p3Value, p4Value, p5Value, p6Value, p7Value, p8Value, p9Value, p10Value, p11Value, p12Value, p13Value, p14Value, p15Value, p16Value, p17Value, p18Value, p19Value, p20Value, p21Value, p22Value, p23Value, p24Value, p25Value, p26Value, p27Value, p28Value, p29Value, p30Value, p31Value, p32Value, (TypeConstraintMode)(constraintMode ?? ConstraintMode)), resultFunc, cancellationToken);
        }
        
        /// <summary>
        /// Validates the specified parameters are valid to use with an Execute*All overload.
        /// </summary>
        /// <param name="p1Value">Value of SQL Parameter 1.</param>
        /// <param name="p2Value">Value of SQL Parameter 2.</param>
        /// <param name="p3Value">Value of SQL Parameter 3.</param>
        /// <param name="p4Value">Value of SQL Parameter 4.</param>
        /// <param name="p5Value">Value of SQL Parameter 5.</param>
        /// <param name="p6Value">Value of SQL Parameter 6.</param>
        /// <param name="p7Value">Value of SQL Parameter 7.</param>
        /// <param name="p8Value">Value of SQL Parameter 8.</param>
        /// <param name="p9Value">Value of SQL Parameter 9.</param>
        /// <param name="p10Value">Value of SQL Parameter 10.</param>
        /// <param name="p11Value">Value of SQL Parameter 11.</param>
        /// <param name="p12Value">Value of SQL Parameter 12.</param>
        /// <param name="p13Value">Value of SQL Parameter 13.</param>
        /// <param name="p14Value">Value of SQL Parameter 14.</param>
        /// <param name="p15Value">Value of SQL Parameter 15.</param>
        /// <param name="p16Value">Value of SQL Parameter 16.</param>
        /// <param name="p17Value">Value of SQL Parameter 17.</param>
        /// <param name="p18Value">Value of SQL Parameter 18.</param>
        /// <param name="p19Value">Value of SQL Parameter 19.</param>
        /// <param name="p20Value">Value of SQL Parameter 20.</param>
        /// <param name="p21Value">Value of SQL Parameter 21.</param>
        /// <param name="p22Value">Value of SQL Parameter 22.</param>
        /// <param name="p23Value">Value of SQL Parameter 23.</param>
        /// <param name="p24Value">Value of SQL Parameter 24.</param>
        /// <param name="p25Value">Value of SQL Parameter 25.</param>
        /// <param name="p26Value">Value of SQL Parameter 26.</param>
        /// <param name="p27Value">Value of SQL Parameter 27.</param>
        /// <param name="p28Value">Value of SQL Parameter 28.</param>
        /// <param name="p29Value">Value of SQL Parameter 29.</param>
        /// <param name="p30Value">Value of SQL Parameter 30.</param>
        /// <param name="p31Value">Value of SQL Parameter 31.</param>
        /// <param name="p32Value">Value of SQL Parameter 32.</param>
        /// <exception cref="ArgumentException">Out&lt;T&gt; values are not allowed when executing against all connections. Use MultiOut&lt;T&gt; or omit the parameter instead.</exception>
        private void ValidateExecuteAllOutParameters(T1 p1Value, T2 p2Value, T3 p3Value, T4 p4Value, T5 p5Value, T6 p6Value, T7 p7Value, T8 p8Value, T9 p9Value, T10 p10Value, T11 p11Value, T12 p12Value, T13 p13Value, T14 p14Value, T15 p15Value, T16 p16Value, T17 p17Value, T18 p18Value, T19 p19Value, T20 p20Value, T21 p21Value, T22 p22Value, T23 p23Value, T24 p24Value, T25 p25Value, T26 p26Value, T27 p27Value, T28 p28Value, T29 p29Value, T30 p30Value, T31 p31Value, T32 p32Value)
        {
            if ((p1Value is IOut && !(p1Value is IMultiOut)) || 
                (p2Value is IOut && !(p2Value is IMultiOut)) || 
                (p3Value is IOut && !(p3Value is IMultiOut)) || 
                (p4Value is IOut && !(p4Value is IMultiOut)) || 
                (p5Value is IOut && !(p5Value is IMultiOut)) || 
                (p6Value is IOut && !(p6Value is IMultiOut)) || 
                (p7Value is IOut && !(p7Value is IMultiOut)) || 
                (p8Value is IOut && !(p8Value is IMultiOut)) || 
                (p9Value is IOut && !(p9Value is IMultiOut)) || 
                (p10Value is IOut && !(p10Value is IMultiOut)) || 
                (p11Value is IOut && !(p11Value is IMultiOut)) || 
                (p12Value is IOut && !(p12Value is IMultiOut)) || 
                (p13Value is IOut && !(p13Value is IMultiOut)) || 
                (p14Value is IOut && !(p14Value is IMultiOut)) || 
                (p15Value is IOut && !(p15Value is IMultiOut)) || 
                (p16Value is IOut && !(p16Value is IMultiOut)) || 
                (p17Value is IOut && !(p17Value is IMultiOut)) || 
                (p18Value is IOut && !(p18Value is IMultiOut)) || 
                (p19Value is IOut && !(p19Value is IMultiOut)) || 
                (p20Value is IOut && !(p20Value is IMultiOut)) || 
                (p21Value is IOut && !(p21Value is IMultiOut)) || 
                (p22Value is IOut && !(p22Value is IMultiOut)) || 
                (p23Value is IOut && !(p23Value is IMultiOut)) || 
                (p24Value is IOut && !(p24Value is IMultiOut)) || 
                (p25Value is IOut && !(p25Value is IMultiOut)) || 
                (p26Value is IOut && !(p26Value is IMultiOut)) || 
                (p27Value is IOut && !(p27Value is IMultiOut)) || 
                (p28Value is IOut && !(p28Value is IMultiOut)) || 
                (p29Value is IOut && !(p29Value is IMultiOut)) || 
                (p30Value is IOut && !(p30Value is IMultiOut)) || 
                (p31Value is IOut && !(p31Value is IMultiOut)) || 
                (p32Value is IOut && !(p32Value is IMultiOut)))
                throw new ArgumentException(Resources.SqlProgram_ValidateExecuteAllOutParameters_InvalidOut);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
    #endregion
}

namespace WebApplications.Utilities.Database.Configuration
{
    #region Extensions to DatabasesConfiguration
    /// <summary>
    /// Used to specify database configuration.
    /// </summary>
    /// <remarks></remarks>
    public partial class DatabasesConfiguration
    {    
        /// <summary>
        /// Gets the SQL program with the specified name and parameters, respecting the active configured options.
        /// </summary>
        /// <typeparam name="T1">The type of parameter 1.</typeparam>
        /// <typeparam name="T2">The type of parameter 2.</typeparam>
        /// <typeparam name="T3">The type of parameter 3.</typeparam>
        /// <typeparam name="T4">The type of parameter 4.</typeparam>
        /// <typeparam name="T5">The type of parameter 5.</typeparam>
        /// <typeparam name="T6">The type of parameter 6.</typeparam>
        /// <typeparam name="T7">The type of parameter 7.</typeparam>
        /// <typeparam name="T8">The type of parameter 8.</typeparam>
        /// <typeparam name="T9">The type of parameter 9.</typeparam>
        /// <typeparam name="T10">The type of parameter 10.</typeparam>
        /// <typeparam name="T11">The type of parameter 11.</typeparam>
        /// <typeparam name="T12">The type of parameter 12.</typeparam>
        /// <typeparam name="T13">The type of parameter 13.</typeparam>
        /// <typeparam name="T14">The type of parameter 14.</typeparam>
        /// <typeparam name="T15">The type of parameter 15.</typeparam>
        /// <typeparam name="T16">The type of parameter 16.</typeparam>
        /// <typeparam name="T17">The type of parameter 17.</typeparam>
        /// <typeparam name="T18">The type of parameter 18.</typeparam>
        /// <typeparam name="T19">The type of parameter 19.</typeparam>
        /// <typeparam name="T20">The type of parameter 20.</typeparam>
        /// <typeparam name="T21">The type of parameter 21.</typeparam>
        /// <typeparam name="T22">The type of parameter 22.</typeparam>
        /// <typeparam name="T23">The type of parameter 23.</typeparam>
        /// <typeparam name="T24">The type of parameter 24.</typeparam>
        /// <typeparam name="T25">The type of parameter 25.</typeparam>
        /// <typeparam name="T26">The type of parameter 26.</typeparam>
        /// <typeparam name="T27">The type of parameter 27.</typeparam>
        /// <typeparam name="T28">The type of parameter 28.</typeparam>
        /// <typeparam name="T29">The type of parameter 29.</typeparam>
        /// <typeparam name="T30">The type of parameter 30.</typeparam>
        /// <typeparam name="T31">The type of parameter 31.</typeparam>
        /// <typeparam name="T32">The type of parameter 32.</typeparam>
        /// <param name="database">The database id.</param>
        /// <param name="name">The name.</param>
        /// <param name="p1Name">Name of parameter 1.</param>
        /// <param name="p2Name">Name of parameter 2.</param>
        /// <param name="p3Name">Name of parameter 3.</param>
        /// <param name="p4Name">Name of parameter 4.</param>
        /// <param name="p5Name">Name of parameter 5.</param>
        /// <param name="p6Name">Name of parameter 6.</param>
        /// <param name="p7Name">Name of parameter 7.</param>
        /// <param name="p8Name">Name of parameter 8.</param>
        /// <param name="p9Name">Name of parameter 9.</param>
        /// <param name="p10Name">Name of parameter 10.</param>
        /// <param name="p11Name">Name of parameter 11.</param>
        /// <param name="p12Name">Name of parameter 12.</param>
        /// <param name="p13Name">Name of parameter 13.</param>
        /// <param name="p14Name">Name of parameter 14.</param>
        /// <param name="p15Name">Name of parameter 15.</param>
        /// <param name="p16Name">Name of parameter 16.</param>
        /// <param name="p17Name">Name of parameter 17.</param>
        /// <param name="p18Name">Name of parameter 18.</param>
        /// <param name="p19Name">Name of parameter 19.</param>
        /// <param name="p20Name">Name of parameter 20.</param>
        /// <param name="p21Name">Name of parameter 21.</param>
        /// <param name="p22Name">Name of parameter 22.</param>
        /// <param name="p23Name">Name of parameter 23.</param>
        /// <param name="p24Name">Name of parameter 24.</param>
        /// <param name="p25Name">Name of parameter 25.</param>
        /// <param name="p26Name">Name of parameter 26.</param>
        /// <param name="p27Name">Name of parameter 27.</param>
        /// <param name="p28Name">Name of parameter 28.</param>
        /// <param name="p29Name">Name of parameter 29.</param>
        /// <param name="p30Name">Name of parameter 30.</param>
        /// <param name="p31Name">Name of parameter 31.</param>
        /// <param name="p32Name">Name of parameter 32.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> will ignore validation errors regardless of configuration.</param>
        /// <param name="checkOrder">if set to <see langword="true"/> will check parameter order matches regardless of configuration.</param>
        /// <param name="defaultCommandTimeout">The default command timeout, if set will override the configuration.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/>.</returns>
        [NotNull]
        public static Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> GetConfiguredSqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
            [NotNull] string database,
            [NotNull] string name,
            [NotNull] string p1Name, 
            [NotNull] string p2Name, 
            [NotNull] string p3Name, 
            [NotNull] string p4Name, 
            [NotNull] string p5Name, 
            [NotNull] string p6Name, 
            [NotNull] string p7Name, 
            [NotNull] string p8Name, 
            [NotNull] string p9Name, 
            [NotNull] string p10Name, 
            [NotNull] string p11Name, 
            [NotNull] string p12Name, 
            [NotNull] string p13Name, 
            [NotNull] string p14Name, 
            [NotNull] string p15Name, 
            [NotNull] string p16Name, 
            [NotNull] string p17Name, 
            [NotNull] string p18Name, 
            [NotNull] string p19Name, 
            [NotNull] string p20Name, 
            [NotNull] string p21Name, 
            [NotNull] string p22Name, 
            [NotNull] string p23Name, 
            [NotNull] string p24Name, 
            [NotNull] string p25Name, 
            [NotNull] string p26Name, 
            [NotNull] string p27Name, 
            [NotNull] string p28Name, 
            [NotNull] string p29Name, 
            [NotNull] string p30Name, 
            [NotNull] string p31Name, 
            [NotNull] string p32Name,
            bool ignoreValidationErrors = false,
            bool checkOrder = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode? constraintMode = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (database == null) throw new ArgumentNullException("database");
            if (name == null) throw new ArgumentNullException("name");
            if (p1Name == null) throw new ArgumentNullException("p1Name");
            if (p2Name == null) throw new ArgumentNullException("p2Name");
            if (p3Name == null) throw new ArgumentNullException("p3Name");
            if (p4Name == null) throw new ArgumentNullException("p4Name");
            if (p5Name == null) throw new ArgumentNullException("p5Name");
            if (p6Name == null) throw new ArgumentNullException("p6Name");
            if (p7Name == null) throw new ArgumentNullException("p7Name");
            if (p8Name == null) throw new ArgumentNullException("p8Name");
            if (p9Name == null) throw new ArgumentNullException("p9Name");
            if (p10Name == null) throw new ArgumentNullException("p10Name");
            if (p11Name == null) throw new ArgumentNullException("p11Name");
            if (p12Name == null) throw new ArgumentNullException("p12Name");
            if (p13Name == null) throw new ArgumentNullException("p13Name");
            if (p14Name == null) throw new ArgumentNullException("p14Name");
            if (p15Name == null) throw new ArgumentNullException("p15Name");
            if (p16Name == null) throw new ArgumentNullException("p16Name");
            if (p17Name == null) throw new ArgumentNullException("p17Name");
            if (p18Name == null) throw new ArgumentNullException("p18Name");
            if (p19Name == null) throw new ArgumentNullException("p19Name");
            if (p20Name == null) throw new ArgumentNullException("p20Name");
            if (p21Name == null) throw new ArgumentNullException("p21Name");
            if (p22Name == null) throw new ArgumentNullException("p22Name");
            if (p23Name == null) throw new ArgumentNullException("p23Name");
            if (p24Name == null) throw new ArgumentNullException("p24Name");
            if (p25Name == null) throw new ArgumentNullException("p25Name");
            if (p26Name == null) throw new ArgumentNullException("p26Name");
            if (p27Name == null) throw new ArgumentNullException("p27Name");
            if (p28Name == null) throw new ArgumentNullException("p28Name");
            if (p29Name == null) throw new ArgumentNullException("p29Name");
            if (p30Name == null) throw new ArgumentNullException("p30Name");
            if (p31Name == null) throw new ArgumentNullException("p31Name");
            if (p32Name == null) throw new ArgumentNullException("p32Name");
            return Active.GetSqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(database, name, p1Name, p2Name, p3Name, p4Name, p5Name, p6Name, p7Name, p8Name, p9Name, p10Name, p11Name, p12Name, p13Name, p14Name, p15Name, p16Name, p17Name, p18Name, p19Name, p20Name, p21Name, p22Name, p23Name, p24Name, p25Name, p26Name, p27Name, p28Name, p29Name, p30Name, p31Name, p32Name, ignoreValidationErrors, checkOrder, defaultCommandTimeout, constraintMode, cancellationToken);
        }

        /// <summary>
        /// Gets the SQL program with the specified name and parameters, respecting configured options.
        /// </summary>
        /// <typeparam name="T1">The type of parameter 1.</typeparam>
        /// <typeparam name="T2">The type of parameter 2.</typeparam>
        /// <typeparam name="T3">The type of parameter 3.</typeparam>
        /// <typeparam name="T4">The type of parameter 4.</typeparam>
        /// <typeparam name="T5">The type of parameter 5.</typeparam>
        /// <typeparam name="T6">The type of parameter 6.</typeparam>
        /// <typeparam name="T7">The type of parameter 7.</typeparam>
        /// <typeparam name="T8">The type of parameter 8.</typeparam>
        /// <typeparam name="T9">The type of parameter 9.</typeparam>
        /// <typeparam name="T10">The type of parameter 10.</typeparam>
        /// <typeparam name="T11">The type of parameter 11.</typeparam>
        /// <typeparam name="T12">The type of parameter 12.</typeparam>
        /// <typeparam name="T13">The type of parameter 13.</typeparam>
        /// <typeparam name="T14">The type of parameter 14.</typeparam>
        /// <typeparam name="T15">The type of parameter 15.</typeparam>
        /// <typeparam name="T16">The type of parameter 16.</typeparam>
        /// <typeparam name="T17">The type of parameter 17.</typeparam>
        /// <typeparam name="T18">The type of parameter 18.</typeparam>
        /// <typeparam name="T19">The type of parameter 19.</typeparam>
        /// <typeparam name="T20">The type of parameter 20.</typeparam>
        /// <typeparam name="T21">The type of parameter 21.</typeparam>
        /// <typeparam name="T22">The type of parameter 22.</typeparam>
        /// <typeparam name="T23">The type of parameter 23.</typeparam>
        /// <typeparam name="T24">The type of parameter 24.</typeparam>
        /// <typeparam name="T25">The type of parameter 25.</typeparam>
        /// <typeparam name="T26">The type of parameter 26.</typeparam>
        /// <typeparam name="T27">The type of parameter 27.</typeparam>
        /// <typeparam name="T28">The type of parameter 28.</typeparam>
        /// <typeparam name="T29">The type of parameter 29.</typeparam>
        /// <typeparam name="T30">The type of parameter 30.</typeparam>
        /// <typeparam name="T31">The type of parameter 31.</typeparam>
        /// <typeparam name="T32">The type of parameter 32.</typeparam>
        /// <param name="database">The database id.</param>
        /// <param name="name">The name.</param>
        /// <param name="p1Name">Name of parameter 1.</param>
        /// <param name="p2Name">Name of parameter 2.</param>
        /// <param name="p3Name">Name of parameter 3.</param>
        /// <param name="p4Name">Name of parameter 4.</param>
        /// <param name="p5Name">Name of parameter 5.</param>
        /// <param name="p6Name">Name of parameter 6.</param>
        /// <param name="p7Name">Name of parameter 7.</param>
        /// <param name="p8Name">Name of parameter 8.</param>
        /// <param name="p9Name">Name of parameter 9.</param>
        /// <param name="p10Name">Name of parameter 10.</param>
        /// <param name="p11Name">Name of parameter 11.</param>
        /// <param name="p12Name">Name of parameter 12.</param>
        /// <param name="p13Name">Name of parameter 13.</param>
        /// <param name="p14Name">Name of parameter 14.</param>
        /// <param name="p15Name">Name of parameter 15.</param>
        /// <param name="p16Name">Name of parameter 16.</param>
        /// <param name="p17Name">Name of parameter 17.</param>
        /// <param name="p18Name">Name of parameter 18.</param>
        /// <param name="p19Name">Name of parameter 19.</param>
        /// <param name="p20Name">Name of parameter 20.</param>
        /// <param name="p21Name">Name of parameter 21.</param>
        /// <param name="p22Name">Name of parameter 22.</param>
        /// <param name="p23Name">Name of parameter 23.</param>
        /// <param name="p24Name">Name of parameter 24.</param>
        /// <param name="p25Name">Name of parameter 25.</param>
        /// <param name="p26Name">Name of parameter 26.</param>
        /// <param name="p27Name">Name of parameter 27.</param>
        /// <param name="p28Name">Name of parameter 28.</param>
        /// <param name="p29Name">Name of parameter 29.</param>
        /// <param name="p30Name">Name of parameter 30.</param>
        /// <param name="p31Name">Name of parameter 31.</param>
        /// <param name="p32Name">Name of parameter 32.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> will ignore validation errors regardless of configuration.</param>
        /// <param name="checkOrder">if set to <see langword="true"/> will check parameter order matches regardless of configuration.</param>
        /// <param name="defaultCommandTimeout">The default command timeout, if set will override the configuration.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/>.</returns>
        [NotNull]
        public Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> GetSqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
            [NotNull] string database, 
            [NotNull] string name,
            [NotNull] string p1Name, 
            [NotNull] string p2Name, 
            [NotNull] string p3Name, 
            [NotNull] string p4Name, 
            [NotNull] string p5Name, 
            [NotNull] string p6Name, 
            [NotNull] string p7Name, 
            [NotNull] string p8Name, 
            [NotNull] string p9Name, 
            [NotNull] string p10Name, 
            [NotNull] string p11Name, 
            [NotNull] string p12Name, 
            [NotNull] string p13Name, 
            [NotNull] string p14Name, 
            [NotNull] string p15Name, 
            [NotNull] string p16Name, 
            [NotNull] string p17Name, 
            [NotNull] string p18Name, 
            [NotNull] string p19Name, 
            [NotNull] string p20Name, 
            [NotNull] string p21Name, 
            [NotNull] string p22Name, 
            [NotNull] string p23Name, 
            [NotNull] string p24Name, 
            [NotNull] string p25Name, 
            [NotNull] string p26Name, 
            [NotNull] string p27Name, 
            [NotNull] string p28Name, 
            [NotNull] string p29Name, 
            [NotNull] string p30Name, 
            [NotNull] string p31Name, 
            [NotNull] string p32Name,
            bool ignoreValidationErrors = false,
            bool checkOrder = false,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode? constraintMode = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (database == null) throw new ArgumentNullException("database");
            if (name == null) throw new ArgumentNullException("name");
            if (p1Name == null) throw new ArgumentNullException("p1Name");
            if (p2Name == null) throw new ArgumentNullException("p2Name");
            if (p3Name == null) throw new ArgumentNullException("p3Name");
            if (p4Name == null) throw new ArgumentNullException("p4Name");
            if (p5Name == null) throw new ArgumentNullException("p5Name");
            if (p6Name == null) throw new ArgumentNullException("p6Name");
            if (p7Name == null) throw new ArgumentNullException("p7Name");
            if (p8Name == null) throw new ArgumentNullException("p8Name");
            if (p9Name == null) throw new ArgumentNullException("p9Name");
            if (p10Name == null) throw new ArgumentNullException("p10Name");
            if (p11Name == null) throw new ArgumentNullException("p11Name");
            if (p12Name == null) throw new ArgumentNullException("p12Name");
            if (p13Name == null) throw new ArgumentNullException("p13Name");
            if (p14Name == null) throw new ArgumentNullException("p14Name");
            if (p15Name == null) throw new ArgumentNullException("p15Name");
            if (p16Name == null) throw new ArgumentNullException("p16Name");
            if (p17Name == null) throw new ArgumentNullException("p17Name");
            if (p18Name == null) throw new ArgumentNullException("p18Name");
            if (p19Name == null) throw new ArgumentNullException("p19Name");
            if (p20Name == null) throw new ArgumentNullException("p20Name");
            if (p21Name == null) throw new ArgumentNullException("p21Name");
            if (p22Name == null) throw new ArgumentNullException("p22Name");
            if (p23Name == null) throw new ArgumentNullException("p23Name");
            if (p24Name == null) throw new ArgumentNullException("p24Name");
            if (p25Name == null) throw new ArgumentNullException("p25Name");
            if (p26Name == null) throw new ArgumentNullException("p26Name");
            if (p27Name == null) throw new ArgumentNullException("p27Name");
            if (p28Name == null) throw new ArgumentNullException("p28Name");
            if (p29Name == null) throw new ArgumentNullException("p29Name");
            if (p30Name == null) throw new ArgumentNullException("p30Name");
            if (p31Name == null) throw new ArgumentNullException("p31Name");
            if (p32Name == null) throw new ArgumentNullException("p32Name");
            // We have to find the database otherwise we cannot get a load balanced connection.
            DatabaseElement db = Databases[database];
            if ((db == null) ||
                (!db.Enabled))
                {
                    return TaskResult<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>>.FromException(
                        new LoggingException(
                            () => Resources.DatabaseConfiguration_GetSqlProgram_DatabaseIdNotFound,
                            database));
                }

            return db.GetSqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(name, p1Name, p2Name, p3Name, p4Name, p5Name, p6Name, p7Name, p8Name, p9Name, p10Name, p11Name, p12Name, p13Name, p14Name, p15Name, p16Name, p17Name, p18Name, p19Name, p20Name, p21Name, p22Name, p23Name, p24Name, p25Name, p26Name, p27Name, p28Name, p29Name, p30Name, p31Name, p32Name, ignoreValidationErrors, checkOrder, defaultCommandTimeout, constraintMode, cancellationToken);
        }
    }
    #endregion
    
    #region Extensions to DatabaseElement
    /// <summary>
    /// Used to specify database configuration.
    /// </summary>
    /// <remarks></remarks>
    public partial class DatabaseElement
    {
        /// <summary>
        /// Gets the SQL program with the specified name and parameters, respecting configured options.
        /// </summary>
        /// <typeparam name="T1">The type of parameter 1.</typeparam>
        /// <typeparam name="T2">The type of parameter 2.</typeparam>
        /// <typeparam name="T3">The type of parameter 3.</typeparam>
        /// <typeparam name="T4">The type of parameter 4.</typeparam>
        /// <typeparam name="T5">The type of parameter 5.</typeparam>
        /// <typeparam name="T6">The type of parameter 6.</typeparam>
        /// <typeparam name="T7">The type of parameter 7.</typeparam>
        /// <typeparam name="T8">The type of parameter 8.</typeparam>
        /// <typeparam name="T9">The type of parameter 9.</typeparam>
        /// <typeparam name="T10">The type of parameter 10.</typeparam>
        /// <typeparam name="T11">The type of parameter 11.</typeparam>
        /// <typeparam name="T12">The type of parameter 12.</typeparam>
        /// <typeparam name="T13">The type of parameter 13.</typeparam>
        /// <typeparam name="T14">The type of parameter 14.</typeparam>
        /// <typeparam name="T15">The type of parameter 15.</typeparam>
        /// <typeparam name="T16">The type of parameter 16.</typeparam>
        /// <typeparam name="T17">The type of parameter 17.</typeparam>
        /// <typeparam name="T18">The type of parameter 18.</typeparam>
        /// <typeparam name="T19">The type of parameter 19.</typeparam>
        /// <typeparam name="T20">The type of parameter 20.</typeparam>
        /// <typeparam name="T21">The type of parameter 21.</typeparam>
        /// <typeparam name="T22">The type of parameter 22.</typeparam>
        /// <typeparam name="T23">The type of parameter 23.</typeparam>
        /// <typeparam name="T24">The type of parameter 24.</typeparam>
        /// <typeparam name="T25">The type of parameter 25.</typeparam>
        /// <typeparam name="T26">The type of parameter 26.</typeparam>
        /// <typeparam name="T27">The type of parameter 27.</typeparam>
        /// <typeparam name="T28">The type of parameter 28.</typeparam>
        /// <typeparam name="T29">The type of parameter 29.</typeparam>
        /// <typeparam name="T30">The type of parameter 30.</typeparam>
        /// <typeparam name="T31">The type of parameter 31.</typeparam>
        /// <typeparam name="T32">The type of parameter 32.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="p1Name">Name of parameter 1.</param>
        /// <param name="p2Name">Name of parameter 2.</param>
        /// <param name="p3Name">Name of parameter 3.</param>
        /// <param name="p4Name">Name of parameter 4.</param>
        /// <param name="p5Name">Name of parameter 5.</param>
        /// <param name="p6Name">Name of parameter 6.</param>
        /// <param name="p7Name">Name of parameter 7.</param>
        /// <param name="p8Name">Name of parameter 8.</param>
        /// <param name="p9Name">Name of parameter 9.</param>
        /// <param name="p10Name">Name of parameter 10.</param>
        /// <param name="p11Name">Name of parameter 11.</param>
        /// <param name="p12Name">Name of parameter 12.</param>
        /// <param name="p13Name">Name of parameter 13.</param>
        /// <param name="p14Name">Name of parameter 14.</param>
        /// <param name="p15Name">Name of parameter 15.</param>
        /// <param name="p16Name">Name of parameter 16.</param>
        /// <param name="p17Name">Name of parameter 17.</param>
        /// <param name="p18Name">Name of parameter 18.</param>
        /// <param name="p19Name">Name of parameter 19.</param>
        /// <param name="p20Name">Name of parameter 20.</param>
        /// <param name="p21Name">Name of parameter 21.</param>
        /// <param name="p22Name">Name of parameter 22.</param>
        /// <param name="p23Name">Name of parameter 23.</param>
        /// <param name="p24Name">Name of parameter 24.</param>
        /// <param name="p25Name">Name of parameter 25.</param>
        /// <param name="p26Name">Name of parameter 26.</param>
        /// <param name="p27Name">Name of parameter 27.</param>
        /// <param name="p28Name">Name of parameter 28.</param>
        /// <param name="p29Name">Name of parameter 29.</param>
        /// <param name="p30Name">Name of parameter 30.</param>
        /// <param name="p31Name">Name of parameter 31.</param>
        /// <param name="p32Name">Name of parameter 32.</param>
        /// <param name="ignoreValidationErrors">if set to <see langword="true"/> will ignore validation errors regardless of configuration.</param>
        /// <param name="checkOrder">if set to <see langword="true"/> will check parameter order matches regardless of configuration.</param>
        /// <param name="defaultCommandTimeout">The default command timeout, if set will override the configuration.</param>
        /// <param name="constraintMode">The constraint mode, if set will override the configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task, resulting in a <see cref="SqlProgram&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32&gt;"/>.</returns>
        [NotNull]
        public async Task<SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>> GetSqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>(
            [NotNull] string name,
            [NotNull] string p1Name, 
            [NotNull] string p2Name, 
            [NotNull] string p3Name, 
            [NotNull] string p4Name, 
            [NotNull] string p5Name, 
            [NotNull] string p6Name, 
            [NotNull] string p7Name, 
            [NotNull] string p8Name, 
            [NotNull] string p9Name, 
            [NotNull] string p10Name, 
            [NotNull] string p11Name, 
            [NotNull] string p12Name, 
            [NotNull] string p13Name, 
            [NotNull] string p14Name, 
            [NotNull] string p15Name, 
            [NotNull] string p16Name, 
            [NotNull] string p17Name, 
            [NotNull] string p18Name, 
            [NotNull] string p19Name, 
            [NotNull] string p20Name, 
            [NotNull] string p21Name, 
            [NotNull] string p22Name, 
            [NotNull] string p23Name, 
            [NotNull] string p24Name, 
            [NotNull] string p25Name, 
            [NotNull] string p26Name, 
            [NotNull] string p27Name, 
            [NotNull] string p28Name, 
            [NotNull] string p29Name, 
            [NotNull] string p30Name, 
            [NotNull] string p31Name, 
            [NotNull] string p32Name,
            bool? ignoreValidationErrors = null,
            bool? checkOrder = null,
            TimeSpan? defaultCommandTimeout = null,
            TypeConstraintMode? constraintMode = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (name == null) throw new ArgumentNullException("name");
            if (p1Name == null) throw new ArgumentNullException("p1Name");
            if (p2Name == null) throw new ArgumentNullException("p2Name");
            if (p3Name == null) throw new ArgumentNullException("p3Name");
            if (p4Name == null) throw new ArgumentNullException("p4Name");
            if (p5Name == null) throw new ArgumentNullException("p5Name");
            if (p6Name == null) throw new ArgumentNullException("p6Name");
            if (p7Name == null) throw new ArgumentNullException("p7Name");
            if (p8Name == null) throw new ArgumentNullException("p8Name");
            if (p9Name == null) throw new ArgumentNullException("p9Name");
            if (p10Name == null) throw new ArgumentNullException("p10Name");
            if (p11Name == null) throw new ArgumentNullException("p11Name");
            if (p12Name == null) throw new ArgumentNullException("p12Name");
            if (p13Name == null) throw new ArgumentNullException("p13Name");
            if (p14Name == null) throw new ArgumentNullException("p14Name");
            if (p15Name == null) throw new ArgumentNullException("p15Name");
            if (p16Name == null) throw new ArgumentNullException("p16Name");
            if (p17Name == null) throw new ArgumentNullException("p17Name");
            if (p18Name == null) throw new ArgumentNullException("p18Name");
            if (p19Name == null) throw new ArgumentNullException("p19Name");
            if (p20Name == null) throw new ArgumentNullException("p20Name");
            if (p21Name == null) throw new ArgumentNullException("p21Name");
            if (p22Name == null) throw new ArgumentNullException("p22Name");
            if (p23Name == null) throw new ArgumentNullException("p23Name");
            if (p24Name == null) throw new ArgumentNullException("p24Name");
            if (p25Name == null) throw new ArgumentNullException("p25Name");
            if (p26Name == null) throw new ArgumentNullException("p26Name");
            if (p27Name == null) throw new ArgumentNullException("p27Name");
            if (p28Name == null) throw new ArgumentNullException("p28Name");
            if (p29Name == null) throw new ArgumentNullException("p29Name");
            if (p30Name == null) throw new ArgumentNullException("p30Name");
            if (p31Name == null) throw new ArgumentNullException("p31Name");
            if (p32Name == null) throw new ArgumentNullException("p32Name");
            // Grab the default load balanced connection for the database.
            LoadBalancedConnectionElement connectionElement = this.Connections.FirstOrDefault(c => c.Enabled);

            if (connectionElement == null)
                throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Unknown_Database, this.Id);
            
            // Look for program mapping information
            ProgramElement prog = this.Programs[name];
            if (prog != null)
            {
                // Check for name mapping
                if (!String.IsNullOrWhiteSpace(prog.MapTo))
                    name = prog.MapTo;

                // Set options if not already set.
                ignoreValidationErrors = ignoreValidationErrors ?? prog.IgnoreValidationErrors;
                checkOrder = checkOrder ?? prog.CheckOrder;
                defaultCommandTimeout = defaultCommandTimeout ?? prog.DefaultCommandTimeout;
                constraintMode = constraintMode ?? prog.ConstraintMode;

                if (!String.IsNullOrEmpty(prog.Connection))
                {
                    connectionElement = this.Connections[prog.Connection];
                    if ((connectionElement == null) ||
                        (!connectionElement.Enabled))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Unknown_Database_Program,
                            prog.Connection, this.Id, name);
                }
                
                // Check for parameter mappings
                ParameterElement param;
                param = prog.Parameters[p1Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p1Name, prog.Name);
                
                    p1Name = param.MapTo;
                }
                param = prog.Parameters[p2Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p2Name, prog.Name);
                
                    p2Name = param.MapTo;
                }
                param = prog.Parameters[p3Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p3Name, prog.Name);
                
                    p3Name = param.MapTo;
                }
                param = prog.Parameters[p4Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p4Name, prog.Name);
                
                    p4Name = param.MapTo;
                }
                param = prog.Parameters[p5Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p5Name, prog.Name);
                
                    p5Name = param.MapTo;
                }
                param = prog.Parameters[p6Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p6Name, prog.Name);
                
                    p6Name = param.MapTo;
                }
                param = prog.Parameters[p7Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p7Name, prog.Name);
                
                    p7Name = param.MapTo;
                }
                param = prog.Parameters[p8Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p8Name, prog.Name);
                
                    p8Name = param.MapTo;
                }
                param = prog.Parameters[p9Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p9Name, prog.Name);
                
                    p9Name = param.MapTo;
                }
                param = prog.Parameters[p10Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p10Name, prog.Name);
                
                    p10Name = param.MapTo;
                }
                param = prog.Parameters[p11Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p11Name, prog.Name);
                
                    p11Name = param.MapTo;
                }
                param = prog.Parameters[p12Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p12Name, prog.Name);
                
                    p12Name = param.MapTo;
                }
                param = prog.Parameters[p13Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p13Name, prog.Name);
                
                    p13Name = param.MapTo;
                }
                param = prog.Parameters[p14Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p14Name, prog.Name);
                
                    p14Name = param.MapTo;
                }
                param = prog.Parameters[p15Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p15Name, prog.Name);
                
                    p15Name = param.MapTo;
                }
                param = prog.Parameters[p16Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p16Name, prog.Name);
                
                    p16Name = param.MapTo;
                }
                param = prog.Parameters[p17Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p17Name, prog.Name);
                
                    p17Name = param.MapTo;
                }
                param = prog.Parameters[p18Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p18Name, prog.Name);
                
                    p18Name = param.MapTo;
                }
                param = prog.Parameters[p19Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p19Name, prog.Name);
                
                    p19Name = param.MapTo;
                }
                param = prog.Parameters[p20Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p20Name, prog.Name);
                
                    p20Name = param.MapTo;
                }
                param = prog.Parameters[p21Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p21Name, prog.Name);
                
                    p21Name = param.MapTo;
                }
                param = prog.Parameters[p22Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p22Name, prog.Name);
                
                    p22Name = param.MapTo;
                }
                param = prog.Parameters[p23Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p23Name, prog.Name);
                
                    p23Name = param.MapTo;
                }
                param = prog.Parameters[p24Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p24Name, prog.Name);
                
                    p24Name = param.MapTo;
                }
                param = prog.Parameters[p25Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p25Name, prog.Name);
                
                    p25Name = param.MapTo;
                }
                param = prog.Parameters[p26Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p26Name, prog.Name);
                
                    p26Name = param.MapTo;
                }
                param = prog.Parameters[p27Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p27Name, prog.Name);
                
                    p27Name = param.MapTo;
                }
                param = prog.Parameters[p28Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p28Name, prog.Name);
                
                    p28Name = param.MapTo;
                }
                param = prog.Parameters[p29Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p29Name, prog.Name);
                
                    p29Name = param.MapTo;
                }
                param = prog.Parameters[p30Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p30Name, prog.Name);
                
                    p30Name = param.MapTo;
                }
                param = prog.Parameters[p31Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p31Name, prog.Name);
                
                    p31Name = param.MapTo;
                }
                param = prog.Parameters[p32Name];
                if (param != null) {
                    if (String.IsNullOrWhiteSpace(param.MapTo))
                        throw new LoggingException(() => Resources.DatabaseElement_GetSqlProgram_Invalid_Mapping,
                            p32Name, prog.Name);
                
                    p32Name = param.MapTo;
                }
            }
            
            if (ignoreValidationErrors == null) ignoreValidationErrors = false;
            if (checkOrder == null) checkOrder = false;
            if (constraintMode == null) constraintMode = TypeConstraintMode.Warn;

            LoadBalancedConnection connection = await connectionElement.GetLoadBalancedConnection(cancellationToken).ConfigureAwait(false);

            return await SqlProgram<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>.Create(connection, name, p1Name, p2Name, p3Name, p4Name, p5Name, p6Name, p7Name, p8Name, p9Name, p10Name, p11Name, p12Name, p13Name, p14Name, p15Name, p16Name, p17Name, p18Name, p19Name, p20Name, p21Name, p22Name, p23Name, p24Name, p25Name, p26Name, p27Name, p28Name, p29Name, p30Name, p31Name, p32Name, ignoreValidationErrors.Value, checkOrder.Value, defaultCommandTimeout, (TypeConstraintMode) constraintMode, cancellationToken).ConfigureAwait(false);
        }
    }
    #endregion
}
#endregion
