using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure;
using PinataMiniGame.Settings;
using UnityEngine;

namespace PinataMiniGame
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private PinataHandler _pinata;
        [SerializeField] private Transform _lightsMain;
        [SerializeField] private Transform _pinataMain;
        [SerializeField] private CanvasGroup _menu;
        [SerializeField] private AudioClip _introSfx;
        [SerializeField] private List<PinataJuiceSettings> _settings;

        public static JuiceType Juice { get; private set; } = JuiceType.Normal;
        public void Show()
        {
            Application.targetFrameRate = 60;
            if (_settings.Count == 0)
            {
                Debug.LogError("Pinata game can not be played! Settings is empty");
                return;
            }
            var settings = GetJuiceSettings();
            _pinata.Init(settings);
            _lightsMain.gameObject.SetActive(true);
            _pinataMain.gameObject.SetActive(true);
            HideMenu();
            SoundManager.Instance.PlaySFX(_introSfx);
            _pinata.OnExplosion += OnPinataExplosion;
            Enter();
        }

        private PinataJuiceSettings GetJuiceSettings()
        {
            var settings = _settings.Find(s => s.Juice == Juice);
            return settings == null ? _settings[0] : settings;
        }

        private void Start()
        {
            JuiceToggle.OnSelectJuiceType = SelectJuiceType;
            ShowMenu();
        }

        private async Task ShowMenu()
        {
            await _menu.DOFade(1, 0.5f).AsyncWaitForCompletion();
            _menu.interactable = true;
            _menu.blocksRaycasts = true;
        }

        private void HideMenu()
        {
            _menu.alpha = 0;
            _menu.interactable = false;
            _menu.blocksRaycasts = false;
        }

        private void SelectJuiceType(JuiceType juiceType)
        {
            Debug.Log($"select {juiceType}");
            Juice = juiceType;
        }

        private void OnPinataExplosion()
        {
            DelayShowMenu();
        }

        private async Task DelayShowMenu()
        {
            await Task.Delay(TimeSpan.FromSeconds(7));
            await _lightsMain.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutSine).AsyncWaitForCompletion();
            _lightsMain.gameObject.SetActive(false);
            _pinataMain.gameObject.SetActive(false);
            _pinata.Reset();
            await ShowMenu();
        }

        private async Task Enter()
        {
            _pinataMain.localScale = Vector3.zero;
            _lightsMain.localScale = Vector3.zero;
            await ShowLights();
            await RevealPinata();
        }

        private async Task RevealPinata()
        {
            await _pinataMain.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            _pinata.StartGame();
        }

        private async Task ShowLights()
        {
            _lightsMain.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }
}
