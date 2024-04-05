/**
 *******************************************************************************
 * @file serialportdriver.h
 *
 * @brief Description
 *
 * @author  Cristian
 *
 * @version 1.00
 *
 * @date Mar 28, 2024
 *
 *******************************************************************************
 * This file is part of the CCDevelop.SerialPortDriver project 
 * https://github.com/ccdevelop-net/CCDevelop.SerialPort.
 * Copyright (c) 2024 CCDevelop.NET
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 *******************************************************************************
 */
#ifndef _SERIALPORTDRIVER_H_
#define _SERIALPORTDRIVER_H_

#include <stddef.h>
#include <stdint.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/shm.h>
#include <termios.h>
#include <string.h>
#include <sys/time.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/ioctl.h>
#include <strings.h>

//******************************************************************************
// Public Enumerators **********************************************************
//******************************************************************************

/**
 * @brief Number of serial data bits
 */
typedef enum {
  SERIAL_DATABITS_5 = 0,  //**< 5 databits
  SERIAL_DATABITS_6,      //**< 6 databits
  SERIAL_DATABITS_7,      //**< 7 databits
  SERIAL_DATABITS_8,      //**< 8 databits
  SERIAL_DATABITS_16,     //**< 16 databits
} SerialDataBits;

/**
 * @brief Number of serial stop bits
 */
typedef enum {
  SERIAL_STOPBITS_1 = 0,  //**< 1 stop bit
  SERIAL_STOPBITS_1_5,    //**< 1.5 stop bits
  SERIAL_STOPBITS_2,      //**< 2 stop bits
} SerialStopBits;

/**
 * @brief Type of serial parity bits
 */
typedef enum {
  SERIAL_PARITY_NONE = 0, //**< No parity bit
  SERIAL_PARITY_EVEN,     //**< Even parity bit
  SERIAL_PARITY_ODD,      //**< Odd parity bit
  SERIAL_PARITY_MARK,     //**< Mark parity
  SERIAL_PARITY_SPACE     //**< Space bit
} SerialParity;

//******************************************************************************
// Public Functions ************************************************************
//******************************************************************************

/**
 * @brief Initialize serial port driver
 */
extern void Init(int32_t * fd);

/**
 * @brief Open a device
 * @param device - Port name (COM1, COM2, ... for Windows ) or (/dev/ttyS0, /dev/ttyACM0, /dev/ttyUSB0 ... for linux)
 * @param bauds - Baud rate of the serial port.
 *        @n Supported baud rate for Windows :
 *            - 110
 *            - 300
 *            - 600
 *            - 1200
 *            - 2400
 *            - 4800
 *            - 9600
 *            - 14400
 *            - 19200
 *            - 38400
 *            - 56000
 *            - 57600
 *            - 115200
 *            - 128000
 *            - 256000
 *
 *        @n Supported baud rate for Linux :@n
 *            - 110
 *            - 300
 *            - 600
 *            - 1200
 *            - 2400
 *            - 4800
 *            - 9600
 *            - 19200
 *            - 38400
 *            - 57600
 *            - 115200
 *
 *        @n Optionally supported baud rates, depending on Linux kernel:@n
              - 230400
 *            - 460800
 *            - 500000
 *            - 576000
 *            - 921600
 *            - 1000000
 *            - 1152000
 *            - 1500000
 *            - 2000000
 *            - 2500000
 *            - 3000000
 *            - 3500000
 *            - 4000000
 * @param databits - Number of data bits in one UART transmission.
 *        @n Supported values: @n
 *            - SERIAL_DATABITS_5 (5)
 *            - SERIAL_DATABITS_6 (6)
 *            - SERIAL_DATABITS_7 (7)
 *            - SERIAL_DATABITS_8 (8)
 *            - SERIAL_DATABITS_16 (16) (not supported on Unix)
 * @param parity - Parity type
 *        @n Supported values: @n
 *            - SERIAL_PARITY_NONE (N)
 *            - SERIAL_PARITY_EVEN (E)
 *            - SERIAL_PARITY_ODD (O)
 *            - SERIAL_PARITY_MARK (MARK) (not supported on Unix)
 *            - SERIAL_PARITY_SPACE (SPACE) (not supported on Unix)
 * @param stopbits - Number of stop bits
 *        @n Supported values:
 *            - SERIAL_STOPBITS_1 (1)
 *            - SERIAL_STOPBITS_1_5 (1.5) (not supported on Unix)
 *            - SERIAL_STOPBITS_2 (2)
 * @return Handle ID of the device opened
 * @return -1 device not found
 * @return -2 error while opening the device
 * @return -3 error while getting port parameters
 * @return -4 Speed (Bauds) not recognized
 * @return -5 error while writing port parameters
 * @return -6 error while writing timeout parameters
 * @return -7 Databits not recognized
 * @return -8 Stopbits not recognized
 * @return -9 Parity not recognized
 */
