using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenario.Editor.Model;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Core.Services;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Editor.Tools.Files.Update;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Utilities.Providers;
using ScenarioToolkit.Editor.Windows.ContextEditor;
using ScenarioToolkit.Editor.Windows.ElementEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using ScenarioToolkit.Editor.Windows.Search;
using ScenarioToolkit.Shared.VRF;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

namespace ScenarioToolkit.Editor.Windows.GraphEditor
{
    /// <summary>
    /// Ядро редактора сценария. Является главным окном в сценарии, администрирует также другие окна,
    /// контролирует текущий сценарий и является неким EntryPoint в для всей подсистемы Scenario.Editor
    /// </summary>
    public class GraphEditorWindow : BaseScenarioWindow
    {
        #region Fields
        public ScenarioGraphEditorSession Session { get; private set; }
        
        public ScenarioSerializationService Serialization { get; private set; }
        public ScenarioLoadService LoadService { get; private set; }
        public ScenarioSceneProvider SceneProvider { get; private set; }
        
        public ScenarioModelController ModelController { get; private set; }
        public ElementSearchWindow ElementSearch { get; private set; }
        
        public ScenarioGraphController GraphController { get; private set; }
        public ScenarioGraphView GraphView { get; private set; }
        
        private GraphEditorBehaviour GraphEditor { get; set; }
        public FileMenuBehaviour FileMenu { get; private set; }
        public GraphMenuBehaviour GraphMenu { get; private set; }
        public FunctionsMenuBehaviour FunctionsMenu { get; private set; }
        public PlayMenuBehaviour PlayMenu { get; private set; }
        private KeyInputGraphBehaviour KeyInput { get; set; }
        
        public GraphHighlightBehaviour GraphHighlight { get; private set; }
        private SubGraphStackBehaviour SubGraphStack { get; set; }
        
        private TemplateContainer root;
        private ElementEditorWindow elementWindow;
        private ContextEditorWindow contextWindow;
        
        [InjectOptional] public ScenarioModules Modules { get; private set; }
        [InjectOptional] public ScenarioPlayer ContainerPlayer { get; private set; }
        [InjectOptional] public SignalBus Bus { get; private set; }
        [InjectOptional] public IdentityService IdentityService { get; private set; }
        #endregion
        
        #region Static
        public static async void OpenWindow(TextAsset scenarioAsset, IScenarioWorldID sceneContext = null)
        {
            var instance = WindowsStatic.GetGraphEditor();
            await Task.Delay(WindowsStatic.TickDelay); // нужно, так как не успевает инициализация
            
            instance.Session.SetSceneContext(sceneContext);
            instance.FileMenu.Open(scenarioAsset);
            instance.GraphHighlight.UpdatePlayer();
        }
        public static ElementEditorWindow ConstructElementEditor(bool focus = false)
        {
            var window = WindowsStatic.GetElementEditor(focus);
            window.Construct(SWEContext.Graph, SWEContext.Graph.ModelController.Model.Context);
            return window;
        }
        public static ContextEditorWindow ConstructContextEditor(bool focus = false)
        {
            var window = WindowsStatic.GetContextEditor(focus);
            window.Construct(SWEContext.Graph);
            window.BindScenario(SWEContext.Graph.ModelController.Model);
            return window;
        }
        #endregion

        #region Class/Unity/Override
        public GraphEditorWindow()
        {
            saveChangesMessage = "Scenario graph has unsaved changes. Would you like to save?";
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorDiContainerService.ContainerUpdated += ResolveGraph;
            EditorDiContainerService.ContainerRemoved += UnResolveGraph;

            SWEContext.SetGraph(this);
            Construct();
        }
        private void CreateGUI()
        {
            OnInitializedGraph();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorDiContainerService.ContainerUpdated -= ResolveGraph;
            EditorDiContainerService.ContainerRemoved -= UnResolveGraph;
            
            Backup();
            
            SWEContext.SetGraph(null);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Backup();
            UnResolveGraph();
        }
        protected override void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            base.OnSceneOpened(scene, mode);
            
            if (mode == OpenSceneMode.Single)
            {
                InitSaveServices();
                InitSceneProvider();
                if (Session != null)
                    FunctionsMenu.Reload();
            }
        }
        #endregion
        
