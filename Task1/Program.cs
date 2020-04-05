using System;
using System.IO;
using System.Diagnostics;

namespace Task1 {
    class Program {
        static int Main(string[] args) {
            if(args.Length < 1) {
                return 1;
            }

            var lines = File.ReadAllLines(args[0]);

            var numbers = new int[lines.Length];
            for(var i = 0; i < lines.Length; i += 1) {
                numbers[i] = int.Parse(lines[i]);
            }

            var insertionStopwatch = new Stopwatch();
            var insertionNumbers = (int[])numbers.Clone();

            insertionStopwatch.Start();
            // Implementation based on https://en.wikipedia.org/wiki/Insertion_sort
            for(var i = 1; i < insertionNumbers.Length; i += 1) {
                var pivotValue = insertionNumbers[i];

                for(var j = 0; j <= i; j += 1) {
                    if(j == i || insertionNumbers[i - j - 1] <= pivotValue) {
                        insertionNumbers[i - j] = pivotValue;

                        break;
                    } else {
                        insertionNumbers[i - j] = insertionNumbers[i - j - 1];
                    }
                }
            }
            insertionStopwatch.Stop();

            for(var i = 1; i < insertionNumbers.Length; i += 1) {
                if(insertionNumbers[i] < insertionNumbers[i - 1]) {
                    throw new Exception($"Insertion sort failed at {i}");
                }
            }

            var shellStopwatch = new Stopwatch();
            var shellNumbers = (int[])numbers.Clone();
            var shellGaps = new int[] { 1750, 701, 301, 132, 57, 23, 10, 4, 1 }; // https://oeis.org/A102549

            shellStopwatch.Start();
            // Implementation based on https://en.wikipedia.org/wiki/Shellsort
            foreach(var gap in shellGaps) {
                for(var i = gap; i < shellNumbers.Length; i += 1) {
                    var pivotValue = shellNumbers[i];

                    for(var j = 0; j <= i - gap + 1; j += gap) {
                        if(j >= i - gap + 1 || shellNumbers[i - j - gap] <= pivotValue) {
                            shellNumbers[i - j] = pivotValue;

                            break;
                        } else {
                            shellNumbers[i - j] = shellNumbers[i - j - gap];
                        }
                    }
                }
            }
            shellStopwatch.Stop();

            for(var i = 1; i < shellNumbers.Length; i += 1) {
                if(shellNumbers[i] < shellNumbers[i - 1]) {
                    throw new Exception($"Shellsort failed at {i}");
                }
            }

            var insertionTime = insertionStopwatch.Elapsed;
            var shellTime = shellStopwatch.Elapsed;

            var insertionMillisPerElement = insertionTime.TotalMilliseconds / insertionNumbers.Length;
            var shellMillisPerElement = shellTime.TotalMilliseconds / shellNumbers.Length;

            Console.WriteLine($"Insert Sort: Time: {insertionTime.TotalSeconds}s, Time Per Element: {insertionMillisPerElement}ms");
            Console.WriteLine($"Shellsort: Time: {shellTime.TotalSeconds}s, Time Per Element: {shellMillisPerElement}ms");

            return 0;
        }
    }
}