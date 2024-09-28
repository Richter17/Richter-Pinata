using System;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure;
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
        private bool _ranOnce;

        public static JuiceType Juice { get; private set; } = JuiceType.Normal;
        public void Show()
        {
            JuiceToggle.OnSelectJuiceType = SelectJuiceType;
            if(_ranOnce)
                _pinata.Reset();
            _ranOnce = true;
            _lightsMain.gameObject.SetActive(true);
            _pinataMain.gameObject.SetActive(true);
            HideMenu();
            SoundManager.Instance.PlaySFX(_introSfx);
            _pinata.OnExplosion += OnPinataExplosion;
            Enter();
        }

        private void Start()
        {
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
            Juice = juiceType;
        }

        private void OnPinataExplosion()
        {
            DelayShowMenu();
        }

        private async Task DelayShowMenu()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            await _lightsMain.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutSine).AsyncWaitForCompletion();
            await ShowMenu();
            _lightsMain.gameObject.SetActive(false);
            _pinataMain.gameObject.SetActive(false);
            _pinata.Reset();
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
