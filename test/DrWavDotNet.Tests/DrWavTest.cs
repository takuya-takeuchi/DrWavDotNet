using Xunit;

// ReSharper disable once CheckNamespace
namespace DrWavDotNet.Tests
{

    public sealed partial class DrWavTest : TestBase
    {

        #region Fields

        private const string ResultDirectory = "Result";

        private const string TestImageDirectory = "TestImages";

        #endregion

        [Fact]
        public void GetNativeVersion()
        {
            var version = DrWav.GetNativeVersion();
            Assert.True(!string.IsNullOrWhiteSpace(version));
        }

    }

}
