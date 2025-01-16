// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Events;
using Cratis.Chronicle.Reducers;

namespace Cratis.Chronicle.Integration.Orleans.InProcess.for_Reducers;

[DependencyInjection.IgnoreConvention]
public class ReducerThatCanFail(TaskCompletionSource tcs) : IReducerFor<SomeReadModel>
{
    public bool ShouldFail { get; set; }
    public TimeSpan HandleTime { get; set; } = TimeSpan.Zero;

    public async Task<SomeReadModel?> OnSomeEvent(SomeEvent evt, SomeReadModel? input, EventContext ctx)
    {
        tcs.SetResult();
        await Task.Delay(HandleTime);
        if (ShouldFail)
        {
            ShouldFail = false;
            throw new Exception("Something went wrong");
        }

        input ??= new SomeReadModel(0);
        return input with { Number = evt.Number };
    }
}
