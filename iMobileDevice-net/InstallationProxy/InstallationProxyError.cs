// <copyright file="InstallationProxyError.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.InstallationProxy
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
    public enum InstallationProxyError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        PlistError = -2,
        
        ConnFailed = -3,
        
        OpInProgress = -4,
        
        OpFailed = -5,
        
        ReceiveTimeout = -6,
        
        AlreadyArchived = -7,
        
        ApiInternalError = -8,
        
        ApplicationAlreadyInstalled = -9,
        
        ApplicationMoveFailed = -10,
        
        ApplicationSinfCaptureFailed = -11,
        
        ApplicationSandboxFailed = -12,
        
        ApplicationVerificationFailed = -13,
        
        ArchiveDestructionFailed = -14,
        
        BundleVerificationFailed = -15,
        
        CarrierBundleCopyFailed = -16,
        
        CarrierBundleDirectoryCreationFailed = -17,
        
        CarrierBundleMissingSupportedSims = -18,
        
        CommCenterNotificationFailed = -19,
        
        ContainerCreationFailed = -20,
        
        ContainerP0wnFailed = -21,
        
        ContainerRemovalFailed = -22,
        
        EmbeddedProfileInstallFailed = -23,
        
        ExecutableTwiddleFailed = -24,
        
        ExistenceCheckFailed = -25,
        
        InstallMapUpdateFailed = -26,
        
        ManifestCaptureFailed = -27,
        
        MapGenerationFailed = -28,
        
        MissingBundleExecutable = -29,
        
        MissingBundleIdentifier = -30,
        
        MissingBundlePath = -31,
        
        MissingContainer = -32,
        
        NotificationFailed = -33,
        
        PackageExtractionFailed = -34,
        
        PackageInspectionFailed = -35,
        
        PackageMoveFailed = -36,
        
        PathConversionFailed = -37,
        
        RestoreContainerFailed = -38,
        
        SeatbeltProfileRemovalFailed = -39,
        
        StageCreationFailed = -40,
        
        SymlinkFailed = -41,
        
        UnknownCommand = -42,
        
        ItunesArtworkCaptureFailed = -43,
        
        ItunesMetadataCaptureFailed = -44,
        
        DeviceOsVersionTooLow = -45,
        
        DeviceFamilyNotSupported = -46,
        
        PackagePatchFailed = -47,
        
        IncorrectArchitecture = -48,
        
        PluginCopyFailed = -49,
        
        BreadcrumbFailed = -50,
        
        BreadcrumbUnlockFailed = -51,
        
        GeojsonCaptureFailed = -52,
        
        NewsstandArtworkCaptureFailed = -53,
        
        MissingCommand = -54,
        
        NotEntitled = -55,
        
        MissingPackagePath = -56,
        
        MissingContainerPath = -57,
        
        MissingApplicationIdentifier = -58,
        
        MissingAttributeValue = -59,
        
        LookupFailed = -60,
        
        DictCreationFailed = -61,
        
        InstallProhibited = -62,
        
        UninstallProhibited = -63,
        
        MissingBundleVersion = -64,
        
        UnknownError = -256,
    }
}
