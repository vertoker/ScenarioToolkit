namespace VRF.Tools
{
    public static class ToolsNamespace
    {
        public const string CreativeTools = "Tools";
        public const string AdditionForSelection = "/For Selection";
        public const string AdditionForScene = "/For Scene";

        public const string SetupAll = CreativeTools + "/Setup All";
        public const string TestAll = CreativeTools + "/Test All";
        
        public const string Configs = CreativeTools + "/Configs";
        public const string ConfigsReload = Configs + "/Reload";
        public const string ConfigsRefreshCache = Configs + "/Refresh Cache";
        
        public const string Teleport = CreativeTools + "/Teleport";
        public const string TeleportScene = Teleport + "/Scene";
        public const string TeleportSelection = Teleport + "/Selection";
        public const string TeleportSelectionSetupAll = Teleport + "/Setup Selection";
        public const string TeleportSelectionAdd = TeleportSelection + "/Add TeleportObjects";
        public const string TeleportSelectionSetup = TeleportSelection + "/Setup Locked By Tags";
        public const string TeleportSelectionRemove = TeleportSelection + "/Remove TeleportObjects";
        
        public const string Utility = CreativeTools + "/Utility";
        
        public const string UtilityView = Utility + "/View";
        public const string UtilityViewNormalize = UtilityView + "/Normalize";
        public const string UtilityViewRoflalize = UtilityView + "/Roflalize";
        
        public const string UtilitySelectMissingComponents = Utility + "/Select Missing Components";
        public const string UtilitySelectMissingComponentsForSelection = UtilitySelectMissingComponents + AdditionForSelection;
        public const string UtilitySelectMissingComponentsForScene = UtilitySelectMissingComponents + AdditionForScene;
        
        public const string UtilityMoveSibling = Utility + "/Move Sibling";
        public const string UtilityMoveSiblingToTop = UtilityMoveSibling + "/To Top";
        public const string UtilityMoveSiblingToBottom = UtilityMoveSibling + "/To Bottom";

        public const string UtilityDeleteMissingComponents = Utility + "/Delete Missing Components";
        public const string UtilityDeleteMissingComponentsForSelection = UtilityDeleteMissingComponents + AdditionForSelection;
        public const string UtilityDeleteMissingComponentsForScene = UtilityDeleteMissingComponents + AdditionForScene;
        
        public const string UtilityDeleteComponents = Utility + "/Delete Components (Danger Zone)";
        public const string UtilityDeleteComponentsAll = UtilityDeleteComponents + "/Delete All";
        public const string UtilityDeleteComponentsAllForSelection = UtilityDeleteComponentsAll + AdditionForSelection;
        //public const string UtilityDeleteComponentsAllForScene = UtilityDeleteComponentsAll + "/For Scene"; // Cruel as FUCK
        public const string UtilityDeleteComponentsAllNonGraphics = UtilityDeleteComponents + "/Delete All non Graphics";
        public const string UtilityDeleteComponentsAllNonGraphicsForSelection = UtilityDeleteComponentsAllNonGraphics + AdditionForSelection;
        //public const string UtilityDeleteComponentsAllNonGraphicsForScene = UtilityDeleteComponentsAllNonGraphics + "/For Scene"; // Same shit
    }
}