namespace mastermind_solver;

internal class MinMaxSolver(bool verbose=true, bool allowGuessKnownToNotBeTheSolution=false): Solver {

    public override Combination ComputeNextGuess(List<PlayedCombination> playedCombinations)
    {
        if (playedCombinations.Count == 0)
        {
            // That solver is pretty slow, especially for the first guess. And the first guess is always the same anyway, so let's return it hardcoded instead of wasting CPU cycles.
            return new Combination(Token.BLACK, Token.BLACK, Token.WHITE, Token.WHITE);
        }
        var possibleSolutions = AllCombinations
                .Where(c => c.IsCandidateSolution(playedCombinations))
                .ToArray();
        if (possibleSolutions.Length == 1)
        {
            Console.WriteLine("That's the last possible combination");
            return possibleSolutions[0];
        }

        var candidates = AllCombinations;
        if (allowGuessKnownToNotBeTheSolution)
        {
            candidates = candidates
                .Where(c => c.IsCandidateSolution(playedCombinations))
                .ToArray();
        }

        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        var originalPlayedCombinations = new List<PlayedCombination>(playedCombinations);

        var nextGuess = candidates
            .Select(c => (c, ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(c, possibleSolutions, originalPlayedCombinations)))
            .MinBy(t => t.Item2);

        if (verbose)
        {
            Console.WriteLine($"Still {possibleSolutions.Length} candidates. After this guess we should have at most {nextGuess.Item2} candidates left");
        }

        return nextGuess.Item1;
    }

    // remainingCandidatesSoFar could be deduced from playedCombinations, but since this method is meant to be used in a loop, we take it as a parameter so it does not need to be
    // recomputed at each iteration
    private int ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(Combination consideredCandidate, Combination[] remainingCandidatesSoFar, IEnumerable<PlayedCombination> playedCombinations)
    {
        var possibleResults = new HashSet<CombinationResult>(remainingCandidatesSoFar.Select(consideredCandidate.ComputeResult));

        var worstCase = 0;
        foreach (var result in possibleResults)
        {
            var nextPlayedCombinations = new List<PlayedCombination>(playedCombinations) { new(consideredCandidate, result) };
            var nbRemainingCandidatesNextTurn = remainingCandidatesSoFar.Count(c => c.IsCandidateSolution(nextPlayedCombinations));
            worstCase = Math.Max(worstCase, nbRemainingCandidatesNextTurn);
        }

        return worstCase;
    }
}