extern int32_t Open(const char * device, const uint32_t bauds,
                    SerialDataBits databits,
                    SerialParity parity,
                    SerialStopBits stopbits);

/**
 * @brief Check device opening state
 * @param fd - Device ID
 * @return Return true if device opened, overwise false
 */
extern bool IsOpen(int32_t fd);

/**
 * @param fd - Device ID
 * @brief Close the connection with the current device
 */
extern void Close(int32_t fd);

//-------------------------------------------
//---- Read/Write operation on characters ---
//-------------------------------------------

/**
 * @brief Write a byte on the current serial port
 * @param fd - Device ID
 * @param byte - Byte to send on the port (must be terminated by '\0')
 * @return 1 success
 * @return -1 error while writing data
 */
extern int32_t WriteByte(int32_t fd, uint8_t byte);

/**
 * @brief Wait for a byte from the serial device and return the data read
 * @param fd - Device ID
 * @param data - data read on the serial device
 * @param timeoutMS - delay of timeout before giving up the reading
 *                    If set to zero, timeout is disable (Optional)
 * @return 1 success
 * @return 0 Timeout reached
 * @return -1 error while setting the Timeout
 * @return -2 error while reading the byte
 */
extern int32_t ReadByte(int32_t fd, uint8_t * data, const uint32_t timeoutMS);

//-------------------------------------------
//---- Read/Write operation on strings ------
//-------------------------------------------

/**
 * @brief Write a string on the current serial port
 * @param fd - Device ID
 * @param str - string to send on the port (must be terminated by '\0')
 * @return 1 success
 * @return -1 error while writing data
 */
extern int32_t WriteString(int32_t fd, const char * str);

/**
 * @brief Read a string from the serial device (with timeout)
 * @param fd - Device ID
 * @param receivedString - String read on the serial device
 * @param finalChar - Final char of the string
 * @param maxNbBytes - Maximum allowed number of characters read
 * @param timeoutMS - Delay of timeout before giving up the reading
 * @return > 0 success, return the number of bytes read (including the null character)
 * @return  0 timeout is reached
 * @return -1 error while setting the Timeout
 * @return -2 error while reading the character
 * @return -3 MaxNbBytes is reached
 */
extern int32_t ReadString(int32_t fd, char * receivedString, char finalChar, uint32_t maxNbBytes, const uint32_t timeoutMS);

//-------------------------------------------
//---- Read/Write operation on bytes --------
//-------------------------------------------

/**
 * @brief Write an array of data on the current serial port
 * @param fd - Device ID
 * @param buffer - Array of bytes to send on the port
 * @param nbBytes - Number of byte to send
 * @return 1 success
 * @return -1 error while writing data
 */
extern int32_t WriteBytes(int32_t fd, const uint8_t * buffer, const uint32_t nbBytes);

/**
 * @brief Read an array of bytes from the serial device (with timeout)
 * @param fd - Device ID
 * @param buffer - Array of bytes read from the serial device
 * @param maxNbBytes - Maximum allowed number of bytes read
 * @param timeoutMS - Delay of timeout before giving up the reading
 * @param sleepDurationUs - Delay of CPU relaxing in microseconds (Linux only)
 *                          In the reading loop, a sleep can be performed after each reading
 *                          This allows CPU to perform other tasks
 * @return >=0 return the number of bytes read before timeout or requested data is completed
 * @return -1 error while setting the Timeout
 * @return -2 error while reading the byte
 */
