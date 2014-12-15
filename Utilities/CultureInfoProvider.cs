using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities
{
    /// <summary>
    /// Provides <see cref="CultureInfo"/>.
    /// </summary>
    public class CultureInfoProvider : ICultureInfoProvider
    {
        /// <summary>
        /// A <see cref="ICurrencyInfoProvider" /> with no currencies!
        /// </summary>
        private class EmptyCultureInfoProvider : ICultureInfoProvider
        {
            private readonly DateTime _published;

            /// <summary>
            /// Initializes a new instance of the <see cref="EmptyCultureInfoProvider"/> class.
            /// </summary>
            /// <param name="published">The published.</param>
            public EmptyCultureInfoProvider(DateTime published)
            {
                _published = published;
            }

            /// <summary>
            /// The date this file was published.
            /// </summary>
            public DateTime Published { get { return _published; } }

            /// <summary>
            /// The cultures in the provider.
            /// </summary>
            public IEnumerable<ExtendedCultureInfo> All
            {
                get { return Enumerable.Empty<ExtendedCultureInfo>(); }
            }

            /// <summary>
            /// Gets the number of cultures specified in the provider.
            /// </summary>
            /// <value>
            /// The count.
            /// </value>
            public int Count
            {
                get { return 0; }
            }

            /// <summary>
            /// Retrieves an <see cref="ExtendedCultureInfo" /> with the ISO Code specified.
            /// </summary>
            /// <param name="cultureName">The ISO Code.</param>
            /// <returns>
            /// The 
            /// <see cref="CultureInfo" /> that corresponds to the 
            /// <paramref name="cultureName" /> specified (if any);
            ///   otherwise the default value for the type is returned.
            /// </returns>
            /// <remarks>
            ///   There is a 
            /// <see cref="System.Diagnostics.Contracts.Contract">contract</see> for this method,
            ///   
            /// <paramref name="cultureName" /> cannot be null.
            /// </remarks>
            public ExtendedCultureInfo Get(string cultureName)
            {
                return null;
            }

            /// <summary>
            /// Retrieves an <see cref="ExtendedCultureInfo" /> equivalent to the specified (see <see cref="CultureInfo" />).
            /// </summary>
            /// <param name="cultureInfo">The culture info.</param>
            /// <returns>
            /// The 
            /// <see cref="CultureInfo" /> that corresponds to the 
            /// <paramref name="cultureInfo" /> specified (if any);
            ///   otherwise the default value for the type is returned.
            /// </returns>
            /// <remarks>
            /// There is a 
            /// <see cref="System.Diagnostics.Contracts.Contract">contract</see> for this method,
            ///   
            /// <paramref name="cultureInfo" /> cannot be null.
            /// </remarks>
            public ExtendedCultureInfo Get(CultureInfo cultureInfo)
            {
                return null;
            }

            /// <summary>
            /// Finds the cultures that use a specific currency.
            /// </summary>
            /// <param name="currencyInfo">The currency information.</param>
            /// <returns>
            /// The cultures that us the specified currency.
            /// </returns>
            public IEnumerable<ExtendedCultureInfo> FindByCurrency(CurrencyInfo currencyInfo)
            {
                return Enumerable.Empty<ExtendedCultureInfo>();
            }
        }
        
        /// <summary>
        /// The empty culture info provider has no cultures.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static readonly ICultureInfoProvider Empty;

        /// <summary>
        /// The provider that supplies the BCL <see cref="CultureInfo">cultures</see>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static readonly BclCultureInfoProvider Bcl;

        /// <summary>
        /// The current provider.
        /// </summary>
        [NotNull]
        private static ICultureInfoProvider _current;

        /// <summary>
        /// Gets or sets the current provider.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        [PublicAPI]
        [NotNull]
        public static ICultureInfoProvider Current
        {
            get { return _current; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _current = value;
            }
        }

        /// <summary>
        /// Initializes the <see cref="CultureInfoProvider"/> class.
        /// </summary>
        static CultureInfoProvider()
        {
            Empty = new EmptyCultureInfoProvider(DateTime.MinValue);

            // Create BCL provider
            // Note we must update the DateTime whenever we build against a new framework.
            _current = Bcl =  new BclCultureInfoProvider();

#if false
            string path = Configuration.UtilityConfiguration.Active.ISO4217;
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (!File.Exists(path))
                throw new FileNotFoundException(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    string.Format(Resources.CurrencyInfoProvider_CurrencyInfoProvider_FileNotFound, path));

            try
            {
                CurrencyInfoProvider currencyInfoProvider = LoadFromFile(path);

                if (currencyInfoProvider == null)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    throw new InvalidDataException(string.Format(Resources.CurrencyInfoProvider_CurrencyInfoProvider_DataInvalid, path));

                _current = currencyInfoProvider;
            }
            catch (Exception e)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                throw new FileLoadException(
                    string.Format(Resources.CurrencyInfoProvider_CurrencyInfoProvider_LoadError, path),
                    e);
            }
#endif
        }

        private readonly DateTime _published;

        /// <summary>
        ///   Stores currency info (by code).
        /// </summary>
        [NotNull]
        private readonly IReadOnlyDictionary<string, ExtendedCultureInfo> _cultureInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInfoProvider" /> class.
        /// </summary>
        /// <param name="published">The published date time.</param>
        /// <param name="cultures">The cultures.</param>
        [PublicAPI]
        public CultureInfoProvider(DateTime published, [NotNull][ItemNotNull] IEnumerable<ExtendedCultureInfo> cultures)
        {
            _published = published;
            _cultureInfos = cultures.Distinct().ToDictionary(c => c.Name, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// The date this provider was published.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public DateTime Published
        {
            get { return _published; }
        }

        /// <summary>
        /// The cultures in the provider.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<ExtendedCultureInfo> All
        {
            get { return _cultureInfos.Values; }
        }

        /// <summary>
        /// Gets the number of cultures specified in the provider.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Count
        {
            get { return _cultureInfos.Count; }
        }

        /// <summary>
        /// Retrieves an <see cref="ExtendedCultureInfo" /> with the name specified (see <see cref="CultureInfo.Name"/>).
        /// </summary>
        /// <param name="cultureName">The ISO Code.</param>
        /// <returns>
        /// The 
        /// <see cref="CultureInfo" /> that corresponds to the 
        /// <paramref name="cultureName" /> specified (if any);
        ///   otherwise the default value for the type is returned.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// There is a 
        /// <see cref="System.Diagnostics.Contracts.Contract">contract</see> for this method,
        ///   
        /// <paramref name="cultureName" /> cannot be null.
        /// </remarks>
        public ExtendedCultureInfo Get(string cultureName)
        {
#if !BUILD_TASKS
            Contract.Requires(cultureName != null, Resources.CultureInfoProvider_CultureNameCannotBeNull);
#endif

            ExtendedCultureInfo cultureInfo;
            _cultureInfos.TryGetValue(cultureName, out cultureInfo);
            return cultureInfo;
        }

        /// <summary>
        /// Retrieves an <see cref="ExtendedCultureInfo" /> equivalent to the specified (see <see cref="CultureInfo" />).
        /// </summary>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>
        /// The 
        /// <see cref="CultureInfo" /> that corresponds to the 
        /// <paramref name="cultureInfo" /> specified (if any);
        ///   otherwise the default value for the type is returned.
        /// </returns>
        /// <remarks>
        /// There is a 
        /// <see cref="System.Diagnostics.Contracts.Contract">contract</see> for this method,
        ///   
        /// <paramref name="cultureInfo" /> cannot be null.
        /// </remarks>
        public ExtendedCultureInfo Get(CultureInfo cultureInfo)
        {
            Contract.Requires(cultureInfo != null, Resources.CultureHelper_CultureInfoCannotBeNull);

            ExtendedCultureInfo eci;
            _cultureInfos.TryGetValue(cultureInfo.Name, out eci);
            return eci;
        }

        public IEnumerable<ExtendedCultureInfo> FindByCurrency(CurrencyInfo currencyInfo)
        {
            throw new NotImplementedException();
        }
    }
}