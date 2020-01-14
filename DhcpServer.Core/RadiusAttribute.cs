// <copyright file="RadiusAttribute.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a RADIUS attribute.
    /// </summary>
    public readonly struct RadiusAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttribute"/> struct.
        /// </summary>
        /// <param name="type">The attribute type.</param>
        /// <param name="data">The attribute data.</param>
        public RadiusAttribute(RadiusAttributeType type, Memory<byte> data)
        {
            this.Type = type;
            this.Data = data;
        }

        /// <summary>
        /// Gets the attribute type.
        /// </summary>
        public RadiusAttributeType Type { get; }

        /// <summary>
        /// Gets the attribute data.
        /// </summary>
        public Memory<byte> Data { get; }

        /// <summary>
        /// Represents a sequence of RADIUS attributes.
        /// </summary>
        public readonly struct Sequence
        {
            private readonly DhcpSubOption subOption;

            /// <summary>
            /// Initializes a new instance of the <see cref="Sequence"/> struct.
            /// </summary>
            /// <param name="subOption">The RADIUS attributes sub-option.</param>
            public Sequence(DhcpSubOption subOption)
            {
                this.subOption = subOption;
            }

            /// <summary>
            /// Gets an enumerator which reads RADIUS attributes in sequential order.
            /// </summary>
            /// <returns>The RADIUS attributes enumerator.</returns>
            public Enumerator GetEnumerator() => new Enumerator(this.subOption.Data);
        }

        /// <summary>
        /// An enumerator which reads RADIUS attributes in sequential order.
        /// </summary>
        public struct Enumerator
        {
            private readonly Memory<byte> data;

            private RadiusAttribute current;
            private int pos;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="data">The RADIUS attributes sub-option data.</param>
            public Enumerator(Memory<byte> data)
            {
                this.data = data;
                this.current = default;
                this.pos = 0;
            }

            /// <summary>
            /// Gets the <see cref="RadiusAttribute"/> at the current position of the enumerator.
            /// </summary>
            public RadiusAttribute Current => this.current;

            /// <summary>
            /// Advances the enumerator to the next <see cref="RadiusAttribute"/> element.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element;
            /// <c>false</c> if the enumerator has passed the end.</returns>
            public bool MoveNext()
            {
                int end = this.data.Length;
                int i = this.pos;
                if (i < end)
                {
                    Span<byte> span = this.data.Span;
                    RadiusAttributeType type = (RadiusAttributeType)span[i++];
                    byte length = (byte)(span[i++] - 2);
                    this.current = new RadiusAttribute(type, this.data.Slice(i, length));
                    this.pos = i + length;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
