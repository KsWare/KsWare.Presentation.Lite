using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace KsWare.Presentation.Lite {

    //TODO simple implementation of a shared resource dictionary. improved later.

    /// <summary>
    /// The shared resource dictionary is a specialized resource dictionary
    /// that loads it content only once. If a second instance with the same source
    /// is created, it only merges the resources from the cache.
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary {

	    private static readonly bool IsInDesignerMode;

        private Uri _sourceUri;

        static SharedResourceDictionary() {
            IsInDesignerMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
        }

        /// <summary>
        /// Gets the internal cache of loaded dictionaries.
        /// </summary>
        public static Dictionary<Uri, ResourceDictionary> SharedDictionaries { get; } = new Dictionary<Uri, ResourceDictionary>();

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source {
            get {
                return _sourceUri;
            }

            set {
                _sourceUri = value;

                // Always load the dictionary by default in designer mode.
                if (!SharedDictionaries.ContainsKey(value) || IsInDesignerMode) {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    // add it to the cache if we're not in designer mode
                    if (!IsInDesignerMode) {
                        SharedDictionaries.Add(value, this);
                    }
                }
                else {
                    // If the dictionary is already loaded, get it from the cache
                    MergedDictionaries.Add(SharedDictionaries[value]);
                }
            }
        }
    }
}
