using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAllHardcodeChinese
{
    struct Reference
    {
        public Reference(string file, int line)
        {
            this.file = file;
            this.line = line;
        }

        public override string ToString()
        {
            return string.Format("File: \"{0}\", Line: {1}", file, line);
        }

        string file;
        int line;
    }

    class WordRef
    {
        public WordRef(string word)
        {
            this.word = word;
            reference = new List<Reference>();
        }

        public void AddReference(string file, int line)
        {
            reference.Add(new Reference(file, line));
        }

        public List<Reference> GetReferenceList()
        {
            return reference;
        }

        private string word;
        private List<Reference> reference;
    }
}
