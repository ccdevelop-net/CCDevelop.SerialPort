// CCDevelop - Serial port library for Linux and Windows
// Copyright (C) 2024 - Cristian Croci
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CCDevelop.SerialPort.Abstractions {
  /// <summary>
  /// Interface for a serial port
  /// </summary>
  public interface ISerialPort : IDisposable {
    /// <summary>
    /// Base stream of the serial <see cref="System.IO.Stream"/>
    /// </summary>
    Stream          BaseStream  { get; }
    /// <summary>
    /// Baudrate of the serial
    /// Values:
    /// </summary>
    /// <list type="bullet">
    ///   <listheader>
    ///     <term>
    ///       Baudrates
    ///     </term>
    ///   </listheader>
    ///   <item>
    ///     4800
    ///   </item>
    ///   <item>
    ///     9600
    ///   </item>
    ///   <item>
    ///     19200
    ///   </item>
    ///   <item>
    ///     38400
    ///   </item>
    ///   <item>
    ///     57600
    ///   </item>
    ///   <item>
    ///     115200
    ///   </item>
    ///   <item>
    ///     230400
    ///   </item>
    ///   <item>
    ///     460800
    ///   </item>
    ///   <item>
    ///     921600
    ///   </item>
    /// </list>
    int             BaudRate    { get; set; }
    /// <summary>
    /// Serial data bits
    /// </summary>
    int             DataBits    { get; set; }
    /// <summary>
    /// Serial handshake type <see cref="CCDevelop.SerialPort.Abstractions.Enums.Handshake"/>
    /// </summary>
    Enums.Handshake Handshake   { get; set; }
    /// <summary>
    /// Check is serial is opened
    /// </summary>
    bool            IsOpen      { get; }
    /// <summary>
    /// Serial parity type <see cref="CCDevelop.SerialPort.Abstractions.Enums.Parity"/>
    /// </summary>
    Enums.Parity    Parity      { get; set; }
    /// <summary>
    /// Serial port name
    /// </summary>
    string          PortName    { get; }
    /// <summary>
    /// Serial reading timeout in milliseconds
    /// </summary>
    int             ReadTimeout { get; set; }
    /// <summary>
    /// Serial stop bits type <see cref="CCDevelop.SerialPort.Abstractions.Enums.StopBits"/>
    /// </summary>    
    Enums.StopBits  StopBits    { get; set; }

    /// <summary>
    /// Open serial port
    /// </summary>
    void   Open();
    /// <summary>
    /// Close serial port
    /// </summary>
    void   Close();
    /// <summary>
    /// Read serial object
    /// </summary>
    /// <returns>Return serial object</returns>
    string ToString();
    /// <summary>
    /// Discard data in received buffer
    /// </summary>
    void   DiscardInBuffer();
    /// <summary>
    /// Discard data in received buffer
    /// </summary>
    /// <returns>Returns task that execute operation</returns>
    Task   DiscardInBufferAsync(CancellationToken token);
    /// <summary>
    /// Discard data in transmit buffer
    /// </summary>
    void   DiscardOutBuffer();
    /// <summary>
    /// Discard data in transmit buffer, in asyncronus mode
    /// </summary>
    /// <returns>Returns task that execute operation</returns>
    Task   DiscardOutBufferAsync(CancellationToken token);
  }
}