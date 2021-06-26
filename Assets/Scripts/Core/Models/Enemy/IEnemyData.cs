namespace Core.Models.Enemy
{
    public interface IEnemyData
    {
        EnemyType Type { get; }
        int Score { get; }
    }
}