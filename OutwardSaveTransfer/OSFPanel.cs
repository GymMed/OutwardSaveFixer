using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutwardSaveFixer
{
    public class OSFPanel
    {
        private Panel menuPanel;
        private Panel parentPanel;

        public OSFPanel(Panel mainPanel, Panel mainParentPanel)
        {
            this.menuPanel = mainPanel;
            this.parentPanel = mainParentPanel;
        }
            
        public Panel Get_Panel()
        {
            return menuPanel;
        }

        public Panel Get_Parent_Panel()
        {
            return parentPanel;
        }
    }
}
