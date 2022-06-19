using System;
using System.Threading.Tasks;

namespace Soultion.UI.Operations.Abstractions
{
    /// <summary>
    /// We can use Mocks and Stubs to replace IConsoleManager while writing unit tests.
    /// </summary>
    public interface IRegularConsoleOperations
    {
        /// <summary>Writes the specified value.</summary>
        /// <param name="value">The value.</param>
        Task Write(string value);

        /// <summary>Writes the line.</summary>
        /// <param name="value">The value.</param>
        Task WriteLine(string value = "");

        /// <summary>Reads the key.</summary>
        /// <returns>ConsoleKeyInfo.</returns>
        Task<ConsoleKeyInfo> ReadKey();

        /// <summary>Reads the line.</summary>
        /// <returns>System.String.</returns>
        Task<string> ReadLine();

        /// <summary>Clears this instance.</summary>
        Task Clear();
    }
}
