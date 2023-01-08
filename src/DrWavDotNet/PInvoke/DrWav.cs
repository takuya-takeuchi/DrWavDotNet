using System;
using System.Runtime.InteropServices;
using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using int64_t = System.Int64;
using int8_t = System.SByte;
using int16_t = System.Int16;
using int32_t = System.Int32;

using drwav = System.IntPtr;
using drwav_int16 = System.Int16;
using drwav_int32 = System.Int32;
using drwav_uint16 = System.UInt16;
using drwav_uint32 = System.UInt32;
using drwav_uint64 = System.UInt64;
using drwav_bool32 = System.UInt32;
using drwav_result = System.Int32;

// ReSharper disable once CheckNamespace
namespace DrWavDotNet
{

    internal sealed partial class NativeMethods
    {

        internal enum ErrorType
        {

            OK = 0x00000000,

            #region General

            GeneralError = 0x76000000,

            GeneralFileIOError      = -(GeneralError | 0x00000001),

            GeneralOutOfRange       = -(GeneralError | 0x00000002),

            #endregion

        }

        #region Fields
        
        public const drwav_bool32 DRWAV_TRUE = 1;
        public const drwav_bool32 DRWAV_FALSE = 0;

        #endregion

        #region Structures

        #region drwav

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav drwav_drwav_new();

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern void drwav_drwav_delete(drwav wav);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint16 drwav_drwav_init_params_get_channels(drwav wav);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint32 drwav_drwav_init_params_get_sampleRate(drwav wav);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint16 drwav_drwav_init_params_get_bitsPerSample(drwav wav);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint64 drwav_drwav_init_params_get_totalPCMFrameCount(drwav wav);

        #endregion

        #endregion

        #region Functions

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_bool32 drwav_drwav_init_file(drwav wav,
                                                                uint8_t[] filename,
                                                                uint32_t filename_length,
                                                                IntPtr pAllocationCallback);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_result drwav_drwav_uninit(drwav wav);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint64 drwav_drwav_read_pcm_frames_s16(drwav wav,
                                                                          drwav_uint64 framesToRead,
                                                                          drwav_int16[] pBufferOut);

        [DllImport(NativeLibrary, CallingConvention = CallingConvention)]
        public static extern drwav_uint64 drwav_drwav_read_pcm_frames_s16(drwav wav,
                                                                          drwav_uint64 framesToRead,
                                                                          IntPtr pBufferOut);

        #endregion

    }

}