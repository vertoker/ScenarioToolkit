using System;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Content.Fields.Experimental.Elements
{
  /// <summary>
  /// Базовый класс поля для unity объекта. Основан на ObjectField,
  /// но с отличием, что здесь есть фильтр на тип принимаемого Object 
  /// </summary>
  [Obsolete] // TODO работы ну очень много, когда-нибудь это можно сделать
  public class BaseObjectField<TObject> : BaseField<TObject> where TObject : UnityEngine.Object
  {
    public BaseObjectField(string label, VisualElement visualInput) : base(label, visualInput)
    {
            
    }
      
    /*private System.Type m_objectType; 
    private readonly ObjectFieldDisplay m_ObjectFieldDisplay; 
    private readonly Action m_AsyncOnProjectOrHierarchyChangedCallback; 
    private readonly Action m_OnProjectOrHierarchyChangedCallback;
        
    public new static readonly string ussClassName = "unity-object-field";
    public new static readonly string labelUssClassName = ussClassName + "__label";
    public new static readonly string inputUssClassName = ussClassName + "__input";
    public static readonly string objectUssClassName = ussClassName + "__object";
    public static readonly string selectorUssClassName = ussClassName + "__selector";
    internal static readonly PropertyName serializedPropertyKey = new("--unity-object-field-serialized-property");

    internal event Action onObjectSelectorShow;

    public override void SetValueWithoutNotify(UnityEngine.Object newValue)
    {
      newValue = this.TryReadComponentFromGameObject(newValue, this.objectType);
      bool flag = !EqualityComparer<UnityEngine.Object>.Default.Equals(this.value, newValue);
      base.SetValueWithoutNotify(newValue);
      if (!flag)
        return;
      this.UpdateDisplay();
    }

    /// <summary>
    ///        <para>
    /// The type of the objects that can be assigned.
    /// </para>
    ///      </summary>
    public System.Type objectType
    {
      get => this.m_objectType;
      set
      {
        if (!(this.m_objectType != value))
          return;
        this.m_objectType = value;
        this.UpdateDisplay();
      }
    }

    internal void SetObjectTypeWithoutDisplayUpdate(System.Type type) => this.m_objectType = type;

    /// <summary>
    ///        <para>
    /// Allows scene objects to be assigned to the field.
    /// </para>
    ///      </summary>
    public bool allowSceneObjects { get; set; }

    protected override void UpdateMixedValueContent()
    {
      this.m_ObjectFieldDisplay?.ShowMixedValue(this.showMixedValue);
    }

    internal void UpdateDisplay() => this.m_ObjectFieldDisplay.Update();

    /// <summary>
    ///        <para>
    /// Constructor.
    /// </para>
    ///      </summary>
    public ObjectField()
      : this((string) null)
    {
    }

    /// <summary>
    ///        <para>
    /// Constructor.
    /// </para>
    ///      </summary>
    /// <param name="label"></param>
    public ObjectField(string label)
      : base(label, (VisualElement) null)
    {
      this.visualInput.focusable = false;
      this.labelElement.focusable = false;
      this.AddToClassList(ObjectField.ussClassName);
      this.labelElement.AddToClassList(ObjectField.labelUssClassName);
      this.allowSceneObjects = true;
      this.m_objectType = typeof (UnityEngine.Object);
      ObjectField.ObjectFieldDisplay objectFieldDisplay = new ObjectField.ObjectFieldDisplay(this);
      objectFieldDisplay.focusable = true;
      this.m_ObjectFieldDisplay = objectFieldDisplay;
      this.m_ObjectFieldDisplay.AddToClassList(ObjectField.objectUssClassName);
      ObjectField.ObjectFieldSelector child = new ObjectField.ObjectFieldSelector(this);
      child.AddToClassList(ObjectField.selectorUssClassName);
      this.visualInput.AddToClassList(ObjectField.inputUssClassName);
      this.visualInput.Add((VisualElement) this.m_ObjectFieldDisplay);
      this.visualInput.Add((VisualElement) child);
      this.m_AsyncOnProjectOrHierarchyChangedCallback = (Action) (() => this.schedule.Execute(this.m_OnProjectOrHierarchyChangedCallback));
      this.m_OnProjectOrHierarchyChangedCallback = new Action(this.UpdateDisplay);
      this.RegisterCallback<AttachToPanelEvent>((EventCallback<AttachToPanelEvent>) (evt =>
      {
        EditorApplication.projectChanged += this.m_AsyncOnProjectOrHierarchyChangedCallback;
        EditorApplication.hierarchyChanged += this.m_AsyncOnProjectOrHierarchyChangedCallback;
      }));
      this.RegisterCallback<DetachFromPanelEvent>((EventCallback<DetachFromPanelEvent>) (evt =>
      {
        EditorApplication.projectChanged -= this.m_AsyncOnProjectOrHierarchyChangedCallback;
        EditorApplication.hierarchyChanged -= this.m_AsyncOnProjectOrHierarchyChangedCallback;
      }));
    }

    internal void OnObjectChanged(UnityEngine.Object obj)
    {
      this.value = this.TryReadComponentFromGameObject(obj, this.objectType);
    }

    internal void ShowObjectSelector()
    {
      ObjectSelector.get.Show(this.value, this.objectType, (UnityEngine.Object) null, this.allowSceneObjects, onObjectSelectedUpdated: new Action<UnityEngine.Object>(this.OnObjectChanged));
      Action objectSelectorShow = this.onObjectSelectorShow;
      if (objectSelectorShow == null)
        return;
      objectSelectorShow();
    }

    private UnityEngine.Object TryReadComponentFromGameObject(UnityEngine.Object obj, System.Type type)
    {
      GameObject gameObject = obj as GameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && type != (System.Type) null && type.IsSubclassOf(typeof (Component)))
      {
        Component component = gameObject.GetComponent(this.objectType);
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return (UnityEngine.Object) component;
      }
      return obj;
    }

    /// <summary>
    ///        <para>
    /// Instantiates an ObjectField using the data read from a UXML file.
    /// </para>
    ///      </summary>
    public new class UxmlFactory : UnityEngine.UIElements.UxmlFactory<ObjectField, ObjectField.UxmlTraits>
    {
    }

    /// <summary>
    ///        <para>
    /// Defines UxmlTraits for the ObjectField.
    /// </para>
    ///      </summary>
    public new class UxmlTraits : BaseField<UnityEngine.Object>.UxmlTraits
    {
      private UxmlBoolAttributeDescription m_AllowSceneObjects;
      private UxmlTypeAttributeDescription<UnityEngine.Object> m_ObjectType;

      /// <summary>
      ///        <para>
      /// Initialize ObjectField properties using values from the attribute bag.
      /// </para>
      ///      </summary>
      /// <param name="ve">The object to initialize.</param>
      /// <param name="bag">The attribute bag.</param>
      /// <param name="cc">The creation context; unused.</param>
      public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
      {
        base.Init(ve, bag, cc);
        ((ObjectField) ve).allowSceneObjects = this.m_AllowSceneObjects.GetValueFromBag(bag, cc);
        ((ObjectField) ve).objectType = this.m_ObjectType.GetValueFromBag(bag, cc);
      }

      public UxmlTraits()
      {
        UxmlBoolAttributeDescription attributeDescription1 = new UxmlBoolAttributeDescription();
        attributeDescription1.name = "allow-scene-objects";
        attributeDescription1.defaultValue = true;
        this.m_AllowSceneObjects = attributeDescription1;
        UxmlTypeAttributeDescription<UnityEngine.Object> attributeDescription2 = new UxmlTypeAttributeDescription<UnityEngine.Object>();
        attributeDescription2.name = "type";
        attributeDescription2.defaultValue = typeof (UnityEngine.Object);
        this.m_ObjectType = attributeDescription2;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class ObjectFieldDisplay : VisualElement
    {
      private readonly ObjectField m_ObjectField;
      private readonly Image m_ObjectIcon;
      private readonly Label m_ObjectLabel;
      private static readonly string ussClassName = "unity-object-field-display";
      private static readonly string iconUssClassName = ObjectField.ObjectFieldDisplay.ussClassName + "__icon";
      internal static readonly string labelUssClassName = ObjectField.ObjectFieldDisplay.ussClassName + "__label";
      private static readonly string acceptDropVariantUssClassName = ObjectField.ObjectFieldDisplay.ussClassName + "--accept-drop";

      internal void ShowMixedValue(bool show)
      {
        if (show)
        {
          this.m_ObjectLabel.text = BaseField<UnityEngine.Object>.mixedValueString;
          this.m_ObjectLabel.AddToClassList(BaseField<UnityEngine.Object>.mixedValueLabelUssClassName);
          this.m_ObjectIcon.image = (Texture) null;
        }
        else
        {
          this.m_ObjectLabel.RemoveFromClassList(BaseField<UnityEngine.Object>.mixedValueLabelUssClassName);
          this.Update();
        }
      }

      public ObjectFieldDisplay(ObjectField objectField)
      {
        this.AddToClassList(ObjectField.ObjectFieldDisplay.ussClassName);
        Image image = new Image();
        image.scaleMode = ScaleMode.ScaleAndCrop;
        image.pickingMode = PickingMode.Ignore;
        this.m_ObjectIcon = image;
        this.m_ObjectIcon.AddToClassList(ObjectField.ObjectFieldDisplay.iconUssClassName);
        Label label = new Label();
        label.pickingMode = PickingMode.Ignore;
        this.m_ObjectLabel = label;
        this.m_ObjectLabel.AddToClassList(ObjectField.ObjectFieldDisplay.labelUssClassName);
        this.m_ObjectField = objectField;
        this.Update();
        this.Add((VisualElement) this.m_ObjectIcon);
        this.Add((VisualElement) this.m_ObjectLabel);
      }

      public void Update()
      {
        GUIContent guiContent = EditorGUIUtility.ObjectContent(this.m_ObjectField.value, this.m_ObjectField.objectType, this.m_ObjectField.GetProperty(ObjectField.serializedPropertyKey) as SerializedProperty);
        this.m_ObjectIcon.image = guiContent.image;
        this.m_ObjectLabel.text = guiContent.text;
      }

      [EventInterest(new System.Type[] {typeof (MouseDownEvent), typeof (KeyDownEvent), typeof (DragUpdatedEvent), typeof (DragPerformEvent), typeof (DragLeaveEvent)})]
      protected override void ExecuteDefaultActionAtTarget(EventBase evt)
      {
        base.ExecuteDefaultActionAtTarget(evt);
        if (evt == null)
          return;
        // ISSUE: explicit non-virtual call
        if (evt is MouseDownEvent mouseDownEvent && __nonvirtual (mouseDownEvent.button) == 0)
          this.OnMouseDown(evt as MouseDownEvent);
        else if (evt.eventTypeId == EventBase<KeyDownEvent>.TypeId())
        {
          KeyDownEvent keyDownEvent1 = evt as KeyDownEvent;
          // ISSUE: explicit non-virtual call
          // ISSUE: explicit non-virtual call
          // ISSUE: explicit non-virtual call
          if ((evt is KeyDownEvent keyDownEvent2 ? (__nonvirtual (keyDownEvent2.keyCode) == KeyCode.Space ? 1 : 0) : 0) != 0 || (evt is KeyDownEvent keyDownEvent3 ? (__nonvirtual (keyDownEvent3.keyCode) == KeyCode.KeypadEnter ? 1 : 0) : 0) != 0 || evt is KeyDownEvent keyDownEvent4 && __nonvirtual (keyDownEvent4.keyCode) == KeyCode.Return)
            this.OnKeyboardEnter();
          else if (keyDownEvent1.keyCode == KeyCode.Delete || keyDownEvent1.keyCode == KeyCode.Backspace)
            this.OnKeyboardDelete();
        }
        if (!this.enabledInHierarchy)
          return;
        if (evt.eventTypeId == EventBase<DragUpdatedEvent>.TypeId())
          this.OnDragUpdated(evt);
        else if (evt.eventTypeId == EventBase<DragPerformEvent>.TypeId())
        {
          this.OnDragPerform(evt);
        }
        else
        {
          if (evt.eventTypeId != EventBase<DragLeaveEvent>.TypeId())
            return;
          this.OnDragLeave();
        }
      }

      [EventInterest(new System.Type[] {typeof (MouseDownEvent)})]
      internal override void ExecuteDefaultActionDisabledAtTarget(EventBase evt)
      {
        base.ExecuteDefaultActionDisabledAtTarget(evt);
        // ISSUE: explicit non-virtual call
        if (!(evt is MouseDownEvent mouseDownEvent) || __nonvirtual (mouseDownEvent.button) != 0)
          return;
        this.OnMouseDown(evt as MouseDownEvent);
      }

      private void OnDragLeave()
      {
        this.EnableInClassList(ObjectField.ObjectFieldDisplay.acceptDropVariantUssClassName, false);
      }

      private void OnMouseDown(MouseDownEvent evt)
      {
        UnityEngine.Object gameObject = this.m_ObjectField.value;
        Component component = gameObject as Component;
        if ((bool) (UnityEngine.Object) component)
          gameObject = (UnityEngine.Object) component.gameObject;
        if (gameObject == (UnityEngine.Object) null)
          return;
        if (evt.clickCount == 1)
        {
          if (!evt.shiftKey && !evt.ctrlKey && (bool) gameObject)
            EditorGUIUtility.PingObject(gameObject);
          evt.StopPropagation();
        }
        else
        {
          if (evt.clickCount != 2)
            return;
          if ((bool) gameObject)
          {
            AssetDatabase.OpenAsset(gameObject);
            GUIUtility.ExitGUI();
          }
          evt.StopPropagation();
        }
      }

      private void OnKeyboardEnter() => this.m_ObjectField.ShowObjectSelector();

      private void OnKeyboardDelete()
      {
        this.m_ObjectField.SetProperty(ObjectField.serializedPropertyKey, (object) null);
        this.m_ObjectField.value = (UnityEngine.Object) null;
      }

      private UnityEngine.Object DNDValidateObject()
      {
        UnityEngine.Object target = EditorGUI.ValidateObjectFieldAssignment(DragAndDrop.objectReferences, this.m_ObjectField.objectType, this.m_ObjectField.GetProperty(ObjectField.serializedPropertyKey) as SerializedProperty, EditorGUI.ObjectFieldValidatorOptions.None);
        if (target != (UnityEngine.Object) null && !this.m_ObjectField.allowSceneObjects && !EditorUtility.IsPersistent(target))
          target = (UnityEngine.Object) null;
        return target;
      }

      private void OnDragUpdated(EventBase evt)
      {
        if (!(this.DNDValidateObject() != (UnityEngine.Object) null))
          return;
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        this.EnableInClassList(ObjectField.ObjectFieldDisplay.acceptDropVariantUssClassName, true);
        evt.StopPropagation();
      }

      private void OnDragPerform(EventBase evt)
      {
        UnityEngine.Object @object = this.DNDValidateObject();
        if (!(@object != (UnityEngine.Object) null))
          return;
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        this.m_ObjectField.value = @object;
        DragAndDrop.AcceptDrag();
        this.RemoveFromClassList(ObjectField.ObjectFieldDisplay.acceptDropVariantUssClassName);
        evt.StopPropagation();
      }
    }

    private class ObjectFieldSelector : VisualElement
    {
      private readonly ObjectField m_ObjectField;

      public ObjectFieldSelector(ObjectField objectField) => this.m_ObjectField = objectField;

      [EventInterest(new System.Type[] {typeof (MouseDownEvent)})]
      protected override void ExecuteDefaultAction(EventBase evt)
      {
        base.ExecuteDefaultAction(evt);
        // ISSUE: explicit non-virtual call
        if (!(evt is MouseDownEvent mouseDownEvent) || __nonvirtual (mouseDownEvent.button) != 0)
          return;
        this.m_ObjectField.ShowObjectSelector();
      }
    }*/
  }
}