﻿using Parsec.Shaiya;

namespace Parsec.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var killStatus = new KillStatus("KillStatus.SData");
            killStatus.Read();
            killStatus.CreateFriendlyDump("KillStatus.dump.txt");
        }
    }
}