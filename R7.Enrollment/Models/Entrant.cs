using System;
using System.Collections.Generic;

namespace R7.Enrollment.Models
{
    public class Entrant : IEntrant
    {
        public string Name { get; set; }

        public string Snils { get; set; }

        public string PersonalNumber { get; set; }

        public int Position { get; set; }

        public int AbsolutePosition { get; set; }

        public int FinalMark { get; set; }

        public int AchievementMark { get; set; }

        public bool OriginalIn { get; set; }

        public bool AcceptedEntrant { get; set; }

        public bool Recommended { get; set; }

        public bool RefusedToBeEnrolled { get; set; }

        public string Status { get; set; }

        public IList<string> MarkStrings { get; set; }

        [Obsolete ("Could be used later on over/with MarkStrings", false)]
        public IList<EntrantMark> Marks { get; set; } = new List<EntrantMark> ();
    }
}
