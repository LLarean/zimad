namespace ZIMAD
{
    public class GameplayPresenter
    {
        private readonly PlayerController _playerController;
        private readonly GameplayView _gameplayView;

        public GameplayPresenter(PlayerController playerController, GameplayView gameplayView)
        {
            _playerController = playerController;
            _gameplayView = gameplayView;
        }

        public void Subscribe()
        {
            _gameplayView.OnForwardClicked += MoveForward;
            _gameplayView.OnBackClicked += MoveBack;
            _gameplayView.OnStopClicked += StopMoving;
        }

        public void Dispose()
        {
            _gameplayView.OnForwardClicked -= MoveForward;
            _gameplayView.OnBackClicked -= MoveBack;
            _gameplayView.OnStopClicked -= StopMoving;
        }

        private void MoveForward() => _playerController.MoveForward();

        private void MoveBack() => _playerController.MoveBack();

        private void StopMoving() => _playerController.StopMoving();
    }
}