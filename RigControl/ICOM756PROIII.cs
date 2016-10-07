using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Collections;

namespace RigControl
{
    public class ICOM756PROIII
    {

        #region Enums

        //private enum VFOValues
        //{
        //    SelectVFOMode,
        //    ExchangeMainAndSubReadouts,
        //    EqualizeMainAndSubReadouts,
        //    TurnTheDualwatchOFF,
        //    TurnTheDualwatchON,
        //    SelectMainReadout,
        //    SelectSubReadout
        //}

        //public enum Mode
        //{
        //    LSB,
        //    USB,
        //    AM,
        //    CW,
        //    RTTY,
        //    FM,
        //    FM_WIDE,
        //    CW_REVERSE,
        //    RTTY_REVERSE
        //}


        #endregion Enums


        #region Constants

        #region Message Constants

        const byte PREAMBLE = 254; //0xFE
        const byte RADIO = 110; //0x6E
        const byte CONTROLLER = 224; //0xE0
        const byte EOM = 253; //0xFD
        const byte OK = 251; //0xFB
        const byte NG = 250; //0xFA

        #endregion Message Constants

        #region Command Constants


        //Send frequency data, if rig in tranceive mode, does not acknowledge
        const byte CMD_SEND_FREQ = 0x00; // 0x00 

        // Send mode data, if rig in tranceive mode, does not acknowledge
        const byte CMD_SEND_MODE = 0x01; // 0x01

        // Read band edge frequencies 
        const byte CMD_READ_BAND = 0x02; // 0x02

        // Read operating frequency 
        const byte CMD_READ_FREQ = 0x03; // 0x03

        // Read operating mode
        const byte CMD_READ_MODE = 0x04; // 0x04

        // Set frequency data 
        const byte CMD_SET_FREQ = 0x05; // 0x05

        // Set mode data
        const byte CMD_SET_MODE = 0x06; // 0x06

        // Set VFO mode
        const byte CMD_SET_VFO = 0x07; // 0x07

        // Set Memory Channel
        const byte CMD_SET_MEMORY = 0x08; // 0x08

        // Memory write
        const byte CMD_WRITE_MEM = 0x09; // 0x09 

        // Memory to VFO 
        const byte CMD_MEM_TO_VFO = 0x0A; // 10

        // Memory clear
        const byte CMD_CLEAR_MEMORY = 0x0B; // 11

        // Read duplex offset frequency default changes with HF/6M/2M 
        const byte CMD_READ_OFFS = 0x0C; // 12

        const byte CMD_SET_OFFS = 0x0D; // Set duplex offset frequency 
        const byte CMD_CTL_SCAN = 0x0E; // Control scan, Sc 
        const byte CMD_CTL_SPLT = 0x0F; // Control split, and duplex mode Sc 
        const byte CMD_SET_TUNING_STEP = 0x10; // Set tuning step, Sc 
        const byte CMD_ATTENUATOR = 0x11; // Set/get attenuator, Sc 
        const byte CMD_ANTENNA = 0x12; // Set/get antenna, Sc 
        const byte CMD_VOICE_SYNTHESIZER = 0x13; // Control announce (speech synth.), Sc 
        const byte CMD_LEVELS = 0x14; // Set AF/RF/squelch, Sc 
        const byte CMD_READ_METER_LEVELS = 0x15; // Read squelch condiction/S-meter level, Sc 
        const byte CMD_FUNCTIONS = 0x16; // Function settings (AGC,NB,etc.), Sc 
        const byte CMD_SEND_CW = 0x17; // Send CW message 
        const byte CMD_SET_PWR = 0x18; // Set Power ON/OFF, Sc 
        const byte CMD_READ_TRANCEIVER_ID = 0x19; // Read transceiver ID code 
        const byte CMD_MISC = 0x1A; // Misc memory/bank/rig control functions, Sc 
        const byte CMD_SET_TONE = 0x1b; // Set tone frequency 
        const byte CMD_CTL_PTT = 0x1c; // Control Transmit On/Off, Sc 
        const byte CMD_CTL_MISC = 0x7f; // Miscellaneous control, Sc 

        #endregion Command Constants

        #region Sub Command Constants

        const byte S_LSB = 0x00; // Set to LSB 
        const byte S_USB = 0x01; // Set to USB 
        const byte S_AM = 0x02; // Set to AM 
        const byte S_CW = 0x03; // Set to CW 
        const byte S_RTTY = 0x04; // Set to RTTY 
        const byte S_FM = 0x05; // Set to FM 
        const byte S_WFM = 0x06; // Set to Wide FM 
        const byte S_CWR = 0x07; // Set to CW Reverse 
        const byte S_RTTYR = 0x08; // Set to RTTY Reverse 
        const byte S_AMS = 0x11; // Set to AMS 
        const byte S_PSK = 0x12; // 7800 PSK USB 
        const byte S_PSKR = 0x13; // 7800 PSK LSB 

        const byte S_R7000_SSB = 0x05; // Set to SSB on R-7000 

        // filter width coding for older ICOM rigs with 2 filter width 
        // there is no special 'wide' for that rigs 
        const byte PD_MEDIUM_2 = 0x01; // Medium 
        const byte PD_NARROW_2 = 0x02; // Narrow 

        // filter width coding for newer ICOM rigs with 3 filter width 
        const byte FILTER_WIDTH_WIDE_3 = 0x01; // Wide 
        const byte FILTER_WIDTH_MEDIUM_3 = 0x02; // Medium 
        const byte FILTER_WIDTH_NARROW_3 = 0x03; // Narrow 

        // Set VFO (C_SET_VFO) sub commands

        const byte S_VFOA = 0x00; // Set to VFO A 
        const byte S_VFOB = 0x01; // Set to VFO B 
        const byte S_BTOA = 0xa0; // VFO A=B 
        const byte S_XCHNG = 0xb0; // Switch VFO A and B 
        const byte S_SUBTOMAIN = 0xb1; // MAIN = SUB 
        const byte S_DUAL_OFF = 0xc0; // Dual watch off 
        const byte S_DUAL_ON = 0xc1; // Dual watch on 
        const byte S_MAIN = 0xd0; // Select MAIN band 
        const byte S_SUB = 0xd1; // Select SUB band 
        const byte S_FRONTWIN = 0xe0; // Select front window 

