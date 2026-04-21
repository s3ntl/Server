using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImprovedMissiles.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace ImprovedMissiles.States
{
    public class CustomIRSeekerTest : IRSeekerBehaviour
    {
        public static float fovAngle = 0.75f;
        public static TimeSpan delay = TimeSpan.FromMilliseconds(200);
        public CustomIRSeekerTest(IRSeeker irSeeker) : base(irSeeker)
        {
            Plugin.logger.LogInfo("Adding");
            
            MissileManager.Add(irSeeker, this);
            Plugin.logger.LogInfo("Added");
        }
        public async Task AsyncWaitDelay(TimeSpan timeout)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(timeout);
            });
        }
        public override async void IR_Seeker_OnTargetFlare(IRSource source)
        {
            Plugin.logger.LogInfo($"[{DateTime.Now}] before delay");

            await AsyncWaitDelay(delay);
             
            Plugin.logger.LogInfo($"[{DateTime.Now}] after delay");

            float angle = Vector3.Angle(targetUnit.transform.position - this.missile.transform.position, source.transform.position - this.missile.transform.position);
            Plugin.logger.LogInfo($"Angle: {angle}");   
            if (angle < fovAngle)
            {
                base.LoseLock();
                WritePrivateField<IRSource>("IRTarget", null);
            }
        }
    }
}
