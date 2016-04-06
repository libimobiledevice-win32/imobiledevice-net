// <copyright file="NameConversions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class NameConversions
    {
        public static string ToClrName(string nativeName, NameConversion conversion)
        {
            var patchedName = nativeName;

            patchedName = patchedName.Replace("DIAGNOSTICS_RELAY", "diagnosticsrelay");
            patchedName = patchedName.Replace("HOUSE_ARREST", "housearrest");
            patchedName = patchedName.Replace("MOBILE_IMAGE_MOUNTER", "mobileimagemounter");
            patchedName = patchedName.Replace("PROPERTY_LIST_SERVICE", "propertylistservice");
            patchedName = patchedName.Replace("SYSLOG_RELAY", "syslogrelay");
            patchedName = patchedName.Replace("FILE_RELAY", "filerelay");

            List<string> parts = new List<string>(patchedName.Split('_'));

            StringBuilder nameBuilder = new StringBuilder();

            int i = 0;

            while (i < parts.Count)
            {
                if (
                    (conversion == NameConversion.Function || conversion == NameConversion.Field)
                    && i == 0)
                {
                    i++;
                    continue;
                }

                if (i == 1 && parts[i] == "E")
                {
                    i++;
                    continue;
                }

                if (parts[i] == "t")
                {
                    i++;
                    continue;
                }

                // Manual patching of naming inconsistencies
                if (parts[i] == "diagnosticsrelay")
                {
                    parts[i] = "Diagnostics";
                    parts.Insert(i + 1, "Relay");
                }
                else if (parts[i] == "cb")
                {
                    parts[i] = "Call";
                    parts.Insert(i + 1, "Back");
                }
                else if (parts[i] == "instproxy")
                {
                    parts[i] = "Installation";
                    parts.Insert(i + 1, "Proxy");
                }
                else if (parts[i] == "lockdownd")
                {
                    parts[i] = "lockdown";
                }
                else if (parts[i] == "np")
                {
                    parts[i] = "Notification";
                    parts.Insert(i + 1, "Proxy");
                }
                else if (parts[i] == "restored")
                {
                    parts[i] = "restore";
                }
                else if (parts[i] == "debugserver")
                {
                    parts[i] = "Debug";
                    parts.Insert(i + 1, "Server");
                }
                else if (parts[i] == "libimobiledevice")
                {
                    parts[i] = "iDevice";
                }
                else if (parts[i] == "heartbeat")
                {
                    parts[i] = "Heart";
                    parts.Insert(i + 1, "Beat");
                }
                else if (parts[i] == "mobilebackup")
                {
                    parts[i] = "Mobile";
                    parts.Insert(i + 1, "Backup");
                }
                else if (parts[i] == "mobilebackup2")
                {
                    parts[i] = "Mobile";
                    parts.Insert(i + 1, "Backup2");
                }
                else if (parts[i] == "mobilesync")
                {
                    parts[i] = "Mobile";
                    parts.Insert(i + 1, "Sync");
                }
                else if (parts[i] == "webinspector")
                {
                    parts[i] = "Web";
                    parts.Insert(i + 1, "Inspector");
                }
                else if (parts[i] == "sbservices")
                {
                    parts[i] = "Spring";
                    parts.Insert(i + 1, "Board");
                    parts.Insert(i + 2, "Services");
                }
                else if (parts[i] == "housearrest")
                {
                    parts[i] = "House";
                    parts.Insert(i + 1, "Arrest");
                }
                else if (parts[i] == "propertylistservice")
                {
                    parts[i] = "Property";
                    parts.Insert(i + 1, "List");
                    parts.Insert(i + 2, "Service");
                }
                else if (parts[i] == "syslogrelay")
                {
                    parts[i] = "Syslog";
                    parts.Insert(i + 1, "Relay");
                }
                else if (parts[i] == "filerelay")
                {
                    parts[i] = "File";
                    parts.Insert(i + 1, "Relay");
                }

                if (conversion == NameConversion.Parameter && i == 0)
                {
                    nameBuilder.Append(char.ToLowerInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant());
                }
                else if (parts[i].StartsWith("idevice", StringComparison.OrdinalIgnoreCase))
                {
                    nameBuilder.Append("i" + char.ToUpperInvariant(parts[i][1]) + parts[i].Substring(2).ToLowerInvariant());
                }
                else
                {
                    nameBuilder.Append(char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant());
                }

                i++;
            }

            return nameBuilder.ToString();
        }
    }
}
