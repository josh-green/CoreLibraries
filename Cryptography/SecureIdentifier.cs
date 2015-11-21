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
using System.Security.Cryptography;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Cryptography.Configuration;

namespace WebApplications.Utilities.Cryptography
{
    /// <summary>
    /// A secure identifier that can be used safely in a URL (as it's base64 encoded) and can be more secure than a
    /// GUID as  it uses a cryptographically secure random number generator.  It also has a built in hash to ensure
    /// only certain identifiers are valid, making it capable of rejecting invalid identifiers (for example during
    /// a DDOS attack).
    /// </summary>
    public class SecureIdentifier : IComparable<SecureIdentifier>,
        IEquatable<SecureIdentifier>
    {
        /// <summary>
        /// The minimum length.
        /// </summary>
        [PublicAPI]
        public const byte MinLength = 12;

        /// <summary>
        /// The base64 encoded string.
        /// </summary>
        [NotNull]
        private readonly string _base64Encoded;

        /// <summary>
        /// The hash code embedded into the byte array for consistency checking.
        /// </summary>
        private readonly int _hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureIdentifier" /> class.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public SecureIdentifier(byte length = 0)
        {
            if (length == 0) length = (byte)CryptographyConfiguration.Active.SecureIdentifierSize;

            if (length < MinLength)
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    string.Format(Resources.SecureIdentifier_Ctor_Invalid_Length_Supplied, MinLength));
            
            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            byte[] bytes = new byte[length];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                rng.GetBytes(bytes);

            // Calculate hash code
            uint hashCode = 2147483647;
            for (int i = 0; i < length - 4; i++)
                unchecked
                {
                    hashCode = (hashCode * 397) + bytes[i];
                }
            _hashCode = unchecked((int)hashCode);

            // Set at end of byte array.
            for (int i = length - 4; i < length; i++)
            {
                bytes[i] = unchecked((byte)hashCode);
                hashCode /= 256;
            }

            _base64Encoded = Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureIdentifier" /> class.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <param name="base64Encoded">The base64 encoded.</param>
        private SecureIdentifier(int hashCode, [NotNull] string base64Encoded)
        {
            _hashCode = hashCode;
            _base64Encoded = base64Encoded;
        }

        /// <summary>
        /// Parses the specified value as a <see cref="SecureIdentifier" />.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <returns>SecureIdentifier.</returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException">
        /// </exception>
        /// <exception cref="CryptographicException">The secure identifier is invalid</exception>
        /// <exception cref="System.ArgumentException"></exception>
        [PublicAPI]
        public static SecureIdentifier Parse([NotNull] string value)
        {
            SecureIdentifier result;
            if (!TryParse(value, out result))
                throw new CryptographicException(Resources.SecureIdentifier_Parse_Invalid);
            return result;
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result. Out.</param>
        /// <returns></returns>
        [PublicAPI]
        public static bool TryParse([NotNull] string value, out SecureIdentifier result)
        {
            result = null;
            try
            {
                byte[] bytes = Convert.FromBase64String(value);
                int length = bytes.Length;
                if (length < MinLength) return false;

                // Calculate hashcode
                uint hashCode = 2147483647;
                for (int i = 0; i < length - 4; i++)
                    unchecked
                    {
                        hashCode = (hashCode * 397) + bytes[i];
                    }

                int hc = unchecked((int)hashCode);

                // Validate
                for (int i = length - 4; i < length; i++)
                {
                    if (unchecked((byte)hashCode) != bytes[i])
                        return false;
                    hashCode /= 256;
                }

                result = new SecureIdentifier(hc, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => _base64Encoded;

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(SecureIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _hashCode == other._hashCode &&
                   string.Equals(_base64Encoded, other._base64Encoded);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SecureIdentifier)obj);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SecureIdentifier left, SecureIdentifier right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SecureIdentifier left, SecureIdentifier right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Compares this SecureIdentifier to an object, which will be cast to a SecureIdentifier during the comparision.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        [PublicAPI]
        public int CompareTo(object other)
        {
            return CompareTo(other as SecureIdentifier);
        }

        /// <summary>
        /// Compares this SecureIdentifier to another.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        [PublicAPI]
        public int CompareTo(SecureIdentifier other)
        {
            if (ReferenceEquals(null, other)) return 1;
            if (Equals(this, other)) return 0;
            
            int d = _hashCode.CompareTo(other._hashCode);
            return d != 0 ? d : string.CompareOrdinal(_base64Encoded, other._base64Encoded);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(SecureIdentifier left, SecureIdentifier right)
        {
            if (ReferenceEquals(left, null)) return true;
            return (left.CompareTo(right) >= 0);
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(SecureIdentifier left, SecureIdentifier right)
        {
            return ReferenceEquals(left, null)
                ? ReferenceEquals(right, null)
                : left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(SecureIdentifier left, SecureIdentifier right)
        {
            return ReferenceEquals(left, null)
                ? !ReferenceEquals(right, null)
                : left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(SecureIdentifier left, SecureIdentifier right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => _hashCode;
        
        /// <summary>
        /// Implements the operator implicit string.
        /// </summary>
        /// <param name="secureIdentifier">The secure identifier.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [NotNull]
        public static implicit operator string([NotNull] SecureIdentifier secureIdentifier)
            => secureIdentifier._base64Encoded;

        /// <summary>
        /// Implements the operator explicit SecureIdentifier.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [CanBeNull]
        public static implicit operator SecureIdentifier([NotNull] string value) => Parse(value);
    }
}