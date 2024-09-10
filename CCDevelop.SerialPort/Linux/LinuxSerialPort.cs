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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CCDevelop.SerialPort.Abstractions;
using CCDevelop.SerialPort.Abstractions.Enums;
using static CCDevelop.SerialPort.Linux.Helpers.SttyExecution;
using static CCDevelop.SerialPort.Linux.Helpers.SttyParameters;

namespace CCDevelop.SerialPort.Linux {
  public class SerialPortInfo {
    public string Name        { get; set; }
    public string Description { get; set; }
  }
  
  /// <summary>
  /// A serial port implementation for POSIX style systems that have /bin/stty available.
  /// </summary>
  public class LinuxSerialPort : ISerialPort {
    #region PUBLIC - Constants
    /// <summary>
    /// The value representing an infinite timout on the serial port.
    /// </summary>
    public const int InfiniteTimeout = 0;
    #endregion

    #region PRIVARE - Variables
    // The original port path is the path passed into the constructor
    // when the serial port is created. This value may contain wildcards,
    // and never changes.
    //
    private readonly string _originalPortPath;

    // Set to true when the port is disposed.
    // After the port is disposed, it cannot be reopened.
    //
    private bool _isDisposed;

    // When the port is opened, this gets set to the filestream of the
    // serial port file, and remains until the port is closed or disposed.
    //
    private FileStream _internalStream;


    // Backing fields for the public serial port properties
    //
    private bool       _enableRawMode;
    private int?       _minimumBytesToRead;
    private int?       _readTimeout;
    private int?       _baudRate;
    private int?       _dataBits;
    private StopBits?  _stopBits;
    private Handshake? _handshake;
    private Parity?    _parity;
    #endregion

    #region Constructors
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of SerialPort for accessing a serial port on the system.
    /// Enables the serial port in raw mode by default.
    /// </summary>
    /// <param name="port">The path of the serial port device, for example /dev/ttyUSB0.
    /// Wildcards are accepted, for example /dev/ttyUSB* will open the first port that matches that path.
    /// </param>
    public LinuxSerialPort(string port) {
      // Check that stty is actually available on this platform before continuing.
      //
      if (!IsPlatformCompatible()) {
        throw new PlatformNotSupportedException("This serial implementation only works on platforms with stty");
      }

      // Set the original port path to whatever value was passed in.
      _originalPortPath = port ?? throw new ArgumentNullException(nameof(port));

      // Also set port path to the original port path.
      // This port path may be changed when the port is actually opened.
      this.PortName = _originalPortPath;

      // Default to raw mode, as this will be the most common use case
      _enableRawMode = true;

      _isDisposed = false;
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion

    #region PUBLIC - Properties
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// True if the serialport has been opened, and the stream is avialable for reading and writing.
    /// </summary>
    public bool IsOpen => _internalStream != null;
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// The path of the opened port.
    /// </summary>
    public string PortName { get; private set; }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// The stream for reading from and writing to the serial port.
    /// </summary>
    public Stream BaseStream {
      get {
        ThrowIfDisposed();
        ThrowIfNotOpen();

        return _internalStream;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Disables as much of the kernel tty layer as possible,
    /// to provide raw serialport like behaviour over the underlying tty.
    /// </summary>
    public bool EnableRawMode {
      get => _enableRawMode;
      set {
        if (IsOpen) {
          // Set the raw mode
          SetTtyOnSerialWithPrefix(GetRawModeTtyParam(value));

          // Only set the backing field after the raw mode was set successfully.
          _enableRawMode = value;

          // Since this command is composite and sets multiple parameters,
          // we must re-commit all other settings back over the top of it once set.
          SetAllSerialParams();
        } else {
          _enableRawMode = value;
        }
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls whether stty will attempt to flush the output buffer before applying serial configuration.
    /// If the stty version installed supports the [-]drain option, it is recommended to set this to false
    /// to avoid potential hangs when opening the serial port.
    /// If stty does not support [-]drain, this should be set to null (default).
    /// </summary>
    public bool? EnableDrain { get; set; } = null;
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// The minimum bytes that must fill the serial read buffer before the Read command
    /// will return. (However, it may still time out and return less than this).
    /// </summary>
    public int MinimumBytesToRead {
      get => _minimumBytesToRead ?? 0;
      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetMinDataTtyParam(value));
        }

        _minimumBytesToRead = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// The maximum amount of time a Read command will block for before returning.
    /// The time is in milliseconds, but is rounded to tenths of a second when passed to stty.
    /// </summary>
    public int ReadTimeout {
      get => _readTimeout ?? 0;
      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetReadTimeoutTtyParam(value));
        }

        _readTimeout = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the baud rate of the serial port.
    /// </summary>
    public int BaudRate {
      get => _baudRate ?? -1;

      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetBaudTtyParam(value));
        }

        _baudRate = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the databits to use for the serial port.
    /// </summary>
    public int DataBits {
      get => _dataBits ?? 8;

      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetDataBitsTtyParam(value));
        }

        _dataBits = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the stopbits to use for the serial port.
    /// </summary>
    public StopBits StopBits {
      get => _stopBits ?? StopBits.One;

      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetStopBitsTtyParam(value));
        }

