﻿using System;
using System.Text;
using Parsec.Extensions;

namespace Parsec.Shaiya.SDATA
{
    public class SDataHeader
    {
        /// <summary>
        /// Encryption signature. Read as char[40]
        /// </summary>
        public string Signature { get; set; }

        public uint Checksum { get; set; }

        public uint RealSize { get; set; }

        /// <summary>
        /// Read as char[16]
        /// </summary>
        public byte[] Padding { get; set; }

        public SDataHeader(string signature, uint checksum, uint realSize, byte[] padding)
        {
            Signature = signature;
            Checksum = checksum;
            RealSize = realSize;
            Padding = padding;
        }

        public SDataHeader(byte[] data)
        {
            Signature = Encoding.ASCII.GetString(data.SubArray(0, 40));
            Checksum = BitConverter.ToUInt32(data.SubArray(40, 4));
            RealSize = BitConverter.ToUInt32(data.SubArray(44, 4));
            Padding = data.SubArray(48, 16);
        }
    }
}