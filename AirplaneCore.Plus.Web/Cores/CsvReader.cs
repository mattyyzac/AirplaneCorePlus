using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AirplaneCore.Plus.Web.Cores
{
    /// <summary>
    /// ref: https://stackoverflow.com/questions/21342949/how-can-i-split-a-string-while-ignore-commas-in-between-quotes
    /// </summary>
    public class CsvReader : IDisposable
    {
        private readonly TextReader _reader;
        private static Regex rexCsvSplitter = new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
        private static Regex rexRunOnLine = new Regex(@"^[^""]*(?:""[^""]*""[^""]*)*""[^""]*$");

        public long RowIndex { get; private set; } = 0;

        public CsvReader(string fileName)
        : this(new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        public CsvReader(Stream stream)
        {
            this._reader = new StreamReader(stream);
        }

        public System.Collections.IEnumerable RowEnumerator
        {
            get
            {
                if (null == this._reader)
                    throw new ApplicationException("I can't start reading without CSV input.");

                RowIndex = 0;

                string sLine;
                string sNextLine;

                while (null != (sLine = _reader.ReadLine()))
                {
                    while (rexRunOnLine.IsMatch(sLine) && (sNextLine = this._reader.ReadLine()) != null)
                        sLine += "\n" + sNextLine;

                    RowIndex++;
                    var values = rexCsvSplitter.Split(sLine);

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = CSV.Unescape(values[i]);
                    }
                    yield return values;
                }
                this._reader.Close();
            }

        }


        public void Dispose()
        {
            if (this._reader != null)
                this._reader.Dispose();
        }
    }
}