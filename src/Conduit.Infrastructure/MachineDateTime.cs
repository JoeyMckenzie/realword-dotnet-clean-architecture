namespace Conduit.Infrastructure
{
    using System;
    using Shared;

    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;

        public int CurrentMonth => DateTime.Now.Month;

        public int CurrentDay => DateTime.Now.Day;
    }
}