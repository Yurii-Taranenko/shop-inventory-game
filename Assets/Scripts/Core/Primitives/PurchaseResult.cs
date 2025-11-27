namespace Game.Core.Primitives
{
    /// <summary>
    /// Result codes returned by shop purchase attempts.
    /// </summary>
    public enum PurchaseResult
    {
        Success,
        NotEnoughMoney,
        AlreadyOwned,
        Error
    }
}
