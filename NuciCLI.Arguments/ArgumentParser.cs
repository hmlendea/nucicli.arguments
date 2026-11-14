using System;
using System.Collections.Generic;

namespace NuciCLI.Arguments
{
    /// <summary>
    /// A class for parsing command line arguments.
    /// </summary>
    public class ArgumentParser
    {
        private static string ArgumentPrefix => "--";

        private readonly Dictionary<string, Argument> registeredArguments = [];

        /// <summary>
        /// Adds an argument to the parser.
        /// </summary>
        /// <param name="name">The name of the argument (without the leading '--').</param>
        /// <param name="help">A brief description of the argument's purpose.</param>
        /// <param name="required">Indicates whether the argument is required.</param>
        /// <param name="defaultValue">The default value for the argument if it is not provided.</param>
        public void AddArgument(string name, string help = "", bool required = false, object? defaultValue = null)
            => registeredArguments[name] = new Argument(name, help, required, defaultValue);

        /// <summary>
        /// Parses the command line arguments and returns a collection containing the parsed values.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>A collection containing the parsed arguments.</returns>
        /// <exception cref="ArgumentException">Thrown when an argument is invalid or unknown.</exception>
        /// <exception cref="ArgumentNullException">Thrown when a required argument is missing or an argument has no value.</exception>
        public ArgumentsCollection ParseArgs(string[] args)
        {
            ArgumentsCollection parsed = new();

            ConsumeTokens(new Queue<string>(args), parsed);
            ApplyDefaults(parsed);

            return parsed;
        }

        private void ConsumeTokens(Queue<string> remaining, ArgumentsCollection parsed)
        {
            while (remaining.Count > 0)
            {
                string current = remaining.Dequeue();

                if (!current.StartsWith(ArgumentPrefix, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Invalid argument format: {current}");
                }

                string argumentName = current[ArgumentPrefix.Length..];

                if (!registeredArguments.ContainsKey(argumentName))
                {
                    throw new ArgumentException($"Unknown argument: {argumentName}");
                }

                if (remaining.Count == 0)
                {
                    throw new ArgumentNullException(argumentName);
                }

                parsed[argumentName] = remaining.Dequeue();
            }
        }

        private void ApplyDefaults(ArgumentsCollection parsed)
        {
            foreach (Argument argument in registeredArguments.Values)
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
        }
    }
}