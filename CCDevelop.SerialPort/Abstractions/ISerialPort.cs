using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CCDevelop.SerialPort.Abstractions {
  public interface ISerialPort : IDisposable {
    Stream          BaseStream  { get; }
    int             BaudRate    { get; set; }
    int             DataBits    { get; set; }
    Enums.Handshake Handshake   { get; set; }
    bool            IsOpen      { get; }
    Enums.Parity    Parity      { get; set; }
    string          PortName    { get; }
    int             ReadTimeout { get; set; }
    Enums.StopBits  StopBits    { get; set; }

    void   Open();
    void   Close();
    string ToString();
    void   DiscardInBuffer();
    Task   DiscardInBufferAsync(CancellationToken token);
    void   DiscardOutBuffer();
    Task   DiscardOutBufferAsync(CancellationToken token);
  }
}