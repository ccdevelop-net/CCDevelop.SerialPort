/**
 *******************************************************************************
 * @file serialportdriver.c
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

#include <serialportdriver.h>

typedef struct timeout {
  // Used to store the previous time (for computing timeout)
  struct timeval      previousTime;
} timeout;


//------------------------------------------------------------------------------
/**
 * @brief Initialize the timer. It writes the current time of the day in the structure PreviousTime.
 * @param tmo - Pointer to timeout struct
 */
static void InitTimer(struct timeout * tmo) {
  gettimeofday(&tmo->previousTime, NULL);
}
//------------------------------------------------------------------------------
/**
 * @brief Returns the time elapsed since initialization.  It write the current time of the day in the structure CurrentTime.
 *        Then it returns the difference between CurrentTime and PreviousTime.
 * @param tmo - Pointer to timeout struct
 * @return The number of microseconds elapsed since the functions InitTimer was called.
 */
uint64_t ElapsedTimeMS(struct timeout * tmo) {
  // Function variables
  struct timeval CurrentTime;
  int32_t sec;  // Number of seconds and microseconds since last call
  int32_t usec;

  // Get current time
  gettimeofday(&CurrentTime, NULL);

  // Compute the number of seconds and microseconds elapsed since last call
  sec = CurrentTime.tv_sec - tmo->previousTime.tv_sec;
  usec = CurrentTime.tv_usec - tmo->previousTime.tv_usec;

  // If the previous usec is higher than the current one
  if (usec < 0) {
    // Recompute the microseconds and subtract one second
    usec = 1000000 - tmo->previousTime.tv_usec + CurrentTime.tv_usec;
    sec--;
  }

  // Return the elapsed time in milliseconds
  return ((sec * 1000) + (usec / 1000));
}
//------------------------------------------------------------------------------
/**
 * @brief Read a string from the serial device (without TimeOut)
 * @param receivedString : string read on the serial device
 * @param FinalChar : final char of the string
 * @param MaxNbBytes : maximum allowed number of bytes read
 * @return >0 success, return the number of bytes read
 * @return -1 error while setting the Timeout
 * @return -2 error while reading the byte
 * @return -3 MaxNbBytes is reached
 */
