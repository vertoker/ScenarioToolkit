namespace VRF.Editor.TestBase
{
    public static class TestMetaData
    {
        public static class Layers
        {
            public const string Default = nameof(Default);
            
            public const string Player = nameof(Player);
            public const string Item = nameof(Item);
            public const string NetPlayer = nameof(NetPlayer);
            public const string Interactor = nameof(Interactor);
            public const string IgnorePlayer = nameof(IgnorePlayer);
            public const string IgnorePhysicsRaycast = nameof(IgnorePhysicsRaycast);
        }
        
        public static class Category
        {
            public const string InitTests = nameof(InitTests);
            public const string PureFunctions = nameof(PureFunctions);
            public const string Unity = nameof(Unity);
            public const string Files = nameof(Files);
        
            public const string HashCodes = nameof(HashCodes);
            public const string ProjectContext = nameof(ProjectContext);
            public const string ProjectInstallers = nameof(ProjectInstallers);
        
            public const string ScenesExistence = nameof(ScenesExistence);
            public const string SceneContext = nameof(SceneContext);
            public const string SceneInstallers = nameof(SceneInstallers);
        
            public const string ScriptablesReferencesValidation = nameof(ScriptablesReferencesValidation);
            public const string ScriptablesValuesValidation = nameof(ScriptablesValuesValidation);
            public const string GameObjectViewsValidation = nameof(GameObjectViewsValidation);
            public const string GameObjectReferencesValidation = nameof(GameObjectReferencesValidation);
            public const string GameObjectValuesValidation = nameof(GameObjectValuesValidation);
        }

        public static class Author
        {
            public const string Vertoker = "vertoker";
        }
    }
}