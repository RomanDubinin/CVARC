﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    public class DemoLogicPart : LogicPart
    {
        public DemoLogicPart() : base(
            new DemoWorld(),
            ()=>new SimpleMovementTwoPlayersKeyboardControllerPool(),
            double.PositiveInfinity
            )
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);
        }
    }
}