﻿using System;
using TMPro;
using UnityEngine;

namespace PracticePlugin
{
	public class SpeedSettingsCreator : MonoBehaviour
	{
		public event Action<float> ValueChangedEvent;
		
		private GameObject _speedSettings;
		private TMP_Text _leaderboardText;
		private float _newTimeScale = 1;
		
		private void OnEnable()
		{
			_leaderboardText = new GameObject("Leaderboard Text").AddComponent<TextMeshProUGUI>();
			var rectTransform = (RectTransform) _leaderboardText.transform;
			rectTransform.SetParent(transform, false);
			rectTransform.anchorMin = Vector2.right * 0.5f;
			rectTransform.anchorMax = Vector2.right * 0.5f;
			rectTransform.sizeDelta = new Vector2(100, 10);
			rectTransform.anchoredPosition = new Vector2(0, 15);
			_leaderboardText.fontSize = 4f;
			_leaderboardText.alignment = TextAlignmentOptions.Center;
			if (Plugin.HasTimeScaleChanged)
			{
				_leaderboardText.text = "Leaderboard has been disabled\nSet speed to 100% and restart to enable again";
			}
			
			_speedSettings = Instantiate(Plugin.SettingsObject, transform);
			_speedSettings.SetActive(true);
			
			rectTransform = (RectTransform) _speedSettings.transform;
			rectTransform.anchorMin = Vector2.right * 0.5f;
			rectTransform.anchorMax = Vector2.right * 0.5f;
			rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y * 1.5f);

			
			var speedController = _speedSettings.GetComponent<SpeedSettingsController>();
			speedController.ValueChangedEvent += SpeedControllerOnValueChangedEvent;
			speedController.Init();
		}

		private void OnDisable()
		{
			if (ValueChangedEvent != null)
			{
				ValueChangedEvent(_newTimeScale);
			}
			DestroyImmediate(_leaderboardText);
			DestroyImmediate(_speedSettings);
		}

		private void SpeedControllerOnValueChangedEvent(float timeScale)
		{
			_newTimeScale = timeScale;
			if (!Plugin.NoFail && !Plugin.HasTimeScaleChanged && Math.Abs(_newTimeScale - 1) > 0.0000000001f)
			{
				_leaderboardText.text = "Leaderboard will be disabled!";
			}
			else
			{
				_leaderboardText.text = Plugin.HasTimeScaleChanged ? "Leaderboard has been disabled\nSet speed to 100% and restart to enable again" : string.Empty;
			}
		}
	}
}