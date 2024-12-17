using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ListingCode.Data;

public class DocGenerator
{
      public static void Generate(string filePath, IEnumerable<string> files)
      {
            try
            {
                  using (StreamWriter writer = new(filePath))
                  {
                        foreach (var file in files)
                        {
                              writer.WriteLine($"Файл {Path.GetFileName(file)}");

                              string fileContent = File.ReadAllText(file);
                              writer.WriteLine(fileContent);

                              writer.WriteLine();
                        }
                  }

                  MessageBox.Show("Listing saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                  MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
      }
}
