using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugConsole
{
    [CreateAssetMenu(fileName = "Console Settings", menuName = "Console Settings")]
    public class DebugConsoleSettings : ScriptableObject
    {
        public GUISkin skin;
        public string title = "Console";
        public float startWidthScale = 0.5f;
        public float startHeightScale = 0.5f;
        public Vector2 startingPosition;
        public float offsetDrag = 30f;

        [Header("Text Properties")]
        public Color commandColor = Color.blue;
        public Color logColor = Color.white;
        public Color warningColor = Color.yellow;
        public Color errorColor = Color.red;

        public int entrySize = 12;
        public int currentEntrySize = 14;

        [Header("Text Field")]
        public float height = 25;

        [Header("Toggle Bar")]
        public float toolbarFontSize = 14;
        public float toggleBarMaxWidth = 200;
        public float toggleBarMinWidth = 300;

        [Header("ExtendButton Properties")]
        public Texture2D extendTexture;
        public Vector2 extendButtonOffset;
        public float extendButtonSize;

    }
}
