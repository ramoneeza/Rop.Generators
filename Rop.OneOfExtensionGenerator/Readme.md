# Rop.OneOfExtensionGenerator

Features
--------

It is a source generator solution to create Extensions based on OneOf types.

The source generator will split the OneOf extension methods in several extensions methods, one for each type in the OneOf.
This allows to use the extension methods without the need to specify the type. And avoid the need to repeat the same code for each type.

The package is published in nuget as 'Rop.OneOfExtensionGenerator'

The source generator must be included as:

* OutputItemType="Analyzer" 
* ReferenceOutputAssembly="false"

 ------
 (C)2022 Ramón Ordiales Plaza
