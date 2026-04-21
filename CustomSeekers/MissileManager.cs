using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImprovedMissiles.States;

namespace ImprovedMissiles.Utils
{
    public static class MissileManager
    {
        public static Dictionary<IRSeeker, IRSeekerBehaviour> missilesPool = new Dictionary<IRSeeker, IRSeekerBehaviour>();
        public static void Add(IRSeeker seeker, IRSeekerBehaviour missileBehaviour)
        {
            missilesPool.Add(seeker, missileBehaviour);
        }

        public static void Remove(IRSeeker seeker)
        {
            missilesPool.Remove(seeker);
        }

        public static IRSeekerBehaviour GetBehaviour(IRSeeker seeker)
        {
            return missilesPool[seeker];
        }
    }
}
