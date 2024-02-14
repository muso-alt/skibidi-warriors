using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skibidi.Views
{
    public class GamePopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private Button _start;

        public Button StartButton => _start;

        public void SetStatusText(string text)
        {
            _statusText.text = text;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}