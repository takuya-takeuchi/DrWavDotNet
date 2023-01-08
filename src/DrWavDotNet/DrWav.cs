using System;
using System.Text;

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
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            this.ThrowIfDisposed();

            var str = Encoding.GetBytes(filePath);
            return NativeMethods.drwav_drwav_init_file(this.NativePtr, str, (uint)str.Length, IntPtr.Zero) == NativeMethods.DRWAV_TRUE;
        }

        public DrWavResult Uninit()
        {
            this.ThrowIfDisposed();

            return (DrWavResult)NativeMethods.drwav_drwav_uninit(this.NativePtr);
        }

        public ulong ReadPcmFramesS16(ulong framesToRead, short[] pBufferOut)
        {
            this.ThrowIfDisposed();

            if ((ulong)pBufferOut.Length < framesToRead)
                throw new ArgumentOutOfRangeException($"{nameof(framesToRead)} must be equal or less than length of {nameof(pBufferOut)}");

            return NativeMethods.drwav_drwav_read_pcm_frames_s16(this.NativePtr, framesToRead, pBufferOut);
        }

        public ulong ReadPcmFramesS16(ulong framesToRead, IntPtr pBufferOut)
        {
            this.ThrowIfDisposed();

            return NativeMethods.drwav_drwav_read_pcm_frames_s16(this.NativePtr, framesToRead, pBufferOut);
        }

        #endregion

        #region Properties

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