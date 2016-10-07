using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;

namespace RigControl
{
    public class ComPort
    {

        #region Member Variables

        private SerialPort port = new SerialPort( );
        private int baudrate;
        private bool breakstate;
        private bool cdholding;
        private bool ctsholding;
        private int databits;
        private bool discardnull;
        private bool dsrholding;
        private bool dtrenable;
        private Encoding encoding;
        private Handshake handshake;
        private bool isopen;
        private string newline;
        private Parity parity;
        private byte parityreplace;
        private string portname;
        private int readbuffersize;
        private int readtimeout;
        private int receivedbytesthreshold;
        private bool rtsenable;
        private StopBits stopbits;
        private int writebuffersize;
        private int writetimeout;

        #endregion Member Variables

        #region Properties
        /// <summary>
        /// The baud rate must be supported by the user's serial driver. 
        /// The default value is 9600 bits per second (bps).
        /// </summary>
        public int BaudRate
        {
            get
            {
                baudrate = port.BaudRate;
                return baudrate;
            }
            set
            {
                baudrate = value;
                port.BaudRate = baudrate;
            }
        }

        /// <summary>
        /// Gets or Sets the break signal state
        /// The break signal state occurs when a transmission is suspended 
        /// and the line is placed in a break state (all low, no stop bit) until released. 
        /// To enter a break state, set this property to true. 
        /// If the port is already in a break state, setting this property again to true does not result in an exception. 
        /// It is not possible to write to the SerialPort object while BreakState is true. 
        /// </summary>
        public bool BreakState
        {
            get
            {
                breakstate = port.BreakState;
                return breakstate;
            }

            set
            {
                breakstate = value;
                port.BreakState = breakstate;
            }
        }

        /// <summary>
        /// Gets the state of the Carrier Detect line for the port (READONLY)
        /// true if the carrier is detected; otherwise, false. 
        /// This property can be used to monitor the state of the carrier detection line for a port. 
        /// No carrier usually indicates that the receiver has hung up and the carrier has been dropped.
        /// </summary>
        public bool CDHolding
        {
            get
            {
                cdholding = port.CDHolding;
                return cdholding;
            }
        }

