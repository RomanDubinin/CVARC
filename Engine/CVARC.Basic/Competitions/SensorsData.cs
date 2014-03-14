﻿using System.Runtime.Serialization;
using CVARC.Basic.Sensors;

namespace CVARC.Basic
{
    [DataContract]
    public class SensorsData
    {
        [DataMember]
        public MapSensorData MapSensor { get; set; }

        [DataMember]
        public RobotIdSensorData RobotIdSensor { get; set; }

        [DataMember]
        public ManyPositionData LightHouseSensor { get; set; }
    }
}
