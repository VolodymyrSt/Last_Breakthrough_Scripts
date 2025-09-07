using NUnit.Framework;
using System;

namespace LastBreakthrought.Infrustructure.AssetManagment
{
    public static class AssetPath
    {
        public static string CrashedShipBigPath = "CrashedShips/CrashedShip_Big";
        public static string CrashedShipSmallPath = "CrashedShips/CrashedShip_Small";
        public static string CrashedShipMediumPath = "CrashedShips/CrashedShip_Medium";
        public static string CrashedShipLargePath = "CrashedShips/CrashedShip_Large";

        public static string ShipMaterialViewPath = "ShipMaterial/ShipMaterial";
        public static string ShipDetailViewPath = "ShipDetail/ShipDetail";

        public static string GolemPath = "NPC/Enemies/Golem";
        public static string BatPath = "NPC/Enemies/Bat";

        public static string RobotMinerPath = "NPC/Robots/RobotMiner";
        public static string RobotTransporterPath = "NPC/Robots/RobotTransporter";
        public static string RobotDefenderPath = "NPC/Robots/RobotDefender";

        public static string RobotMinerControlPath = "RobotControl/RobotMinerControl";
        public static string RobotTransporterControlPath = "RobotControl/RobotTransporterControl";
        public static string RobotDefenderControlPath = "RobotControl/RobotDefenderControl";

        public static string CrashedShipMarker = "CrashedShips/Marker/CrashedShipMarker";

        public static string MechanismPath = "Mechanism/Mechanism";

        public static string MechanismCraftPath = "CraftForCraftMachine/MechanismCraft";

        public static string LightningEffectPath = "Effects/Lightning Hit Blue";
        public static string FireEffectPath = "Effects/Fire Hit";
        public static string FireExplosionPath = "Effects/Explosion";

        public const string BEGINNING_VIDEO_PATH = "Video/Beginning.mp4";
        public const string STAR_EXPLOTION_VIDEO_PATH = "Video/StarExplotion.mp4";
        public const string VICTORY_VIDEO_PATH = "Video/Victory.mp4";

        public static string GetRandomEnemyPath()
        {
            var randomPath = UnityEngine.Random.Range(1, 3);

            if (randomPath == 1)
                return GolemPath;
            if (randomPath == 2) 
                return BatPath;

            throw new Exception("The enemyPath doesnt exist");
        }

        public static string GetRandomCrashedShipPath()
        {
            var randomPath = UnityEngine.Random.Range(1, 5);

            return randomPath switch
            {
                1 => CrashedShipBigPath,
                2 => CrashedShipLargePath,
                3 => CrashedShipSmallPath,
                4 => CrashedShipMediumPath,
                _ => throw new Exception("The enemyPath doesnt exist"),
            };
        }
    }
}
