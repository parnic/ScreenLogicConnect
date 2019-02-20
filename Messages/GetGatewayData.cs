﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScreenLogicConnect.Messages
{
    public class GetGatewayData : HLMessage
    {
        public string GatewayName;
        public bool GatewayFound;
        public bool LicenseOK;
        public string IPAddr;
        public short Port;
        public bool PortOpen;
        public bool RelayOn;

        public const short HLM_GETGATEWAYDATA = 18003;

        public static GetGatewayData QUERY(string systemName, short senderID = 0)
        {
            var ret = new GetGatewayData(senderID, HLM_GETGATEWAYDATA);
            ret.GatewayName = systemName;
            return ret;
        }

        public GetGatewayData(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public GetGatewayData(HLMessage msg)
            : base(msg)
        {
        }

        public override byte[] asByteArray()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WritePrefixLength(GatewayName);
                    bw.WritePrefixLength(GatewayName);
                }

                data = ms.ToArray();
            }

            return base.asByteArray();
        }

        protected override void decode()
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    GatewayFound = br.ReadBoolean();
                    LicenseOK = br.ReadBoolean();
                    IPAddr = HLMessageTypeHelper.extractString(br);
                    Port = br.ReadInt16();
                    PortOpen = br.ReadBoolean();
                    RelayOn = br.ReadBoolean();
                }
            }
        }
    }
}
