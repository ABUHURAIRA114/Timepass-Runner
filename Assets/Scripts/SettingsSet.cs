using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SettingsSet : MonoBehaviour, ISavable
{
    [SerializeField] private Toggle clouds;
    [SerializeField] private TMP_Dropdown shadows;
    [SerializeField] private TMP_Dropdown time;
    [SerializeField] private Toggle ambientOcculsion;
    [SerializeField] private Toggle bloom;
    [SerializeField] private Toggle motionBlur;
    [SerializeField] private Slider volume;
    [SerializeField] private VolumeProfile profile;
    [SerializeField] private new UniversalRendererData renderer;

    [SerializeField] List<TextMeshProUGUI> highScoresUI;
    private CrossSceneData sceneComm => CrossSceneData.i;

    private Setting settings = new Setting();
    private bool isLoading;
    private Bloom pBloom;
    private MotionBlur blur;
    private WhiteBalance whiteBalance;

    private void Start()
    {
        SavingSystem.i.Load("save1");
        
        sceneComm.clouds = clouds.isOn;
        if (time.value == 0) sceneComm.time = TimeOfDay.Morning;
        if (time.value == 1) sceneComm.time = TimeOfDay.Day;
        if (time.value == 2) sceneComm.time = TimeOfDay.Evening;
        sceneComm.shadowLevel = shadows.value;

        profile.TryGet(out pBloom);
        profile.TryGet(out whiteBalance);
        profile.TryGet(out blur);
        ApplySetting();
    }

    private void FixedUpdate() 
    {
        if (!isLoading) 
            settings.SetSettings(clouds.isOn, shadows.value, time.value, ambientOcculsion.isOn, bloom.isOn, motionBlur.isOn, volume.value);
    }

    public object CaptureState()
    {
        ApplySetting();

        sceneComm.clouds = clouds.isOn;
        if (time.value == 0) sceneComm.time = TimeOfDay.Morning;
        if (time.value == 1) sceneComm.time = TimeOfDay.Day;
        if (time.value == 2) sceneComm.time = TimeOfDay.Evening;
        sceneComm.shadowLevel = shadows.value;

        return settings;
    }

    public void RestoreState(object state)
    {
        isLoading = true;
    
        settings = (Setting)state;

        clouds.SetIsOnWithoutNotify(settings.clouds);
        ambientOcculsion.SetIsOnWithoutNotify(settings.ambientO);
        shadows.SetValueWithoutNotify(settings.shadowQuality);
        time.SetValueWithoutNotify(settings.time);
        bloom.SetIsOnWithoutNotify(settings.bloom);
        motionBlur.SetIsOnWithoutNotify(settings.motionBlur);
        volume.SetValueWithoutNotify(settings.volume);
            
        isLoading = false;
    }

    private void ApplySetting() {

        renderer.rendererFeatures[0].SetActive(ambientOcculsion.isOn);
        pBloom.active = bloom.isOn;
        blur.active = motionBlur.isOn;
        whiteBalance.temperature.value = time.value == 0 ? -40 : time.value == 1 ? -20 : 70;
        AudioManager.i.SetVolume(volume.value);
  
        for (int i = 0; i < highScoresUI.Count; i++)
        {
            highScoresUI[i].text = sceneComm.highScores[i].ToString();
        }
    }

    public void SaveSettings()
    {
        SavingSystem.i.Save("save1");
    }
}

[System.Serializable]
public class Setting
{
    public bool clouds;
    public int shadowQuality;
    public int time;
    public bool ambientO;
    public bool bloom;
    public bool motionBlur;
    public float volume;

    public void SetSettings(bool _clouds, int _shadowQuality, int _time, bool _ambientO, bool _bloom, bool _motionBlur, float _volume)
    {
        clouds = _clouds;
        shadowQuality = _shadowQuality;
        time = _time;
        ambientO = _ambientO;
        bloom = _bloom;
        motionBlur = _motionBlur;
        volume = _volume;
    }
}
