using System;

namespace RiverLinkReporter.cli
{
    /// <summary>
    /// Identifies optional settings to be applied when parsing program arguments.
    /// </summary>
    [Flags]
    public enum GetOptionsSettings
    {
        /// <summary>
        /// None; parse with default settings.
        /// </summary>
        None = 0,
        /// <summary>
        /// Maintain strict emulation of the glibc getopt implementation.
        /// </summary>
        GlibcCorrect = 0x01,
        /// <summary>
        /// Maintain strict emulation of the POSIX getopt implementation.
        /// </summary>
        PosixCorrect = 0x02,
        /// <summary>
        /// Composite value that sets both <see cref="GlibcCorrect"/> and <see cref="PosixCorrect"/>
        /// </summary>
        Strict = (GlibcCorrect | PosixCorrect),
        /// <summary>
        /// Throw an <see cref="System.ApplicationException"/> when a parsing error occurs.
        /// </summary>
        ThrowOnError = 0x04,
        /// <summary>
        /// Print an error message to <see cref="Console.Out"/> when a parsing error occurs.
        /// </summary>
        PrintOnError = 0x08
    }

    /// <summary>
    /// Provides an implementation of the getopt functions.
    /// </summary>
    /// <remarks>
    /// All state information that is maintained by any of the getopt methods is thread-static,
    /// meaning that it is particular to that thread of execution. Calling any of the methods
    /// of this class in separate threads will not conflict with each other.
    /// </remarks>
    public static class GetOpt
    {
        [ThreadStatic]
        private static object _target;
        [ThreadStatic]
        private static string _item;
        [ThreadStatic]
        private static string _text;
        [ThreadStatic]
        private static int _argsN;
        [ThreadStatic]
        private static int _textN;
        [ThreadStatic]
        private static int _charN;
        [ThreadStatic]
        private static bool _completed;
        [ThreadStatic]
        private static bool _corrected;

        /// <summary>
        /// Gets the name of the current option.
        /// </summary>
        /// <remarks>
        /// If <see cref="GetOptionsSettings.GlibcCorrect"/> or <see cref="GetOptionsSettings.PosixCorrect"/>
        /// are given as settings during parsing then this property will be null unless the current option has
        /// caused an error.
        /// </remarks>
        public static string Item => _item;

        /// <summary>
        /// Gets the argument associated with the current argument, or returns null if no argument was found.
        /// </summary>
        public static string Text => _text;

        /// <summary>
        /// Gets the index within the source array where normal non-option parsing should start.
        /// </summary>
        public static int Index => _argsN;

        /// <summary>
        /// Resets the internal state of the parser.
        /// </summary>
        public static void Reset()
        {
            Reset(null);
        }

        private static void Reset(object Target)
        {
            // Reset all state variables.
            _target = Target;
            _argsN = _textN = 0;
            _charN = 1;
            _item = _text = null;
            _completed = false;
            _corrected = false;
        }

