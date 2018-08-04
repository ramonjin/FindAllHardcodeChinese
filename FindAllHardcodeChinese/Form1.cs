using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FindAllHardcodeChinese
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Dictionary<string, WordRef> result = new Dictionary<string, WordRef>();
            SearchChinese(txtPath.Text, result);
            DisPlayResult(result);
            SaveResult(result, "1.txt");
        }

        private void SearchChinese(string path, Dictionary<string, WordRef> result)
        {
            if ((path.EndsWith(".lua") || path.EndsWith(".cs")) && File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int line = 0;
                    while (!sr.EndOfStream)
                    {
                        line++;
                        string txt = sr.ReadLine();
                        Regex regex = new Regex("\"[\u4E00-\u9FFF]+\"");
                        var matches = regex.Matches(txt);
                        foreach (Match match in matches)
                        {
                            WordRef wordRef;
                            if (!result.TryGetValue(match.Value, out wordRef))
                            {
                                wordRef = new WordRef(match.Value);
                                result[match.Value] = wordRef;
                            }

                            wordRef.AddReference(path, line);
                        }
                    }
                }
            }
            else if (Directory.Exists(path))
            {
                string[] entries = Directory.GetFileSystemEntries(path);
                foreach (string entry in entries)
                {
                    SearchChinese(entry, result);
                }
            }
        }

        private void DisPlayResult(Dictionary<string, WordRef> result)
        {
            int count = 0;
            foreach (var pair in result)
            {
                lstResult.Items.Add(pair.Key);
                count += pair.Value.GetReferenceList().Count;
            }
            lblCount.Text = string.Format("总共找到 {0} 个中文，共 {1} 处", result.Count, count);
        }

        private void SaveResult(Dictionary<string, WordRef> result, string outFilePath)
        {
            using (StreamWriter sw = new StreamWriter(outFilePath))
            {
                foreach (var pair in result)
                {
                    var list = pair.Value.GetReferenceList();
                    sw.WriteLine(string.Format("{0}\t{1}", pair.Key, list.Count));
                    foreach (Reference refer in list)
                    {
                        sw.WriteLine(refer.ToString());
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
