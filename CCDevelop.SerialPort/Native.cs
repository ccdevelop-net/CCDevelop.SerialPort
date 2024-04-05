using System;
using System.Runtime.InteropServices;

namespace CCDevelop.SerialPort {
  internal static class Native {
    [DllImport("libCCDevelop.SerialPortDriver.so", EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Init(ref int fd);
  }
}