public class CleanupManager {
  public static CleanupManager Instance { get; set; } = new ();
  public delegate void CleanupFunction();
  private CleanupFunction onSceneTransition;
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