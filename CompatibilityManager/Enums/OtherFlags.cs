﻿using CompatibilityManager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CompatibilityManager.Enums
{
    [Flags]
    public enum OtherFlags
    {
        [Browsable(false)] None                    = 0b000,
        [Description("640X480")] RESOLUTION640X480 = 0b001,
        DISABLEDXMAXIMIZEDWINDOWEDMODE             = 0b010,
        RUNASADMIN                                 = 0b100,
    }

    public static class OtherFlagsServices
    {
        private static Dictionary<OtherFlags, string> descriptions;
        /// <summary>
        /// OtherFlags Description lookup table.
        /// </summary>
        public static Dictionary<OtherFlags, string> Descriptions => descriptions ?? (descriptions = EnumServices.GetDescriptions<OtherFlags>());

        /// <summary>
        /// Convert OtherFlags to their AppCompatFlag REG_SZ representation.
        /// </summary>
        public static List<string> ToRegistryString(this OtherFlags enumValue)
        {
            var appCompatFlags = new List<string>();
            foreach (var flag in Enum.GetValues(typeof(OtherFlags)).Cast<OtherFlags>())
            {
                var description = OtherFlagsServices.Descriptions[flag];
                if (enumValue.HasFlag(flag) && !string.IsNullOrWhiteSpace(description)) { appCompatFlags.Add(description); }
            }
            return appCompatFlags;
        }

        /// <summary>
        /// Convert an AppCompatFlag REG_SZ to its OtherFlags representation.
        /// </summary>
        public static OtherFlags FromRegistryString(ref List<string> substrings)
        {
            var otherFlags = OtherFlags.None;
            foreach (var kvp in OtherFlagsServices.Descriptions)
            {
                if (substrings.Contains(kvp.Value))
                {
                    substrings.Remove(kvp.Value);
                    otherFlags |= kvp.Key;
                }
            }
            return otherFlags;
        }
    }
}