extern int32_t ReadBytes(int32_t fd, uint8_t * buffer, uint32_t maxNbBytes, const uint32_t timeoutMS, uint32_t sleepDurationUs);

//-------------------------------------------
//---- Special operation --------------------
//-------------------------------------------

/**
 * @brief Empty receiver buffer
 * @note That when using serial over USB on Unix systems, a delay of 20ms may be necessary before calling the flushReceiver function
 * @return If the function succeeds, the return true.
 *         If the function fails, the return is false.
 */
extern bool FlushReceiver(int32_t fd);

/**
 * @brief Return the number of bytes in the received buffer (UNIX only)
 * @return The number of bytes received by the serial provider but not yet read.
 */
extern int32_t Available(int32_t fd);

//-------------------------------------------
//---- Access to IO bits --------------------
//-------------------------------------------

/**
 * @brief Set or unset the bit DTR (pin 4)
 *        DTR stands for Data Terminal Ready
 *        Convenience method :This method calls setDTR and clearDTR
 * @param status = true set DTR
 *        status = false unset DTR
 * @return If the function fails, the return value is false
 *         If the function succeeds, the return value is true.
 */
extern bool DTR(int32_t fd, bool status);

/**
 * @brief Set the bit DTR (pin 4)
 *        DTR stands for Data Terminal Ready
 * @return If the function fails, the return value is false
 *         If the function succeeds, the return value is true.
 */
extern bool SetDTR(int32_t fd);

/**
 * @brief Clear the bit DTR (pin 4)
 *        DTR stands for Data Terminal Ready
 * @return If the function fails, the return value is false
 *         If the function succeeds, the return value is true.
 */
extern bool ClearDTR(int32_t fd);

/**
 * @brief Set or unset the bit RTS (pin 7)
 *        RTS stands for Data Terminal Ready
 *        Convenience method :This method calls setDTR and clearDTR
 * @param status - true set DTR
 *        status - false unset DTR
 * @return false if the function fails
 * @return true if the function succeeds
 */
extern bool RTS(int32_t fd, bool status);

/**
 * @brief Set the bit RTS (pin 7)
 *        RTS stands for Data Terminal Ready
 * @return If the function fails, the return value is false
 *         If the function succeeds, the return value is true.
 */
extern bool SetRTS(int32_t fd);

/**
 * @brief Clear the bit RTS (pin 7)
 *        RTS stands for Data Terminal Ready
 * @return If the function fails, the return value is false
 *         If the function succeeds, the return value is true.
 */
extern bool ClearRTS(int32_t fd);

/**
 * @brief Get the RING's status (pin 9)
 *        Ring Indicator
 * @return Return true if RING is set otherwise false
 */
extern bool IsRI(int32_t fd);

/**
 * @brief Get the DCD's status (pin 1)
 *        CDC stands for Data Carrier Detect
 * @return true if DCD is set
 * @return false otherwise
 */
extern bool IsDCD(int32_t fd);

/**
 * @brief Get the CTS's status (pin 8)
 *        CTS stands for Clear To Send
 * @return Return true if CTS is set otherwise false
 */
extern bool IsCTS(int32_t fd);

/**
 * @brief Get the DSR's status (pin 6)
 *        DSR stands for Data Set Ready
 * @return Return true if DTR is set otherwise false
 */
extern bool IsDSR(int32_t fd);

/**
 * @brief Get the DTR's status (pin 4)
 *        DTR stands for Data Terminal Ready
 *        May behave abnormally on Windows
 * @return Return true if CTS is set otherwise false
 */
extern bool IsDTR(int32_t fd);

/**
 * @brief Get the RTS's status (pin 7)
 *        RTS stands for Request To Send
 *        May behave abnormally on Windows
 * @return Return true if RTS is set otherwise false
 */
extern bool IsRTS(int32_t fd);

#endif // _SERIALPORTDRIVER_H_
