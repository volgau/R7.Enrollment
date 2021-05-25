using System;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Tests
{
    public class Program
    {
        static void Main (string[] args)
        {
            Dump ();
        }

        static void Dump ()
        {
            var xml = XDocument.Load ("./data/sample.xml");
            var competitionElem = xml.Root.Element ("competition");
            foreach (var competitionRow in competitionElem.Elements ("row")) {
                var competition = Competition.FromXElement (competitionRow);
                Console.WriteLine (competition.EduProgramTitle);
                foreach (var entrantRow in competitionRow.Element ("entrant").Elements ("row")) {
                    var entrant = CompetitionEntrant.FromXElement (entrantRow);
                    Console.WriteLine ($"{entrant.Position} {entrant.Name} {entrant.FinalMark}");
                }
            }
        }
    }
}
