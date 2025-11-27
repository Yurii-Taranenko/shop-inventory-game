namespace Game.Services.Clicker
{
    public interface IIncomeCalculator
    {
        int GetPassiveIncomePerSecond();   // passive income per second
        float GetClickMultiplier();       // click income
    }
}