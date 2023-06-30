using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DebugConsole
{
    [System.Serializable]
    public class DebugConsoleDrawer
    {
        [SerializeField]
        private List<ConsoleEntry> _entries;
        public List<ConsoleEntry> entries { get { return _entries; } }

        private DebugConsoleSettings _settings;

        private Rect _windowRect;
        private string _currentEntry;
        private Vector2 _scrollPosition;
        private Vector2 _dragMousePosition;
        private Vector2 _windowDimensions;
        private bool _scaling = false;

        public Action<string> commandPosted;


        private bool showTime;
        private bool showType;
        private bool showTrace;
        private bool showErrors;
        private bool showWarnings;


        public DebugConsoleDrawer(DebugConsoleSettings settings)
        {
            this._settings = settings;
            _currentEntry = "";
            _windowDimensions = new Vector2(Screen.width * this._settings.startWidthScale, Screen.height * this._settings.startHeightScale);
            _windowRect = new Rect(this._settings.startingPosition.x, this._settings.startingPosition.y, _windowDimensions.x, _windowDimensions.y);
            _entries = new List<ConsoleEntry>();
        }

        public void Reset()
        {
            _currentEntry = "";
            _windowDimensions = new Vector2(Screen.width * _settings.startWidthScale, Screen.height * _settings.startHeightScale);
            _windowRect = new Rect(_settings.startingPosition.x, _settings.startingPosition.y, _windowDimensions.x, _windowDimensions.y);
        }

        /// <summary>
        /// Edits how the console displays entries
        /// </summary>
        /// <param name="showTime"></param>
        /// <param name="showType"></param>
        /// <param name="showTrace"></param>
        public void EditConsole(bool showTime, bool showType = true, bool showTrace = true)
        {
            //this.showTime = showTime;
            //this.showType = showType;
            //this.showTrace = showTrace;
        }

        public void ClearEntries()
        {
            _entries.Clear();
        }
        public void Draw()
        {
            if (GUI.skin != null)
            {
                GUI.skin = _settings.skin;
                _settings.skin.label.fontSize = _settings.entrySize;
                _settings.skin.textField.fontSize = _settings.currentEntrySize;
            }
            _windowRect = GUILayout.Window(0, _windowRect, DrawConsole, _settings.title);
        }

        private void DrawConsole(int windowID)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        
            ScrollField();
            Toolbar();
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(_settings.height));

            TextField();
            ScalingButton();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            GUI.DragWindow(new Rect(0, 0, 10000, _windowDimensions.y - _settings.offsetDrag));
        }

        private void Toolbar()
        {
            GUILayout.BeginHorizontal(GUILayout.MaxWidth(_settings.toggleBarMaxWidth), 
                GUILayout.MinWidth(_settings.toggleBarMinWidth), GUILayout.ExpandWidth(true));
            showTime = GUILayout.Toggle(showTime, "Time", "button");
            showWarnings = GUILayout.Toggle(showWarnings, "Warnings", "button");
            showErrors = GUILayout.Toggle(showErrors, "Errors", "button");
            showType = GUILayout.Toggle(showType, "Type", "button");
            showTrace = GUILayout.Toggle(showTrace, "Trace", "button");
            GUILayout.EndHorizontal();

        }
        private void ScrollField()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            Color color = GUI.contentColor;
            
            foreach (ConsoleEntry entry in _entries)
            {
                if (entry.type == LogType.Log)
                {
                    GUI.contentColor = _settings.logColor;
                }
                else if (entry.type == LogType.Warning && showWarnings)
                {
                    GUI.contentColor = _settings.warningColor;
                }
                else if (entry.type == LogType.Error && showErrors)
                {
                    GUI.contentColor = _settings.errorColor;
                }
                else
                {
                    continue;
                }
                
                GUILayout.Label(entry.GetEntry(showTime, showType, showErrors));
            }

            GUI.contentColor = color;

            GUILayout.EndScrollView();
        }

        private void TextField()
        {
            _currentEntry = GUILayout.TextField(_currentEntry, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (string.IsNullOrEmpty(_currentEntry)) { return; }

            if (GUILayout.Button("Submit", GUILayout.Width(100), GUILayout.ExpandHeight(false)))
            {
                commandPosted?.Invoke(_currentEntry);
                _currentEntry = "";
                
            }

            if (Event.current.type == EventType.KeyDown)
            {
                commandPosted?.Invoke(_currentEntry);
                _currentEntry = "";
            }
        }

        public void CreateEntry(string text, LogType type = LogType.Log)
        {
            ConsoleEntry entry = new ConsoleEntry(text, type);
            _entries.Add(entry);
            _scrollPosition.y = Mathf.Infinity;
 
        }

        private void ScalingButton()
        {
            GUILayout.Box("", GUILayout.Width(20), GUILayout.Height(20));

            if (Event.current.type == EventType.MouseUp)
            {
                _scaling = false;
                _windowDimensions = new Vector2(_windowRect.width, _windowRect.height);
            }
            else if (Event.current.type == EventType.MouseDown &&
                     GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                _dragMousePosition = Event.current.mousePosition;
                _scaling = true;
            }

            if (_scaling)
            {
                Vector2 resizeDelta = Event.current.mousePosition - _dragMousePosition;
                _windowRect = new Rect(_windowRect.x, _windowRect.y,_windowDimensions.x + resizeDelta.x, _windowDimensions.y + resizeDelta.y);
            }

        }
    }
}
