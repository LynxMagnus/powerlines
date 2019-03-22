using System.Collections.Generic;
using PowerLines.Models;

namespace PowerLines.Inbound
{
    public interface IResultReader
    {
        List<Result> Read(string filePath);
    }
}