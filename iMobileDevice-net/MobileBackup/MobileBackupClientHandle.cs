// <copyright file="MobileBackupClientHandle.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
#endif
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
#endif
    public partial class MobileBackupClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private string creationStackTrace;
        
        private ILibiMobileDevice api;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileBackupClientHandle"/> class.
        /// </summary>
        protected MobileBackupClientHandle() : 
                base(true)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileBackupClientHandle"/> class, specifying whether the handle is to be reliably released.
        /// </summary>
        /// <param name="ownsHandle">
        /// <see langword="true"/> to reliably release the handle during the finalization phase; <see langword="false"/> to prevent reliable release (not recommended).
        /// </param>
        protected MobileBackupClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        /// <summary>
        /// Gets or sets the API to use
        /// </summary>
        public ILibiMobileDevice Api
        {
            get
            {
                return this.api;
            }
            set
            {
                this.api = value;
            }
        }
        
        /// <summary>
        /// Gets a value which represents a pointer or handle that has been initialized to zero.
        /// </summary>
        public static MobileBackupClientHandle Zero
        {
            get
            {
                return MobileBackupClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        /// <inheritdoc/>
#if !NETSTANDARD1_5
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1} using {2}. This object was created at {3}", this.GetType().Name, this.handle, this.Api, this.creationStackTrace);
            return (this.Api.MobileBackup.mobilebackup_client_free(this.handle) == MobileBackupError.Success);
        }
        
        /// <summary>
        /// Creates a new <see cref="MobileBackupClientHandle"/> from a <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="unsafeHandle">
        /// The underlying <see cref="IntPtr"/>
        /// </param>
        /// <param name="ownsHandle">
        /// <see langword="true"/> to reliably release the handle during the finalization phase; <see langword="false"/> to prevent reliable release (not recommended).
        /// </param>
        /// <returns>
        /// </returns>
        public static MobileBackupClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileBackupClientHandle safeHandle;
            safeHandle = new MobileBackupClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        /// <summary>
        /// Creates a new <see cref="MobileBackupClientHandle"/> from a <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="unsafeHandle">
        /// The underlying <see cref="IntPtr"/>
        /// </param>
        /// <returns>
        /// </returns>
        public static MobileBackupClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileBackupClientHandle.DangerousCreate(unsafeHandle, true);
        }
        
        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.handle, "MobileBackupClientHandle");
        }
        
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (((obj != null) & (obj.GetType() == typeof(MobileBackupClientHandle))))
            {
                return ((MobileBackupClientHandle)obj).handle.Equals(this.handle);
            }
            else
            {
                return false;
            }
        }
        
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.handle.GetHashCode();
        }
        
        /// <summary>
        /// Determines whether two specified instances of <see cref="MobileBackupClientHandle"/> are equal.
        /// </summary>
        /// <param name="value1">
        /// The first pointer or handle to compare.
        /// </param>
        /// <param name="value2">
        /// The second pointer or handle to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator == (MobileBackupClientHandle value1, MobileBackupClientHandle value2) 
        {
            return value1.handle == value2.handle;
        }
        
        /// <summary>
        /// Determines whether two specified instances of <see cref="MobileBackupClientHandle"/> are not equal.
        /// </summary>
        /// <param name="value1">
        /// The first pointer or handle to compare.
        /// </param>
        /// <param name="value2">
        /// The second pointer or handle to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value1"/> does not equal <paramref name="value2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator != (MobileBackupClientHandle value1, MobileBackupClientHandle value2) 
        {
            return value1.handle != value2.handle;
        }
    }
}
