using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace DrWavDotNet.Tests
{

    public abstract class TestBase
    {

        #region Fields

        protected const string TestDataDirectory = "TestData";

        protected const string OutputDirectory = "OutputData";

        #endregion

        #region Methods

        protected void DisposeAndCheckDisposedState(DrWavObject obj)
        {
            if (obj == null)
                return;

            obj.Dispose();
            Assert.True(obj.IsDisposed);
            Assert.True(obj.NativePtr == IntPtr.Zero);
        }

        protected void DisposeAndCheckDisposedStates(IEnumerable<DrWavObject> objs)
        {
            foreach (var obj in objs)
                this.DisposeAndCheckDisposedState(obj);
        }

        protected FileInfo GetDataFile(string filename)
        {
            return new FileInfo(Path.Combine(TestDataDirectory, filename));
        }

        protected string GetOutDir(params string[] function)
        {
            var path = Path.Combine(OutputDirectory, Path.Combine(function));
            Directory.CreateDirectory(path);
            return path;
        }

        #endregion

    }

}
