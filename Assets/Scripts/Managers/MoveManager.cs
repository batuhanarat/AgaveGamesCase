using System;

public interface IMoveManager
{
    int MoveCount { get; }
    bool CanMakeMove { get; }
    void MakeMove();
    void LockMove();
    void OpenMove();
    void Reset();
}

public class MoveManager : IProvidable , IMoveManager
{
    public int MoveCount { get; private set; }
    public bool CanMakeMove { get => !_locked && MoveCount>0 ; }

    private bool _locked = false;
    private Action<int> _onMoveCountChanged;

    public MoveManager()
    {
        ServiceProvider.Register(this);
        MoveCount = ServiceProvider.GameConfig.MoveCount;
        _onMoveCountChanged = ServiceProvider.UIManager.OnMoveCountUIChanged;
    }

    public void MakeMove()
    {
        if(!CanMakeMove) return;
        MoveCount--;
        _onMoveCountChanged(MoveCount);
        if(MoveCount == 0)
        {
            LockMove();
            ServiceProvider.LevelManager.DecideLevelStatus();
        }
    }

    public void LockMove()
    {
        _locked = true;
    }

    public void OpenMove()
    {
        _locked = false;
    }

    public void Reset()
    {
        MoveCount = ServiceProvider.GameConfig.MoveCount;
        _onMoveCountChanged(MoveCount);
        OpenMove();
    }

}