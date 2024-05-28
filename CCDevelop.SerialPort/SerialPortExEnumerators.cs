// ============================================================================
// This file is part of SerialPortEnhanced project
// (https://github.com/cristianc1972/SerialPort-Enhanced)
// Copyright (C) 2024 Cristian Croci
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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ===========================================================================
// File        : SerialPortExEnumerators.cs
// Author      : Cristian Croci
// Date        : Thu, 29 Feb 2024 14:21:25 +0100
// Description : Serial Port Extended Enumerators
// Notes       :
// ===========================================================================
// Change History :
//     2024-02-29 - First Version
// ===========================================================================
namespace CCDevelop.SerialPort {
  /**
   * DataBits enumerator
   */
  public enum DataBits : int {
    Five = 5, // >> Five Bits
    Six,      // >> Six Bits
    Seven,    // >> Seven Bits
    Eight,    // >> Eight Bits
    Nine,     // >> Nine Bits
  }
  
  /**
   * @brief Number of serial stop bits
   */
  public enum StopBits : int {
    StopBits1 = 0,  //**< 1 stop bit
    StopBits15,     //**< 1.5 stop bits
    StopBits2,      //**< 2 stop bits
  }

  /**
   * @brief Type of serial parity bits
   */
  public enum Parity : int {
    ParityNone = 0, //**< No parity bit
    ParityEven,     //**< Even parity bit
    ParityOdd,      //**< Odd parity bit
    ParityMark,     //**< Mark parity
    ParitySpace     //**< Space bit
  }
}