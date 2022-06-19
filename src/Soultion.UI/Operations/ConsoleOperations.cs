using Soultion.UI.Operations.Abstractions;
using System;
using System.Threading.Tasks;
using appConsole = System.Console;

namespace Soultion.UI.Operations
{
    /// <summary>
    /// Simplest not-sealed console interaction code.
    /// </summary>
    public class ConsoleOperations : IRegularConsoleOperations
    {
        public async Task Clear()
        {
            appConsole.Clear();
            await Task.CompletedTask;
        }

        /// <summary>Reads the key.</summary>
        /// <returns>ConsoleKeyInfo.</returns>
        public async Task<ConsoleKeyInfo> ReadKey()
        {
            await Task.CompletedTask;
            return appConsole.ReadKey();
        }

        /// <summary>Reads the line.</summary>
        /// <returns>System.String.</returns>
        public async Task<string> ReadLine()
        {
            await Task.CompletedTask;
            return appConsole.ReadLine();
        }

        /// <summary>Writes the specified value.</summary>
        /// <param name="value">The value.</param>
        public async Task Write(string value)
        {
            appConsole.Write(value);
            await Task.CompletedTask;
        }

        /// <summary>Writes the line.</summary>
        /// <param name="value">The value.</param>
        public async Task WriteLine(string value)
        {
            appConsole.WriteLine(value);
            await Task.CompletedTask;
        }
    }
}
