// <copyright file="Handles.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

using iMobileDevice.Generator.Nustache;
using System.Collections.Generic;

namespace iMobileDevice.Generator
{
    public static class Handles
    {
        public static IEnumerable<NustacheGeneratedType> CreateSafeHandle(string name, ModuleGenerator generator)
        {
            yield return new NustacheGeneratedType(
                new HandleType(generator.Name, $"{name}Handle"), "Handle.cs.template");
            yield return new NustacheGeneratedType(
                new HandleMarshalerType(generator.Name, $"{name}HandleDelegateMarshaler", $"{name}Handle"), "HandleDelegateMarshaler.cs.template");
        }
    }
}
