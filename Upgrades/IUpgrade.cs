using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTools.Upgrades
{
    public interface IUpgrade
    {
        string ModuleName { get; }
        int ModuleLevel { get; }
        UpgradeType UpgradeType { get; }

        void Apply(Unit unit);
    }
}
