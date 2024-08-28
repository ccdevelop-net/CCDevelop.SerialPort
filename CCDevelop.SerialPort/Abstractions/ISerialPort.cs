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