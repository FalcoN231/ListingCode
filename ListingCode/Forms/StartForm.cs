using ListingCode.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ListingCode;

public partial class StartForm : Form
{
      private List<string> ignoreFiles = [];
      private List<string> ignoreFolders = [];
      private string folderPath = "";
      private string filePath = "";

      public StartForm()
      {
            InitializeComponent();
      }

      private void btn1_Click(object sender, EventArgs e)
      {
            using var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                  folderPath = txtFolderPath.Text =
                        folderDialog.SelectedPath;
                  remove.Enabled = add.Enabled = btn4.Enabled = true;
                  UpdateFile();
            }
      }

      private void add_Click(object sender, EventArgs e)
      {
            using var filterDialog = new OpenFileDialog();
            filterDialog.Multiselect = true;
            filterDialog.CheckFileExists = false;
            filterDialog.ValidateNames = false;
            filterDialog.FileName = "Select files to ignore";

            if (filterDialog.ShowDialog() == DialogResult.OK)
            {
                  foreach (var selectedPath in filterDialog.FileNames)
                  {
                        if (!ignoreFolders.Any(selectedPath.Contains) && !ignoreFiles.Contains(selectedPath))
                        {
                              ignoreFiles.Add(selectedPath);
                        }
                  }
                  UpdateFilter();
                  UpdateFile();
            }
      }

      private void remove_Click(object sender, EventArgs e)
      {
            int index = lstFilters.SelectedIndex;
            if (index != -1)
            {
                  if (index >= ignoreFolders.Count)
                        ignoreFiles.RemoveAt(index - ignoreFolders.Count);
                  else ignoreFolders.RemoveAt(index);
                  UpdateFilter();
                  UpdateFile();
            }
      }

      private void btn2_Click(object sender, EventArgs e)
      {
            using var saveDialog = new SaveFileDialog()
            {
                  Filter = "Microsoft Word (*.docx)|*.docx",
                  FileName = "Code Listing.docx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                  filePath = docFilePath.Text =
                        saveDialog.FileName;
            }
      }

      private void UpdateFile()
      {
            lstFiles.Items.Clear();
            var files = GetFiles();
            foreach (var file in files)
            {
                  lstFiles.Items.Add(file.Replace(folderPath, ""));
            }
      }

      private void UpdateFilter()
      {
            lstFilters.Items.Clear();
            ignoreFolders.ForEach(filter => lstFilters.Items.Add(filter.Replace(folderPath, "")));
            ignoreFiles.ForEach(filter => lstFilters.Items.Add(filter.Replace(folderPath, "")));
      }

      private IEnumerable<string> GetFiles()
      {
            var allFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            var ignoreList = new List<string>();
            ignoreList.AddRange(ignoreFolders);
            ignoreList.AddRange(ignoreFiles);
            return allFiles.Where(file => !ignoreList.Any(filter => file.Contains(filter)));
      }

      private void btn3_Click(object sender, EventArgs e)
      {
            if (string.IsNullOrEmpty(filePath) &&
                  string.IsNullOrEmpty(folderPath))
                  return;

            DocGenerator.Generate(filePath, GetFiles());
      }

      private void btn4_Click(object sender, EventArgs e)
      {
            using (var folderDialog = new FolderBrowserDialog())
            {
                  folderDialog.Multiselect = true;

                  if (folderDialog.ShowDialog() == DialogResult.OK)
                  {
                        foreach (var selectedPath in folderDialog.SelectedPaths)
                        {
                              if (!ignoreFolders.Contains(selectedPath))
                              {
                                    if (!ignoreFiles.Any(file => file.Contains(selectedPath)))
                                    {
                                          ignoreFiles = ignoreFiles.Where(file => !file.Contains(selectedPath)).ToList();
                                    }

                                    ignoreFolders.Add(selectedPath);
                              }
                        }
                        UpdateFilter();
                        UpdateFile();
                  }
            }
      }
}