        /// <summary>
        /// Gets the next option from the argument list.
        /// </summary>
        /// <param name="Args">An array containing the program arguments.</param>
        /// <param name="Options">
        /// A string that specifies the option characters that are valid for this program.
        /// An option character in this string can be followed by a colon (`:') to indicate
        /// that it takes a required argument. If an option character is followed by two
        /// colons (`::'), its argument is optional
        /// </param>
        /// <returns>The character representing the next option found, or -1 if the end of parsing is reached.</returns>
        public static int GetOptions(string[] Args, string Options)
        {
            return GetOptions(Args, Options, GetOptionsSettings.None);
        }
        /// <summary>
        /// Gets the next option from the argument list.
        /// </summary>
        /// <param name="Args">An array containing the program arguments.</param>
        /// <param name="Options">
        /// A string that specifies the option characters that are valid for this program.
        /// An option character in this string can be followed by a colon (`:') to indicate
        /// that it takes a required argument. If an option character is followed by two
        /// colons (`::'), its argument is optional
        /// </param>
        /// <param name="Settings">A bitwise OR combination of <see cref="GetOptionsSettings"/> enumeration values.</param>
        /// <returns>The character representing the next option found, or -1 if the end of parsing is reached.</returns>
        public static int GetOptions(string[] Args, string Options, GetOptionsSettings Settings)
        {
            if (Args == null)
                throw new ArgumentNullException(nameof(Args));

            if ((Options == null) || (Options.Length < 1))
                throw new ArgumentNullException(nameof(Options));

            // This clause prevents operation on zero length arrays
            if (Args.Length > 0)
            {
                // Detect which flags are set
                bool glibcCorrect = ((Settings & GetOptionsSettings.GlibcCorrect) == GetOptionsSettings.GlibcCorrect);
                bool posixCorrect = ((Settings & GetOptionsSettings.PosixCorrect) == GetOptionsSettings.PosixCorrect);
                bool throwOnError = ((Settings & GetOptionsSettings.ThrowOnError) == GetOptionsSettings.ThrowOnError);
                bool printOnError = ((Settings & GetOptionsSettings.PrintOnError) == GetOptionsSettings.PrintOnError);

                // If this is not a consecutive call, then reset all state variables.
                if (!object.ReferenceEquals(_target, Args) || _completed)
                    Reset(Args);

                // Changed (@2008-05-29 21:38): Permutation of the 'args' array is now the default behavior.
                if (!_corrected && !posixCorrect && (Options[0] != '+'))
                {
                    // The default implementation of glibc's getopt permutes the array being parsed
                    // so that all non-option arguments are at the end in their respective order. This allows
                    // programs to accept optional arguments even if they were not expecting them.

                    // A temporary array to hold the result of the permutation is created.
                    string[] tmp = new string[Args.Length];
                    int tmpI = 0, tmpJ = (-1), tmpK = (-1), tmpN = 0;

                    // Iterate through the array and pick out all option and their possible 
                    // arguments and add them to the result array in the order they are detected.
                    for (tmpI = 0; tmpI < Args.Length; tmpI++)
                    {
                        if ((Args[tmpI] != null) && (Args[tmpI].Length > 1) && (Args[tmpI][0] == '-'))
                        {
                            tmp[tmpN++] = Args[tmpI];

                            for (tmpJ = 1; tmpJ < Args[tmpI].Length; tmpJ++)
                            {
                                if ((tmpK = Options.IndexOf(Args[tmpI][tmpJ])) != (-1))
                                {
                                    if (((tmpK + 1) < Options.Length) && (Options[tmpK + 1] == ':'))
                                    {
                                        // If the option argument is supplied with the option itself then continue
                                        // iteration through the array.
                                        if ((tmpJ < 2) && (Args[tmpI].Length > 2))
                                        {
                                            if (((tmpJ + 1) < Args[tmpI].Length) && (Options.IndexOf(Args[tmpI][tmpJ + 1]) < 0))
                                                continue;
                                        }

                                        // If this option had an argument add it in succession to the 'result' array.
                                        if ((tmpI + 1) < Args.Length)
                                            tmp[tmpN++] = Args[++tmpI];
                                    }
                                }
                            }
                        }
                    }

                    // Iterate through the array and pick out all non-option arguments and add them
                    // to the result array in the order they are detected.
                    for (tmpI = 0; tmpI < Args.Length; tmpI++)
                    {
                        if ((Args[tmpI] != null) && (Args[tmpI].Length > 1) && (Args[tmpI][0] == '-'))
                        {
                            for (tmpJ = 1; tmpJ < Args[tmpI].Length; tmpJ++)
                            {
                                if ((tmpK = Options.IndexOf(Args[tmpI][tmpJ])) != (-1))
                                {
                                    if (((tmpK + 1) < Options.Length) && (Options[tmpK + 1] == ':'))
                                    {
                                        // If the option argument is supplied with the option itself then continue
                                        // iteration through the array.
                                        if ((tmpJ < 2) && (Args[tmpI].Length > 2))
                                        {
                                            if (((tmpJ + 1) < Args[tmpI].Length) && (Options.IndexOf(Args[tmpI][tmpJ + 1]) < 0))
                                                continue;
                                        }

                                        // If this option had an argument, then it has already been stored in the 'result' array
                                        // so pass over it.
                                        if ((tmpI + 1) < Args.Length)
                                            ++tmpI;
                                    }
                                }
                            }
                        }
                        else
                        {
                            tmp[tmpN++] = Args[tmpI];
                        }
                    }

                    // Overwrite the contents of 'array' with 'result'.
                    Array.Copy(tmp, 0, Args, 0, Args.Length);

                    // Set _corrected to true so that this process does not reoccur for this array.
                    _corrected = true;
                }

                // Begin parsing by iteration over the array.
                // Any exit or conclusion from this loop breaks parsing and
                // causes the reset of all state variables.
                for (int tmpItr = (-1); _argsN < Args.Length;)
                {
                    // Throw an exception if any element in the 'args' array is null.
                    if (Args[_argsN] == null)
                        throw new ArgumentNullException("args[" + _argsN + "]");

                    // Reset these state variables to null so that their values are not
                    // confused with the values set by a previous call.
                    _item = null;
                    _text = null;

                    if ((Args[_argsN].Length < 2) || (Args[_argsN][0] != '-'))
                    {
                        // POSIX demands that option parsing stop at the first non-option argument
                        // however, if complete POSIX compliance is not specified, GetOptions will
                        // will iterate over the array and parse all options it finds.
                        if (!_corrected && !posixCorrect && (Options[0] != '+'))
                        {
                            // Small loop that breaks when an option is encountered.
                            for (tmpItr = (_argsN + 1); (tmpItr < Args.Length) && ((Args[tmpItr] == null) || (Args[tmpItr].Length < 2) || (Args[tmpItr][0] != '-')); tmpItr++)

                                // Reset state variables so that iteration can continue;
                                if (tmpItr < Args.Length)
                                {
                                    _argsN = _textN = tmpItr;
                                    _charN = 1;
                                    continue;
                                }
                        }

                        // If no more options could be encountered (for any reason) iteration ends
                        // and so does parsing.
                        break;
                    }
                    else
                    {
                        // If "--" is encountered all option parsing must end.
                        if (Args[_argsN][1] == '-')
                        {
                            ++_argsN;
                            break;
                        }

                        for (; _charN < Args[_argsN].Length;)
                        {
                            // Save the current option name.
                            char c = Args[_argsN][_charN];

                            // If no compliance flags are set, then set the _item state variable
                            // to the current option name.
                            if (!glibcCorrect && !posixCorrect)
                                _item = c.ToString();

                            if ((tmpItr = Options.IndexOf(Args[_argsN][_charN])) != (-1))
                            {
                                // Both glibc and POSIX have the _item counterpart 'optopt' unset when the option does not cause an error.
                                if (glibcCorrect || posixCorrect)
                                    _item = null;

                                // If the current option is specified to have an argument...
                                if (((tmpItr + 1) < Options.Length) && (Options[tmpItr + 1] == ':'))
                                {
                                    // Detect if the argument is given with the option (i.e. "-cfoo") by examining the option string
                                    // and the character immediately following the option name. If it is not a valid optioon, then interpret
                                    // it and the following characters as the option argument.
                                    if ((_charN < 2) && (Args[_argsN].Length > 2))
                                    {
                                        if (((_charN + 1) < Args[_argsN].Length) && (Options.IndexOf(Args[_argsN][_charN + 1]) < 0))
                                        {
                                            _text = Args[_argsN++].Remove(0, 2);
                                            _charN = 1;
                                            return (int)c;
                                        }
                                    }

                                    if (((_textN + 1) < Args.Length) && (Args[(_textN + 1)] != "--"))
                                    {
                                        _text = Args[++_textN];
                                    }
                                    else if (((tmpItr + 2) >= Options.Length) || (Options[(tmpItr + 2)] != ':'))
                                    {
                                        // If this option is missing its argument then perform standard error setting procedures.
                                        if (glibcCorrect || posixCorrect)
                                            _item = c.ToString();

                                        if (throwOnError)
                                            throw new ApplicationException("Option '" + c + "' requires an argument.");
                                        else if (printOnError)
                                            Console.WriteLine("Option '{0}' requires an argument.", c);

                                        ++_charN;
                                        return ((int)'?');
                                    }
                                }

                                ++_charN;
                                return (int)c;
                            }
                            else
                            {
                                // If this option is missing its argument then perform standard error setting procedures.
                                if (glibcCorrect || posixCorrect)
                                    _item = c.ToString();

                                if (throwOnError)
                                    throw new ApplicationException("Unknown option '" + c + "'.");
                                else if (printOnError)
                                    Console.WriteLine("Unknown option '{0}'.", c);

                                ++_charN;
                                return ((int)'?');
                            }
                        }

                        // If the option had an argument skip over it.
                        if (_textN > _argsN++)
                            _argsN = ++_textN;

                        _textN = _argsN;
                        _charN = 1;
                    }
                }
            }

            // Flag parsing as completed and exit parsing.
            _completed = true;
            return (-1);
        }
    }
}