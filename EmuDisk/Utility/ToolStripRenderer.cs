using System.Windows.Forms;
using System.Drawing;

namespace EmuDisk
{
    public class ToolStripRenderer : ToolStripSystemRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem)
            {
                if (((ToolStripMenuItem)e.Item).Checked)
                {
                    e.TextColor = Color.FromArgb(49, 106, 197);
                }
            }

            base.OnRenderItemText(e);
        }
    }
}
