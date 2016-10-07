using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigControl
{

    class Program
    {
        static void Main( string[ ] args )
        {


            Test t = new Test( );

            //bool bReturn = t.TestSendFrequencyData(14070000);  //0x00
            //Console.WriteLine(bReturn);

            //bool bReturn = t.TestSendModeData(ICOMEnums.Mode.USB);  // 0x01
            //Console.WriteLine(bReturn);
            //return;


            //List<string> ul = t.TestReadBandEdgeFrequencies(); //0x02
            //Console.WriteLine("Lower:{0} Upper:{1}", ul[0], ul[1]);
            //return;

            //string freq = t.TestReadOperatingFrequency(); //0x03
            //Console.WriteLine(freq);
            //return;

            //ICOMEnums.Mode mode = t.TestReadOperatingMode(); //0x04
            //Console.WriteLine(mode.ToString());
            //return;

            //bool bReturn = t.TestSetFrequencyData(14070002); //0x05
            //string freq = t.TestReadOperatingFrequency(); //0x03
            //Console.WriteLine(freq);
            //return;


            //bool bReturn = t.TestSetModeData(ICOMEnums.Mode.USB); //0x06
            //Console.WriteLine(bReturn);
            //ICOMEnums.Mode mode = t.TestReadOperatingMode(); //0x04
            //Console.WriteLine(mode.ToString());
            //return;

            //bool bReturn = t.TestSelectVFOMode(ICOMEnums.VFOValues.SelectVFOMode); //0x07
            //Console.WriteLine(bReturn);
            //return;

            //for (int i = 1; i < 102; i++)
            //{
            //    bool bReturn = t.TestSelectMemoryMode(i); //0x08
            //    Console.WriteLine("Result:{0} Channel:{1}", bReturn, i);
            //    System.Threading.Thread.Sleep(0250);
            //}

            //return;


            //t.TestMemoryWrite(); //0x09
            //t.TestMemoryToVFO(); //0x0A
            //t.TestMemoryClear(); //0x0B
            //t.TestScan(); //0x0E
            //t.TestSplit(); //0x0F
            //t.TestSelectTuningStep(); //0x10
            //t.TestSetAttenuator(); //0x11
            //t.TestSetAntenna(); //0x12
            //t.TestSetVoiceSynthesizer(); //0x13
            //t.TestSetLevels(); //0x14
            //t.TestReadMeterLevels(); //0x15
            //t.TestSetFunction(); //0x16
            //t.TestReadTransceiverID(); //0x19CMD_MISC
            t.TestSetMisc( ); //0x1A













            /*
             * Testing GetFrequency()
            while (true)
            {
                string freq = pro3.GetFreqency();

                Console.WriteLine(freq);
                
            }
             * /


            /*
             *  Testing Mode Changing
            string[] input = { "LSB", "AM", "CW", "RTTY", "FM", "CW-R", "RTTY-R", "USB", "CW" };

            List<string> modes = new List<string>(input);

            ICOM756PROIII pro3 = new ICOM756PROIII("COM4", 19200);

            foreach (string mode in modes)
            {
                bool result = pro3.ChangeMode(mode);
                Console.WriteLine(string.Format("Mode:{0} Result:{1}", mode, result));
                // Wait 5 second
                System.Threading.Thread.Sleep(5000);
            }
            */

        }
    }
}


//string[] ports = SerialPort.GetPortNames();

//SerialPort port = new SerialPort("COM3");


////ComPort cp = new ComPort("COM3");


////port.Open();
//int baudrate = port.BaudRate;
//bool breakstate = port.BreakState;
//bool cdholding = port.CDHolding;
//bool ctsholding = port.CtsHolding;
//int databits = port.DataBits;
//bool discardnull = port.DiscardNull;
//bool dsrholding = port.DsrHolding;
//bool dtrenable = port.DtrEnable;
//Encoding encoding = port.Encoding;
//Handshake handshake = port.Handshake;
//bool isopen = port.IsOpen;
//string newline = port.NewLine;
//Parity parity = port.Parity;
//byte parityreplace = port.ParityReplace;
//string portname = port.PortName;
//int readbuffersize = port.ReadBufferSize;
//int timeout = port.ReadTimeout;
//int receivedbytesthreshold = port.ReceivedBytesThreshold;
//bool rtsenable = port.RtsEnable;
//StopBits stopbits = port.StopBits;
//int writebuffersize = port.WriteBufferSize;
//int writetimeout = port.WriteTimeout;

//00 Select LSB
//01 Select USB
//02 Select AM
//03 Select CW
//04 Select RTTY
//05 Select FM
//07 Select CW-R
//08 Select RTTY-R



//}
