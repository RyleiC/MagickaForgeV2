using System.Xml.Linq;

namespace ContentCompiler.Data.Languages
{
    public class LanguageFile
    {
        private XDocument _document;
        private bool _isDirty;

        public LanguageFile()
        {
        }

        public void RegisterEntry(string displayText, string code)
        {
            var table = _document.Element("Workbook").Element("Worksheet").Element("Table");
            foreach (var row in table.Elements("Row"))
            {
                var cells = row.Elements("Cell").ToList();
                if (cells.Count >= 2 && cells[0].Element("String").Value == code)
                {
                    if (cells[1].Element("String").Value == displayText)
                    {
                        return;
                    }

                    _isDirty = true;
                    cells[1].Element("String").Value = displayText;
                    return;
                }
            }

            _isDirty = true;
            table.Add(new XElement("Row"));
            table.Elements("Row").Last().Add(new XElement("Cell", new XElement("String", code)));
            table.Elements("Row").Last().Add(new XElement("Cell", new XElement("String", displayText)));
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                _document = XDocument.Load(filePath);
            }
        }

        public void LoadDefault()
        {
            _document = new XDocument
            (
                new XElement("Workbook",
                    new XElement("Worksheet",
                            new XElement("Table")
                    )
                )
            );
        }
        public void Write(string filePath)
        {
            _document.Save(filePath);
        }

        public bool IsDirty => _isDirty;
    }
}