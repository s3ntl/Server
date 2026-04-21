using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTools.IPC;
using ServerTools.Utils;
using UnityEngine;

namespace ServerTools.Upgrades.ConcreteUpgrades
{
    public class TestUpgrade2 : IUpgrade
    {
        public string ModuleName { get; } = "maneuverabilityTest";

        public int ModuleLevel { get; } = 1;

        public UpgradeType UpgradeType { get; } = UpgradeType.Missile;

        public void Apply(Unit unit)
        {
            try
            {
                if (!(unit is Missile missile)) return;
                if (!missile.definition.unitName.ToLower().Contains("aam-29")) return;

                // missile.SetTorque(missile.GetTorque() * 2, 200);

                missile.SetGLimit(missile.GetGLimit() * 2);

                var lift = missile.GetLiftCurve();
                var newLift = new AnimationCurve();
                foreach (var key in lift.keys)
                    newLift.AddKey(key.time, key.value * 6f);
                missile.SetLiftCurve(newLift);

                IPCService.BroadcastChannel("/weaponTest", "Agility upgrade applied to aam-29");
            }
            catch (Exception ex)
            {
                IPCService.BroadcastChannel("/weaponTest", "exception in testupgrade2");
            }
        }
    }
}
