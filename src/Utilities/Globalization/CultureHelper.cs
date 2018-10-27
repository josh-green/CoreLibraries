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

#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using WebApplications.Utilities.Annotations;

#endregion

namespace WebApplications.Utilities.Globalization
{
    /// <summary>
    ///   Helps map cultures, regions and currencies.
    /// </summary>
    [PublicAPI]
    public static class CultureHelper
    {
        /// <summary>
        ///   A lookup of <see cref="CultureInfo"/>s and <see cref="System.Globalization.RegionInfo"/>s
        ///   by currency ISO code (e.g. USD, GBP, JPY).
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, Dictionary<CultureInfo, RegionInfo>> _currencyCultureInfo;

        /// <summary>
        ///   A lookup of regions by their two and three letter ISO names.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, RegionInfo> _regionIsoNames;

        /// <summary>
        ///   A lookup of regions by their English name.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, RegionInfo> _regionNames;

        /// <summary>
        /// All the regions by LCID.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<int, RegionInfo> _regionLcids;

        /// <summary>
        ///   A lookup of culture by their two and three letter ISO names.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, CultureInfo> _cultureIsoNames;

        /// <summary>
        ///   All the specified culture names.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, CultureInfo> _cultureNames;

        /// <summary>
        /// All the cultures by LCID.
        /// </summary>
        [NotNull]
        private static readonly Dictionary<int, CultureInfo> _cultureLcids;

        /// <summary>
        ///   The invariant culture LCID.
        /// </summary>
        /// <seealso cref="System.Globalization.CultureInfo.InvariantCulture"/>
        public static readonly int InvariantLCID;

        private static readonly NumberFormatInfo _symbolessCurrencyFormatInfo;

        /// <summary>
        ///   Gets the cultures (both specific and neutral) as well as the currency and
        ///   <see cref="System.Globalization.RegionInfo"/>.
        /// </summary>
        /// <seealso cref="Globalization.CurrencyInfo"/>
        static CultureHelper()
        {
            CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            int length = allCultures.Length;

            _currencyCultureInfo = new Dictionary<string, Dictionary<CultureInfo, RegionInfo>>(
                length,
                StringComparer.InvariantCultureIgnoreCase);
            _regionIsoNames = new Dictionary<string, RegionInfo>(length * 2, StringComparer.InvariantCultureIgnoreCase);
            _cultureIsoNames = new Dictionary<string, CultureInfo>(length * 2, StringComparer.InvariantCultureIgnoreCase);
            _cultureNames = new Dictionary<string, CultureInfo>(length, StringComparer.InvariantCultureIgnoreCase);
            _regionNames = new Dictionary<string, RegionInfo>(length, StringComparer.InvariantCultureIgnoreCase);
            _cultureLcids = new Dictionary<int, CultureInfo>(length);
            _regionLcids = new Dictionary<int, RegionInfo>(length);

            InvariantLCID = CultureInfo.InvariantCulture.LCID;

            foreach (CultureInfo ci in allCultures
                .Select(CultureInfo.ReadOnly)
                .OrderBy(c => c.Name.Length)
                .ThenBy(c => c.Name, StringComparer.InvariantCultureIgnoreCase))
            {
                Debug.Assert(ci != null);

                if (!_cultureLcids.ContainsKey(ci.LCID))
                    _cultureLcids.Add(ci.LCID, ci);

                if (!_cultureNames.ContainsKey(ci.Name))
                    _cultureNames.Add(ci.Name, ci);

                if (!_cultureIsoNames.ContainsKey(ci.TwoLetterISOLanguageName))
                    _cultureIsoNames.Add(ci.TwoLetterISOLanguageName, ci);

                if (!_cultureIsoNames.ContainsKey(ci.ThreeLetterISOLanguageName))
                    _cultureIsoNames.Add(ci.ThreeLetterISOLanguageName, ci);

                // We are not interested in neutral cultures, since
                // currency and RegionInfo is only applicable to specific cultures
                if ((ci.CultureTypes & CultureTypes.SpecificCultures) != CultureTypes.SpecificCultures ||
                    ci.IsInvariant())
                    continue;

                // Create a RegionInfo from culture id. 
                // RegionInfo holds the currency ISO code
                RegionInfo ri = ci.RegionInfo();

                // multiple cultures can have the same currency code
                Dictionary<CultureInfo, RegionInfo> cdict;
                if (!_currencyCultureInfo.TryGetValue(ri.ISOCurrencySymbol, out cdict))
                {
                    cdict = new Dictionary<CultureInfo, RegionInfo>();
                    _currencyCultureInfo.Add(ri.ISOCurrencySymbol, cdict);
                }
                Debug.Assert(cdict != null);
                cdict.Add(ci, ri);

                if (!_regionNames.ContainsKey(ri.EnglishName))
                    _regionNames.Add(ri.EnglishName, ri);
                
                if (!_regionIsoNames.ContainsKey(ri.TwoLetterISORegionName))
                    _regionIsoNames.Add(ri.TwoLetterISORegionName, ri);

                if (!_regionIsoNames.ContainsKey(ri.ThreeLetterISORegionName))
                    _regionIsoNames.Add(ri.ThreeLetterISORegionName, ri);

                if ((ci.CultureTypes & CultureTypes.UserCustomCulture) == 0 && !_regionLcids.ContainsKey(ci.LCID))
                    _regionLcids.Add(ci.LCID, ri);
            }

            _symbolessCurrencyFormatInfo = CultureInfo.CreateSpecificCulture("en-GB").NumberFormat;
            _symbolessCurrencyFormatInfo.CurrencySymbol = string.Empty;
            _symbolessCurrencyFormatInfo.CurrencyDecimalDigits = 2;
            _symbolessCurrencyFormatInfo.CurrencyDecimalSeparator = ".";
            _symbolessCurrencyFormatInfo.CurrencyGroupSeparator = ",";
        }

