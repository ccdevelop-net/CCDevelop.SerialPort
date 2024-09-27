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
using System.Collections.Generic;

namespace CCDevelop.SerialPort.Linux.Helpers {
  internal static class SttyParameters {
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get list all TTY terminal
    /// </summary>
    /// <returns>Return list of terminal</returns>
    public static IEnumerable<string> GetListAllTtyParam() {
      yield return "-a";
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get terminal TTY parameter of a specific port
    /// </summary>
    /// <param name="port">Requaired port</param>
    /// <returns>List of parameter</returns>
    public static IEnumerable<string> GetPortTtyParam(string port) {
      yield return $"-F {port}";
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get sane TTY parameter
    /// </summary>
    /// <returns>List of sane parameter</returns>
    public static IEnumerable<string> GetSaneModeTtyParam() {
      // sane is a composite command that sets:
      //
      // cread -ignbrk brkint -inlcr -igncr icrnl -iutf8 -ixoff -iuclc -ixany imaxbel opost
      // -olcuc -ocrnl onlcr -onocr -onlret -ofill -ofdel nl0 cr0 tab0 bs0 vt0 ff0 isig icanon iexten
      // echo echoe echok -echonl -noflsh -xcase -tostop -echoprt echoctl echoke
      //
      // as well as "special characters" to their default values
      yield return "sane";
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get list of raw parameter
    /// </summary>
    /// <param name="rawEnabled">Flag of raw enabled</param>
    /// <returns>List of raw parameter</returns>
    public static IEnumerable<string> GetRawModeTtyParam(bool rawEnabled) {
      if (rawEnabled) {
        // raw is a composite command that sets:
        //
        // -ignbrk -brkint -ignpar -parmrk -inpck -istrip -inlcr -igncr -icrnl -ixon -ixoff
        // -iuclc -ixany -imaxbel -opost -isig -icanon -xcase min 1 time 0
        yield return "raw";

        // Unfortunately, the raw parameter on its own doesn't set enough parameters to
        // actually get the tty to anywhere near a true byte in, byte out raw serial socket.
        // Remove echo and other things that will get in the way of reading raw data how we expect.

        // Don't send a hangup signal when the last process closes the tty
        yield return "-hupcl";

        // Disable modem control signals (in the negative sense. -clocal actually enables modem control signals).
        yield return "clocal";

        // Don't enable non-POSIX special characters
        yield return "-iexten";

        // Don't echo erase characters as backspace-space-backspace
        yield return "-echo";

        // Don't echo erase characters as backspace-space-backspace
        yield return "-echoe";

        // Don't echo a newline after a kill characters
        yield return "-echok";

        // Don't echo newline even if not echoing other characters
        yield return "-echonl";

        // Don't echo erased characters backward, between '\' and '/'
        yield return "-echoprt";

        // Don't echo control characters in hat notation ('^c')
        yield return "-echoctl";

        // Kill all line by obeying the echoctl and echok settings
        yield return "-echoke";
      } else {
        yield return "-raw";
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Enable or disable Drain parameters
    /// </summary>
    /// <param name="drainEnabled">Flag of drain enabled</param>
    /// <returns>Return list string of the values</returns>
    public static IEnumerable<string> GetDrainTtyParam(bool drainEnabled) {
      if (drainEnabled) {
        yield return "drain";
      } else {
        yield return "-drain";
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrive baudrate string
    /// </summary>
    /// <param name="baudRate">Baud to be retrived</param>
    /// <returns>Return list string of the values</returns>
    public static IEnumerable<string> GetBaudTtyParam(int baudRate) {
      yield return $"{baudRate}";
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Timeout on each read. Time is in tenths of a second, 1 = 100ms
    /// </summary>
    /// <param name="readTimeout">Read timeout</param>
    /// <returns>Return list string of the values</returns>
    public static IEnumerable<string> GetReadTimeoutTtyParam(int readTimeout) {
      yield return
        $"time {(readTimeout + 50) / 100}"; // 
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Minimum bytes that can be read out of the stream
    /// </summary>
    /// <param name="byteCount">Number of byte</param>
    /// <returns>Return list string of the values</returns>
    public static IEnumerable<string> GetMinDataTtyParam(int byteCount) {
      yield return $"min {byteCount}"; 
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Translate handshake mode
    /// </summary>
    /// <param name="handshake">Handshake mode to translate</param>
    /// <returns>Return list string of the values</returns>
    /// <exception cref="InvalidOperationException">Parameter provided is invalid</exception>
    public static IEnumerable<string> GetHandshakeTtyParams(Abstractions.Enums.Handshake handshake) {
      switch (handshake) {
        case Abstractions.Enums.Handshake.None:
          yield return "-crtscts";
          yield return "-ixoff";
          yield return "-ixon";
          yield break;
        case Abstractions.Enums.Handshake.RequestToSend:
          yield return "crtscts";
          yield return "-ixoff";
          yield return "-ixon";
          yield break;
        case Abstractions.Enums.Handshake.XOnXOff:
          yield return "-crtscts";
          yield return "ixoff";
          yield return "ixon";
          yield break;
        case Abstractions.Enums.Handshake.RequestToSendXOnXOff:
          yield return "crtscts";
          yield return "ixoff";
          yield return "ixon";
          yield break;
        default:
          throw new InvalidOperationException($"Invalid Handshake {handshake}");
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get parity translation
    /// </summary>
    /// <param name="parity" ><see cref="CCDevelop.SerialPort.Abstractions.Enums.Parity"/>Parity to translate</param>
    /// <returns>Return list string of the values</returns>
    /// <exception cref="InvalidOperationException">Parameter provided is invalid</exception>
    public static IEnumerable<string> GetParityTtyParams(Abstractions.Enums.Parity parity) {
      switch (parity) {
        case Abstractions.Enums.Parity.None:
          yield return "-parenb";
          yield return "-cmspar";
          yield break;
        case Abstractions.Enums.Parity.Odd:
          yield return "parenb";
          yield return "-cmspar";
          yield return "parodd";
          yield break;
        case Abstractions.Enums.Parity.Even:
          yield return "parenb";
          yield return "-cmspar";
          yield return "-parodd";
          yield break;
        case Abstractions.Enums.Parity.Mark:
          yield return "-parenb";
          yield return "cmspar";
          yield return "parodd";
          yield break;
        case Abstractions.Enums.Parity.Space:
          yield return "-parenb";
          yield return "cmspar";
          yield return "-parodd";
          yield break;
        default:
          throw new InvalidOperationException($"Invalid Parity {parity}");
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataBits">Databits to translate</param>
    /// <returns>Return list string of the values</returns>
    /// <exception cref="ArgumentOutOfRangeException">Provided parameter is out of range</exception>
    public static IEnumerable<string> GetDataBitsTtyParam(int dataBits) {
      if (dataBits < 5 || dataBits > 8) {
        throw new ArgumentOutOfRangeException(nameof(dataBits), $"{nameof(dataBits)} must be between 5 and 8");
      }

      yield return $"cs{dataBits}";
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrive stopbits translation
    /// </summary>
    /// <param name="stopBits"><see cref="CCDevelop.SerialPort.Abstractions.Enums.StopBits"/>Stop bits to convert</param>
    /// <returns>Return list string of the values</returns>
    /// <exception cref="InvalidOperationException">Invalid provided parameters</exception>
    public static IEnumerable<string> GetStopBitsTtyParam(Abstractions.Enums.StopBits stopBits) {
      switch (stopBits) {
        //=================================================
        case Abstractions.Enums.StopBits.None: {
          throw new InvalidOperationException($"Stop bits cannot be set to {Abstractions.Enums.StopBits.None}");
        }
        //=================================================
        case Abstractions.Enums.StopBits.One: {
          yield return "-cstopb";
          yield break;
        }
        //=================================================
        case Abstractions.Enums.StopBits.OnePointFive: {
          throw new InvalidOperationException($"Stop bits cannot be set to {Abstractions.Enums.StopBits.OnePointFive}");
        }
        //=================================================
        case Abstractions.Enums.StopBits.Two: {
          yield return "cstopb";
          yield break;
        }
        //=================================================
        default: {
          throw new InvalidOperationException($"Invalid StopBits {stopBits}");
        }
        //=================================================
      }
    }
    //------------------------------------------------------------------------------------------------------------------
  }
}