using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.RandomNPC
{
    public class NPCSphere : MonoBehaviour
    {
        public void setup(Vector3 pos)
        {
            this.transform.localPosition = pos;
        }
    }

}