using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

// ReSharper disable once CheckNamespace
namespace DrWavDotNet.Tests
{

    public sealed partial class DrWavTest : TestBase
    {

        [Fact]
        public void DrWav()
        {
            using var drwav = new DrWav();
            Assert.NotNull(drwav);

            drwav.Dispose();
        }

        [Fact]
        public void DrWavInitFile()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                Assert.Throws<ArgumentNullException>(() => { drwav.InitFile(null); }); 

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));

                Assert.Equal(file.Answer, drwav.SampleRate);
                
                drwav.Dispose();
           
                Assert.Throws<ArgumentNullException>(() => { drwav.InitFile(null); }); 
                Assert.Throws<ObjectDisposedException>(() => { drwav.InitFile(path); });
            }
        }
    
        [Fact]
        public void DrWavInitMemory()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                Assert.Throws<ArgumentNullException>(() => { drwav.InitMemory(null); }); 

                var path = this.GetDataFile(file.File).FullName;
                var data = File.ReadAllBytes(path);
                Assert.True(drwav.InitMemory(data));

                Assert.Equal(file.Answer, drwav.SampleRate);
                
                drwav.Dispose();
           
                Assert.Throws<ArgumentNullException>(() => { drwav.InitMemory(null); }); 
                Assert.Throws<ObjectDisposedException>(() => { drwav.InitMemory(data); });
            }
        }

        [Fact]
        public void DrWavInitMemoryNative()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                Assert.Throws<ArgumentNullException>(() => { drwav.InitMemory(null); }); 

                var path = this.GetDataFile(file.File).FullName;
                var data = File.ReadAllBytes(path);
                var buffer = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, buffer, data.Length);
                Assert.True(drwav.InitMemory(buffer, (uint)data.Length));
                
                Assert.Equal(file.Answer, drwav.SampleRate);

                drwav.Dispose();
           
                Assert.Throws<ArgumentNullException>(() => { drwav.InitMemory(null); }); 
                Assert.Throws<ObjectDisposedException>(() => { drwav.InitMemory(buffer, (uint)data.Length); });

                Marshal.FreeHGlobal(buffer);
            }
        }
    
        [Fact]
        public void DrWavUninit()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                Assert.Throws<ArgumentNullException>(() => { drwav.InitFile(null); }); 

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));
                Assert.True(drwav.Uninit() == DrWavResult.Success);
                
                drwav.Dispose();
           
                Assert.Throws<ObjectDisposedException>(() => { drwav.Uninit(); });                
            }
        }

        [Fact]
        public void DrWavReadPcmFramesS16()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                var path = this.GetDataFile(file.File).FullName;
                var data = File.ReadAllBytes(path);
                Assert.True(drwav.InitMemory(data));

                var buffer = new short[100];
                Assert.True(drwav.ReadPcmFramesS16((uint)buffer.Length, buffer) == (ulong)buffer.Length);

                Assert.Equal(file.Answer, drwav.SampleRate);
                
                drwav.Dispose();
           
                Assert.Throws<ArgumentNullException>(() => { drwav.ReadPcmFramesS16(0, null); }); 
                Assert.Throws<ArgumentOutOfRangeException>(() => { drwav.ReadPcmFramesS16((uint)200, buffer); }); 
                Assert.Throws<ObjectDisposedException>(() => { drwav.ReadPcmFramesS16((uint)buffer.Length, buffer); });
            }
        }

        [Fact]
        public void DrWavReadPcmFramesS16Native()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();
                var path = this.GetDataFile(file.File).FullName;
                var data = File.ReadAllBytes(path);
                Assert.True(drwav.InitMemory(data));

                // buffer should be treated as short
                var buffer = Marshal.AllocHGlobal(100);
                Assert.True(drwav.ReadPcmFramesS16((uint)50, buffer) == (ulong)50);

                Assert.Equal(file.Answer, drwav.SampleRate);
                
                drwav.Dispose();
           
                Assert.Throws<ArgumentException>(() => { drwav.ReadPcmFramesS16(0, IntPtr.Zero); }); 
                Assert.Throws<ObjectDisposedException>(() => { drwav.ReadPcmFramesS16((uint)50, buffer); });

                Marshal.FreeHGlobal(buffer);
            }
        }

        [Fact]
        public void DrWavBitsPerSample()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_8khz.wav",     Answer = (ushort)16 },
                new { File = "sin_1000Hz_-3dBFS_3s_16khz.wav",    Answer = (ushort)16 },
                new { File = "sin_1000Hz_-3dBFS_3s_22.05khz.wav", Answer = (ushort)16 },
                new { File = "sin_1000Hz_-3dBFS_3s_32khz.wav",    Answer = (ushort)16 },
                new { File = "sin_1000Hz_-3dBFS_3s_44.1khz.wav",  Answer = (ushort)16 },
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)16 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));

                Assert.Equal(file.Answer, drwav.BitsPerSample);
                
                drwav.Dispose();
           
                Assert.Throws<ObjectDisposedException>(() => { var _ = drwav.BitsPerSample; });
            }
        }

        [Fact]
        public void DrWavChannels()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_8khz.wav",     Answer = (ushort)1 },
                new { File = "sin_1000Hz_-3dBFS_3s_16khz.wav",    Answer = (ushort)1 },
                new { File = "sin_1000Hz_-3dBFS_3s_22.05khz.wav", Answer = (ushort)1 },
                new { File = "sin_1000Hz_-3dBFS_3s_32khz.wav",    Answer = (ushort)1 },
                new { File = "sin_1000Hz_-3dBFS_3s_44.1khz.wav",  Answer = (ushort)1 },
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)1 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));

                Assert.Equal(file.Answer, drwav.Channels);
                
                drwav.Dispose();
           
                Assert.Throws<ObjectDisposedException>(() => { var _ = drwav.Channels; });
            }
        }

        [Fact]
        public void DrWavSampleRate()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_8khz.wav",     Answer = (ushort)8000 },
                new { File = "sin_1000Hz_-3dBFS_3s_16khz.wav",    Answer = (ushort)16000 },
                new { File = "sin_1000Hz_-3dBFS_3s_22.05khz.wav", Answer = (ushort)22050 },
                new { File = "sin_1000Hz_-3dBFS_3s_32khz.wav",    Answer = (ushort)32000 },
                new { File = "sin_1000Hz_-3dBFS_3s_44.1khz.wav",  Answer = (ushort)44100 },
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ushort)48000 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));

                Assert.Equal(file.Answer, drwav.SampleRate);
                
                drwav.Dispose();
           
                Assert.Throws<ObjectDisposedException>(() => { var _ = drwav.SampleRate; });
            }
        }

        [Fact]
        public void DrWavTotalPCMFrameCount()
        {
            var files = new []
            {
                new { File = "sin_1000Hz_-3dBFS_3s_8khz.wav",     Answer = (ulong)24001 },
                new { File = "sin_1000Hz_-3dBFS_3s_16khz.wav",    Answer = (ulong)48001 },
                new { File = "sin_1000Hz_-3dBFS_3s_22.05khz.wav", Answer = (ulong)66151 },
                new { File = "sin_1000Hz_-3dBFS_3s_32khz.wav",    Answer = (ulong)96001 },
                new { File = "sin_1000Hz_-3dBFS_3s_44.1khz.wav",  Answer = (ulong)132301 },
                new { File = "sin_1000Hz_-3dBFS_3s_48khz.wav",    Answer = (ulong)144001 }
            };

            foreach (var file in files)
            {
                var drwav = new DrWav();

                var path = this.GetDataFile(file.File).FullName;
                Assert.True(drwav.InitFile(path));

                Assert.Equal(file.Answer, drwav.TotalPCMFrameCount);
                
                drwav.Dispose();
           
                Assert.Throws<ObjectDisposedException>(() => { var _ = drwav.TotalPCMFrameCount; });
            }
        }

    }

}
