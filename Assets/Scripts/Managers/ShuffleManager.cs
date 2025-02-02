
public class ShuffleManager : IProvidable
{
    public ShuffleManager()
    {
        ServiceProvider.Register(this);
    }

    public void TryShuffle()
    {
        if(ServiceProvider.MatchManager.IsThereValidMatchGroupPresent()) return;
        Shuffle();

    }

    private void Shuffle()
    {
        UnityEngine.Debug.Log("Deadlock detected");

    }

    private void CheckDeadlock()
    {
        // Check if the board is in a deadlock state
    }

}