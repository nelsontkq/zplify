using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zplify.Tests
{
    public class LabelLoader : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return Directory.GetFiles("Data").Where(e => Path.GetExtension(e) == ".png").Select(e => new object[] {e}).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
