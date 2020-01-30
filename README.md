# KsWare.Presentation.Lite

- [NotifyPropertyChangedBase](##NotifyPropertyChangedBase)
- [BootstrapperBase](##BootstrapperBase)
- [ViewLocator](##ViewLocator)
- [ViewModelPresenter](##ViewModelPresenter)
- [ViewModelViewConverter](##ViewModelViewConverter)
- [IViewLocatorStrategy](##IViewLocatorStrategy)
- [(Debug)Binding](##(Debug)Binding))
- [DebugHint](##DebugHint)
- [ObjectDebugExtensions](##ObjectDebugExtensions)

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


## ViewModelPresenter
The ViewModelPresenter presents the matching view for the current data context.
```xml
<ViewModelPresenter/>
```

## ViewModelViewConverter
A ValueConverter to convert a ViewModel into the matching View.

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