using Scenario.Editor.Content.Fields.Base;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenario.Editor.Content.Fields.Types.Convertible
{
    public class QuaternionFieldCreator : BaseConvertibleFieldCreator<Quaternion, Vector4, Vector4Field>
    {
        protected override Vector4 Serialize(Quaternion value)
        {
            return new Vector4(value.x, value.y, value.z, value.w);
        }
        protected override Quaternion Deserialize(Vector4 value)
        {
            return new Quaternion(value.x, value.y, value.z, value.w);
        }
    }
}