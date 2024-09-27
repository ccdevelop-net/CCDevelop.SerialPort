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
using CCDevelop.SerialPort.Abstractions;

using Handshake = CCDevelop.SerialPort.Abstractions.Enums.Handshake;
using Parity = CCDevelop.SerialPort.Abstractions.Enums.Parity;
using StopBits = CCDevelop.SerialPort.Abstractions.Enums.StopBits;

namespace CCDevelop.SerialPort.Windows {
  /// <summary>
  /// Wrapper for System.IO.Ports.SerialPort to interface it to the ISerialPort interface
  /// </summary>
  public class WindowsSerialPort : ISerialPort {
    #region PUBLIC - Constants
    /// <summary>
    /// The value representing an infinite timout on the serial port.
    /// </summary>
    public const int InfiniteTimeout = System.IO.Ports.SerialPort.InfiniteTimeout;
    #endregion

    #region PRIVARE - Variables
    // Serial port
    private System.IO.Ports.SerialPort _serialPort;
    #endregion

    /// <summary>
    /// Class that manage serial port for Windows systems
    /// </summary>
    /// <param name="serialPort">Serial port object <see cref="System.IO.Ports.SerialPort"/></param>
    /// <exception cref="ArgumentNullException">Serial port not provided</exception>
    public WindowsSerialPort(System.IO.Ports.SerialPort serialPort) {
      _serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
    }

    #region PUBLIC - Properties
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Base stram of the serial <see cref="System.IO.Stream"/>
    /// </summary>
    public Stream BaseStream => _serialPort.BaseStream;
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Set and get serial baudrate 
    /// </summary>
    public int BaudRate {
      get => _serialPort.BaudRate;
      set => _serialPort.BaudRate = value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Set and get data bits
    /// </summary>
    public int DataBits {
      get => _serialPort.DataBits;
      set => _serialPort.DataBits = value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Set and get serial handshake <see cref="CCDevelop.SerialPort.Abstractions.Enums.Handshake"/>
    /// </summary>
    public Handshake Handshake {
      get => (Handshake)_serialPort.Handshake;
      set => _serialPort.Handshake = (System.IO.Ports.Handshake)value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Check if serial is open
    /// </summary>
    public bool IsOpen => _serialPort.IsOpen;
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get serial port name
    /// </summary>
    public string PortName => _serialPort.PortName;
    //------------------------------------------------------------------------------------------------------------------
    #endregion

    #region PUBLIC - Functions
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get and set serial parity <see cref="CCDevelop.SerialPort.Abstractions.Enums.Parity"/>
    /// </summary>
    public Parity Parity {
      get => (Parity)_serialPort.Parity;
      set => _serialPort.Parity = (System.IO.Ports.Parity)value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Serial read timeout
    /// </summary>
    public int ReadTimeout {
      get => _serialPort.ReadTimeout;
      set => _serialPort.ReadTimeout = value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Set and get serial stop bits <see cref="CCDevelop.SerialPort.Abstractions.Enums.StopBits"/>
    /// </summary>
    public StopBits StopBits {
      get => (StopBits)_serialPort.StopBits;
      set => _serialPort.StopBits = (System.IO.Ports.StopBits)value;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Close serial port
    /// </summary>
    public void Close() {
      _serialPort.Close();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discard data in received buffer
    /// </summary>
    public void DiscardInBuffer() {
      _serialPort.DiscardInBuffer();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discard data in received buffer
    /// </summary>
    /// <returns>Returns task that execute operation</returns>
    public Task DiscardInBufferAsync(CancellationToken token) {
      _serialPort.DiscardInBuffer();
      return Task.CompletedTask;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discard data in transmit buffer
    /// </summary>
    public void DiscardOutBuffer() {
      _serialPort.DiscardOutBuffer();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discard data in transmit buffer, in asyncronus mode
    /// </summary>
    /// <returns>Returns task that execute operation</returns>
    public Task DiscardOutBufferAsync(CancellationToken token) {
      _serialPort.DiscardOutBuffer();
      return Task.CompletedTask;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Dispose serial port
    /// </summary>
    public void Dispose() {
      _serialPort.Dispose();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Open serial port
    /// </summary>
    public void Open() {
      _serialPort.Open();
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion
  }
}