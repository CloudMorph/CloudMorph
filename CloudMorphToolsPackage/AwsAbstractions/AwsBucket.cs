using System;
using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsBucket : IBucket
    {
        public string Name { get; internal set; }
    }
}