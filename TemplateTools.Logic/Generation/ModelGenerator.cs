﻿//@CodeCopy

namespace TemplateTools.Logic.Generation
{
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using TemplateTools.Logic.Common;
    using TemplateTools.Logic.Contracts;
    using TemplateTools.Logic.Extensions;
    using TemplateTools.Logic.Models;

    /// <summary>
    /// Represents a class that generates models based on a given type. This class is abstract and internal.
    /// </summary>
    /// <inheritdoc cref="ItemGenerator"/>
    /// <remarks>
    /// Initializes a new instance of the ModelGenerator class.
    /// </remarks>
    /// <param name="solutionProperties">The solution properties.</param>
    internal abstract partial class ModelGenerator(ISolutionProperties solutionProperties) : ItemGenerator(solutionProperties)
    {
        /// <summary>
        /// Gets the properties of the item.
        /// </summary>
        protected abstract ItemProperties ItemProperties { get; }
        #region overrides
        /// <summary>
        /// Returns the type of the property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <returns>The type of the property after converting it to the model type.</returns>
        protected override string GetPropertyType(PropertyInfo propertyInfo)
        {
            var propertyType = base.GetPropertyType(propertyInfo);
            var result = ItemProperties.ConvertEntityToModelType(propertyType);

            return ConvertPropertyType(result);
        }
        /// <summary>
        /// Copies the property value from one object to another.
        /// </summary>
        /// <param name="copyType">The type of the object to copy the property value to.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property to be copied.</param>
        /// <returns>
        /// The copied property value, or the value returned by the base implementation of <see cref="CopyProperty"/>
        /// if the property does not meet the specified conditions for copying.
        /// </returns>
        protected override string CopyProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;
            string modelFolder = string.Empty;

