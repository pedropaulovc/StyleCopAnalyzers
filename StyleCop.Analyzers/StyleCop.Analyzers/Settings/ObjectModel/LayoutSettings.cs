// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    using System.Linq;
    using LightJson;

    internal class LayoutSettings
    {
        /// <summary>
        /// These are the valid line endings for a file.
        /// </summary>
        private static readonly string[] ValidLineEndings = new[] { "\n", "\r\n", "\r" };

        /// <summary>
        /// This is the backing field for the <see cref="NewlineAtEndOfFile"/> property.
        /// </summary>
        private readonly OptionSetting newlineAtEndOfFile;

        /// <summary>
        /// This is the backing field for the <see cref="AllowConsecutiveUsings"/> property.
        /// </summary>
        private readonly bool allowConsecutiveUsings;

        /// <summary>
        /// This is the backing field for the <see cref="LineEnding"/> property.
        /// </summary>
        private readonly string lineEnding;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSettings"/> class.
        /// </summary>
        protected internal LayoutSettings()
        {
            this.newlineAtEndOfFile = OptionSetting.Allow;
            this.allowConsecutiveUsings = true;
            this.lineEnding = "\n";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSettings"/> class.
        /// </summary>
        /// <param name="layoutSettingsObject">The JSON object containing the settings.</param>
        protected internal LayoutSettings(JsonObject layoutSettingsObject)
            : this()
        {
            foreach (var kvp in layoutSettingsObject)
            {
                switch (kvp.Key)
                {
                case "newlineAtEndOfFile":
                    this.newlineAtEndOfFile = kvp.ToEnumValue<OptionSetting>();
                    break;

                case "allowConsecutiveUsings":
                    this.allowConsecutiveUsings = kvp.ToBooleanValue();
                    break;

                case "lineEnding":
                    this.lineEnding = kvp.ToStringValue();

                    if (!this.IsValidLineEnding(this.lineEnding))
                    {
                        throw new InvalidSettingsException($"`{this.lineEnding}` is not valid line ending. " +
                            $"Valid line endings are: `{string.Join("`, `", ValidLineEndings)}`.");
                    }

                    break;

                default:
                    break;
                }
            }
        }

        public OptionSetting NewlineAtEndOfFile =>
            this.newlineAtEndOfFile;

        public bool AllowConsecutiveUsings =>
            this.allowConsecutiveUsings;

        public string LineEnding =>
            this.lineEnding;

        private bool IsValidLineEnding(string lineEnding)
        {
            return ValidLineEndings.Contains(lineEnding);
        }
    }
}
