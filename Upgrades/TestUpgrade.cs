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

            try
            {
                
                if (!(unit is Missile missile)) return;
                
                if (!missile.definition.unitName.ToLower().Contains("aam-29")) return;
                Plugin.logger.LogInfo("3");

                ReflectionUtils.WritePrivateField<float>("supersonicDrag",
                    ReflectionUtils.ReadPrivateField<float>("supersonicDrag", missile) * 0.01f, missile);
                Plugin.logger.LogInfo("4");
                ReflectionUtils.WritePrivateField<float>("blastYield",
                    ReflectionUtils.ReadPrivateField<float>("blastYield", missile) * 1000, missile);
                Plugin.logger.LogInfo("5");
                ApplyMotorUpgrade(missile);
            }
            catch (Exception e)
            {
                Plugin.logger.LogInfo($"Error in upgrade apply: {e.Message}");
            }
        }

        private void ApplyMotorUpgrade(Missile missile)
        {

            try
            {
                Plugin.logger.LogInfo("6");
                
                object[] motors = MissileMotorReflection.GetMotors(missile);
                foreach (var motor in motors)
                {
                    float thrust = MissileMotorReflection.GetThrust(motor);
                    MissileMotorReflection.SetThrust(motor, thrust * 50);
                }
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError(ex.Message);
            }
            
        }
    }
}
