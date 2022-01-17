import json,logging
from ryu.base import app_manager
from webob import Response
from ryu.controller import ofp_event
from ryu.controller.handler import MAIN_DISPATCHER
from ryu.controller.handler import set_ev_cls
from ryu.ofproto import ofproto_v1_3
from ryu.ofproto import ether
from ryu.ofproto import inet
from ryu.lib.packet import packet
from ryu.lib.packet import ethernet
from ryu.lib.packet import ipv4
from ryu.lib.packet import tcp
from ryu.lib.packet import udp
from ryu.lib.packet import arp
from netaddr import IPNetwork, IPAddress

IN_PORT = 0
OUT_PORT = 1
Ori_SIP = 2
Alt_SIP = 2.5
Ori_DIP = 3
Alt_DIP = 3.5
Ori_SPORT = 4
Alt_SPORT = 4.5
Ori_DPORT = 5
Alt_DPORT = 5.5
PROTO = 6
ETH_TYPE = 7

class DropIT(Exception):
    def __init__(self,myTup):
        self.myTup = myTup
        self.prioi = 5
    def drop(self,datapath,add_f,idle_t,buffer_id,parser):
        actions = []
	match = None
        if self.myTup[PROTO] is inet.IPPROTO_UDP:
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=17,udp_src=self.myTup[Ori_SPORT],udp_dst=self.myTup[Ori_DPORT],ipv4_src=self.myTup[Ori_SIP], ipv4_dst=self.myTup[Ori_DIP])
        elif self.myTup[PROTO] is inet.IPPROTO_TCP:
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=6,tcp_src=self.myTup[Ori_SPORT],tcp_dst=self.myTup[Ori_DPORT],ipv4_src=self.myTup[Ori_SIP], ipv4_dst=self.myTup[Ori_DIP])
        else:
	    return
        add_f(datapath, match=match, actions=actions, idle_timeout=idle_t, priority=self.prioi)
        out = parser.OFPPacketOut(datapath=datapath, buffer_id=buffer_id,in_port=self.myTup[IN_PORT], actions=actions)
        datapath.send_msg(out)  
class DelegateToL2(Exception):
    pass
class Done(Exception):
    pass
