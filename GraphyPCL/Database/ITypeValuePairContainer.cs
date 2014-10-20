using System;

namespace GraphyPCL
{
    public interface ITypeValuePairContainer
    {
        string Type { get; set; }
        string Value { get; set; }
    }
}