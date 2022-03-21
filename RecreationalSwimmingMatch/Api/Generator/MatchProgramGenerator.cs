using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Api.Generator;

public class MatchProgramGenerator
{
    private readonly Match _match;
    private readonly List<Registrations> _registrations;

    public MatchProgramGenerator(Match match, List<Registrations> registrations)
    {
        _match = match;
        _registrations = registrations;
    }

    public GeneratedMatchProgram Generate()
    {
        var generatedMatchProgram = new GeneratedMatchProgram()
        {
            Id = Guid.NewGuid().ToString(),
            MatchId = _match.Id,
            GenerateDateTime = DateTime.Now,
            GeneratedPrograms = new List<GeneratedProgram>()
        };

        foreach (var program in _match.Programs)
        {
            var generatedProgram = new GeneratedProgram()
            {
                ProgramId = program.Id,
                NrOfSwimmersPerSeries = new Dictionary<int, int>(),
                SwimmersOrdered = _registrations
                    .Where(r => r.RegistrationList?.Any() == true)
                    .SelectMany(r => r.RegistrationList)
                    .Where(r => r.ProgramIds.Contains(program.Id))
                    .OrderBy(r => r.DateOfBirth)
                    .Select(r => r.Id)
                    .ToList()
            };

            var nrOfSeries = (int)Math.Ceiling((decimal)generatedProgram.SwimmersOrdered.Count / _match.SwimmingLanes);
            for (var i = 1; i <= nrOfSeries; i++)
            {
                var swimmersInLane = i == 1
                    ? generatedProgram.SwimmersOrdered.Count % _match.SwimmingLanes
                    : _match.SwimmingLanes;

                generatedProgram.NrOfSwimmersPerSeries[i] = swimmersInLane;
            }

            generatedMatchProgram.GeneratedPrograms.Add(generatedProgram);
        }

        return generatedMatchProgram;
    }
}