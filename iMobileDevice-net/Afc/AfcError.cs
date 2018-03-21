// <copyright file="AfcError.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Afc
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// Error Codes 
    /// </summary>
    public enum AfcError : int
    {
        
        Success = 0,
        
        UnknownError = 1,
        
        OpHeaderInvalid = 2,
        
        NoResources = 3,
        
        ReadError = 4,
        
        WriteError = 5,
        
        UnknownPacketType = 6,
        
        InvalidArg = 7,
        
        ObjectNotFound = 8,
        
        ObjectIsDir = 9,
        
        PermDenied = 10,
        
        ServiceNotConnected = 11,
        
        OpTimeout = 12,
        
        TooMuchData = 13,
        
        EndOfData = 14,
        
        OpNotSupported = 15,
        
        ObjectExists = 16,
        
        ObjectBusy = 17,
        
        NoSpaceLeft = 18,
        
        OpWouldBlock = 19,
        
        IoError = 20,
        
        OpInterrupted = 21,
        
        OpInProgress = 22,
        
        InternalError = 23,
        
        MuxError = 30,
        
        NoMem = 31,
        
        NotEnoughData = 32,
        
        DirNotEmpty = 33,
        
        ServiceClientFailed = 34,
        
        EmptyResponse = 35,
        
        IncompleteHeader = 36,
        
        ForceSignedType = -1,
    }
}
