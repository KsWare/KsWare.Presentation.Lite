# KsWare.Presentation.Lite

- view model first abroach
- easy (codeless)  mapping between view and view model
- easy to implement
- modularly expandable

## Quickstart
- create [ShellViewModel.cs](###ShellViewModel.cs) or use your existing view model
- create [ShellView.xaml](###ShellView.xaml) or use your existing view
- create [Bootstrapper.cs](###Bootstrapper.cs)
- create/modify [App.xaml](###App.xaml)

You can easily reuse your existing MainWindow.xaml by renaming it to ShellView.xaml [[1]()]. 
Finally you should have a pair where the names ends with ..**View**.xaml and ..**ViewModel**.cs. [[2]()]. 
You don't need the code behind file ShellView.xaml.cs

[1] The name "ShellView" is not fix but is used as default. You can use any valid name. 
[2] The suffixes "View" and "ViewModel" could be changed with a custom ViewLocatorStrategy, but who wants to do unnecessary work.

## Content

- [NotifyPropertyChangedBase](##NotifyPropertyChangedBase)
- [BootstrapperBase](##BootstrapperBase)
- [ViewLocator](##ViewLocator)
- [ViewModelPresenter](##ViewModelPresenter)
- [ViewModelViewConverter](##ViewModelViewConverter)
- [IViewLocatorStrategy](##IViewLocatorStrategy)
- [(Debug)Binding](##(Debug)Binding)
- [DebugHint](##DebugHint)
- [ObjectDebugExtensions](##ObjectDebugExtensions)
- [ViewModelPresenterExtension](##ViewModelPresenterExtension)
- [Adorner](##Adorner)
- [RelayCommand](##RelayCommand) 
- [SharedResourceDictionary](##SharedResourceDictionary)

## NotifyPropertyChangedBase
Base class which implements INotifyPropertyChanged.

## BootstrapperBase

1. create own class and derive from BootstrapperBase
2. delete App.xaml.cs
3. delete StartUpUri in App.xaml
4. add your Bootstrapper as merged ResourceDictionary
4. delete MainWindows.xaml.cs
4. rename MainWindows.xaml into ShellView.xml (optionial, but suggested)
7. in Bootstrapper override OnStartup and call `Show(new ShellViewModdel())`
- see [Example](###Bootstrapper.cs)


## ViewModelPresenter
The ViewModelPresenter presents the matching view for the current data context.
```xml
<ViewModelPresenter/>
```
## ViewModelPresenterExtension
With specification of the type:
```xml
<ListBox 
    ItemsSource="{Binding Items}"
	ItemTemplate="{ViewModelPresenter DataType={x:Type MyViewModel}, ViewTypeHint={x:Type MyView}}" />
```
Without specification of the type:
```xml
<ItemsControl 
    ItemsSource="{Binding Items}" 
    ItemTemplate="{ViewModelPresenter}" />
```

## ViewModelViewConverter
A ValueConverter to convert a ViewModel into the matching View. Supports also DataTemplate and ControlTemplate as TargetType and accepts also a Type as value.

_used by ViewModelPresenter._

## ViewLocator
The core component to create the view for a specific view-model.


## IViewLocatorStrategy
Presents a mapping strategy for ViewLocator

## Binding/BindingBase markup extension
- 'copy' of System.Windows.Data.Binding/BindingBase with unsealed ProvideValue
- 'copy' means same interface, the orginal Binding/BindingBase is wrapped

## DebugBinding (_experimental_)

## DebugHint (_experimental_)

## ObjectDebugExtensions (_experimental_)
With SetDebugInformation, GetDebugInformation, DeleteDebugInformation you can manage "debug information" on each object.
```csharp

Stream MethodA(){
    var file ="myfile.dat"
    var stream = new FileStream(file);
    stream.SetDebugInformation("FileName", file)
}

void MethodB(Stream stream){
    try{
        .. do anything with the stream
    } 
    catch (Exception ex) {
        ShowMessage("Error reading stream!" +
        "\nDebugInfo:\n" +
        stream.GetDebugInformation()
        )
    }
}

```

## Adorner

Extents the default Adorner with add/remove functionality

## RelayCommand

An ICommand implementation which relays the functionality to custom methods.

## SharedResourceDictionary

A ResourceDictionary which shares merged dictionary instead of duplicates.

# Examples
### Bootstrapper.cs
```csharp
using KsWare.Presentation.Lite;

namespace SampleApp {

    internal sealed class Bootstrapper : BootstrapperBase {

        protected override void OnStartup() {
            Show(new ShellViewModel());
        }
    }
}
```
### App.xaml
```xml
<Application
    x:Class="SampleApp.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:sampleApp="clr-namespace:SampleApp"
>
    <!-- TODO delete StartupUri="MainWindow.xaml" -->
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <sampleApp:Bootstrapper/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
### ShellViewModel.cs
```csharp
namespace SampleApp {

    public class ShellViewModel : NotifyPropertyChangedBase {
    }
}
```