        //
        // Set MEM (C_SET_MEM) sub commands

        const byte S_BANK = 0xa0; // Select memory bank 

        //
        // Scan control (C_CTL_SCAN) subcommands

        const byte S_SCAN_STOP = 0x00; // Stop scan/window scan 
        const byte S_SCAN_START = 0x01; // Programmed/Memory scan 
        const byte S_SCAN_PROG = 0x02; // Programmed scan 
        const byte S_SCAN_DELTA = 0x03; // Delta-f scan 
        const byte S_SCAN_WRITE = 0x04; // auto memory-write scan 
        const byte S_SCAN_FPROG = 0x12; // Fine programmed scan 
        const byte S_SCAN_FDELTA = 0x13; // Fine delta-f scan 
        const byte S_SCAN_MEM2 = 0x22; // Memory scan 
        const byte S_SCAN_SLCTN = 0x23; // Selected number memory scan 
        const byte S_SCAN_SLCTM = 0x24; // Selected mode memory scan 
        const byte S_SCAN_PRIO = 0x42; // Priority / window scan 
        const byte S_SCAN_NSLCT = 0xB0; // Set as non select channel 
        const byte S_SCAN_SLCT = 0xB1; // Set as select channel 
        const byte S_SCAN_SL_NUM = 0xB2; // select programed mem scan 7800 only 
        const byte S_SCAN_RSMOFF = 0xD0; // Set scan resume OFF 
        const byte S_SCAN_RSMON = 0xD3; // Set scan resume ON 


        //
        // Split control (S_CTL_SPLT) subcommands

        const byte S_SPLT_OFF = 0x00; // Split OFF 
        const byte S_SPLT_ON = 0x01; // Split ON 
        const byte S_DUP_OFF = 0x10; // Simplex mode 
        const byte S_DUP_M = 0x11; // Duplex - mode 
        const byte S_DUP_P = 0x12; // Duplex + mode 

        //
        // Set Attenuator (C_CTL_ATT) subcommands

        const byte S_ATT_RD = byte.MinValue; // Without subcommand, reads out setting 
        const byte S_ATT_OFF = 0x00; // Off 
        const byte S_ATT_6dB = 0x06; // 6 dB, IC-756Pro 
        const byte S_ATT_10dB = 0x10; // 10 dB 
        const byte S_ATT_12dB = 0x12; // 12 dB, IC-756Pro 
        const byte S_ATT_18dB = 0x18; // 18 dB, IC-756Pro 
        const byte S_ATT_20dB = 0x20; // 20 dB 
        const byte S_ATT_30dB = 0x30; // 30 dB, or Att on for IC-R75 

        //
        // Set Preamp (S_FUNC_PAMP) data

        const byte D_PAMP_OFF = 0x00;
        const byte D_PAMP1 = 0x01;
        const byte D_PAMP2 = 0x02;

        //
        // Set AGC (S_FUNC_AGC) data

        const byte D_AGC_FAST = 0x00;
        const byte D_AGC_MID = 0x01;
        const byte D_AGC_SLOW = 0x02;
        const byte D_AGC_SUPERFAST = 0x03; // IC746 pro 

        //
        // Set antenna (C_SET_ANT) subcommands

        const byte S_ANT_RD = byte.MinValue; // Without subcommand, reads out setting 
        const byte S_ANT1 = 0x00; // Antenna 1 
        const byte S_ANT2 = 0x01; // Antenna 2 

        //
        // Announce control (C_CTL_ANN) subcommands

        const byte S_ANN_ALL = 0x00; // Announce all 
        const byte S_ANN_FREQ = 0x01; // Announce freq 
        const byte S_ANN_MODE = 0x02; // Announce operating mode 

        //
        // Function settings (C_CTL_LVL) subcommands

        const byte S_LVL_AF = 0x01; // AF level setting 
        const byte S_LVL_RF = 0x02; // RF level setting 
        const byte S_LVL_SQL = 0x03; // SQL level setting 
        const byte S_LVL_IF = 0x04; // IF shift setting 
        const byte S_LVL_APF = 0x05; // APF level setting 
        const byte S_LVL_NR = 0x06; // NR level setting 
        const byte S_LVL_PBTIN = 0x07; // Twin PBT setting (inside) 
        const byte S_LVL_PBTOUT = 0x08; // Twin PBT setting (outside) 
        const byte S_LVL_CWPITCH = 0x09; // CW pitch setting 
        const byte S_LVL_RFPOWER = 0x0a; // RF power setting 
        const byte S_LVL_MICGAIN = 0x0b; // MIC gain setting 
        const byte S_LVL_KEYSPD = 0x0c; // Key Speed setting 
        const byte S_LVL_NOTCHF = 0x0d; // Notch freq. setting 
        const byte S_LVL_COMP = 0x0e; // Compressor level setting 
        const byte S_LVL_BKINDL = 0x0f; // BKin delay setting 
        const byte S_LVL_BALANCE = 0x10; // Balance setting (Dual watch) 
        const byte S_LVL_AGC = 0x11; // AGC (7800) 
        const byte S_LVL_NB = 0x12; // NB setting 
        const byte S_LVL_DIGI = 0x13; // DIGI-SEL (7800) 
        const byte S_LVL_DRIVE = 0x14; // DRIVE gain setting 
        const byte S_LVL_MON = 0x15; // Monitor gain setting 
        const byte S_LVL_VOXGAIN = 0x16; // VOX gain setting 
        const byte S_LVL_ANTIVOX = 0x17; // Anti VOX gain setting 
        const byte S_LVL_CONTRAST = 0x18; // CONTRAST level setting 
        const byte S_LVL_BRIGHT = 0x19; // BRIGHT level setting 

        //
        // Read squelch condition/S-meter level/other meter levels (C_RD_SQSM) subcommands

        const byte S_SQL = 0x01; // Read squelch condition 
        const byte S_SML = 0x02; // Read S-meter level 
        const byte S_RFML = 0x11; // Read RF-meter level 
        const byte S_SWR = 0x12; // Read SWR-meter level 
        const byte S_ALC = 0x13; // Read ALC-meter level 
        const byte S_CMP = 0x14; // Read COMP-meter level 
        const byte S_VD = 0x15; // Read Vd-meter level 
        const byte S_ID = 0x16; // Read Id-meter level 

