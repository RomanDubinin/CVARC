﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;
using CVARC.V2.SimpleMovement;

namespace RepairTheStarship.Robot
{
    public abstract class RTSRobot<TSensorsData> : SimpleMovementRobot<IRTSActorManager,RTSWorld,TSensorsData>, IRTSRobot
        where TSensorsData : new()
    {

        public RTSRobot(string controllerName) : base(controllerName) { }

        public string GrippedObjectId { get; private set; }

        public override void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan)
        {
            nextRequestTimeSpan=0.1;
            if (commandName == "Grip")
            {
                if (GrippedObjectId != null) return;
                GrippedObjectId = Manager.Grip();
                return;
            }
            if (commandName == "Release")
            {
                if (GrippedObjectId == null) return;
                if (Manager.Release())
                {
                    var detailType = World.IdGenerator.GetKey<DetailColor>(GrippedObjectId);
                    World.DetailInstalled(detailType, ControllerId);
                }
                GrippedObjectId = null;
            }
        }
    }
}
