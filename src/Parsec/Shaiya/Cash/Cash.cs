using System.Collections.Generic;
using Newtonsoft.Json;
using Parsec.Shaiya.Core;

namespace Parsec.Shaiya.CASH
{
    public class Cash : FileBase
    { 
        [JsonIgnore]
        public int Total { get; set; }
        public List<Product> Products { get; } = new();

        public Cash(string path) : base(path)
        {
        }

        public override void Read()
        {
            Total = _binaryReader.Read<int>();

            for (int i = 0; i < Total; i++)
            {
                var product = new Product(_binaryReader);
                Products.Add(product);
            }
        }
    }
}