        //
        // Function settings (C_CTL_FUNC) subcommands  Set and Read

        const byte S_FUNC_PAMP = 0x02; // Preamp setting 
        const byte S_FUNC_AGCOFF = 0x10; // IC-R8500 only 
        const byte S_FUNC_AGCON = 0x11; // IC-R8500 only 
        const byte S_FUNC_AGC = 0x12; // AGC setting presets: the dsp models allow these to be modified 
        const byte S_FUNC_NBOFF = 0x20; // IC-R8500 only 
        const byte S_FUNC_NBON = 0x21; // IC-R8500 only 
        const byte S_FUNC_NB = 0x22; // NB setting 
        const byte S_FUNC_APFOFF = 0x30; // IC-R8500 only 
        const byte S_FUNC_APFON = 0x31; // IC-R8500 only 
        const byte S_FUNC_APF = 0x32; // APF setting 
        const byte S_FUNC_NR = 0x40; // NR setting 
        const byte S_FUNC_ANF = 0x41; // ANF setting 
        const byte S_FUNC_TONE = 0x42; // TONE setting 
        const byte S_FUNC_TSQL = 0x43; // TSQL setting 
        const byte S_FUNC_COMP = 0x44; // COMP setting 
        const byte S_FUNC_MON = 0x45; // Monitor setting 
        const byte S_FUNC_VOX = 0x46; // VOX setting 
        const byte S_FUNC_BKIN = 0x47; // BK-IN setting 
        const byte S_FUNC_MN = 0x48; // Manual notch setting 
        const byte S_FUNC_RF = 0x49; // RTTY Filter setting 
        const byte S_FUNC_AFC = 0x4A; // Auto Frequency Control (AFC) setting 
        const byte S_FUNC_DTCS = 0x4B; //DTCS tone code squelch setting
        const byte S_FUNC_VSC = 0x4C; // voice squelch control useful for scanning
        const byte S_FUNC_MANAGC = 0x4D; // manual AGC 
        const byte S_FUNC_DIGISEL = 0x4E; // DIGI-SEL 
        const byte S_FUNC_TW_PK = 0x4F; // RTTY Twin Peak filter 0= off 1 = on 
        const byte S_FUNC_DIAL_LK = 0x50; // Dial lock 

        //
        // Set Power On/Off (C_SET_PWR) subcommands

        const byte S_PWR_OFF = 0x00;
        const byte S_PWR_ON = 0x01;

        //
        // Transmit control (C_CTL_PTT) subcommands

        const byte S_PTT = 0x00;
        const byte S_ANT_TUN = 0x01; // Auto tuner 0=OFF, 1 = ON, 2=Start Tuning 

        //
        // Misc contents (C_CTL_MEM) subcommands applies to newer rigs.

        const byte S_MEM_CNTNT = 0x00; // Memory content 2 bigendian 
        const byte S_MEM_BAND_REG = 0x01; // band stacking register 
        const byte S_MEM_FILT_WDTH = 0x03; // current passband filter width 
        const byte S_MEM_PARM = 0x05; // rig parameters; extended parm # + param value:  should be coded 
        // in the rig files because they are different for each rig 
        const byte S_MEM_DATA_MODE = 0x06; // data mode 
        const byte S_MEM_TX_PB = 0x07; // SSB tx passband 
        const byte S_MEM_FLTR_SHAPE = 0x08; // DSP filter shape 0=sharp 1=soft 

        // Icr75c 
        const byte S_MEM_CNTNT_SLCT = 0x01;
        const byte S_MEM_FLT_SLCT = 0x01;
        const byte S_MEM_MODE_SLCT = 0x02;
        // For IC-910H rig. 
        const byte S_MEM_RDWR_MEM = 0x00; // Read/write memory channel 
        const byte S_MEM_SATMEM = 0x01; // Satellite memory 
        const byte S_MEM_VOXGAIN = 0x02; // VOX gain level (0=0%, 255=100%) 
        const byte S_MEM_VOXDELAY = 0x03; // VOX delay (0=0.0 sec, 20=2.0 sec) 
        const byte S_MEM_ANTIVOX = 0x04; // anti VOX setting 
        const byte S_MEM_ATTLEVEL = 0x05; // Attenuation level (0=0%, 255=100%) 
        const byte S_MEM_RIT = 0x06; // RIT (0=off, 1=on, 2=sub dial) 
        const byte S_MEM_SATMODE = 0x07; // Satellite mode (on/off) 
        const byte S_MEM_BANDSCOPE = 0x08; // Simple bandscope (on/off) 


        //
        // Tone control (C_SET_TONE) subcommands

        const byte S_TONE_RPTR = 0x00; // Tone frequency setting for repeater receive 
        const byte S_TONE_SQL = 0x01; // Tone frequency setting for squelch 
        const byte S_TONE_DTCS = 0x02; // DTCS code and polarity for squelch 

        //
        // Transceiver ID (C_RD_TRXID) subcommands

        const byte S_RD_TRXID = 0x00;

        //
        // C_CTL_MISC	OptoScan extension

        const byte S_OPTO_LOCAL = 0x01;
        const byte S_OPTO_REMOTE = 0x02;
        const byte S_OPTO_TAPE_ON = 0x03;
        const byte S_OPTO_TAPE_OFF = 0x04;
        const byte S_OPTO_RDSTAT = 0x05;
        const byte S_OPTO_RDCTCSS = 0x06;
        const byte S_OPTO_RDDCS = 0x07;
        const byte S_OPTO_RDDTMF = 0x08;
        const byte S_OPTO_RDID = 0x09;
        const byte S_OPTO_SPKRON = 0x0a;
        const byte S_OPTO_SPKROFF = 0x0b;
        const byte S_OPTO_5KSCON = 0x0c;
        const byte S_OPTO_5KSCOFF = 0x0d;
        const byte S_OPTO_NXT = 0x0e;
        const byte S_OPTO_SCON = 0x0f;
        const byte S_OPTO_SCOFF = 0x10;

