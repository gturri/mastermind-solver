namespace mastermind_solver;

internal abstract class Solver
{
    private static readonly Lazy<Combination[]> LazyAllCombinations = new(ComputeAllCombinations);
    public static Combination[] AllCombinations => LazyAllCombinations.Value;

    private static Combination[] ComputeAllCombinations()
    {
        var result = new List<Combination> { Capacity = 6*6*6*6 };
        for (var token1=Token.BLACK; token1 <= Token.BLUE; token1++)
        for (var token2=Token.BLACK; token2 <= Token.BLUE; token2++)
        for (var token3=Token.BLACK; token3 <= Token.BLUE; token3++)
        for (var token4=Token.BLACK; token4 <= Token.BLUE; token4++)
            result.Add(new Combination(token1, token2, token3, token4));
        return result.ToArray();
    }

    public abstract Combination ComputeNextGuess(IEnumerable<PlayedCombination> playedCombinations);

}