        /// <summary>
        /// Gets the state of the Clear-to-Send line. (READONLY)
        /// true if the Clear-to-Send line is detected; otherwise, false. 
        /// The Clear-to-Send (CTS) line is used in Request to Send/Clear to Send (RTS/CTS) hardware handshaking. 
        /// The CTS line is queried by a port before data is sent.
        /// </summary>
        public bool Ctsholding
        {
            get
            {
                ctsholding = port.CtsHolding;
                return cdholding;
            }
        }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// The range of values for this property is from 5 through 8. The default value is 8.
        /// </summary>
        public int Databits
        {
            get
            {
                databits = port.DataBits;
                return databits;
            }

            set
            {
                databits = value;
                port.DataBits = databits;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted between the port and the receive buffer.
        /// This value should normally be set to false, especially for binary transmissions. 
        /// Setting this property to true can cause unexpected results for UTF32- and UTF16-encoded bytes. 
        /// </summary>
        public bool DiscardNull
        {
            get
            {
                discardnull = port.DiscardNull;
                return discardnull;
            }

            set
            {
                discardnull = value;
                port.DiscardNull = discardnull;
            }
        }

        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.  (READONLY)
        /// true if a Data Set Ready signal has been sent to the port; otherwise, false. 
        /// This property is used in Data Set Ready/Data Terminal Ready (DSR/DTR) handshaking. 
        /// The Data Set Ready (DSR) signal is usually sent by a modem to a port to indicate 
        /// that it is ready for data transmission or data reception.
        /// </summary>
        public bool DsrHolding
        {
            get
            {
                dsrholding = port.DsrHolding;
                return dsrholding;
            }
        }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during serial communication.
        /// true to enable Data Terminal Ready (DTR); otherwise, false. The default is false.
        /// Data Terminal Ready (DTR) is typically enabled during XON/XOFF software handshaking and 
        /// Request to Send/Clear to Send (RTS/CTS) hardware handshaking, and modem communications.
        /// </summary>
        public bool DtrEnable
        {
            get
            {
                dtrenable = port.DtrEnable;
                return dtrenable;
            }

            set
            {
                dtrenable = value;
                port.DtrEnable = dtrenable;
            }
        }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// The default is ASCIIEncoding. 
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                encoding = port.Encoding;
                return encoding;
            }

            set
            {
                encoding = value;
                port.Encoding = encoding;
            }
        }

        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data.
        /// 
        /// The default is None. 
        /// 
        /// When handshaking is used, the device connected to the SerialPort object is instructed 
        /// to stop sending data when there is at least ( ReadBufferSize-1024) bytes in the buffer. 
        /// The device is instructed to start sending data again when there are 1024 or fewer bytes in the buffer. 
        /// If the device is sending data in blocks that are larger than 1024 bytes, this may cause the buffer to overflow. 
        ///
        /// If the Handshake property is set to RequestToSendXOnXOff and CtsHolding is set to false, 
        /// the XOff character will not be sent. 
        /// If CtsHolding is then set to true, more data must be sent before the XOff character will be sent. 
        /// </summary>
        public Handshake Handshake
        {
            get
            {
                handshake = port.Handshake;
                return handshake;
            }

            set
            {
                handshake = value;
                port.Handshake = handshake;
            }
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of the SerialPort object.  (READONLY)
        /// 
        /// true if the serial port is open; otherwise, false. The default is false.
        /// 
        /// The IsOpen property tracks whether the port is open for use by the caller, 
        /// not whether the port is open by any application on the machine. 
        /// </summary>
        public bool IsOpen
        {
            get
            {
                isopen = port.IsOpen;
                return isopen;
            }
        }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods. 
        /// 
        /// A value that represents the end of a line. The default is a line feed, ( NewLine). 
        /// 
        /// This property defines the end of a line for the ReadLine and WriteLine methods. 
        /// </summary>
        public string NewLine
        {
            get
            {
                newline = port.NewLine;
                return newline;
            }

            set
            {
                newline = value;
                port.NewLine = newline;
            }
        }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// 
        /// One of the Parity values that represents the parity-checking protocol. The default is None. 
        /// 
        /// If a parity error occurs on the trailing byte of a stream, 
        /// an extra byte will be added to the input buffer with a value of 126.
        /// </summary>
        public Parity Parity
        {
            get
            {
                parity = port.Parity;
                return parity;
            }

            set
            {
                parity = value;
                port.Parity = parity;
            }
        }

        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error occurs.
        /// 
        /// A byte that replaces invalid bytes.
        /// 
        /// If the value is set to the null character, parity replacement is disabled.
        /// </summary>
        public byte ParityReplace
        {
            get
            {
                parityreplace = port.ParityReplace;
                return parityreplace;
            }

            set
            {
                parityreplace = value;
                port.ParityReplace = parityreplace;
            }
        }

        /// <summary>
        /// Gets or sets the port for communications, including but not limited to all available COM ports.
        /// 
        /// The communications port. The default is COM1.
        /// 
        /// A list of valid port names can be obtained using the GetPortNames method. 
        /// </summary>
        public string PortName
        {
            get
            {
                portname = port.PortName;
                return portname;
            }

            set
            {
                portname = value;
                port.PortName = portname;
            }
        }

        /// <summary>
        /// Gets or sets the size of the SerialPort input buffer. 
        /// 
        /// The buffer size. The default value is 4096.
        /// 
        /// The ReadBufferSize property ignores any value smaller than 4096. 
        /// 
        /// The BytesToRead property can return a value larger than the ReadBufferSize property 
        /// because the ReadBufferSize property represents only the Windows-created buffer 
        /// while the BytesToRead property represents the SerialPort buffer 
        /// in addition to the Windows-created buffer. 
        /// </summary>
        public int ReadBufferSize
        {
            get
            {
                readbuffersize = port.ReadBufferSize;
                return readbuffersize;
            }

            set
            {
                readbuffersize = value;
                port.ReadBufferSize = readbuffersize;
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation does not finish.
        /// 
        /// The number of milliseconds before a time-out occurs when a read operation does not finish.
        /// 
        /// The read time-out value was originally set at 500 milliseconds in the Win32 Communications API. 
        /// This property allows you to set this value. 
        /// The time-out can be set to any value greater than zero, or set to InfiniteTimeout, 
        /// in which case no time-out occurs. InfiniteTimeout is the default. 
        /// </summary>
        public int ReadTimeout
        {
            get
            {
                readtimeout = port.ReadTimeout;
                return readtimeout;
            }

            set
            {
                readtimeout = value;
                port.ReadTimeout = readtimeout;
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes in the internal input buffer before a DataReceived event occurs.
        /// 
        /// The number of bytes in the internal input buffer before a DataReceived event is fired. The default is 1. 
        /// 
        /// The DataReceived event may also be raised if an End of File byte is received, 
        /// regardless of the number of bytes in the internal input buffer and the 
        /// value of the ReceivedBytesThreshold property. 
        /// </summary>
        public int Receivedbytesthreshold
        {
            get
            {
                receivedbytesthreshold = port.ReceivedBytesThreshold;
                return receivedbytesthreshold;
            }

            set
            {
                receivedbytesthreshold = value;
                port.ReceivedBytesThreshold = receivedbytesthreshold;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled during serial communication.
        /// 
        /// true to enable Request to Transmit (RTS); otherwise, false. The default is false. 
        /// 
        /// The Request to Transmit (RTS) signal is typically used in Request to Send/Clear to Send (RTS/CTS) hardware handshaking. 
        /// </summary>
        public bool RtsEnable
        {
            get
            {
                rtsenable = port.RtsEnable;
                return rtsenable;
            }

            set
            {
                rtsenable = value;
                port.RtsEnable = rtsenable;
            }
        }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// 
        /// The default value for StopBits is One. 
        /// The StopBits.None option is not supported. 
        /// Setting the StopBits property to None will raise an ArgumentOutOfRangeException. 
        /// </summary>
        public StopBits Stopbits
        {
            get
            {
                stopbits = port.StopBits;
                return stopbits;
            }

            set
            {
                stopbits = value;
                port.StopBits = stopbits;
            }
        }

        /// <summary>
        /// Gets or sets the size of the serial port output buffer.
        /// 
        /// The size of the output buffer. The default is 2048.
        /// 
        /// The WriteBufferSize property ignores any value smaller than 2048. 
        /// </summary>
        public int Writebuffersize
        {
            get
            {
                writebuffersize = port.WriteBufferSize;
                return writebuffersize;
            }

            set
            {
                writebuffersize = value;
                port.WriteBufferSize = writebuffersize;
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a write operation does not finish.
        /// 
        /// The number of milliseconds before a time-out occurs. The default is InfiniteTimeout. 
        /// 
        /// The write time-out value was originally set at 500 milliseconds in the Win32 Communications API. 
        /// This property allows you to set this value. 
        /// The time-out can be set to any value greater than zero, or set to InfiniteTimeout, in which case no time-out occurs. 
        /// InfiniteTimeout is the default. 
        /// 
        /// Users of the unmanaged COMMTIMEOUTS structure might expect to set the time-out value to zero to suppress time-outs. 
        /// To suppress time-outs with the WriteTimeout property, however, you must specify InfiniteTimeout. 
        /// 
        /// This property does not affect the BeginWrite method of the stream returned by the BaseStream property. 
        /// </summary>
        public int Writetimeout
        {
            get
            {
                writetimeout = port.WriteTimeout;
                return writetimeout;
            }

            set
            {
                writetimeout = value;
                port.WriteTimeout = writetimeout;
            }
        }

        #endregion Properties

        #region Constructors

        public ComPort() { }
        public ComPort( string name )
        {
            port.DataReceived += new SerialDataReceivedEventHandler( port_DataReceived );
            PortName = name;
        }

        void port_DataReceived( object sender, SerialDataReceivedEventArgs e )
        {

        }

        #endregion // Constructors



    }
}
