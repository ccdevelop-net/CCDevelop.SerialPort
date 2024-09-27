﻿// CCDevelop - Serial port library for Linux and Windows
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

namespace CCDevelop.SerialPort.Abstractions.Enums {
  /// <summary>
  /// Serial handshake
  /// </summary>
  public enum Handshake {
    /// <summary>
    /// No handhake
    /// </summary>
    None                 = 0,
    /// <summary>
    /// XOn and XOff handshake
    /// </summary>
    XOnXOff              = 1,
    /// <summary>
    /// Requanst to Send handshake
    /// </summary>
    RequestToSend        = 2,
    /// <summary>
    /// Requanst to Send nad XOn/XOff handshake
    /// </summary>
    RequestToSendXOnXOff = 3
  }
}