        /// <summary>
        ///   Gets the culture names (specific and neutral cultures).
        /// </summary>
        /// <remarks>
        ///   This is particularly useful when looking for culture specific directories (e.g. for resource files).
        /// </remarks>
        [NotNull]
        public static IEnumerable<string> CultureNames => _cultureNames.Keys;

        /// <summary>
        ///   Gets the region names.
        /// </summary>
        /// <remarks>
        ///   This is particularly useful when looking for culture specific directories (e.g. for resource files).
        /// </remarks>
        [NotNull]
        public static IEnumerable<string> RegionNames => _regionNames.Keys;

        /// <summary>
        ///   Gets the region names.
        /// </summary>
        /// <remarks>
        ///   This is particularly useful when looking for culture specific directories (e.g. for resource files).
        /// </remarks>
        [NotNull]
        public static IEnumerable<string> CurrencyNames => _currencyCultureInfo.Keys;

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> with the two or three letter ISO language name given.
        /// </summary>
        /// <param name="iso">The ISO name of the culture to find.</param>
        /// <returns>
        ///   The <see cref="System.Globalization.RegionInfo"/> that corresponds to the <paramref name="iso"/> name specified.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">iso</exception>
        public static CultureInfo GetCultureInfoIsoName(string iso)
        {
            if (iso == null) throw new ArgumentNullException(nameof(iso));
            return _cultureIsoNames.TryGetValue(iso, out CultureInfo cultureInfo) ? cultureInfo : null;
        }