nat_instance_name = 'nat_instance_api_app'
class SNAT(app_manager.RyuApp):
    OFP_VERSIONS = [ofproto_v1_3.OFP_VERSION]

    def __init__(self, *args, **kwargs):
        super(SNAT, self).__init__(*args, **kwargs)
        self.fw_prioity_count = 300
        self.NAT_PRIOITY = 10
        self.IDLE_TIME = 2

	self.added = []
        self.IP_TO_PORT = {}
        self.IP_TO_MAC = {}
        self.MAC_TO_PORT = {}
        self.FAKE_IP = '10.0.0.1'
        self.PRI_IP_RANGE = '10.0.0.0/24'
        self.FAKE_MAC = '00:00:00:00:00:87'
	self.IP_TO_MAC[self.FAKE_IP] = self.FAKE_MAC
        self.port_pool = self.InitPort_Pool()
	self.Debug = self.logger.info
	self.BlackList = self.FWruleFromFile()
	self.mapper, self.DNAT_Set = self.DNATruleFromFile()
    def FWruleFromFile(self):
	ans = set()
	with open("FW") as f:
	    for l in f:
		tmp = l.split(',')
		ans.add((tmp[0],self.FAKE_IP,int(tmp[1])))
	return ans
    def DNATruleFromFile(self):
	ans = {}
	ans2 = set()
	with open("DNAT") as f:
	    for l in f:
		tmp = l.split(',')
		ans2.add((tmp[0],int(tmp[1])))
		ans[(tmp[0],int(tmp[1]))] = (tmp[2],int(tmp[3]))
		ans[(tmp[2],int(tmp[3]))] = (tmp[0],int(tmp[1]))
	return [ans,ans2]
    def InitPort_Pool(self):
        from random import shuffle
        x = range(2000, 65536)
        shuffle(x)
        return x   
    def add_flow(self, datapath, priority, match, actions, idle_timeout,
                 buffer_id=None):
	self.Debug("add_flow")
        ofproto = datapath.ofproto
        parser = datapath.ofproto_parser

        inst = [parser.OFPInstructionActions(ofproto.OFPIT_APPLY_ACTIONS,
                                             actions)]
        if buffer_id:
            mod = parser.OFPFlowMod(datapath=datapath, buffer_id=buffer_id,
                                    priority=priority, match=match,
				    idle_timeout=idle_timeout,
                                    instructions=inst)
        else:
            mod = parser.OFPFlowMod(datapath=datapath, priority=priority,
				    idle_timeout=idle_timeout,
                                    match=match, instructions=inst)
        datapath.send_msg(mod)

        #inst = [parser.OFPInstructionActions(ofproto.OFPIT_APPLY_ACTIONS,
        #                                     actions)]
        #if buffer_id:
        #    mod = parser.OFPFlowMod(datapath=datapath,
        #                            idle_timeout=idle_timeout,
        #                            buffer_id=buffer_id,
        #                            priority=priority,
        #                            flags=ofproto.OFPFF_SEND_FLOW_REM,
        #                            match=match,
        #                            instructions=inst)
        #else:
        #    mod = parser.OFPFlowMod(datapath=datapath,
        #                            idle_timeout=idle_timeout,
        #                            priority=priority,
        #                            flags=ofproto.OFPFF_SEND_FLOW_REM,
        #                            match=match,
        #                            instructions=inst)
        #datapath.send_msg(mod)
    def GeneratePort(self):
	self.Debug("GeneratePort")
        return self.port_pool.pop(0)
    def AddPortToPool(self,port):
	self.Debug("AddPortToPool")
        self.port_pool.append(port)
    def UpdateDNAT(self):
	mapper, dnat_set = self.DNATruleFromFile()
	self.mapper = mapper
	#TODO: future work => support delete not exist rule
    def Defake(self,ip,port):
	self.Debug("Defake")
	self.UpdateDNAT()
        return self.mapper.get((ip,port))
    #@set_ev_cls(ofp_event.EventOFPFlowRemoved, MAIN_DISPATCHER)
    #def flow_removed_handler(self, ev):
        #msg = ev.msg
        #TODO: futrue work
        #port = msg.match.get('tcp_dst') or msg.match.get('udp_dst')
        #if port and port not in range(2000, 65536) and port not in self.port_pool:
        #    print '[*] Available port %d' % tcp_port
        #    self.AddPortToPool(port)
        #else:
        #    pass
    def IsNeedPretend(self,ip):
	return ip[-1] == '1' and ip[-2] == '.'
    def Handle_ARP(self,pkt_arp,datapath):
	self.Debug("Handle_ARP")
        self.IP_TO_MAC[pkt_arp.src_ip] = pkt_arp.src_mac
        self.IP_TO_PORT[pkt_arp.src_ip] = self.MAC_TO_PORT[pkt_arp.src_mac]
        if pkt_arp.opcode is arp.ARP_REQUEST and self.IsNeedPretend(pkt_arp.dst_ip):
            actions = [datapath.ofproto_parser.OFPActionOutput(self.MAC_TO_PORT[pkt_arp.src_mac])]
            ARP_Reply = packet.Packet()
            ARP_Reply.add_protocol(ethernet.ethernet(
                ethertype=ether.ETH_TYPE_ARP,
                dst=pkt_arp.src_mac,
                src=self.IP_TO_MAC[self.FAKE_IP]))
            ARP_Reply.add_protocol(arp.arp(
                opcode=arp.ARP_REPLY,
                src_mac=self.IP_TO_MAC[self.FAKE_IP],
                src_ip=pkt_arp.dst_ip,
                dst_mac=pkt_arp.src_mac,
                dst_ip=pkt_arp.src_ip))

            ARP_Reply.serialize()
            out = datapath.ofproto_parser.OFPPacketOut(
                datapath=datapath,
                buffer_id=datapath.ofproto.OFP_NO_BUFFER,
                in_port=datapath.ofproto.OFPP_CONTROLLER,
                actions=actions, data=ARP_Reply.data)
            datapath.send_msg(out)
        else:
            return
    def Handle_L2(self,pkt,datapath):
	#self.Debug("Handle_L2")
        pkt_arp = pkt.get_protocol(arp.arp)
        if pkt_arp:
            self.Handle_ARP(pkt_arp,datapath)
        else:
            #L2_Switch(pkt)
            pass
    def GenerateRules(self,myTup,buffer_id,datapath,data,parser,ofproto):
	self.Debug("GenerateRules")
        SetField = parser.OFPActionSetField
        OutPkt = parser.OFPActionOutput
        
	match = None
        actions = [SetField(eth_src=self.IP_TO_MAC[myTup[Alt_SIP]]),SetField(eth_dst=self.IP_TO_MAC[myTup[Alt_DIP]]),
                SetField(ipv4_src=myTup[Alt_SIP]),SetField(ipv4_dst=myTup[Alt_DIP])]
        if myTup[PROTO] is inet.IPPROTO_UDP:
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=17,udp_src=myTup[Ori_SPORT],udp_dst=myTup[Ori_DPORT],ipv4_src=myTup[Ori_SIP], ipv4_dst=myTup[Ori_DIP])
            actions.append(SetField(udp_src=myTup[Alt_SPORT]))
            actions.append(SetField(udp_dst=myTup[Alt_DPORT]))
        elif myTup[PROTO] is inet.IPPROTO_TCP:
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=6,tcp_src=myTup[Ori_SPORT],tcp_dst=myTup[Ori_DPORT],ipv4_src=myTup[Ori_SIP], ipv4_dst=myTup[Ori_DIP])
            actions.append(SetField(tcp_src=myTup[Alt_SPORT]))
            actions.append(SetField(tcp_dst=myTup[Alt_DPORT]))
        else:
	    return
        
        actions.append(OutPkt(myTup[OUT_PORT]))
	if myTup not in self.added:
            self.add_flow(datapath, match=match, actions=actions,
                        idle_timeout=self.IDLE_TIME, priority=self.NAT_PRIOITY)
	    self.added.append(myTup)
        d = None
        if buffer_id == ofproto.OFP_NO_BUFFER:
            d = data
        out = parser.OFPPacketOut(datapath=datapath, buffer_id=buffer_id,in_port=myTup[IN_PORT], actions=actions, data=d)
        datapath.send_msg(out)
    def ClearRemovedFWRule(self,toDel,datapath):
	for tup in toDel:	
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=17,udp_dst=tup[2],ipv4_src=tup[0], ipv4_dst=tup[1])
	    mod = parser.OFPFlowMod(datapath=datapath, cookie=0,out_port=ofproto.OFPP_ANY,out_group=ofproto.OFPG_ANY,match=match,command=ofproto.OFPFC_DELETE)
	    datapath.send_msg(msg)
            match = parser.OFPMatch(eth_type=0x0800,ip_proto=6,tcp_dst=tup[2],ipv4_src=tup[0], ipv4_dst=tup[1])
	    mod = parser.OFPFlowMod(datapath=datapath, cookie=0,out_port=ofproto.OFPP_ANY,out_group=ofproto.OFPG_ANY,match=match,command=ofproto.OFPFC_DELETE)
	    datapath.send_msg(msg)
    def UpdateBL(self,datapath):
	#bl = self.FWruleFromFile()
	#toDel = self.BlackList - bl
	#self.ClearRemovedFWRule(toDel,datapath)
	self.BlackList = self.FWruleFromFile()
    def InBlackList(self,myTup,datapath):
	self.UpdateBL(datapath)
	theTup = (myTup[Ori_SIP],myTup[Ori_DIP],myTup[Ori_DPORT])
	return theTup in self.BlackList
    @set_ev_cls(ofp_event.EventOFPPacketIn, MAIN_DISPATCHER)
    def _packet_in_handler(self, ev):
	self.Debug("PktIn")
        msg = ev.msg
        datapath = msg.datapath
        ofproto, parser = datapath.ofproto, datapath.ofproto_parser
        pkt = packet.Packet(msg.data)
        eth_pkt = pkt.get_protocol(ethernet.ethernet)
        self.MAC_TO_PORT[eth_pkt.src] = msg.match['in_port']
        try:
	    myTup = self.GenerateMyTup(pkt,datapath) 
	    if self.InBlackList(myTup,datapath):
	        self.Debug("**blacklist**")
		raise DropIT(myTup)
	    else:
	        myTup = self.Rewrite(myTup,ofproto,datapath) 
                self.GenerateRules(myTup,msg.buffer_id,datapath,msg.data,parser,ofproto) 
        except Done:
            pass
        except DelegateToL2:
            self.Handle_L2(pkt,datapath)
        except DropIT as e:
            e.drop(datapath,self.add_flow,self.IDLE_TIME,msg.buffer_id,parser)
    def IsFakeIP(self,ip):
        ans = ip == self.FAKE_IP
	self.Debug("IsFakeIP %s %s %s",ip,self.FAKE_IP,ans)
	return ans
    def IsFromEx(self,ip):
	self.Debug("IsFromEx")
        return not self.IsFromIn(ip)
    def IsFromIn(self,ip):
	self.Debug("IsFromIn")
        return IPAddress(ip) in IPNetwork(self.PRI_IP_RANGE)
    def ARP_REQ(self,theIP,datapath,ofproto):
	self.Debug("ARP_REQ %s",theIP)
        actions = [datapath.ofproto_parser.OFPActionOutput(ofproto.OFPP_FLOOD)]
        ARP_Reply = packet.Packet()
        ARP_Reply.add_protocol(ethernet.ethernet(
            ethertype=ether.ETH_TYPE_ARP,
            dst='ff:ff:ff:ff:ff:ff',
            src=self.FAKE_MAC))
        ARP_Reply.add_protocol(arp.arp(
            opcode=arp.ARP_REQUEST,
            src_mac=self.FAKE_MAC,
            src_ip=self.FAKE_IP,
            dst_mac='ff:ff:ff:ff:ff:ff',
            dst_ip=theIP))

        ARP_Reply.serialize()
        out = datapath.ofproto_parser.OFPPacketOut(
            datapath=datapath,
            buffer_id=datapath.ofproto.OFP_NO_BUFFER,
            in_port=datapath.ofproto.OFPP_CONTROLLER,
            actions=actions, data=ARP_Reply.data)
        datapath.send_msg(out)

    def Rewrite(self,myTup,ofproto,datapath):
	self.Debug("Rewrite")
        newTup = myTup
        if self.IsFakeIP(myTup[Ori_DIP]) and self.IsFromEx(myTup[Ori_SIP]):
            pairORnone = self.Defake(myTup[Ori_DIP], myTup[Ori_DPORT])
            if pairORnone:
                newTup[Alt_DIP], newTup[Alt_DPORT] = pairORnone
            else:
                raise DropIT(myTup)
            newTup[Alt_SIP], newTup[Alt_SPORT] = myTup[Ori_SIP], myTup[Ori_SPORT]
        elif self.IsFromEx(myTup[Ori_DIP]) and self.IsFromIn(myTup[Ori_SIP]):
            newTup[Alt_SIP], newTup[Alt_SPORT] = self.Fake(myTup[Ori_SIP], myTup[Ori_SPORT])
            newTup[Alt_DIP], newTup[Alt_DPORT] = myTup[Ori_DIP], myTup[Ori_DPORT]
        elif self.IsFromIn(myTup[Ori_SIP]) and self.IsFromIn(myTup[Ori_DIP]):
            raise DelegateToL2()
        else:
            raise DropIT(myTup)
	if newTup[Alt_DIP] in self.IP_TO_PORT:
            newTup[OUT_PORT] = self.IP_TO_PORT[newTup[Alt_DIP]]
	else:
	    self.ARP_REQ(newTup[Alt_DIP],datapath,ofproto)
	    newTup[OUT_PORT] = ofproto.OFPP_FLOOD
        return myTup
    def GenerateMyTup(self,pkt,datapath):
	self.Debug("GenerateMyTup")
        pkt_ip = pkt.get_protocol(ipv4.ipv4)
        if pkt_ip:
            myTup = {}
            myTup[IN_PORT] = self.IP_TO_PORT[pkt_ip.src]
            myTup[Ori_SIP] = pkt_ip.src
            myTup[Ori_DIP] = pkt_ip.dst
            myTup[ETH_TYPE] = ether.ETH_TYPE_IP
            pkt_udp = pkt.get_protocol(udp.udp)
            if pkt_udp:
                myTup[Ori_SPORT] = pkt_udp.src_port
                myTup[Ori_DPORT] = pkt_udp.dst_port
                myTup[PROTO] = inet.IPPROTO_UDP
                return myTup
            pkt_tcp = pkt.get_protocol(tcp.tcp)
            if pkt_tcp:
                myTup[Ori_SPORT] = pkt_tcp.src_port
                myTup[Ori_DPORT] = pkt_tcp.dst_port
                myTup[PROTO] = inet.IPPROTO_TCP
                return myTup
            raise DropIT(myTup)
        else:
            self.Handle_L2(pkt,datapath)
            raise Done()

    def Fake(self,ip,port):
	self.Debug("Fake")
        oldPair = (ip,port)
        if oldPair not in self.mapper:
            newPair = (self.FAKE_IP, self.GeneratePort())
            self.mapper[oldPair] = newPair
            self.mapper[newPair] = oldPair
            return newPair
        else:
            return self.mapper[oldPair]
