// <copyright file="DhcpOptionTag.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP option tag, as defined in
    /// <see href="https://www.iana.org/assignments/bootp-dhcp-parameters/bootp-dhcp-parameters.xhtml#options"/> .
    /// </summary>
    public enum DhcpOptionTag : byte
    {
        /// <summary>
        /// Zero-value options buffer padding.
        /// </summary>
        Pad = 0,

        /// <summary>
        /// Subnet Mask Value
        /// </summary>
        SubnetMask = 1,

        /// <summary>
        /// Router addresses
        /// </summary>
        Router = 3,

        /// <summary>
        /// Time server addresses
        /// </summary>
        TimeServer = 4,

        /// <summary>
        /// IEN-116 server addresses
        /// </summary>
        NameServer = 5,

        /// <summary>
        /// DNS server addresses
        /// </summary>
        DomainServer = 6,

        /// <summary>
        /// MIT-LCS UDP log server addresses
        /// </summary>
        LogServer = 7,

        /// <summary>
        /// Quote of the Day server addresses
        /// </summary>
        QuotesServer = 8,

        /// <summary>
        /// Line printer server addresses
        /// </summary>
        LprServer = 9,

        /// <summary>
        /// Imagen Impress server addresses
        /// </summary>
        ImpressServer = 10,

        /// <summary>
        /// Resource Location server addresses
        /// </summary>
        RlpServer = 11,

        /// <summary>
        /// Host name string
        /// </summary>
        HostName = 12,

        /// <summary>
        /// Size of boot file in 512 byte chunks
        /// </summary>
        BootFileSize = 13,

        /// <summary>
        /// Client to dump and name the file to dump it to
        /// </summary>
        MeritDumpFile = 14,

        /// <summary>
        /// The DNS domain name of the client
        /// </summary>
        DomainName = 15,

        /// <summary>
        /// Swap server address
        /// </summary>
        SwapServer = 16,

        /// <summary>
        /// Path name for root disk
        /// </summary>
        RootPath = 17,

        /// <summary>
        /// Path name for more BOOTP info
        /// </summary>
        ExtensionFile = 18,

        /// <summary>
        /// Enable/disable IP forwarding
        /// </summary>
        Forward = 19,

        /// <summary>
        /// Enable/Disable source routing
        /// </summary>
        SrcRte = 20,

        /// <summary>
        /// Routing policy filters
        /// </summary>
        PolicyFilter = 21,

        /// <summary>
        /// Max datagram reassembly size
        /// </summary>
        MaxDGAssembly = 22,

        /// <summary>
        /// Default IP time to live
        /// </summary>
        DefaultIPTtl = 23,

        /// <summary>
        /// Path MTU aging timeout
        /// </summary>
        MtuTimeout = 24,

        /// <summary>
        /// Path MTU plateau table
        /// </summary>
        MtuPlateau = 25,

        /// <summary>
        /// Interface MTU size
        /// </summary>
        MtuInterface = 26,

        /// <summary>
        /// All subnets are local
        /// </summary>
        MtuSubnet = 27,

        /// <summary>
        /// Broadcast address
        /// </summary>
        BroadcastAddress = 28,

        /// <summary>
        /// Perform mask discovery
        /// </summary>
        MaskDiscovery = 29,

        /// <summary>
        /// Provide mask to others
        /// </summary>
        MaskSupplier = 30,

        /// <summary>
        /// Perform router discovery
        /// </summary>
        RouterDiscovery = 31,

        /// <summary>
        /// Router solicitation address
        /// </summary>
        RouterRequest = 32,

        /// <summary>
        /// Static routing table
        /// </summary>
        StaticRoute = 33,

        /// <summary>
        /// Trailer encapsulation
        /// </summary>
        Trailers = 34,

        /// <summary>
        /// ARP cache timeout
        /// </summary>
        ArpTimeout = 35,

        /// <summary>
        /// Ethernet encapsulation
        /// </summary>
        Ethernet = 36,

        /// <summary>
        /// Default TCP time to live
        /// </summary>
        DefaultTcpTtl = 37,

        /// <summary>
        /// TCP keepalive interval
        /// </summary>
        KeepaliveTime = 38,

        /// <summary>
        /// TCP keepalive garbage
        /// </summary>
        KeepaliveData = 39,

        /// <summary>
        /// NIS domain name
        /// </summary>
        NisDomain = 40,

        /// <summary>
        /// NIS domain name
        /// </summary>
        NisServers = 41,

        /// <summary>
        /// NTP Server Addresses
        /// </summary>
        NtpServers = 42,

        /// <summary>
        /// Vendor specific information
        /// </summary>
        VendorSpecific = 43,

        /// <summary>
        /// NetBIOS name servers
        /// </summary>
        NetBiosNameSrv = 44,

        /// <summary>
        /// NetBIOS datagram distribution servers
        /// </summary>
        NetBiosDistSrv = 45,

        /// <summary>
        /// NetBIOS node type
        /// </summary>
        NetBiosNodeType = 46,

        /// <summary>
        /// NetBIOS scope
        /// </summary>
        NetBiosScope = 47,

        /// <summary>
        /// X Window font server
        /// </summary>
        XWindowFont = 48,

        /// <summary>
        /// X Window display manager
        /// </summary>
        XWindowManager = 49,

        /// <summary>
        /// Requested IP address
        /// </summary>
        AddressRequest = 50,

        /// <summary>
        /// IP address lease time
        /// </summary>
        AddressTime = 51,

        /// <summary>
        /// Overload "sname" or "file"
        /// </summary>
        Overload = 52,

        /// <summary>
        /// DHCP message type
        /// </summary>
        DhcpMsgType = 53,

        /// <summary>
        /// DHCP server identification
        /// </summary>
        DhcpServerId = 54,

        /// <summary>
        /// Parameter Request List
        /// </summary>
        ParameterList = 55,

        /// <summary>
        /// DHCP Error Message
        /// </summary>
        DhcpMessage = 56,

        /// <summary>
        /// DHCP Maximum Message Size
        /// </summary>
        DhcpMaxMsgSize = 57,

        /// <summary>
        /// Client Identifier
        /// </summary>
        ClientId = 61,

        /// <summary>
        /// Relay Agent Information
        /// </summary>
        RelayAgentInformation = 82,

        /// <summary>
        /// Marks the end of an options buffer.
        /// </summary>
        End = 255,
    }
}
