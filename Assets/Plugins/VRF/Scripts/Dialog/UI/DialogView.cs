using System;
using System.Collections.Generic;
using NaughtyAttributes;
using SimpleUI.Core;
using SimpleUI.Templates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.Utilities;

namespace VRF.Dialog.UI
{
    public class DialogView : UIView
    {
        [Serializable]
        public struct Item
        {
            public GameObject obj;
            public TMP_Text text;
            public Button button;
            
            public Item(GameObject item)
            {
                obj = item;
                text = item.GetComponentInChildren<TMP_Text>();
                button = item.GetComponentInChildren<Button>();
            }
        }
        
        [SerializeField] private bool closeMenuAfterClear = true;
        [ShowIf(nameof(closeMenuAfterClear))]
        [SerializeField] private SwitchScreenButtonView switchScreenBtn;
        [Space]
        [SerializeField] private Button upBtn;
        [SerializeField] private Button downBtn;
        [SerializeField] private GameObject[] dialogLineList;
        [SerializeField, ReadOnly] private Item[] dialogLineItems;
        
        public Button UpBtn => upBtn;
        public Button DownBtn => downBtn;

        public int ItemsPerPage => dialogLineItems.Length;
        public IReadOnlyList<Item> DialogLineItems => dialogLineItems;

        public bool CloseMenuAfterClear => closeMenuAfterClear;
        public SwitchScreenButtonView SwitchScreenBtn => switchScreenBtn;

        public override void OnValidate()
        {
            base.OnValidate();
            
            dialogLineItems = new Item[dialogLineList.Length];
            for (var i = 0; i < dialogLineList.Length; i++)
                dialogLineItems[i] = new Item(dialogLineList[i]);
            VrfRuntimeEditor.SetDirty(this);
        }
        
        public override Type GetControllerType() => typeof(DialogController);
    }
}