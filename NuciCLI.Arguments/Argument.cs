namespace NuciCLI.Arguments
{
    /// <summary>
    /// Represents a command line argument with its properties.
    /// </summary>
    /// <param name="name">The name of the argument (without the leading '--').</param>
    /// <param name="help">A brief description of the argument's purpose.</param>
    /// <param name="isRequired">Indicates whether the argument is required.</param>
    /// <param name="defaultValue">The default value for the argument if it is not provided.</param>
    public class Argument(string name, string help, bool isRequired, object defaultValue = null)
    {
        /// <summary>
        /// The name of the argument.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// A brief description of the argument's purpose.
        /// </summary>
        public string Description { get; } = help;

        /// <summary>
        /// Indicates whether the argument is required.
        /// </summary>
        public bool IsRequired { get; } = isRequired;

        /// <summary>
        /// The default value for the argument if it is not provided.
        /// </summary>
        public object DefaultValue { get; } = defaultValue;
    }
}