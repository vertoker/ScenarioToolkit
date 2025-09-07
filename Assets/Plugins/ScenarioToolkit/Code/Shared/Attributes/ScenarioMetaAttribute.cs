using System;
using System.Text;
using JetBrains.Annotations;

namespace Scenario.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field)]
    public class ScenarioMetaAttribute : Attribute
    {
        public string Tooltip { get; set; } = string.Empty;
        [CanBeNull] public Type[] RelatedComponents { get; set; }
        public bool CanBeNull { get; set; }

        public string GetTooltip()
        {
            var builder = new StringBuilder();
            builder.Append(Tooltip);

            if (RelatedComponents != null)
            {
                builder.Append("\nПохожее: ");

                var length = RelatedComponents.Length;
                for (var i = 0; i < length;)
                {
                    builder.Append(RelatedComponents[i].Name);
                    i++;
                    if (i < length)
                        builder.Append(" : ");
                }
            }

            return builder.ToString();
        }

        public ScenarioMetaAttribute() { }
        public ScenarioMetaAttribute(string tooltip)
        {
            Tooltip = tooltip;
        }
        public ScenarioMetaAttribute(string tooltip, params Type[] relatedComponents)
        {
            Tooltip = tooltip;
            RelatedComponents = relatedComponents;
        }
    }
}