using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
    public static Transform GetItemParent(Transform current) {
        if (current.GetComponent<Item>() != null) {
            return current;
        } else if (current.parent != null) {
            return GetItemParent(current.parent);
        }
        return null;
    }
}
