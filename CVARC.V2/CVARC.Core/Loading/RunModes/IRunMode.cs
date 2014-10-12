﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface IRunMode
    {
        void CheckArguments(Configuration arguments);
        void InitializeCompetitions(Competitions competitions);
        IController GetController(string controllerId);
        Configuration Configuration { get; }
    }

}