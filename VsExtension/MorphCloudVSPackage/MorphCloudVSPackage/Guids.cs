// Guids.cs
// MUST match guids.h
using System;

namespace BlueMetalArchitects.MorphCloudVSPackage
{
    static class GuidList
    {
        public const string guidMorphCloudVSPackagePkgString = "6036f975-5cf7-429f-940c-4e4105cd5a06";
        public const string guidMorphCloudVSPackageCmdSetString = "f67ff87f-58b4-4bb9-b1b2-eb03bb912562";
        public const string guidToolWindowPersistanceString = "a4631028-3681-49bc-bb2d-999bb967078f";

        public static readonly Guid guidMorphCloudVSPackageCmdSet = new Guid(guidMorphCloudVSPackageCmdSetString);
    };
}