#ifndef _CPP_DRWAV_H_
#define _CPP_DRWAV_H_

#include "shared.hpp"
#include "export.hpp"

#include <dr_wav.h>

#pragma region structs

DLLEXPORT drwav* drwav_drwav_new()
{
    return new drwav();
}

DLLEXPORT void drwav_drwav_delete(const drwav* wav)
{
    delete wav;
}

DLLEXPORT drwav_uint16 drwav_drwav_init_params_get_channels(drwav* const wav)
{
    return wav->channels;
}

DLLEXPORT drwav_uint32 drwav_drwav_init_params_get_sampleRate(drwav* const wav)
{
    return wav->sampleRate;
}

DLLEXPORT drwav_uint16 drwav_drwav_init_params_get_bitsPerSample(drwav* const wav)
{
    return wav->bitsPerSample;
}

DLLEXPORT drwav_uint64 drwav_drwav_init_params_get_totalPCMFrameCount(drwav* const wav)
{
    return wav->totalPCMFrameCount;
}

#pragma endregion structs

#pragma region functions

DLLEXPORT drwav_bool32 drwav_drwav_init_file(drwav* const wav,
                                             const char* filename,
                                             const uint32_t filename_length,
                                             const drwav_allocation_callbacks* pAllocationCallback)
{
    std::string tmp(filename, filename_length);
    return ::drwav_init_file(wav, tmp.c_str(), pAllocationCallback);
}

DLLEXPORT drwav_bool32 drwav_drwav_init_memory(drwav* const wav,
                                               const void* data,
                                               const size_t dataSize,
                                               const drwav_allocation_callbacks* pAllocationCallback)
{
    return ::drwav_init_memory(wav, data, dataSize, pAllocationCallback);
}

DLLEXPORT drwav_result drwav_drwav_uninit(drwav* const wav)
{
    return ::drwav_uninit(wav);
}

DLLEXPORT drwav_uint64 drwav_drwav_read_pcm_frames_s16(drwav* const wav,
                                                       const drwav_uint64 framesToRead,
                                                       drwav_int16* const pBufferOut)
{
    return ::drwav_read_pcm_frames_s16(wav, framesToRead, pBufferOut);
}

#pragma endregion functions

#endif // _CPP_STRING_H_