        #region Init
        private void Construct()
        {
            CreateRootUI();
            CreateWindows();
            
            InitSaveServices();
            InitSceneProvider();
            
            GraphView = new ScenarioGraphView(this);
            GraphController = new ScenarioGraphController(GraphView, LoadService);
            GraphView.SetPresenter(GraphController);
            
            ModelController = new ScenarioModelController(Serialization, this, 
                LoadService, GraphController, GraphView, contextWindow, elementWindow);
            ElementSearch = new ElementSearchWindow(this);
            
            GraphHighlight = new GraphHighlightBehaviour(this);
            PlayMenu = new PlayMenuBehaviour(this, root);
            //SubGraphStack = new SubGraphStackBehaviour(this, root);
            
            GraphEditor = new GraphEditorBehaviour(this, root);
            FileMenu = new FileMenuBehaviour(this, root);
            GraphMenu = new GraphMenuBehaviour(this, root);
            FunctionsMenu = new FunctionsMenuBehaviour(this, root);
            KeyInput = new KeyInputGraphBehaviour(GraphView, rootVisualElement);
            
            if (EditorDiContainerService.Container != null)
                ResolveGraph();
        }
        private void OnInitializedGraph()
        {
            Session = ScenarioGraphEditorSession.Load(Serialization);
            Session.FileNameChanged += SessionOnFileNameChanged;
            GraphView.SetEditorSession(Session);
            
            PlayMenu.UpdateUI();
            SubGraphStack = new SubGraphStackBehaviour(this, root);

            TryLoadSession(Session.LastOpenedPath, true);
        }
        
        private void CreateRootUI()
        {
            root = UxmlEditorProvider.instance.GraphEditor.Instantiate();
            rootVisualElement.Add(root);
        }
        private void CreateWindows()
        {
            elementWindow = WindowsStatic.GetElementEditor();
            contextWindow = WindowsStatic.GetContextEditor();
            elementWindow.DataUpdated += SetDirtyScenario;
            BindWindow(contextWindow);
            contextWindow.BindEmptyScenario();
        }
        private void InitSaveServices()
        {
            if (Serialization == null)
            {
                Serialization ??= new ScenarioSerializationService();
                LoadService ??= new ScenarioLoadService(Serialization);
            }
            else
            {
                // Reload after scene switching
                //Debug.Log("Editor: Reload save services");
                Serialization.Init();
            }
        }
        private void InitSceneProvider()
        {
            SceneProvider = FindAnyObjectByType<ScenarioSceneProvider>();
        }
        
