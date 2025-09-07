using LastBreakthrought.Configs.Game;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuDifficultyButton : MonoBehaviour 
    {
        public enum Difficulty {
            Easy = 1, Normal = 2, Hard = 3
        }

        [Header("UI")]
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _difficultyText;

        [Header("GameConfigs")]
        [SerializeField] private GameConfigSO _easy;
        [SerializeField] private GameConfigSO _normal;
        [SerializeField] private GameConfigSO _hard;

        private IConfigProviderService _configProviderService;

        private int _currentDifficultyIndex = 1;
        private Difficulty _currentDifficulty;

        [Inject]
        private void Construct(IConfigProviderService configProviderService) =>
            _configProviderService = configProviderService;

        private void Awake()
        {
            _button.onClick.AddListener(() => ChangeDifficulty());
            UpdateDifficultyText(_currentDifficultyIndex);
            SetGameConfig(_currentDifficultyIndex);
        }

        public Difficulty GetCurrentDifficulty() => _currentDifficulty;

        private void ChangeDifficulty()
        {
            if (_currentDifficultyIndex < Constants.MAX_DIFFICULTY_INDEX)
            {
                _currentDifficultyIndex++;
                UpdateDifficulty(_currentDifficultyIndex);
                SetGameConfig(_currentDifficultyIndex);
            }
            else
            {
                _currentDifficultyIndex = 1;
                UpdateDifficulty(_currentDifficultyIndex);
                SetGameConfig(_currentDifficultyIndex);
            }
        }

        private void UpdateDifficulty(int currentDifficultyIndex)
        {
            UpdateDifficultyText(currentDifficultyIndex);
            _currentDifficulty = GetDifficultyByIndex(currentDifficultyIndex);
        }

        private void UpdateDifficultyText(int currentDifficultyIndex) =>
            _difficultyText.text = GetDifficultyByIndex(currentDifficultyIndex).ToString();

        private Difficulty GetDifficultyByIndex(int index)
        {
            return index switch
            {
                1 => Difficulty.Easy,
                2 => Difficulty.Normal,
                3 => Difficulty.Hard,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
        }

        private void SetGameConfig(int index)
        {
            switch(index)
            {
                case 1:
                    _configProviderService.GameConfigSO = _easy;
                    break;
                case 2:
                    _configProviderService.GameConfigSO = _normal;
                    break;
                case 3:
                    _configProviderService.GameConfigSO = _hard;
                    break;
                    default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            };
        }
    }
}
