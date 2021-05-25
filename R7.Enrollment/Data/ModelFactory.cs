using System.Linq;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Xml.Schema;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class ModelFactory
    {
        public static CompetitionEntrant CreateCompetitionEntrant (XElement xelem)
        {
            return new CompetitionEntrant {
                PersonalNumber = xelem.Descendants ("entrantPersonalNumber").FirstOrDefault ()?.Value,
                Name = xelem.Attribute ("fio")?.Value,
                Position = TryParseInt (xelem.Attribute ("position")?.Value) ?? 0,
                FinalMark = TryParseInt (xelem.Attribute ("finalMark")?.Value) ?? 0
            };
        }
        
        public static Competition CreateCompetition (XElement xelem)
        {
            return new Competition {
                EduProgramForm = xelem.Attribute ("eduProgramForm")?.Value,
                EduLevel = xelem.Attribute ("eduLevel")?.Value,
                EduProgramTitle = xelem.Attribute ("programSetPrintTitle")?.Value,
                OrgUnitTitle = xelem.Attribute ("formativeOrgUnitTitle")?.Value,
                CompetitionType = xelem.Attribute ("competitionType")?.Value,
                CompensationType = xelem.Attribute ("compensationTypeShortTitle")?.Value
            };
        }

        static int? TryParseInt (string value)
        {
            return int.TryParse (value, out int result) ? (int?) result : null;
        }
    }
}