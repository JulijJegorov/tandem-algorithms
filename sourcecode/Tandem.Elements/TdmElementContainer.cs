
namespace Tandem.Elements
{
    public class TdmElementContainer
    {
        /// <summary>
        /// Store for the count property</summary>
        private uint _count;
        /// <summary>
        /// Store for the name property</summary>
        private string _name = null;
        /// <summary>
        /// Store for the alias property</summary>
        private string _alias = null;

        private readonly TdmContainerPool _tdmContainerPool;

        /// <summary>
        /// Count property</summary>
        /// <value>
        /// Value tag is used to describe property value</value>
        /// <remarks>Indicates how many times <see cref="ITdmContainer"/> was modified in <see cref="TdmContainerPool"/></remarks>
        public uint Count
        {
            get { return _count; }
            set { _count = value; }
        }
        /// <summary>
        /// Name property</summary>
        /// <value>
        /// Value tag is used to describe property value</value>
        /// <remarks>Is overwritten by alias/></remarks>
        public string Name
        {
            get { return _alias != null ? _alias : _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Alias property</summary>
        /// <value>
        /// Value tag is used to describe property value</value>
        /// <remarks>Overwrites name property/></remarks>
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// Persist property</summary>
        /// <value>
        /// A value tag is used to describe the property value</value>
        /// <remarks>If persists, <see cref="ITdmContainer"/>  is removed from <see cref="TdmContainerPool"/> 
        /// if used in another <see cref="ITdmContainer"/></remarks>
        public bool Persist { get; set; }

        /// <summary>
        /// TdmElement property</summary>
        /// <value>
        /// A value tag is used to describe the property value</value>
        public ITdmElement Element { get; set; }


        /// <summary>
        /// Class constructor </summary>
        public TdmElementContainer(): this (TdmContainerPool.Instance)
        {
        }

        /// <summary>
        /// Class constructor </summary>
        public TdmElementContainer(TdmContainerPool tdmContainerPool)
        {
            _tdmContainerPool = tdmContainerPool;
            _count = 0;
        }

        /// <summary>
        /// FullName property</summary>
        /// <value>
        /// A value tag is used to describe the property value</value>
        /// <remarks>Name property + : + counter property</remarks>
        public string FullName
        {
            get { return Name + ":" + _count; }
        }

        /// <summary>
        /// Adds <see cref="ITdmContainer"/> to <see cref="TdmContainerPool"/> </summary>
        public void AddToPool()
        {
            _tdmContainerPool.Add(this);
        }
    }
}
