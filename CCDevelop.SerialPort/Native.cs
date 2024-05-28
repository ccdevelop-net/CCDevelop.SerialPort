using System;
using System.Runtime.InteropServices;

namespace CCDevelop.SerialPort {
  internal static unsafe class Native {
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Init(int * fd);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Open", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)] 
    public static extern int Open(sbyte * device, uint bauds, DataBits databits, Parity parity, StopBits stopbits);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "IsOpen", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I1)] 
    public static extern bool IsOpen(int fd);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Close", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Close(int fd);
    
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "WriteByte", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int WriteByte(int fd, byte byteTx);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "ReadByte", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int ReadByte(int fd, byte * data, uint timeoutMs);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "WriteString", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int WriteString(int fd, sbyte * str);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "ReadString", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int ReadString(int fd, sbyte * receivedString, char finalChar, uint maxNbBytes, uint timeoutMs);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "WriteBytes", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int WriteBytes(int fd, byte * buffer, uint nbBytes);

    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "ReadBytes", CallingConvention = CallingConvention.Cdecl)]
    [return:MarshalAs(UnmanagedType.I4)]
    public static extern int ReadBytes(int fd, byte * buffer, uint maxNbBytes, uint timeoutMs, uint sleepDurationUs);

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