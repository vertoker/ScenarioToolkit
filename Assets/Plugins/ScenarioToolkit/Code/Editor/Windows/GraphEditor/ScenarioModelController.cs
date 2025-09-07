using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Windows.ContextEditor;
using ScenarioToolkit.Editor.Windows.ElementEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;

namespace ScenarioToolkit.Editor.Windows.GraphEditor
{
    /// <summary>
    /// Контроллер модели, отвечает за хранение актуальной модели,
    /// а также за основные функции загрузки/выгрузки данных
    /// </summary>
    public class ScenarioModelController
    {
        private readonly ScenarioSerializationService serializationService;
        private readonly GraphEditorWindow graphEditorWindow;
        private readonly ScenarioLoadService loadService;

        // Делегирует управление графов к соответствующему контроллеру
        private readonly ScenarioGraphController graphController;
        private readonly ScenarioGraphView view;
        
        private readonly ContextEditorWindow contextWindow;
        private readonly ElementEditorWindow elementWindow;

        public IScenarioModel Model { get; private set; }

        public ScenarioModelController(ScenarioSerializationService serializationService, 
            GraphEditorWindow graphEditorWindow, ScenarioLoadService loadService,
            ScenarioGraphController graphController, ScenarioGraphView view, 
            ContextEditorWindow contextWindow, ElementEditorWindow elementWindow)
        {
            this.serializationService = serializationService;
            this.graphEditorWindow = graphEditorWindow;
            this.loadService = loadService;

            this.graphController = graphController;
            this.view = view;
            
            this.contextWindow = contextWindow;
            this.elementWindow = elementWindow;
        }
        
        public string SerializeModel()
        {
            if (Model == null) return string.Empty;
            elementWindow.OverridesController.Save();
            Model.Graph = graphController.Graph;
            Model.EditorGraph = CopyUtils.CreateEditorGraph(view.graphElements);
            return serializationService.Serialize(Model, IScenarioModel.GetModelType);
        }
        public void DeserializeModel(string data)
        {
            graphController.Clear();
            Model = (IScenarioModel)serializationService.Deserialize
                (data, IScenarioModel.GetModelType) ?? IScenarioModel.CreateNew();
            elementWindow.Construct(graphEditorWindow, Model.Context);
            contextWindow.Construct(graphEditorWindow);
            contextWindow.BindScenario(Model);
            graphController.Load(Model);
        }
        public void DeserializeModel(IScenarioModel newModel)
        {
            graphController.Clear();
            Model = newModel;
            elementWindow.Construct(graphEditorWindow, Model.Context);
            contextWindow.Construct(graphEditorWindow);
            contextWindow.BindScenario(Model);
            graphController.Load(Model);
        }
    }
}