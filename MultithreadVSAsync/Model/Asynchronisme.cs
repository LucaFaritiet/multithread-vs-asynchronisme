using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadVSAsync.Model
{
    internal static class Asynchronisme 
    {
        public static async Task AsynchronismeMethod()
        {
            Console.WriteLine("=== Asynchronous Programming Demo ===\n");

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Example 1: Multiple async operations running concurrently
            Console.WriteLine("Starting multiple async operations...");
            
            Task task1 = DownloadDataAsync("https://jsonplaceholder.typicode.com/posts/1", 1);
            Task task2 = DownloadDataAsync("https://jsonplaceholder.typicode.com/posts/2", 2);
            Task task3 = DownloadDataAsync("https://jsonplaceholder.typicode.com/posts/3", 3);

            // Wait for all tasks to complete
            await Task.WhenAll(task1, task2, task3);

            stopwatch.Stop();
            Console.WriteLine($"\nAll async operations completed in {stopwatch.ElapsedMilliseconds}ms");

            // Example 2: Sequential async operations
            Console.WriteLine("\n--- Sequential async example ---");
            await ProcessOrderAsync(101);

            // Example 3: Async with cancellation token
            Console.WriteLine("\n--- Async with cancellation example ---");
            await LongRunningOperationWithCancellationAsync();

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private static async Task DownloadDataAsync(string url, int taskNumber)
        {
            Console.WriteLine($"[Task {taskNumber}] Starting download from {url}");
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string result = await client.GetStringAsync(url);
                    Console.WriteLine($"[Task {taskNumber}] Downloaded {result.Length} characters");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Task {taskNumber}] Error: {ex.Message}");
            }
        }

        private static async Task ProcessOrderAsync(int orderId)
        {
            Console.WriteLine($"Processing order {orderId}...");
            
            await ValidateOrderAsync(orderId);
            await ChargePaymentAsync(orderId);
            await SendConfirmationEmailAsync(orderId);
            
            Console.WriteLine($"Order {orderId} processed successfully!");
        }

        private static async Task ValidateOrderAsync(int orderId)
        {
            Console.WriteLine($"  Validating order {orderId}...");
            await Task.Delay(1000); // Simulate async I/O operation
            Console.WriteLine($"  Order {orderId} validated");
        }

        private static async Task ChargePaymentAsync(int orderId)
        {
            Console.WriteLine($"  Charging payment for order {orderId}...");
            await Task.Delay(1500); // Simulate async I/O operation
            Console.WriteLine($"  Payment charged for order {orderId}");
        }

        private static async Task SendConfirmationEmailAsync(int orderId)
        {
            Console.WriteLine($"  Sending confirmation email for order {orderId}...");
            await Task.Delay(800); // Simulate async I/O operation
            Console.WriteLine($"  Confirmation email sent for order {orderId}");
        }

        private static async Task LongRunningOperationWithCancellationAsync()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Auto-cancel after 3 seconds
                cts.CancelAfter(3000);

                try
                {
                    await SimulateLongTaskAsync(cts.Token);
                    Console.WriteLine("Long task completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Long task was cancelled");
                }
            }
        }

        private static async Task SimulateLongTaskAsync(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"  Step {i}/10 of long task...");
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}
