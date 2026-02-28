using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Editor.Misc
{
    public class SchedulerDebuggerWindow : EditorWindow
    {
        [MenuItem("Window/YanickSenn/Scheduler Debugger")]
        public static void ShowWindow()
        {
            var window = GetWindow<SchedulerDebuggerWindow>("Scheduler Debugger");
            window.titleContent = new GUIContent("Scheduler Debugger");
            window.Show();
        }

        private Scheduler _targetScheduler;
        private SchedulerTreeView _treeView;
        private TreeViewState _treeViewState;
        private MultiColumnHeaderState _multiColumnHeaderState;

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            _treeViewState = new TreeViewState();
            var headerState = CreateDefaultMultiColumnHeaderState();
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(_multiColumnHeaderState, headerState))
                MultiColumnHeaderState.OverwriteSerializedFields(_multiColumnHeaderState, headerState);
            _multiColumnHeaderState = headerState;
            
            var multiColumnHeader = new MultiColumnHeader(_multiColumnHeaderState);
            _treeView = new SchedulerTreeView(_treeViewState, multiColumnHeader);
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Repaint();
        }

        private void Update()
        {
            if (Application.isPlaying && _targetScheduler != null)
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            _targetScheduler = (Scheduler)EditorGUILayout.ObjectField(_targetScheduler, typeof(Scheduler), true, GUILayout.Width(250));
            if (GUILayout.Button("Find Active Scheduler", EditorStyles.toolbarButton, GUILayout.Width(150)))
            {
#if UNITY_2023_1_OR_NEWER
                _targetScheduler = UnityEngine.Object.FindAnyObjectByType<Scheduler>();
#else
                _targetScheduler = UnityEngine.Object.FindObjectOfType<Scheduler>();
#endif
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (_targetScheduler == null)
            {
                EditorGUILayout.HelpBox("Select a Scheduler component to debug.", MessageType.Info);
                return;
            }

            _treeView.TargetScheduler = _targetScheduler;
            RefreshData();

            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            _treeView.OnGUI(rect);
        }

        private void RefreshData()
        {
            if (!Application.isPlaying)
            {
                _treeView.UpdateData(new List<OrderDebugData>());
                return;
            }

            if (SchedulerReflection.OrdersField == null) return;

            var data = new List<OrderDebugData>();

            var orders = (IEnumerable)SchedulerReflection.OrdersField.GetValue(_targetScheduler);
            if (orders != null)
            {
                foreach (var order in orders) data.Add(new OrderDebugData(order, "Active"));
            }

            var pendingAdd = (IEnumerable)SchedulerReflection.PendingAdditionsField.GetValue(_targetScheduler);
            if (pendingAdd != null)
            {
                foreach (var order in pendingAdd) data.Add(new OrderDebugData(order, "Pending Add"));
            }

            var pendingRem = (IEnumerable)SchedulerReflection.PendingRemovalsField.GetValue(_targetScheduler);
            if (pendingRem != null)
            {
                foreach (var order in pendingRem) data.Add(new OrderDebugData(order, "Pending Remove"));
            }

            _treeView.UpdateData(data);
        }

        private MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Order ID"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 150,
                    minWidth = 60,
                    autoResize = true,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Target / Callback"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 200,
                    minWidth = 100,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Trigger Time"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 80,
                    minWidth = 50,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Remaining"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 80,
                    minWidth = 50,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Delay"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 60,
                    minWidth = 40,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Interval"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 60,
                    minWidth = 40,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Periodic"),
                    headerTextAlignment = TextAlignment.Center,
                    width = 60,
                    minWidth = 40,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Status"),
                    headerTextAlignment = TextAlignment.Center,
                    width = 110,
                    minWidth = 60,
                    autoResize = true
                }
            };

            return new MultiColumnHeaderState(columns);
        }
    }

    public static class SchedulerReflection
    {
        public static readonly FieldInfo OrdersField;
        public static readonly FieldInfo PendingAdditionsField;
        public static readonly FieldInfo PendingRemovalsField;

        public static readonly Type ScheduledOrderType;
        public static readonly PropertyInfo OrderIdProperty;
        public static readonly PropertyInfo CallbackProperty;
        public static readonly PropertyInfo DelayProperty;
        public static readonly PropertyInfo IntervalProperty;
        public static readonly PropertyInfo IsPeriodicProperty;
        public static readonly PropertyInfo TriggerTimeProperty;
        public static readonly PropertyInfo IsPausedProperty;
        public static readonly PropertyInfo RemainingTimeProperty;

        static SchedulerReflection()
        {
            var schedulerType = typeof(Scheduler);
            OrdersField = schedulerType.GetField("_orders", BindingFlags.NonPublic | BindingFlags.Instance);
            PendingAdditionsField = schedulerType.GetField("_pendingAdditions", BindingFlags.NonPublic | BindingFlags.Instance);
            PendingRemovalsField = schedulerType.GetField("_pendingRemovals", BindingFlags.NonPublic | BindingFlags.Instance);

            ScheduledOrderType = schedulerType.GetNestedType("ScheduledOrder", BindingFlags.NonPublic);
            if (ScheduledOrderType != null)
            {
                OrderIdProperty = ScheduledOrderType.GetProperty("OrderId", BindingFlags.Public | BindingFlags.Instance);
                CallbackProperty = ScheduledOrderType.GetProperty("Callback", BindingFlags.Public | BindingFlags.Instance);
                DelayProperty = ScheduledOrderType.GetProperty("Delay", BindingFlags.Public | BindingFlags.Instance);
                IntervalProperty = ScheduledOrderType.GetProperty("Interval", BindingFlags.Public | BindingFlags.Instance);
                IsPeriodicProperty = ScheduledOrderType.GetProperty("IsPeriodic", BindingFlags.Public | BindingFlags.Instance);
                TriggerTimeProperty = ScheduledOrderType.GetProperty("TriggerTime", BindingFlags.Public | BindingFlags.Instance);
                IsPausedProperty = ScheduledOrderType.GetProperty("IsPaused", BindingFlags.Public | BindingFlags.Instance);
                RemainingTimeProperty = ScheduledOrderType.GetProperty("RemainingTime", BindingFlags.Public | BindingFlags.Instance);
            }
        }
    }

    public class OrderDebugData
    {
        public object RawOrder { get; }
        public string OrderId { get; }
        public string TargetName { get; }
        public UnityEngine.Object TargetObject { get; }
        public float Delay { get; }
        public float Interval { get; }
        public bool IsPeriodic { get; }
        public float TriggerTime { get; }
        public bool IsPaused { get; }
        public float RemainingTime { get; }
        public string State { get; }

        public float RemainingTimeDisplay => IsPaused ? RemainingTime : Mathf.Max(0, TriggerTime - Time.time);

        public OrderDebugData(object rawOrder, string state)
        {
            RawOrder = rawOrder;
            State = state;

            if (rawOrder == null || SchedulerReflection.ScheduledOrderType == null) return;

            OrderId = (string)SchedulerReflection.OrderIdProperty.GetValue(rawOrder);
            var callback = SchedulerReflection.CallbackProperty.GetValue(rawOrder);
            
            TargetName = callback != null ? callback.ToString() : "null";
            if (callback is UnityEngine.Object obj && obj != null)
            {
                TargetObject = obj;
                TargetName = $"{obj.name} ({obj.GetType().Name})";
            }
            else if (callback != null)
            {
                TargetName = callback.GetType().Name;
            }

            Delay = (float)SchedulerReflection.DelayProperty.GetValue(rawOrder);
            Interval = (float)SchedulerReflection.IntervalProperty.GetValue(rawOrder);
            IsPeriodic = (bool)SchedulerReflection.IsPeriodicProperty.GetValue(rawOrder);
            TriggerTime = (float)SchedulerReflection.TriggerTimeProperty.GetValue(rawOrder);
            IsPaused = (bool)SchedulerReflection.IsPausedProperty.GetValue(rawOrder);
            RemainingTime = (float)SchedulerReflection.RemainingTimeProperty.GetValue(rawOrder);
        }
    }

    public class OrderTreeViewItem : TreeViewItem
    {
        public OrderDebugData Data { get; set; }

        public OrderTreeViewItem(int id, int depth, string displayName, OrderDebugData data) : base(id, depth, displayName)
        {
            Data = data;
        }
    }

    public class SchedulerTreeView : TreeView
    {
        public Scheduler TargetScheduler { get; set; }
        private List<OrderDebugData> _data = new List<OrderDebugData>();

        public SchedulerTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            multiColumnHeader.sortingChanged += OnSortingChanged;
        }

        private void OnSortingChanged(MultiColumnHeader header)
        {
            SortData();
            Reload();
        }

        private void SortData()
        {
            if (multiColumnHeader.sortedColumnIndex < 0) return;

            var columnIndex = multiColumnHeader.sortedColumnIndex;
            var ascending = multiColumnHeader.IsSortedAscending(columnIndex);

            switch (columnIndex)
            {
                case 0: _data = ascending ? _data.OrderBy(d => d.OrderId).ToList() : _data.OrderByDescending(d => d.OrderId).ToList(); break;
                case 1: _data = ascending ? _data.OrderBy(d => d.TargetName).ToList() : _data.OrderByDescending(d => d.TargetName).ToList(); break;
                case 2: _data = ascending ? _data.OrderBy(d => d.TriggerTime).ToList() : _data.OrderByDescending(d => d.TriggerTime).ToList(); break;
                case 3: _data = ascending ? _data.OrderBy(d => d.RemainingTimeDisplay).ToList() : _data.OrderByDescending(d => d.RemainingTimeDisplay).ToList(); break;
                case 4: _data = ascending ? _data.OrderBy(d => d.Delay).ToList() : _data.OrderByDescending(d => d.Delay).ToList(); break;
                case 5: _data = ascending ? _data.OrderBy(d => d.Interval).ToList() : _data.OrderByDescending(d => d.Interval).ToList(); break;
                case 6: _data = ascending ? _data.OrderBy(d => d.IsPeriodic).ToList() : _data.OrderByDescending(d => d.IsPeriodic).ToList(); break;
                case 7: _data = ascending ? _data.OrderBy(d => d.State == "Active" && d.IsPaused ? "Paused" : d.State).ToList() : _data.OrderByDescending(d => d.State == "Active" && d.IsPaused ? "Paused" : d.State).ToList(); break;
            }
        }

        public void UpdateData(List<OrderDebugData> data)
        {
            bool requiresReload = _data.Count != data.Count;
            if (!requiresReload)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (_data[i].RawOrder != data[i].RawOrder)
                    {
                        requiresReload = true;
                        break;
                    }
                }
            }

            _data = data;

            if (requiresReload)
            {
                SortData();
                Reload();
            }
            else
            {
                var rows = GetRows();
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        if (row is OrderTreeViewItem orderItem)
                        {
                            var updatedData = _data.FirstOrDefault(d => d.RawOrder == orderItem.Data.RawOrder);
                            if (updatedData != null)
                                orderItem.Data = updatedData;
                        }
                    }
                }
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var allItems = new List<TreeViewItem>();

            for (int i = 0; i < _data.Count; i++)
            {
                var item = new OrderTreeViewItem(i + 1, 0, _data[i].OrderId, _data[i]);
                allItems.Add(item);
            }

            SetupParentsAndChildrenFromDepths(root, allItems);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as OrderTreeViewItem;
            if (item == null) return;

            var data = item.Data;

            for (int i = 0; i < args.GetNumVisibleColumns(); i++)
            {
                var cellRect = args.GetCellRect(i);
                CenterRectUsingSingleLineHeight(ref cellRect);

                int columnIndex = args.GetColumn(i);
                switch (columnIndex)
                {
                    case 0:
                        EditorGUI.LabelField(cellRect, data.OrderId);
                        break;
                    case 1:
                        EditorGUI.LabelField(cellRect, data.TargetName);
                        break;
                    case 2:
                        EditorGUI.LabelField(cellRect, data.TriggerTime.ToString("F2"));
                        break;
                    case 3:
                        EditorGUI.LabelField(cellRect, data.RemainingTimeDisplay.ToString("F2"));
                        break;
                    case 4:
                        EditorGUI.LabelField(cellRect, data.Delay.ToString("F2"));
                        break;
                    case 5:
                        EditorGUI.LabelField(cellRect, data.Interval.ToString("F2"));
                        break;
                    case 6:
                        EditorGUI.LabelField(cellRect, data.IsPeriodic ? "Yes" : "No");
                        break;
                    case 7:
                        var originalColor = GUI.color;
                        string displayState = data.State;
                        if (data.State == "Active" && data.IsPaused)
                        {
                            displayState = "Paused";
                            GUI.color = Color.yellow;
                        }
                        else if (data.State == "Pending Add") GUI.color = Color.cyan;
                        else if (data.State == "Pending Remove") GUI.color = Color.red;
                        else GUI.color = Color.green;
                        
                        EditorGUI.LabelField(cellRect, displayState, EditorStyles.boldLabel);
                        GUI.color = originalColor;
                        break;
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            var item = FindItem(id, rootItem) as OrderTreeViewItem;
            if (item != null && item.Data.TargetObject != null)
            {
                EditorGUIUtility.PingObject(item.Data.TargetObject);
                Selection.activeObject = item.Data.TargetObject;
            }
        }

        protected override void ContextClickedItem(int id)
        {
            var item = FindItem(id, rootItem) as OrderTreeViewItem;
            if (item == null || TargetScheduler == null) return;

            var data = item.Data;
            var callback = SchedulerReflection.CallbackProperty.GetValue(data.RawOrder) as ISchedulerEventHandler;

            var menu = new GenericMenu();
            
            if (data.IsPaused)
            {
                menu.AddItem(new GUIContent("Resume Order"), false, () => 
                {
                    TargetScheduler.ResumeOrder(callback, data.OrderId);
                });
            }
            else
            {
                menu.AddItem(new GUIContent("Pause Order"), false, () => 
                {
                    TargetScheduler.PauseOrder(callback, data.OrderId);
                });
            }

            menu.AddSeparator("");
            
            menu.AddItem(new GUIContent("Cancel Order"), false, () => 
            {
                TargetScheduler.CancelOrder(callback, data.OrderId);
            });

            menu.ShowAsContext();
        }
    }
}