        private void OnPlayModeChanged(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange is PlayModeStateChange.EnteredPlayMode or PlayModeStateChange.EnteredEditMode)
            {
                PlayMenu.UpdateUI();
                InitSaveServices();
                InitSceneProvider();
                GraphView?.ResetPlayer();
                
                if (Session == null) return;
                //Session ??= ScenarioGraphEditorSession.Load(Serialization);
                TryLoadSession(Session.LastOpenedPath);
            }
        }
        public void ResolveGraph()
        {
            Modules = EditorDiContainerService.Container.TryResolve<ScenarioModules>();
            ContainerPlayer = EditorDiContainerService.Container.TryResolve<ScenarioPlayer>();
            IdentityService = EditorDiContainerService.Container.TryResolve<IdentityService>();
            Bus = EditorDiContainerService.Container.TryResolve<SignalBus>();
        }
        public void UnResolveGraph()
        {
            Modules = null;
            ContainerPlayer = null;
            IdentityService = null;
            Bus = null;
        }
        #endregion
        
        #region Save/Load/Dirty
        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }
        protected override void SaveWindow()
        {
            if (IsHasAnyUnsavedChanges())
            {
                if (string.IsNullOrWhiteSpace(Session.LastOpenedPath))
                    FileMenu.SaveAs(); else SaveSession(Session.LastOpenedPath);
            }
        }
        
        public bool TryLoadSession(string filePath, bool canBeNew = false, bool validate = true)
        {
            Session.UpdatePose(Session.LastOpenedPath, GraphView.GetCamera());
            //Session.Save();
            //Backup();
            
            if (string.IsNullOrWhiteSpace(filePath) && !canBeNew) return false;
            if (!DirectoryFileHelper.TryLoadFile(filePath, out var dataOnDiscard) && !canBeNew)
            {
                Debug.LogWarning($"<b>Scenario not founded</b>: {filePath}");
                return false;
            }
            if (!canBeNew && !CheckValidSerialization()) return false;
            
            if (hasUnsavedChanges)
                Debug.Log($"<b>Scenario discarded</b>: {DirectoryFileHelper.GetFileName(Session.LastOpenedPath)}");
            
            LoadSession(dataOnDiscard, filePath, validate);
            return true;
        }
        public void LoadSession(string data, string filePath, bool validate = true)
        {
            var scenarioName = DirectoryFileHelper.GetFileName(filePath);
            var setDirty = false;

            if (validate)
            {
                try
                {
                    ModelController.DeserializeModel(data);
                }
                catch (Exception exception)
                {
                    if (exception is JsonSerializationException or JsonReaderException)
                    {
                        if (!DisplayJsonErrorDialog(scenarioName))
                            return;

                        ScenarioNamespaceTools.UpdateNamespaces(ref data, scenarioName);
                        ScenarioModelTools.UpdateModel(ref data, Serialization, 
                            LoadService, scenarioName, out var newModel);
                        
                        setDirty = true;
                        ModelController.DeserializeModel(newModel);
                    }
                    else
                    {
                        Debug.LogException(exception);
                        throw;
                    }
                }
            }
            else
            {
                ModelController.DeserializeModel(data);
            }

            var pose = Session.GetPose(filePath);
            Session.RefreshSession(filePath);
            
            SubGraphStack = new SubGraphStackBehaviour(this, root);
            if (hasUnsavedChanges) DiscardChanges();
            if (setDirty) SetDirtyScenario();
            GraphHighlight.UpdatePlayer();

            //if (string.IsNullOrEmpty(data)) Debug.Log($"<b>Scenario new created</b>");
            //if (!string.IsNullOrEmpty(data)) Debug.Log($"<b>Scenario loaded</b>: {scenarioName}");
            
            SetCameraAsync(pose); // Вынужденная мера
        }
        public void SaveSession(string filePath)
        {
            //var fileName = DirectoryFileHelper.GetFileName(filePath);
            var model = ModelController.SerializeModel();
            
            File.WriteAllText(filePath, model);
            AssetDatabase.Refresh();
            
            Session.UpdatePose(filePath, GraphView.GetCamera());
            Session.RefreshSession(filePath);
            
            hasUnsavedChanges = false;
            //Debug.Log($"<b>Scenario saved</b>: {fileName}");
        }
        private void Backup()
        {
            if (Session == null) return;
            //Session ??= ScenarioGraphEditorSession.Load(Serialization);
            Session.Save();
            var backupName = Session == null ? "unknown" : Path.GetFileNameWithoutExtension(Session.LastOpenedPath);
            BackupHelper.SaveBackup(ModelController.SerializeModel(), backupName);
        }
        #endregion

        #region Utility
        private bool DisplayJsonErrorDialog(string scenarioName)
        {
            const string dialogTitle = "Can't load scenario";
            var message = $"Can't load scenario {scenarioName} because of JsonError. " +
                          "Try to fix it with internal tools?";
            const string ok = "Fix and open scenario";
            const string cancel = "Discard loading";
            
            var solution = EditorUtility.DisplayDialog(dialogTitle, message, ok, cancel);
            Debug.Log($"<b>Scenario discarded</b>: {DirectoryFileHelper.GetFileName(Session.LastOpenedPath)}");
            return solution;
        }
        private bool CheckValidSerialization()
        {
            var valid = Serialization.IsValidConverters();
            if (valid) return true;

            const string dialogTitle = "Incorrect serialization";
            const string message = "Detected broken converters in serialization service. " +
                                   "Scenario will be with empty GO and Component links. " +
                                   "Do you want to load scenario?";
            const string ok = "Open scenario anyway";
            const string cancel = "Discard loading";
            
            var solution = EditorUtility.DisplayDialog(dialogTitle, message, ok, cancel);
            Debug.Log($"<b>Scenario discarded</b>: {DirectoryFileHelper.GetFileName(Session.LastOpenedPath)}");
            return solution;
        }
        private async void SetCameraAsync([CanBeNull] GraphCameraPose cameraPose)
        {
            await Task.Delay(WindowsStatic.TickDelay);
            
            if (cameraPose != null)
                GraphView.SetCamera(cameraPose);
            else FunctionsMenu.CenterGraph();
        }
        private void SessionOnFileNameChanged(string fileName)
        {
            var newTitle = string.IsNullOrEmpty(fileName)
                ? WindowsStatic.GraphEditorTitle : DirectoryFileHelper.GetFileName(fileName);
            titleContent = new GUIContent(newTitle);
        }
        #endregion
    }
}