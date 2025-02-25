namespace mastermind_solver;

internal class Solver
{
    public Combination FindCandidate(IEnumerable<PlayedCombination> playedCombinations)
    {
        // It would be cheaper to stop at the first candidate, but I'm interested in the number of candidate left, and it's still pretty cheap anyway
        var candidates = EnumeratesAllCombinations().Where(c => c.IsCandidateSolution(playedCombinations)).ToArray();
        Console.WriteLine($"Still {candidates.Length} candidates");
        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        return candidates[0];
    }

    private IEnumerable<Combination> EnumeratesAllCombinations()
    {
        for (var token1=Token.BLACK ; token1 < Token.BLUE ; token1++)
        for (var token2=Token.BLACK ; token2 < Token.BLUE ; token2++)
        for (var token3=Token.BLACK ; token3 < Token.BLUE ; token3++)
        for (var token4 = Token.BLACK; token4 < Token.BLUE; token4++)
            yield return new Combination(token1, token2, token3, token4);
    }

}

public enum Token
{
    BLACK=0,
    WHITE,
    RED,
    YELLOW,
    GREEN,
    BLUE,
}

