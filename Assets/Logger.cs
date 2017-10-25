﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;

public class Logger 
{
    private List<string[]> bufferList = new List<string[]>();
    private string path;

    public Logger(string path)
    {
        this.path = path;
    }

    public void AddLineToBuffer(params string[] values)
    {
        bufferList.Add(values);
    }


    public void Commit()
    {
        StreamWriter streamWriter = new StreamWriter(path, true);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bufferList.Count; i++)
        {
            var row = bufferList[i];
            bool firstColumn = true;

            foreach (string value in row)
            {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(',');

                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                        builder.Append(value);

                    firstColumn = false;
            }
            builder.Append(streamWriter.NewLine);
        }

        //Write rows to the file
        streamWriter.Write(builder.ToString());
        streamWriter.Close();
        //Clear the buffer list
        bufferList.Clear();
    }
}