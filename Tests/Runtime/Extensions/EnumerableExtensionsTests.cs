using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void Flatten_NestedList_ShouldReturnFlatList()
        {
            var nested = new List<List<int>>
            {
                new List<int> { 1, 2 },
                new List<int> { 3 },
                new List<int> { 4, 5 }
            };

            var flat = nested.Flatten().ToList();

            Assert.AreEqual(5, flat.Count);
            Assert.AreEqual(1, flat[0]);
            Assert.AreEqual(5, flat[4]);
        }

        private class Node
        {
            public int Value;
            public List<Node> Children = new List<Node>();
        }

        [Test]
        public void Flatten_Recursive_ShouldReturnAllNodes()
        {
            var root = new Node { Value = 1 };
            var child1 = new Node { Value = 2 };
            var child2 = new Node { Value = 3 };
            var grandChild = new Node { Value = 4 };

            root.Children.Add(child1);
            root.Children.Add(child2);
            child2.Children.Add(grandChild);

            var nodes = new List<Node> { root };
            var flat = nodes.Flatten(n => n.Children).ToList();

            Assert.AreEqual(4, flat.Count);
            Assert.IsTrue(flat.Any(n => n.Value == 4));
        }

        [Test]
        public void MinOrDefault_ShouldReturnMin()
        {
            var list = new List<int> { 5, 2, 8 };
            Assert.AreEqual(2, list.MinOrDefault());
        }

        [Test]
        public void MinOrDefault_Empty_ShouldReturnDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.MinOrDefault());
        }

        [Test]
        public void MaxOrDefault_ShouldReturnMax()
        {
            var list = new List<int> { 5, 2, 8 };
            Assert.AreEqual(8, list.MaxOrDefault());
        }

        [Test]
        public void MinBy_ShouldReturnElementWithMinKey()
        {
            var list = new List<string> { "a", "ccc", "bb" };
            var result = list.MinBy(s => s.Length);
            Assert.AreEqual("a", result.element);
            Assert.AreEqual(1, result.key);
        }

        [Test]
        public void MaxBy_ShouldReturnElementWithMaxKey()
        {
            var list = new List<string> { "a", "ccc", "bb" };
            var result = list.MaxBy(s => s.Length);
            Assert.AreEqual("ccc", result.element);
            Assert.AreEqual(3, result.key);
        }

        [Test]
        public void Union_WithParams_ShouldReturnUnionOfElements()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.Union(3, 4, 5).ToList();
            
            Assert.AreEqual(5, result.Count);
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
            Assert.IsTrue(result.Contains(3));
            Assert.IsTrue(result.Contains(4));
            Assert.IsTrue(result.Contains(5));
        }

        [Test]
        public void Concat_WithParams_ShouldReturnConcatenatedElements()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.Concat(3, 4, 5).ToList();
            
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(4, result[4]);
            Assert.AreEqual(5, result[5]);
        }
    }
}
