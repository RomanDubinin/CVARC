﻿using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
     public class PositionLogItem
    {
        public double Time { get; set; }
        public Frame3D Location { get; set; }
        public bool Exists { get; set; }
    }

    [Serializable]
    public class Log
    {
        public readonly Dictionary<string, List<PositionLogItem>> Positions = new Dictionary<string, List<PositionLogItem>>();
       
    }

    public class Logger
    {
        public double LoggingDeltaTime = 0.1;
        public string LogFileName { get; set; }
        public bool SaveLog { get; set; }
        public Log Log { get; private set; }
        IWorld world;
        public Logger(IWorld world)
        {
            this.world = world;
            Log=new V2.Log();
            world.Clocks.AddRenewableTrigger(0, UpdatePositions);
            world.Exit += world_Exit;
        }

        void world_Exit()
        {
            if (!SaveLog) return;
            string filename=null;
            for (int i = 0; ; i++)
            {
                filename = "log" + i + ".cvarclog";
                if (!File.Exists(filename))
                    break;
            }
            using (var stream= File.Open(filename,FileMode.Create,FileAccess.Write))
            {
                new BinaryFormatter().Serialize(stream, Log);
            }
        }

        void UpdatePositions(RenewableTriggerData data, out double nextCall)
        {
            foreach (var e in world.IdGenerator.GetAllId())
            {
                if (!Log.Positions.ContainsKey(e))
                    Log.Positions[e] = new List<PositionLogItem>();
                var item = new PositionLogItem { Time = data.ThisCallTime };
                if (!world.Engine.ContainBody(e))
                    item.Exists = false;
                else
                {
                    item.Exists = true;
                    item.Location = world.Engine.GetAbsoluteLocation(e);
                }
                Log.Positions[e].Add(item);
            }
            nextCall = data.ThisCallTime + LoggingDeltaTime;
        }


    }
}