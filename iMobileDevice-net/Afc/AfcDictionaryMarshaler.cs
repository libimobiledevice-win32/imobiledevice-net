// <copyright file="AfcDictionaryMarshaler.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Afc
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public class AfcDictionaryMarshaler : NativeStringArrayMarshaler
    {
        
        public static System.Runtime.InteropServices.ICustomMarshaler GetInstance(string cookie)
        {
            return new AfcDictionaryMarshaler();
        }
        
        public override void CleanUpNativeData(System.IntPtr nativeData)
        {
            if ((nativeData != System.IntPtr.Zero))
            {
                LibiMobileDevice.Instance.Afc.afc_dictionary_free(nativeData).ThrowOnError();
            }
        }
    }
}
