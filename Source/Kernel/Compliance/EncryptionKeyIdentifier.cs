// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Cratis.Compliance
{
    /// <summary>
    /// Represents the unique identifier of an <see cref="EncryptionKey"/>.
    /// </summary>
    /// <param name="Value">The underlying value.</param>
    public record EncryptionKeyIdentifier(string Value) : ConceptAs<string>(Value)
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> to <see cref="EncryptionKeyIdentifier"/>.
        /// </summary>
        /// <param name="value"><see cref="string"/> to convert from.</param>
        public static implicit operator EncryptionKeyIdentifier(string value) => new(value);
    }
}
