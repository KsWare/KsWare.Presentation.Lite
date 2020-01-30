// Wrapper for System.Windows.Data.BindingBase
// uses public interface of:
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: F8ACF359-3458-4950-A98E-4BE86054B5AD
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\3.1.0\PresentationFramework.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Base class for Binding, PriorityBinding, and MultiBinding.
	/// </summary>
	[MarkupExtensionReturnType(typeof(object))]
	[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)] // Not localizable by-default
	public abstract class BindingBase : MarkupExtension {

		private protected System.Windows.Data.BindingBase _bindingBase;

		/// <summary> Initialize a new instance of BindingBase. </summary>
		/// <remarks>
		/// This constructor can only be called by one of the built-in
		/// derived classes.
		/// </remarks>
		internal BindingBase() {
		}

 
		//------------------------------------------------------
		//
		//  Public Properties
		//
		//------------------------------------------------------

		/// <summary> Value to use when source cannot provide a value </summary>
		/// <remarks>
		///     Initialized to DependencyProperty.UnsetValue; if FallbackValue is not set, BindingExpression
		///     will return target property's default when Binding cannot get a real value.
		/// </remarks>
		public object FallbackValue {
			get => _bindingBase.FallbackValue;
			set => _bindingBase.FallbackValue = value;
		}

		/// <summary>
		/// This method is used by TypeDescriptor to determine if this property should
		/// be serialized.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeFallbackValue() => _bindingBase.ShouldSerializeFallbackValue();

		/// <summary> Format string used to convert the data to type String.
		/// </summary>
		/// <remarks>
		///     This property is used when the target of the binding has type
		///     String and no Converter is declared.  It is ignored in all other
		///     cases.
		/// </remarks>
		[System.ComponentModel.DefaultValue(null)]
		public string StringFormat {
			get => _bindingBase.StringFormat;
			set => _bindingBase.StringFormat = value;
		}

		/// <summary> Value used to represent "null" in the target property.
		/// </summary>
		public object TargetNullValue {
			get => _bindingBase.TargetNullValue;
			set => _bindingBase.TargetNullValue = value;
		}

		/// <summary>
		/// This method is used by TypeDescriptor to determine if this property should
		/// be serialized.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeTargetNullValue() => _bindingBase.ShouldSerializeTargetNullValue();

		/// <summary> Name of the <see cref="BindingGroup"/> this binding should join.
		/// </summary>
		[DefaultValue("")]
		public string BindingGroupName {
			get => _bindingBase.BindingGroupName;
			set => _bindingBase.BindingGroupName = value;
		}

		/// <summary>
		/// The time (in milliseconds) to wait after the most recent property
		/// change before performing source update.
		/// </summary>
		/// <remarks>
		/// This property affects only TwoWay bindings with UpdateSourceTrigger=PropertyChanged.
		/// </remarks>
		[DefaultValue(0)]
		public int Delay {
			get => _bindingBase.Delay;
			set => _bindingBase.Delay = value;
		}
        
		//------------------------------------------------------
		//
		//  MarkupExtension overrides
		//
		//------------------------------------------------------

		/// <summary>
		/// Return the value to set on the property for the target for this
		/// binding.
		/// </summary>
		public override object ProvideValue(IServiceProvider serviceProvider) {
			// TODO collect debug info (see DebugHint extension)
			return _bindingBase.ProvideValue(serviceProvider);
		}
	}

}
