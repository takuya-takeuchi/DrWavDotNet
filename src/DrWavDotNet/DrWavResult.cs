namespace DrWavDotNet
{

    public enum DrWavResult
    {

        Success = 0,

        Error = -1,

        InvalidArgs = -2,

        InvalidOperation = -3,

        OutOfMemory = -4,

        OutOfRange = -5,

        AccessDenied = -6,

        DoesNotExist = -7,

        AlreadyExists = -8,

        TooManyOpenFiles = -9,

        InvalidFile = -10,

        TooBig = -11,

        PathTooLong = -12,

        NameTooLong = -13,

        NotDirectory = -14,

        IsDirectory = -15,

        DirectoryNotEmpty = -16,

        EndOfFile = -17,

        NoSpace = -18,

        Busy = -19,

        IoError = -20,

        Interrupt = -21,

        Unavailable = -22,

        AlreadyInUse = -23,

        BadAddress = -24,

        BadSeek = -25,

        BadPipe = -26,

        Deadlock = -27,

        TooManyLinks = -28,

        NotImplemented = -29,

        NoMessage = -30,

        BadMessage = -31,

        NoDataAvailable = -32,

        InvalidData = -33,

        Timeout = -34,

        NoNetwork = -35,

        NotUnique = -36,

        NotSocket = -37,

        NoAddress = -38,

        BadProtocol = -39,

        ProtocolUnavailable = -40,

        ProtocolNotSupported = -41,

        ProtocolFamilyNotSupported = -42,

        AddressFamilyNotSupported = -43,

        SocketNotSupported = -44,

        ConnectionReset = -45,

        AlreadyConnected = -46,

        NotConnected = -47,

        ConnectionRefused = -48,

        NoHost = -49,

        InProgress = -50,

        Cancelled = -51,

        MemoryAlreadyMapped = -52,

        AtEnd = -53

    }

}