int32_t ReadStringNoTimeOut(int32_t fd, char * receivedString, char finalChar, uint32_t maxNbBytes) {
  // Function Variables
  uint32_t nbBytes =0;
  char charRead;

  // While the buffer is not full
  while (nbBytes < maxNbBytes) {
    // Read a character with the restant time
    charRead = ReadByte(fd, (uint8_t*)&receivedString[nbBytes], 0);

    // Check a character has been read
    if (charRead == 1) {
      // Check if this is the final char
      if (receivedString[nbBytes] == finalChar) {
          // This is the final char, add zero (end of string)
          receivedString[++nbBytes] = 0;
          // Return the number of bytes read
          return nbBytes;
      }

      // The character is not the final char, increase the number of bytes read
      nbBytes++;
    }

    // An error occured while reading, return the error number
    if (charRead < 0) {
      return charRead;
    }
  }

  // Buffer is full : return -3
  return -3;
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
void Init(int32_t * fd) {
  (void)(fd);
  *fd = -1;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t Open(const char * device, const uint32_t bauds, SerialDataBits databits, SerialParity parity, SerialStopBits stopbits) {
    // Function variables
    struct termios options;
    int32_t fd = -1;


    // Open device
    fd = open(device, O_RDWR | O_NOCTTY | O_NDELAY);

    // If the device is not open, return -1
    if (fd == -1) {
      return -2;
    }

    // Open the device in nonblocking mode
    fcntl(fd, F_SETFL, O_NONBLOCK);


    // Get the current options of the port
    tcgetattr(fd, &options);

    // Clear all the options
    bzero(&options, sizeof(options));

    // Prepare speed (Bauds)
    speed_t         speed;
    switch (bauds) {
      //==========================================
      case 110: {
        speed = B110;
        break;
      }
      //==========================================
      case 300: {
        speed = B300;
        break;
      }
      //==========================================
      case 600: {
        speed = B600;
        break;
      }
      //==========================================
      case 1200: {
        speed = B1200;
        break;
      }
      //==========================================
      case 2400: {
        speed = B2400;
        break;
      }
      //==========================================
      case 4800: {
        speed = B4800;
        break;
      }
      //==========================================
      case 9600: {
        speed = B9600;
        break;
      }
      //==========================================
      case 19200: {
        speed = B19200;
        break;
      }
      //==========================================
      case 38400: {
        speed = B38400;
        break;
      }
      //==========================================
      case 57600: {
        speed = B57600;
        break;
      }
      //==========================================
      case 115200: {
        speed = B115200;
        break;
      }
      //==========================================
#if defined B230400
      case 230400: {
        speed = B230400;
        break;
      }
#endif
      //==========================================
#if defined B460800
      case 460800: {
        speed = B460800;
        break;
      }
#endif
      //==========================================
#if defined B500000
      case 500000: {
        speed = B500000;
        break;
      }
#endif
      //==========================================
#if defined B576000
      case 576000: {
        speed = B576000;
        break;
      }
#endif
      //==========================================
#if defined B921600
      case 921600: {
        speed = B921600;
        break;
      }
#endif
      //==========================================
#if defined B1000000
      case 1000000: {
        speed = B1000000;
        break;
      }
#endif
      //==========================================
#if defined B1152000
      case 1152000: {
        speed = B1152000;
        break;
      }
#endif
      //==========================================
#if defined B1500000
      case 1500000: {
        speed = B1500000;
        break;
      }
#endif
      //==========================================
#if defined B2000000
      case 2000000: {
        speed = B2000000;
        break;
      }
#endif
      //==========================================
#if defined B2500000
      case 2500000: {
        speed = B2500000;
        break;
      }
#endif
      //==========================================
#if defined B3000000
      case 3000000: {
        speed = B3000000;
        break;
      }
#endif
      //==========================================
#if defined B3500000
      case 3500000: {
        speed = B3500000;
        break;
      }
#endif
      //==========================================
#if defined B4000000
      case 4000000: {
        speed = B4000000;
        break;
      }
#endif
      //==========================================
      default: {
        return -4;
      }
      //==========================================
    }

    int32_t databitsFlag = 0;
    switch(databits) {
      //==========================================
      case SERIAL_DATABITS_5: {
        databitsFlag = CS5; break;
      }
      //==========================================
      case SERIAL_DATABITS_6: {
        databitsFlag = CS6; break;
      }
      //==========================================
      case SERIAL_DATABITS_7: {
        databitsFlag = CS7; break;
      }
      //==========================================
      case SERIAL_DATABITS_8: {
        databitsFlag = CS8; break;
      }
      //==========================================
      default: {
        return -7;  // 16 bits and everything else not supported
      }
      //==========================================
    }

    int32_t stopbitsFlag = 0;
    switch(stopbits) {
      //==========================================
      case SERIAL_STOPBITS_1: {
        stopbitsFlag = 0;
        break;
      }
      //==========================================
      case SERIAL_STOPBITS_2: {
        stopbitsFlag = CSTOPB;
        break;
      }
      //==========================================
      default: {
        return -8; // 1.5 stopbits and everything else not supported
      }
      //==========================================
    }

    int32_t parityFlag = 0;
    switch(parity) {
      //==========================================
      case SERIAL_PARITY_NONE: {
        parityFlag = 0;
        break;
      }
      //==========================================
      case SERIAL_PARITY_EVEN: {
        parityFlag = PARENB;
        break;
      }
      //==========================================
      case SERIAL_PARITY_ODD: {
        parityFlag = (PARENB | PARODD);
        break;
      }
      //==========================================
      default: {
        return -9; // Mark and space parity not supported
      }
      //==========================================
    }

    // Set the baud rate
    cfsetispeed(&options, speed);
    cfsetospeed(&options, speed);

    // Configure the device : data bits, stop bits, parity, no control flow
    // Ignore modem control lines (CLOCAL) and Enable receiver (CREAD)
    options.c_cflag |= ( CLOCAL | CREAD | databitsFlag | parityFlag | stopbitsFlag);
    options.c_iflag |= ( IGNPAR | IGNBRK );

    // Timer unused
    options.c_cc[VTIME] = 0;

    // At least on character before satisfy reading
    options.c_cc[VMIN] = 0;

    // Activate the settings
    tcsetattr(fd, TCSANOW, &options);

    // Success
    return fd;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsOpen(int32_t fd) {
  return fd >= 0;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
void Close(int32_t fd) {
  close (fd);
  fd = -1;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t WriteByte(int32_t fd, const uint8_t byte) {
    // Write the char
    if (write(fd, &byte, 1) != 1) {
      return -1;
    }

    // Write operation successfull
    return 1;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t ReadByte(int32_t fd, uint8_t * byte, uint32_t timeoutMS) {
  // Timer used for timeout
  struct timeout         timer;

  // Initialize the timer
  InitTimer(&timer);

  // While Timeout is not reached
  while (timeoutMS == 0 || ElapsedTimeMS(&timer) < timeoutMS) {
    // Try to read a byte on the device
    switch (read(fd, byte, 1)) {
      case 1: {
        return 1; // Read successful
      }
      case -1: {
        return -2; // Error while reading
      }
    }
  }

  return 0;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t WriteString(int32_t fd, const char * receivedString) {
  // Length of the string
  int32_t length = strlen(receivedString);

  // Write the string
  if (write(fd, receivedString, length) != length) {
    return -1;
  }

  // Write operation successful
  return 1;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t ReadString(int32_t fd, char * receivedString, char finalChar, uint32_t maxNbBytes, uint32_t timeoutMS) {
  // Check if timeout is requested
  if (timeoutMS == 0) {
    return ReadStringNoTimeOut(fd, receivedString, finalChar, maxNbBytes);
  }

  // Function Variables
  uint32_t        nbBytes = 0;
  char            charRead;
  struct timeout  timer;
  uint64_t        timeoutParam;

  // Initialize the timer (for timeout)
  InitTimer(&timer);

  // While the buffer is not full
  while (nbBytes < maxNbBytes) {
    // Compute the TimeOut for the next call of ReadByte
    timeoutParam = timeoutMS - ElapsedTimeMS(&timer);

    // If there is time remaining
    if (timeoutParam > 0) {
      // Wait for a byte on the serial link with the remaining time as timeout
      charRead = ReadByte(fd, (uint8_t*)&receivedString[nbBytes], timeoutParam);

      // If a byte has been received
      if (charRead == 1) {
        // Check if the character received is the final one
        if (receivedString[nbBytes] == finalChar) {
            // Final character: add the end character 0
            receivedString[++nbBytes] = 0;

            // Return the number of bytes read
            return nbBytes;
        }

        // This is not the final character, just increase the number of bytes read
        nbBytes++;
      }

      // Check if an error occurred during reading char
      // If an error occurred, return the error number
      if (charRead < 0) {
        return charRead;
      }
    }

    // Check if timeout is reached
    if (ElapsedTimeMS(&timer) > timeoutMS) {
        // Add the end caracter
        receivedString[nbBytes] = 0;

        // Return 0 (timeout reached)
        return 0;
    }
  }

  // Buffer is full : return -3
  return -3;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t WriteBytes(int32_t fd, const uint8_t * buffer, const uint32_t nbBytes) {
    // Write data
    if (write (fd, buffer, nbBytes) != (ssize_t)nbBytes) {
      return -1;
    }

    // Write operation successful
    return 1;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t ReadBytes (int32_t fd, uint8_t * buffer, uint32_t maxNbBytes, uint32_t timeoutMS, uint32_t sleepDurationUS) {
  // Function VAriables
  struct timeout  timer;
  uint32_t nbByteRead = 0;

  // Initialize the timer
  InitTimer(&timer);

  // While Timeout is not reached
  while (timeoutMS == 0 || ElapsedTimeMS(&timer) < timeoutMS) {
    // Compute the position of the current byte
    uint8_t * ptr = (uint8_t*)buffer + nbByteRead;

    // Try to read a byte on the device
    int ret = read(fd, (void*)ptr, maxNbBytes - nbByteRead);

    // Error while reading
    if (ret == -1) {
      return -2;
    }

    // One or several byte(s) has been read on the device
    if (ret > 0) {
      // Increase the number of read bytes
      nbByteRead += ret;
      // Success : bytes has been read
      if (nbByteRead >= maxNbBytes) {
        return nbByteRead;
      }
    }

    // Suspend the loop to avoid charging the CPU
    usleep(sleepDurationUS);
  }

  // Timeout reached, return the number of bytes read
  return nbByteRead;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool FlushReceiver(int32_t fd) {
  // Purge receiver
  return tcflush(fd, TCIFLUSH) == 0;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
int32_t Available(int32_t fd) {
  // Function Variables
  int nBytes = 0;

  // Return number of pending bytes in the receiver
  ioctl(fd, FIONREAD, &nBytes);

  return nBytes;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool DTR(int32_t fd, bool status) {
  if (status) {
    return SetDTR(fd);    // Set DTR
  } else {
    return ClearDTR(fd);  // Unset DTR
  }
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool SetDTR(int32_t fd) {
  // Function Variables
  int32_t statusDTR = 0;

  ioctl(fd, TIOCMGET, &statusDTR);
  statusDTR |= TIOCM_DTR;
  ioctl(fd, TIOCMSET, &statusDTR);

  return true;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool ClearDTR(int32_t fd) {
  // Function Variables
  int32_t statusDTR = 0;

  ioctl(fd, TIOCMGET, &statusDTR);
  statusDTR &= ~TIOCM_DTR;
  ioctl(fd, TIOCMSET, &statusDTR);

  return true;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool RTS(int32_t fd, bool status) {
  if (status) {
    return SetRTS(fd);   // Set RTS
  } else {
    return ClearRTS(fd); // Unset RTS
  }
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool SetRTS(int32_t fd) {
  // Function Variables
  int32_t statusRTS=0;

  ioctl(fd, TIOCMGET, &statusRTS);
  statusRTS |= TIOCM_RTS;
  ioctl(fd, TIOCMSET, &statusRTS);

  return true;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool ClearRTS(int32_t fd) {
  // Function Variables
  int32_t statusRTS = 0;

  ioctl(fd, TIOCMGET, &statusRTS);
  statusRTS &= ~TIOCM_RTS;
  ioctl(fd, TIOCMSET, &statusRTS);

  return true;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsCTS(int32_t fd) {
  // Function Variables
  int32_t status = 0;

  //Get the current status of the CTS bit
  ioctl(fd, TIOCMGET, &status);

  return status & TIOCM_CTS;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsDSR(int32_t fd) {
  // Function Variables
  int32_t status = 0;

  //Get the current status of the DSR bit
  ioctl(fd, TIOCMGET, &status);

  return status & TIOCM_DSR;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsDCD(int32_t fd) {
  // Function Variables
  int32_t status = 0;

  //Get the current status of the DCD bit
  ioctl(fd, TIOCMGET, &status);

  return status & TIOCM_CAR;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsRI(int32_t fd) {
  // Function Variables
  int32_t status = 0;

  //Get the current status of the RING bit
  ioctl(fd, TIOCMGET, &status);

  return status & TIOCM_RNG;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsDTR(int32_t fd) {
  // Function Variables
  int32_t status = 0;

  //Get the current status of the DTR bit
  ioctl(fd, TIOCMGET, &status);
  return status & TIOCM_DTR  ;
}
//------------------------------------------------------------------------------
__attribute__ ((visibility ("default")))
bool IsRTS(int32_t fd) {
  // Function Variables
  int status = 0;

  //Get the current status of the CTS bit
  ioctl(fd, TIOCMGET, &status);

  return status & TIOCM_RTS;
}
//------------------------------------------------------------------------------


