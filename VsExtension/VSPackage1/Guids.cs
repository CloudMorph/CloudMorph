// Guids.cs
// MUST match guids.h
using System;

namespace Company.VSPackage1
{
    static class GuidList
    {
        public const string guidVSPackage1PkgString = "995a90b3-b18b-4ee5-819a-67d4bac36320";
        public const string guidVSPackage1CmdSetString = "6e5792ef-ba17-428c-8b79-6b8a679b24d3";
        public const string guidToolWindowPersistanceString = "3c172693-af24-4a17-96a8-dea9fa8b577a";

        public static readonly Guid guidVSPackage1CmdSet = new Guid(guidVSPackage1CmdSetString);
    };
}