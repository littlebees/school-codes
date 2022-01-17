#!/usr/bin/env python

from mininet.net import Mininet
from mininet.node import Controller, RemoteController
from mininet.cli import CLI
from mininet.log import setLogLevel, info
from mininet.link import Link, Intf, TCLink
from mininet.topo import Topo
from mininet.util import dumpNodeConnections
import logging
import os 

logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger( __name__ )
NODE_BASE = 4
CORE_BW = 1000
CORE_LOSS = 2
EDGE_BW = 100

class HugeTopo(Topo):
    logger.debug("Class HugeTopo")
    def __init__(self):
        logger.debug("Class HugeTopo init")
        self.CoreL = [None]*(NODE_BASE)
        self.AggL = [None]*(NODE_BASE*2)
        self.EdgeL = [None]*(NODE_BASE*2)
        self.HostL = [None]*(NODE_BASE*4)
        Topo.__init__(self)

    def GenerateDevices(self,slotsL):
        tmp = list(map(lambda mem:[mem,self.addSwitch],slotsL))
        tmp[-1][1]=self.addHost
        i = 0
        for [slots,makerF] in tmp:
            i += 1
            for j in xrange(len(slots)):
                name = '{}{:03d}'.format(i,j+1)
                slots[j] = makerF(name)
    def createTopo(self):    
        self.GenerateDevices([self.CoreL,self.AggL,self.EdgeL,self.HostL])
    def connectEachOther(self):
        LINK = self.addLink
        for i in xrange(2):
            for j in xrange(i,len(self.AggL), 2):
                LINK(self.CoreL[i*2],self.AggL[j], bw=CORE_BW, loss=CORE_LOSS)
                LINK(self.CoreL[i*2+1],self.AggL[j], bw=CORE_BW, loss=CORE_LOSS)
        for i in xrange(0,len(self.AggL), 2):
            for (a,b) in ((i,i),(i,i+1),(i+1,i),(i+1,i+1)):
                LINK(self.AggL[a],self.EdgeL[b], bw=EDGE_BW)
        for i in xrange(len(self.EdgeL)):
            for j in (2*i,2*i+1):
                LINK(self.EdgeL[i], self.HostL[j])

def iperfTest(net, topo):
    def AsServer(h):
        h.popen('iperf -s -u -i 1', shell=True)
    def MakeClient(h):
        def Test(ip):
            CMD = 'iperf -c {} -u -t 10 -i 1 -b 100m'.format(ip) 
            h.cmdPrint(CMD)
        return Test

    logger.debug("Start iperfTEST")
    h0, h3, h00 = net.get(topo.HostL[0], topo.HostL[15], topo.HostL[2])
    Test = MakeClient(h00)
    
    AsServer(h0)
    AsServer(h3)
    Test(h0.IP())
    Test(h3.IP())

def createTopo():
    logging.debug("Create HugeTopo")
    topo = HugeTopo()
    topo.createTopo() 
    topo.connectEachOther() 
    
    logging.debug("Start Mininet")
    CONTROLLER_IP = "127.0.0.1"
    net = Mininet(topo=topo, link=TCLink, controller=None)
    net.addController( 'controller',controller=RemoteController,ip=CONTROLLER_IP)
    net.start()

    logger.debug("dumpNode")
    dumpNodeConnections(net.hosts)
    
    net.pingFull()
    iperfTest(net, topo)

    CLI(net)
    net.stop()

if __name__ == '__main__':
    setLogLevel('info')
    if os.getuid() == 0:
        createTopo()
    else:
        logger.debug("You are NOT root")
