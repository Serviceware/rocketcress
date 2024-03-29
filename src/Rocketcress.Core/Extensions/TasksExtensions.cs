﻿using System.Threading.Tasks;

namespace Rocketcress.Core.Extensions;

/// <summary>
/// Provides extension methods for the Task class.
/// </summary>
public static class TasksExtensions
{
    /// <summary>
    /// Checks if one of many tasks have completed and has a specific result.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="tasks">The tasks to check.</param>
    /// <param name="expectedResult">The expected result.</param>
    /// <returns>Returns wether a task has finished and has a result that matches the expectedResult.</returns>
    public static bool HasResult<T>(this IEnumerable<Task<T>> tasks, T expectedResult)
    {
        return HasResult(tasks, x => Equals(x, expectedResult));
    }

    /// <summary>
    /// Checks if one of many tasks have completed and has a result that fulfills a condition.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="tasks">The tasks to check.</param>
    /// <param name="predicate">The condition.</param>
    /// <returns>Returns wether a task has finished and has a result that fulfills the condition.</returns>
    public static bool HasResult<T>(this IEnumerable<Task<T>> tasks, Func<T, bool> predicate)
    {
        Guard.NotNull(tasks);
        Guard.NotNull(predicate);

        return (from task in tasks
                where task.Status == TaskStatus.RanToCompletion
                select task.Result).Any(predicate);
    }
}
