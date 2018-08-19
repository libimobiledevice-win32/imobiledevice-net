// <copyright file="GlobalSuppressions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator.Nustache
{
    public class HandleType : NustacheType
    {
        public HandleType(string @namespace, string name)
            : base(@namespace, name)
        {
        }

        /// <summary>
        /// The name of the method which is to be invoked to release the native handle.
        /// Can be <see langword="null"/> if no such method exists.
        /// </summary>
        public string ReleaseMethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the release method returns a value.
        /// </summary>
        public bool ReleaseMethodReturnsValue
        {
            get;
            set;
        }
    }
}
