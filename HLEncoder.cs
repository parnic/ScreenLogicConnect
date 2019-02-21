﻿namespace ScreenLogicConnect
{
    public class HLEncoder
    {
        private const int BLOCK_SIZE = 16;
        private const int MAX_KC = 8;
        private const int MAX_ROUNDS = 14;

        private readonly int[,] Kd = new int[MAX_ROUNDS + 1, MAX_KC];
        private readonly int[,] Ke = new int[MAX_ROUNDS + 1, MAX_KC];
        private readonly byte[] sm_S = new byte[] { 0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x1, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, 0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, 0x4, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x5, 0x9a, 0x7, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, 0x9, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, 0x53, 0xd1, 0x0, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, 0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x2, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, 0xcd, 0xc, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, 0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0xb, 0xdb, 0xe0, 0x32, 0x3a, 0xa, 0x49, 0x6, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, 0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x8, 0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x3, 0xf6, 0xe, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, 0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, 0x8c, 0xa1, 0x89, 0xd, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0xf, 0xb0, 0x54, 0xbb, 0x16 };
        private readonly int[] sm_T1 = new int[] { -966564955, -126059388, -294160487, -159679603, -855539, -697603139, -563122255, -1849309868, 1613770832, 33620227, -832084055, 1445669757, -402719207, -1244145822, 1303096294, -327780710, -1882535355, 528646813, -1983264448, -92439161, -268764651, -1302767125, -1907931191, -68095989, 1101901292, -1277897625, 1604494077, 1169141738, 597466303, 1403299063, -462261610, -1681866661, 1974974402, -503448292, 1033081774, 1277568618, 1815492186, 2118074177, -168298750, -2083730353, 1748251740, 1369810420, -773462732, -101584632, -495881837, -1411852173, 1647391059, 706024767, 134480908, -1782069422, 1176707941, -1648114850, 806885416, 932615841, 168101135, 798661301, 235341577, 605164086, 461406363, -538779075, -840176858, 1311188841, 2142417613, -361400929, 302582043, 495158174, 1479289972, 874125870, 907746093, -596742478, -1269146898, 1537253627, -1538108682, 1983593293, -1210657183, 2108928974, 1378429307, -572267714, 1580150641, 327451799, -1504488459, -1177431704, 0, -1041371860, 1075847264, -469959649, 2041688520, -1235526675, -731223362, -1916023994, 1740553945, 1916352843, -1807070498, -1739830060, -1336387352, -2049978550, -1143943061, -974131414, 1336584933, -302253290, -2042412091, -1706209833, 1714631509, 293963156, -1975171633, -369493744, 67240454, -25198719, -1605349136, 2017213508, 631218106, 1269344483, -1571728909, 1571005438, -2143272768, 93294474, 1066570413, 563977660, 1882732616, -235539196, 1673313503, 2008463041, -1344611723, 1109467491, 537923632, -436207846, -34344178, -1076702611, -2117218996, 403442708, 638784309, -1007883217, -1101045791, 899127202, -2008791860, 773265209, -1815821225, 1437050866, -58818942, 2050833735, -932944724, -1168286233, 840505643, -428641387, -1067425632, 427917720, -1638969391, -1545806721, 1143087718, 1412049534, 999329963, 193497219, -1941551414, -940642775, 1807268051, 672404540, -1478566279, -1134666014, 369822493, -1378100362, -606019525, 1681011286, 1949973070, 336202270, -1840690725, 201721354, 1210328172, -1201906460, -1614626211, -1110191250, 1135389935, -1000185178, 965841320, 831886756, -739974089, -226920053, -706222286, -1949775805, 1849112409, -630362697, 26054028, -1311386268, -1672589614, 1235855840, -663982924, -1403627782, -202050553, -806688219, -899324497, -193299826, 1202630377, 268961816, 1874508501, -260540280, 1243948399, 1546530418, 941366308, 1470539505, 1941222599, -1748580783, -873928669, -1579295364, -395021156, 1042226977, -1773450275, 1639824860, 227249030, 260737669, -529502064, 2084453954, 1907733956, -865704278, -1874310952, 100860677, -134810111, 470683154, -1033805405, 1781871967, -1370007559, 1773779408, 394692241, -1715355304, 974986535, 664706745, -639508168, -336005101, 731420851, 571543859, -764843589, -1445340816, 126783113, 865375399, 765172662, 1008606754, 361203602, -907417312, -2016489911, -1437248001, 1344809080, -1512054918, 59542671, 1503764984, 160008576, 437062935, 1707065306, -672733647, -2076032314, -798463816, -2109652541, 697932208, 1512910199, 504303377, 2075177163, -1470868228, 1841019862, 739644986 };
        private readonly int[] sm_T2 = new int[] { -1513725085, -2064089988, -1712425097, -1913226373, 234877682, -1110021269, -1310822545, 1418839493, 1348481072, 50462977, -1446090905, 2102799147, 434634494, 1656084439, -431117397, -1695779210, 1167051466, -1658879358, 1082771913, -2013627011, 368048890, -340633255, -913422521, 201060592, -331240019, 1739838676, -44064094, -364531793, -1088185188, -145513308, -1763413390, 1536934080, -1032472649, 484572669, -1371696237, 1783375398, 1517041206, 1098792767, 49674231, 1334037708, 1550332980, -195975771, 886171109, 150598129, -1813876367, 1940642008, 1398944049, 1059722517, 201851908, 1385547719, 1699095331, 1587397571, 674240536, -1590192490, 252314885, -1255171430, 151914247, 908333586, -1692696448, 1038082786, 651029483, 1766729511, -847269198, -1612024459, 454166793, -1642232957, 1951935532, 775166490, 758520603, -1294176658, -290170278, -77881184, -157003182, 1299594043, 1639438038, -830622797, 2068982057, 1054729187, 1901997871, -1760328572, -173649069, 1757008337, 0, 750906861, 1614815264, 535035132, -931548751, -306816165, -1093375382, 1183697867, -647512386, 1265776953, -560706998, -728216500, -391096232, 1250283471, 1807470800, 717615087, -447763798, 384695291, -981056701, -677753523, 1432761139, -1810791035, -813021883, 283769337, 100925954, -2114027649, -257929136, 1148730428, -1171939425, -481580888, -207466159, -27417693, -1065336768, -1979347057, -1388342638, -1138647651, 1215313976, 82966005, -547111748, -1049119050, 1974459098, 1665278241, 807407632, 451280895, 251524083, 1841287890, 1283575245, 337120268, 891687699, 801369324, -507617441, -1573546089, -863484860, 959321879, 1469301956, -229267545, -2097381762, 1199193405, -1396153244, -407216803, 724703513, -1780059277, -1598005152, -1743158911, -778154161, 2141445340, 1715741218, 2119445034, -1422159728, -2096396152, -896776634, 700968686, -747915080, 1009259540, 2041044702, -490971554, 487983883, 1991105499, 1004265696, 1449407026, 1316239930, 504629770, -611169975, 168560134, 1816667172, -457679780, 1570751170, 1857934291, -280777556, -1497079198, -1472622191, -1540254315, 936633572, -1947043463, 852879335, 1133234376, 1500395319, -1210421907, -1946055283, 1689376213, -761508274, -532043351, -1260884884, -89369002, 133428468, 634383082, -1345690267, -1896580486, -381178194, 403703816, -714097990, -1997506440, 1867130149, 1918643758, 607656988, -245913946, -948718412, 1368901318, 600565992, 2090982877, -1662487436, 557719327, -577352885, -597574211, -2045932661, -2062579062, -1864339344, 1115438654, -999180875, -1429445018, -661632952, 84280067, 33027830, 303828494, -1547542175, 1600795957, -106014889, -798377543, -1860729210, 1486471617, 658119965, -1188585826, 953803233, 334231800, -1288988520, 857870609, -1143838359, 1890179545, -1995993458, -1489791852, -1238525029, 574365214, -1844082809, 550103529, 1233637070, -5614251, 2018519080, 2057691103, -1895592820, -128343647, -2146858615, 387583245, -630865985, 836232934, -964410814, -1194301336, -1014873791, -1339450983, 2002398509, 287182607, -881086288, -56077228, -697451589, 975967766 };
        private readonly int[] sm_T3 = new int[] { 1671808611, 2089089148, 2006576759, 2072901243, -233963534, 1807603307, 1873927791, -984313403, 810573872, 16974337, 1739181671, 729634347, -31856642, -681396777, -1410970197, 1989864566, -901410870, -2103631998, -918517303, 2106063485, -99225606, 1508618841, 1204391495, -267650064, -1377025619, -731401260, -1560453214, -1343601233, -1665195108, -1527295068, 1922491506, -1067738176, -1211992649, -48438787, -1817297517, 644500518, 911895606, 1061256767, -150800905, -867204148, 878471220, -1510714971, -449523227, -251069967, 1905517169, -663508008, 827548209, 356461077, 67897348, -950889017, 593839651, -1017209405, 405286936, -1767819370, 84871685, -1699401830, 118033927, 305538066, -2137318528, -499261470, -349778453, 661212711, -1295155278, 1973414517, 152769033, -2086789757, 745822252, 439235610, 455947803, 1857215598, 1525593178, -1594139744, 1391895634, 994932283, -698239018, -1278313037, 695947817, -482419229, 795958831, -2070473852, 1408607827, -781665839, 0, -315833875, 543178784, -65018884, -1312261711, 1542305371, 1790891114, -884568629, -1093048386, 961245753, 1256100938, 1289001036, 1491644504, -817199665, -798245936, -282409489, -1427812438, -82383365, 1137018435, 1305975373, 861234739, -2053893755, 1171229253, -116332039, 33948674, 2139225727, 1357946960, 1011120188, -1615190625, -1461498968, 1374921297, -1543610973, 1086357568, -1886780017, -1834139758, -1648615011, 944271416, -184225291, -1126210628, -1228834890, -629821478, 560153121, 271589392, -15014401, -217121293, -764559406, -850624051, 202643468, 322250259, -332413972, 1608629855, -1750977129, 1154254916, 389623319, -1000893500, -1477290585, 2122513534, 1028094525, 1689045092, 1575467613, 422261273, 1939203699, 1621147744, -2120738431, 1339137615, -595614756, 577127458, 712922154, -1867826288, -2004677752, 1187679302, -299251730, -1194103880, 339486740, -562452514, 1591917662, 186455563, -612979237, -532948000, 844522546, 978220090, 169743370, 1239126601, 101321734, 611076132, 1558493276, -1034051646, -747717165, -1393605716, 1655096418, -1851246191, -1784401515, -466103324, 2039214713, -416098841, -935097400, 928607799, 1840765549, -1920204403, -714821163, 1322425422, -1444918871, 1823791212, 1459268694, -200805388, -366620694, 1706019429, 2056189050, -1360443474, 135794696, -1160417350, 2022240376, 628050469, 779246638, 472135708, -1494132826, -1261997132, -967731258, -400307224, -579034659, 1956440180, 522272287, 1272813131, -1109630531, -1954148981, -1970991222, 1888542832, 1044544574, -1245417035, 1722469478, 1222152264, 50660867, -167643146, 236067854, 1638122081, 895445557, 1475980887, -1177523783, -2037311610, -1051158079, 489110045, -1632032866, -516367903, -132912136, -1733088360, 288563729, 1773916777, -646927911, -1903622258, -1800981612, -1682559589, 505560094, -2020469369, -383727127, -834041906, 1442818645, 678973480, -545610273, -1936784500, -1577559647, -1988097655, 219617805, -1076206145, -432941082, 1120306242, 1756942440, 1103331905, -1716508263, 762796589, 252780047, -1328841808, 1425844308, -1143575109, 372911126 };
        private readonly int[] sm_T4 = new int[] { 1667474886, 2088535288, 2004326894, 2071694838, -219017729, 1802223062, 1869591006, -976923503, 808472672, 16843522, 1734846926, 724270422, -16901657, -673750347, -1414797747, 1987484396, -892713585, -2105369313, -909557623, 2105378810, -84273681, 1499065266, 1195886990, -252703749, -1381110719, -724277325, -1566376609, -1347425723, -1667449053, -1532692653, 1920112356, -1061135461, -1212693899, -33743647, -1819038147, 640051788, 909531756, 1061110142, -134806795, -859025533, 875846760, -1515850671, -437963567, -235861767, 1903268834, -656903253, 825316194, 353713962, 67374088, -943238507, 589522246, -1010606435, 404236336, -1768513225, 84217610, -1701137105, 117901582, 303183396, -2139055333, -488489505, -336910643, 656894286, -1296904833, 1970642922, 151591698, -2088526307, 741110872, 437923380, 454765878, 1852748508, 1515908788, -1600062629, 1381168804, 993742198, -690593353, -1280061827, 690584402, -471646499, 791638366, -2071685357, 1398011302, -774805319, 0, -303223615, 538992704, -50585629, -1313748871, 1532751286, 1785380564, -875870579, -1094788761, 960056178, 1246420628, 1280103576, 1482221744, -808498555, -791647301, -269538619, -1431640753, -67430675, 1128514950, 1296947098, 859002214, -2054843375, 1162203018, -101117719, 33687044, 2139062782, 1347481760, 1010582648, -1616922075, -1465326773, 1364325282, -1549533603, 1077985408, -1886418427, -1835881153, -1650607071, 943212656, -168491791, -1128472733, -1229536905, -623217233, 555836226, 269496352, -58651, -202174723, -757961281, -842183551, 202118168, 320025894, -320065597, 1600119230, -1751670219, 1145359496, 387397934, -993765485, -1482165675, 2122220284, 1027426170, 1684319432, 1566435258, 421079858, 1936954854, 1616945344, -2122213351, 1330631070, -589529181, 572679748, 707427924, -1869567173, -2004319477, 1179044492, -286381625, -1195846805, 336870440, -555845209, 1583276732, 185277718, -606374227, -522175525, 842159716, 976899700, 168435220, 1229577106, 101059084, 606366792, 1549591736, -1027449441, -741118275, -1397952701, 1650632388, -1852725191, -1785355215, -454805549, 2038008818, -404278571, -926399605, 926374254, 1835907034, -1920103423, -707435343, 1313788572, -1448484791, 1819063512, 1448540844, -185333773, -353753649, 1701162954, 2054852340, -1364268729, 134748176, -1162160785, 2021165296, 623210314, 774795868, 471606328, -1499008681, -1263220877, -960081513, -387439669, -572687199, 1953799400, 522133822, 1263263126, -1111630751, -1953790451, -1970633457, 1886425312, 1044267644, -1246378895, 1718004428, 1212733584, 50529542, -151649801, 235803164, 1633788866, 892690282, 1465383342, -1179004823, -2038001385, -1044293479, 488449850, -1633765081, -505333543, -117959701, -1734823125, 286339874, 1768537042, -640061271, -1903261433, -1802197197, -1684294099, 505291324, -2021158379, -370597687, -825341561, 1431699370, 673740880, -539002203, -1936945405, -1583220647, -1987477495, 218961690, -1077945755, -421121577, 1111672452, 1751693520, 1094828930, -1717981143, 757954394, 252645662, -1330590853, 1414855848, -1145317779, 370555436 };
        private readonly int[] sm_U1 = new int[] { 0, 235474187, 470948374, 303765277, 941896748, 908933415, 607530554, 708780849, 1883793496, 2118214995, 1817866830, 1649639237, 1215061108, 1181045119, 1417561698, 1517767529, -527380304, -291906117, -58537306, -225720403, -659233636, -692196969, -995688822, -894438527, -1864845080, -1630423581, -1932877058, -2101104651, -1459843900, -1493859889, -1259432238, -1159226407, -616842373, -718096784, -953573011, -920605594, -484470953, -317291940, -15887039, -251357110, -1418472669, -1518674392, -1218328267, -1184316354, -1822955761, -1654724092, -1891238631, -2125664238, 1001089995, 899835584, 666464733, 699432150, 59727847, 226906860, 530400753, 294930682, 1273168787, 1172967064, 1475418501, 1509430414, 1942435775, 2110667444, 1876241833, 1641816226, -1384747530, -1551933187, -1318815776, -1083344149, -1789765158, -1688513327, -1992277044, -2025238841, -583137874, -751368027, -1054072904, -819653965, -451268222, -351060855, -116905068, -150919521, 1306967366, 1139781709, 1374988112, 1610459739, 1975683434, 2076935265, 1775276924, 1742315127, 1034867998, 866637845, 566021896, 800440835, 92987698, 193195065, 429456164, 395441711, 1984812685, 2017778566, 1784663195, 1683407248, 1315562145, 1080094634, 1383856311, 1551037884, 101039829, 135050206, 437757123, 337553864, 1042385657, 807962610, 573804783, 742039012, -1763899843, -1730933962, -1966138325, -2067394272, -1359400431, -1594867942, -1293211641, -1126030068, -426414491, -392404114, -91786125, -191989384, -558802359, -793225406, -1029488545, -861254316, 1106041591, 1340463100, 1576976609, 1408749034, 2043211483, 2009195472, 1708848333, 1809054150, 832877231, 1068351396, 766945465, 599762354, 159417987, 126454664, 361929877, 463180190, -1585706425, -1351284916, -1116860335, -1285087910, -1722270101, -1756286112, -2058738563, -1958532746, -785096161, -549621996, -853116919, -1020300030, -384805325, -417768648, -184398811, -83148498, -1697160820, -1797362553, -2033878118, -1999866223, -1561111136, -1392879445, -1092530250, -1326955843, -358676012, -459930401, -158526526, -125559095, -759480840, -592301837, -827774994, -1063245083, 2051518780, 1951317047, 1716890410, 1750902305, 1113818384, 1282050075, 1584504582, 1350078989, 168810852, 67556463, 371049330, 404016761, 841739592, 1008918595, 775550814, 540080725, -325404927, -493635062, -259478249, -25059300, -725712083, -625504730, -928212677, -962227152, -1663901863, -1831087534, -2134850225, -1899378620, -1527321739, -1426069890, -1192955549, -1225917336, 202008497, 33778362, 270040487, 504459436, 875451293, 975658646, 675039627, 641025152, 2084704233, 1917518562, 1615861247, 1851332852, 1147550661, 1248802510, 1484005843, 1451044056, 933301370, 967311729, 733156972, 632953703, 260388950, 25965917, 328671808, 496906059, 1206477858, 1239443753, 1543208500, 1441952575, 2144161806, 1908694277, 1675577880, 1842759443, -684598070, -650587711, -886847780, -987051049, -283776794, -518199827, -217582864, -49348613, -1485196142, -1452230247, -1150570876, -1251826801, -1621262146, -1856729675, -2091935064, -1924753501 };
        private readonly int[] sm_U2 = new int[] { 0, 185469197, 370938394, 487725847, 741876788, 657861945, 975451694, 824852259, 1483753576, 1400783205, 1315723890, 1164071807, 1950903388, 2135319889, 1649704518, 1767536459, -1327460144, -1141990947, -1493400886, -1376613433, -1663519516, -1747534359, -1966823682, -2117423117, -393160520, -476130891, -24327518, -175979601, -995558260, -811141759, -759894378, -642062437, 2077965243, 1893020342, 1841768865, 1724457132, 1474502543, 1559041666, 1107234197, 1257309336, 598438867, 681933534, 901210569, 1052338372, 261314535, 77422314, 428819965, 310463728, -885281941, -1070226842, -584599183, -701910916, -419197089, -334657966, -249586363, -99511224, -1823743229, -1740248562, -2057834215, -1906706412, -1082931401, -1266823622, -1452288723, -1570644960, -156404115, -39616672, -525245321, -339776134, -627748263, -778347692, -863420349, -947435186, -1361232379, -1512884472, -1195299809, -1278270190, -2098914767, -1981082820, -1795618773, -1611202266, 1179510461, 1296297904, 1347548327, 1533017514, 1786102409, 1635502980, 2087309459, 2003294622, 507358933, 355706840, 136428751, 53458370, 839224033, 957055980, 605657339, 790073846, -1921626666, -2038938405, -1687527476, -1872472383, -1588696606, -1438621457, -1219331080, -1134791947, -721025602, -569897805, -1021700188, -938205527, -113368694, -231724921, -282971248, -466863459, 1033297158, 915985419, 730517276, 545572369, 296679730, 446754879, 129166120, 213705253, 1709610350, 1860738147, 1945798516, 2029293177, 1239331162, 1120974935, 1606591296, 1422699085, -146674470, -61872681, -513933632, -363595827, -612775698, -797457949, -848962828, -966011911, -1355701070, -1539330625, -1188186456, -1306280027, -2096529274, -2012771957, -1793748324, -1642357871, 1201765386, 1286567175, 1371368976, 1521706781, 1805211710, 1620529459, 2105887268, 1988838185, 533804130, 350174575, 164439672, 46346101, 870912086, 954669403, 636813900, 788204353, -1936009375, -2020286868, -1702443653, -1853305738, -1599933611, -1414727080, -1229004465, -1112479678, -722821367, -538667516, -1024029421, -906460130, -120407235, -203640272, -288446169, -440360918, 1014646705, 930369212, 711349675, 560487590, 272786309, 457992840, 106852767, 223377554, 1678381017, 1862534868, 1914052035, 2031621326, 1211247597, 1128014560, 1580087799, 1428173050, 32283319, 182621114, 401639597, 486441376, 768917123, 651868046, 1003007129, 818324884, 1503449823, 1385356242, 1333838021, 1150208456, 1973745387, 2125135846, 1673061617, 1756818940, -1324610969, -1174273174, -1492117379, -1407315600, -1657524653, -1774573730, -1960297399, -2144979644, -377732593, -495826174, -10465259, -194094824, -985373125, -833982666, -749177823, -665420500, 2050466060, 1899603969, 1814803222, 1730525723, 1443857720, 1560382517, 1075025698, 1260232239, 575138148, 692707433, 878443390, 1062597235, 243256656, 91341917, 409198410, 325965383, -891866660, -1042728751, -590666810, -674944309, -420538904, -304014107, -252508174, -67301633, -1834518092, -1716948807, -2068091986, -1883938141, -1096852096, -1248766835, -1467789414, -1551022441 };
        private readonly int[] sm_U3 = new int[] { 0, 218828297, 437656594, 387781147, 875313188, 958871085, 775562294, 590424639, 1750626376, 1699970625, 1917742170, 2135253587, 1551124588, 1367295589, 1180849278, 1265195639, -793714544, -574886247, -895026046, -944901493, -459482956, -375925059, -24460122, -209597777, -1192718120, -1243373871, -1560376118, -1342864701, -1933268740, -2117097739, -1764576018, -1680229657, -1149510853, -1234119374, -1586641111, -1402549984, -1890065633, -2107839210, -1790836979, -1739919100, -752637069, -567761542, -919226527, -1002522264, -418409641, -368796322, -48656571, -267222708, 1808481195, 1723872674, 1910319033, 2094410160, 1608975247, 1391201670, 1173430173, 1224348052, 59984867, 244860394, 428169201, 344873464, 935293895, 984907214, 766078933, 547512796, 1844882806, 1627235199, 2011214180, 2062270317, 1507497298, 1423022939, 1137477952, 1321699145, 95345982, 145085239, 532201772, 313773861, 830661914, 1015671571, 731183368, 648017665, -1119466010, -1337113617, -1487908364, -1436852227, -1989511742, -2073986101, -1820562992, -1636341799, -719438418, -669699161, -821550660, -1039978571, -516815478, -331805821, -81520232, -164685935, -695372211, -611944380, -862229921, -1047501738, -492745111, -274055072, -122203525, -172204942, -1093335547, -1277294580, -1530717673, -1446505442, -1963377119, -2014171096, -1863376333, -1645990854, 104699613, 188127444, 472615631, 287343814, 840019705, 1058709744, 671593195, 621591778, 1852171925, 1668212892, 1953757831, 2037970062, 1514790577, 1463996600, 1080017571, 1297403050, -621329940, -671330331, -1058972162, -840281097, -287606328, -472877119, -187865638, -104436781, -1297141340, -1079754835, -1464259146, -1515052097, -2038232704, -1954019447, -1667951214, -1851909221, 172466556, 122466165, 273792366, 492483431, 1047239000, 861968209, 612205898, 695634755, 1646252340, 1863638845, 2013908262, 1963115311, 1446242576, 1530455833, 1277555970, 1093597963, 1636604631, 1820824798, 2073724613, 1989249228, 1436590835, 1487645946, 1337376481, 1119727848, 164948639, 81781910, 331544205, 516552836, 1039717051, 821288114, 669961897, 719700128, -1321436601, -1137216434, -1423284651, -1507760036, -2062531997, -2011476886, -1626972559, -1844621192, -647755249, -730921978, -1015933411, -830924780, -314035669, -532464606, -144822727, -95084496, -1224610662, -1173691757, -1390940024, -1608712575, -2094148418, -1910056265, -1724135252, -1808742747, -547775278, -766340389, -984645440, -935031095, -344611594, -427906305, -245122844, -60246291, 1739656202, 1790575107, 2108100632, 1890328081, 1402811438, 1586903591, 1233856572, 1149249077, 266959938, 48394827, 369057872, 418672217, 1002783846, 919489135, 567498868, 752375421, 209336225, 24197544, 376187827, 459744698, 945164165, 895287692, 574624663, 793451934, 1679968233, 1764313568, 2117360635, 1933530610, 1343127501, 1560637892, 1243112415, 1192455638, -590686415, -775825096, -958608605, -875051734, -387518699, -437395172, -219090169, -262898, -1265457287, -1181111952, -1367032981, -1550863006, -2134991011, -1917480620, -1700232369, -1750889146 };
        private readonly int[] sm_U4 = new int[] { 0, 151849742, 303699484, 454499602, 607398968, 758720310, 908999204, 1059270954, 1214797936, 1097159550, 1517440620, 1400849762, 1817998408, 1699839814, 2118541908, 2001430874, -1865371424, -1713521682, -2100648196, -1949848078, -1260086056, -1108764714, -1493267772, -1342996022, -658970480, -776608866, -895287668, -1011878526, -57883480, -176042074, -292105548, -409216582, 1002142683, 850817237, 698445255, 548169417, 529487843, 377642221, 227885567, 77089521, 1943217067, 2061379749, 1640576439, 1757691577, 1474760595, 1592394909, 1174215055, 1290801793, -1418998981, -1570324427, -1183720153, -1333995991, -1889540349, -2041385971, -1656360673, -1807156719, -486304949, -368142267, -249985705, -132870567, -952647821, -835013507, -718427793, -601841055, 1986918061, 2137062819, 1685577905, 1836772287, 1381620373, 1532285339, 1078185097, 1229899655, 1040559837, 923313619, 740276417, 621982671, 439452389, 322734571, 137073913, 19308535, -423803315, -273658557, -190361519, -39167137, -1031181707, -880516741, -795640727, -643926169, -1361764803, -1479011021, -1127282655, -1245576401, -1964953083, -2081670901, -1728371687, -1846137065, 1305906550, 1155237496, 1607244650, 1455525988, 1776460110, 1626319424, 2079897426, 1928707164, 96392454, 213114376, 396673818, 514443284, 562755902, 679998000, 865136418, 983426092, -586793578, -737462632, -820237430, -971956092, -114159186, -264299872, -349698126, -500888388, -1787927066, -1671205144, -2022411270, -1904641804, -1319482914, -1202240816, -1556062270, -1437772596, -321194175, -438830001, -20913827, -137500077, -923870343, -1042034569, -621490843, -738605461, -1531793615, -1379949505, -1230456531, -1079659997, -2138668279, -1987344377, -1835231979, -1684955621, 2081048481, 1963412655, 1846563261, 1729977011, 1480485785, 1362321559, 1243905413, 1126790795, 878845905, 1030690015, 645401037, 796197571, 274084841, 425408743, 38544885, 188821243, -681472870, -563312748, -981755258, -864644728, -212492126, -94852180, -514869570, -398279248, -1626745622, -1778065436, -1928084746, -2078357000, -1153566510, -1305414692, -1457000754, -1607801408, 1202797690, 1320957812, 1437280870, 1554391400, 1669664834, 1787304780, 1906247262, 2022837584, 265905162, 114585348, 499347990, 349075736, 736970802, 585122620, 972512814, 821712160, -1699282452, -1816524062, -2001922064, -2120213250, -1098699308, -1215420710, -1399243832, -1517014842, -757114468, -606973294, -1060810880, -909622130, -152341084, -1671510, -453942344, -302225226, 174567692, 57326082, 410887952, 292596766, 777231668, 660510266, 1011452712, 893681702, 1108339068, 1258480242, 1343618912, 1494807662, 1715193156, 1865862730, 1948373848, 2100090966, -1593017801, -1476300487, -1290376149, -1172609243, -2059905521, -1942659839, -1759363053, -1641067747, -379313593, -529979063, -75615141, -227328171, -850391425, -1000536719, -548792221, -699985043, 836553431, 953270745, 600235211, 718002117, 367585007, 484830689, 133361907, 251657213, 2041877159, 1891211689, 1806599355, 1654886325, 1568718495, 1418573201, 1335535747, 1184342925 };
        private readonly byte[] sm_rcon = new byte[] { 0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91 };
        readonly int[] tk = new int[MAX_KC];

        private bool bKeyInit;
        int iROUNDS;

        public HLEncoder(string pwd)
        {
            if (!string.IsNullOrEmpty(pwd))
            {
                MakeKey(pwd);
            }
        }

        public byte[] GetEncryptedPassword(string cls)
        {
            if (!string.IsNullOrEmpty(cls))
            {
                return Encrypt(cls);
            }

            return null;
        }

        private byte[] MakeBlock(string str, byte byFill)
        {
            int iLen = BLOCK_SIZE;
            if (str.Length >= BLOCK_SIZE)
            {
                iLen = str.Length;
            }
            byte[] block = new byte[(((iLen / BLOCK_SIZE) + (iLen % BLOCK_SIZE > 0 ? 1 : 0)) * BLOCK_SIZE)];
            for (int i = 0; i < block.Length; i++)
            {
                if (i < str.Length)
                {
                    block[i] = (byte)str[i];
                }
                else
                {
                    block[i] = byFill;
                }
            }
            return block;
        }

        private byte[] MakeBlock(byte[] source, byte byFill)
        {
            int iLen = BLOCK_SIZE;
            if (source.Length >= BLOCK_SIZE)
            {
                iLen = source.Length;
            }
            byte[] block = new byte[(((iLen / BLOCK_SIZE) + (iLen % BLOCK_SIZE > 0 ? 1 : 0)) * BLOCK_SIZE)];
            for (int i = 0; i < block.Length; i++)
            {
                if (i < source.Length)
                {
                    block[i] = source[i];
                }
                else
                {
                    block[i] = byFill;
                }
            }
            return block;
        }

        private void MakeKey(string sChallengeStr)
        {
            MakeKeyFromBlock(MakeBlock(sChallengeStr, 0));
        }

        private void MakeKeyFromBlock(byte[] block)
        {
            if (block == null)
            {
                return;
            }

            if (block.Length == 16 || block.Length == 24 || block.Length == 32)
            {
                switch (block.Length)
                {
                    case 16:
                        iROUNDS = 10;
                        break;
                    case 24:
                        iROUNDS = 12;
                        break;
                    default:
                        iROUNDS = MAX_ROUNDS;
                        break;
                }

                for (int round = 0; round <= iROUNDS; round++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Ke[round, i] = 0;
                        Kd[round, i] = 0;
                    }
                }

                int ROUND_KEY_COUNT = (iROUNDS + 1) * 4;
                int numBlockChunks = block.Length / 4;
                for (int blockChunk = 0; blockChunk < numBlockChunks; blockChunk++)
                {
                    tk[blockChunk] = (block[(blockChunk * 4) + 0] << 24) | (block[(blockChunk * 4) + 1] << 16) | (block[(blockChunk * 4) + 2] << 8) | block[(blockChunk * 4) + 3];
                }

                int roundKey = 0;
                for (int i = 0; i < numBlockChunks && roundKey < ROUND_KEY_COUNT; i++, roundKey++)
                {
                    Ke[roundKey / 4, roundKey % 4] = tk[i];
                    Kd[iROUNDS - (roundKey / 4), roundKey % 4] = tk[i];
                }

                for (int rconPointer = 0; roundKey < ROUND_KEY_COUNT; rconPointer++)
                {
                    {
                        var tkVal = tk[numBlockChunks - 1];
                        tk[0] ^= ((((sm_S[(tkVal >> 16) & 0xFF] << 24) ^ (sm_S[(tkVal >> 8) & 0xFF] << 16)) ^ (sm_S[tkVal & 0xFF] << 8)) ^ sm_S[(tkVal >> 24) & 0xFF]) ^ (sm_rcon[rconPointer] << 24);
                    }

                    if (numBlockChunks != MAX_KC)
                    {
                        for (int prevIdx = 0, tkIdx = 1; tkIdx < numBlockChunks; prevIdx++, tkIdx++)
                        {
                            tk[tkIdx] ^= tk[prevIdx];
                        }
                    }
                    else
                    {
                        var blockChunkMidpoint = numBlockChunks / 2;
                        for (int prevIdx = 0, tkIdx = 1; tkIdx < blockChunkMidpoint; prevIdx++, tkIdx++)
                        {
                            tk[tkIdx] ^= tk[prevIdx];
                        }

                        var tkVal = tk[blockChunkMidpoint - 1];
                        tk[blockChunkMidpoint] ^= ((sm_S[tkVal & 0xFF] ^ (sm_S[(tkVal >> 8) & 0xFF] << 8)) ^ (sm_S[(tkVal >> 16) & 0xFF] << 16)) ^ (sm_S[(tkVal >> 24) & 0xFF] << 24);
                        for (int prevIdx = blockChunkMidpoint, tkIdx = blockChunkMidpoint + 1; tkIdx < numBlockChunks; prevIdx++, tkIdx++)
                        {
                            tk[tkIdx] ^= tk[prevIdx];
                        }
                    }

                    for (int tkIdx = 0; tkIdx < numBlockChunks && roundKey < ROUND_KEY_COUNT; tkIdx++, roundKey++)
                    {
                        Ke[roundKey / 4, roundKey % 4] = tk[tkIdx];
                        Kd[iROUNDS - (roundKey / 4), roundKey % 4] = tk[tkIdx];
                    }
                }

                for (int round = 1; round < iROUNDS; round++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var tt = Kd[round, i];
                        Kd[round, i] = ((sm_U1[(tt >> 24) & 0xFF] ^ sm_U2[(tt >> 16) & 0xFF]) ^ sm_U3[(tt >> 8) & 0xFF]) ^ sm_U4[tt & 0xFF];
                    }
                }

                bKeyInit = true;
            }
        }

        private byte[] EncryptBlock(byte[] block)
        {
            if (!bKeyInit)
            {
                return null;
            }

            var a = (block[0] << 24 | block[1] << 16 | block[2] << 8 | block[3]) ^ Ke[0, 0];
            var b = (block[4] << 24 | block[5] << 16 | block[6] << 8 | block[7]) ^ Ke[0, 1];
            var c = (block[8] << 24 | block[9] << 16 | block[10] << 8 | block[11]) ^ Ke[0, 2];
            var d = (block[12] << 24 | block[13] << 16 | block[14] << 8 | block[15]) ^ Ke[0, 3];
            for (int round = 1; round < iROUNDS; round++)
            {
                var a1 = sm_T1[(a >> 24 & 0xFF)] ^ sm_T2[(b >> 16 & 0xFF)] ^ sm_T3[(c >> 8 & 0xFF)] ^ sm_T4[(d & 0xFF)] ^ Ke[round, 0];
                var b1 = sm_T1[(b >> 24 & 0xFF)] ^ sm_T2[(c >> 16 & 0xFF)] ^ sm_T3[(d >> 8 & 0xFF)] ^ sm_T4[(a & 0xFF)] ^ Ke[round, 1];
                var c1 = sm_T1[(c >> 24 & 0xFF)] ^ sm_T2[(d >> 16 & 0xFF)] ^ sm_T3[(a >> 8 & 0xFF)] ^ sm_T4[(b & 0xFF)] ^ Ke[round, 2];
                var d1 = sm_T1[(d >> 24 & 0xFF)] ^ sm_T2[(a >> 16 & 0xFF)] ^ sm_T3[(b >> 8 & 0xFF)] ^ sm_T4[(c & 0xFF)] ^ Ke[round, 3];

                a = a1;
                b = b1;
                c = c1;
                d = d1;
            }

            return new byte[]
            {
                (byte)(sm_S[(a >> 24 & 0xFF)] ^ Ke[iROUNDS, 0] >> 24), (byte)(sm_S[(b >> 16 & 0xFF)] ^ Ke[iROUNDS, 0] >> 16), (byte)(sm_S[(c >> 8 & 0xFF)] ^ Ke[iROUNDS, 0] >> 8), (byte)(sm_S[(d & 0xFF)] ^ Ke[iROUNDS, 0]),
                (byte)(sm_S[(b >> 24 & 0xFF)] ^ Ke[iROUNDS, 1] >> 24), (byte)(sm_S[(c >> 16 & 0xFF)] ^ Ke[iROUNDS, 1] >> 16), (byte)(sm_S[(d >> 8 & 0xFF)] ^ Ke[iROUNDS, 1] >> 8), (byte)(sm_S[(a & 0xFF)] ^ Ke[iROUNDS, 1]),
                (byte)(sm_S[(c >> 24 & 0xFF)] ^ Ke[iROUNDS, 2] >> 24), (byte)(sm_S[(d >> 16 & 0xFF)] ^ Ke[iROUNDS, 2] >> 16), (byte)(sm_S[(a >> 8 & 0xFF)] ^ Ke[iROUNDS, 2] >> 8), (byte)(sm_S[(b & 0xFF)] ^ Ke[iROUNDS, 2]),
                (byte)(sm_S[(d >> 24 & 0xFF)] ^ Ke[iROUNDS, 3] >> 24), (byte)(sm_S[(a >> 16 & 0xFF)] ^ Ke[iROUNDS, 3] >> 16), (byte)(sm_S[(b >> 8 & 0xFF)] ^ Ke[iROUNDS, 3] >> 8), (byte)(sm_S[(c & 0xFF)] ^ Ke[iROUNDS, 3])
            };
        }

        byte[] Encrypt(string sString)
        {
            if (bKeyInit)
            {
                return EncryptData(MakeBlock(sString, 0));
            }
            return null;
        }

        byte[] Encrypt(byte[] source)
        {
            if (bKeyInit)
            {
                return EncryptData(MakeBlock(source, 0));
            }
            return null;
        }

        byte[] EncryptData(byte[] data)
        {
            if (data == null || data.Length < BLOCK_SIZE || data.Length % BLOCK_SIZE != 0)
            {
                return null;
            }
            byte[] byEncrypted = new byte[data.Length];
            int iNBlocks = data.Length / BLOCK_SIZE;
            for (int iBlock = 0; iBlock < iNBlocks; iBlock++)
            {
                int i;
                byte[] decryptedBlock = new byte[BLOCK_SIZE];
                for (i = 0; i < BLOCK_SIZE; i++)
                {
                    decryptedBlock[i] = data[(iBlock * BLOCK_SIZE) + i];
                }
                byte[] encryptedBlock = EncryptBlock(decryptedBlock);
                for (i = 0; i < encryptedBlock.Length; i++)
                {
                    byEncrypted[(iBlock * BLOCK_SIZE) + i] = encryptedBlock[i];
                }
            }
            return byEncrypted;
        }
    }
}
