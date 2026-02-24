using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NuclearOption.Networking;
using System;
using ServerTools.Data;
using System.Security.Cryptography;



namespace ServerTools.Services
{
    public static class CustomSpawner
    {
        private static List<UnitDefinition> missileDefinitions = new List<UnitDefinition>();
        private static List<UnitDefinition> allUnits = new List<UnitDefinition>();
        

        private static bool _defenitionsInited = false;
        public static void Awake()
        {
            Plugin.logger.LogInfo("CustomSpawner awake");
        }

        private static void InitDefenitions()
        {
            try
            {
                allUnits.AddRange(Resources.FindObjectsOfTypeAll<UnitDefinition>().ToList());
                missileDefinitions.AddRange(Resources.FindObjectsOfTypeAll<MissileDefinition>().ToList());
                Plugin.logger.LogInfo($"Inited {missileDefinitions.Count} missiles");

                

                foreach (var m in missileDefinitions)
                {
                    Plugin.logger.LogInfo($"Missile unitName: {m.unitName}");
                }
                _defenitionsInited=true;
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError(ex.Message);
            }
        }

        private static bool GetDefinitionsByUnitName(string name, out UnitDefinition def)
        {
            foreach (var m in allUnits)
            {
                if (m.unitName.ToLower().Contains(name.ToLower()))
                {
                    def = m;
                    return true;
                } 
            }
            def = null;
            return false;
        }
        public static void SpawnMissilesAtPlayer(string missileName, int count, Player target, Player owner = null)
        {
            if (!_defenitionsInited) { InitDefenitions(); }
            if (target.Aircraft == null) return;

            UnitDefinition def = missileDefinitions[0];
            bool missileFounded = false;
            foreach (var m in missileDefinitions)
            {
                if (m.unitName.ToLower().Contains(missileName.ToLower()))
                {
                    def = m;
                    missileFounded = true;
                    break;
                }
                
            }
            if (!missileFounded && !GetDefinitionsByUnitName(missileName, out def))
            {
                return;
            }

            float radius = 15000;

            if (def.unitName.Contains("Genie"))
            {
                
            }

            
            Unit weaponOwner;
            if (owner == null) weaponOwner = target.Aircraft;
            else weaponOwner = owner.Aircraft;
            float magnitude = target.Aircraft.rb.velocity.magnitude;
            for (int i = 0; i < count; i++)
            {
                float angleX = (i * Mathf.PI * 2 / count);
                
                Vector3 positionOffset = new Vector3(Mathf.Cos(angleX) * radius, 0f, Mathf.Sin(angleX) * radius);
                Vector3 spawnPosition = target.Aircraft.transform.position + positionOffset;

                Vector3 directionToCenter = target.Aircraft.transform.position - spawnPosition;
                
                Quaternion rotation = Quaternion.LookRotation(directionToCenter);

                Vector3 offsetY = new Vector3(0, radius, 0f);
                positionOffset += offsetY;

                //float goldenRation = (float)(1 + Mathf.Sqrt(5)) / 2;
                //float theta = 2 * Mathf.PI * i / goldenRation;
                //float phi = Mathf.Acos(-1 + (2 * i + 1) / count);
                 
                //Vector3 positionOffset = new Vector3(
                // Mathf.Sin(phi) * Mathf.Cos(theta),
                // Mathf.Sin(phi) * Mathf.Sin(theta),
                // Mathf.Cos(phi)
                //) * radius;

                //Vector3 spawnPosition = target.Aircraft.transform.position + positionOffset;
                //Vector3 directionToCenter = target.Aircraft.transform.position - spawnPosition;
                //Quaternion rotation = Quaternion.LookRotation(directionToCenter);


                Vector3 vectorVelocity = directionToCenter.normalized * magnitude;
                if (string.Equals(def.code, "MSL", StringComparison.OrdinalIgnoreCase)) vectorVelocity = vectorVelocity.normalized;
                if (string.Equals(def.code, "BOMB", StringComparison.OrdinalIgnoreCase)) vectorVelocity = directionToCenter.normalized * magnitude * 4;
                Plugin.logger.LogWarning("Spawning missile...");
                NetworkSceneSingleton<Spawner>.i.SpawnMissile((MissileDefinition)def, spawnPosition, rotation, vectorVelocity, target.Aircraft, weaponOwner);
            }
        }

       
        private static float CalcDistance(UnitDefinition unit, Unit target)
        {
            return 0;
        }

        private static float CalcVelocity(UnitDefinition unit, Unit target)
        {
            return 0;
        }
 
    }
}
