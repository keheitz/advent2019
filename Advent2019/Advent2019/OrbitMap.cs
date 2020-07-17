﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Advent2019
{
    internal class OrbitMap
    {
        private List<string> map_entries = new List<string>();
        private Dictionary<string, Orbit> orbit_dictionary = new Dictionary<string, Orbit>();
        internal void ParseOrbits()
        {
            setup_orbits();
            setup_indirect_orbits();
        }
        internal int GetTotalNumberOfOrbits()
        {
            int total_orbits = 0;
            foreach (var orbit in orbit_dictionary)
            {
                total_orbits += orbit.Value.GetTotalOrbitedByCount();
            }
            return total_orbits;
        }

        private void setup_indirect_orbits()
        {
            foreach (var orbit in orbit_dictionary)
            {
                List<string> indirect_orbits = findIndirectOrbits(orbit.Value);
                foreach (string indirect in indirect_orbits)
                {
                    if (indirect != orbit.Value.GetParent())
                    {
                        orbit.Value.AddOrbitedValue(indirect, false);
                    }
                }
            }
        }

        private List<string> findIndirectOrbits(Orbit value, List<string> current_list = null)
        {
            if (current_list == null)
            {
                current_list = new List<string>();
            }
            Orbit parent_orbit = null;
            string parent = value.GetParent();
            if (parent != null ) { parent_orbit = orbit_dictionary[parent]; }
            while (parent_orbit != null)
            {
                current_list.Add(parent_orbit.OrbitName);
                if(parent_orbit.OrbitName != "COM")
                {
                    findIndirectOrbits(parent_orbit, current_list);
                }
                break;
            }
            return current_list;
        }

        internal int GetOrbitalTransfersNeeded(string start, string end)
        {
            Orbit start_orbit = orbit_dictionary[start];
            Orbit end_orbit = orbit_dictionary[end];
            List<string> common_orbits = new List<string>();
            common_orbits.AddRange(start_orbit.GetAllOrbits().Intersect(end_orbit.GetAllOrbits()));
            Dictionary<string, int> common_dictionary = common_orbits.ToDictionary(x => x, x => 0);
            foreach (var common in common_orbits)
            {
                Orbit intersection = orbit_dictionary[common];
                int distance_from_start = findTransferCount(start_orbit, intersection);
                common_dictionary[common] += distance_from_start;
                int distance_to_end = findTransferCount(end_orbit, intersection);
                common_dictionary[common] += distance_to_end;
            }
            return common_dictionary.Values.Min();
        }

        private int findTransferCount(Orbit start_orbit, Orbit end_orbit, int? distance = null)
        {
            int count = distance ?? 0;
            string parent = start_orbit.GetParent();
            Orbit parent_orbit = null;
            if (parent != null)
            {
                parent_orbit = orbit_dictionary[parent];
                if (parent_orbit.OrbitName == end_orbit.OrbitName)
                {
                    return count;
                }
            }
            else
            {
                return count;
            }
            count++;
            return findTransferCount(parent_orbit, end_orbit, count);
        }

        private void setup_orbits()
        {
            foreach (string entry in map_entries)
            {
                var parsed_entry = entry.Split(")");
                string parent = parsed_entry[0];
                string child = parsed_entry[1];
                addOrbits(parent, child);
            }
        }


        private void addOrbits(string parent, string child)
        {
            if (orbit_dictionary.ContainsKey(child))
            {
                orbit_dictionary[child].AddOrbitedValue(parent, true);
            }
            else
            {
                Orbit orbit = new Orbit(child);
                orbit.AddOrbitedValue(parent, true);
                orbit_dictionary.Add(child, orbit);
            }
            if (!orbit_dictionary.ContainsKey(parent))
            {
                Orbit orbit = new Orbit(parent);
                orbit_dictionary.Add(parent, orbit);
            }
        }

        public OrbitMap()
        {
            //string input = "COM)B,B)C,C)D,D)E,E)F,B)G,G)H,D)I,E)J,J)K,K)L,K)YOU,I)SAN";
            string input = "HX5)C21,VZD)VW7,BTP)JHL,699)GSD,2FB)ZM1,QS3)7XJ,59Z)N3Z,HS2)BKY,7SN)YLN,NJV)YC5,YJP)D32,5C8)KZ2,3D2)FWP,LN3)JHZ,2CQ)4Q6,NLJ)KWV,7MZ)PDW,Q2N)LP8,2VJ)BRY,V6D)CNL,BBT)7QW,Y9S)T8J,F3Y)1X3,G6V)1MG,Q1C)ZL3,8YV)F2H,F8B)DRS,D3H)R95,GG5)WRZ,KZ2)T52,3Z7)X96,9JC)B98,VVK)96T,1V9)52M,F56)VKQ,TV7)HQG,TYL)NH7,WQM)C3P,ZQK)HCT,LR4)P5H,PKR)D9J,MC9)4PC,WKR)PHX,8CS)B5P,RBX)TV7,CQT)J48,XRB)CFS,K32)57Y,97B)Y3Q,7XJ)41B,59S)PDL,N32)CW1,Y8J)8NY,R9B)S4Y,13L)61J,3MC)WK9,NB9)XPY,R8V)LHB,1S5)2SL,8K2)LST,2NW)6H1,B27)Q2B,14L)W35,41H)Z5M,Y8G)8VG,X5S)CFP,FRK)6PM,HTR)QRC,BJ2)5Y3,CS3)R9W,ZL3)GTG,FKX)NF5,HC6)6FG,QG7)85C,Y5Y)1VG,FLJ)DJH,28P)K4Z,S2N)J1C,GY6)GW9,CMW)QC7,L7F)VJ2,YG1)FKX,GQJ)GY7,WC8)KTZ,6JS)M79,L7J)TQD,YVQ)ZYV,R6X)1HB,GTH)TM5,JJJ)GJS,CKH)NQK,2NG)ZVK,564)3RC,VJ2)L7Z,VW7)7SD,6F7)SAN,YVH)6JN,T8C)CKH,K8Y)F6G,953)HQ9,FBW)HWJ,SGS)65F,GXR)6SL,M6K)K3X,TJH)QKG,DN7)YS4,2B3)B2P,ZQ1)WFM,YF1)KS3,NNS)NDD,VTS)PF1,9T4)H7Q,GJS)8VP,D5Q)NR3,LHB)3RK,H32)5DQ,3GZ)1WV,H48)8CN,H5S)WZH,SBH)LFH,BCX)6M8,T46)9P6,Q1X)4L9,72F)ZC2,PCB)SC1,YQ6)S3M,Q2B)PMF,V4S)YVQ,NFK)5C8,43S)9CQ,CNC)YPF,HVS)41H,FBW)4GX,NHF)45D,QRB)CZQ,L79)7MY,W47)4XS,GMS)8BJ,6K1)5NX,K53)JY1,583)GZJ,XHD)MRD,NWM)DFD,G46)JX6,8BJ)1FW,1MJ)692,HTQ)F9B,6PZ)92W,1Q1)8MN,9HS)772,BN2)29B,9Q7)VJ3,C5D)C4R,JS6)9F9,PF1)J3T,ZP4)BCX,3X3)VKL,V6L)SYJ,3VP)Z3Q,P1L)HXV,1DS)8YV,9P6)C16,RMW)Y34,W1D)72Q,FC7)B3Z,BD8)GMH,38M)PCB,6DT)LN3,KBY)9W8,3JS)R85,C46)RBX,NHF)9K5,DHR)STK,SSP)Z6F,WYW)H7R,NDX)NX9,M19)TH9,P3R)RTW,BTS)R8V,5GC)DSN,D9J)3LM,KCB)LWT,TTQ)FH4,72Q)FHY,Z6F)F3Y,XBB)VFV,QW5)VVK,YNC)26J,5LK)2LR,H2V)GCW,BC5)GY6,1P3)D8V,TL2)465,65F)S9V,B5P)K32,27Z)YVH,VQ2)G6N,Z2W)LHX,D8K)JN4,1FK)RK1,HQP)2N8,COM)Q1H,CHK)VZD,Q6V)Q1X,1NX)C5D,CC6)J37,L3G)KKP,KST)QG7,6FG)T8C,YC5)56M,43X)HMM,D45)2RD,TH9)7FY,SM5)S81,9MC)MVR,J48)JH9,VQT)1D2,2TV)SP7,Q9N)658,J1V)26X,8L5)XWZ,DC2)R89,B3Z)7WL,DJH)CHK,LST)X22,F9B)L4V,RGX)K85,68T)VG7,YM3)SJN,JSF)97X,B98)RZZ,959)PKP,6M8)2VZ,MT3)CMM,K2Q)QYF,RTK)HPL,RWY)2PT,7QW)LGX,LXZ)X8T,T43)KV9,XSM)2ND,X2B)SDQ,HZC)KZG,2RS)C6L,N3Z)BR9,2B3)P3D,ZST)GL1,QQ8)TYL,7TD)BT1,2VZ)8YW,KS3)V4T,Q1H)MJY,S9V)ZMG,MRK)9V8,JVS)LL3,8VG)BJ2,SCM)72S,GP9)RZB,KS5)DN7,Y34)Y3W,LX1)8VF,T6B)D8K,RPB)564,41B)364,Q7Q)1FT,TBG)479,XBX)XP9,JLZ)F2F,XHK)SZQ,P1P)ZDH,583)Z7L,CMM)DK2,KP1)CFW,69X)2VJ,SYJ)BQQ,147)2XS,GQJ)FLJ,ZCK)Q2N,NZS)1BD,3RC)3Z7,TCC)BLQ,FH4)SCM,VM6)FQD,G8V)ZXS,8WX)2CQ,HV4)J3F,72L)MF6,W66)953,C3R)1KK,4JC)YQQ,Q7S)ZCT,BJY)R9B,2SL)XC1,XRM)J1V,SK3)NP7,2TV)G1V,HX9)J7C,J37)6LZ,GMH)NPR,5T6)YRR,7RB)89P,T9L)Q7Q,4GX)2DP,6LZ)79K,NHQ)YQ6,Y3W)C3R,6SL)K9L,C46)P1P,8NY)8RM,966)HXQ,B6S)C46,T39)DDV,XV7)2Q3,YJB)ZX2,QC7)3NF,Y42)N28,B1J)1SG,PHZ)38M,48G)GVK,ZLY)X5H,YKK)NB9,WK9)4GG,R5G)BTP,J4C)QS3,45D)LGP,658)BN2,B91)Z6D,XPY)YF1,FQT)MN9,4ZQ)PJG,W86)K8Y,8VF)B91,P3D)J31,MTT)HH7,GVK)RV9,GY7)6R6,72S)PB4,XP9)FBW,5NR)72L,YM9)BBT,C4G)Z5C,987)G95,CDG)YYB,SZQ)J39,BZY)YZC,SKT)6PZ,ZVK)5VD,BHG)TBG,X1X)9JC,MST)959,RYV)MQQ,GL1)V26,FQD)JLZ,N5S)GWD,B2H)L3G,QYF)3JP,WWB)B27,PHX)VM6,MSZ)CMW,HH7)MKP,9XZ)VNR,3NF)H48,YQM)CVF,B98)H2V,K91)2NZ,8MN)PF7,RZB)C1F,YRR)3PR,2VR)9P2,4V5)VVC,2DP)M6K,KWV)8FW,YPF)28L,5LG)SSP,3JP)TSR,JW9)7WZ,M71)P5J,Y34)MRS,GSD)XSM,Q59)59S,71Y)958,QZC)FDZ,QRC)RYV,RDF)CS3,3T3)LYZ,5NX)PL2,5L1)JJ6,F1X)9T4,RV9)MJJ,ZHD)7JQ,CZ9)3D2,92W)C34,VHH)QQ8,364)RRK,J3G)TZ5,8VP)TZG,3PR)VQ2,ZM1)ZCK,D7F)43S,TSR)DKK,F7C)BV2,DFD)92T,84M)HMX,M34)RN9,49F)KDG,P9T)966,6RG)TL2,QCF)K6Q,KKP)B7H,JQF)XHK,2XS)W1L,XJR)XRB,4GC)KB5,BX8)G6V,VKL)ZLY,RBX)KZJ,3RK)489,V26)QRB,2Q3)7TW,GNF)WPF,YJT)7K6,PL2)71Y,2ND)2NG,HWJ)V9X,VF5)VQT,2WD)WD9,MF6)GKK,787)J4C,G8Q)358,HVH)FNN,XQG)Z11,JG9)PBB,Y5Q)GXR,R19)N99,P5R)DLL,246)SFG,1SP)Y5Z,16N)NFK,2NZ)HW8,B7M)LVQ,GZ8)F7C,L39)WK8,2LR)YWY,BNT)3Q8,T46)98H,VQT)5T6,JX6)T43,9FZ)P9T,QRL)Q1C,J3T)7H1,PJG)2RS,YMG)1NX,YYB)XP3,5LH)TY6,NX9)VHH,XBR)PF5,89P)TQY,254)L7F,J6L)4LK,FLW)QCF,8FW)84M,P7C)1V9,LFL)3ZN,7TW)JNM,RZZ)GCT,D33)4L2,KK9)V63,K3X)GG5,B3F)FVX,XW6)GMC,4L9)ZRN,6PM)BNT,V54)XJR,8ZK)MV7,XW4)NJV,QYM)SPV,BPB)G46,BQQ)DDN,H7Q)4WC,B58)Y9S,9P2)SQX,729)C4G,ZFX)QLH,Z5C)3B7,M79)73S,RN9)W9V,F2X)H5S,2HW)9FZ,669)7PQ,SPN)SKT,QCV)MQ8,Z5T)ZQK,4HJ)3B6,WZH)246,1KK)2R9,WD9)JFP,YWY)8NZ,CFW)R5K,Y42)3KH,LF8)8JX,4WC)5T5,X8K)W66,T3V)1DS,ZMG)NYX,VVD)KBY,N4B)49J,TQ1)7RL,WYW)3XW,P4G)V6D,2N8)GTH,C16)YC6,ZM1)TQX,PBY)Y1Y,X8T)BD8,NQK)NLJ,3KH)PBY,4LF)MT3,K85)69W,SYD)72J,CVK)HTR,55S)M4C,R95)XV7,YLC)WC8,FRK)D7F,DRS)ZKJ,7H1)F7Y,F7Y)HQS,HD2)CWS,GZ8)P7C,Q8B)HYP,QCV)KXK,YLN)MS4,RYV)HTQ,V4T)CBX,VKQ)T6Y,26X)YNC,2KX)VQQ,RX6)KRR,P5J)CD1,K46)2NW,CWS)61S,6QX)KFZ,S4Y)NKG,GW9)Y4L,1W7)94J,XQG)L39,S3Z)365,Y3Q)GZ8,XWZ)X8K,RRR)28P,Y4L)7C2,26J)SM5,96T)Z9K,QKG)N4B,97X)XRM,592)B58,YS6)L7J,ZS4)DZM,Z7L)F3Q,1D2)5TM,LKM)P98,K6Q)ZHD,SSQ)HC6,ZSH)MRK,MQ8)BJK,JHL)CVB,HYP)YLC,1SG)2VR,ZC2)ZQ1,JPZ)FRK,P55)GX2,Z6D)3JS,8RM)C3J,35P)JPZ,PKP)JG9,9BV)5NR,7PL)CQT,7MY)QW5,TQX)XVY,LR4)HQP,29B)PN8,7YM)P5R,QYF)7Y7,W1L)6RG,7SC)NZS,2RD)YT6,BPZ)Y42,3HR)5FP,27Z)S3Z,4LK)B2H,C5T)RX6,LF8)5CF,RRK)LK1,YQQ)R5G,BLQ)J5Q,KFZ)6B3,Q3P)KVC,HXV)BTS,NYX)HK1,J5Q)T6B,YC6)H3Z,NP7)3T3,KR4)T5R,SJN)JGB,PN8)X2B,Y7T)5LK,G5C)8G2,JN4)611,NR3)CVK,N99)WC3,SPV)QRL,ZYV)K91,ZXS)ZSH,V6D)LR4,Z5M)YM9,VPT)JFZ,FDZ)5MR,2VR)WFF,BRK)NWM,8YW)D33,1VG)RTL,7ZZ)5LS,4GC)7RB,FDC)96K,GJP)VDS,DKK)K53,Q1Z)2VC,PDL)RGX,9V8)XBB,7Y7)MPF,C21)XHD,LL3)CRZ,7FV)YPC,T3D)GM1,G7H)LXZ,TSW)1FK,R5K)JKZ,8P7)TQ1,KV9)V54,TM4)G8V,K3X)SFH,1TX)C8B,L6R)T9L,CFR)NCP,YKK)LX1,7K6)P55,R85)9ZJ,NGV)NHQ,F2F)HX9,HMX)3SB,HQG)JL8,V18)PGR,YZC)HV4,VCP)2F8,4LY)FZ5,F7Z)CZ9,M63)XW3,9W8)7YM,QSX)D3H,LVQ)B1J,GX2)DC2,6QC)T71,LGX)D9T,S81)9K1,C1F)M34,JBW)W9M,PZK)973,1LW)WZX,VLH)QCV,358)KMC,XP3)SYD,S7X)1LW,61J)1P3,H7R)TJH,4C3)8K2,1BD)GJP,3B7)GQJ,CRZ)X4L,7PQ)N57,DDN)LFL,28L)9RZ,LWT)J6L,692)LF8,BT1)4YS,C3J)TCC,Z3Q)G7H,MRD)YJP,D32)SK3,HPL)2HW,P5H)R19,3JP)V4S,GM1)H9L,Q7Q)699,ZRF)KGW,K9L)2WD,Q5Y)Q8B,GY3)FQT,4RN)D5Q,SC1)ZP4,W35)2FB,5D9)8L5,59C)592,ZDH)4Q7,RTL)2KX,GCT)GMS,HMM)TM4,RGP)C55,YZW)1W7,6H1)H32,SK3)XJ1,J4T)59Z,Y1Y)D4L,F6G)YSC,YVP)3X3,QTF)13L,CVF)G8Q,96K)HZC,G95)NHF,BV2)9BY,5LQ)XBX,SFH)V18,3RC)MSZ,WFF)YOU,CNL)HK8,ZX2)787,NKX)YKK,C3P)WB3,HQ9)WQM,RK1)T3V,9BY)MST,489)YZW,LP8)JQF,71Q)CTP,S3M)P3R,N57)FDC,TQY)9J6,3SB)729,3ZY)B3F,HGL)97B,BQQ)4GC,KLD)F7Z,BQB)N1Q,WD9)XW6,HQG)P1L,VC1)KS5,P67)BC5,3B6)RPB,X4L)XB6,NKG)5GC,GCW)HSC,4GG)MSS,2F8)HD2,611)5DS,W9M)PHZ,6SL)72F,8CN)C4F,Y6C)K2Q,3LW)9GD,MFC)J9X,B2P)X5S,N28)Q3P,CW1)VF5,CFP)R6X,K91)GQV,3Q8)TNM,KZJ)1MJ,SDQ)PLC,8JC)5YG,3ZN)3HR,4J6)N5S,HK8)BZY,FQM)TTQ,TM5)526,SQX)FQM,772)BRK,V63)FC7,DZM)2B3,BRY)YS6,LCZ)YG1,C4F)W47,WFM)6QX,N28)TVJ,L7Z)468,MVR)254,ZCT)9BV,SX1)583,52M)TSW,2PT)1Q1,7FY)3BF,YVP)LCZ,2CQ)7TD,D8V)5L1,Q3F)ZST,L7Z)YDP,4LF)SWH,MQQ)PKR,WC8)FLW,JL8)DF1,5Y3)2TV,5DQ)8X6,M5Q)6D7,KGW)QTF,T2T)RDF,4Q7)K46,L4K)JSF,FVX)HVH,49J)XWX,8NZ)3MC,F6N)KP1,WZX)ZFX,H9L)F8B,1MG)W86,KXK)SS3,X2B)RWY,N1Q)669,43R)Q59,JH9)458,C87)T2T,XW3)N1Y,GZJ)MC9,2DP)3ZY,5DS)JS6,MRS)L79,YP2)71Q,MJJ)VC1,7SD)MFC,13L)997,4YS)4LY,479)CFR,V9X)7MZ,LP1)NNS,B7H)ZS4,5CF)VCP,FZ5)3JN,T6Y)R3T,J53)VPT,VVC)7ZZ,776)Q1Z,94J)LP1,4PC)4ZQ,T5R)GP9,F2F)4V5,PND)KST,Z11)N7V,TX7)M5Q,VDS)X1X,6D7)YP2,YS4)5LQ,FHY)7PL,HD5)VTS,BKY)WWN,4HJ)4C3,ZX2)V6L,BR9)3LW,DX4)BPB,TY6)GY3,XBX)Y5Q,4XS)94T,32F)8P7,2S1)B7M,D9J)YT7,54D)Z5T,KMC)7SC,NJK)1KF,CBX)SPN,KDG)M7P,PB4)147,L4V)YPD,JHZ)YJB,J31)KK9,JKZ)Q6V,LWX)ZRF,3LM)LKM,4TW)4HC,N1Y)4J6,LYZ)TX7,JFZ)4LF,1FT)8CS,8JX)LWX,W9V)5LH,73S)9Q7,HFK)YVP,RGV)HFK,5MR)3MP,8JC)987,BJY)SGS,NYS)DXM,X1X)6K1,JJ6)KLD,ZRN)VVD,6RG)GNF,4Q6)NJK,H3Z)7SN,R3T)RRR,57Y)KNF,DF1)JYX,XWX)6QC,FWP)4HJ,3B6)1SP,9F9)9HS,Y21)WVQ,N57)1S5,TVJ)Y8G,TZG)T46,1X3)BN6,DXM)CC6,T3D)SFB,XQC)NDX,X22)JBW,365)TJL,J7C)XQG,VFV)S2N,4PZ)PZK,4HC)YQM,NPR)Y8J,VG7)WYW,C6L)M63,P98)JMK,Z9K)1RM,SFG)4RN,VJ3)ZJH,WZX)WKR,PGR)NGV,SYB)B6S,J39)VQM,N7V)RMW,22F)59C,H3Z)NKX,MJY)HX5,TJL)5LX,GWD)S7X,123)Y5Y,XJ1)4PZ,DK2)J3G,V11)L4K,958)F2X,6R6)V11,VW7)Y6C,CD1)8ZK,96K)JG2,YT6)C5T,KZ2)9X2,JFP)PND,WB3)9MC,98H)BJY,C8B)DX4,465)HS2,WC3)QZC,9J6)XBR,LHX)68T,B2B)L6R,468)XW4,MN9)RTK,TSF)6F7,XB6)F56,C55)HGL,526)MTT,9CQ)KR4,699)9XZ,ZF1)5D9,NDD)BQB,Y5Z)43R,M7P)Y7T,GQV)K1J,HQS)4TW,1WV)XL6,LGP)QSX,6JN)P67,X5H)NYS,DDV)74T,PNT)ZF1,V54)Q9N,KB5)SX1,1FW)3GZ,CHK)G5C,T8J)ZMN,CZQ)VML,4L2)CDG,TNM)JVS,JG2)D45,D4L)Y21,VJ3)RGP,K1J)T39,SWH)BX8,ZMN)35P,458)F6N,43S)JW9,KVC)Q5Y,F3Q)69X,S5H)8JC,JY1)J4T,VML)BHG,NCP)43X,2VC)4JC,MSZ)YM3,9K1)P4G,SP7)XM6,J1C)3VP,973)PNT,79K)YJT,5YG)SBH,56M)T3D,SCM)Y7X,R89)2S1,YDP)C87,VQM)32F,J3F)W1D,PDW)16N,QLH)49F,G1V)WWB,ZJH)YMG,VTS)B2B,KTZ)48G,MSS)FJV,HXQ)27Z,C34)CNC,NH7)JJJ,SKT)14L,VLH)8WX,7WL)SSQ,D9T)BPZ,YT7)54D,72J)M19,92T)HVS,DLL)RGV,S9V)J53,BQB)QRH,NF5)DHR,85C)RYH,XW4)XQC,JGB)F1X,2R9)PB2,KZG)M71,SS3)QYM,T52)S5H,7C2)HD5,4TW)Q7S,HW8)123,JX6)Q3F,HK1)22F,9K5)7FV,9X2)TSF,3BF)6DT,JLZ)1TX,Q1Z)VLH,5LX)S9H,7WZ)N32,X96)55S,PF7)9Q6,59Z)6Y3,K4Z)SYB,P7C)KCB,997)776,SFB)Z2W,HSC)5LG,9GD)PY5,YPD)6JS";
            map_entries = input.Split(",").ToList();
        }

    }


    public class Orbit
    {
        public string OrbitName { get; set; } = "";
        private string DirectOrbit;
        private List<string> IndirectlyOrbits = new List<string>();
        public int GetTotalOrbitedByCount()
        {
            return (DirectOrbit != null ? 1 : 0) + IndirectlyOrbits.Count();
        }
        public void AddOrbitedValue(string name, bool is_direct)
        {
            if (is_direct)
            {
                //check that it doesn't already contain this orbiter
                if (DirectOrbit == null)
                {
                    DirectOrbit = name;
                }
            }
            else
            {
                if (!IndirectlyOrbits.Contains(name))
                {
                    IndirectlyOrbits.Add(name);
                }
            }
        }

        internal string GetParent()
        {
            return DirectOrbit;
        }

        internal bool IncludesIndirect(string parent)
        {
            return IndirectlyOrbits.Contains(parent);
        }

        internal List<string> GetAllOrbits()
        {
            List<string> all_orbits = new List<string>();
            all_orbits.Add(this.GetParent());
            all_orbits.AddRange(this.IndirectlyOrbits);
            return all_orbits;
        }

        public Orbit(string name)
        {
            OrbitName = name;
        }
    }

    
}