using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ServerTools.Utils;

namespace ServerTools.Upgrades
{
    public class TestUpgrade : IUpgrade
    {
        public string ModuleName { get;  } = "test";
        public int ModuleLevel { get; } = 1;
        public UpgradeType UpgradeType { get; } = UpgradeType.Missile;

        public void Apply(Unit unit)
        {
            if (!(unit is Missile missile)) return;
            if (!missile.unitName.ToLower().Contains("aam-29")) return; 
            missile.airDensity *= 0.5f;
            
            ReflectionUtils.WritePrivateField<float>("supersonicDrag", 
                ReflectionUtils.ReadPrivateField<float>("supersonicDrag", missile) * 0.01f, missile);

            ReflectionUtils.WritePrivateField<float>("blastYield",
                ReflectionUtils.ReadPrivateField<float>("blastYield", missile) * 1000, missile);

            ApplyMotorUpgrade(missile);
        }

        private void ApplyMotorUpgrade(Missile missile)
        {
            try
            {
                float thrust = MissileMotorReflection.GetThrust(missile);
                object[] motors = MissileMotorReflection.GetMotors(missile);
                foreach (var motor in motors) MissileMotorReflection.SetThrust(motor, thrust * 50);
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError(ex.Message);
            }
        }
    }
}