        /// <summary>
        /// Tries to get the culture info with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><see langword="true" /> if found, otherwise <see langword="false" />.</returns>
        public static CultureInfo GetCultureInfo([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _cultureNames.TryGetValue(name, out CultureInfo cultureInfo) ? cultureInfo : null;
        }

        /// <summary>
        /// Tries to get the culture info with the specified LCID.
        /// </summary>
        /// <param name="lcid">The LCID.</param>
        /// <returns><see langword="true" /> if found, otherwise <see langword="false" />.</returns>
        public static CultureInfo GetCultureInfo(int lcid)
        {
            return _cultureLcids.TryGetValue(lcid, out CultureInfo cultureInfo) ? cultureInfo : null;
        }

        /// <summary>
        /// Gets the <see cref="RegionInfo"/> with the two or three letter ISO name given.
        /// </summary>
        /// <param name="iso">The ISO name of the region to find.</param>
        /// <returns>
        ///   The <see cref="System.Globalization.RegionInfo"/> that corresponds to the <paramref name="iso"/> name specified.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">iso</exception>
        public static RegionInfo GetRegionFromIsoName(string iso)
        {
            if (iso == null) throw new ArgumentNullException(nameof(iso));
            return _regionIsoNames.TryGetValue(iso, out RegionInfo regionInfo) ? regionInfo : null;
        }

        /// <summary>
        /// Tries to get the culture info with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><see langword="true" /> if found, otherwise <see langword="false" />.</returns>
        public static RegionInfo GetRegionInfo([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _regionNames.TryGetValue(name, out RegionInfo regionInfo) ? regionInfo : null;
        }

        /// <summary>
        /// Tries to get the region info with the specified LCID.
        /// </summary>
        /// <param name="lcid">The LCID.</param>
        /// <returns><see langword="true" /> if found, otherwise <see langword="false" />.</returns>
        public static RegionInfo GetRegionInfo(int lcid)
        {
            return _regionLcids.TryGetValue(lcid, out RegionInfo regionInfo) ? regionInfo : null;
        }

        /// <summary>
        ///   Gets the <see cref="System.Globalization.RegionInfo"/> for the specified
        ///   <see cref="CultureInfo">culture</see>.
        /// </summary>
        /// <param name="cultureInfo">The specific culture information.</param>
        /// <returns>
        ///   The corresponding <see cref="System.Globalization.RegionInfo"/> for the
        ///   <paramref name="cultureInfo"/> specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="cultureInfo"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static RegionInfo RegionInfo([NotNull] this CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo), Resources.CultureHelper_CultureInfoCannotBeNull);

            return new RegionInfo(cultureInfo.LCID);
        }

        /// <summary>
        ///   Gets the currency info for the specified region (if any).
        /// </summary>
        /// <param name="regionInfo">The region information.</param>
        /// <returns>
        ///   The corresponding currency info for the <paramref name="regionInfo"/> specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="regionInfo"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Globalization.CurrencyInfo"/>
        [CanBeNull]
        public static CurrencyInfo CurrencyInfo([NotNull] this RegionInfo regionInfo)
        {
            if (regionInfo == null)
                throw new ArgumentNullException(nameof(regionInfo), Resources.CultureHelper_RegionInfoCannotBeNull);

            return CurrencyInfoProvider.Current.Get(regionInfo.ISOCurrencySymbol);
        }

        /// <summary>
        ///   Gets the <see cref="Globalization.CurrencyInfo">currency info</see> for the specified culture (if any).
        /// </summary>
        /// <param name="cultureInfo">The specific culture information.</param>
        /// <returns>
        ///   The corresponding currency info for the <paramref name="cultureInfo"/> specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="cultureInfo"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Globalization.CurrencyInfo"/>
        [CanBeNull]
        public static CurrencyInfo CurrencyInfo([NotNull] this CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo), Resources.CultureHelper_CultureInfoCannotBeNull);

            return CurrencyInfoProvider.Current.Get(cultureInfo);
        }

        /// <summary>
        ///   Lookup a <see cref="CultureInfo"/> by the specified currency ISO code.
        /// </summary>
        /// <param name="isoCode">The ISO Code.</param>
        /// <returns>
        ///   A list of <see cref="CultureInfo"/>s that have the specified currency <paramref name="isoCode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="isoCode"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<CultureInfo> CultureInfoFromCurrencyISO([NotNull] string isoCode)
        {
            if (isoCode == null)
                throw new ArgumentNullException(nameof(isoCode), Resources.CultureHelper_RegionInfoCannotBeNull);

            if (string.IsNullOrEmpty(isoCode))
                return Enumerable.Empty<CultureInfo>();

            return _currencyCultureInfo.TryGetValue(isoCode, out Dictionary<CultureInfo, RegionInfo> dict)
                // ReSharper disable once PossibleNullReferenceException
                ? new List<CultureInfo>(dict.Keys.Distinct())
                : Enumerable.Empty<CultureInfo>();
        }

        /// <summary>
        ///   Lookup <see cref="System.Globalization.RegionInfo">region information</see> by the currency ISO code.
        /// </summary>
        /// <param name="isoCode">The ISO Code.</param>
        /// <returns>
        ///   A list of <see cref="System.Globalization.RegionInfo"/>s that have the specified currency.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="isoCode"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static IEnumerable<RegionInfo> RegionInfoFromCurrencyISO([NotNull] string isoCode)
        {
            if (isoCode == null)
                throw new ArgumentNullException(nameof(isoCode), Resources.CultureHelper_IsoCodeCannotBeNull);

            if (string.IsNullOrEmpty(isoCode))
                return Enumerable.Empty<RegionInfo>();

            return _currencyCultureInfo.TryGetValue(isoCode, out Dictionary<CultureInfo, RegionInfo> dict)
                // ReSharper disable once PossibleNullReferenceException
                ? new List<RegionInfo>(dict.Values.Distinct())
                : Enumerable.Empty<RegionInfo>();
        }

