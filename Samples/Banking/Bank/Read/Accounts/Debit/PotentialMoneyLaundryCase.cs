// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.AccountHolders;
using Concepts.Accounts;

namespace Read.Accounts.Debit;

public record PotentialMoneyLaundryCase(AccountHolderId CustomerId, AccountId AccountId, DateOnly LastOccurrence);