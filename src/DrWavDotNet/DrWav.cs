using System;
using System.Text;

using drwav_int16 = System.Int16;
using drwav_int32 = System.Int32;
using drwav_uint16 = System.UInt16;
using drwav_uint32 = System.UInt32;
using drwav_uint64 = System.UInt64;
using drwav_bool32 = System.UInt32;
using drwav_result = System.Int32;

namespace DrWavDotNet
{

    public sealed class DrWav : DrWavObject
    {

        #region Constructors

        public DrWav()
        {
            this.NativePtr = NativeMethods.drwav_drwav_new();
        }

        #endregion

        #region Methods

        public bool InitFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            this.ThrowIfDisposed();

            var str = Encoding.GetBytes(filePath);
            return NativeMethods.drwav_drwav_init_file(this.NativePtr, str, (uint)str.Length, IntPtr.Zero) == NativeMethods.DRWAV_TRUE;
        }

        public bool InitMemory(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            this.ThrowIfDisposed();

            return NativeMethods.drwav_drwav_init_memory(this.NativePtr, data, (long)data.Length, IntPtr.Zero) == NativeMethods.DRWAV_TRUE;
        }

        public bool InitMemory(IntPtr data, uint length)
        {
            if (data == IntPtr.Zero)
                throw new ArgumentException($"{nameof(data)} must be not {nameof(IntPtr)}.{nameof(IntPtr.Zero)}", nameof(data));

            this.ThrowIfDisposed();

            return NativeMethods.drwav_drwav_init_memory(this.NativePtr, data, (long)length, IntPtr.Zero) == NativeMethods.DRWAV_TRUE;
        }

        public DrWavResult Uninit()
        {
            this.ThrowIfDisposed();

            return (DrWavResult)NativeMethods.drwav_drwav_uninit(this.NativePtr);
        }

        public ulong ReadPcmFramesS16(uint framesToRead, short[] bufferOut)
        {
            if (bufferOut == null)
                throw new ArgumentNullException(nameof(bufferOut));
            if ((ulong)bufferOut.Length < framesToRead)
                throw new ArgumentOutOfRangeException($"{nameof(framesToRead)} must be equal or less than length of {nameof(bufferOut)}");

            this.ThrowIfDisposed();

            return NativeMethods.drwav_drwav_read_pcm_frames_s16(this.NativePtr, framesToRead, bufferOut);
        }

        public ulong ReadPcmFramesS16(ulong framesToRead, IntPtr bufferOut)
        {
            if (bufferOut == IntPtr.Zero)
                throw new ArgumentException($"{nameof(bufferOut)} must be not {nameof(IntPtr)}.{nameof(IntPtr.Zero)}", nameof(bufferOut));

            this.ThrowIfDisposed();

            return NativeMethods.drwav_drwav_read_pcm_frames_s16(this.NativePtr, framesToRead, bufferOut);
        }

        #endregion

        #region Properties

        public drwav_uint16 BitsPerSample
        {
            get
            {
                this.ThrowIfDisposed();
                return NativeMethods.drwav_drwav_init_params_get_bitsPerSample(this.NativePtr);
            }
        }

        public drwav_uint16 Channels
        {
            get
            {
                this.ThrowIfDisposed();
                return NativeMethods.drwav_drwav_init_params_get_channels(this.NativePtr);
            }
        }

        public drwav_uint32 SampleRate
        {
            get
            {
                this.ThrowIfDisposed();
                return NativeMethods.drwav_drwav_init_params_get_sampleRate(this.NativePtr);
            }
        }

        public drwav_uint64 TotalPCMFrameCount
        {
            get
            {
                this.ThrowIfDisposed();
                return NativeMethods.drwav_drwav_init_params_get_totalPCMFrameCount(this.NativePtr);
            }
        }

        private static Encoding _Encoding = Encoding.UTF8;

        public static Encoding Encoding
        {
            get => _Encoding;
            set => _Encoding = value ?? Encoding.UTF8;
        }

        #endregion

        #region Methods

        #region Overrides 

        /// <summary>
        /// Releases all unmanaged resources.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();

            if (this.NativePtr == IntPtr.Zero)
                return;

            NativeMethods.drwav_drwav_delete(this.NativePtr);
        }

        #endregion

        #endregion

    }

}