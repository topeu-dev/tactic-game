namespace Turn
{
    public interface TurnRelated
    {
        void EndOfTurn();

        void StartOfTurn();

        Initiative GetInitiative();
    }
}