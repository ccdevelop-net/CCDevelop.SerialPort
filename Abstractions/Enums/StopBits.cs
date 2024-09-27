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

namespace CCDevelop.SerialPort.Abstractions.Enums {
  /// <summary>
  /// Serial stop bits
  /// </summary>
  public enum StopBits {
    /// <summary>
    /// No stop bit
    /// </summary>
    None         = 0,
    /// <summary>
    /// One stop bit
    /// </summary>
    One          = 1,
    /// <summary>
    /// Two stop bits
    /// </summary>
    Two          = 2,
    /// <summary>
    /// One and half stop bit
    /// </summary>
    OnePointFive = 3
  }
}