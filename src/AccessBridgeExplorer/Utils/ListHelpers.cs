// Copyright 2016 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;

namespace AccessBridgeExplorer.Utils {
  public interface IIncrementalUpdateOperations<TSource, TNew> {
    int FindOldItemIndex(IList<TSource> items, int startIndex, TNew newItem);
    void InsertNewItem(IList<TSource> items, int index, TNew newItem);
    void UpdateOldItem(IList<TSource> items, int index, TNew newItem);
  }

  public static class ListHelpers {
    /// <summary>
    /// Update the <paramref name="oldItems"/> list incrementally by adding/removing/updating elements
    /// so that it ends up being equivalent to <paramref name="newItems"/>
    /// </summary>
    public static void IncrementalUpdate<TSource, TNew>(IList<TSource> oldItems, IList<TNew> newItems, IIncrementalUpdateOperations<TSource, TNew> operations) {
      // The insertion position in "oldItems". Elements located *before* |oldInsertionIndex|
      // in "oldItems" have been processed and won't be touched anymore.
      var oldInsertionIndex = 0;

      // We go through each item in the new list and decide what to do in the
      // existing list of items currently displayed. If there are additional
      // nodes in the new list, we insert them in the existing list. If there
      // are missing nodes in the new list we delete them from the existing
      // list.
      for (var newIndex = 0; newIndex < newItems.Count; newIndex++) {
        var newItem = newItems[newIndex];

        // Find item with same tag in old list.
        var oldItemIndex = operations.FindOldItemIndex(oldItems, oldInsertionIndex, newItem);
        if (oldItemIndex < 0) {
          // If this is a new node (existing node not found), insert new list
          // view item at current insertion location (at end or in middle)

          operations.InsertNewItem(oldItems, oldInsertionIndex, newItem);
          oldInsertionIndex++;
        } else {
          // If we found an equivalent node in the existing list, delete
          // existing items in between if needed, then update the existing
          // item with the updated values.

          // Delete items in range [oldIndex, oldItemIndex[
          for (var i = oldInsertionIndex; i < oldItemIndex; i++) {
            oldItems.RemoveAt(oldInsertionIndex);
          }
          oldItemIndex = oldInsertionIndex;

          // Update existing item with new property data
          operations.UpdateOldItem(oldItems, oldItemIndex, newItem);
          oldInsertionIndex++;
        }
      }

      // Delete all the existing items that don't exist anymore, since we
      // reached the end of the new list.
      while (oldInsertionIndex < oldItems.Count) {
        oldItems.RemoveAt(oldInsertionIndex);
      }
    }
  }
}