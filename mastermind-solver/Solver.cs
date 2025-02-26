namespace mastermind_solver;

internal class Solver(bool verbose = true)
{
    public Combination FindCandidate(IEnumerable<PlayedCombination> playedCombinations)
    {
        // It would be cheaper to stop at the first candidate, but I'm interested in the number of candidate left, and it's still pretty cheap anyway
        var candidates = EnumeratesAllCombinations()
            .Where(c => c.IsCandidateSolution(playedCombinations))
            .ToArray();
        if (verbose)
        {
            Console.WriteLine($"Still {candidates.Length} candidates");
        }

        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        return candidates[0];
    }

    public Combination FindNextMinMaxCandidate(IEnumerable<PlayedCombination> playedCombinations)
    {
        var candidates = EnumeratesAllCombinations()
            .Where(c => c.IsCandidateSolution(playedCombinations))
            .ToArray();

        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        var originalPlayedCombinations = playedCombinations.ToArray();

        var nextGuess = candidates
            .Select(c => (c, ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(c, candidates, originalPlayedCombinations)))
            .MinBy(t => t.Item2);

        if (verbose)
        {
            Console.WriteLine($"Still {candidates.Length} candidates. After this guess we should have at most {nextGuess.Item2} candidates left");
        }

        return nextGuess.Item1;
    }

    private int ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(Combination consideredCandidate, Combination[] remainingCandidatesSoFar, PlayedCombination[] playedCombinations)
    {
        var worstCase = 0;
        foreach (var solution in remainingCandidatesSoFar)
        {
            var nextPlayedCombination = new List<PlayedCombination>(playedCombinations) { new(consideredCandidate, consideredCandidate.ComputeResult(solution)) };
            var nbRemainingCandidatesNextTurn = EnumeratesAllCombinations().Count(c => c.IsCandidateSolution(nextPlayedCombination));
            worstCase = Math.Max(worstCase, nbRemainingCandidatesNextTurn);
        }

        return worstCase;
    }

    public static IEnumerable<Combination> EnumeratesAllCombinations()
    {
        for (var token1=Token.BLACK; token1 <= Token.BLUE; token1++)
        for (var token2=Token.BLACK; token2 <= Token.BLUE; token2++)
        for (var token3=Token.BLACK; token3 <= Token.BLUE; token3++)
        for (var token4=Token.BLACK; token4 <= Token.BLUE; token4++)
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

