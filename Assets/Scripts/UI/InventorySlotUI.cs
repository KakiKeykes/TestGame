using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] public Image _image;
        [SerializeField] public TMP_Text _textCount;
        [SerializeField] public RectTransform _rectTransform;
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            EventManager.onSelectInventorySlot?.Invoke(this);
        }
    }
}