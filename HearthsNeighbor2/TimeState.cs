using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthsNeighbor2
{
    public enum TimeState
    {
        /// <summary>
        /// First 8 minutes, all systems online
        /// </summary>
        Normal,
        /// <summary>
        /// 8-14 minutes, gravity weakens, starts at 14 minutes remaining
        /// </summary>
        LowGrav,
        /// <summary>
        /// 14-19 minutes, gravity off, starts at 8 minutes remaining
        /// </summary>
        NoGrav,
        /// <summary>
        /// 19-21 minutes, red lights during emergency power, starts at 3 minutes remaining
        /// </summary>
        RedLights,
        /// <summary>
        /// 20+ minutes, power completely dead, starts at 2 minutes remaining
        /// </summary>
        Dead
    }
}
