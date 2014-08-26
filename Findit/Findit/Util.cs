using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Findit
{
    static class Util
    {
        public static string[] ComboToStrArry(ref ComboBox cbo, int cutoff)
        {
            //transfer the combo box items into a string array
            string[] result = { "" };

            while (cbo.Items.Count > cutoff)
            {
                cbo.Items.RemoveAt(0);
            }

            int nonblankitemcount = 0;
            foreach (string s in cbo.Items)
            {
                if (0 < s.Length)
                {
                    nonblankitemcount++;
                }
            }

            int n = 0;
            Array.Resize(ref result, nonblankitemcount);
            for (int i = 0; i < cbo.Items.Count; ++i)
            {
                string currentitem = cbo.Items[i].ToString();
                if (0 < currentitem.Length)
                {
                    result[n] = currentitem;
                    n++;
                }
            }
            return result;
        }

        public static string[] MenuChildrenToStrArry(ref ToolStripMenuItem menu, Boolean includeBlanks)
        {
            //return the text values of a menu's children as a string array
            string[] result = { };
            int elementcnt = 0;
            foreach (ToolStripMenuItem child in menu.DropDownItems)
            {
                if (includeBlanks || (0 < child.Text.Length))
                {
                    Array.Resize(ref result, ++elementcnt);
                    result[elementcnt - 1] = child.Text;
                }
            }
            return result;
        }

        public static void StrArryToMenuChildren(ref ToolStripMenuItem menu, string[] elements, Boolean includeBlanks)
        {
            //make a menu's children match the elements from a string array
            menu.DropDownItems.Clear();
            foreach (string s in elements)
            {
                if (includeBlanks || (0 < s.Length))
                {
                    menu.DropDownItems.Add(s);
                }
            }
        }

        public static void StrArryToComboItems(ref ComboBox cbo, string[] strarry, Boolean AllowDuplicates)
        {
            cbo.Items.Clear();
            foreach (string s in strarry)
            {
                if (AllowDuplicates || (-1 == cbo.Items.IndexOf(s)))
                {
                    cbo.Items.Add(s);
                }
            }
        }

        public static void HighlightWordInRtb(ref RichTextBox rtb, string WordToHighlight)
        {
            if (0 < WordToHighlight.Trim().Length)
            {
                int i = 0;
                while (i != -1)
                {
                    i = rtb.Text.IndexOf(WordToHighlight, i, StringComparison.CurrentCultureIgnoreCase);
                    if (i != -1)
                    {
                        rtb.SelectionStart = i;
                        rtb.SelectionLength = WordToHighlight.Length;
                        rtb.SelectionColor = Color.Red;
                        i = i + WordToHighlight.Length;
                    }
                }
            }
        }

        public static void OpenFile(string fname, string customexe)
        {
            if(System.IO.File.Exists(fname))
            {
                if (System.IO.File.Exists(customexe))
                {
                    //start using the custom exe they asked for
                    Process ntpdplpl = new Process();
                    ntpdplpl.StartInfo.FileName = customexe;
                    ntpdplpl.StartInfo.Arguments = fname;
                    ntpdplpl.Start();
                }
                else
                {
                    //let the system decide what is the best editor
                    System.Diagnostics.Process.Start(fname);
                }
            }
        }

        public static void OpenFileInCustomEditor(string filename, string customEditorExeName)
        {
            Util.OpenFile(filename, customEditorExeName);
        }

        public static void OpenEnclosingFolder(string fullfilename)
        {
            if (System.IO.File.Exists(fullfilename))
            {
                string foldername = System.IO.Directory.GetParent(fullfilename).ToString();
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + fullfilename);
            }
        }

        public static void RemoveEmptyEntriesFromCombo(ref ComboBox cbo)
        {
            for (int i = cbo.Items.Count - 1; i >= 0; i--)
            {
                if (0 == cbo.Items[i].ToString().Trim().Length)
                {
                    cbo.Items.Remove(cbo.Items[i]);
                }
            }
        }

        public static int GetPixelWidthOfFormattedText(string text, Font fnt, int currentcontrolwidth)
        {
            //tell me how wide, in pixels, the text of the textbox is
            if (frmMain.ActiveForm == null)
            {
                //just in case we get here before the form has loaded
                return currentcontrolwidth;
            }
            else
            {
                Graphics g = Graphics.FromHwnd(frmMain.ActiveForm.Handle);
                return (int)g.MeasureString(text, fnt).Width;
            }
        }

        public static Boolean IsStringInStrArry(string s, ref string[] arry)
        {
            foreach (string element in arry)
            {
                if (element == s)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static Boolean IsValidGuid(string g)
        {
            try
            {
                Guid tmp = new Guid(g);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string[] EmptyStringArray()
        {
            return new string[0];
        }

    }
}
