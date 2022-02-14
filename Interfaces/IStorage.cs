using System.Collections.Generic;

namespace Interfaces
{
    /// <summary>
    /// Storage.
    /// </summary>
    /// <typeparam name="T">The type of the object in storage.</typeparam>
    public interface IStorage<T>
    {
        /// <summary>
        /// Saves objects to current storage.
        /// </summary>
        /// <param name="collection">Collection of objects to save.</param>
        void Save(IEnumerable<T> collection);

        /// <summary>
        /// Loads objects from current storage.
        /// </summary>
        /// <returns>Collection of objects to load.</returns>
        IEnumerable<T> Load();
    }
}
