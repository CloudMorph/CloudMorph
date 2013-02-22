using System.Collections.Generic;

namespace CloudAbstractions
{
    public interface IEnvironment
    {
        string MyLocalAddress { get; }
        string MyPublicAddress { get; }
        IDictionary<string, string> GetMetadata();
    }
}