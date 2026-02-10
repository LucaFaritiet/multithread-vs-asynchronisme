using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MultithreadVSAsync.Model
{
    internal static class Multithread
    {
        private static readonly object lockObject = new object();
        private static int sharedCounter = 0;

        public static void MultithreadMethod()
        {
            Console.WriteLine("=== Multithreading Demo ===\n");

            // Example 1: Basic thread creation and execution
            Console.WriteLine("--- Example 1: Basic Threading ---");
            BasicThreadingExample();

            // Example 2: Multiple threads working on the same data
            Console.WriteLine("\n--- Example 2: Multiple Threads with Shared Data ---");
            SharedDataExample();

            // Example 3: Thread pool usage
            Console.WriteLine("\n--- Example 3: Thread Pool ---");
            ThreadPoolExample();

            // Example 4: Thread synchronization with lock
            Console.WriteLine("\n--- Example 4: Thread Synchronization ---");
            ThreadSynchronizationExample();

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private static void BasicThreadingExample()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Thread thread1 = new Thread(() => PerformTask("Thread 1", 2000));
            Thread thread2 = new Thread(() => PerformTask("Thread 2", 1500));
            Thread thread3 = new Thread(() => PerformTask("Thread 3", 1000));

            thread1.Start();
            thread2.Start();
            thread3.Start();

            // Wait for all threads to complete
            thread1.Join();
            thread2.Join();
            thread3.Join();

            stopwatch.Stop();
            Console.WriteLine($"All threads completed in {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void PerformTask(string threadName, int duration)
        {
            Console.WriteLine($"[{threadName}] Started on thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(duration); // Simulate CPU-bound work
            Console.WriteLine($"[{threadName}] Completed after {duration}ms");
        }

        private static void SharedDataExample()
        {
            List<int> results = new List<int>();
            Thread[] threads = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                int taskNumber = i + 1;
                threads[i] = new Thread(() => ProcessData(taskNumber, results));
                threads[i].Start();
            }

            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine($"Total results collected: {results.Count}");
        }

        private static void ProcessData(int taskNumber, List<int> results)
        {
            Console.WriteLine($"[Task {taskNumber}] Processing on thread {Thread.CurrentThread.ManagedThreadId}");
            
            // Simulate some computation
            int result = taskNumber * taskNumber;
            Thread.Sleep(500);

            lock (results) // Ensure thread-safe access to shared list
            {
                results.Add(result);
                Console.WriteLine($"[Task {taskNumber}] Added result: {result}");
            }
        }

        private static void ThreadPoolExample()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int completedTasks = 0;
            ManualResetEvent allDone = new ManualResetEvent(false);

            for (int i = 1; i <= 10; i++)
            {
                int taskNumber = i;
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Console.WriteLine($"[Pool Task {taskNumber}] Running on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(300); // Simulate work
                    
                    Interlocked.Increment(ref completedTasks);
                    
                    if (completedTasks == 10)
                    {
                        allDone.Set();
                    }
                });
            }

            allDone.WaitOne(); // Wait for all tasks to complete
            stopwatch.Stop();
            Console.WriteLine($"All thread pool tasks completed in {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void ThreadSynchronizationExample()
        {
            sharedCounter = 0;
            Thread[] threads = new Thread[10];

            // Create threads that increment a shared counter
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(IncrementCounter);
                threads[i].Start();
            }

            // Wait for all threads
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine($"Final counter value: {sharedCounter} (expected: 1000)");
        }

        private static void IncrementCounter()
        {
            for (int i = 0; i < 100; i++)
            {
                lock (lockObject) // Critical section - prevent race conditions
                {
                    sharedCounter++;
                }
            }
        }
    }
}
