global using System;
global using System.Linq;
global using System.Reflection;
global using System.Collections.Generic;
global using System.IO;
global using System.Text.RegularExpressions;
global using System.Globalization;
global using aoc2021;

var day = DateTime.Now.Day;
if (args.Length == 1)
{
    day = int.Parse(args[0]);
}

var t = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => x.Name == $"Day{day:d2}")
    .Single();

var c = Activator.CreateInstance(t) as BaseDay;
if (c != null)
{
    c.SolvePart1();
    c.SolvePart2();
}