using System;
using System.Collections.Generic;

namespace Miracle.Settings
{
    /// <summary>
    /// Provides the basic functionality for a settings loader
    /// </summary>
    public interface ISettingsLoader
    {
        /// <summary>
        /// Create an object of type T initialized with settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix">Prefix of all settings</param>
        /// <returns>Initialized settings object</returns>
        T Create<T>(string prefix = null);

        /// <summary>
        /// Create array of type T from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the array</param>
        T[] CreateArray<T>(string prefix = null);

        /// <summary>
        /// Create array of type T from setting with key <param name="key"/> and split into string array using <param name="separator"/> and <param name="options"/>.
        /// </summary>
        /// <param name="key">The key of the setting containing separated values</param>
        /// <param name="separator">the separator(s) used to split string value</param>
        /// <param name="options">the option used to split string value</param>
        T[] CreateArray<T>(string key, char[] separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Create list from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the array</param>
        List<T> CreateList<T>(string prefix = null);

        /// <summary>
        /// Create list of type T from setting with key <param name="key"/> and split into string array using <param name="separator"/> and <param name="options"/>.
        /// </summary>
        /// <param name="key">The key of the setting containing separated values</param>
        /// <param name="separator">the separator(s) used to split string value</param>
        /// <param name="options">the option used to split string value</param>
        List<T> CreateList<T>(string key, char[] separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Create dictionary from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the dictionary</param>
        /// <param name="comparer">Optional dictionary key comparer</param>
        Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(string prefix = null, IEqualityComparer<TKey> comparer = null);

        /// <summary>
        /// Load settings prefixed by <param name="prefix"/> into existing object instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">Existing instance </param>
        /// <param name="prefix">Prefix of all settings</param>
        /// <returns>ISettingsLoader instance for fluid binding</returns>
        ISettingsLoader Load<T>(T instance, string prefix = null);
    }
}