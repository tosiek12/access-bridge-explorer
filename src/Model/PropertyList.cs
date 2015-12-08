using System.Collections.Generic;

namespace AccessBridgeExplorer.Model {
  /// <summary>
  /// A list of <see cref="PropertyNode"/>.
  /// </summary>
  public class PropertyList : List<PropertyNode> {
    public PropertyNode AddProperty(string name, object value) {
      var prop = new PropertyGroup(name, value);
      Add(prop);
      return prop;
    }

    public PropertyGroup AddGroup(string name, object value = null) {
      var group = new PropertyGroup(name, value);
      Add(group);
      return group;
    }
  }
}