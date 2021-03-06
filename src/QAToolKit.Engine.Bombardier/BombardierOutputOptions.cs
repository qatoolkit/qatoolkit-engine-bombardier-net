﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("QAToolKit.Engine.Bombardier.Test")]
namespace QAToolKit.Engine.Bombardier
{
    /// <summary>
    /// Bombardier output options
    /// </summary>
    public class BombardierOutputOptions
    {
        /// <summary>
        /// Obfuscate authentication header in the results output
        /// </summary>
        public bool ObfuscateAuthenticationHeader { get; set; } = true;
    }
}