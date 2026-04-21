using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ServerTools.IPC;
using ServerTools.Utils;

namespace ServerTools.Upgrades.ConcreteUpgrades
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
                

                ReflectionUtils.WritePrivateField<float>("supersonicDrag",
                    ReflectionUtils.ReadPrivateField<float>("supersonicDrag", missile) * 0.4f, missile);
                
                ReflectionUtils.WritePrivateField<float>("blastYield",
                    ReflectionUtils.ReadPrivateField<float>("blastYield", missile) * 3.3f, missile);
                
                ApplyMotorUpgrade(missile);

                IPCService.BroadcastChannel("/weaponTest", $"Speed+blastdmg upgrade applied to missile {missile.definition.unitName}");
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
                
                
                object[] motors = MissileHelper.GetMotors(missile);
                foreach (var motor in motors)
                {
                    float thrust = MissileHelper.GetThrust(motor);
                    MissileHelper.SetThrust(motor, thrust * 1.5f);
                    MissileHelper.SetThrustVectoring(motor, 30);
                }
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError(ex.Message);
            }
            
        }
    }
}
