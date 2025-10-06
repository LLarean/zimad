using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZIMAD
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private Button _forward;
        [SerializeField] private Button _stop;
        [SerializeField] private Button _back;

        public event Action OnForwardClicked;
        public event Action OnStopClicked;
        public event Action OnBackClicked;
    
        private void Start()
        {
            _forward.onClick.AddListener(ForwardClick);
            _stop.onClick.AddListener(StopClick);
            _back.onClick.AddListener(BackClick);
        }

        private void ForwardClick() => OnForwardClicked?.Invoke();

        private void StopClick() => OnStopClicked?.Invoke();

        private void BackClick() => OnBackClicked?.Invoke();
    }
}