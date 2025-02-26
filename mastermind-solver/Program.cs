using mastermind_solver;

Play();



void Play()
{
    var stillPlaying = true;
    var solver = new Solver();
    var playedCombinations = new List<PlayedCombination>();
    while (stillPlaying)
    {
        var candidate = solver.FindCandidate(playedCombinations);
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
