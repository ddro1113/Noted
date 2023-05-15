using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Noted
{
    public partial class Noted : Form
    {
        public Noted()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Noted_FormClosing);
        }

        private void richTextBoxNotes_TextChanged(object sender, EventArgs e)
        {

        }

        // Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBoxNotes.Text != "") // Check if the rich text box is not empty
            {
                DialogResult exitDialog = MessageBox.Show("Are you sure you want to exit?", "Noted", MessageBoxButtons.YesNo); // Open a dialog box to confirm the exit
                if (exitDialog == DialogResult.Yes) // If yes
                {
                    this.Close(); // Close the program 
                }
                else if (exitDialog == DialogResult.No) // If no
                {
                    return; // Return the user to the program 
                }
            }
            this.Close(); // Close the program 
        }

        // Save
        private string currentFilePath = null; // Hold the path of the currently opened/saved .txt file / Will be used to determine wether the user wants to save using the Save or Save as option
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (richTextBoxNotes.Text != "") // Check if the rich text box is not empty
            { // If yes
                if (string.IsNullOrEmpty(currentFilePath)) // Check if the currently opened .txt file is not yet saved
                { // If not
                    using (var saveAsDialog = new SaveFileDialog()) // Open a SaveFileDialog() to confirm the save
                    {
                        saveAsDialog.Filter = "Text Files (*.txt)|*.txt"; // Specify the file type to .txt
                        if (saveAsDialog.ShowDialog() == DialogResult.OK) // If yes
                        {
                            richTextBoxNotes.SaveFile(saveAsDialog.FileName, RichTextBoxStreamType.PlainText); // Save the file as .txt file
                            currentFilePath = saveAsDialog.FileName; // Update the currentFilePath variable with the new file path
                        }
                    }
                }
                // If the currently opened .txt file is already saved
                else
                {
                    richTextBoxNotes.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText); // Save/overwrite the .txt file using the current file path
                }
            }
            // If the rich text box is empty
            else
            {
                DialogResult emptyDialog = MessageBox.Show("The file is empty. Nothing to save.", "Noted", MessageBoxButtons.OK); // Show a dialog 
            }
        }

        // Save As
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveAsDialog = new SaveFileDialog()) // Open a SaveFileDialog() to confirm the save
            {
                saveAsDialog.Filter = "Text Files (*.txt)|*.txt"; // Specify the file type to .txt
                if (saveAsDialog.ShowDialog() == DialogResult.OK) // If yes
                {
                    richTextBoxNotes.SaveFile(saveAsDialog.FileName, RichTextBoxStreamType.PlainText); // Save the file as .txt file
                }
            }
        }

        // Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog()) // Open a OpenFileDialog() to confirm the open
            {
                openDialog.Filter = "Text Files (*.txt)|*.txt"; // Specify the file type to .txt
                if (openDialog.ShowDialog() == DialogResult.OK) // If yes
                {
                    var extension = Path.GetExtension(openDialog.FileName); // Get the file extension that user selected
                    if (extension != ".txt") // If the file extension is not .txt
                    {
                        MessageBox.Show("The file you selected is not a .txt file."); // Open a popup message 
                        return;
                    }
                    richTextBoxNotes.LoadFile(openDialog.FileName, RichTextBoxStreamType.PlainText); // Else, load the .txt file
                }
            }
        }

        // New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBoxNotes.Text != "") // Check if the rich text box is not empty
            {
                // If not empty
                DialogResult newDialog = MessageBox.Show("Do you want to save the current file?", "Noted", MessageBoxButtons.YesNo); //  Open a SaveFileDialog() to confirm the save before creating new

                if (newDialog == DialogResult.Yes) // If yes
                {
                    using (var saveAsDialog = new SaveFileDialog()) // Open a SaveFileDialog() to confirm the save
                    {
                        saveAsDialog.Filter = saveAsDialog.Filter = "Text Files (*.txt)|*.txt"; // Specify the file type to .txt
                        if (saveAsDialog.ShowDialog() == DialogResult.OK) // If yes
                        {
                            richTextBoxNotes.SaveFile(saveAsDialog.FileName, RichTextBoxStreamType.PlainText); // Save the file as .txt file
                            richTextBoxNotes.Clear(); // Clear the rich text box
                        }
                    }
                }
                else if (newDialog == DialogResult.No) // If no
                {
                    richTextBoxNotes.Clear(); // Clear the rich text box
                }
            }
        }

        // Close "X"
        private void Noted_FormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            if (richTextBoxNotes.Text != "") // Check if the rich text box is not empty
            {
                DialogResult closeDialog = MessageBox.Show("Are you sure you want to exit?", "Noted", MessageBoxButtons.YesNo); // Open a dialog box to confirm the exit
                if (closeDialog == DialogResult.Yes) // If yes
                {
                    DialogResult saveAsDialog = MessageBox.Show("Do you want to save the current file?", "Noted", MessageBoxButtons.YesNo);
                    if (saveAsDialog == DialogResult.Yes)
                    {
                        using (var saveAsDialogConfirm = new SaveFileDialog()) // Open a SaveFileDialog() to confirm the save
                        {
                            saveAsDialogConfirm.Filter = "Text Files (*.txt)|*.txt"; // Specify the file type to .txt
                            if (saveAsDialogConfirm.ShowDialog() == DialogResult.OK) // If yes
                            {
                                richTextBoxNotes.SaveFile(saveAsDialogConfirm.FileName, RichTextBoxStreamType.PlainText); // Save the file as .txt file
                            }
                        }
                    }
                }
                else if (closeDialog == DialogResult.No) // If no
                {
                    formClosingEventArgs.Cancel = true; // Prevent the program from closing
                }
            }
        }

        // Find 
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (true) // Loop for the find text box
            {
                string defaultFindText = ""; // Set the find text to empty
                string findText = Microsoft.VisualBasic.Interaction.InputBox("Enter text to find:", "Find", defaultFindText);
                if (string.IsNullOrEmpty(findText)) // If the find text box is empty or if the user clicked "cancel"
                {
                    break; // Close the find text box
                }
                else
                // If not / if user input a text to find
                {
                    // Find text variables 
                    int textStart = 0;
                    int textLength = richTextBoxNotes.TextLength;
                    int findIndex = richTextBoxNotes.Find(findText, textStart, textLength, RichTextBoxFinds.MatchCase);
                    if (findIndex == -1)
                    {
                        richTextBoxNotes.Find(findText, 0, richTextBoxNotes.TextLength, RichTextBoxFinds.None);
                        MessageBox.Show("No match found.");
                    }
                    else
                    {
                        richTextBoxNotes.Select(findIndex, findText.Length);
                        break;
                    }
                }
            }
        }

        // Replace
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (true) // Loop for the find text box
            {
                string defaultFindText = ""; // Set the find text to empty
                string findText = Microsoft.VisualBasic.Interaction.InputBox("Enter text to find:", "Find", defaultFindText);
                if (string.IsNullOrEmpty(findText)) // If the find text box is empty or if the user clicked "cancel"
                {
                    richTextBoxNotes.SelectAll();
                    richTextBoxNotes.SelectionBackColor = Color.White; // Remove all highlights
                    break; // Close the find text box
                }
                else
                {
                    // Find text variables 
                    int textStart = 0;
                    int textLength = richTextBoxNotes.TextLength;
                    int findIndex = richTextBoxNotes.Find(findText, textStart, textLength, RichTextBoxFinds.MatchCase);
                    if (findIndex == -1)
                    {
                        richTextBoxNotes.Find(findText, 0, richTextBoxNotes.TextLength, RichTextBoxFinds.None);
                        MessageBox.Show("No match found.");
                    }
                    else
                    {
                        while (true)
                        {
                            richTextBoxNotes.Select(findIndex, findText.Length);
                            richTextBoxNotes.SelectionBackColor = Color.LightBlue; // Highlight the text
                            string replaceText = Microsoft.VisualBasic.Interaction.InputBox("Replace text with:", "Replace");
                            if (string.IsNullOrEmpty(replaceText))
                            {
                                break;
                            }
                            else
                            {
                                richTextBoxNotes.SelectedText = replaceText; // Replace the text
                                textStart = findIndex + replaceText.Length; // Update the text start index
                                if (textStart >= richTextBoxNotes.TextLength)
                                {
                                    break;
                                }
                                textLength = richTextBoxNotes.TextLength - textStart; // Update the text length
                                findIndex = richTextBoxNotes.Find(findText, textStart, textLength, RichTextBoxFinds.MatchCase); // Find the next occurrence

                                if (findIndex == -1)
                                {
                                    richTextBoxNotes.SelectAll();
                                    richTextBoxNotes.SelectionBackColor = Color.White; // Remove all highlights
                                    MessageBox.Show("No more matches found.");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        // About
        private void devToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/sinnedpenguin";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}