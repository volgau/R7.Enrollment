using System;
using System.Linq;
using System.Xml.Linq;
using R7.Enrollment.Models;

namespace R7.Enrollment.Data
{
    public class TandemXmlModelFactory
    {
        public static Competition CreateCompetition (XElement xelem)
        {
            var competition = new Competition {
                EduLevel = xelem.Attribute ("eduLevel")?.Value,
                OrgTitle = xelem.Attribute ("enrOrgUnit")?.Value,
                OrgUnitTitle = xelem.Attribute ("formativeOrgUnitTitle")?.Value,
                CompetitionType = xelem.Attribute ("competitionType")?.Value,
                CompensationType = xelem.Attribute ("compensationTypeShortTitle")?.Value,
                CompensationTypeBudget = TryParseBool (xelem.Attribute ("compensationTypeBudget")?.Value) ?? false,
                Plan = TryParseInt (xelem.Attribute ("plan")?.Value) ?? 0,
                EduLevelRequirement = xelem.Attribute ("eduLevelRequirement")?.Value
            };

            competition.EduProgram.Subject = xelem.Attribute ("eduProgramSubject")?.Value;
            competition.EduProgram.Title = xelem.Attribute ("programSetPrintTitle")?.Value;

            return competition;
        }

        public static Entrant CreateCompetitionEntrant (XElement xelem)
        {
            return new Entrant {
                PersonalNumber = xelem.Descendants ("entrantPersonalNumber").FirstOrDefault ()?.Value,
                Name = xelem.Attribute ("fio")?.Value,
                Snils = xelem.Attribute ("snils")?.Value,
                Position = TryParseInt (xelem.Attribute ("position")?.Value) ?? 0,
                FinalMark = TryParseInt (xelem.Attribute ("finalMark")?.Value) ?? 0,
                AchievementMark = TryParseInt (xelem.Attribute ("achievementMark")?.Value) ?? 0,
                OriginalIn = TryParseBool (xelem.Attribute ("originalIn")?.Value) ?? false,
                AcceptedEntrant = TryParseBool (xelem.Attribute ("acceptedEntrant")?.Value) ?? false,
                RefusedToBeEnrolled = TryParseBool (xelem.Attribute ("refusedToBeEnrolled")?.Value) ?? false,
                Recommended = TryParseBool (xelem.Attribute ("recommended")?.Value) ?? false,
                Status = xelem.Attribute ("status")?.Value
            };
        }

        public static EntranceDiscipline CreateEntranceDiscipline (XElement xelem)
        {
            return new EntranceDiscipline {
                Title = xelem.Attribute ("title")?.Value,
                ShortTitle = xelem.Attribute ("shortTitle")?.Value
            };
        }

        public static EntrantMark CreateEntrantMark (XElement xelem, Competition competition)
        {
            var markTitle = xelem.Attribute ("markTitle")?.Value;
            return new EntrantMark {
                Mark = (TryParseInt (xelem.Attribute ("mark")?.Value) ?? 0) / 1000,
                EntranceDiscipline = competition.EntranceDisciplines
                    .FirstOrDefault (ed => ed.Title.IndexOf (markTitle, StringComparison.CurrentCultureIgnoreCase) >= 0)
            };
        }

        public static void FillEduProgram (XElement xelem, EduProgram eduProgram)
        {
            eduProgram.Form = xelem.Attribute ("eduProgramForm")?.Value;
            eduProgram.Duration = xelem.Attribute ("duration")?.Value;
            eduProgram.FullTitle = xelem.Attribute ("fullTitleWithoutSubjectIndex")?.Value;
            eduProgram.Specialization = xelem.Attribute ("programSpec")?.Value;
            eduProgram.ConditionsWithForm = xelem.Attribute ("conditionsWithForm")?.Value;
            eduProgram.TitleAndConditionsShortWithForm = xelem.Attribute ("titleAndConditionsShortWithForm")?.Value;
        }

        static int? TryParseInt (string value)
         {
             return int.TryParse (value, out int result) ? (int?) result : null;
         }

        static bool? TryParseBool (string value)
        {
            return bool.TryParse (value, out bool result) ? (bool?) result : null;
        }
    }
}
