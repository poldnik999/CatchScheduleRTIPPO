using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    internal class ImportMaster
    {
        public static string AttachPDF()
        {
            string filePath = "";
            OpenFileDialog OPF = new OpenFileDialog();
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                filePath = OPF.FileName;
            }
            return filePath;
        }
    }
}
