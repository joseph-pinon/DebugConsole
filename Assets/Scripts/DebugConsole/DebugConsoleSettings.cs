using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugConsole
{
    [CreateAssetMenu(fileName = "Console Settings", menuName = "Console Settings")]
    public class DebugConsoleSettings : ScriptableObject
    {
        [field: SerializeField]
        public GUISkin Skin { get; private set; }

        public string title = "Console";
        public float startWidthScale = 0.5f;
        public float startHeightScale = 0.5f;
        public Vector2 startingPosition;

        [Header("Text Properties")]
        public Color commandColor = Color.blue;
        public Color logColor = Color.white;
        public Color warningColor = Color.yellow;
        public Color errorColor = Color.red;

        public int entrySize = 12;

        [Header("Toggle/Text Bar")]
        public int currentEntrySize = 14;

        [Header("ExtendButton Properties")]
        public Texture2D extendTexture;
        public int extendButtonOffset;
        public int extendButtonSize;

        private void OnValidate()
        {
            Skin.label.fontSize = entrySize;
            Skin.textField.fontSize = currentEntrySize;
            Skin.button.fontSize = currentEntrySize;
            Skin.customStyles[1].fontSize = currentEntrySize;
            Skin.window.padding.bottom = extendButtonSize / 2 + extendButtonOffset;
            Skin.window.padding.right = extendButtonSize + extendButtonOffset;
        }

    }
}
