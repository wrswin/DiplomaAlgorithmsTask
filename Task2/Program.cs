using System;
using System.IO;
using System.Diagnostics;

namespace Task2 {
    class Program {
        private static int BinarySearchStep(int[] numbers, int searchNumber, int left, int right) {
            var center = (left + right) / 2;

            if(numbers[center] > searchNumber) {
                return BinarySearchStep(numbers, searchNumber, left, center);
            } else if(numbers[center] < searchNumber) {
                return BinarySearchStep(numbers, searchNumber, center, right);
            } else {
                return center;
            }
        }

        static int Main(string[] args) {
            if(args.Length < 1) {
                return 1;
            }

            var lines = File.ReadAllLines(args[0]);

            var numbers = new int[lines.Length];
            for(var i = 0; i < lines.Length; i += 1) {
                numbers[i] = int.Parse(lines[i]);
            }

            int runs = 100;

            var sortedNumbers = (int[])numbers.Clone();
            var gaps = new int[] { 1750, 701, 301, 132, 57, 23, 10, 4, 1 }; // https://oeis.org/A102549

            // Implementation based on https://en.wikipedia.org/wiki/Shellsort
            foreach(var gap in gaps) {
                for(var i = gap; i < sortedNumbers.Length; i += 1) {
                    var pivotValue = sortedNumbers[i];

                    var offset = 0;
                    while(offset < i - (gap - 1) && sortedNumbers[i - offset - gap] > pivotValue) {
                        sortedNumbers[i - offset] = sortedNumbers[i - offset - gap];

                        offset += gap;
                    }

                    sortedNumbers[i - offset] = pivotValue;
                }
            }

            var searchNumbers = new int[] { 575154, 182339, 17132, 773788, 296934, 991395, 303270, 45231, 580, 629822 };

            var linearStopwatch = new Stopwatch();
            var linearIndices = new int[searchNumbers.Length];

            linearStopwatch.Start();
            for(var k = 0; k < runs; k += 1) {
                for(var i = 0; i < searchNumbers.Length; i += 1) {
                    for(var j = 0; j < sortedNumbers.Length; j += 1) {
                        if(sortedNumbers[j] == searchNumbers[i]) {
                            linearIndices[i] = j;

                            break;
                        }
                    }
                }
            }
            linearStopwatch.Stop();

            var binaryStopwatch = new Stopwatch();
            var binaryIndices = new int[searchNumbers.Length];

            binaryStopwatch.Start();
            for(var j = 0; j < runs; j += 1) {
                for(var i = 0; i < searchNumbers.Length; i += 1) {
                    binaryIndices[i] = BinarySearchStep(sortedNumbers, searchNumbers[i], 0, sortedNumbers.Length);
                }
            }
            binaryStopwatch.Stop();

            var linearTime = linearStopwatch.Elapsed;
            var binaryTime = binaryStopwatch.Elapsed;

            var linearMillisPerElement = linearTime.TotalMilliseconds / sortedNumbers.Length / searchNumbers.Length / runs;
            var binaryMillisPerElement = binaryTime.TotalMilliseconds / sortedNumbers.Length / searchNumbers.Length / runs;

            var actualTimeRatio = linearTime.TotalSeconds / binaryTime.TotalSeconds;

            var worstCaseTimeComplexityRatio = sortedNumbers.Length / Math.Log(sortedNumbers.Length);
            var bestCaseTimeComplexityRatio = 1 / 1.0;
            var averageTimeComplexityRatio = sortedNumbers.Length / Math.Log(sortedNumbers.Length);

            Console.WriteLine($"Linear Search: Time: {linearTime.TotalSeconds}s, Time Per Element: {linearMillisPerElement}ms");
            Console.WriteLine($"Binary Search: Time: {binaryTime.TotalSeconds}s, Time Per Element: {binaryMillisPerElement}ms");

            Console.WriteLine($"Actual Time Ratio: {actualTimeRatio}");

            Console.WriteLine($"Worst Case Time Complexity Ratio (n / log n): {worstCaseTimeComplexityRatio}");
            Console.WriteLine($"Best Case Time Complexity Ratio (1 / 1): {bestCaseTimeComplexityRatio}");
            Console.WriteLine($"Average Time Complexity Ratio (n / log n): {averageTimeComplexityRatio}");

            return 0;
        }
    }
}