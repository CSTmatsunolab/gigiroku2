﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScriptEdit
    {
        [Test]
        public void MyClassTest1()
        {
            var x = new MyClass();
            Assert.IsEmpty(x.GetName());
            x.SetName("Alice");
            Assert.AreEqual("Alice", x.GetName());
        }
        [Test]
        public void MyClassTest2()
        {
            var x = new MyClass("Bob");
            Assert.AreEqual("Bob", x.GetName());
        }
    }
}
