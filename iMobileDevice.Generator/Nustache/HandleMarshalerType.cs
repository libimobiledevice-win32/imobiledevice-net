// <copyright file="GlobalSuppressions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator.Nustache
{
    /// <summary>
    /// Represents a marshaler for a handle.
    /// </summary>
    public class HandleMarshalerType : NustacheType
    {
        public HandleMarshalerType(string @namespace, string name, string handleName)
            : base(@namespace, name)
        {
            this.HandleName = handleName;
        }

        public string HandleName
        {
            get;
            set;
        }
    }
}
