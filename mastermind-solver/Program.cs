using mastermind_solver;
using System.Diagnostics;

//Play();
FindAndPrintLongestToFindCombination();



void Play()
{
    var stillPlaying = true;
    var playedCombinations = new List<PlayedCombination>();
    while (stillPlaying)
    {
        var candidate = new Solver().FindCandidate(playedCombinations);
        Console.WriteLine(
            $"Candidate: {candidate} (expected intput: \"nbAtTheCorrectPosition nbGoodColorAtInCorrectPosition\"");
        string? read = null;
        while (read == null || read.Trim() == "")
        {
            read = Console.ReadLine();
        }

        // TODO: add some verification on the type of the input
        var splits = read.Trim().Split(' ');
        var result = new CombinationResult(nbAtGoodPosition: Convert.ToInt32(splits[0]),
            nbGoodColorAtBadPosition: Convert.ToInt32(splits[1]));

        if (result.NbAtGoodPosition == 4)
        {
            stillPlaying = false;
        }

        playedCombinations.Add(new PlayedCombination(candidate, result));

    }

    Console.WriteLine("DONE");
}

// TODO: put the following method in a better location
void FindAndPrintLongestToFindCombination()
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    var hardestTuple = Solver.AllCombinations.Select(c => (c, ComputeNbIterationsToSolve(c))).MaxBy(t => t.Item2);
    stopwatch.Stop();

    Console.WriteLine("Took: " + stopwatch.ElapsedMilliseconds + "ms");
    Console.WriteLine($"Hardest combination to find: {hardestTuple.Item1}. Requires {hardestTuple.Item2} iterations");
}

int ComputeNbIterationsToSolve(Combination solution)
{
    var nbIterations = 0;
    var playedCombinations = new List<PlayedCombination>();

    while (true)
    {
        nbIterations++;
        var candidate = new Solver(false).FindCandidate(playedCombinations);
        var result = candidate.ComputeResult(solution);
        if (result.NbAtGoodPosition == 4)
        {
            return nbIterations;
        }
        playedCombinations.Add(new PlayedCombination(candidate, result));
    }
}
