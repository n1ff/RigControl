using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace RigControl
{
    public class Test
    {

        private ICOM756PROIII pro3 = new ICOM756PROIII( "COM4", 19200 );



        public Test() { }

        //0x00
        public bool TestSendFrequencyData( int iT )
        {
            bool OK = pro3.SendFrequencyData( iT );


            return OK;
            ////pro3.SetFrequency(iT+1);
            //for (int x = 1; x <= 10000; x++)
            //{
            //   pro3.SendFrequencyData(iT+x);
            //   Console.WriteLine(pro3.ReadOperatingFrequency());
            //   //system.Threading.Thread.Sleep(2000);

            //}
        }

        //0x01
        public bool TestSendModeData( ICOMEnums.Mode mode )
        {
            // 0x01


            //string[] input = { "LSB", "AM", "CW", "RTTY", "FM", "CW-R", "RTTY-R", "USB", "CW" };

            //List<string> modes = new List<string>(input);


            bool result = pro3.SendModeData( mode );

            return result;
            //// Wait 5 second
            //System.Threading.Thread.Sleep(5000);



        }

        //0x02
        public List<string> TestReadBandEdgeFrequencies() { return pro3.ReadBandEdgeFrequencies( ); }

        //0x03
        public string TestReadOperatingFrequency() { return pro3.ReadOperatingFrequency( ); }

        //0x04
        public ICOMEnums.Mode TestReadOperatingMode() { return pro3.ReadOperatingMode( ); }

        //0x05
        public bool TestSetFrequencyData( int iT ) { return pro3.SetFrequencyData( iT ); }

        //0x06
        public bool TestSetModeData( ICOMEnums.Mode mode ) { return pro3.SetModeData( mode ); }

        //0x07
        public bool TestSelectVFOMode( ICOMEnums.VFOValues vfovalue ) { return pro3.SelectVFOMode( vfovalue ); }

        //0x08
        public bool TestSelectMemoryMode( int memoryaddress ) { return pro3.SelectMemoryMode( memoryaddress ); }

        //0x09
        public void TestMemoryWrite() { }

        //0x0A
        public void TestMemoryToVFO() { }

        //0x0B
        public void TestMemoryClear() { }

        // See 0x0C  && 0x0D abouve in command constants. (Not documented)

        //0x0E
        public void TestScan() { }

        //0x0F
        public void TestSplit() { }

        //0x10
        public void TestSelectTuningStep() { }

        //0x11
        public void TestSetAttenuator() { }

        //0x12
        public void TestSetAntenna() { }

        //0x13
        public void TestSetVoiceSynthesizer() { }

        //0x14
        public void TestSetLevels() { }

        //0x15
        public void TestReadMeterLevels() { }

        //0x16
        public void TestSetFunction() { }

        //0x17 Send CW Message

        //0x18 Power ON/OFF

        //0x19
        public void TestReadTransceiverID()
        {
            //pro3.ReadTranceiverID();          
        }

        //0x1A
        public void TestSetMisc() { pro3.SetMisc( ); }



    }
}
