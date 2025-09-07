using System;
using System.Collections.Generic;
using Scenario.Editor.Content.Fields;
using Scenario.Editor.Content.UXML;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenario.Editor.SRF
{
    /// <summary>
    /// Контекст для работы SRF, используется как главный проводник с пользовательскими классами
    /// </summary>
    public class SRFContext
    {
        public struct ParameterEntry
        {
            public object Value;
            public VisualElement RootElement;
            public VisualElement Parent;
        }

        public struct ListEntry
        {
            public int Count;
            public VisualElement RootElement;
            public VisualElement Parent;
        }

        private readonly Foldout foldoutRoot;
        private readonly VisualTreeAsset foldoutAsset;
        private readonly Button executableBtn;
        private readonly VisualTreeAsset fieldAsset;

        private readonly Dictionary<string, ParameterEntry> parameters = new();
        private readonly Dictionary<string, ListEntry> lists = new();

        public SRFContext(Foldout foldoutRoot, VisualTreeAsset foldoutAsset, Button executableBtn,
            VisualTreeAsset fieldAsset)
        {
            this.foldoutRoot = foldoutRoot;
            this.foldoutAsset = foldoutAsset;
            this.executableBtn = executableBtn;
            this.fieldAsset = fieldAsset;
        }

        #region Parameter

        /// <summary>
        /// Создаёт field, значение которого сохраняется в контексте под указанным именем
        /// </summary>
        /// <param name="name">Имя параметра (в UI тоже отображается) (и в хэш-таблице используется)</param>
        /// <param name="initialValue">Стартовое значение (опционально) (должен быть T, иначе исключение)</param>
        /// <param name="tooltip">Пояснение для параметра (опционально)</param>
        /// <param name="onValueInitialized">Ивент после инициализации initialValue (опционально)</param>
        /// <param name="onValueChanged">Ивент после изменения value через field (опционально)</param>
        /// <param name="overrideParent">Родитель, где будет находится VisualElement (null = корень) (опционально)</param>
        /// <typeparam name="T">Тип значения, для которого будет создан field</typeparam>
        public void CreateParameter<T>(string name, object initialValue = default,
            string tooltip = "",
            Action<SRFContext, string, T> onValueInitialized = null,
            Action<SRFContext, string, T, T> onValueChanged = null,
            VisualElement overrideParent = null)
        {
            if (ValidateInitialValue<T>(initialValue)) return;

            var valueType = typeof(T);
            var creator = ScenarioFields.GetCreator(valueType);
            if (!creator.CanCreate(valueType)) return; // если ошибка в GetCreator, это доп сравнение

            // создание field и зависимостей
            var entry = new ParameterEntry();
            var field = creator.CreateField(valueType, initialValue, ValueChangedCallback);

            // создание визуальных элементов для field
            var fieldUxml = new SRFFieldUxml(fieldAsset);
            fieldUxml.Field.Add(field);
            fieldUxml.Name.text = name;
            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                fieldUxml.Info.SetEnabled(false);
                fieldUxml.Info.tooltip = tooltip;
            }
            else
            {
                fieldUxml.Info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }

            // создание entry для контекста
            overrideParent ??= foldoutRoot;
            entry.Value = initialValue;
            entry.RootElement = fieldUxml.RootContainer;
            entry.Parent = overrideParent;

            // добавление подготовленных данных, завершение функции
            RemoveParameter(name);
            entry.Parent.Add(entry.RootElement);
            foldoutRoot.Remove(executableBtn);
            foldoutRoot.Add(executableBtn);
            parameters.Add(name, entry);
            onValueInitialized?.Invoke(this, name, (T)entry.Value);
            return;

            void ValueChangedCallback(object value)
            {
                var lastValue = entry.Value;
                entry.Value = value;
                parameters[name] = entry;

                if (value != default && value is not T)
                {
                    Debug.LogError($"Can't cast value \"{value}\" with type " +
                                   $"\"{value.GetType().Name}\" to type \"{valueType.Name}\"");
                    return;
                }

                onValueChanged?.Invoke(this, name, (T)lastValue, (T)entry.Value);
            }
        }


        /// <summary>
        /// Удаляет field и последнее значение сохранённого параметра в контексте
        /// </summary>
        /// <param name="name">Имя параметра, под которым хранилось value и field</param>
        /// <returns>Был ли удалён параметр и field</returns>
        public bool RemoveParameter(string name)
        {
            if (parameters.Remove(name, out var entry))
            {
                entry.Parent.Remove(entry.RootElement);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Возвращает значение параметра, введённое в field и сохранённое в контексте
        /// </summary>
        /// <param name="name">Имя параметра, под которым хранится значение</param>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <returns>Если имя найдено и T совпадает с ним - возвращает значение параметра, иначе default(T)</returns>
        public T GetParameterValue<T>(string name)
        {
            if (!TryGetValue(parameters, name, out var entry)) return default;

            switch (entry.Value)
            {
                case null:
                    return default;
                case T valueTyped:
                    return valueTyped;
                default:
                    Debug.LogError($"Can't cast value \"{entry.Value}\" with type \"{entry.Value.GetType().Name}\" " +
                                   $"to type \"{typeof(T).Name}\", return default");
                    return default;
            }
        }

        /// <summary>
        /// Возвращает структуру со всеми данными, ассоциируемыми с именем параметра
        /// </summary>
        /// <param name="name">Имя параметра, под которым хранится значение</param>
        /// <returns>Если имя найдено и T совпадает с ним - возвращает UI элемент параметра, иначе default(T)</returns>
        public ParameterEntry GetParameterEntry(string name)
        {
            return TryGetValue(parameters, name, out var entry) ? entry : default;
        }

        #endregion

        #region List

        public void CreateList<T>(string name, object initialValue = default, int initialCount = 1, string tooltip = "")
        {
            if (ValidateInitialValue<T>(initialValue)) return;

            var foldoutUxml = new SRFFuncUxml(foldoutAsset)
            {
                Foldout =
                {
                    //foldoutUxml.Field.Add(field);
                    text = name
                }
            };
            var entry = new ListEntry
            {
                Count = Mathf.Max(initialCount, 0),
                Parent = foldoutRoot,
                RootElement = foldoutUxml.Foldout,
            };

            // TODO не работает, кнопка не отображается
            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                foldoutUxml.Info.SetEnabled(false);
                foldoutUxml.Info.tooltip = tooltip;
            }
            else
            {
                foldoutUxml.Info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }

            RemoveList(name);
            entry.Parent.Add(entry.RootElement);
            lists[name] = entry;

            CreateParameter<int>(GetListCountName(name), initialCount, string.Empty,
                OnValueInitialized, OnValueChanged, foldoutUxml.Foldout);
            return;

            void OnValueInitialized(SRFContext context, string _, int current)
            {
                for (var i = 0; i < current; i++)
                    CreateParameter<T>(GetListIndexName(name, i), initialValue, overrideParent: foldoutUxml.Foldout);
                entry.Count = Mathf.Max(current, 0);
                lists[name] = entry;
            }

            void OnValueChanged(SRFContext context, string _, int prevValue, int nextValue)
            {
                for (var i = 0; i < prevValue; i++)
                    RemoveParameter(GetListIndexName(name, i));
                for (var i = 0; i < nextValue; i++)
                    CreateParameter<T>(GetListIndexName(name, i), initialValue, overrideParent: foldoutUxml.Foldout);
                entry.Count = Mathf.Max(nextValue, 0);
                lists[name] = entry;
            }
        }

        public bool RemoveList(string name)
        {
            if (lists.Remove(name, out var entry))
            {
                RemoveParameter(GetListCountName(name));
                for (var i = 0; i < entry.Count; i++)
                    RemoveParameter(GetListIndexName(name, i));

                entry.Parent.Remove(entry.RootElement);
                return true;
            }

            return false;
        }

        public T[] GetList<T>(string name)
        {
            if (!lists.TryGetValue(name, out var entryList))
            {
                Debug.LogError($"Can't find value by name \"{name}\", return default");
                return default;
            }

            var array = new T[entryList.Count];
            for (var i = 0; i < entryList.Count; i++)
            {
                var entry = parameters[GetListIndexName(name, i)];
                switch (entry.Value)
                {
                    case null:
                        array[i] = default;
                        break;
                    case T valueTyped:
                        array[i] = valueTyped;
                        break;
                    default:
                        Debug.LogError(
                            $"Can't cast value \"{entry.Value}\" with type \"{entry.Value.GetType().Name}\" " +
                            $"to type \"{typeof(T).Name}\", return default");
                        return default;
                }
            }

            return array;
        }

        public ListEntry GetListEntry(string name)
        {
            return TryGetValue(lists, name, out var entry) ? entry : default;
        }

        #endregion

        #region Utility

        private static bool ValidateInitialValue<T>(object initialValue)
        {
            if (initialValue != default && initialValue is not T)
            {
                Debug.LogError($"Can't cast initialValue \"{initialValue}\" with type " +
                               $"\"{initialValue.GetType().Name}\" to type \"{typeof(T).Name}\"");
                return true;
            }

            return false;
        }

        private static bool TryGetValue<T>(Dictionary<string, T> dict, string name, out T entry)
        {
            if (!dict.TryGetValue(name, out entry))
            {
                Debug.LogError($"Can't find value by name \"{name}\", return default");
                return true;
            }

            return false;
        }

        private static string GetListCountName(string name) => $"{name} Count";
        private static string GetListIndexName(string name, int index) => $"{name} {index}";

        #endregion
    }
}