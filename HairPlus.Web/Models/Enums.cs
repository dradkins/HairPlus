using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public enum GenderEnum
    {
        Male = 1,
        Female = 2
    };

    public enum PriorityEnum
    {
        High = 1,
        Low = 2
    };

    public enum StatusEnum
    {
        Done = 1,
        In_Process = 2
    };

    public enum CustomerTypeEnum
    {
        Surgical = 1,
        NonSurgical = 2
    };


    public enum LengthEnum
    {
        One = 1,
        Two = 2,
        Two_A = 3,
        Three = 4,
        Four = 5,
        Four_A = 6,
        Five = 7,
        Six = 8
    };

    public enum CurlyEnum
    {
        Straight = 1,
        MM40 = 2,
        MM36 = 3,
        MM30 = 4,
        MM25 = 5,
        MM20 = 6,
        MM18 = 7,
        MM13 = 8,
        MM10 = 9,
        MM6 = 10,
        MM4 = 11,
    }

    public enum DensityEnum
    {
        Super_Light = 1,
        Light = 2,
        Medium = 3,
        Midium_Heavy = 4,
        Heavy = 5,
        Super_Heavy = 6
    }

    public enum InvoiceTypeEnum
    {
        Surgical = 1,
        NonSurgical = 2,
        Maintanance = 3
    };
}