        _stopBits = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the handshake method to use for the serial port.
    /// </summary>
    public Handshake Handshake {
      get => _handshake ?? Handshake.None;
      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetHandshakeTtyParams(value).ToArray());
        }

        _handshake = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parity to use for the serial port.
    /// </summary>
    public Parity Parity {
      get => _parity ?? Parity.None;
      set {
        if (IsOpen) {
          SetTtyOnSerialWithPrefix(GetParityTtyParams(value).ToArray());
        }

        _parity = value;
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion

    
    #region PUBLIC - Static Functions
    //------------------------------------------------------------------------------------------------------------------
    public static SerialPortInfo[] Ports() {
      // Function Variables
      List<SerialPortInfo> ports   = new List<SerialPortInfo>();
      string[]             serials = null;

      try {
        serials = Directory.GetFiles(@"/dev/serial/by-id");

        foreach (string path in serials) {
          FileInfo info = new FileInfo(path);
          ports.Add(new SerialPortInfo()
                    { Name = info.LinkTarget!.Replace(@"../../", "/dev/"), Description = info.Name });
        }
      } catch {
        return null;
      }

      return ports.ToArray();
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion
    
    #region PUBLIC - Functions
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Opens the serial port.
    /// If any of the serial port properties have been set, they will be applied
    /// as stty commands to the serial port as it is opened.
    /// </summary>
    public void Open() {
      ThrowIfDisposed();

      // Resolve the actual port, since the path given may be a wildcard.
      // Do this by getting all the files that match the given path and search string,
      // order by descending, and get the first path. This will get the first port.
      string portPath = Directory.EnumerateFiles(
                                                 Path.GetDirectoryName(_originalPortPath)!,
                                                 Path.GetFileName(_originalPortPath)!
                                                )
        .OrderBy(p => p)
        .FirstOrDefault();

      this.PortName = portPath ?? throw new FileNotFoundException($"No ports match the path {_originalPortPath}");

      // Instead of using the linux kernel API to configure the serial port, call stty from the shell.
      // Open the serial port file as a filestream
      _internalStream = File.Open(this.PortName, FileMode.Open);

      // Set all settings that were configured before the port was opened
      SetAllSerialParams();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Closes the serial port.
    /// The serial port may be re-opened, as long as it is not disposed.
    /// </summary>
    public void Close() {
      ThrowIfDisposed();
      _internalStream?.Dispose();
      _internalStream = null;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Disposes the serial port.
    /// Once it has been disposed, it cannot be re-opened.
    /// </summary>
    public void Dispose() {
      if (_isDisposed) {
        return;
      }
      Close();
      _isDisposed = true;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discards the contents of the serial port read buffer.
    /// Note, the current implementation only reads all bytes from the buffer,
    /// which is less than ideal.
    /// </summary>
    public void DiscardInBuffer() {
      ThrowIfDisposed();
      ThrowIfNotOpen();

      byte[] buffer = new byte[128];
      while (_internalStream.Read(buffer, 0, buffer.Length) > 0) {
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discards the contents of the serial port read buffer.
    /// Note, the current implementation only reads all bytes from the buffer,
    /// which is less than ideal. This will cause problems if MinimumBytesToRead
    /// is not set to 0.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task DiscardInBufferAsync(CancellationToken token) {
      ThrowIfDisposed();
      ThrowIfNotOpen();

      byte[] buffer = new byte[128];
      while (await _internalStream.ReadAsync(buffer, 0, buffer.Length, token) > 0) {
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discards the contents of the serial port write buffer.
    /// Note, the current implementation only flushes the stream,
    /// which is less than ideal. This will cause problems if hardware flow control
    /// is enabled.
    /// </summary>
    public void DiscardOutBuffer() {
      ThrowIfDisposed();
      ThrowIfNotOpen();

      _internalStream.Flush();
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Discards the contents of the serial port write buffer.
    /// Note, the current implementation only flushes the stream,
    /// which is less than ideal. This will cause problems if hardware flow control
    /// is enabled.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task DiscardOutBufferAsync(CancellationToken token) {
      ThrowIfDisposed();
      ThrowIfNotOpen();

      await _internalStream.FlushAsync(token);
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the serial port name
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return PortName;
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion

    #region PRIVATE - Functions
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Throw an InvalidOperationException if the port is not open.
    /// </summary>
    private void ThrowIfNotOpen() {
      if (!IsOpen) {
        throw new InvalidOperationException("Port is not open");
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Throw an ObjectDisposedException if the port is disposed.
    /// </summary>
    private void ThrowIfDisposed() {
      if (_isDisposed) {
        throw new ObjectDisposedException(nameof(LinuxSerialPort));
      }
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies stty arguments to the active serial port.
    /// </summary>
    /// <param name="sttyArguments"></param>
    /// <returns></returns>
    private string SetTtyOnSerial(IEnumerable<string> sttyArguments) {
      // Append the serial port file argument to the list of provided arguments
      // to make the stty command target the active serial port
      IEnumerable<string> arguments = GetPortTtyParam(PortName).Concat(sttyArguments);

      // Call stty with the parameters given
      return SetTtyWithParam(arguments);
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies stty arguments to the active serial port, including any prefix commands.
    /// </summary>
    /// <param name="sttyArguments"></param>
    /// <returns></returns>
    private string SetTtyOnSerialWithPrefix(IEnumerable<string> sttyArguments) {
      return SetTtyOnSerial(GetAllPrefixTtyParams().Concat(sttyArguments));
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets serial "sane".
    /// </summary>
    private void SetSerialSane() {
      SetTtyOnSerial(GetSaneModeTtyParam());
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the stty parameters for all currently set properties on the serial port.
    /// </summary>
    /// <returns></returns>
    private void SetAllSerialParams() {
      SetTtyOnSerial(GetAllTtyParams());
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Get tty parameters that should be run during every stty command.
    /// These should be applied before all other parameters.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> GetAllPrefixTtyParams() {
      IEnumerable<string> allParams = Enumerable.Empty<string>();

      // Set [-]drain parameter.
      // Setting this to false (-drain) will prevent the port from attempting to flush the output buffer before
      // setting any stty settings, avoiding a potential indefinite hang under certain conditions.
      // 
      if (EnableDrain != null) {
        allParams = allParams.Concat(GetDrainTtyParam(EnableDrain.Value));
      }

      return allParams;
    }
    //------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the stty parameters for all currently set properties on the serial port, including prefix commands.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> GetAllTtyParams() {
      IEnumerable<string> allParams = GetAllPrefixTtyParams();

      // Start with sane to reset any previous commands
      //
      allParams = allParams.Concat(GetSaneModeTtyParam());

      //
      // When properties are set for the first time, their backing value transitions
      // from null to the requested value.
      //
      // Return parameters for all property backing values that aren't null, since
      // these have been set.
      //

      allParams = allParams.Concat(GetRawModeTtyParam(_enableRawMode));

      if (_baudRate.HasValue) {
        allParams = allParams.Concat(GetBaudTtyParam(_baudRate.Value));
      }

      if (_minimumBytesToRead.HasValue) {
        allParams = allParams.Concat(GetMinDataTtyParam(_minimumBytesToRead.Value));
      }

      if (_readTimeout.HasValue) {
        allParams = allParams.Concat(GetReadTimeoutTtyParam(_readTimeout.Value));
      }

      if (_dataBits.HasValue) {
        allParams = allParams.Concat(GetDataBitsTtyParam(_dataBits.Value));
      }

      if (_stopBits.HasValue) {
        allParams = allParams.Concat(GetStopBitsTtyParam(_stopBits.Value));
      }

      if (_handshake.HasValue) {
        allParams = allParams.Concat(GetHandshakeTtyParams(_handshake.Value));
      }

      if (_parity.HasValue) {
        allParams = allParams.Concat(GetParityTtyParams(_parity.Value));
      }

      return allParams;
    }
    //------------------------------------------------------------------------------------------------------------------
    #endregion
  }
}