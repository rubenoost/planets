namespace Planets.Controller.GameRules.Abstract
{
    public interface INativeGameRule
    {
        void Execute(GameEngine ge, double ms);
    }
}
