using System;

namespace VRF.Utilities.Attributes
{
    /// <summary>
    /// Отражает степень воздействия метода на объекты
    /// </summary>
    public enum MethodType
    {
        /// <summary>
        /// Метод воздействует только на выбранные объекты
        /// </summary>
        Regular,
        /// <summary>
        /// Метод воздействует на все объекты на сцене
        /// </summary>
        Danger,
        /// <summary>
        /// Метод никак не изменяет объекты
        /// </summary>
        Readonly
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ToolMethodAttribute : Attribute
    {
        public string About { get; }
        public string Description { get; }
        public MethodType MethodType { get; }

        public ToolMethodAttribute(string about = "", 
            string description = "", 
            MethodType methodType = MethodType.Regular)
        {
            About = string.IsNullOrEmpty(about) ? TypeId.ToString() : about;
            Description = description;
            MethodType = methodType;
        }
    }
}