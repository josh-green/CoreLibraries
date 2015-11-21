#region � Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
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
using System.Xml.Linq;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Cryptography
{
    /// <summary>
    /// Base class for all hashing cryptographic providers.
    /// </summary>
    public class HashingCryptographyProvider : CryptographyProvider
    {
        /// <inheritdoc />
        public override bool CanEncrypt => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashingCryptographyProvider" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="configuration">The configuration.</param>
        protected HashingCryptographyProvider(
            [NotNull] string name,
            [NotNull] XElement configuration)
            // We set preserves length to true, even though it's doesn't preserve length as we can't decrypt anyway.
            : base(name, configuration, true)
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform GetEncryptor()
        {
            HashAlgorithm algorithm = CryptoConfig.CreateFromName(Name) as HashAlgorithm;
            if (algorithm == null) throw new InvalidOperationException(string.Format(Resources.HashingCryptographyProvider_GetEncryptor_Create_Failed, Name));
            
            return new CryptoTransform<HashAlgorithm>(algorithm);
        }

        /// <inheritdoc />
        public override ICryptoTransform GetDecryptor()
        {
            throw new CryptographicException(Resources.CryptographyProvider_Decryption_Not_Supported);
        }

        /// <summary>
        /// Creates a <see cref="CryptographyProvider" /> from an <see cref="AsymmetricAlgorithm" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="configurationElement">The optional configuration element.</param>
        /// <returns>A <see cref="CryptographyProvider" />.</returns>
        /// <exception cref="CryptographicException">The algorithm is unsupported.</exception>
        [NotNull]
        internal static HashingCryptographyProvider Create(
            [NotNull] string name,
            [NotNull] HashAlgorithm algorithm,
            [CanBeNull] XElement configurationElement = null)
        {
            // Check for keyed hashing algorithmns
            KeyedHashAlgorithm keyed = algorithm as KeyedHashAlgorithm;
            if (keyed != null) return KeyedHashingCryptographyProvider.Create(name, keyed, configurationElement);

            // Simple hashing algorithm, no real configuration.
            if (configurationElement == null)
                configurationElement = new XElement("configuration");

            return new HashingCryptographyProvider(name, configurationElement);
        }
    }
}