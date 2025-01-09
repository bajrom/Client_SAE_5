using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests
{
    public class TestsConfig
    {
        public static string BaseURL => Environment.GetEnvironmentVariable("URL") ?? "http://localhost:5258";
    }
}
