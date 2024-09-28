using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Plugins.Richter.Haptic.Editor
{

public class VibrationTesterWindow : EditorWindow
{
    private AudioSource _audioSource; // Reference to the audio source
    private AudioClip _vibrationClip; // The vibration sound effect
    private Vector2 _scrollPosition; // Scroll position for the list
    private bool _isPlaying = false; // Track if vibration is playing

    private const string VibrateSoundPath = "Assets/Plugins/Richter/Haptic/Editor/vibrate_sfx.wav";

    [Serializable]
    private class VibrateWave
    {
        [SerializeField][Range(0,1)] public float _amplitude = 1.0f; // Volume of the sound
        [SerializeField] public float _duration = 1.0f; // Duration of the vibration in seconds
    }

    private VibrateWave[] _vibrationSettings;

    [MenuItem("Window/Vibration Tester")]
    public static void ShowWindow()
    {
        GetWindow<VibrationTesterWindow>("Vibration Tester");
    }

    private void OnEnable()
    {
        _vibrationSettings = new [] {new VibrateWave()};
        _vibrationClip = AssetDatabase.LoadAssetAtPath<AudioClip>(VibrateSoundPath);
        if(!_vibrationClip)
            Debug.LogError($"Audio clip at: {VibrateSoundPath} is missing");
    }

    private void OnDisable()
    {
        if(_audioSource)
            DestroyImmediate(_audioSource.gameObject);
    }

    private void OnGUI()
    {
        GUILayout.Label("Vibration Tester", EditorStyles.boldLabel);
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        // Display settings for each vibration combination
        for (int i = 0; i < _vibrationSettings.Length; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Vibration {i + 1}", EditorStyles.boldLabel);
            
        
            _vibrationSettings[i]._amplitude = EditorGUILayout.Slider("Amplitude", _vibrationSettings[i]._amplitude, 0f, 1f);
            _vibrationSettings[i]._duration = EditorGUILayout.FloatField("Duration (s)", _vibrationSettings[i]._duration);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // Play and stop buttons
        if (GUILayout.Button(_isPlaying ? "Stop Vibration" : "Play Vibration"))
        {
            if (_isPlaying)
            {
                StopVibration();
            }
            else
            {
                PlayVibration();
            }
        }

        if (GUILayout.Button("Add Vibration"))
        {
            AddVibration();
        }

        if (GUILayout.Button("Remove Vibration"))
        {
            RemoveVibration();
        }
    }

    private void PlayVibration()
    {
        if(!_vibrationClip)
            return;
        if (!_audioSource)
        {
            var vibrator = new GameObject("VibrationTest");
            _audioSource = vibrator.AddComponent<AudioSource>();
        }
        _audioSource.clip = _vibrationClip;
        _audioSource.loop = true;
        _isPlaying = true;
        PlayVibrationAsync();
    }

    private async Task PlayVibrationAsync()
    {
        if(!_audioSource)
            return;
        foreach (var settings in _vibrationSettings)
        {
            _audioSource.volume = settings._amplitude;
            _audioSource.clip = _vibrationClip;
            _audioSource.Play();
            await Task.Delay((int)(settings._duration * 1000));
            _audioSource.Stop();
        }

        _isPlaying = false;
    }

    private void StopVibration()
    {
        if (_audioSource)
        {
            _audioSource.Stop();
        }
        _isPlaying = false;
    }

    private void AddVibration()
    {
        var newSettings = new VibrateWave();
        var tempList = new VibrateWave[_vibrationSettings.Length + 1];
        _vibrationSettings.CopyTo(tempList, 0);
        tempList[_vibrationSettings.Length] = newSettings;
        _vibrationSettings = tempList;
    }

    private void RemoveVibration()
    {
        if (_vibrationSettings.Length > 1)
        {
            var tempList = new VibrateWave[_vibrationSettings.Length - 1];
            for (int i = 0; i < tempList.Length; i++)
            {
                tempList[i] = _vibrationSettings[i];
            }
            _vibrationSettings = tempList;
        }
    }
}
}