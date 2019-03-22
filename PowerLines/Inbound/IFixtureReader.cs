using System.Collections.Generic;
using PowerLines.Models;

namespace PowerLines.Inbound
{
    public interface IFixtureReader
    {
        List<Fixture> Read(string filePath);
    }
}