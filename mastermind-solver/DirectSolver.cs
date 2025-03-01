namespace mastermind_solver;

/**
 * Fastest solver to compute the next guess, but it may require up to 9 guesses for some combinations (like "GREEN, BLUE, GREEN, YELLOW", but for 5 others as well).
 * Setting "useMinMaxFirstGuess" uses the same guess as the min-max solver for the first guess, hence leading to some hybrid solver. If this is set to "true" then in
 * the worst case if finds the solution in 8 guesses
 */
internal class DirectSolver(bool verbose=true, bool useMinMaxFirstGuess=false) : Solver
{
    public override Combination ComputeNextGuess(List<PlayedCombination> playedCombinations)
    {
        if (useMinMaxFirstGuess && playedCombinations.Count == 0)
        {
            return new Combination(Token.BLACK, Token.BLACK, Token.WHITE, Token.WHITE);
        }

        // It would be cheaper to stop at the first candidate, but I'm interested in the number of candidate left, and it's still pretty cheap anyway
        var candidates = AllCombinations
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
}