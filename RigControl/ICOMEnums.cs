using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigControl
{
    public static class ICOMEnums
    {

        public enum VFOValues
        {
            SelectVFOMode,
            ExchangeMainAndSubReadouts,
            EqualizeMainAndSubReadouts,
            TurnTheDualwatchOFF,
            TurnTheDualwatchON,
            SelectMainReadout,
            SelectSubReadout
        }

        public enum Mode
        {
            LSB,
            USB,
            AM,
            CW,
            RTTY,
            FM,
            FM_WIDE,
            CW_REVERSE,
            RTTY_REVERSE
        }



    }
}
