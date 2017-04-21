using System.Collections.Generic;
using System.Collections.Specialized;

namespace Tandem.Elements
{
    public sealed class TdmContainerPool : INotifyCollectionChanged
    {
        private Dictionary<string, TdmElementContainer> _collection;

        private TdmContainerPool()
        {
            _collection = new Dictionary<string, TdmElementContainer>();
        }

        public static TdmContainerPool Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly TdmContainerPool instance = new TdmContainerPool();
        }


        private int _count = 0;
        private const int MAX_POOL_SIZE = 50000;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Number of elements of type <see cref="TdmElementContainer"/> in the <see cref="TdmPoolConstructor"/> </summary>
        public int Count { get { return _count; } }

        public Dictionary<string, TdmElementContainer> PoolTable { get { return _collection; } }

        /// <summary>
        /// Determines wheteher <see cref="TdmPoolConstructor"/> contains a specific <see cref="ITdmContainer"/>.</summary>
        /// <param name="tdmContainer">The <see cref="ITdmContainer"/> to locate in the <see cref="TdmContainerPool"/></param>
        /// <returns>true if the <see cref="TdmContainerPool"/> contains an element; otherwise, false.</returns>
        public bool Contains(TdmElementContainer tdmContainer)
        {
            return (_count == 0) ? false : _collection.ContainsKey(tdmContainer.Name);
        }

        /// <summary>
        /// Determines wheteher <see cref="TdmPoolConstructor"/> contains a specific <see cref="ITdmContainer"/>.</summary>
        /// <param name="tdmContainerName">The <see cref="string"/> to locate in the <see cref="TdmContainerPool"/></param>
        /// <returns>true if the <see cref="TdmContainerPool"/> contains an element; otherwise, false.</returns>
        public bool Contains(string tdmContainerName)
        {
            return (_count == 0) ? false : _collection.ContainsKey(tdmContainerName);
        }

        /// <summary>
        /// Returns <see cref="ITdmContainer"/> from the <see cref="TdmPoolConstructor"/>.</summary>
        /// <param name="tdmContainerName">Name of the <see cref="ITdmContainer"/> to return.</param>
        /// <returns> <see cref="ITdmContainer"/></returns>
        public TdmElementContainer Get(string tdmContainerName)
        {
            return _collection.ContainsKey(tdmContainerName) ?
                            (TdmElementContainer)_collection[tdmContainerName] : null;
        }

        /// <summary>
        /// Adds <see cref="ITdmContainer"/> to the <see cref="TdmContainerPool"/>.</summary>
        /// <param name="tdmContainer">The <see cref="ITdmContainer"/> to add.</param>
        public void Add(TdmElementContainer tdmContainer)
        {
            if (_count < MAX_POOL_SIZE)
            {
                if (_collection.ContainsKey(tdmContainer.Name))
                {
                    _replace(tdmContainer);
                }
                else
                {
                    _add(tdmContainer);
                }
            }
        }

        /// <summary>
        /// Removes element of type <see cref="ITdmContainer"/> from the <see cref="TdmContainerPool"/>.</summary> 
        /// <param name="tdmContainerName">The <see cref="string"/> to remove.</param>
        public void Remove(string tdmContainerName)
        {
            if (_count != 0)
            {
                if (_collection.ContainsKey(tdmContainerName))
                {
                    _collection.Remove(tdmContainerName);
                    _count--;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, tdmContainerName));
                }
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="TdmContainerPool"/>.</summary> 
        public void RemoveAll()
        {
            var copy = new Dictionary<string, TdmElementContainer>(_collection);
            foreach (string key in copy.Keys)
            {
                this.Remove(key);
            }
        }

        private void _replace(TdmElementContainer tdmContainer)
        {
            var oldContainer = (TdmElementContainer)_collection[tdmContainer.Name];
            tdmContainer.Count = oldContainer.Count + 1;

            _collection.Remove(tdmContainer.Name);
            _collection.Add(tdmContainer.Name, tdmContainer);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, tdmContainer.Name));
        }

        private void _add(TdmElementContainer tdmContainer)
        {
            _collection.Add(tdmContainer.Name, tdmContainer);
            _count++;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, tdmContainer.Name));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
    }
}


