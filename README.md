# KsWare.Presentation.Lite

- [NotifyPropertyChangedBase](##NotifyPropertyChangedBase)
- [BootstrapperBase](##BootstrapperBase)
- [ViewLocator](##ViewLocator)
- [ViewModelPresenter](##ViewModelPresenter)
- [ViewModelViewConverter](##ViewModelViewConverter)
- [IViewLocatorStrategy](##IViewLocatorStrategy)

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

_Internally used by ViewModelPresenter._

## ViewLocator
The core component to create the view for an specified view-model.

_Primarily used internally._

## IViewLocatorStrategy
Presents a mapping strategy for ViewLocator