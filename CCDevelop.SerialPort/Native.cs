using System;
using System.Runtime.InteropServices;

namespace CCDevelop.SerialPort {
  internal static class Native {
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Init(ref int fd);

    //extern int Open(const char * device, const uint bauds, SerialDataBits databits, SerialParity parity, SerialStopBits stopbits);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsOpen", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)] 
    public static extern bool IsOpen(int fd);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Close", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Close(int fd);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "WriteByte", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int WriteByte(int fd, byte byteTx);

    //extern int ReadByte(int32_t fd, uint8_t * data, const uint32_t timeoutMS);

    //extern int WriteString(int32_t fd, const char * str);

    //extern int ReadString(int32_t fd, char * receivedString, char finalChar, uint maxNbBytes, const uint timeoutMS);

    //extern int WriteBytes(int32_t fd, const uint8_t * buffer, const uint32_t nbBytes);

    //extern int ReadBytes(int fd, uint8_t * buffer, uint maxNbBytes, const uint timeoutMS, uint sleepDurationUs);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "FlushReceiver", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool FlushReceiver(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Available", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int Available(int fd);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "DTR", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool DTR(int fd, bool status);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "SetDTR", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool SetDTR(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "ClearDTR", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool ClearDTR(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "RTS", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool RTS(int fd, bool status);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "SetRTS", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool SetRTS(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "ClearRTS", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool ClearRTS(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsRI", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsRI(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsDCD", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsDCD(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsCTS", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsCTS(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsDSR", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsDSR(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsDTR", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsDTR(int fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsRTS", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)]
    public static extern bool IsRTS(int fd);

  }
}