#ifndef _CPP_SHARED_H_
#define _CPP_SHARED_H_

#include <cstdint>
#include <string>
#include <string.h>
#include <vector>

#define ERR_OK                                                            0x00000000

// General
#define ERR_GENERAL_ERROR                                                 0x76000000
#define ERR_GENERAL_FILE_IO                         -(ERR_GENERAL_ERROR | 0x00000001)
#define ERR_GENERAL_OUT_OF_RANGE                    -(ERR_GENERAL_ERROR | 0x00000002)

#endif