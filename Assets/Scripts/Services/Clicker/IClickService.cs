namespace Game.Services.Clicker
{
    /// Simple interface for click service which performs the click action
    public interface IClickService
    {
        /// Perform a click. Returns the gained amount (0 or positive).
        int Click();
    }
}
