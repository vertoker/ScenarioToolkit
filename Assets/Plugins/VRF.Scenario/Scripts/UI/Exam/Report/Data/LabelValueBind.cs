using System;
using TMPro;
using UnityEngine;
using VRF.Scenario.Models;

namespace VRF.Scenario.UI.Exam.Report.Data
{
    public class LabelValueBind : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private TMP_Text value;

        public void Set(string labelText, string valueText)
        {
            SetLabel(labelText);
            SetValue(valueText);
        }
        public void SetLabel(string labelText)
        {
            label?.SetText(labelText);
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        
        public void SetValue(string valueText)
        {
            value?.SetText(valueText);
        }
        public void SetTime(TimeSpan timeSpan)
        {
            var text = timeSpan.ToString(@"hh\:mm\:ss");
            //$"{timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}.{timeSpan.Milliseconds}";
            value?.SetText(text);
        }
        public void SetValue(Color color)
        {
            if (value)
                value.color = color;
        }

        public static readonly Color DefaultColor = Color.black;
        public static readonly Color InProgressColor = new(0.8f, 0.8f, 0);
        public static readonly Color CompletedColor = new(0, 0.8f, 0);
        public static readonly Color FailedColor = new(0.8f, 0, 0);
        public static readonly Color CancelledColor = Color.grey;
        
        public const string None = "Нан";
        public const string InProgress = "В процессе";
        public const string Completed = "Пройден";
        public const string Failed = "Провален";
        public const string Cancelled = "Отменён";
        
        public void SetStatus(ExamStatus status)
        {
            switch (status)
            {
                case ExamStatus.None: SetValue(DefaultColor); SetValue(None); break;
                case ExamStatus.InProgress: SetValue(InProgressColor); SetValue(InProgress); break;
                case ExamStatus.Completed: SetValue(CompletedColor); SetValue(Completed); break;
                case ExamStatus.Failed: SetValue(FailedColor); SetValue(Failed); break;
                case ExamStatus.Cancelled: SetValue(CancelledColor); SetValue(Cancelled); break;
                default: SetValue(DefaultColor); break;
            }
        }
        public void SetStatus(StepStatus status)
        {
            switch (status)
            {
                case StepStatus.None: SetValue(DefaultColor); SetValue(None); break;
                case StepStatus.InProgress: SetValue(InProgressColor); SetValue(InProgress); break;
                case StepStatus.Completed: SetValue(CompletedColor); SetValue(Completed); break;
                case StepStatus.Failed: SetValue(FailedColor); SetValue(Failed); break;
                default: SetValue(DefaultColor); break;
            }
        }

        private void OnValidate()
        {
            if (!label) label = transform.GetChild(0)?.GetComponent<TMP_Text>();
            if (!value) value = transform.GetChild(1)?.GetComponent<TMP_Text>();
        }
    }
}