// Guids.cs
// MUST match guids.h
using System;

namespace Company.VSPackage2
{
    static class GuidList
    {
        public const string guidVSPackage2PkgString = "930f89b2-4ccd-49fd-8bb6-0698c115856b";
        public const string guidVSPackage2CmdSetString = "6a1b14c0-b964-4df0-ac84-83749af8cf20";
        public const string guidToolWindowPersistanceString = "ed67a5a3-b3e7-43ca-8653-4f180809b897";
        public const string guidVSPackage2EditorFactoryString = "83bcce3d-5395-4238-866f-6a942f47150b";

        public static readonly Guid guidVSPackage2CmdSet = new Guid(guidVSPackage2CmdSetString);
        public static readonly Guid guidVSPackage2EditorFactory = new Guid(guidVSPackage2EditorFactoryString);
    };
}