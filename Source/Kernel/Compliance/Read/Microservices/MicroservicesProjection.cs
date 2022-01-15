// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Compliance.Events.Microservices;
using Cratis.Events.Projections;

namespace Cratis.Compliance.Read.Microservices
{
    /// <summary>
    /// Defines the projection for <see cref="Microservice"/>.
    /// </summary>
    [Projection("afcdf3df-53ab-4c35-94ab-07be4500b2ec")]
    public class MicroservicesProjection : IProjectionFor<Microservice>
    {
        /// <inheritdoc/>
        public void Define(IProjectionBuilderFor<Microservice> builder) => builder
            .From<MicroserviceAdded>(_ => _
                .Set(m => m.Name).To(e => e.Name));
    }
}
