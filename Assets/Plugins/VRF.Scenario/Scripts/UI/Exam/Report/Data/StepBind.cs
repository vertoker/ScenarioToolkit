using System;
using UnityEngine;
using VRF.Scenario.Models;

namespace VRF.Scenario.UI.Exam.Report.Data
{
    public class StepBind : MonoBehaviour
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private LabelValueBind stepName;
        [SerializeField] private LabelValueBind stepStatus;
        [SerializeField] private LabelValueBind stepTime;
        [Space]
        [SerializeField] private LabelValueBind template;
        
        [Serializable]
        public class Input
        {
            public bool active = true;
            public string labelText = string.Empty;
        }

        public void SetActive(bool activeName, bool activeStatus, bool activeTime)
        {
            stepName.SetActive(activeName);
            stepStatus.SetActive(activeStatus);
            stepTime.SetActive(activeTime);

            var y = (activeName ? 1f : 0f) + (activeStatus ? 1f : 0f) + (activeTime ? 1f : 0f);
            y *= 90f;
            parent.sizeDelta = new Vector2(parent.sizeDelta.x, y);
        }
        
        public void Set(Step stepData)
        {
            stepName.SetValue(stepData.StepText);
            stepStatus.SetStatus(stepData.Status);
            stepTime.SetTime(stepData.TimeSpan);
        }
    }
}