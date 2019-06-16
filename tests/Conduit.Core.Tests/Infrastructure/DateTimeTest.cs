namespace Conduit.Core.Tests.Infrastructure
{
    using System;
    using Shared;

    public class DateTimeTest : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;

        public int CurrentMonth => DateTime.Now.Month;

        public int CurrentDay => DateTime.Now.Day;
    }
}