using Common;
using System;

namespace Infrastructure
{
    public class UniversalDateTime : IDateTime
    {
        private readonly static DateTime MinimumDate = DateTime.Parse("1/1/1800 12:00:00 AM");

        public DateTime Now => DateTime.UtcNow;

        public DateTime MinDate => MinimumDate;
    }
}
