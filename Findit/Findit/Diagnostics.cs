using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Findit
{
    public partial class Diagnostics : Form
    {
        public Grepper[] searchers;

        public Diagnostics()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Diagnostics_Load(object sender, EventArgs e)
        {
            DisplayFileQueues();
            DisplayThreadStates();
        }

        private void DisplayFileQueues()
        {
            rtbFileQueues.Clear();
            rtbFileQueues.Text = "Queue #,Has been searched,Matching line #,File name" + Environment.NewLine;
            for (int i = 0; i < Globals.processorQueues.Count; ++i)
            {
                foreach (QueuedFile f in Globals.processorQueues[i].filesToSearch)
                {
                    rtbFileQueues.Text += i.ToString() + "," + f.HasBeenSearched.ToString() + "," + f.MatchLineNumber.ToString() + "," + f.file.Name + Environment.NewLine;
                }
            }
        }

        private void DisplayThreadStates()
        {
            rtbThreadStates.Clear();
            if(searchers != null)
            {
                rtbThreadStates.Text += "Thread #,Search Result Count,Binary skipped" + Environment.NewLine;
                for(int i = 0; i < searchers.Length; ++i)
                {
                    rtbThreadStates.Text += i.ToString() + "," + searchers[i].SearchResultCount.ToString() + "," + searchers[i].perfStats.BinarySkipped.ToString() + Environment.NewLine;
                }
            }
        }
    }
}
