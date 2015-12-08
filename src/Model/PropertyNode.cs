namespace AccessBridgeExplorer.Model {
  /// <summary>
  /// A (name, value) pair.
  /// </summary>
  public class PropertyNode {
    private readonly string _name;
    private readonly object _value;

    public PropertyNode(string name, object value) {
      _name = name;
      _value = value;
    }

    public string Name {
      get { return _name; }
    }

    public object Value {
      get { return _value; }
    }
  }
}