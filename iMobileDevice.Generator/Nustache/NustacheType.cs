namespace iMobileDevice.Generator.Nustache
{
    /// <summary>
    /// A common class for all types which are generated using a Nustache template. This class, or any of its
    /// subclasses, serve as the model for the Nustache templates.
    /// </summary>
    public class NustacheType
    {
        public NustacheType(string @namespace, string name)
        {
            this.Name = name;
            this.Namespace = @namespace;
        }

        public string Name
        {
            get;
            set;
        }

        public string Namespace
        {
            get;
            set;
        }
    }
}
