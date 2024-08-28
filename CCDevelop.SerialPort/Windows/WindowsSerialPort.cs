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
    /// <summary>
    /// The value representing an infinite timout on the serial port.
    /// </summary>
    public const int InfiniteTimeout = System.IO.Ports.SerialPort.InfiniteTimeout;

    private System.IO.Ports.SerialPort _serialPort;

    public WindowsSerialPort(System.IO.Ports.SerialPort serialPort) {
      _serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
    }

    public Stream BaseStream => _serialPort.BaseStream;

    public int BaudRate {
      get => _serialPort.BaudRate;
      set => _serialPort.BaudRate = value;
    }

    public int DataBits {
      get => _serialPort.DataBits;
      set => _serialPort.DataBits = value;
    }

    public Handshake Handshake {
      get => (Handshake)_serialPort.Handshake;
      set => _serialPort.Handshake = (System.IO.Ports.Handshake)value;
    }

    public bool IsOpen => _serialPort.IsOpen;

    public Parity Parity {
      get => (Parity)_serialPort.Parity;
      set => _serialPort.Parity = (System.IO.Ports.Parity)value;
    }

    public string PortName => _serialPort.PortName;

    public int ReadTimeout {
      get => _serialPort.ReadTimeout;
      set => _serialPort.ReadTimeout = value;
    }

    public StopBits StopBits {
      get => (StopBits)_serialPort.StopBits;
      set => _serialPort.StopBits = (System.IO.Ports.StopBits)value;
    }

    public void Close() {
      _serialPort.Close();
    }

    public void DiscardInBuffer() {
      _serialPort.DiscardInBuffer();
    }

    public Task DiscardInBufferAsync(CancellationToken token) {
      _serialPort.DiscardInBuffer();
      return Task.CompletedTask;
    }

    public void DiscardOutBuffer() {
      _serialPort.DiscardOutBuffer();
    }

    public Task DiscardOutBufferAsync(CancellationToken token) {
      _serialPort.DiscardOutBuffer();
      return Task.CompletedTask;
    }

    public void Dispose() {
      _serialPort.Dispose();
    }

    public void Open() {
      _serialPort.Open();
    }
  }
}