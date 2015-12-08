namespace AccessBridgeExplorer.Model {
  /// <summary>
  /// A property node that can have children (as a <see cref="PropertyList"/>).
  /// </summary>
  public class PropertyGroup : PropertyNode {
    private readonly PropertyList _children = new PropertyList();

    public PropertyGroup(string name, object value = null) : base(name, value) {
      Expanded = true;
    }

    public PropertyNode AddProperty(string name, object value) {
      return _children.AddProperty(name, value);
    }

    public PropertyGroup AddGroup(string name, object value = null) {
      return _children.AddGroup(name, value);
    }

    public PropertyList Children {
      get { return _children; }
    }

    public bool Expanded { get; set; }
  }
}