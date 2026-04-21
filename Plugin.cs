using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;
using ServerTools.Commands;
using System.IO;
using ServerTools.Data;
using ServerTools.Services;
using System.Runtime.CompilerServices;
using System.Timers;
using ServerTools.Database;
using ServerTools.Upgrades;
using ServerTools.IPC;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace ServerTools
{
    
    [BepInPlugin("ServerTools", "ServerTools", "1.0.0.0")]
    
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource logger;
        public static ConfigEntry<string> CommandPrefix { get; private set; }
        public static ConfigEntry<string> Admins { get; private set; }
        public static ConfigEntry<string> Moderators { get; private set; }
        public static ConfigEntry<string> Owner { get; private set; }

        public static List<string> AdminsIDS { get; private set; }
        public static List<string> ModeratorsIDS { get; private set; }

        public static ConfigEntry<string> PathToBanList { get; private set; }
        public static List<string> Bans { get; private set; }

        public static IDatabase _POMDatabase;
        public static ConfigEntry<string> ConfigPathToPOMDatabase { get; private set; }
        public static string PathToPOMDatabase;

        public void Awake()
        {
            try
            {
                Plugin.logger = base.Logger;
                Harmony harmony = new Harmony("ServerTools");
                harmony.PatchAll();
                InitConfig();
                InitServices();
                AdminsIDS = ParseList(Admins.Value);
                ModeratorsIDS = ParseList(Moderators.Value);

                RegisterCommands();

                int i = 0;
                foreach (string id in AdminsIDS)
                {
                    logger.LogInfo($"Admin {i + 1}: {id}");
                    i++;
                }
                i = 0;
                foreach (string id in ModeratorsIDS)
                {
                    logger.LogInfo($"Moderator {i + 1}: {id}");
                    i++;
                }
                logger.LogInfo($"Owner {Owner.Value}");

                //i = 0;
                //Bans = ParseBanList(PathToBanList.Value);
                //logger.LogInfo($"Bans: ");
                //foreach (string id in Bans)
                //{
                //    logger.LogInfo($"\n{id}");
                //    i++;
                //}

                IPCService.Start(10042);
                

                System.Timers.Timer timer = new System.Timers.Timer(3000);
                timer.Elapsed += LateInit;
                timer.Start();
                timer.AutoReset = false;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
            } 
        }

     

        private void InitServices()
        {
            PlayerService.Awake();
            ChatService.Awake();
            PersonalOppressionMode.Awake();
            UpgradeManager.Instance.Awake();
        }

        

        private void RegisterCommands()
        {
            CommandService.RegisterChatCommand(new SendBalance(this.Config));
            CommandService.RegisterChatCommand(new AddBalance(this.Config));
            CommandService.RegisterChatCommand(new DebugSortiesCommand(this.Config));
            CommandService.RegisterChatCommand(new BanCommand(this.Config));
            CommandService.RegisterChatCommand(new KickCommand(this.Config));
            CommandService.RegisterChatCommand(new BanCommandOffline(this.Config));
            CommandService.RegisterChatCommand(new Test(this.Config));
            CommandService.RegisterChatCommand(new Help(this.Config));
            CommandService.RegisterChatCommand(new SupressClientCommand(this.Config));
            CommandService.RegisterChatCommand(new SpawnCommand(this.Config));
            CommandService.RegisterChatCommand(new SayCommand(this.Config));
            CommandService.RegisterChatCommand(new TpTestCommand(this.Config));
        }
        private void LateInit(object sender, ElapsedEventArgs e)
        {
            Plugin.logger.LogInfo("Late init");
            CustomSpawner.Awake();
        }
        private void InitConfig()
        {
            CommandPrefix = Config.Bind<string>("Chat Commands", "Command prefix", "/", "symbol to write command");
            Admins = Config.Bind<string>("Permissions", "Admins", "", "Server admins. Separate SteamID with ; without spaces");
            Moderators = Config.Bind<string>("Permissions", "Moderators", "", "Server moderators. Separate SteamID with ; without spaces");
            Owner = Config.Bind<string>("Permissions", "Owner", "", "Server moderators. ");


            PathToBanList = Config.Bind<string>("Ban System", "Path to ban list file", "", "");
            ConfigPathToPOMDatabase = Config.Bind<string>("Databases", "Path to POM db", "", "");
            PathToPOMDatabase = ConfigPathToPOMDatabase.Value;
        }
        private static List<string> ParseList(string input)
        {
            logger.LogInfo($"parsing: {input}");
            List<string> list = input.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        private static List<string> ParseBanList(string path)
        {
            string ids = string.Empty;
            foreach (var line in File.ReadAllLines(path))
            {
                ids += line + ';';
            }
            return ParseList(ids);
        }
        
        public void Update() 
        {
            PersonalOppressionMode.FixedUpdate();
           
        }

        public static void IPCLog(string msg, object sender = null)
        {
            if (sender != null)
            {
                if (sender is string) msg = $"[{sender}]: {msg}";
                else
                    msg = $"[{sender.GetType().Name}]: {msg}";
            }
            //Plugin.logger.LogWarning($"[DEBUG INFO]{msg}");
            try
            {
                IPCService.BroadcastChannel("/stats", msg);
            }
            catch (Exception e)
            {
                Plugin.logger.LogError(e);
            }
        }

        
        public void FixedUpdate()
        {
            
            PersonalOppressionMode.FixedUpdate();
        }

    }
}
