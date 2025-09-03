using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using VRF.Utilities.Attributes;

namespace VRF.Editor.MethodTools.Models
{
    [Serializable]
    public class ListUnitModel
    {
        public string About { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public MethodType MethodType { get; set; }
        public Action Invoke { get; set; }

        private static readonly StyleColor Regular = new(Color.grey);
        private static readonly StyleColor Danger = new(new Color(0.7f, 0.2f, 0.2f, 1));
        private static readonly StyleColor Readonly = new(new Color(0.2f, 0.7f, 0.2f, 1));

        private StyleColor GetStyleColor()
        {
            return MethodType switch
            {
                MethodType.Regular => Regular,
                MethodType.Danger => Danger,
                MethodType.Readonly => Readonly,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public ListUnitModel(ToolMethodAttribute attribute, MethodInfo method)
        {
            About = attribute.About.ToLower();
            Description = attribute.Description.ToLower();
            MethodType = attribute.MethodType;
            MethodName = method.Name.ToLower();
            Invoke = (Action)method.CreateDelegate(typeof(Action));
        }
        public ListUnitModel(string about, string description, MethodType methodType, string methodName, Action invoke)
        {
            About = about;
            Description = description;
            MethodType = methodType;
            MethodName = methodName;
            Invoke = invoke;
        }

        public void BindItem(StaticMethodVisualElement element)
        {
            element.NameLabel.text = About;
            element.NameLabel.tooltip = Description;
            
            element.InvokeButton.style.backgroundColor = GetStyleColor();
            if (Invoke != null) element.InvokeButton.clicked += Invoke;
        }
        public void UnbindItem(StaticMethodVisualElement element)
        {
            element.InvokeButton.style.backgroundColor = Regular;
            if (Invoke != null)
                element.InvokeButton.clicked -= Invoke;
        }

        public bool IsSearchable(string searchQuery)
        {
            return About.Contains(searchQuery)
                   || Description.Contains(searchQuery)
                   || MethodName.Contains(searchQuery);
        }
    }
}