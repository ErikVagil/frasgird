using UnityEngine;
using TMPro;
public class ColonyUIController : MonoBehaviour {
  public static ColonyUIController Instance { get; private set; }
  public GameObject canvas;
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    } // else error?

    UpdateUI();
    Colony.Instance.SubscribeToUpdates(UpdateUI);
  }
  private void UpdateUI() {
    UpdateResources();
    SetText("Tick", $"Tick: {Colony.Instance.CurrentTick}");
  }
  public void onNextTickClick() {
    Colony.Instance.NextTick();
  }
  private void UpdateResources() {
    SetText("Resources/Food", $"Food: {Colony.Instance.Food}");
    SetText("Resources/Water", $"Water: {Colony.Instance.Water}");
    SetText("Resources/Population", $"Population: {Colony.Instance.Population}");
    SetText("Resources/Power", $"Power: {Colony.Instance.PowerSurplus}");
  }
  private void SetText(string name, string text) {
    TextMeshProUGUI tmp = canvas.transform
      .Find($"Panel/{name}").gameObject
      .GetComponent<TextMeshProUGUI>();
    tmp.SetText(text);
  }
}