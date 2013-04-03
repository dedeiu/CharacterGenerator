using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            CharacterSet charSet = new CharacterSet();
            charSet.CharLength = 8;
            charSet.CharNumber = 60000000;
            charSet.Generate();
        }
    }
}
