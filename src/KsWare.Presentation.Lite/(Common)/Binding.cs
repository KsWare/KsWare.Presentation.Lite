// Wrapper for System.Windows.Data.Binding
// uses public interface of:
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: F8ACF359-3458-4950-A98E-4BE86054B5AD
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\3.1.0\PresentationFramework.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace KsWare.Presentation.Lite {

	/// <summary>
    ///  Describes an instance of a Binding, binding a target
    ///  (DependencyObject, DependencyProperty) to a source (object, property)
    /// </summary>
    public class Binding : BindingBase {

	    private readonly System.Windows.Data.Binding _binding;

        // //------------------------------------------------------
        // //
        // //  Dynamic properties and events
        // //
        // //------------------------------------------------------
        //
        // /// <summary>
        // /// The SourceUpdated event is raised whenever a value is transferred from the target to the source,
        // /// but only for Bindings that have requested the event by setting BindFlags.NotifyOnSourceUpdated.
        // /// </summary>
        // public static readonly RoutedEvent SourceUpdatedEvent =
        //         EventManager.RegisterRoutedEvent("SourceUpdated",
        //                                 RoutingStrategy.Bubble,
        //                                 typeof(EventHandler<DataTransferEventArgs>),
        //                                 typeof(Binding));
        //
        // /// <summary>
        // ///     Adds a handler for the SourceUpdated attached event
        // /// </summary>
        // /// <param name="element">UIElement or ContentElement that listens to this event</param>
        // /// <param name="handler">Event Handler to be added</param>
        // public static void AddSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) {
        //     FrameworkElement.AddHandler(element, SourceUpdatedEvent, handler);
        // }
        //
        // /// <summary>
        // ///     Removes a handler for the SourceUpdated attached event
        // /// </summary>
        // /// <param name="element">UIElement or ContentElement that listens to this event</param>
        // /// <param name="handler">Event Handler to be removed</param>
        // public static void RemoveSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) {
        //     FrameworkElement.RemoveHandler(element, SourceUpdatedEvent, handler);
        // }
        //
        // /// <summary>
        // /// The TargetUpdated event is raised whenever a value is transferred from the source to the target,
        // /// but only for Bindings that have requested the event by setting BindFlags.NotifyOnTargetUpdated.
        // /// </summary>
        // public static readonly RoutedEvent TargetUpdatedEvent =
        //         EventManager.RegisterRoutedEvent("TargetUpdated",
        //                                 RoutingStrategy.Bubble,
        //                                 typeof(EventHandler<DataTransferEventArgs>),
        //                                 typeof(Binding));
        //
        // /// <summary>
        // ///     Adds a handler for the TargetUpdated attached event
        // /// </summary>
        // /// <param name="element">UIElement or ContentElement that listens to this event</param>
        // /// <param name="handler">Event Handler to be added</param>
        // public static void AddTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) 
	       //  => System.Windows.Data.Binding.AddTargetUpdatedHandler(element, handler);
        //
        // /// <summary>
        // ///     Removes a handler for the TargetUpdated attached event
        // /// </summary>
        // /// <param name="element">UIElement or ContentElement that listens to this event</param>
        // /// <param name="handler">Event Handler to be removed</param>
        // public static void RemoveTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler) 
	       //  => System.Windows.Data.Binding.RemoveTargetUpdatedHandler(element, handler);
        


//         // PreSharp uses message numbers that the C# compiler doesn't know about.
//         // Disable the C# complaints, per the PreSharp documentation.
// #pragma warning disable 1634, 1691
//
//         // PreSharp checks that the type of the DP agrees with the type of the static
//         // accessors.  But setting the type of the DP to XmlNamespaceManager would
//         // load System.Xml during the static cctor, which is considered a perf bug.
//         // So instead we set the type of the DP to 'object' and use the
//         // ValidateValueCallback to ensure that only values of the right type are allowed.
//         // Meanwhile, disable the PreSharp warning
// #pragma warning disable 7008
//
//         /// <summary>
//         /// The XmlNamespaceManager to use to perform Namespace aware XPath queries in XmlData bindings
//         /// </summary>
//         public static readonly DependencyProperty XmlNamespaceManagerProperty =
//                 DependencyProperty.RegisterAttached("XmlNamespaceManager", typeof(object), typeof(Binding),
//                                             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits),
//                                             new ValidateValueCallback(IsValidXmlNamespaceManager));
//
//         /// <summary> Static accessor for XmlNamespaceManager property </summary>
//         /// <exception cref="ArgumentNullException"> DependencyObject target cannot be null </exception>
//         public static XmlNamespaceManager GetXmlNamespaceManager(DependencyObject target) {
//             if (target == null)
//                 throw new ArgumentNullException("target");
//
//             return (XmlNamespaceManager)target.GetValue(XmlNamespaceManagerProperty);
//         }

        /// <summary> Static modifier for XmlNamespaceManager property </summary>
        /// <exception cref="ArgumentNullException"> DependencyObject target cannot be null </exception>
        public static void SetXmlNamespaceManager(DependencyObject target, XmlNamespaceManager value) 
			=> System.Windows.Data.Binding.SetXmlNamespaceManager(target, value);



#pragma warning restore 7008
#pragma warning restore 1634, 1691


        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Binding() {
	        _bindingBase = _binding = new System.Windows.Data.Binding();

        }

        /// <summary>
        /// Convenience constructor.  Sets most fields to default values.
        /// </summary>
        /// <param name="path">source path </param>
        public Binding(string path) {
	        _bindingBase = _binding = new System.Windows.Data.Binding(path);
        }


        /// <summary>
        ///     Collection&lt;ValidationRule&gt; is a collection of ValidationRule
        ///     implementations on either a Binding or a MultiBinding.  Each of the rules
        ///     is run by the binding engine when validation on update to source
        /// </summary>
        public Collection<ValidationRule> ValidationRules  => _binding.ValidationRules;

        /// <summary>
        /// This method is used by TypeDescriptor to determine if this property should
        /// be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeValidationRules() => _binding.ShouldSerializeValidationRules();

        /// <summary> True if an exception during source updates should be considered a validation error.</summary>
        [DefaultValue(false)]
        public bool ValidatesOnExceptions {
	        get => _binding.ValidatesOnExceptions;
	        set => _binding.ValidatesOnExceptions = value;
        }

        /// <summary> True if a data error in the source item should be considered a validation error.</summary>
        [DefaultValue(false)]
        public bool ValidatesOnDataErrors {
	        get => _binding.ValidatesOnDataErrors;
	        set => _binding.ValidatesOnDataErrors = value;
        }

        /// <summary> True if a data error from INotifyDataErrorInfo source item should be considered a validation error.</summary>
        [DefaultValue(true)]
        public bool ValidatesOnNotifyDataErrors {
	        get => _binding.ValidatesOnNotifyDataErrors;
	        set => _binding.ValidatesOnNotifyDataErrors = value;
        }


        /// <summary> The source path (for CLR bindings).</summary>
        public PropertyPath Path {
	        get => _binding.Path;
	        set => _binding.Path = value;
        }

        /// <summary>
        /// This method is used by TypeDescriptor to determine if this property should
        /// be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePath() => _binding.ShouldSerializePath();

        /// <summary> The XPath path (for XML bindings).</summary>
        [DefaultValue(null)]
        public string XPath {
	        get => _binding.XPath;
	        set => _binding.XPath = value;
        }

        /// <summary> Binding mode </summary>
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode {
	        get => _binding.Mode;
	        set => _binding.Mode = value;
        }

        /// <summary> Update type </summary>
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger {
	        get => _binding.UpdateSourceTrigger;
	        set => _binding.UpdateSourceTrigger = value;
        }

        /// <summary> Raise SourceUpdated event whenever a value flows from target to source </summary>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated {
	        get => _binding.NotifyOnSourceUpdated;
	        set => _binding.NotifyOnSourceUpdated = value;
        }


        /// <summary> Raise TargetUpdated event whenever a value flows from source to target </summary>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated {
	        get => _binding.NotifyOnTargetUpdated;
	        set => _binding.NotifyOnTargetUpdated = value;
        }

        /// <summary> Raise ValidationError event whenever there is a ValidationError on Update</summary>
        [DefaultValue(false)]
        public bool NotifyOnValidationError {
	        get => _binding.NotifyOnValidationError;
	        set => _binding.NotifyOnValidationError = value;
        }

        /// <summary> The Converter to apply </summary>
        [DefaultValue(null)]
        public IValueConverter Converter {
	        get => _binding.Converter;
	        set => _binding.Converter = value;
        }

        /// <summary>
        /// The parameter to pass to converter.
        /// </summary>
        /// <value></value>
        [DefaultValue(null)]
        public object ConverterParameter {
	        get => _binding.ConverterParameter;
	        set => _binding.ConverterParameter = value;
        }

        /// <summary> Culture in which to evaluate the converter </summary>
        [DefaultValue(null)]
        [TypeConverter(typeof(System.Windows.CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture {
	        get => _binding.ConverterCulture;
	        set => _binding.ConverterCulture = value;
        }

        /// <summary> object to use as the source </summary>
        /// <remarks> To clear this property, set it to DependencyProperty.UnsetValue. </remarks>
        public object Source {
	        get => _binding.Source;
	        set => _binding.Source = value;
        }

        /// <summary>
        /// This method is used by TypeDescriptor to determine if this property should
        /// be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSource() => _binding.ShouldSerializeSource();

        /// <summary>
        /// Description of the object to use as the source, relative to the target element.
        /// </summary>
        [DefaultValue(null)]
        public RelativeSource RelativeSource {
	        get => _binding.RelativeSource;
	        set => _binding.RelativeSource = value;
        }

        /// <summary> Name of the element to use as the source </summary>
        [DefaultValue(null)]
        public string ElementName {
	        get => _binding.ElementName;
	        set => _binding.ElementName = value;
        }

        /// <summary> True if Binding should get/set values asynchronously </summary>
        [DefaultValue(false)]
        public bool IsAsync {
	        get => _binding.IsAsync;
	        set => _binding.IsAsync = value;
        }

        /// <summary> Opaque data passed to the asynchronous data dispatcher </summary>
        [DefaultValue(null)]
        public object AsyncState {
	        get => _binding.AsyncState;
	        set => _binding.AsyncState = value;
        }

        /// <summary> True if Binding should interpret its path relative to
        /// the data item itself.
        /// </summary>
        /// <remarks>
        /// The normal behavior (when this property is false)
        /// includes special treatment for a data item that implements IDataSource.
        /// In this case, the path is treated relative to the object obtained
        /// from the IDataSource.Data property.  In addition, the binding listens
        /// for the IDataSource.DataChanged event and reacts accordingly.
        /// Setting this property to true overrides this behavior and gives
        /// the binding access to properties on the data source object itself.
        /// </remarks>
        [DefaultValue(false)]
        public bool BindsDirectlyToSource {
	        get => _binding.BindsDirectlyToSource;
	        set => _binding.BindsDirectlyToSource = value;
        }

        /// <summary>
        /// called whenever any exception is encountered when trying to update
        /// the value to the source. The application author can provide its own
        /// handler for handling exceptions here. If the delegate returns
        ///     null - don't throw an error or provide a ValidationError.
        ///     Exception - returns the exception itself, we will fire the exception using Async exception model.
        ///     ValidationError - it will set itself as the BindingInError and add it to the element's Validation errors.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter {
	        get => _binding.UpdateSourceExceptionFilter;
	        set => _binding.UpdateSourceExceptionFilter = value;
        }

        //------------------------------------------------------
        //
        //  Public Fields
        //
        //------------------------------------------------------

        /// <summary>
        ///     A source property or a converter can return Binding.DoNothing
        ///     to instruct the binding engine to do nothing (i.e. do not transfer
        ///     a value to the target, do not move to the next Binding in a
        ///     PriorityBinding, do not use the fallback or default value).
        /// </summary>
        public static readonly object DoNothing = System.Windows.Data.Binding.DoNothing;

        /// <summary>
        ///     This string is used as the PropertyName of the
        ///     PropertyChangedEventArgs to indicate that an indexer property
        ///     has been changed.
        /// </summary>
        public const string IndexerName = System.Windows.Data.Binding.IndexerName;

    }
}

