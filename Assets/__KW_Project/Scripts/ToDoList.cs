using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class ToDoList : MonoBehaviour
    {
        [TextArea(10, 10)]
        public string toDoMemo;
    }
}