        /// <summary>
        ///   Format a <see cref="decimal"/> value to a <see cref="string"/> using the currency format specified.
        ///   If the specified currency ISO Code doesn't match a <see cref="CultureInfo">culture</see> then there
        ///   will be no currency symbol and the <paramref name="currencyISO"/> Code will be the prefix.
        /// </summary>
        /// <param name="amount">The numerical amount to format.</param>
        /// <param name="currencyISO">The currency ISO Code.</param>
        /// <param name="countryISO">The country ISO Code.</param>
        /// <returns>
        ///   A formatted <see cref="string"/> in the correct currency format.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="currencyISO"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="CultureInfo"/>
        /// <seealso cref="System.Globalization.RegionInfo"/>
        [NotNull]
        public static string FormatCurrency(
            decimal amount,
            [NotNull] string currencyISO,
            [CanBeNull] string countryISO = null)
        {
            if (currencyISO == null)
                throw new ArgumentNullException(nameof(currencyISO), Resources.CultureHelper_CurrencyIsoCannotBeNull);

            CultureInfo[] cultures = null;

            if (!string.IsNullOrEmpty(currencyISO))
                cultures = CultureInfoFromCurrencyISO(currencyISO).ToArray();

            if ((cultures != null) &&
                (cultures.Length > 0))
            {
                CultureInfo cinfo = cultures[0];
                if (countryISO != null)
                    // Find best match
                    for (int i = cultures.Length - 1; i >= 0; i--)
                    {
                        cinfo = cultures[i];
                        if (cinfo == null) continue;
                        RegionInfo r = new RegionInfo(cinfo.LCID);
                        if (r.TwoLetterISORegionName.Equals(countryISO))
                            break;
                    }

                return FormatCurrency(amount, cinfo);
            }

            // If currency ISO code doesn't match any culture
            // use the ISO code as a prefix (e.g. YEN 123,123.00)
            return currencyISO + " " + amount.ToString("C", _symbolessCurrencyFormatInfo);
        }

        /// <summary>
        ///   Format a <see cref="decimal"/> value to a <see cref="string"/> using the specified <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="amount">The numerical amount to format.</param>
        /// <param name="cultureInfo">
        ///   <para>The culture info.</para>
        ///   <para>If this is a null value then the thread's <see cref="System.Threading.Thread.CurrentUICulture">current
        ///   UI culture</see> is used.</para>
        /// </param>
        /// <returns>
        ///   A formatted <see cref="string"/> in the correct currency format.
        /// </returns>
        [NotNull]
        public static string FormatCurrency(decimal amount, [CanBeNull] CultureInfo cultureInfo = null)
        {
            cultureInfo = cultureInfo ?? Thread.CurrentThread.CurrentUICulture;
            return amount.ToString("C", cultureInfo.NumberFormat);
        }

        /// <summary>
        ///   Retrieves the <see cref="System.Globalization.RegionInfo"/> using the
        ///   <see cref="System.Globalization.RegionInfo.DisplayName">display name</see> specified.
        /// </summary>
        ///   <param name="name">The display name (the country's name in the current culture).</param>
        /// <returns>
        ///   The <see cref="System.Globalization.RegionInfo"/> that corresponds to the <paramref name="name"/> specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="CultureInfo"/>
        /// <seealso cref="System.Globalization.RegionInfo.DisplayName"/>
        [CanBeNull]
        public static RegionInfo FindRegionFromName([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name), Resources.CultureHelper_NameCannotBeNull);

            if (name.Length < 1)
                return null;

            RegionInfo region;
            if (_regionNames.TryGetValue(name, out region))
                return region;

            // Check if we're in English, if we are we've finished looking.
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            // Scan regions for name in current culture.
            return culture.TwoLetterISOLanguageName.Equals("en")
                ? null
                : _regionNames.Values.FirstOrDefault(
                    // ReSharper disable PossibleNullReferenceException
                    info => (info.DisplayName.Equals(name)) || (info.NativeName.Equals(name)));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        ///   Retrieves the <see cref="System.Globalization.RegionInfo"/> using the display name specified.
        /// </summary>
        /// <param name="name">The display name (the country's name in the current culture).</param>
        /// <returns>
        ///   The <see cref="System.Globalization.RegionInfo"/> that corresponds to the <paramref name="name"/> specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        [CanBeNull]
        public static RegionInfo FindRegion([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name), Resources.CultureHelper_NameCannotBeNull);

