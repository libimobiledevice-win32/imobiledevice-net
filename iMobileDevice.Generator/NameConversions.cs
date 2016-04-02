// <copyright file="NameConversions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System.Text;

    internal static class NameConversions
    {
        public static string ToClrName(string nativeName, bool isType)
        {
            string[] parts = nativeName.Split('_');

            StringBuilder nameBuilder = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                if (!isType && i == 0)
                {
                    continue;
                }

                if (i == 1 && parts[i] == "E")
                {
                    continue;
                }

                if (parts[i] == "t")
                {
                    continue;
                }

                if (parts[i].StartsWith("idevice"))
                {
                    nameBuilder.Append("i" + char.ToUpperInvariant(parts[i][1]) + parts[i].Substring(2).ToLowerInvariant());
                }
                else
                {
                    nameBuilder.Append(char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant());
                }
            }

            return nameBuilder.ToString();
        }
    }
}
