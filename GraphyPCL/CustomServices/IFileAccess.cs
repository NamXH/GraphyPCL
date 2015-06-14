using System;
using System.IO;

namespace GraphyPCL
{
    public interface IFileAccess
    {
        bool Exists (string filename);
        string FullPath(string filename); 
        void WriteStream (string filename, Stream streamIn); 
    }
}