using System;

public class ScoreManager : IProvidable
{
    public int Score { get; private set; }
    private int _scoreLimit;
    private Action<int> _onScoreChanged;

    public ScoreManager()
    {
        ServiceProvider.Register(this);
        _scoreLimit = ServiceProvider.GameConfig.ScoreLimit;
        _onScoreChanged = ServiceProvider.UIManager.OnScoreCountUIChanged;
    }

    public void IncrementScore(int value)
    {
        Score += value;
        _onScoreChanged(Score);
    }

    public bool IsWin()
    {
        return Score >= _scoreLimit;
    }

    public void Reset()
    {
        Score = 0;
        _onScoreChanged(Score);
    }

}