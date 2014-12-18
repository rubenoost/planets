namespace Planets.Controller.GameRules.GameTargets
{
    public interface IGameTarget
    {
        bool IsTargetReached(GameEngine ge);
    }
}
