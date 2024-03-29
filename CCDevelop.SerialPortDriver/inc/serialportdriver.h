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
#include <stdlib.h>
#include <sys/types.h>
#include <sys/shm.h>
#include <termios.h>
#include <string.h>
#include <sys/time.h>
// File control definitions
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
extern void Init(int32_t fd);

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
 * @return -1 error while writting data
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
extern int32_t WriteString(const char * str);

// Read a string (with timeout)
/**
 * @brief
 * @param fd - Device ID
 * @param receivedString
 * @param finalChar
 * @param maxNbBytes
 * @param timeoutMS
 * @return
 */
extern int32_t ReadString(char * receivedString, char finalChar, uint32_t maxNbBytes, const uint32_t timeoutMS);

//-------------------------------------------
//---- Read/Write operation on bytes --------
//-------------------------------------------

// Write an array of bytes
/**
 * @brief
 * @param buffer
 * @param nbBytes
 * @return
 */
extern int32_t WriteBytes(const uint8_t * buffer, const uint32_t nbBytes);

// Read an array of byte (with timeout)
/**
 * @brief
 * @param buffer
 * @param maxNbBytes
 * @param timeoutMS
 * @param sleepDurationUs
 * @return
 */
extern int32_t ReadBytes(uint8_t * buffer, uint32_t maxNbBytes, const uint32_t timeoutMS, uint32_t sleepDurationUs);

//-------------------------------------------
//---- Special operation --------------------
//-------------------------------------------

// Empty the received buffer
/**
 * @brief
 * @return
 */
extern char FlushReceiver();

// Return the number of bytes in the received buffer
/**
 * @brief
 * @return
 */
extern int32_t Available();

//-------------------------------------------
//---- Access to IO bits --------------------
//-------------------------------------------

// Set CTR status (Data Terminal Ready, pin 4)
/**
 * @brief
 * @param status -
 * @return
 */
extern bool DTR(bool status);
/**
 * @brief
 * @return
 */
extern bool SetDTR(void);
/**
 * @brief
 * @return
 */
extern bool ClearDTR(void);

// Set RTS status (Request To Send, pin 7)
/**
 * @brief
 * @param status
 * @return
 */
extern bool RTS(bool status);
/**
 * @brief
 * @return
 */
extern bool SetRTS(void);
/**
 * @brief
 * @return
 */
extern bool ClearRTS(void);

// Get RI status (Ring Indicator, pin 9)
/**
 * @brief
 * @return
 */
extern bool IsRI(void);

// Get DCD status (Data Carrier Detect, pin 1)
/**
 * @brief
 * @return
 */
extern bool IsDCD(void);

// Get CTS status (Clear To Send, pin 8)
/**
 * @brief
 * @return
 */
extern bool IsCTS(void);

// Get DSR status (Data Set Ready, pin 9)
/**
 * @brief
 * @return
 */
extern bool IsDSR(void);

// Get RTS status (Request To Send, pin 7)
/**
 * @brief
 * @return
 */
extern bool IsRTS(void);

// Get CTR status (Data Terminal Ready, pin 4)
/**
 * @brief
 * @return
 */
extern bool IsDTR(void);


#endif // _SERIALPORTDRIVER_H_
