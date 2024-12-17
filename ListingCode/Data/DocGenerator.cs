using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace ListingCode.Data;

public class DocGenerator
{
      public static void Generate(string filePath, IEnumerable<string> files)
      {
            try
            {
                  using (DocX document = DocX.Create(filePath))
                  {
                        foreach (var file in files)
                        {
                              document.InsertParagraph($"Файл {Path.GetFileName(file)}")
                                    .Font("Times New Roman")
                                    .Bold()
                                    .FontSize(14)
                                    .SpacingAfter(12);

                              string fileContent = File.ReadAllText(file);
                              document.InsertParagraph(fileContent)
                                    .Font("Times New Roman")
                                    .FontSize(12)
                                    .SpacingAfter(6);
                        }

                        document.Save();
                  }

                  MessageBox.Show("Listing saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                  MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
      }
}
