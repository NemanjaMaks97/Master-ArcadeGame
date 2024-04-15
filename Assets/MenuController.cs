using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public GameObject SettingsPanel;
    public GameObject ControllsPanel;
    public Slider MusicSlider;
    public TMP_Text MusicValue;
    public Slider EffectsSlider;
    public TMP_Text EffectsValue;
    public TMP_Dropdown DifficultyDropdown;

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OppenSettings()
    {
        MusicSlider.value = Game.MusicVolume;
        EffectsSlider.value = Game.EffectVolume;
        DifficultyDropdown.value = Game.GameDificulty;
        MusicValue.text = ((int)(MusicSlider.value * 100)).ToString();
        EffectsValue.text = ((int)(EffectsSlider.value * 100)).ToString();
        SettingsPanel.SetActive(true);
    }

    public void OppenControlls()
    {
        ControllsPanel.SetActive(true);
    }

    public void ControllsBack()
    {
        ControllsPanel.SetActive(false);
    }

    public void SettingsApply()
    {
        Game.MusicVolume = MusicSlider.value;
        Game.EffectVolume = EffectsSlider.value;
        Game.GameDificulty = DifficultyDropdown.value;
        SettingsPanel.SetActive(false);
    }
    public void SettingsCancel()
    {
        MusicSlider.value = Game.MusicVolume;
        EffectsSlider.value = Game.EffectVolume;
        DifficultyDropdown.value = Game.GameDificulty;
    }

    public void SettingsBack()
    {
        SettingsPanel.SetActive(false);
    }

    public void MusicValueChanged()
    {
        MusicValue.text = ((int)(MusicSlider.value * 100)).ToString();
    }

    public void EffectsValueChanged()
    {
        EffectsValue.text = ((int)(EffectsSlider.value * 100)).ToString();
    }

}
