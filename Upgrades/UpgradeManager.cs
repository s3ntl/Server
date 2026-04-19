using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using Dapper;
using NuclearOption.Networking;
using Microsoft.SqlServer.Server;

namespace ServerTools.Upgrades
{
    public class UpgradeManager
    {
        private static UpgradeManager _instance;
        public static UpgradeManager Instance 
        { 
            get
            {
                if (_instance == null) _instance = new UpgradeManager();
                return _instance;
            }
        }
        private string _connectionString;
        private Dictionary<ulong, List<IUpgrade>> upgradesMap = new();
        private List<IUpgrade> registeredUpgrades = new();
        public void Awake() 
        {
            Subscribe();
            RegisterUpgrade(new TestUpgrade());
        }
        private void RegisterUpgrade(IUpgrade upgrade)
        {
            registeredUpgrades.Add(upgrade);
        }
        private void Subscribe()
        {
            Patches.MissilePatch.OnMissileAwake += OnMissileAwake;
        }
        private async void OnPlayerJoinedAsync(Player player)
        {
            try
            {
                if (upgradesMap.ContainsKey(player.SteamID))
                    return;

                var models = await GetMissileUpgradesBySteamIdAsync(player.SteamID);
                var upgrades = new List<IUpgrade>();

                foreach (var model in models)
                {
                    
                    if (TryGetUpgrade(model, out var upgrade))
                        upgrades.Add(upgrade);
                }

                upgradesMap[player.SteamID] = upgrades;
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError($"Не удалось загрузить апгрейды для {player.SteamID}: {ex}");
            }
        }

        private bool TryGetUpgrade(MissileUpgradeModel model, out IUpgrade upgrade)
        {
            upgrade = null;
            foreach(var item in registeredUpgrades)
            {
                if (model.Name == item.ModuleName)
                {
                    upgrade = item;
                    return true;
                }
            }
            return false;
        }

        private void OnMissileAwake(Missile missile)
        {
            //пока для теста применяю все апгрейды из пула, потом буду из бд вытаскивать
            foreach (var upgrade in registeredUpgrades)
            {
                if (upgrade.UpgradeType == UpgradeType.Missile) upgrade.Apply(missile);
            }
        }

        public class MissileUpgradeModel
        {
            public ulong SteamID;
            public string Name;
        }
        public async Task<IEnumerable<MissileUpgradeModel>> GetMissileUpgradesBySteamIdAsync(ulong steamId)
        {
            const string sql = @"
            SELECT SteamId, Name
            FROM PlayerUpgrades
            WHERE SteamId = @SteamId
            "; // пример запроса, модель пока что запросу не соответствует

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<MissileUpgradeModel>(sql, new { SteamId = steamId });
            }
        }
    }
}
