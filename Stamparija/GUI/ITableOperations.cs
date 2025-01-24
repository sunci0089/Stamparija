using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stamparija.GUI
{
    //interfejs koga nasljedjuju sve tabele kako bi mogle komunicirati sa glavnim prozorom
    public interface ITableOperations
    {
        void addRow();
        void updateRow();
        void deleteRow();
        void Refresh();
        void Search(string text);
    }
}
