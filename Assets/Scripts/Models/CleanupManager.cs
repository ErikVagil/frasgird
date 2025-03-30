/// <summary>
/// Used to clean up the event handlers. Specifically, it prevents bugs
/// from popping up when transitioning between scenes, as gameobjects get
/// destroyed and the subscriptions need to be cleaned up.
/// </summary>
public class CleanupManager {
  public static CleanupManager Instance { get; set; } = new ();
  public delegate void CleanupFunction();
  private CleanupFunction onSceneTransition;

  /// <summary>
  /// Cleans up itself after it fires once.
  /// </summary>
  public void Cleanup() {
    onSceneTransition?.Invoke();
    onSceneTransition = null;   // mass unsubscribe
  }

  public void SubscribeToCleanup(CleanupFunction cleanUp) {
    onSceneTransition += cleanUp;
  }
  public void UnsubscribeToCleanup(CleanupFunction cleanUp) {
    onSceneTransition -= cleanUp;
  }
}