            if (name.Length < 1)
                return null;

            RegionInfo region;
            try
            {
                region = new RegionInfo(name);
            }
            catch (ArgumentException)
            {
                region = null;
            }
            return region ?? FindRegionFromName(name);
        }

        /// <summary>
        ///   Determines whether or not the <see cref="CultureInfo"/> is the invariant culture.
        ///   (http://msdn.microsoft.com/en-us/library/4c5zdc6a.aspx)
        /// </summary>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the specified <paramref name="cultureInfo"/> is invariant; otherwise returns <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///   The invariant culture is culture-insensitive, it is useful for when culture-specific presentation isn't required/needed.
        /// </remarks>
        /// <seealso cref="System.Globalization.CultureInfo.InvariantCulture"/>
        public static bool IsInvariant(this CultureInfo cultureInfo) => cultureInfo?.LCID == InvariantLCID;

        /// <summary>
        /// Gets the child cultures of the specified culture.
        /// </summary>
        /// <param name="culture">The culture to get the children of.</param>
        /// <returns>The child cultures of the specified culture.</returns>
        [NotNull]
        public static IEnumerable<ExtendedCultureInfo> GetChildren([NotNull] this CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return CultureInfoProvider.Current.GetChildren(culture);
        }

        /// <summary>
        /// Gets the fall back cultures for the specified culture, in order of preference, from the <see cref="CultureInfoProvider.Current"/> provider.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// The fall back cultures for the specified culture, in order of preference. The first element will always be the given culture.
        /// If the invariant culture is given, all cultures will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="culture"/> was null.</exception>
        /// <exception cref="ArgumentException">$The <see cref="CultureInfoProvider.Current"/> provider does not contain the <paramref name="culture"/> given.</exception>
        [NotNull]
        public static IEnumerable<ExtendedCultureInfo> GetFallBack([NotNull] this CultureInfo culture)
            => GetFallBack(CultureInfoProvider.Current, culture);

        /// <summary>
        /// Gets the fall back cultures for the specified culture, in order of preference, from the specified <see cref="ICultureInfoProvider"/>.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// The fall back cultures for the specified culture, in order of preference. The first element will always be the given culture.
        /// If the invariant culture is given, all cultures will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> was null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="culture"/> was null.</exception>
        /// <exception cref="ArgumentException">$The <paramref name="provider"/> does not contain the <paramref name="culture"/> given.</exception>
        [NotNull]
        public static IEnumerable<ExtendedCultureInfo> GetFallBack(
            [NotNull] this ICultureInfoProvider provider,
            [NotNull] CultureInfo culture)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            ExtendedCultureInfo extendedCultureInfo = provider.Get(culture);
            if (extendedCultureInfo == null)
                throw new ArgumentException(
                    string.Format(Resources.CultureHelper_GetFallBack_UnknownCulture, culture),
                    nameof(culture));

            HashSet<ExtendedCultureInfo> yielded = new HashSet<ExtendedCultureInfo>();

            // First return the culture passed in
            yield return extendedCultureInfo;
            yielded.Add(extendedCultureInfo);

            // Next return any descendants in a breadth first order 
            Queue<ExtendedCultureInfo> queue = new Queue<ExtendedCultureInfo>(provider.GetChildren(extendedCultureInfo));
            while (queue.TryDequeue(out ExtendedCultureInfo childCulture))
            {
                yield return childCulture;

                foreach (ExtendedCultureInfo child in provider.GetChildren(childCulture))
                    queue.Enqueue(child);
            }

            // Lastly return the ancestors and each ones children, excluding invariant
            extendedCultureInfo = extendedCultureInfo.Parent;

            while (!extendedCultureInfo.IsInvariant)
            {
                yield return extendedCultureInfo;
                yielded.Add(extendedCultureInfo);

                foreach (ExtendedCultureInfo child in provider.GetChildren(extendedCultureInfo))
                {
                    if (!yielded.Contains(child))
                        yield return child;
                }

                extendedCultureInfo = extendedCultureInfo.Parent;
            }

            if (!yielded.Contains(extendedCultureInfo))
                yield return extendedCultureInfo;
        }
    }
}