        //
        // OmniVIPlus (Omni VI) extensions

        const byte C_OMNI6_XMT = 0x16;

        //
        // S_MEM_MODE_SLCT	Misc CI-V Mode settings

        const byte S_PRM_BEEP = 0x02;
        const byte S_PRM_CWPITCH = 0x10;
        const byte S_PRM_LANG = 0x15;
        const byte S_PRM_BACKLT = 0x21;
        const byte S_PRM_SLEEP = 0x32;
        const byte S_PRM_SLPTM = 0x33;
        const byte S_PRM_TIME = 0x27;

        #endregion Sub Command Constants

        #endregion Constants

        #region Members

        private byte[ ] OK_Message = { PREAMBLE, PREAMBLE, CONTROLLER, RADIO, OK, EOM };
        private byte[ ] Not_OK_Message = { PREAMBLE, PREAMBLE, CONTROLLER, RADIO, NG, EOM };

        private SerialPort port;

        #endregion Members

        #region Properties

        #endregion Properties

        #region Constructors

        public ICOM756PROIII()
        {

        }

        public ICOM756PROIII( string portname, int baudrate )
        {
            port = new SerialPort( portname, baudrate );
            //port.BaudRate = baudrate;
            //port.DataBits = 8;
            //port.Parity = Parity.None;
            //port.StopBits = StopBits.One;
            //port.PortName = portname;
            //port.RtsEnable = true;
            //port.DtrEnable = true;

            if ( !port.IsOpen )
                port.Open( );

        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Sets Rig's Frequency 
        /// Returns command only
        /// </summary>
        /// <param name="frequency"></param>
        public bool SendFrequencyData( int frequency )
        {

            string sFrequency = frequency.ToString( ).PadLeft( 10, '0' );

            //string s10Hz = sFrequency.Substring(8,1);
            int i10Hz = Convert.ToInt32( sFrequency.Substring( 8, 1 ) );
            //string s1Hz = sFrequency.Substring(9,1);
            int i1Hz = Convert.ToInt32( sFrequency.Substring( 9, 1 ) );
            byte b10Hzand1Hz = toBCD( i10Hz, i1Hz );
            //string sb10Hzand1Hz = ((int)b10Hzand1Hz).ToString("X").PadLeft(2, '0');

            //string s1kHz = sFrequency.Substring(6, 1);
            int i1kHz = Convert.ToInt32( sFrequency.Substring( 6, 1 ) );
            //string s100Hz = sFrequency.Substring(7, 1);
            int i100Hz = Convert.ToInt32( sFrequency.Substring( 7, 1 ) );
            byte b1kHzand100Hz = toBCD( i1kHz, i100Hz );
            //string sb1kHzand100Hz = ((int)b1kHzand100Hz).ToString("X").PadLeft(2, '0');

            //string s100kHz = sFrequency.Substring(4, 1);
            int i100kHz = Convert.ToInt32( sFrequency.Substring( 4, 1 ) );
            //string s10kHz = sFrequency.Substring(5, 1);
            int i10kHz = Convert.ToInt32( sFrequency.Substring( 5, 1 ) );
            byte b100kHzand10kHz = toBCD( i100kHz, i10kHz );
            //string sb100kHzand10kHz = ((int)b100kHzand10kHz).ToString("X").PadLeft(2, '0');

            //string s10MHz = sFrequency.Substring(2, 1);
            int i10MHz = Convert.ToInt32( sFrequency.Substring( 2, 1 ) );
            //string s1MHz = sFrequency.Substring(3, 1);
            int i1MHz = Convert.ToInt32( sFrequency.Substring( 3, 1 ) );
            byte b10MHzand1MHz = toBCD( i10MHz, i1MHz );
            //string sb10MHzand1MHz = ((int)b10MHzand1MHz).ToString("X").PadLeft(2, '0');

            //string s100MHz = sFrequency.Substring(0,1);
            int i100MHz = Convert.ToInt32( sFrequency.Substring( 0, 1 ) );
            //string s1GHz = sFrequency.Substring(1,1);
            int i1GHz = Convert.ToInt32( sFrequency.Substring( 1, 1 ) );
            byte b100MHzand1GHz = toBCD( i100MHz, i1GHz );
            //string sb100MHzand1GHz = ((int)b100MHzand1GHz).ToString("X").PadLeft(2, '0');

            byte[ ] frame = new byte[ ] {  PREAMBLE, PREAMBLE, RADIO, CONTROLLER, CMD_SEND_FREQ,
                b10Hzand1Hz, b1kHzand100Hz, b100kHzand10kHz, b10MHzand1MHz, b100MHzand1GHz, EOM };

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            byte[ ] commandReturn = GetFramesS( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OK = ByteArrayCompare( frame, commandReturn );

            if ( !OK )
                throw new Exception( "Not OK" );

            return OK;

        }

        public bool SendModeData( ICOMEnums.Mode mode )
        {

            //0x01

            //00 Select LSB
            //01 Select USB
            //02 Select AM
            //03 Select CW
            //04 Select RTTY
            //05 Select FM
            //07 Select CW-R
            //08 Select RTTY-R

            byte command = CMD_SEND_MODE;  // 0x01
            byte[ ] subcommand = new byte[ 1 ];

            switch ( mode )
            {
                case ICOMEnums.Mode.LSB:
                    subcommand[ 0 ] = S_LSB;
                    break;
                case ICOMEnums.Mode.USB:
                    subcommand[ 0 ] = S_USB;
                    break;
                case ICOMEnums.Mode.AM:
                    subcommand[ 0 ] = S_AM;
                    break;
                case ICOMEnums.Mode.CW:
                    subcommand[ 0 ] = S_CW;
                    break;
                case ICOMEnums.Mode.RTTY:
                    subcommand[ 0 ] = S_RTTY;
                    break;
                case ICOMEnums.Mode.FM:
                    subcommand[ 0 ] = S_FM;
                    break;
                case ICOMEnums.Mode.FM_WIDE:
                    subcommand[ 0 ] = S_FM;
                    break;
                case ICOMEnums.Mode.CW_REVERSE:
                    subcommand[ 0 ] = S_CWR;
                    break;
                case ICOMEnums.Mode.RTTY_REVERSE:
                    subcommand[ 0 ] = S_RTTYR;
                    break;
                default:
                    subcommand[ 0 ] = S_USB;
                    break;
            }


            byte[ ] frame = MakeCommandFrame( command, subcommand );

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            byte[ ] commandReturn = GetFramesS( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OK = ByteArrayCompare( frame, commandReturn );

            if ( !OK )
                throw new Exception( "Not OK" );

            return OK;



        }

        public List<string> ReadBandEdgeFrequencies()
        {
            // 0x02
            byte command = CMD_READ_BAND;

            List<string> listReturn = new List<string>( );

            byte[ ] frame = MakeCommandFrame( command, new byte[ 0 ] );
            port.Write( frame, 0, frame.Length );
            List<byte[ ]> byteReturn = GetTwoFrames( );

            byte[ ] returnCommand = byteReturn[ 0 ];
            bool OK = ByteArrayCompare( frame, returnCommand );

            if ( !OK )
                throw new Exception( "Bad ReadBandEdgeFrequencies" );

            byte[ ] answer = byteReturn[ 1 ];

            // Parse the frequencies
            byte[ ] freq = new byte[ answer.Length - 5 - 1 ];
            Buffer.BlockCopy( answer, 5, freq, 0, answer.Length - 6 );

            if ( freq[ 5 ] != 0x2D )
                throw new Exception( "Bad Parse" );

            byte[ ] freqLower = new byte[ 5 ];
            Buffer.BlockCopy( freq, 0, freqLower, 0, 5 );
            byte[ ] freqUpper = new byte[ 5 ];
            Buffer.BlockCopy( freq, 6, freqUpper, 0, 5 );

            string sLower = BCDFrequency( freqLower );
            string sUpper = BCDFrequency( freqUpper );

            listReturn.Add( sLower );
            listReturn.Add( sUpper );

            return listReturn;
        }

        /// <summary>
        /// 0x00 Gets the Rig's frequency
        /// </summary>
        /// <returns></returns>
        public string ReadOperatingFrequency()
        {
            byte command = CMD_READ_FREQ;  //0x03
            byte[ ] subcommand = new byte[ 0 ];

            byte[ ] frame = MakeCommandFrame( command, subcommand );
            port.Write( frame, 0, frame.Length );
            List<byte[ ]> byteReturn = GetTwoFrames( );

            byte[ ] returnCommand = byteReturn[ 0 ];
            bool OK = ByteArrayCompare( frame, returnCommand );

            if ( !OK )
                throw new Exception( "Bad ReadBandEdgeFrequencies" );

            byte[ ] answer = byteReturn[ 1 ];

            // We are only interested in the 5 bytes after the command
            int l = Buffer.ByteLength( answer );
            int begin = l - 5 - 1;
            byte[ ] result = new byte[ begin ];
            Buffer.BlockCopy( answer, 5, result, 0, begin );

            string sResult = BCDFrequency( result );

            return sResult;

        }

        public ICOMEnums.Mode ReadOperatingMode()
        {
            //0x04
            byte command = CMD_READ_MODE;
            byte[ ] subcommand = new byte[ 0 ];

            byte[ ] frame = MakeCommandFrame( command, subcommand );
            port.Write( frame, 0, frame.Length );
            List<byte[ ]> byteReturn = GetTwoFrames( );

            byte[ ] returnCommand = byteReturn[ 0 ];
            bool OK = ByteArrayCompare( frame, returnCommand );

            if ( !OK )
                throw new Exception( "Bad ReadOperatingMode" );

            byte[ ] answer = byteReturn[ 1 ];

            byte bMode = answer[ 5 ];
            byte bFilter = answer[ 6 ];
            string sFilter = string.Empty;

            switch ( bFilter )
            {
                case FILTER_WIDTH_WIDE_3:
                    sFilter = "Wide";
                    break;
                case FILTER_WIDTH_MEDIUM_3:
                    sFilter = "Medium";
                    break;
                case FILTER_WIDTH_NARROW_3:
                    sFilter = "Narrow";
                    break;
                default:
                    break;
            }

            ICOMEnums.Mode eMode = ICOMEnums.Mode.AM;

            switch ( bMode )
            {

                //                    const byte S_LSB = 0x00; // Set to LSB 
                //const byte S_USB = 0x01; // Set to USB 
                //const byte S_AM = 0x02; // Set to AM 
                //const byte S_CW = 0x03; // Set to CW 
                //const byte S_RTTY = 0x04; // Set to RTTY 
                //const byte S_FM = 0x05; // Set to FM 
                //const byte S_WFM = 0x06; // Set to Wide FM 
                //const byte S_CWR = 0x07; // Set to CW Reverse 
                //const byte S_RTTYR = 0x08; // Set to RTTY Reverse 

                case S_LSB:
                    eMode = ICOMEnums.Mode.LSB;
                    break;
                case S_USB:
                    eMode = ICOMEnums.Mode.USB;
                    break;
                case S_AM:
                    eMode = ICOMEnums.Mode.AM;
                    break;
                case S_CW:
                    eMode = ICOMEnums.Mode.CW;
                    break;
                case S_RTTY:
                    eMode = ICOMEnums.Mode.RTTY;
                    break;
                case S_FM:
                    eMode = ICOMEnums.Mode.FM;
                    break;
                case S_WFM:
                    eMode = ICOMEnums.Mode.FM_WIDE;
                    break;
                case S_CWR:
                    eMode = ICOMEnums.Mode.CW_REVERSE;
                    break;
                case S_RTTYR:
                    eMode = ICOMEnums.Mode.RTTY_REVERSE;
                    break;
                default:
                    break;
            }


            return eMode;
        }

        public bool SetFrequencyData( int frequency )
        {
            byte command = CMD_SET_FREQ; // 0x05

            string sFrequency = frequency.ToString( ).PadLeft( 10, '0' );

            //string s10Hz = sFrequency.Substring(8,1);
            int i10Hz = Convert.ToInt32( sFrequency.Substring( 8, 1 ) );
            //string s1Hz = sFrequency.Substring(9,1);
            int i1Hz = Convert.ToInt32( sFrequency.Substring( 9, 1 ) );
            byte b10Hzand1Hz = toBCD( i10Hz, i1Hz );
            //string sb10Hzand1Hz = ((int)b10Hzand1Hz).ToString("X").PadLeft(2, '0');

            //string s1kHz = sFrequency.Substring(6, 1);
            int i1kHz = Convert.ToInt32( sFrequency.Substring( 6, 1 ) );
            //string s100Hz = sFrequency.Substring(7, 1);
            int i100Hz = Convert.ToInt32( sFrequency.Substring( 7, 1 ) );
            byte b1kHzand100Hz = toBCD( i1kHz, i100Hz );
            //string sb1kHzand100Hz = ((int)b1kHzand100Hz).ToString("X").PadLeft(2, '0');

            //string s100kHz = sFrequency.Substring(4, 1);
            int i100kHz = Convert.ToInt32( sFrequency.Substring( 4, 1 ) );
            //string s10kHz = sFrequency.Substring(5, 1);
            int i10kHz = Convert.ToInt32( sFrequency.Substring( 5, 1 ) );
            byte b100kHzand10kHz = toBCD( i100kHz, i10kHz );
            //string sb100kHzand10kHz = ((int)b100kHzand10kHz).ToString("X").PadLeft(2, '0');

            //string s10MHz = sFrequency.Substring(2, 1);
            int i10MHz = Convert.ToInt32( sFrequency.Substring( 2, 1 ) );
            //string s1MHz = sFrequency.Substring(3, 1);
            int i1MHz = Convert.ToInt32( sFrequency.Substring( 3, 1 ) );
            byte b10MHzand1MHz = toBCD( i10MHz, i1MHz );
            //string sb10MHzand1MHz = ((int)b10MHzand1MHz).ToString("X").PadLeft(2, '0');

            //string s100MHz = sFrequency.Substring(0,1);
            int i100MHz = Convert.ToInt32( sFrequency.Substring( 0, 1 ) );
            //string s1GHz = sFrequency.Substring(1,1);
            int i1GHz = Convert.ToInt32( sFrequency.Substring( 1, 1 ) );
            byte b100MHzand1GHz = toBCD( i100MHz, i1GHz );
            //string sb100MHzand1GHz = ((int)b100MHzand1GHz).ToString("X").PadLeft(2, '0');

            byte[ ] frame = new byte[ ] {  PREAMBLE, PREAMBLE, RADIO, CONTROLLER, command,
                b10Hzand1Hz, b1kHzand100Hz, b100kHzand10kHz, b10MHzand1MHz, b100MHzand1GHz, EOM };

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            List<byte[ ]> byteReturn = GetTwoFrames( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OKcommand = ByteArrayCompare( frame, byteReturn[ 0 ] );

            // See if you get an OK response back
            bool OKresponse = ByteArrayCompare( OK_Message, byteReturn[ 1 ] );

            return (OKcommand && OKresponse);

        }

        public bool SetModeData( ICOMEnums.Mode mode )
        {
            byte command = CMD_SET_MODE; //0x06

            byte[ ] subcommand = new byte[ 1 ];

            switch ( mode )
            {
                case ICOMEnums.Mode.LSB:
                    subcommand[ 0 ] = S_LSB;
                    break;
                case ICOMEnums.Mode.USB:
                    subcommand[ 0 ] = S_USB;
                    break;
                case ICOMEnums.Mode.AM:
                    subcommand[ 0 ] = S_AM;
                    break;
                case ICOMEnums.Mode.CW:
                    subcommand[ 0 ] = S_CW;
                    break;
                case ICOMEnums.Mode.RTTY:
                    subcommand[ 0 ] = S_RTTY;
                    break;
                case ICOMEnums.Mode.FM:
                    subcommand[ 0 ] = S_FM;
                    break;
                case ICOMEnums.Mode.FM_WIDE:
                    subcommand[ 0 ] = S_FM;
                    break;
                case ICOMEnums.Mode.CW_REVERSE:
                    subcommand[ 0 ] = S_CWR;
                    break;
                case ICOMEnums.Mode.RTTY_REVERSE:
                    subcommand[ 0 ] = S_RTTYR;
                    break;
                default:
                    subcommand[ 0 ] = S_USB;
                    break;
            }


            byte[ ] frame = MakeCommandFrame( command, subcommand );

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            List<byte[ ]> byteReturn = GetTwoFrames( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OKcommand = ByteArrayCompare( frame, byteReturn[ 0 ] );

            // See if you get an OK response back
            bool OKresponse = ByteArrayCompare( OK_Message, byteReturn[ 1 ] );

            return (OKcommand && OKresponse);

        }

        public bool SelectVFOMode( ICOMEnums.VFOValues value )
        {
            byte command = CMD_SET_VFO; //0x07
            byte[ ] subcommand = new byte[ 1 ];

            switch ( value )
            {
                case ICOMEnums.VFOValues.SelectVFOMode:
                    subcommand = new byte[ 0 ];
                    break;
                case ICOMEnums.VFOValues.ExchangeMainAndSubReadouts:
                    subcommand[ 0 ] = 0xB0;
                    break;
                case ICOMEnums.VFOValues.EqualizeMainAndSubReadouts:
                    subcommand[ 0 ] = 0xB1;
                    break;
                case ICOMEnums.VFOValues.TurnTheDualwatchOFF:
                    subcommand[ 0 ] = 0xC0;
                    break;
                case ICOMEnums.VFOValues.TurnTheDualwatchON:
                    subcommand[ 0 ] = 0xC1;
                    break;
                case ICOMEnums.VFOValues.SelectMainReadout:
                    subcommand[ 0 ] = 0xD0;
                    break;
                case ICOMEnums.VFOValues.SelectSubReadout:
                    subcommand[ 0 ] = 0xD1;
                    break;
                default:
                    break;
            }

            //B0 Exchange main and sub readouts
            //B1 Equalize main and sub readouts
            //C0 Turn the dualwatch OFF
            //C1 Turn the dualwatch ON
            //D0 Select main readout
            //D1 Select sub readout

            byte[ ] frame = MakeCommandFrame( command, subcommand );

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            List<byte[ ]> byteReturn = GetTwoFrames( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OKcommand = ByteArrayCompare( frame, byteReturn[ 0 ] );

            // See if you get an OK response back
            bool OKresponse = ByteArrayCompare( OK_Message, byteReturn[ 1 ] );

            return (OKcommand && OKresponse);


        }

        public bool SelectMemoryMode( int memoryId )
        {
            byte command = CMD_SET_MEMORY; //0x08
            byte[ ] subcommand = new byte[ 0 ];

            if ( memoryId > 0 && memoryId <= 99 )
            {
                string smemoryId = memoryId.ToString( ).PadLeft( 2, '0' );
                int intOne = Convert.ToInt32( smemoryId.Substring( 0, 1 ) );
                int intTwo = Convert.ToInt32( smemoryId.Substring( 1, 1 ) );
                byte bcd = toBCD( intOne, intTwo );
                subcommand = new byte[ 2 ];
                subcommand[ 0 ] = 0x00;
                subcommand[ 1 ] = bcd;
            }

            if ( memoryId > 99 && memoryId < 102 )
            {
                subcommand = new byte[ 2 ];
                subcommand[ 0 ] = 0x01;
                if ( memoryId == 100 )
                    subcommand[ 1 ] = 0x00;
                else
                    subcommand[ 1 ] = 0x01;
            }

            byte[ ] frame = MakeCommandFrame( command, subcommand );

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            List<byte[ ]> byteReturn = GetTwoFrames( );

            // Make sure you are getting back the original command
            // that was sent to the serial port.
            bool OKcommand = ByteArrayCompare( frame, byteReturn[ 0 ] );

            // See if you get an OK response back
            bool OKresponse = ByteArrayCompare( OK_Message, byteReturn[ 1 ] );

            return (OKcommand && OKresponse);


        }

        //public void MemoryWrite()
        //{
        //    byte command = CMD_WRITE_MEM; //0x09
        //}

        //public void MemoryToVFO()
        //{
        //    byte command = CMD_MEM_TO_VFO; //0x0A
        //}

        //public void MemoryClear()
        //{
        //    byte command = CMD_CLEAR_MEMORY; //0x0B
        //}

        //public void Scan()
        //{
        //    byte command = CMD_CTL_SCAN; //0x0E
        //}

        //public void Split()
        //{
        //    byte command = CMD_CTL_SPLT; //0x0F
        //}

        //public void SelectTuningStep()
        //{
        //    byte command = CMD_SET_TUNING_STEP; //0x10
        //}

        //public void SetAttenuator()
        //{
        //    byte command = CMD_ATTENUATOR; //0x11
        //}

        //public void SetAntenna()
        //{
        //    byte command = CMD_ANTENNA; //0x12
        //}

        //public void SetVoiceSynthesizer()
        //{
        //    byte command = CMD_VOICE_SYNTHESIZER; //0x13
        //}

        //public void SetLevels()
        //{
        //    byte command = CMD_LEVELS; //0x14
        //}

        //public void ReadMeterLevels()
        //{
        //    byte command = CMD_READ_METER_LEVELS; //0x15
        //}

        //public void SetFunction()
        //{
        //    byte command = CMD_FUNCTIONS; //0x16
        //}

        //public void ReadTranceiverID()
        //{
        //    byte command = CMD_READ_TRANCEIVER_ID; //0x19
        //    IcomTransactionSS(command, new byte[0]);


        //}

        public void SetMisc()
        {
            byte command = CMD_MISC; //0x1A

            byte[ ] subcommand = new byte[ 3 ];
            subcommand[ 0 ] = 0x00;
            subcommand[ 1 ] = 0x00;
            byte waltham = 0x03;
            //byte PEPPERELL = 0x04;
            //byte PEPPERELLx = 0x05;
            subcommand[ 2 ] = waltham; // 0x10;

            byte[ ] frame = MakeCommandFrame( command, subcommand );

            port.Write( frame, 0, frame.Length );

            // The response is the command echoed back without NG or OK response
            List<byte[ ]> byteReturn = GetTwoFrames( );

            byte[ ] returndata = byteReturn[ 1 ];

            byte whatisthisfor = returndata[ 8 ];  //split select ???

            // frequency
            byte[ ] frequencyBlock = new byte[ 5 ];
            Buffer.BlockCopy( returndata, 9, frequencyBlock, 0, 5 );
            string sFrequency = BCDFrequency( frequencyBlock );

            //mode ??
            byte modeBlock = returndata[ 14 ];  // Mode

            // IF Width
            byte IFWidth = returndata[ 15 ];  // Filter Width ?

            // Duplex 
            byte duplex = returndata[ 16 ];  // Duplex (OFF/-/+)

            // WTF is THIS
            byte[ ] wtf = new byte[ 6 ];
            Buffer.BlockCopy( returndata, 17, wtf, 0, 6 );


            //// Tone
            //byte tone = returndata[17]; // Tone (OFF/ON/TSQL)

            //byte[] tonefrequency = new byte[3];
            //Buffer.BlockCopy(returndata, 18, tonefrequency, 0, 3);

            //byte[] tsqlfrequency = new byte[6];
            //Buffer.BlockCopy(returndata, 25, tonefrequency, 0, 6);

            byte[ ] text = new byte[ 10 ];
            Buffer.BlockCopy( returndata, 23, text, 0, 10 );

            string myString = System.Text.Encoding.ASCII.GetString( text );
            //string myString = enc.GetString(text);


        }




        /// <summary>
        /// Changes the Rig's mode
        /// </summary>
        /// <param name="modename"></param>
        /// <returns>true if successful</returns>
        public bool ChangeMode( string modename )
        {

            //00 Select LSB
            //01 Select USB
            //02 Select AM
            //03 Select CW
            //04 Select RTTY
            //05 Select FM
            //07 Select CW-R
            //08 Select RTTY-R

            //byte command = CMD_SET_MODE;
            byte[ ] subcommand = new byte[ 1 ];
            switch ( modename )
            {
                case "LSB":
                    subcommand[ 0 ] = S_LSB;
                    break;
                case "USB":
                    subcommand[ 0 ] = S_USB;
                    break;
                case "AM":
                    subcommand[ 0 ] = S_AM;
                    break;
                case "CW":
                    subcommand[ 0 ] = S_CW;
                    break;
                case "RTTY":
                    subcommand[ 0 ] = S_RTTY;
                    break;
                case "FM":
                    subcommand[ 0 ] = S_FM;
                    break;
                case "CW-R":
                    subcommand[ 0 ] = S_CWR;
                    break;
                case "RTTY-R":
                    subcommand[ 0 ] = S_RTTYR;
                    break;

                default:
                    subcommand[ 0 ] = S_USB;
                    break;
            }

            //bool bReturn = IcomTransactionSS(command, subcommand);

            return true;

        }



        #endregion Public Methods

        #region Member Methods

        /// <summary>
        /// Set or Send
        /// Will return original command AND OK or NG
        /// </summary>
        /// <param name="command"></param>
        /// <param name="subcommand"></param>
        /// <returns></returns>
        //private bool IcomTransactionSS(byte command, byte[] subcommand)
        //{

        //    //byte[] toSend = MakeFrame(command, subcommand);
        //    SendFrame(command, subcommand);

        //    bool bReturn = GetFramesSS();

        //    return bReturn;
        //}

        /// <summary>
        /// Returns command in the first frame
        /// and the OK or NG message in the second frame.
        /// </summary>
        /// <returns></returns>
        private List<byte[ ]> GetTwoFrames()
        {

            List<byte[ ]> list = new List<byte[ ]>( );
            byte[ ] buffer = new byte[ 100 ];
            byte[ ] frame1 = new byte[ 0 ];
            byte[ ] frame2 = new byte[ 0 ];
            int eomCount = 0;
            int i = 0;

            while ( eomCount != 3 )
            {
                byte b = ( byte )port.ReadByte( );

                //if (b == 0x00)
                //    continue;

                buffer[ i ] = b;
                i++;

                if ( b == EOM )
                    eomCount++;

                if ( eomCount == 1 )
                {
                    frame1 = new byte[ i ];
                    Buffer.BlockCopy( buffer, 0, frame1, 0, i );
                    list.Add( frame1 );
                    i = 0;
                    eomCount = 2;
                    buffer = new byte[ 100 ];
                }

                if ( eomCount == 3 )
                {
                    frame2 = new byte[ i ];
                    Buffer.BlockCopy( buffer, 0, frame2, 0, i );
                    list.Add( frame2 );
                }
            }

            //// Now that you got the two frames
            //// see if ok_ng is OK or No Good
            //bool OK = ByteArrayCompare(ok_ng, OK_Message);



            return list;
        }

        /// <summary>
        /// Retrieves the first EOM frame only.
        /// Some responses only echo the command back.
        /// </summary>
        /// <returns></returns>
        private byte[ ] GetFramesS()
        {
            byte[ ] buffer = new byte[ 100 ];
            byte[ ] command = new byte[ 0 ];
            int eomCount = 0;
            int i = 0;

            while ( eomCount != 1 )
            {
                byte b = ( byte )port.ReadByte( );

                //if (b == 0x00)
                //    continue;

                buffer[ i ] = b;
                i++;

                if ( b == EOM )
                    eomCount++;

                if ( eomCount == 1 )
                {
                    command = new byte[ i ];
                    Buffer.BlockCopy( buffer, 0, command, 0, i );
                }

            }

            return command;
        }

        private string GetFrameFrequency()
        {
            byte[ ] buffer = new byte[ 100 ];
            byte[ ] command = new byte[ 0 ];
            byte[ ] freq = new byte[ 0 ];

            int eomCount = 0;
            int i = 0;

            while ( eomCount != 3 )
            {
                byte b = ( byte )port.ReadByte( );

                buffer[ i ] = b;
                i++;

                if ( b == EOM )
                    eomCount++;

                if ( eomCount == 1 )
                {
                    command = new byte[ i ];
                    Buffer.BlockCopy( buffer, 0, command, 0, i );
                    i = 0;
                    eomCount = 2;
                    buffer = new byte[ 100 ];
                }

                if ( eomCount == 3 )
                {
                    freq = new byte[ i ];
                    Buffer.BlockCopy( buffer, 0, freq, 0, i );
                }
            }

            // The only one you are interested he is the 5 byte before the end of last frame

            int l = Buffer.ByteLength( freq );
            int begin = l - 5 - 1;
            byte[ ] result = new byte[ begin ];
            Buffer.BlockCopy( freq, 5, result, 0, begin );

            string sResult = BCDFrequency( result );

            return sResult;

        }

        private bool ByteArrayCompare( byte[ ] a1, byte[ ] a2 )
        {
            IStructuralEquatable eqa1 = a1;
            return eqa1.Equals( a2, StructuralComparisons.StructuralEqualityComparer );
        }

        private void SendFrame( byte command, byte[ ] subcommand )
        {
            byte[ ] frame = MakeCommandFrame( command, subcommand );
            port.Write( frame, 0, frame.Length );
        }

        private byte[ ] MakeCommandFrame( byte command, byte[ ] subcommand )
        {
            // your basic frame will have 5 plus the subcommand bytes

            byte[ ] frame = new byte[ 6 + subcommand.Length ];

            frame[ 0 ] = PREAMBLE;
            frame[ 1 ] = PREAMBLE;
            frame[ 2 ] = RADIO;
            frame[ 3 ] = CONTROLLER;
            frame[ 4 ] = command;

            for ( int x = 0; x < subcommand.Length; x++ )
                frame[ x + 5 ] = subcommand[ x ];

            frame[ frame.Length - 1 ] = EOM;

            return frame;
        }

        private string BCDFrequency( byte[ ] frame )
        {

            string[ ] aResult = new string[ frame.Length ];

            for ( int x = 0; x < frame.Length; x++ )
                aResult[ x ] = (( int )frame[ x ]).ToString( "X" ).PadLeft( 2, '0' );

            string result = string.Empty;

            int end = aResult.Length - 1;

            for ( int y = end; y >= 0; y-- )
                result += aResult[ y ];

            return result;

        }

        private Byte toBCD( int ileft, int iright )
        {
            return Convert.ToByte( ileft * 16 + iright );
        }

        #endregion Member Methods

    }
}
