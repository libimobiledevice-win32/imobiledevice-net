// <copyright file="PlistNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Plist
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class PlistNativeMethods
    {
        
        const string libraryName = "imobiledevice";
        
        /// <summary>
        /// Create a new root plist_t type #PLIST_DICT
        /// </summary>
        /// <returns>
        /// the created plist
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_dict", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_dict();
        
        /// <summary>
        /// Create a new root plist_t type #PLIST_ARRAY
        /// </summary>
        /// <returns>
        /// the created plist
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_array", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_array();
        
        /// <summary>
        /// Create a new plist_t type #PLIST_STRING
        /// </summary>
        /// <param name="val">
        /// the sting value, encoded in UTF8.
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_string", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_string([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string val);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_BOOLEAN
        /// </summary>
        /// <param name="val">
        /// the boolean value, 0 is false, other values are true.
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_bool", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_bool(char val);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_UINT
        /// </summary>
        /// <param name="val">
        /// the unsigned integer value
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_uint", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_uint(ulong val);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_REAL
        /// </summary>
        /// <param name="val">
        /// the real value
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_real", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_real(double val);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_DATA
        /// </summary>
        /// <param name="val">
        /// the binary buffer
        /// </param>
        /// <param name="length">
        /// the length of the buffer
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_data", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_data([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string val, ulong length);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_DATE
        /// </summary>
        /// <param name="sec">
        /// the number of seconds since 01/01/2001
        /// </param>
        /// <param name="usec">
        /// the number of microseconds
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_date", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_date(int sec, int usec);
        
        /// <summary>
        /// Create a new plist_t type #PLIST_UID
        /// </summary>
        /// <param name="val">
        /// the unsigned integer value
        /// </param>
        /// <returns>
        /// the created item
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_new_uid", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_new_uid(ulong val);
        
        /// <summary>
        /// Destruct a plist_t node and all its children recursively
        /// </summary>
        /// <param name="plist">
        /// the plist to free
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_free", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_free(System.IntPtr plist);
        
        /// <summary>
        /// Return a copy of passed node and it's children
        /// </summary>
        /// <param name="node">
        /// the plist to copy
        /// </param>
        /// <returns>
        /// copied plist
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_copy", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_copy(PlistHandle node);
        
        /// <summary>
        /// Get size of a #PLIST_ARRAY node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <returns>
        /// size of the #PLIST_ARRAY node
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_get_size", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern uint plist_array_get_size(PlistHandle node);
        
        /// <summary>
        /// Get the nth item in a #PLIST_ARRAY node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <param name="n">
        /// the index of the item to get. Range is [0, array_size[
        /// </param>
        /// <returns>
        /// the nth item or NULL if node is not of type #PLIST_ARRAY
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_get_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_array_get_item(PlistHandle node, uint n);
        
        /// <summary>
        /// Get the index of an item. item must be a member of a #PLIST_ARRAY node.
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <returns>
        /// the node index
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_get_item_index", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern uint plist_array_get_item_index(PlistHandle node);
        
        /// <summary>
        /// Set the nth item in a #PLIST_ARRAY node.
        /// The previous item at index n will be freed using #plist_free
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <param name="item">
        /// the new item at index n. The array is responsible for freeing item when it is no longer needed.
        /// </param>
        /// <param name="n">
        /// the index of the item to get. Range is [0, array_size[. Assert if n is not in range.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_set_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_array_set_item(PlistHandle node, PlistHandle item, uint n);
        
        /// <summary>
        /// Append a new item at the end of a #PLIST_ARRAY node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <param name="item">
        /// the new item. The array is responsible for freeing item when it is no longer needed.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_append_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_array_append_item(PlistHandle node, PlistHandle item);
        
        /// <summary>
        /// Insert a new item at position n in a #PLIST_ARRAY node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <param name="item">
        /// the new item to insert. The array is responsible for freeing item when it is no longer needed.
        /// </param>
        /// <param name="n">
        /// The position at which the node will be stored. Range is [0, array_size[. Assert if n is not in range.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_insert_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_array_insert_item(PlistHandle node, PlistHandle item, uint n);
        
        /// <summary>
        /// Remove an existing position in a #PLIST_ARRAY node.
        /// Removed position will be freed using #plist_free.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_ARRAY
        /// </param>
        /// <param name="n">
        /// The position to remove. Range is [0, array_size[. Assert if n is not in range.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_array_remove_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_array_remove_item(PlistHandle node, uint n);
        
        /// <summary>
        /// Get size of a #PLIST_DICT node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <returns>
        /// size of the #PLIST_DICT node
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_get_size", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern uint plist_dict_get_size(PlistHandle node);
        
        /// <summary>
        /// Create an iterator of a #PLIST_DICT node.
        /// The allocated iterator should be freed with the standard free function.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="iter">
        /// iterator of the #PLIST_DICT node
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_new_iter", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_new_iter(PlistHandle node, out PlistDictIterHandle iter);
        
        /// <summary>
        /// Increment iterator of a #PLIST_DICT node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="iter">
        /// iterator of the dictionary
        /// </param>
        /// <param name="key">
        /// a location to store the key, or NULL. The caller is responsible
        /// for freeing the the returned string.
        /// </param>
        /// <param name="val">
        /// a location to store the value, or NULL. The caller should *not*
        /// free the returned value.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_next_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_next_item(PlistHandle node, PlistDictIterHandle iter, out System.IntPtr key, out PlistHandle val);
        
        /// <summary>
        /// Get key associated to an item. Item must be member of a dictionary
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="key">
        /// a location to store the key. The caller is responsible for freeing the returned string.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_get_item_key", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_get_item_key(PlistHandle node, out System.IntPtr key);
        
        /// <summary>
        /// Get the nth item in a #PLIST_DICT node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="key">
        /// the identifier of the item to get.
        /// </param>
        /// <returns>
        /// the item or NULL if node is not of type #PLIST_DICT. The caller should not free
        /// the returned node.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_get_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_dict_get_item(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string key);
        
        /// <summary>
        /// Set item identified by key in a #PLIST_DICT node.
        /// The previous item identified by key will be freed using #plist_free.
        /// If there is no item for the given key a new item will be inserted.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="item">
        /// the new item associated to key
        /// </param>
        /// <param name="key">
        /// the identifier of the item to set.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_set_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_set_item(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string key, PlistHandle item);
        
        /// <summary>
        /// Insert a new item into a #PLIST_DICT node.
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="item">
        /// the new item to insert
        /// </param>
        /// <param name="key">
        /// The identifier of the item to insert.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_insert_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_insert_item(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string key, PlistHandle item);
        
        /// <summary>
        /// Remove an existing position in a #PLIST_DICT node.
        /// Removed position will be freed using #plist_free
        /// </summary>
        /// <param name="node">
        /// the node of type #PLIST_DICT
        /// </param>
        /// <param name="key">
        /// The identifier of the item to remove. Assert if identifier is not present.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_remove_item", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_remove_item(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string key);
        
        /// <summary>
        /// Merge a dictionary into another. This will add all key/value pairs
        /// from the source dictionary to the target dictionary, overwriting
        /// any existing key/value pairs that are already present in target.
        /// </summary>
        /// <param name="target">
        /// pointer to an existing node of type #PLIST_DICT
        /// </param>
        /// <param name="source">
        /// node of type #PLIST_DICT that should be merged into target
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_dict_merge", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_dict_merge(out PlistHandle target, PlistHandle source);
        
        /// <summary>
        /// Get the parent of a node
        /// </summary>
        /// <param name="node">
        /// the parent (NULL if node is root)
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_parent", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_get_parent(PlistHandle node);
        
        /// <summary>
        /// Get the #plist_type of a node.
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <returns>
        /// the type of the node
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_node_type", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistType plist_get_node_type(PlistHandle node);
        
        /// <summary>
        /// Get the value of a #PLIST_KEY node.
        /// This function does nothing if node is not of type #PLIST_KEY
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a C-string. This function allocates the memory,
        /// caller is responsible for freeing it.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_key_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_key_val(PlistHandle node, out System.IntPtr val);
        
        /// <summary>
        /// Get the value of a #PLIST_STRING node.
        /// This function does nothing if node is not of type #PLIST_STRING
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a C-string. This function allocates the memory,
        /// caller is responsible for freeing it. Data is UTF-8 encoded.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_string_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_string_val(PlistHandle node, out System.IntPtr val);
        
        /// <summary>
        /// Get the value of a #PLIST_BOOLEAN node.
        /// This function does nothing if node is not of type #PLIST_BOOLEAN
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a uint8_t variable.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_bool_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_bool_val(PlistHandle node, ref char val);
        
        /// <summary>
        /// Get the value of a #PLIST_UINT node.
        /// This function does nothing if node is not of type #PLIST_UINT
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a uint64_t variable.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_uint_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_uint_val(PlistHandle node, ref ulong val);
        
        /// <summary>
        /// Get the value of a #PLIST_REAL node.
        /// This function does nothing if node is not of type #PLIST_REAL
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a double variable.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_real_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_real_val(PlistHandle node, ref double val);
        
        /// <summary>
        /// Get the value of a #PLIST_DATA node.
        /// This function does nothing if node is not of type #PLIST_DATA
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to an unallocated char buffer. This function allocates the memory,
        /// caller is responsible for freeing it.
        /// </param>
        /// <param name="length">
        /// the length of the buffer
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_data_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_data_val(PlistHandle node, out System.IntPtr val, ref ulong length);
        
        /// <summary>
        /// Get the value of a #PLIST_DATE node.
        /// This function does nothing if node is not of type #PLIST_DATE
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="sec">
        /// a pointer to an int32_t variable. Represents the number of seconds since 01/01/2001.
        /// </param>
        /// <param name="usec">
        /// a pointer to an int32_t variable. Represents the number of microseconds
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_date_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_date_val(PlistHandle node, ref int sec, ref int usec);
        
        /// <summary>
        /// Get the value of a #PLIST_UID node.
        /// This function does nothing if node is not of type #PLIST_UID
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// a pointer to a uint64_t variable.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_get_uid_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_get_uid_val(PlistHandle node, ref ulong val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_KEY
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the key value
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_key_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_key_val(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_STRING
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the string value. The string is copied when set and will be
        /// freed by the node.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_string_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_string_val(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_BOOLEAN
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the boolean value
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_bool_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_bool_val(PlistHandle node, char val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_UINT
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the unsigned integer value
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_uint_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_uint_val(PlistHandle node, ulong val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_REAL
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the real value
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_real_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_real_val(PlistHandle node, double val);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_DATA
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the binary buffer. The buffer is copied when set and will
        /// be freed by the node.
        /// </param>
        /// <param name="length">
        /// the length of the buffer
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_data_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_data_val(PlistHandle node, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string val, ulong length);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_DATE
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="sec">
        /// the number of seconds since 01/01/2001
        /// </param>
        /// <param name="usec">
        /// the number of microseconds
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_date_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_date_val(PlistHandle node, int sec, int usec);
        
        /// <summary>
        /// Set the value of a node.
        /// Forces type of node to #PLIST_UID
        /// </summary>
        /// <param name="node">
        /// the node
        /// </param>
        /// <param name="val">
        /// the unsigned integer value
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_set_uid_val", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_set_uid_val(PlistHandle node, ulong val);
        
        /// <summary>
        /// Export the #plist_t structure to XML format.
        /// </summary>
        /// <param name="plist">
        /// the root node to export
        /// </param>
        /// <param name="plist_xml">
        /// a pointer to a C-string. This function allocates the memory,
        /// caller is responsible for freeing it. Data is UTF-8 encoded.
        /// </param>
        /// <param name="length">
        /// a pointer to an uint32_t variable. Represents the length of the allocated buffer.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_to_xml", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_to_xml(PlistHandle plist, out System.IntPtr plistXml, ref uint length);
        
        /// <summary>
        /// Frees the memory allocated by plist_to_xml
        /// </summary>
        /// <param name="plist_xml">
        /// The object allocated by plist_to_xml
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_to_xml_free", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_to_xml_free(System.IntPtr plistXml);
        
        /// <summary>
        /// Export the #plist_t structure to binary format.
        /// </summary>
        /// <param name="plist">
        /// the root node to export
        /// </param>
        /// <param name="plist_bin">
        /// a pointer to a char* buffer. This function allocates the memory,
        /// caller is responsible for freeing it.
        /// </param>
        /// <param name="length">
        /// a pointer to an uint32_t variable. Represents the length of the allocated buffer.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_to_bin", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_to_bin(PlistHandle plist, out System.IntPtr plistBin, ref uint length);
        
        /// <summary>
        /// Frees the memory allocated by plist_to_bin
        /// </summary>
        /// <param name="plist_bin">
        /// The object allocated by plist_to_bin
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_to_bin_free", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_to_bin_free(System.IntPtr plistBin);
        
        /// <summary>
        /// Import the #plist_t structure from XML format.
        /// </summary>
        /// <param name="plist_xml">
        /// a pointer to the xml buffer.
        /// </param>
        /// <param name="length">
        /// length of the buffer to read.
        /// </param>
        /// <param name="plist">
        /// a pointer to the imported plist.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_from_xml", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_from_xml([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string plistXml, uint length, out PlistHandle plist);
        
        /// <summary>
        /// Import the #plist_t structure from binary format.
        /// </summary>
        /// <param name="plist_bin">
        /// a pointer to the xml buffer.
        /// </param>
        /// <param name="length">
        /// length of the buffer to read.
        /// </param>
        /// <param name="plist">
        /// a pointer to the imported plist.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_from_bin", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_from_bin([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string plistBin, uint length, out PlistHandle plist);
        
        /// <summary>
        /// Import the #plist_t structure from memory data.
        /// This method will look at the first bytes of plist_data
        /// to determine if plist_data contains a binary or XML plist.
        /// </summary>
        /// <param name="plist_data">
        /// a pointer to the memory buffer containing plist data.
        /// </param>
        /// <param name="length">
        /// length of the buffer to read.
        /// </param>
        /// <param name="plist">
        /// a pointer to the imported plist.
        /// </param>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_from_memory", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void plist_from_memory([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string plistData, uint length, out PlistHandle plist);
        
        /// <summary>
        /// Test if in-memory plist data is binary or XML
        /// This method will look at the first bytes of plist_data
        /// to determine if plist_data contains a binary or XML plist.
        /// This method is not validating the whole memory buffer to check if the
        /// content is truly a plist, it's only using some heuristic on the first few
        /// bytes of plist_data.
        /// </summary>
        /// <param name="plist_data">
        /// a pointer to the memory buffer containing plist data.
        /// </param>
        /// <param name="length">
        /// length of the buffer to read.
        /// </param>
        /// <returns>
        /// 1 if the buffer is a binary plist, 0 otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_is_binary", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int plist_is_binary([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string plistData, uint length);
        
        /// <summary>
        /// Get a node from its path. Each path element depends on the associated father node type.
        /// For Dictionaries, var args are casted to const char*, for arrays, var args are caster to uint32_t
        /// Search is breath first order.
        /// </summary>
        /// <param name="plist">
        /// the node to access result from.
        /// </param>
        /// <param name="length">
        /// length of the path to access
        /// </param>
        /// <returns>
        /// the value to access.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_access_path", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_access_path(PlistHandle plist, uint length);
        
        /// <summary>
        /// Variadic version of #plist_access_path.
        /// </summary>
        /// <param name="plist">
        /// the node to access result from.
        /// </param>
        /// <param name="length">
        /// length of the path to access
        /// </param>
        /// <param name="v">
        /// list of array's index and dic'st key
        /// </param>
        /// <returns>
        /// the value to access.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_access_pathv", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern PlistHandle plist_access_pathv(PlistHandle plist, uint length, System.IntPtr v);
        
        /// <summary>
        /// Compare two node values
        /// </summary>
        /// <param name="node_l">
        /// left node to compare
        /// </param>
        /// <param name="node_r">
        /// rigth node to compare
        /// </param>
        /// <returns>
        /// TRUE is type and value match, FALSE otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(PlistNativeMethods.libraryName, EntryPoint="plist_compare_node_value", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern sbyte plist_compare_node_value(PlistHandle nodeL, PlistHandle nodeR);
    }
}
