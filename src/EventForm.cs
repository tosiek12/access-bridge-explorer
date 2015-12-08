using System;
using System.Windows.Forms;
using AccessBridgeExplorer.Model;

namespace AccessBridgeExplorer {
  public partial class EventForm : Form {
    private readonly PropertyListViewWrapper _accessibleContextPropertyListWrapper;
    private readonly PropertyListViewWrapper _oldValuePropertyListWrapper;
    private readonly PropertyListViewWrapper _newValuePropertyListWrapper;

    public EventForm() {
      InitializeComponent();
      _accessibleContextPropertyListWrapper = new PropertyListViewWrapper(accessibleContextPropertyList, propertyImageList);
      _oldValuePropertyListWrapper = new PropertyListViewWrapper(oldValuePropertyList, propertyImageList);
      _newValuePropertyListWrapper = new PropertyListViewWrapper(newValuePropertyList, propertyImageList);

      //accessibleContextTabPage.Enter += (sender, args) => OnContextNodeSelect();
      //oldValuePage.Enter += (sender, args) => OnOldValueSelect();
      //newValuePage.Enter += (sender, args) => OnNewValueSelect();
    }

    public event EventHandler ContextNodeSelect;
    public event EventHandler OldValueSelect;
    public event EventHandler NewValueSelect;

    public void SetContextNodePropertyList(PropertyList propertyList) {
      _accessibleContextPropertyListWrapper.SetPropertyList(propertyList);
    }

    public void SetOldValuePropertyList(PropertyList propertyList) {
      _oldValuePropertyListWrapper.SetPropertyList(propertyList);
    }

    public void SetNewValuePropertyList(PropertyList propertyList) {
      _newValuePropertyListWrapper.SetPropertyList(propertyList);
    }

    protected virtual void OnContextNodeSelect() {
      var handler = ContextNodeSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    protected virtual void OnOldValueSelect() {
      var handler = OldValueSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    protected virtual void OnNewValueSelect() {
      var handler = NewValueSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}
