# Rop.Generators Solution

Features
--------

Rop.Generators is a software solution that provides a wide range of utility code generators for C# based on Roslyn. 
Designed to simplify and expedite application development, rop.Generators offers a set of ready-to-use code generation tools
that can significantly enhance developer productivity.

With rop.Generators, developers can leverage a collection of highly effective and customizable Roslyn code generators. 
These generators are designed to automate common code generation tasks, and avoid pitfalls of current C# language.

In addition, rop.Generators significantly improves programming in WinForms under .NET 7.0. 
Developers using rop.Generators in conjunction with .NET 7.0 can leverage its WinForms-specific code generation capabilities
to accelerate the creation of UI components, such as forms, controls, and dialogs. 
The generators provide boilerplate code for common UI patterns, reducing the amount of manual coding required and promoting consistency across the application.

Rop.Generators is a free and open source project, licensed under the MIT license.

Each package is published in nuget

Rop.ControllerGenerator 
------------------------
This generator is designed to automate the creation of controllers for any application.
This is specialy userful for WinForms applications, but can be used in any other type of application.

- A Controller must be a class that resides in a "Controllers" path in the project, or
alternatively, a class decorated with the attribute `[Controller]`

- A Controller must have derive from a generic class with the following signature:
 `BaseController<T>` where T is the type of the form that the controller will handle.

- A controler must have a contructor with the following signature:
    `public Controller(T form) : base(form)`
    where T is the type of the form that the controller will handle.

In the other hand, the form must be partial and decorated with the attribute `[InsertControllers]` and must have the following line in the constructor:
```
    public Form1() {
        [...] 
        InitControllers(); 
        [...] 
     }
```

The nuget package Rop.ControllerGenerator.Annotations is required in order to use the attributes.

Rop.CopyPartialGenerator
------------------------
This generator is designed to automate the creation of horizontaly derived classes for any application.
There are three types of new classes that can be generated:

- `[CopyPArtialTo]`A partial class that derives from another partial class (usualy to implements an interface).
  This kind of partial class is userful when you need to implement an interface in a class whith exactly the same code.
- `[CopyPartialAsImmutableRecord]` A partial class that derives from another partial class but as Immutable Record.
  This kind of partial class is userful when you need to implement a immutable version of a class.
- `[CopyPartialAsEditableClass]` A partial class that derives from another partial class but as an Editable Class.
  This kind of partial class is userful when you need to implement a editable version of an immutable class.

The nuget package Rop.CopyPartialGenerator.Annotations is required in order to use the attributes.

Rop.DerivedFromGenerator
------------------------
This generator is designed to allow the creation of derived classes when generic base classes are not allowded.
This is specialy userful for WinForms applications, but can be used in any other type of application.

- The derived class must be a partial class whit an interface of type `IDerivedFrom` 

The nuget package Rop.DerivedFromGenerator.Annotations is required in order to use the interface.

Rop.OneOfExtensionsGenerator
------------------------

This generator is designed to allow the creation of extension methods based on OneOf methods.
This is specialy userful when you need to create extension methods for each type of a OneOf type.
This avoid to repeating code for each type of the OneOf type.

- The extension class must be a partial static class decorated with the attribute `[OneOfExtensions]`
- The OneOf Extension methods can be private and prefixed with `_`
- The OneOf Extensio methods must be decorated with the attribute `[SplitOneOf]`

The nuget package Rop.OneOfExtensionsGenerator.Annotations is required in order to use the attributes.
The nuget package OneOf is required in order to use the OneOf type.

Rop.ProxyGenerator
------------------------

Rop.ProxyGenerator is a source generator package to automatic proxy of interfaces.
It can be used to provide Aspect Oriented Programming to c# via a "proxy".

-The partial class to be generated must be decorated with the attribute `[ProxyOf(interface,property into the class to be proxied, excluded names to be proxied)]` 
-The partial class must have a property of a field with a instance of the interface to be proxied.
-There are a lot of auxiliary attributesd to control the proxy behavior.

The nuget package Rop.ProxyGenerator.Annotations is required in order to use the attributes.

Rop.StaticExtensionGenerator
------------------------

Rop.StaticExtensionGenerator is a source generator package to automatic static extension methods.
It can be used to provide static extension methods to classes. The current c# languaje does not allow to create static extension methods.

-The static method to contain the static extension must be a generic class where the first generic type is the type class to be extended.
-This method has to be decorated with the attribute `[StaticExtension]`
-This method has to be included in one of the base class of the class to be extended.
-The class where implement the static extension must be decorated with the attribute `[InsertStaticExtensions]`

The nuget package Rop.StaticExtensionGenerator.Annotations is required in order to use the attributes.

Rop.ObservableGenerator
------------------------

Rop.ObservableGenerator is a source generator package to automatic observable properties.
It can be used to provide observable properties to classes. The current c# languaje does not allow to create observable properties.

- Define a private field to contain the observable property.
- Decorates the field with `[AutoObservable]` or `[AutoNotify]`attributes.
- AutoNotify attribute is intended to be used with INotifyPropertyChanged interface.
- AutoObservable is to use directly with the `On<PropertyName>Changed` and `event EventHandler?  <PropertyName>Changed` pattern.

The nuget package Rop.ObservableGenerator.Annotations is required in order to use the attributes.


 ------
 (C)2022 Ramón Ordiales Plaza
