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

                var offset = 0;
                while(offset < i && insertionNumbers[i - offset - 1] > pivotValue) {
                    insertionNumbers[i - offset] = insertionNumbers[i - offset - 1];

                    offset += 1;
                }

                insertionNumbers[i - offset] = pivotValue;
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

                    var offset = 0;
                    while(offset < i - (gap - 1) && shellNumbers[i - offset - gap] > pivotValue) {
                        shellNumbers[i - offset] = shellNumbers[i - offset - gap];

                        offset += gap;
                    }

                    shellNumbers[i - offset] = pivotValue;
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