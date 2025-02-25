namespace mastermind_solver;

public class CombinationResult(int nbAtGoodPosition, int nbGoodColorAtBadPosition)
{
    public readonly int NbAtGoodPosition = nbAtGoodPosition;
    public readonly int NbGoodColorAtBadPosition = nbGoodColorAtBadPosition;

    public override bool Equals(object? obj) => this.Equals(obj as CombinationResult);

    public override int GetHashCode()
    {
        return NbAtGoodPosition * 5 + NbGoodColorAtBadPosition;
    }

    public override string ToString()
    {
        return $"({NbAtGoodPosition}, {NbGoodColorAtBadPosition})";
    }

    public bool Equals(CombinationResult? other) =>
        other != null && NbAtGoodPosition == other.NbAtGoodPosition && NbGoodColorAtBadPosition == other.NbGoodColorAtBadPosition;
}