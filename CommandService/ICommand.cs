using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        string Usage { get; }   
        PermissionLevel PermissionLevelDefault { get; }
        bool Validate(Player player, string[] args);
        bool Execute(Player player, string[] args);
    }
}
