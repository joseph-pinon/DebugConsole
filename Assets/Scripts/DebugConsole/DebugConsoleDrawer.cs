using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DebugConsole
{
    /// <summary>
    /// Responsible for drawing the console window and holding all console entries
    /// </summary>
    [System.Serializable]
    public class DebugConsoleDrawer
    {
        [field: SerializeField]
        public List<ConsoleEntry> Entries { get; private set; }

        private DebugConsoleSettings _settings;

        //Console State
        private Rect _windowRect;
        private string _currentEntry;
        private Vector2 _scrollPositionEntries;
        private Vector2 _scrollPositionTextfield;
        private Vector2 _dragMousePosition;
        private Vector2 _windowDimensions;
        private bool _scaling = false;

        //Events
        public Action<string> commandPosted;

        //Display settings
        private bool showTime = true;
        private bool showType = true;
        private bool showTrace = true;
        private bool showErrors = true;
        private bool showWarnings = true;

        public DebugConsoleDrawer(DebugConsoleSettings settings)
        {
            this._settings = settings;
            _currentEntry = "";
            _windowDimensions = new Vector2(Screen.width * this._settings.startWidthScale, Screen.height * this._settings.startHeightScale);
            _windowRect = new Rect(this._settings.startingPosition.x, this._settings.startingPosition.y, _windowDimensions.x, _windowDimensions.y);
            Entries = new List<ConsoleEntry>();
        }

        /// <summary>
        /// Resets the console location and size
        /// </summary>
        public void Reset()
        {
            _currentEntry = "";
            _windowDimensions = new Vector2(Screen.width * _settings.startWidthScale, Screen.height * _settings.startHeightScale);
            _windowRect = new Rect(_settings.startingPosition.x, _settings.startingPosition.y, _windowDimensions.x, _windowDimensions.y);
        }


        /// <summary>
        /// Clears console entries
        /// </summary>
        public void ClearEntries()
        {
            Entries.Clear();
        }

        /// <summary>
        /// Creates and adds console entry
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public void CreateEntry(string text, LogType type = LogType.Log)
        {
            ConsoleEntry entry = new ConsoleEntry(text, type);
            Entries.Add(entry);
            _scrollPositionEntries.y = Mathf.Infinity;

        }


        #region Drawing Functions
        /// <summary>
        /// Draws the console to the screen
        /// </summary>
        public void Draw()
        {
            if (GUI.skin != null)
            {
                GUI.skin = _settings.Skin;
            }
            _windowRect = GUILayout.Window(0, _windowRect, DrawConsole, _settings.title);
        }

        private void DrawConsole(int windowID)
        {


            GUILayout.BeginVertical();
            ScrollField();
            Toolbar();
            GUILayout.BeginHorizontal();

            TextField();
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            ScalingButton();

            GUI.DragWindow(new Rect(0, 0, 10000, _windowDimensions.y - _settings.extendButtonSize));


        }

        private void Toolbar()
        {
            GUILayout.BeginHorizontal();
            showTime = GUILayout.Toggle(showTime, "Time", GUI.skin.customStyles[1], GUILayout.ExpandWidth(false));
            showWarnings = GUILayout.Toggle(showWarnings, "Warnings", GUI.skin.customStyles[1], GUILayout.ExpandWidth(false));
            showErrors = GUILayout.Toggle(showErrors, "Errors", GUI.skin.customStyles[1], GUILayout.ExpandWidth(false));
            showType = GUILayout.Toggle(showType, "Type", GUI.skin.customStyles[1], GUILayout.ExpandWidth(false));
            showTrace = GUILayout.Toggle(showTrace, "Trace", GUI.skin.customStyles[1], GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }
        private void ScrollField()
        {
            _scrollPositionEntries = GUILayout.BeginScrollView(_scrollPositionEntries);

            Color color = GUI.contentColor;

            foreach (ConsoleEntry entry in Entries)
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

            _currentEntry = GUILayout.TextField(_currentEntry);


            if (string.IsNullOrEmpty(_currentEntry)) { return; }

            if (GUILayout.Button("Submit", GUILayout.ExpandWidth(false)))
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

        private void ScalingButton()
        {
            float xPos = _windowRect.width - _settings.extendButtonSize - _settings.extendButtonOffset;
            float yPos = _windowRect.height - _settings.extendButtonSize - _settings.extendButtonOffset;
            Rect dragButton = new Rect(xPos, yPos, _settings.extendButtonSize, _settings.extendButtonSize);
            GUI.Label(dragButton, _settings.extendTexture, "icon");

            if (Event.current.type == EventType.MouseUp)
            {
                _scaling = false;
                _windowDimensions = new Vector2(_windowRect.width, _windowRect.height);
            }
            else if (Event.current.type == EventType.MouseDown &&
 
                     dragButton.Contains(Event.current.mousePosition))
            {
                _dragMousePosition = Event.current.mousePosition;
                _scaling = true;
            }

            if (_scaling)
            {
                Vector2 resizeDelta = Event.current.mousePosition - _dragMousePosition;
                _windowRect = new Rect(_windowRect.x, _windowRect.y, _windowDimensions.x + resizeDelta.x, _windowDimensions.y + resizeDelta.y);
            }

        }
        #endregion

    }
}
