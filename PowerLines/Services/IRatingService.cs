using System.Collections.Generic;
using PowerLines.Models.ViewModels;
using PowerLines.Models;

namespace PowerLines.Services
{
    public interface IRatingService
    {
        List<FixtureRating> Calculate(bool test = false, bool reset = false);

        void RunAnalysis();
    }
}