using UnityEngine;
using VRF.Utils.Identifying;

namespace VRF.Dialog
{
    /// <summary> Конфиг реплики, нужен для текста в меню диалога </summary>
    [CreateAssetMenu(fileName  = nameof(DialogLineConfig), menuName = "VRF/Other/" + nameof(DialogLineConfig))]
    public class DialogLineConfig : IdentifiedScriptableObject
    {
        [TextArea] [SerializeField] private string dialogLine;

        public string DialogLine => dialogLine;
    }
}