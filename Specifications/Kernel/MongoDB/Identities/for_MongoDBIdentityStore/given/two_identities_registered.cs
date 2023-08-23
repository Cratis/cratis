// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Identities;

namespace Aksio.Cratis.Kernel.MongoDB.Identities.for_MongoDBIdentityStore.given;

public class two_identities_registered : all_dependencies
{
    protected MongoDBIdentityStore store;
    protected IdentityId first_identity;
    protected IdentityId second_identity;
    protected MongoDBIdentity first_identity_from_database;
    protected MongoDBIdentity second_identity_from_database;

    void Establish()
    {
        first_identity = IdentityId.New();
        second_identity = IdentityId.New();

        first_identity_from_database = new MongoDBIdentity
        {
            Id = first_identity,
            Subject = "First subject",
            Name = "First name",
            UserName = "First user name"
        };

        identities_from_database.Add(first_identity_from_database);

        second_identity_from_database = new MongoDBIdentity
        {
            Id = second_identity,
            Subject = "Second subject",
            Name = "Second name",
            UserName = "Second user name"
        };
        identities_from_database.Add(second_identity_from_database);

        store = new(tenant_database.Object);
    }
}