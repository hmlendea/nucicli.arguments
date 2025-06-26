using System;
using System.Collections.Generic;

using NuciExtensions;

namespace NuciCLI.Arguments
{
    /// <summary>
    /// A class for parsing command line arguments.
    /// </summary>
    public class ArgumentParser
    {
        private readonly Dictionary<string, Argument> arguments = [];

        /// <summary>
        /// Adds an argument to the parser.
        /// </summary>
        /// <param name="name">The name of the argument (without the leading '--').</param>
        /// <param name="help">A brief description of the argument's purpose.</param>
        /// <param name="required">Indicates whether the argument is required.</param>
        /// <param name="defaultValue">The default value for the argument if it is not provided.</param>
        public void AddArgument(string name, string help = "", bool required = false, object defaultValue = null)
            => arguments[name] = new Argument(name, help, required, defaultValue);

        /// <summary>
        /// Parses the command line arguments and returns a Namespace containing the parsed values.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>A Namespace containing the parsed arguments.</returns>
        /// <exception cref="ArgumentException">Thrown when an argument is invalid or unknown.</exception>
        /// <exception cref="ArgumentNullException">Thrown when a required argument is missing.</exception>
        public ArgumentsCollection ParseArgs(string[] args)
        {
            ArgumentsCollection parsed = new();
            Queue<string> remaining = new(args);

            while (remaining.Count > 0)
            {
                var current = remaining.Dequeue();

                if (!current.StartsWith("--"))
                {
                    throw new ArgumentException($"Invalid argument format: {current}");
                }

                var argName = current[2..];

                if (!arguments.ContainsKey(argName))
                {
                    throw new ArgumentException($"Unknown argument: {argName}");
                }

                if (EnumerableExt.IsNullOrEmpty(remaining))
                {
                    throw new ArgumentNullException(argName);
                }

                parsed[argName] = remaining.Dequeue();
            }

            foreach (Argument argument in arguments.Values)
            {
                if (parsed.Has(argument.Name))
                {
                    continue;
                }

                if (argument.IsRequired)
                {
                    throw new ArgumentNullException(argument.Name);
                }

                parsed[argument.Name] = argument.DefaultValue;
            }

            return parsed;
        }
    }
}