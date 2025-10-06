using UnityEngine;

namespace ZIMAD
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GameplayView _gameplayView;
        [SerializeField] private PlayerController _playerController;

        private GameplayPresenter _gameplayPresenter;
        
        private void Start()
        {
            _gameplayPresenter = new GameplayPresenter(_playerController, _gameplayView);
            _gameplayPresenter.Subscribe();
        }

        private void OnDestroy()
        {
            _gameplayPresenter.Dispose();
        }
    }
}