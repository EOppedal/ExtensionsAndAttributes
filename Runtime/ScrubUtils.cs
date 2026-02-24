using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScrubUtils {
    /// <summary>
    /// Loads all the Scrubs of type T from the specified resource folder path.
    /// </summary>
    /// <typeparam name="T">The type of Scrub to load.</typeparam>
    /// <param name="path">The path of the resource folder. If empty, loads from the root resource folder.</param>
    /// <returns>An array collection of Scrubs of type T.</returns>
    public static T[] GetAllScrubsInResourceFolder<T>(string path = "") where T : ScriptableObject =>
        Resources.LoadAll<T>(path);

    /// <summary>
    /// Gets the Scrub of type T with the specified name from the given array.
    /// </summary>
    /// <typeparam name="T">The type of Scrub to get.</typeparam>
    /// <param name="scrubs">The array of Scrubs to search in.</param>
    /// <param name="scrubName">The name of the Scrub to find.</param>
    /// <returns>The Scrub of type T with the specified name, or null if not found.</returns>
    public static T GetByName<T>(this IEnumerable<T> scrubs, string scrubName) where T : ScriptableObject =>
        scrubs.FirstOrDefault(scrub => scrub.name == scrubName);
}