// <copyright file="DhcpMessageType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP message type, as defined in
    /// <see href="https://www.iana.org/assignments/bootp-dhcp-parameters/bootp-dhcp-parameters.xhtml#message-type-53"/> .
    /// </summary>
    public enum DhcpMessageType : byte
    {
        /// <summary>
        /// An unspecified message type.
        /// </summary>
        None = 0,

        /// <summary>
        /// A DHCPDISCOVER message.
        /// </summary>
        Discover = 1,

        /// <summary>
        /// A DHCPOFFER message.
        /// </summary>
        Offer = 2,

        /// <summary>
        /// A DHCPREQUEST message.
        /// </summary>
        Request = 3,

        /// <summary>
        /// A DHCPDECLINE message.
        /// </summary>
        Decline = 4,

        /// <summary>
        /// A DHCPACK message.
        /// </summary>
        Ack = 5,

        /// <summary>
        /// A DHCPNAK message.
        /// </summary>
        Nak = 6,

        /// <summary>
        /// A DHCPRELEASE message.
        /// </summary>
        Release = 7,

        /// <summary>
        /// A DHCPINFORM message.
        /// </summary>
        Inform = 8,

        /// <summary>
        /// A DHCPFORCERENEW message.
        /// </summary>
        ForceRenew = 9,

        /// <summary>
        /// A DHCPLEASEQUERY message.
        /// </summary>
        LeaseQuery = 10,

        /// <summary>
        /// A DHCPLEASEUNASSIGNED message.
        /// </summary>
        LeaseUnassigned = 11,

        /// <summary>
        /// A DHCPLEASEUNKNOWN message.
        /// </summary>
        LeaseUnknown = 12,

        /// <summary>
        /// A DHCPLEASEACTIVE message.
        /// </summary>
        LeaseActive = 13,

        /// <summary>
        /// A DHCPBULKLEASEQUERY message.
        /// </summary>
        BulkLeaseQuery = 14,

        /// <summary>
        /// A DHCPLEASEQUERYDONE message.
        /// </summary>
        LeaseQueryDone = 15,

        /// <summary>
        /// A DHCPACTIVELEASEQUERY message.
        /// </summary>
        ActiveLeaseQuery = 16,

        /// <summary>
        /// A DHCPLEASEQUERYSTATUS message.
        /// </summary>
        LeaseQueryStatus = 17,

        /// <summary>
        /// A DHCPTLS message.
        /// </summary>
        Tls = 18,
    }
}