            if (copyType.Contains($".{StaticLiterals.ModelsFolder}."))
            {
                modelFolder = $"{StaticLiterals.ModelsFolder}.";
            }

            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
                && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (ItemProperties.IsArrayType(propertyInfo.PropertyType)
                    && propertyInfo.PropertyType.GetElementType() != typeof(string)
                    && propertyInfo.PropertyType.GetElementType()!.IsPrimitive == false)
                {
                    var modelType = ItemProperties.GetSubType(propertyInfo.PropertyType.GetElementType()!);

                    modelType = $"{modelFolder}{modelType}";
                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (ItemProperties.IsListType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.GetSubType(propertyInfo.PropertyType.GenericTypeArguments[0]);

                    modelType = $"{modelFolder}{modelType}";
                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToList();";
                }
                else if (ItemProperties.IsEntityType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.GetSubType(propertyInfo.PropertyType);

                    modelType = $"{modelFolder}{modelType}";
                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name} != null ? {modelType}.Create((object)other.{propertyInfo.Name}) : null;";
                }
            }
            return result ?? base.CopyProperty(copyType, propertyInfo);
        }
        /// <summary>
        /// Copies the specified property of the given copy type.
        /// </summary>
        /// <param name="copyType">The type to copy.</param>
        /// <param name="propertyInfo">The PropertyInfo object representing the property to be copied.</param>
        /// <returns>A string representation of the copied property.</returns>
        protected override string CopyDelegateProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;

            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
            && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (ItemProperties.IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (ItemProperties.IsListType(propertyInfo.PropertyType))
                {
                    result = string.Empty;
                }
                else if (ItemProperties.IsEntityType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name} != null ? {modelType}.Create((object)other.{propertyInfo.Name}) : null;";
                }
            }
            return result ?? base.CopyProperty(copyType, propertyInfo);
        }
        #endregion overrides

        #region create attributes
        /// <summary>
        /// Creates the model property attributes for a given PropertyInfo object and UnitType.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <param name="unitType">The UnitType associated with the property.</param>
        /// <param name="codeLines">The list of code lines to add the attributes to.</param>
        protected virtual void CreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines)
        {
            var handled = false;

            BeforeCreateModelPropertyAttributes(propertyInfo, unitType, codeLines, ref handled);
            if (handled == false)
            {
                var itemName = $"{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}";
                var attributes = QuerySetting<string>(unitType, ItemType.ModelProperty, itemName, StaticLiterals.Attribute, string.Empty);

                if (string.IsNullOrEmpty(attributes) == false)
                {
                    codeLines.Add($"[{attributes}]");
                }
            }
            AfterCreateModelPropertyAttributes(propertyInfo, unitType, codeLines);
        }
        /// <summary>
        /// Method called before creating model property attributes.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="codeLines">The list of code lines.</param>
        /// <param name="handled">A reference to a bool indicating if the method has been handled.</param>
        /// <remarks>
        /// This method is called before creating attributes for a model property.
        /// It allows customization of the attribute creation process.
        /// The property information, unit type, and code lines for the property will be passed as parameters.
        /// The handled parameter can be modified by the method to indicate if it has handled the operation.
        /// </remarks>
        /// <seealso cref="AfterCreateModelPropertyAttributes(PropertyInfo, UnitType, List<string>)"/>
        /// <seealso cref="CreateModelPropertyAttributes(PropertyInfo, UnitType, List<string>)"/>
        partial void BeforeCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines, ref bool handled);
        /// <summary>
        /// This method is called after creating model property attributes.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <param name="unitType">The <see cref="UnitType"/> representing the unit type.</param>
        /// <param name="codeLines">A list of strings representing the code lines.</param>
        partial void AfterCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines);
        #endregion create attributes

        #region converters
        /// <summary>
        /// Converts the given model name to a string representation.
        /// </summary>
        /// <param name="modelName">The model name to be converted.</param>
        /// <returns>The converted string representation of the model name.</returns>
        protected virtual string ConvertModelName(string modelName) => modelName;
        /// <summary>
        /// Converts the given model subtype.
        /// </summary>
        /// <param name="modelSubType">The model subtype to be converted.</param>
        /// <returns>The converted model subtype.</returns>
        protected virtual string ConvertModelSubType(string modelSubType) => modelSubType;
        /// <summary>
        /// Converts the specified model namespace.
        /// </summary>
        /// <param name="modelNamespace">The model namespace to be converted.</param>
        /// <returns>The converted model namespace.</returns>
        protected virtual string ConvertModelNamespace(string modelNamespace) => modelNamespace;
        /// <summary>
        /// Converts the full name of the model.
        /// </summary>
        /// <param name="modelFullName">The full name of the model.</param>
        /// <returns>The converted full name of the model.</returns>
        protected virtual string ConvertModelFullName(string modelFullName) => modelFullName;
        /// <summary>
        /// Converts the model subpath to a string representation.
        /// </summary>
        /// <param name="modelSubPath">The model subpath to be converted.</param>
        /// <returns>The converted string representation of the model subpath.</returns>
        protected virtual string ConvertModelSubPath(string modelSubPath) => modelSubPath;
        /// <summary>
        /// Converts the specified model base type.
        /// </summary>
        /// <param name="modelBaseType">The model base type to convert.</param>
        /// <returns>The converted model base type.</returns>
        protected virtual string ConvertModelBaseType(string modelBaseType) => modelBaseType;
        #endregion converters

        /// <summary>
        /// Creates an entity contract for the specified type.
        /// </summary>
        /// <param name="type">The type for which the entity contract is created.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>A <see cref="GeneratedItem"/> representing the created entity contract.</returns>
        protected virtual GeneratedItem CreateEntityContract(Type type, UnitType unitType, ItemType itemType)
        {
            var baseType = type.BaseType;
            var inherit = baseType != null ? (baseType.Name.Equals(StaticLiterals.EntityObjectName) ? $" : {StaticLiterals.GlobalUsingIdentifiableName}" : $" : {ItemProperties.CreateContractName(baseType)}") : string.Empty;
            var itemName = ItemProperties.CreateContractName(type);
            var fileName = $"{itemName}{StaticLiterals.CSharpFileExtension}";
            var visibility = ItemProperties.GetDefaultVisibility(type);
            var itemFullName = ItemProperties.CreateFullContractType(type);
            var itemNamespace = ItemProperties.CreateFullNamespace(type, StaticLiterals.ContractsFolder);
            var typeProperties = type.GetAllPropertyInfos().Where(pi => pi.DeclaringType == type) ?? [];
            var generateProperties = typeProperties.Where(pi => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(pi.Name)) == false
                                                             && ItemProperties.IsEntityType(pi.PropertyType) == false
                                                             && ItemProperties.IsEntityListType(pi.PropertyType) == false
                                                             && ItemProperties.IsEntityArrayType(pi.PropertyType) == false) ?? [];
            var result = new GeneratedItem(unitType, itemType)
            {
                FullName = itemFullName,
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateSubFilePath(type, fileName, StaticLiterals.ContractsFolder),
            };

            visibility = QuerySetting<string>(unitType, itemType, type, StaticLiterals.Visibility, visibility);

            result.AddRange(CreateComment(type));
            result.Add($"{visibility} partial interface {itemName}{inherit}");
            result.Add("{");
            foreach (var propertyInfo in generateProperties)
            {
                if (QuerySetting<bool>(unitType, ItemType.InterfaceProperty, propertyInfo.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    var getAccessor = string.Empty;
                    var setAccessor = string.Empty;
                    var propertyType = GetPropertyType(propertyInfo);

                    if (propertyInfo.CanRead 
                        && QuerySetting<bool>(unitType, ItemType.InterfaceProperty, propertyInfo.DeclaringName(), StaticLiterals.GetAccessor, "True"))
                    {
                        getAccessor = "get;";
                    }
                    if (propertyInfo.CanWrite 
                        && QuerySetting<bool>(unitType, ItemType.InterfaceProperty, propertyInfo.DeclaringName(), StaticLiterals.SetAccessor, "True"))
                    {
                        setAccessor = "set;";
                    }
                    result.Add($"{propertyType} {propertyInfo.Name}" + " { " + $"{getAccessor} {setAccessor}" + " } ");
                }
            }

            // Added copy properties method
            result.AddRange(CreateContractCopyProperties(type, itemName, pi => pi.Name == StaticLiterals.IdentityProperty
                                                                            || (ItemProperties.IsEntityType(pi.PropertyType) == false
                                                                                && ItemProperties.IsEntityListType(pi.PropertyType) == false
                                                                                && ItemProperties.IsEntityArrayType(pi.PropertyType) == false)));


 //           || generateProperties.Contains(pi));;

            result.Add("}");
            result.EnvelopeWithANamespace(itemNamespace);
            result.FormatCSharpCode();
            return result;
        }
        /// <summary>
        /// Creates a copy of the properties of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="visibility">The visibility of the copy properties method.</param>
        /// <param name="type">The type whose properties will be copied.</param>
        /// <param name="copyType">The type to which the properties will be copied.</param>
        /// <param name="filter">An optional filter function to determine which properties to copy.</param>
        /// <returns>
        /// An enumerable collection of strings representing the generated copy properties method.
        /// </returns>
        public virtual IEnumerable<string> CreateContractCopyProperties(Type type, string copyType, Func<PropertyInfo, bool>? filter = null)
        {
            var result = new List<string>();
            var generationProperties = type.GetAllPropertyInfos();
            var filteredProperties = generationProperties.Where(filter ?? (p => true));

            result.AddRange(CreateComment("Copies the properties of another object to this instance."));
            result.Add("/// <param name=\"other\">The object to copy the properties from.</param>");
            result.Add($"void CopyProperties({copyType} other)");
            result.Add("{");
            result.Add("other.CheckArgument(nameof(other));");
            result.Add("bool handled = false;");
            result.Add("BeforeCopyProperties(other, ref handled);");
            result.Add("if (handled == false)");
            result.Add("{");

            foreach (var item in filteredProperties)
            {
                if (item.CanWrite && item.CanRead && CanCreate(item) && CanCopyProperty(item))
                {
                    result.Add(CopyProperty(copyType, item));
                }
            }

            result.Add("}");
            result.Add("AfterCopyProperties(other);");
            result.Add("}");

            result.AddRange(CreateComment("This method is called before copying the properties of another object to the current instance."));
            result.Add("/// <param name=\"other\">The object to copy the properties from.</param>");
            result.Add("/// <param name=\"handled\">A boolean value that indicates whether the method has been handled.</param>");
            result.Add($"partial void BeforeCopyProperties({copyType} other, ref bool handled);");

            result.AddRange(CreateComment("This method is called after copying properties from another instance of the class."));
            result.Add("/// <param name=\"other\">The other instance of the class from which properties were copied.</param>");
            result.Add($"partial void AfterCopyProperties({copyType} other);");
            return result.Where(l => string.IsNullOrEmpty(l) == false);
        }

        /// <summary>
        /// Creates a model from a given type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="unitType">The unit type of the model.</param>
        /// <param name="itemType">The item type of the model.</param>
        /// <returns>The generated model as an GeneratedItem object.</returns>
        protected virtual GeneratedItem CreateModelFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(ItemProperties.CreateModelName(type));
            var modelSubType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var modelNamespace = ConvertModelNamespace(ItemProperties.CreateModelNamespace(type));
            var modelFullName = ConvertModelFullName(CreateModelFullName(type));
            var modelSubFilePath = ConvertModelSubPath(ItemProperties.CreateModelSubPath(type, string.Empty, StaticLiterals.CSharpFileExtension));
            var visibility = QuerySetting<string>(unitType, itemType, type, StaticLiterals.Visibility, "public");
            var attributes = QuerySetting<string>(unitType, itemType, type, StaticLiterals.Attribute, string.Empty);
            var contractType = ItemProperties.CreateFullCommonContractType(type);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? [];
            GeneratedItem result = new(unitType, itemType)
            {
                FullName = modelFullName,
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = modelSubFilePath,
            };
            result.AddRange(CreateComment($"This model represents a transmission model for the '{type.Name}' data unit."));
            CreateModelAttributes(type, unitType, itemType, result.Source);
            result.Add($"{(attributes.HasContent() ? $"[{attributes}]" : string.Empty)}");
            result.Add($"{visibility} partial class {modelName} : {contractType}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName));

            foreach (var propertyInfo in generateProperties)
            {
                if (CanCreate(propertyInfo)
                    && propertyInfo.IsNavigationProperties() == false
                    && QuerySetting<bool>(unitType, ItemType.ModelProperty, type, StaticLiterals.Generate, "True"))
                {
                    CreateModelPropertyAttributes(propertyInfo, unitType, result.Source);
                    result.AddRange(CreateProperty(type, propertyInfo));
                }
            }

            var lambda = QuerySetting<string>(unitType, itemType, type, ItemType.Lambda.ToString(), string.Empty);

            if (lambda.HasContent())
            {
                result.Add($"{lambda};");
            }

            result.AddRange(CreateEquals(type, modelSubType));
            result.AddRange(CreateGetHashCode(type));
            result.Add("}");
            result.EnvelopeWithANamespace(modelNamespace, "using System;");
            result.FormatCSharpCode();
            return result;
        }
        /// <summary>
        /// Creates a model inheritance based on the given type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type to create the model inheritance for.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated item representing the model inheritance.</returns>
        protected virtual GeneratedItem CreateModelInheritance(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(ItemProperties.CreateModelName(type));
            var modelNamespace = ConvertModelNamespace(ItemProperties.CreateModelNamespace(type));
            var modelFullName = ConvertModelFullName(CreateModelFullName(type));
            var modelSubFilePath = ConvertModelSubPath(ItemProperties.CreateModelSubPath(type, "Inheritance", StaticLiterals.CSharpFileExtension));
            var modelBaseType = ConvertModelBaseType(GetBaseClassByType(type));
            var result = new GeneratedItem(unitType, itemType)
            {
                FullName = modelFullName,
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = modelSubFilePath,
            };
            result.AddRange(CreateComment($"This part of the class contains the derivation for the '{type.Name}'."));
            result.Source.Add($"partial class {modelName} : {modelBaseType}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(modelNamespace);
            result.FormatCSharpCode();
            return result;
        }

        /// <summary>
        /// Retrieves the base class by type.
        /// </summary>
        /// <param name="type">The type to get the base class for.</param>
        /// <returns>The name of the base class for the specified type.</returns>
        protected static string GetBaseClassByType(Type type)
        {
            var result = "object";
            var found = false;
            var runType = type.BaseType;

            while (runType != null && found == false)
            {
                if (StaticLiterals.BaseClassMapping.TryGetValue(runType.Name, out string? value))
                {
                    found = true;
                    result = value;
                }
                runType = runType.BaseType;
            }
            return result;
        }
        /// <summary>
        /// Creates the full name of the model by concatenating the namespace and the name of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <returns>The full name of the model.</returns>
        protected string CreateModelFullName(Type type)
        {
            return $"{ItemProperties.CreateModelNamespace(type)}.{type.Name}";
        }
        #region Partial methods
        /// <summary>
        /// Creates model attributes for a given type, unit type, and source.
        /// </summary>
        /// <param name="type">The type for which the model attributes are being created.</param>
        /// <param name="unitType">The unit type for the model attributes.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="source">The source list for the model attributes.</param>
        partial void CreateModelAttributes(Type type, UnitType unitType, ItemType itemType, List<string> source);
        #endregion Partial methods
    }
}

